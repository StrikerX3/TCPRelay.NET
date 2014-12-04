using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Resources;
using System.Threading;

namespace TCPRelayControls
{
    public partial class ConnectionErrorItem : UserControl
    {
        private ConnectionsPanel _ConnectionsPanel;

        public readonly IPEndPoint SrcEndPoint;

        private ResourceManager rm;

        public ConnectionErrorItem(ConnectionsPanel connList, IPEndPoint src, string targetHost, int targetPort, string errorMessage)
        {
            InitializeComponent();

            rm = new ResourceManager("TCPRelayControls.ControlsStrings", typeof(ConnectionErrorItem).Assembly);

            _ConnectionsPanel = connList;
            SrcEndPoint = src;

            IPHostEntry srcHostInfo = Dns.GetHostEntry(src.Address);

            DelegateUtils.SetText(this, groupBox1, "[" + rm.GetString("strError") + "] " + String.Format(rm.GetString("strFromXToY"), srcHostInfo.HostName + ":" + src.Port, targetHost + ":" + targetPort));
            DelegateUtils.SetText(this, lblError, errorMessage);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            _ConnectionsPanel.RemoveError(this);
        }
    }
}
