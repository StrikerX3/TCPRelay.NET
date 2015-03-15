using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using TCPRelayCommon;
using TCPRelayControls;
using System.Drawing;

namespace TCPRelayWindow
{
    public partial class TCPRelayForm : Form, IConnectionListener
    {
        private TCPRelay relay = new TCPRelay();
        private ResourceManager rm;
        private AdvancedSettingsForm advForm;

        public TCPRelayForm()
        {
            InitializeComponent();

            rm = new ResourceManager("TCPRelayWindow.WinFormStrings", typeof(TCPRelayForm).Assembly);
            advForm = new AdvancedSettingsForm(this, relay.Parameters);

            AssemblyInfoHelper helper = new AssemblyInfoHelper(typeof(TCPRelayWindow));
            lblVersionCopyright.Text = "TCPRelay v" + helper.AssemblyInformationalVersion;

            lblTargetURI.Text = rm.GetString("strTargetURI") + ":";
            lblListenPort.Text = rm.GetString("strListenPort") + ":";
            btnAdvancedSettings.Text = rm.GetString("strAdvancedSettings");
            lblRunning.Text = rm.GetString("strStopped");
            btnStartStop.Text = rm.GetString("strStart");
            toolTip1.SetToolTip(btnStartStop, rm.GetString("strStartsTheRelay"));
            toolTip1.SetToolTip(cbxTargetURI, rm.GetString("strTargetURIHint"));
            toolTip1.SetToolTip(numListenPort, rm.GetString("strListenPortHint"));
            toolTip1.SetToolTip(btnLoadTTVServers, rm.GetString("strLoadTwitchTVServersHint"));

            RebuildLayout();
        }

        private int FindWidestString(Font font, params string[] strings)
        {
            int max = 0;
            using (Graphics g = CreateGraphics())
            {
                foreach (string str in strings)
                {
                    SizeF size = g.MeasureString(str, font);
                    int newWidth = (int)Math.Ceiling(size.Width);
                    max = Math.Max(max, newWidth);
                }
            }
            return max;
        }

        private void RebuildLayout()
        {
            const int margin = 4;
            // TODO find a better way to layout the components... for now this will do
            
            // resize and reposition Load Twitch.tv button
            int ttvWidth = 10 + FindWidestString(btnLoadTTVServers.Font,
                rm.GetString("strLoadTwitchTVServers"),
                rm.GetString("strLoading"));
            int ttvDelta = btnLoadTTVServers.Width - ttvWidth;
            btnLoadTTVServers.Width = ttvWidth;
            btnLoadTTVServers.Left += ttvDelta;

            // align Target URI and Listen Port fields
            int uriPortLeft = Math.Max(lblTargetURI.Right, lblListenPort.Right) + margin;
            int uriDelta = cbxTargetURI.Left - uriPortLeft;
            cbxTargetURI.Left = uriPortLeft;
            numListenPort.Left = uriPortLeft;

            // move Advanced Settings button to the right of the Listen Port
            btnAdvancedSettings.Left = numListenPort.Right + margin;
            btnAdvancedSettings.Width = 10 + FindWidestString(lblRunning.Font, rm.GetString("strAdvancedSettings"));

            // resize and reposition Target URI field width to fit the new position
            cbxTargetURI.Width += uriDelta + ttvDelta;

            // resize and reposition Start/Stop button
            int ssWidth = 10 + FindWidestString(btnStartStop.Font,
                rm.GetString("strStart"),
                rm.GetString("strStarting"),
                rm.GetString("strStop"),
                rm.GetString("strStopping"));
            int ssDelta = btnStartStop.Width - ssWidth;
            btnStartStop.Width = ssWidth;
            btnStartStop.Left += ssDelta;

            // resize and reposition status label
            int stWidth = 5 + FindWidestString(lblRunning.Font,
                rm.GetString("strStarted"),
                rm.GetString("strStopped"),
                rm.GetString("strFailed"));
            lblRunning.Width = stWidth;
            lblRunning.Left = btnStartStop.Left - stWidth - margin;
        }

        private void TCPRelayForm_Load(object sender, EventArgs e)
        {
            btnLoadTTVServers.Enabled = false;
            btnLoadTTVServers.Text = rm.GetString("strLoading");
            string val = RegistryUtils.GetString("LastTargetURI");
            if (val != null)
                cbxTargetURI.Text = val;
            int? listenPort = RegistryUtils.GetDWord("LastListenPort");
            if (listenPort != null)
                numListenPort.Value = (decimal)listenPort;

            initTargetURIListWorker.RunWorkerAsync();
            relay.Listeners.Add(this);
        }

