using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using TCPRelayCommon;

namespace TCPRelayWindow
{
    public partial class AdvancedSettingsForm : Form
    {
        private TCPRelayParams RelayParams;
        private ResourceManager rm;

        public AdvancedSettingsForm(TCPRelayParams relayParams)
        {
            rm = new ResourceManager("TCPRelayWindow.WinFormStrings", typeof(AdvancedSettingsForm).Assembly);

            RelayParams = relayParams;
            InitializeComponent();
            LoadSettings();

            RebuildLayout();
        }

        public void LoadSettings()
        {
            // rename window title
            this.Text = rm.GetString("strAdvancedSettingsTitle");

            // load settings from the params object
            chbSendBufferApp.Checked = RelayParams.SendBufferSizeApp != null;
            chbSendBufferRemote.Checked = RelayParams.SendBufferSizeRemote != null;
            chbReceiveBufferApp.Checked = RelayParams.RecvBufferSizeApp != null;
            chbReceiveBufferRemote.Checked = RelayParams.RecvBufferSizeRemote != null;
            chbConnTimeout.Checked = RelayParams.ConnectTimeout != null;

            chbNoDelayApp.Checked = RelayParams.NoDelayApp;
            chbNoDelayRemote.Checked = RelayParams.NoDelayRemote;

            if (chbSendBufferApp.Checked) numSendBufferApp.Value = (decimal)RelayParams.SendBufferSizeApp;
            if (chbSendBufferRemote.Checked) numSendBufferRemote.Value = (decimal)RelayParams.SendBufferSizeRemote;
            if (chbReceiveBufferApp.Checked) numReceiveBufferApp.Value = (decimal)RelayParams.RecvBufferSizeApp;
            if (chbReceiveBufferRemote.Checked) numReceiveBufferRemote.Value = (decimal)RelayParams.RecvBufferSizeRemote;
            if (chbConnTimeout.Checked) numConnTimeout.Value = (decimal)RelayParams.ConnectTimeout;

            // enable the components
            numSendBufferApp.Enabled = chbSendBufferApp.Checked;
            numSendBufferRemote.Enabled = chbSendBufferRemote.Checked;
            numReceiveBufferApp.Enabled = chbReceiveBufferApp.Checked;
            numReceiveBufferRemote.Enabled = chbReceiveBufferRemote.Checked;
            numConnTimeout.Enabled = chbConnTimeout.Checked;

            // populate network interface list
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            List<IPAddressItem> ipv4Addresses = new List<IPAddressItem>();
            List<IPAddressItem> ipv6Addresses = new List<IPAddressItem>();
            foreach (NetworkInterface iface in interfaces)
            {
                if (iface.NetworkInterfaceType == NetworkInterfaceType.Ethernet
                    || iface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    foreach (var addr in iface.GetIPProperties().UnicastAddresses)
                    {
                        switch (addr.Address.AddressFamily)
                        {
                            case AddressFamily.InterNetwork:
                                ipv4Addresses.Add(new IPAddressItem(iface, addr.Address));
                                break;
                            case AddressFamily.InterNetworkV6:
                                ipv6Addresses.Add(new IPAddressItem(iface, addr.Address));
                                break;
                        }
                    }
                }
            }

            bool foundAddress = false;
            cbxAddresses.Items.Clear();
            cbxAddresses.Items.Add(new DefaultIPAddressItem());
            foreach (IPAddressItem item in ipv4Addresses)
            {
                cbxAddresses.Items.Add(item);
                if (RelayParams.BindIP.Equals(item.Address))
                {
                    cbxAddresses.SelectedItem = item;
                    foundAddress = true;
                }
            }
            foreach (IPAddressItem item in ipv6Addresses)
            {
                cbxAddresses.Items.Add(item);
                if (RelayParams.BindIP.Equals(item.Address))
                {
                    cbxAddresses.SelectedItem = item;
                    foundAddress = true;
                }
            }
            if (!foundAddress)
            {
                cbxAddresses.SelectedIndex = 0;
            }
            cbxAddresses.Width = DropDownWidth(cbxAddresses) + 18;
        }

