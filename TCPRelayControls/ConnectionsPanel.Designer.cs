namespace TCPRelayControls
{
    partial class ConnectionsPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionsPanel));
            this.grpConnections = new System.Windows.Forms.GroupBox();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btnSlowest = new System.Windows.Forms.Button();
            this.btnSlower = new System.Windows.Forms.Button();
            this.btnFaster = new System.Windows.Forms.Button();
            this.btnFastest = new System.Windows.Forms.Button();
            this.btnCloseAll = new System.Windows.Forms.Button();
            this.panel1 = new TCPRelayControls.DoubleBufferedPanel();
            this.grpConnections.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpConnections
            // 
            resources.ApplyResources(this.grpConnections, "grpConnections");
            this.grpConnections.Controls.Add(this.panel1);
            this.grpConnections.Name = "grpConnections";
            this.grpConnections.TabStop = false;
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 500;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // btnSlowest
            // 
            resources.ApplyResources(this.btnSlowest, "btnSlowest");
            this.btnSlowest.Name = "btnSlowest";
            this.toolTip1.SetToolTip(this.btnSlowest, resources.GetString("btnSlowest.ToolTip"));
            this.btnSlowest.UseVisualStyleBackColor = true;
            this.btnSlowest.Click += new System.EventHandler(this.btnSlowest_Click);
            // 
            // btnSlower
            // 
            resources.ApplyResources(this.btnSlower, "btnSlower");
            this.btnSlower.Name = "btnSlower";
            this.toolTip1.SetToolTip(this.btnSlower, resources.GetString("btnSlower.ToolTip"));
            this.btnSlower.UseVisualStyleBackColor = true;
            this.btnSlower.Click += new System.EventHandler(this.btnSlower_Click);
            // 
            // btnFaster
            // 
            resources.ApplyResources(this.btnFaster, "btnFaster");
            this.btnFaster.Name = "btnFaster";
            this.toolTip1.SetToolTip(this.btnFaster, resources.GetString("btnFaster.ToolTip"));
            this.btnFaster.UseVisualStyleBackColor = true;
            this.btnFaster.Click += new System.EventHandler(this.btnFaster_Click);
            // 
            // btnFastest
            // 
            resources.ApplyResources(this.btnFastest, "btnFastest");
            this.btnFastest.Name = "btnFastest";
            this.toolTip1.SetToolTip(this.btnFastest, resources.GetString("btnFastest.ToolTip"));
            this.btnFastest.UseVisualStyleBackColor = true;
            this.btnFastest.Click += new System.EventHandler(this.btnFastest_Click);
            // 
            // btnCloseAll
            // 
            resources.ApplyResources(this.btnCloseAll, "btnCloseAll");
            this.btnCloseAll.Image = global::TCPRelayControls.Properties.Resources.XX;
            this.btnCloseAll.Name = "btnCloseAll";
            this.toolTip1.SetToolTip(this.btnCloseAll, resources.GetString("btnCloseAll.ToolTip"));
            this.btnCloseAll.UseVisualStyleBackColor = true;
            this.btnCloseAll.Click += new System.EventHandler(this.btnCloseAll_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            this.panel1.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.panel1_ControlAdded);
            this.panel1.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.panel1_ControlRemoved);
            // 
            // ConnectionsPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSlowest);
            this.Controls.Add(this.btnSlower);
            this.Controls.Add(this.btnFaster);
            this.Controls.Add(this.btnFastest);
            this.Controls.Add(this.btnCloseAll);
            this.Controls.Add(this.grpConnections);
            this.Name = "ConnectionsPanel";
            this.grpConnections.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpConnections;
        private System.Windows.Forms.Timer tmrUpdate;
        private System.Windows.Forms.Button btnCloseAll;
        private System.Windows.Forms.ToolTip toolTip1;
        private DoubleBufferedPanel panel1;
        private System.Windows.Forms.Button btnFastest;
        private System.Windows.Forms.Button btnFaster;
        private System.Windows.Forms.Button btnSlower;
        private System.Windows.Forms.Button btnSlowest;
    }
}