        private void TCPRelayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            relay.Stop();
            SaveURIList();
        }

        private Uri GetTargetURI()
        {
            object item = cbxTargetURI.SelectedItem;
            return ToURI(item);
        }

        private Uri ToURI(object item)
        {
            Uri rtmpUri;
            TwitchTvIngestServerData sd = item as TwitchTvIngestServerData;
            if (sd != null)
                rtmpUri = new Uri(sd.Uri);
            else if (item != null)
                rtmpUri = new Uri(item.ToString());
            else
            {
                string txt = cbxTargetURI.Text;
                if (txt.Contains("://"))
                    rtmpUri = new Uri(txt);
                else
                    rtmpUri = new Uri("tcp://" + txt);
            }
            return rtmpUri;
        }

        private int GetListenPort()
        {
            return (int)numListenPort.Value;
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            if (relay.IsRunning)
            {
                btnStartStop.Enabled = false;
                btnStartStop.Text = rm.GetString("strStopping");

                relay.Stop();

                btnStartStop.Enabled = true;
                btnStartStop.Text = rm.GetString("strStart");
            }
            else
            {
                btnStartStop.Enabled = false;
                btnStartStop.Text = rm.GetString("strStarting");

                Uri rtmpUri = GetTargetURI();

                relay.TargetHost = rtmpUri.Host;
                relay.TargetPort = rtmpUri.Port == -1 ? 1935 : rtmpUri.Port;

                relay.ListenPort = GetListenPort();

                relay.Start();
            }
        }

        private void btnLoadTTVServers_Click(object sender, EventArgs e)
        {
            btnLoadTTVServers.Enabled = false;
            btnLoadTTVServers.Text = rm.GetString("strLoading");

            initTargetURIListWorker.RunWorkerAsync();
        }

        private void initTargetURIListWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            DelegateUtils.DoAction(this, cbxTargetURI, (cbx) => LoadURIList());

            try
            {
                List<TwitchTvIngestServerData> servers = TwitchTvIngestServerData.Retrieve();
                servers.Sort();
                e.Result = servers;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private void initTargetURIListWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsDisposed)
                return;

            if (!e.Cancelled)
            {
                int st = cbxTargetURI.SelectionStart;
                int ln = cbxTargetURI.SelectionLength;

                // TODO refactor this!
                // This is an ugly hack to:
                // - preserve user entries in the Target URI combo list
                // - remove old justin.tv entries generated by earlier versions of TCPRelay
                // The Target URI code should be completely revamped/redesigned
                HashSet<Uri> currentURIs = new HashSet<Uri>();
                List<object> itemsToRemove = new List<object>();
                foreach (object item in cbxTargetURI.Items)
                {
                    Uri uri = ToURI(item);
                    if (uri.Host.Contains("justin.tv"))
                    {
                        itemsToRemove.Add(item);
                    }
                    else
                    {
                        currentURIs.Add(ToURI(item));
                    }
                }

                foreach (object item in itemsToRemove)
                {
                    cbxTargetURI.Items.Remove(item);
                }

                (e.Result as List<TwitchTvIngestServerData>).ForEach((server) =>
                {
                    Uri serverUri = new Uri(server.Uri);
                    if (!currentURIs.Contains(serverUri))
                    {
                        cbxTargetURI.Items.Add(server);
                        currentURIs.Add(serverUri);
                    }
                });

                cbxTargetURI.SelectionStart = st;
                cbxTargetURI.SelectionLength = ln;
            }

