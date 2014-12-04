using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using TCPRelayCommon;
using System.Resources;
using System.Threading;

namespace TCPRelayControls
{
    public partial class ConnectionItem : UserControl
    {
        private List<Speeds> SpeedHistory = new List<Speeds>();

        private ulong LastReadCount;
        private ulong LastWriteCount;

        private Pen upHistPen;
        private Pen downHistPen;
        private Pen upPen;
        private Pen downPen;

        private Pen graphSepPen = new Pen(new SolidBrush(Color.FromArgb(64, 96, 64)));
        private Pen oneSecPen = new Pen(new SolidBrush(Color.FromArgb(8, 48, 8)));
        private Pen tenSecPen = new Pen(new SolidBrush(Color.FromArgb(12, 72, 12)));
        private Brush bgBrush = new SolidBrush(Color.Black);

        private IPEndPoint srcEndPoint;
        private IPEndPoint dstEndPoint;
        private IPHostEntry srcHostInfo;
        private string dstHostName;

        private Connection _Connection;

        private ConnectionsPanel _ConnectionsPanel;

        private bool _Closed;

        public bool Closed { get { return _Closed; } private set { } }

        public Connection Connection { get { return _Connection; } private set { } }

        private ResourceManager rm;

        public ConnectionItem(ConnectionsPanel parent, Connection connection, int updateInterval)
        {
            InitializeComponent();

            rm = new ResourceManager("TCPRelayControls.ControlsStrings", typeof(ConnectionItem).Assembly);

            _ConnectionsPanel = parent;
            _Connection = connection;
            _Closed = false;

            srcEndPoint = connection.SrcEndPoint;
            dstEndPoint = connection.DstEndPoint;
            srcHostInfo = Dns.GetHostEntry(srcEndPoint.Address);
            dstHostName = connection.DstHostName;
            DelegateUtils.SetText(this, groupBox1, String.Format(rm.GetString("strFromXToY"), srcHostInfo.HostName + ":" + srcEndPoint.Port, dstHostName + ":" + dstEndPoint.Port));
            DelegateUtils.SetText(this, lblTransferred, rm.GetString("strTransferred"));
            DelegateUtils.SetText(this, lblSpeed, rm.GetString("strSpeed"));
            DelegateUtils.SetText(this, lblIn, rm.GetString("strInbound"));
            DelegateUtils.SetText(this, lblOut, rm.GetString("strOutbound"));

            UpdateInfo(updateInterval);
        }

        public void ConnectionClosed()
        {
            _Closed = true;

            DelegateUtils.SetText(this, groupBox1, "[" + rm.GetString("strClosed") + "] "
                + String.Format(rm.GetString("strFromXToY"), srcHostInfo.HostName + ":" + srcEndPoint.Port, dstHostName + ":" + dstEndPoint.Port)
                + " - " + UptimeToString());
            DelegateUtils.SetEnabled(this, groupBox1, false);
            DelegateUtils.SetEnabled(this, btnClose, true);
            DelegateUtils.SetVisible(this, btnClose, true);

            // calculate overall average speed
            double uptimeSeconds = _Connection.Uptime.TotalMilliseconds / 1000.0;
            ulong downloaded = _Connection.SrcWriteCount;
            ulong uploaded = _Connection.DstWriteCount;

            if (uptimeSeconds > 0)
            {
                DelegateUtils.SetText(this, lblDownSpd, DataUtils.ToBitSpeedUnits(downloaded / uptimeSeconds));
                DelegateUtils.SetText(this, lblUpSpd, DataUtils.ToBitSpeedUnits(uploaded / uptimeSeconds));
            }
            else
            {
                DelegateUtils.SetText(this, lblDownSpd, DataUtils.ToBitSpeedUnits(0));
                DelegateUtils.SetText(this, lblUpSpd, DataUtils.ToBitSpeedUnits(0));
            }
        }

