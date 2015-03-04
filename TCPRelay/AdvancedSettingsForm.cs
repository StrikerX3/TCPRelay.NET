using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            chbConnTimeoutRemote.Checked = RelayParams.ConnectTimeout != null;

            chbNoDelayApp.Checked = RelayParams.NoDelayApp;
            chbNoDelayRemote.Checked = RelayParams.NoDelayRemote;

            if (chbSendBufferApp.Checked) numSendBufferApp.Value = (decimal)RelayParams.SendBufferSizeApp;
            if (chbSendBufferRemote.Checked) numSendBufferRemote.Value = (decimal)RelayParams.SendBufferSizeRemote;
            if (chbReceiveBufferApp.Checked) numReceiveBufferApp.Value = (decimal)RelayParams.RecvBufferSizeApp;
            if (chbReceiveBufferRemote.Checked) numReceiveBufferRemote.Value = (decimal)RelayParams.RecvBufferSizeRemote;
            if (chbConnTimeoutRemote.Checked) numConnTimeoutRemote.Value = (decimal)RelayParams.ConnectTimeout;

            // enable the components
            numSendBufferApp.Enabled = chbSendBufferApp.Checked;
            numSendBufferRemote.Enabled = chbSendBufferRemote.Checked;
            numReceiveBufferApp.Enabled = chbReceiveBufferApp.Checked;
            numReceiveBufferRemote.Enabled = chbReceiveBufferRemote.Checked;
            numConnTimeoutRemote.Enabled = chbConnTimeoutRemote.Checked;
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
                rm.GetString("strInternalBufferSize") + ":");

            lblSendBuffer.Text = rm.GetString("strSendBuffer") + ":";
            lblRecvBuffer.Text = rm.GetString("strReceiveBuffer") + ":";
            lblConnTimeout.Text = rm.GetString("strConnectionTimeout") + ":";
            lblNoDelay.Text = rm.GetString("strNoDelay") + ":";
            lblInternalBuffer.Text = rm.GetString("strInternalBufferSize") + ":";

            AdjustWidth(lblSendBuffer);
            AdjustWidth(lblRecvBuffer);
            AdjustWidth(lblConnTimeout);
            AdjustWidth(lblNoDelay);
            AdjustWidth(lblInternalBuffer);

            // reposition all components in columns

            // override app values
            int overrideAppLeft = paramsWidth + margin;
            lblFromApp.Left = overrideAppLeft;
            lblFromApp.Text = rm.GetString("strFromApplication");
            AdjustWidth(lblFromApp);
            chbSendBufferApp.Left = overrideAppLeft;
            chbReceiveBufferApp.Left = overrideAppLeft;
            chbNoDelayApp.Left = overrideAppLeft;

            // app values
            int valueAppLeft = chbSendBufferApp.Right + margin;
            numSendBufferApp.Left = valueAppLeft;
            numReceiveBufferApp.Left = valueAppLeft;
            numInternalBufferSize.Left = valueAppLeft;
            
            // override remote values
            int overrideRemoteLeft = Math.Max(lblFromApp.Right + margin + spacing, numSendBufferApp.Right + margin + spacing);
            lblToRemote.Left = overrideRemoteLeft;
            lblToRemote.Text = rm.GetString("strToRemote");
            AdjustWidth(lblToRemote);
            chbSendBufferRemote.Left = overrideRemoteLeft;
            chbReceiveBufferRemote.Left = overrideRemoteLeft;
            chbConnTimeoutRemote.Left = overrideRemoteLeft;
            chbNoDelayRemote.Left = overrideRemoteLeft;

            // remote values
            int valueRemoteLeft = chbSendBufferRemote.Right + margin;
            numSendBufferRemote.Left = valueRemoteLeft;
            numReceiveBufferRemote.Left = valueRemoteLeft;
            numConnTimeoutRemote.Left = valueRemoteLeft;

            // adjust window size
            int windowWidth = Math.Max(numSendBufferRemote.Right + spacing, lblToRemote.Right + spacing);
            int windowHeight = numInternalBufferSize.Bottom + spacing + btnOK.Height + spacing;
            this.ClientSize = new Size(windowWidth, windowHeight);

            // rename and reposition the OK and Cancel buttons
            int btnWidth = Math.Max(75, 10 + FindWidestString(btnOK.Font,
                rm.GetString("strOK"),
                rm.GetString("strCancel")));
            btnCancel.Text = rm.GetString("strCancel");
            btnCancel.Width = btnWidth;
            btnCancel.Left = numConnTimeoutRemote.Right - btnWidth;
            btnCancel.Top = numInternalBufferSize.Bottom + spacing;
            btnOK.Text = rm.GetString("strOK");
            btnOK.Width = btnWidth;
            btnOK.Left = btnCancel.Left - spacing - btnWidth;
            btnOK.Top = numInternalBufferSize.Bottom + spacing;
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
            numConnTimeoutRemote.Enabled = chbConnTimeoutRemote.Checked;
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
            RelayParams.ConnectTimeout = GetValue((int)numConnTimeoutRemote.Value, chbConnTimeoutRemote.Checked);

            RelayParams.InternalBufferSize = (int)numInternalBufferSize.Value;
            
            
            // save settings to the registry
            RegistryUtils.SetBoolean("NoDelayApp", chbNoDelayApp.Checked);
            RegistryUtils.SetBoolean("NoDelayRemote", chbNoDelayRemote.Checked);

            SetOrClearDword("SendBufferApp", (int)numSendBufferApp.Value, chbSendBufferApp.Checked);
            SetOrClearDword("SendBufferRemote", (int)numSendBufferRemote.Value, chbSendBufferRemote.Checked);
            SetOrClearDword("ReceiveBufferApp", (int)numReceiveBufferApp.Value, chbReceiveBufferApp.Checked);
            SetOrClearDword("ReceiveBufferRemote", (int)numReceiveBufferRemote.Value, chbReceiveBufferRemote.Checked);
            SetOrClearDword("ConnectTimeoutRemote", (int)numConnTimeoutRemote.Value, chbConnTimeoutRemote.Checked);

            RegistryUtils.SetDWord("InternalBufferSize", (int)numInternalBufferSize.Value);

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
    }
}