            DelegateUtils.SetEnabled(this, btnLoadTTVServers, true);
            DelegateUtils.SetText(this, btnLoadTTVServers, rm.GetString("strLoadTwitchTVServers"));
        }

        private void cbxTargetURI_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // TODO refactor this!
                HashSet<Uri> currentURIs = new HashSet<Uri>();
                foreach (object item in cbxTargetURI.Items)
                    currentURIs.Add(ToURI(item));

                Uri newUri = GetTargetURI();
                if (!currentURIs.Contains(newUri))
                    cbxTargetURI.Items.Add(newUri);
            }
        }

        private void btnAdvancedSettings_Click(object sender, EventArgs e)
        {
            if (advForm.IsDisposed)
            {
                advForm = new AdvancedSettingsForm(this, relay.Parameters);
            }

            advForm.LoadSettings();
            advForm.ShowDialog(this);
        }

        private void AddControl(Control container, Control control)
        {
            DelegateUtils.DoAction(this, container, control, (ctr, ctl) => ctr.Controls.Add(ctl));
        }

        private void LoadURIList()
        {
            // TODO refactor this!
            HashSet<Uri> currentURIs = new HashSet<Uri>();
            foreach (object item in cbxTargetURI.Items)
                currentURIs.Add(ToURI(item));

            try
            {
                string serverListPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TCPRelay\\";
                if (!Directory.Exists(serverListPath))
                    Directory.CreateDirectory(serverListPath);

                using (FileStream file = File.OpenRead(serverListPath + "serverlist.txt"))
                using (StreamReader reader = new StreamReader(file))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        try
                        {
                            if (line.Contains("|"))
                            {
                                string[] split = line.Split('|');
                                if (split.Length < 3)
                                {
                                    // invalid data; just ignore
                                    continue;
                                }
                                string textUri = split[0];
                                string name = split[1];
                                string def = split[2];
                                Uri uri = new Uri(textUri);
                                TwitchTvIngestServerData data = new TwitchTvIngestServerData(name, textUri, Boolean.Parse(def));
                                if (!currentURIs.Contains(uri))
                                {
                                    cbxTargetURI.Items.Add(data);
                                    currentURIs.Add(uri);
                                }
                            }
                            else
                            {
                                Uri uri = new Uri(line);
                                if (!currentURIs.Contains(uri))
                                {
                                    cbxTargetURI.Items.Add(uri);
                                    currentURIs.Add(uri);
                                }
                            }
                        }
                        catch (UriFormatException e)
                        {
                            Console.WriteLine(e);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                // ignore
            }
        }

        private void SaveURIList()
        {
            // TODO refactor this!
            HashSet<Uri> currentURIs = new HashSet<Uri>();
            foreach (object item in cbxTargetURI.Items)
                currentURIs.Add(ToURI(item));

            Uri currentUri = GetTargetURI();
            if (!currentURIs.Contains(currentUri))
            {
                cbxTargetURI.Items.Add(currentUri);
                currentURIs.Add(currentUri);
            }

            string serverListPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TCPRelay\\";
            if (!Directory.Exists(serverListPath))
                Directory.CreateDirectory(serverListPath);

            using (FileStream file = File.OpenWrite(serverListPath + "serverlist.txt"))
            using (StreamWriter writer = new StreamWriter(file))
                foreach (object item in cbxTargetURI.Items)
                {
                    if (item is TwitchTvIngestServerData)
                    {
                        TwitchTvIngestServerData data = item as TwitchTvIngestServerData;
                        writer.WriteLine(data.Uri + "|" + data.Name + "|" + data.Default);
                    }
                    else
                    {
                        writer.WriteLine(ToURI(item));
                    }
                }
        }

        private void TCPRelayStarted()
        {
            DelegateUtils.SetEnabled(this, cbxTargetURI, false);
            DelegateUtils.SetEnabled(this, numListenPort, false);
            DelegateUtils.SetEnabled(this, btnAdvancedSettings, false);
            DelegateUtils.SetEnabled(this, btnLoadTTVServers, false);
            DelegateUtils.SetEnabled(this, btnStartStop, true);

            DelegateUtils.SetText(this, btnStartStop, rm.GetString("strStop"));
            DelegateUtils.SetText(this, lblRunning, rm.GetString("strStarted"));
            DelegateUtils.SetToolTip(this, toolTip1, lblRunning, "");
            DelegateUtils.SetToolTip(this, toolTip1, btnStartStop, rm.GetString("strStopsTheRelay"));

            DelegateUtils.DoAction(this, cbxTargetURI, (cbx) => RegistryUtils.SetString("LastTargetURI", GetTargetURI().ToString()));
            DelegateUtils.DoAction(this, numListenPort, (cbx) => RegistryUtils.SetDWord("LastListenPort", GetListenPort()));

            // TODO refactor this!
            DelegateUtils.DoAction(this, cbxTargetURI, (cbx) =>
            {
                HashSet<Uri> currentURIs = new HashSet<Uri>();
                foreach (object item in cbx.Items)
                    currentURIs.Add(ToURI(item));

                Uri newUri = GetTargetURI();
                if (!currentURIs.Contains(newUri))
                    cbx.Items.Add(newUri);
            });
        }

        private void TCPRelayStopped()
        {
            DelegateUtils.SetEnabled(this, cbxTargetURI, true);
            DelegateUtils.SetEnabled(this, numListenPort, true);
            DelegateUtils.SetEnabled(this, btnAdvancedSettings, true);
            DelegateUtils.SetEnabled(this, btnLoadTTVServers, true);
            DelegateUtils.SetEnabled(this, btnStartStop, true);

            DelegateUtils.SetText(this, btnStartStop, rm.GetString("strStart"));
            DelegateUtils.SetText(this, lblRunning, rm.GetString("strStopped"));
            DelegateUtils.SetToolTip(this, toolTip1, lblRunning, "");
            DelegateUtils.SetToolTip(this, toolTip1, btnStartStop, rm.GetString("strStartsTheRelay"));
        }

        private void TCPRelayFailed(Exception e)
        {
            DelegateUtils.SetEnabled(this, cbxTargetURI, true);
            DelegateUtils.SetEnabled(this, numListenPort, true);
            DelegateUtils.SetEnabled(this, btnAdvancedSettings, true);

            DelegateUtils.SetEnabled(this, btnStartStop, true);
            DelegateUtils.SetText(this, btnStartStop, rm.GetString("strStart"));
            DelegateUtils.SetText(this, lblRunning, rm.GetString("strFailed"));
            DelegateUtils.SetToolTip(this, toolTip1, lblRunning, e.Message);
            DelegateUtils.SetToolTip(this, toolTip1, btnStartStop, rm.GetString("strStartsTheRelay"));

            DelegateUtils.DoAction(this, this, (frm) =>
            {
                SocketException se = e as SocketException;
                if (se == null)
                    ShowDefaultError(frm, e);
                else
                {
                    switch (se.SocketErrorCode)
                    {
                        case SocketError.AddressAlreadyInUse:
                            ShowAddressAlreadyInUseError(frm);
                            break;
                        case SocketError.TryAgain:
                            ShowCustomError(frm, rm.GetString("strCouldNotResolveHostName") + ": " + relay.TargetHost);
                            break;
                        default:
                            ShowDefaultError(frm, e);
                            break;
                    }
                }
            });
        }

        private void ShowDefaultError(TCPRelayForm frm, Exception e)
        {
            MessageBox.Show(frm, e.Message, rm.GetString("strFailedToStartTCPRelay"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowCustomError(TCPRelayForm frm, string errorMessage)
        {
            MessageBox.Show(frm, errorMessage, rm.GetString("strFailedToStartTCPRelay"), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowAddressAlreadyInUseError(TCPRelayForm frm)
        {
            TcpConnection[] conns = TcpConnectionHelper.GetTcpConnections();
            TcpConnection curr = null;
            foreach (TcpConnection conn in conns)
            {
                if (conn.LocalEndPoint.Port == relay.ListenPort)
                {
                    curr = conn;
                    break;
                }
            }

            if (curr == null || curr.Process == null)
            {
                MessageBox.Show(frm,
                    String.Format(rm.GetString("strPortAlreadyInUse"), relay.ListenPort),
                    rm.GetString("strFailedToStartTCPRelay"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(frm,
                    String.Format(rm.GetString("strPortAlreadyInUseByProcess"), relay.ListenPort, curr.Process.MainModule.ModuleName, curr.Process.Id),
                    rm.GetString("strFailedToStartTCPRelay"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public void Started(TCPRelay relay)
        {
            TCPRelayStarted();
        }

        public void Stopped(TCPRelay relay)
        {
            TCPRelayStopped();
        }

        public void StartFailed(TCPRelay relay, Exception e)
        {
            TCPRelayFailed(e);
        }

        public void ConnectionAttempt(TCPRelay relay, IPEndPoint src, string targetHost, int targetPort)
        {
            tcpRelayConnectionsPanel1.AddConnectionAttempt(src, targetHost, targetPort);
        }

        public void ConnectionAccepted(TCPRelay relay, Connection c)
        {
            tcpRelayConnectionsPanel1.AddConnection(c);
        }

        public void ConnectionClosed(TCPRelay relay, Connection c)
        {
            tcpRelayConnectionsPanel1.ConnectionClosed(c);
        }

        public void ConnectionRefused(TCPRelay relay, IPEndPoint src, string targetHost, int targetPort, SocketException e)
        {
            tcpRelayConnectionsPanel1.AddError(src, targetHost, targetPort, e.Message);
        }

        public void ConnectionFailed(TCPRelay relay, IPEndPoint src, string targetHost, int targetPort, SocketException e)
        {
            DelegateUtils.DoAction(this, tcpRelayConnectionsPanel1, (pnl) => pnl.ClearConnectionAttempts());
            TCPRelayFailed(e);
        }

        public void OpenPipe(TCPRelay relay, Connection c, IPEndPoint src, IPEndPoint dest) { }
        public void ClosePipe(TCPRelay relay, Connection c, IPEndPoint src, IPEndPoint dest) { }
        public void PipeFailure(TCPRelay relay, Connection c, IPEndPoint src, IPEndPoint dest, Exception e) { }
    }
}