        public void UpdateInfo(int updateInterval)
        {
            if (_Connection.Closed) return;

            DelegateUtils.SetText(this, groupBox1, String.Format(rm.GetString("strFromXToY"), srcHostInfo.HostName + ":" + srcEndPoint.Port, dstHostName + ":" + dstEndPoint.Port)
                + " - " + UptimeToString());

            double uptimeSeconds = _Connection.Uptime.TotalMilliseconds / 1000.0;
            ulong downloaded = _Connection.SrcWriteCount;
            ulong uploaded = _Connection.DstWriteCount;
            
            DelegateUtils.SetText(this, lblDownloaded , DataUtils.ToByteUnits(downloaded));
            DelegateUtils.SetText(this, lblUploaded, DataUtils.ToByteUnits(uploaded));

            ulong DeltaWrite = uploaded - LastWriteCount;
            ulong DeltaRead = downloaded - LastReadCount;

            SpeedHistory.Insert(0, new Speeds(uploaded,
                                            downloaded,
                                        DeltaWrite * 1000 / (ulong)updateInterval,
                                        DeltaRead * 1000 / (ulong)updateInterval,
                                        DateTime.Now));
            if (SpeedHistory.Count > 10000) SpeedHistory.RemoveAt(SpeedHistory.Count-1);

            // calculate speed average over the last 2 seconds
            Speeds first = SpeedHistory[0];
            Speeds last = SpeedHistory[0];
            for (int i = 0; i < SpeedHistory.Count; i++)
            {
                last = SpeedHistory[i];
                if ((first.Timestamp - last.Timestamp).TotalSeconds >= 2) break;
            }
            TimeSpan len = first.Timestamp - last.Timestamp;
            if (len.TotalSeconds > 0)
            {
                double downSpeed = (double)(first.CumulativeDownload - last.CumulativeDownload) / len.TotalMilliseconds * 1000;
                double upSpeed = (double)(first.CumulativeUpload - last.CumulativeUpload) / len.TotalMilliseconds * 1000;

                DelegateUtils.SetText(this, lblDownSpd, DataUtils.ToBitSpeedUnits(downSpeed));
                DelegateUtils.SetText(this, lblUpSpd, DataUtils.ToBitSpeedUnits(upSpeed));
            }
            else
            {
                DelegateUtils.SetText(this, lblDownSpd, DataUtils.ToBitSpeedUnits(0));
                DelegateUtils.SetText(this, lblUpSpd, DataUtils.ToBitSpeedUnits(0));
            }

            UpdateHistoryGraph();

            LastWriteCount = uploaded;
            LastReadCount = downloaded;
        }

        private string UptimeToString()
        {
            return _Connection.Uptime.ToString("hh\\:mm\\:ss");
        }
        
        private void UpdateHistoryGraph()
        {
            DelegateUtils.Refresh(this, pnlGraph);
        }

        private void pnlGraph_Paint(object sender, PaintEventArgs e)
        {
            DrawGraph(e.Graphics);
        }

