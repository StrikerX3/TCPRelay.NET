using System.Windows.Forms;

namespace TCPRelayWindow
{
    partial class TCPRelayForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TCPRelayForm));
            this.lblTargetURI = new System.Windows.Forms.Label();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.lblListenPort = new System.Windows.Forms.Label();
            this.numListenPort = new System.Windows.Forms.NumericUpDown();
            this.cbxTargetURI = new System.Windows.Forms.ComboBox();
            this.initTargetURIListWorker = new System.ComponentModel.BackgroundWorker();
            this.btnLoadTTVServers = new System.Windows.Forms.Button();
            this.lblRunning = new System.Windows.Forms.Label();
            this.lblVersionCopyright = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tcpRelayConnectionsPanel1 = new TCPRelayControls.ConnectionsPanel();
            ((System.ComponentModel.ISupportInitialize)(this.numListenPort)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTargetURI
            // 
            resources.ApplyResources(this.lblTargetURI, "lblTargetURI");
            this.lblTargetURI.Name = "lblTargetURI";
            // 
            // btnStartStop
            // 
            resources.ApplyResources(this.btnStartStop, "btnStartStop");
            this.btnStartStop.Name = "btnStartStop";
            this.toolTip1.SetToolTip(this.btnStartStop, resources.GetString("btnStartStop.ToolTip"));
            this.btnStartStop.UseVisualStyleBackColor = true;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // lblListenPort
            // 
            resources.ApplyResources(this.lblListenPort, "lblListenPort");
            this.lblListenPort.Name = "lblListenPort";
            // 
            // numListenPort
            // 
            resources.ApplyResources(this.numListenPort, "numListenPort");
            this.numListenPort.Maximum = new decimal(new int[] {
            65500,
            0,
            0,
            0});
            this.numListenPort.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numListenPort.Name = "numListenPort";
            this.toolTip1.SetToolTip(this.numListenPort, resources.GetString("numListenPort.ToolTip"));
            this.numListenPort.Value = new decimal(new int[] {
            1935,
            0,
            0,
            0});
            // 
            // cbxTargetURI
            // 
            resources.ApplyResources(this.cbxTargetURI, "cbxTargetURI");
            this.cbxTargetURI.FormattingEnabled = true;
            this.cbxTargetURI.Name = "cbxTargetURI";
            this.toolTip1.SetToolTip(this.cbxTargetURI, resources.GetString("cbxTargetURI.ToolTip"));
            this.cbxTargetURI.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cbxTargetURI_KeyDown);
            // 
            // initTargetURIListWorker
            // 
            this.initTargetURIListWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.initTargetURIListWorker_DoWork);
            this.initTargetURIListWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.initTargetURIListWorker_RunWorkerCompleted);
            // 
            // btnLoadTTVServers
            // 
            resources.ApplyResources(this.btnLoadTTVServers, "btnLoadTTVServers");
            this.btnLoadTTVServers.Name = "btnLoadTTVServers";
            this.toolTip1.SetToolTip(this.btnLoadTTVServers, resources.GetString("btnLoadTTVServers.ToolTip"));
            this.btnLoadTTVServers.UseVisualStyleBackColor = true;
            this.btnLoadTTVServers.Click += new System.EventHandler(this.btnLoadTTVServers_Click);
            // 
            // lblRunning
            // 
            resources.ApplyResources(this.lblRunning, "lblRunning");
            this.lblRunning.Name = "lblRunning";
            // 
            // lblVersionCopyright
            // 
            resources.ApplyResources(this.lblVersionCopyright, "lblVersionCopyright");
            this.lblVersionCopyright.Name = "lblVersionCopyright";
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 5000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // tcpRelayConnectionsPanel1
            // 
            resources.ApplyResources(this.tcpRelayConnectionsPanel1, "tcpRelayConnectionsPanel1");
            this.tcpRelayConnectionsPanel1.Name = "tcpRelayConnectionsPanel1";
            // 
            // TCPRelayForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblVersionCopyright);
            this.Controls.Add(this.tcpRelayConnectionsPanel1);
            this.Controls.Add(this.lblRunning);
            this.Controls.Add(this.cbxTargetURI);
            this.Controls.Add(this.numListenPort);
            this.Controls.Add(this.lblListenPort);
            this.Controls.Add(this.btnLoadTTVServers);
            this.Controls.Add(this.btnStartStop);
            this.Controls.Add(this.lblTargetURI);
            this.Name = "TCPRelayForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TCPRelayForm_FormClosing);
            this.Load += new System.EventHandler(this.TCPRelayForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numListenPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label lblTargetURI;
        private Button btnStartStop;
        private Label lblListenPort;
        private NumericUpDown numListenPort;
        private ComboBox cbxTargetURI;
        private System.ComponentModel.BackgroundWorker initTargetURIListWorker;
        private Button btnLoadTTVServers;
        private Label lblRunning;
        private TCPRelayControls.ConnectionsPanel tcpRelayConnectionsPanel1;
        private Label lblVersionCopyright;
        private ToolTip toolTip1;
    }
}

