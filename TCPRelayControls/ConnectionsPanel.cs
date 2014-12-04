using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using TCPRelayCommon;
using System.Resources;
using System.Threading;

namespace TCPRelayControls
{
    public partial class ConnectionsPanel : UserControl
    {
        Dictionary<IPEndPoint, ConnectionAttemptItem> Attempts = new Dictionary<IPEndPoint, ConnectionAttemptItem>();
        Dictionary<Connection, ConnectionItem> Connections = new Dictionary<Connection, ConnectionItem>();

        List<Control> Items = new List<Control>();

        private static int[] Intervals = { 1, 2, 3, 5, 10, 20, 30, 60 };
        private int Speed = 1;
        private static int MaxSpeed = Intervals.Length - 1;

        private ResourceManager rm;

        public ConnectionsPanel()
        {
            InitializeComponent();

            rm = new ResourceManager("TCPRelayControls.ControlsStrings", typeof(ConnectionsPanel).Assembly);

            grpConnections.Text = rm.GetString("strConnections");
            toolTip1.SetToolTip(btnCloseAll, rm.GetString("strClearAllClosedConnections"));

            SetSpeed(Speed);
            tmrUpdate.Start();
        }

        public void AddConnectionAttempt(IPEndPoint src, string targetHost, int targetPort)
        {
            ConnectionAttemptItem item = new ConnectionAttemptItem(this, src, targetHost, targetPort);
            item.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            AddControl(panel1, item);
        }

        public void ClearConnectionAttempts()
        {
            List<Control> controlsToRemove = new List<Control>();
            foreach (Control c in panel1.Controls) if (c is ConnectionAttemptItem) controlsToRemove.Add(c);
            controlsToRemove.ForEach((c) => RemoveControl(panel1, c));

            Refresh();
            grpConnections.Refresh();
            if (!panel1.VerticalScroll.Visible) panel1.Refresh();
        }

        public void AddConnection(Connection c)
        {
            ConnectionItem item = new ConnectionItem(this, c, tmrUpdate.Interval);
            item.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            AddControl(panel1, item);
        }

        public void RemoveConnection(Connection c)
        {
            if (Connections.ContainsKey(c))
            {
                ConnectionItem ci = Connections[c];
                RemoveControl(panel1, ci);
            }
        }

        public void ConnectionClosed(Connection c)
        {
            foreach (Control ct in panel1.Controls)
            {
                ConnectionItem item = ct as ConnectionItem;
                if (item != null && item.Connection == c) { item.ConnectionClosed(); break; }
            }
        }

        public void AddError(IPEndPoint src, string targetHost, int targetPort, string errorMessage)
        {
            ConnectionErrorItem item = new ConnectionErrorItem(this, src, targetHost, targetPort, errorMessage);
            item.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            AddControl(panel1, item);
        }

        public void RemoveError(ConnectionErrorItem item)
        {
            RemoveControl(panel1, item);
        }

        private void AddControl(Control container, Control control)
        {
            DelegateUtils.DoAction(this, container, control, (ctr, ctl) =>
            {
                ctr.Controls.Add(ctl);
                ctl.Width = ctr.ClientRectangle.Width;
            });
        }

        private void RemoveControl(Control container, Control control)
        {
            DelegateUtils.DoAction(this, container, control, (ctr, ctl) =>
            {
                ctr.Controls.Remove(ctl);
            });
        }

        private void panel1_ControlAdded(object sender, ControlEventArgs e)
        {
            int pos = 0;
            if (e.Control is ConnectionAttemptItem)
            {
                ConnectionAttemptItem item = e.Control as ConnectionAttemptItem;
                Attempts[item.SrcEndPoint] = item;
            }
            else if (e.Control is ConnectionErrorItem)
            {
                ConnectionErrorItem item = e.Control as ConnectionErrorItem;
                ConnectionAttemptItem attempt = Attempts[item.SrcEndPoint];

                pos = Items.IndexOf(attempt);
                panel1.Controls.Remove(attempt);
            }
            else if (e.Control is ConnectionItem)
            {
                ConnectionItem item = e.Control as ConnectionItem;
                ConnectionAttemptItem attempt = Attempts[item.Connection.SrcEndPoint];

                pos = Items.IndexOf(attempt);
                panel1.Controls.Remove(attempt);
                
                Connections[item.Connection] = item;
            }
            Items.Insert(pos, e.Control);
            ReflowControls();
        }

        private void panel1_ControlRemoved(object sender, ControlEventArgs e)
        {
            if (e.Control is ConnectionItem) Connections.Remove((e.Control as ConnectionItem).Connection);
            if (e.Control is ConnectionAttemptItem) Attempts.Remove((e.Control as ConnectionAttemptItem).SrcEndPoint);
            Items.Remove(e.Control);
            ReflowControls();
        }

        private void ReflowControls()
        {
            int y = -panel1.VerticalScroll.Value;
            Items.ForEach((item) =>
            {
                item.Top = y;
                y += item.Height;
            });
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            foreach (Control c in panel1.Controls)
            {
                ConnectionItem item = c as ConnectionItem;
                if (item != null) item.UpdateInfo(tmrUpdate.Interval);
            }
        }

        private void btnCloseAll_Click(object sender, EventArgs e)
        {
            List<Control> controlsToRemove = new List<Control>();
            foreach (Control c in panel1.Controls)
            {
                if (c is ConnectionErrorItem)
                    controlsToRemove.Add(c);
                else
                {
                    ConnectionItem item = c as ConnectionItem;
                    if (item != null && item.Closed) controlsToRemove.Add(item);
                }
            }
            controlsToRemove.ForEach((c) => RemoveControl(panel1, c));

            Refresh();
            grpConnections.Refresh();
            if (!panel1.VerticalScroll.Visible) panel1.Refresh();
        }

        private void btnFastest_Click(object sender, EventArgs e) { SetSpeed(MaxSpeed); }
        private void btnFaster_Click(object sender, EventArgs e) { IncSpeed(); }
        private void btnSlower_Click(object sender, EventArgs e) { DecSpeed(); }
        private void btnSlowest_Click(object sender, EventArgs e) { SetSpeed(0); }
        
        private void IncSpeed() { SetSpeed(Speed + 1); }
        private void DecSpeed() { SetSpeed(Speed - 1); }

        private void SetSpeed(int speed)
        {
            // clamp to 0..MaxSpeed
            Speed = Math.Max(0, Math.Min(speed, MaxSpeed));
            tmrUpdate.Interval = 1000 / Intervals[Speed];

            bool enableSlower = speed > 0;
            bool enableFaster = speed < MaxSpeed;
            DelegateUtils.SetEnabled(this, btnSlowest, enableSlower);
            DelegateUtils.SetEnabled(this, btnSlower, enableSlower);
            DelegateUtils.SetEnabled(this, btnFaster, enableFaster);
            DelegateUtils.SetEnabled(this, btnFastest, enableFaster);

            string spd = String.Format(rm.GetString("strCurrentSpeed"), Intervals[Speed]);
            DelegateUtils.SetToolTip(this, toolTip1, btnSlowest, rm.GetString("strUpdateSlowest") + "\n" + spd);
            DelegateUtils.SetToolTip(this, toolTip1, btnSlower, rm.GetString("strUpdateSlower") + "\n" + spd);
            DelegateUtils.SetToolTip(this, toolTip1, btnFaster, rm.GetString("strUpdateFaster") + "\n" + spd);
            DelegateUtils.SetToolTip(this, toolTip1, btnFastest, rm.GetString("strUpdateFastest") + "\n" + spd);
        }
    }
}