        private void DrawGraph(Graphics g)
        {
            if (SpeedHistory.Count == 0) return;

            int ww = pnlGraph.ClientRectangle.Width - 1;
            int w = ww - 8;
            int h = pnlGraph.ClientRectangle.Height - 1;
            int hh = h / 2;

            double maxUp = SpeedHistory[0].UpSpeed;
            double maxDown = SpeedHistory[0].DownSpeed;

            DateTime oneSec = SpeedHistory[SpeedHistory.Count - 1].Timestamp.AddSeconds(1);
            DateTime tenSec = SpeedHistory[SpeedHistory.Count - 1].Timestamp.AddSeconds(10);
            for (int i=0; i<w && i < SpeedHistory.Count; i++)
            {
                Speeds speeds = SpeedHistory[i];

                maxUp = Math.Max(maxUp, speeds.UpSpeed);
                maxDown = Math.Max(maxDown, speeds.DownSpeed);
            }

            try
            {
                int last = Math.Min(SpeedHistory.Count - 1, w);
                
                // clear graph
                g.FillRectangle(bgBrush, 0, 0, w, h);

                // draw history graph
                for (int x = last; x >= 0; x--)
                {
                    int xx = w - x;
                    
                    // draw lines every second
                    if (oneSec.CompareTo(SpeedHistory[x].Timestamp) < 0)
                    {
                        g.DrawLine(oneSecPen, xx, 0, xx, h);
                        while (oneSec.CompareTo(SpeedHistory[x].Timestamp) < 0)
                        {
                            oneSec = oneSec.AddSeconds(1);
                        }
                    }

                    // draw lines every 10 seconds
                    if (tenSec.CompareTo(SpeedHistory[x].Timestamp) < 0)
                    {
                        g.DrawLine(tenSecPen, xx, 0, xx, h);
                        while (tenSec.CompareTo(SpeedHistory[x].Timestamp) < 0)
                        {
                            tenSec = tenSec.AddSeconds(10);
                        }
                    }

                    // draw history
                    int yu = (int)(SpeedHistory[x].UpSpeed / maxUp * hh);
                    int yd = (int)(SpeedHistory[x].DownSpeed / maxDown * hh);

                    if (maxUp != 0.0) g.DrawLine(upHistPen, xx, hh - 1, xx, hh - yu - 1);
                    if (maxDown != 0.0) g.DrawLine(downHistPen, xx, hh + 1, xx, hh + yd + 1);
                }

                // draw speed bars
                int yu2 = (int)(SpeedHistory[0].UpSpeed / maxUp * hh);
                int yd2 = (int)(SpeedHistory[0].DownSpeed / maxDown * hh);
                g.FillRectangle(upPen.Brush, ww - 6, hh - yu2 - 1, 6, yu2);
                g.FillRectangle(downPen.Brush, ww - 6, hh + 1, 6, yd2);

                // draw graph separators
                g.DrawLine(graphSepPen, ww - 7, 0, ww - 7, h);
                g.DrawLine(graphSepPen, 0, hh, ww, hh);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void DisposePen(Pen pen)
        {
            if (pen != null)
            {
                pen.Brush.Dispose();
                pen.Dispose();
            }
        }

        private void pnlGraph_Resize(object sender, EventArgs e)
        {
            int ww = pnlGraph.ClientRectangle.Width - 1;
            int w = ww - 8;
            int h = pnlGraph.ClientRectangle.Height - 1;
            int hh = h / 2;

            DisposePen(upHistPen);
            DisposePen(downHistPen);
            DisposePen(upPen);
            DisposePen(downPen);
            upHistPen = new Pen(new LinearGradientBrush(new Point(0, 0), new Point(0, hh - 1), Color.FromArgb(100, 200, 50), Color.FromArgb(30, 150, 0)));
            downHistPen = new Pen(new LinearGradientBrush(new Point(0, hh), new Point(0, hh + hh + 1), Color.FromArgb(200, 60, 40), Color.FromArgb(200, 140, 100)));
            upPen = new Pen(new LinearGradientBrush(new Point(0, 0), new Point(0, hh - 1), Color.FromArgb(150, 250, 100), Color.FromArgb(50, 200, 0)));
            downPen = new Pen(new LinearGradientBrush(new Point(0, hh), new Point(0, hh + hh + 1), Color.FromArgb(240, 80, 60), Color.FromArgb(250, 180, 160)));

            UpdateHistoryGraph();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            _ConnectionsPanel.RemoveConnection(_Connection);
        }
    }

    struct Speeds
    {
        public readonly ulong CumulativeUpload;
        public readonly ulong CumulativeDownload;
        public readonly double UpSpeed;
        public readonly double DownSpeed;
        public readonly DateTime Timestamp;

        public Speeds(ulong up, ulong down, double upSpeed, double downSpeed, DateTime timestamp)
        {
            CumulativeUpload = up;
            CumulativeDownload = down;
            UpSpeed = upSpeed;
            DownSpeed = downSpeed;
            Timestamp = timestamp;
        }
    }
}