        private int DropDownWidth(ComboBox myCombo)
        {
            int maxWidth = 0, temp = 0;
            foreach (var obj in myCombo.Items)
            {
                temp = TextRenderer.MeasureText(obj.ToString(), myCombo.Font).Width;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            return maxWidth;
        }

        // TODO duplicate method in TCPRelayForm
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

        private void AdjustWidth(Label lbl)
        {
            lbl.Width = FindWidestString(lbl.Font, lbl.Text);
        }

        private void RebuildLayout()
        {
            const int margin = 4;
            const int spacing = 14;
            // TODO find a better way to layout the components... for now this will do

            // resize and rename all labels
            int paramsWidth = 15 + FindWidestString(lblInternalBuffer.Font,
                rm.GetString("strSendBuffer") + ":",
                rm.GetString("strReceiveBuffer") + ":",
                rm.GetString("strConnectionTimeout") + ":",
                rm.GetString("strNoDelay") + ":",
                rm.GetString("strInternalBufferSize") + ":",
                rm.GetString("strBindToAdapter") + ":",
                rm.GetString("strPreferredAddress") + ":");

            lblSendBuffer.Text = rm.GetString("strSendBuffer") + ":";
            lblRecvBuffer.Text = rm.GetString("strReceiveBuffer") + ":";
            lblConnTimeout.Text = rm.GetString("strConnectionTimeout") + ":";
            lblNoDelay.Text = rm.GetString("strNoDelay") + ":";
            lblInternalBuffer.Text = rm.GetString("strInternalBufferSize") + ":";
            lblBindToAddress.Text = rm.GetString("strBindToAddress") + ":";
            
            AdjustWidth(lblSendBuffer);
            AdjustWidth(lblRecvBuffer);
            AdjustWidth(lblConnTimeout);
            AdjustWidth(lblNoDelay);
            AdjustWidth(lblInternalBuffer);
            AdjustWidth(lblBindToAddress);

            // reposition all components in columns

            // override app values
            int overrideAppLeft = paramsWidth + margin;
            lblFromApp.Left = overrideAppLeft;
            lblFromApp.Text = rm.GetString("strFromApplication");
            AdjustWidth(lblFromApp);
            chbSendBufferApp.Left = overrideAppLeft;
            chbReceiveBufferApp.Left = overrideAppLeft;
            chbNoDelayApp.Left = overrideAppLeft;
            chbConnTimeout.Left = overrideAppLeft;

            // app values
            int valueAppLeft = chbSendBufferApp.Right + margin;
            numSendBufferApp.Left = valueAppLeft;
            numReceiveBufferApp.Left = valueAppLeft;
            numInternalBufferSize.Left = valueAppLeft;
            numConnTimeout.Left = valueAppLeft;
            cbxAddresses.Left = valueAppLeft;
            
            // override remote values
            int overrideRemoteLeft = Max(lblFromApp.Right, numSendBufferApp.Right) + margin + spacing;
            lblToRemote.Left = overrideRemoteLeft;
            lblToRemote.Text = rm.GetString("strToRemote");
            AdjustWidth(lblToRemote);
            chbSendBufferRemote.Left = overrideRemoteLeft;
            chbReceiveBufferRemote.Left = overrideRemoteLeft;
            chbNoDelayRemote.Left = overrideRemoteLeft;

            // remote values
            int valueRemoteLeft = chbSendBufferRemote.Right + margin;
            numSendBufferRemote.Left = valueRemoteLeft;
            numReceiveBufferRemote.Left = valueRemoteLeft;

            // adjust window size
            int windowWidth = Max(lblToRemote.Right, numSendBufferRemote.Right, cbxAddresses.Right) + spacing;
            int windowHeight = cbxAddresses.Bottom + spacing + btnOK.Height + spacing;
            this.ClientSize = new Size(windowWidth, windowHeight);

            // rename and reposition the OK and Cancel buttons
            int btnWidth = Math.Max(75, 10 + FindWidestString(btnOK.Font,
                rm.GetString("strOK"),
                rm.GetString("strCancel")));
            btnCancel.Text = rm.GetString("strCancel");
            btnCancel.Width = btnWidth;
            btnCancel.Left = windowWidth - btnWidth - spacing;
            btnCancel.Top = windowHeight - btnOK.Height - spacing;
            btnOK.Text = rm.GetString("strOK");
            btnOK.Width = btnWidth;
            btnOK.Left = btnCancel.Left - spacing - btnWidth;
            btnOK.Top = btnCancel.Top;
        }

        private int Max(int firstValue, int secondValue, params int[] moreValues)
        {
            int max = Math.Max(firstValue, secondValue);
            foreach (int value in moreValues)
            {
                max = Math.Max(max, value);
            }
            return max;
        }

        private void chbSendBufferApp_CheckedChanged(object sender, EventArgs e)
        {
            numSendBufferApp.Enabled = chbSendBufferApp.Checked;
        }

        private void chbSendBufferRemote_CheckedChanged(object sender, EventArgs e)
        {
            numSendBufferRemote.Enabled = chbSendBufferRemote.Checked;
        }

        private void chbReceiveBufferApp_CheckedChanged(object sender, EventArgs e)
        {
            numReceiveBufferApp.Enabled = chbReceiveBufferApp.Checked;
        }

        private void chbReceiveBufferRemote_CheckedChanged(object sender, EventArgs e)
        {
            numReceiveBufferRemote.Enabled = chbReceiveBufferRemote.Checked;
        }

        private void chbConnTimeoutRemote_CheckedChanged(object sender, EventArgs e)
        {
            numConnTimeout.Enabled = chbConnTimeout.Checked;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            // save settings to the params object
            RelayParams.NoDelayApp = chbNoDelayApp.Checked;
            RelayParams.NoDelayRemote = chbNoDelayRemote.Checked;

            RelayParams.SendBufferSizeApp = GetValue((int)numSendBufferApp.Value, chbSendBufferApp.Checked);
            RelayParams.SendBufferSizeRemote = GetValue((int)numSendBufferRemote.Value, chbSendBufferRemote.Checked);
            RelayParams.RecvBufferSizeApp = GetValue((int)numReceiveBufferApp.Value, chbReceiveBufferApp.Checked);
            RelayParams.RecvBufferSizeRemote = GetValue((int)numReceiveBufferRemote.Value, chbReceiveBufferRemote.Checked);
            RelayParams.ConnectTimeout = GetValue((int)numConnTimeout.Value, chbConnTimeout.Checked);

            RelayParams.InternalBufferSize = (int)numInternalBufferSize.Value;
            RelayParams.BindIP = ((IPAddressItem)cbxAddresses.SelectedItem).Address;
            
            
            // save settings to the registry
            RegistryUtils.SetBoolean("NoDelayApp", chbNoDelayApp.Checked);
            RegistryUtils.SetBoolean("NoDelayRemote", chbNoDelayRemote.Checked);

            SetOrClearDword("SendBufferApp", (int)numSendBufferApp.Value, chbSendBufferApp.Checked);
            SetOrClearDword("SendBufferRemote", (int)numSendBufferRemote.Value, chbSendBufferRemote.Checked);
            SetOrClearDword("ReceiveBufferApp", (int)numReceiveBufferApp.Value, chbReceiveBufferApp.Checked);
            SetOrClearDword("ReceiveBufferRemote", (int)numReceiveBufferRemote.Value, chbReceiveBufferRemote.Checked);
            SetOrClearDword("ConnectTimeoutRemote", (int)numConnTimeout.Value, chbConnTimeout.Checked);

            RegistryUtils.SetDWord("InternalBufferSize", (int)numInternalBufferSize.Value);
            SetOrClearString("BindIP", RelayParams.BindIP.ToString(), !RelayParams.BindIP.Equals(IPAddress.Any));

            Hide();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private static int? GetValue(int value, bool enabled)
        {
            return enabled ? (int?)value : null;
        }

        private static void SetOrClearDword(string valueName, int value, bool enabled)
        {
            if (enabled)
            {
                RegistryUtils.SetDWord(valueName, value);
            }
            else
            {
                RegistryUtils.Remove(valueName);
            }
        }

        private static void SetOrClearString(string valueName, string value, bool enabled)
        {
            if (enabled)
            {
                RegistryUtils.SetString(valueName, value);
            }
            else
            {
                RegistryUtils.Remove(valueName);
            }
        }
    }

    internal class IPAddressItem
    {
        public readonly IPAddress Address;
        public readonly NetworkInterface NetworkInterface;

        public IPAddressItem(NetworkInterface NetworkInterface, IPAddress Address)
        {
            this.Address = Address;
            this.NetworkInterface = NetworkInterface;
        }

        public override string ToString()
        {
            return NetworkInterface.Name + " - " + Address.ToString();
        }
    }

    internal class DefaultIPAddressItem : IPAddressItem
    {
        private string defaultString;

        public DefaultIPAddressItem() : base(null, IPAddress.Any)
        {
            ResourceManager rm = new ResourceManager("TCPRelayWindow.WinFormStrings", typeof(AdvancedSettingsForm).Assembly);
            defaultString = rm.GetString("strDefault");
        }

        public override string ToString()
        {
            return defaultString;
        }
    }
}
