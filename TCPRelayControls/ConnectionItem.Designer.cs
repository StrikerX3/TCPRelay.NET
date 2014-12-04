namespace TCPRelayControls
{
    partial class ConnectionItem
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectionItem));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.pnlGraph = new TCPRelayControls.DoubleBufferedPanel();
            this.lblDownloaded = new System.Windows.Forms.Label();
            this.lblDownSpd = new System.Windows.Forms.Label();
            this.lblIn = new System.Windows.Forms.Label();
            this.lblUploaded = new System.Windows.Forms.Label();
            this.lblUpSpd = new System.Windows.Forms.Label();
            this.lblTransferred = new System.Windows.Forms.Label();
            this.lblOut = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.lblSpeed);
            this.groupBox1.Controls.Add(this.pnlGraph);
            this.groupBox1.Controls.Add(this.lblDownloaded);
            this.groupBox1.Controls.Add(this.lblDownSpd);
            this.groupBox1.Controls.Add(this.lblIn);
            this.groupBox1.Controls.Add(this.lblUploaded);
            this.groupBox1.Controls.Add(this.lblUpSpd);
            this.groupBox1.Controls.Add(this.lblTransferred);
            this.groupBox1.Controls.Add(this.lblOut);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lblSpeed
            // 
            resources.ApplyResources(this.lblSpeed, "lblSpeed");
            this.lblSpeed.Name = "lblSpeed";
            // 
            // pnlGraph
            // 
            resources.ApplyResources(this.pnlGraph, "pnlGraph");
            this.pnlGraph.BackColor = System.Drawing.Color.Black;
            this.pnlGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGraph.Name = "pnlGraph";
            this.pnlGraph.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlGraph_Paint);
            this.pnlGraph.Resize += new System.EventHandler(this.pnlGraph_Resize);
            // 
            // lblDownloaded
            // 
            resources.ApplyResources(this.lblDownloaded, "lblDownloaded");
            this.lblDownloaded.Name = "lblDownloaded";
            // 
            // lblDownSpd
            // 
            resources.ApplyResources(this.lblDownSpd, "lblDownSpd");
            this.lblDownSpd.Name = "lblDownSpd";
            // 
            // lblIn
            // 
            resources.ApplyResources(this.lblIn, "lblIn");
            this.lblIn.Name = "lblIn";
            // 
            // lblUploaded
            // 
            resources.ApplyResources(this.lblUploaded, "lblUploaded");
            this.lblUploaded.Name = "lblUploaded";
            // 
            // lblUpSpd
            // 
            resources.ApplyResources(this.lblUpSpd, "lblUpSpd");
            this.lblUpSpd.Name = "lblUpSpd";
            // 
            // lblTransferred
            // 
            resources.ApplyResources(this.lblTransferred, "lblTransferred");
            this.lblTransferred.Name = "lblTransferred";
            // 
            // lblOut
            // 
            resources.ApplyResources(this.lblOut, "lblOut");
            this.lblOut.Name = "lblOut";
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Image = global::TCPRelayControls.Properties.Resources.X;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ConnectionItem
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.Name = "ConnectionItem";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblDownloaded;
        private System.Windows.Forms.Label lblIn;
        private System.Windows.Forms.Label lblUploaded;
        private System.Windows.Forms.Label lblOut;
        private System.Windows.Forms.Label lblTransferred;
        private System.Windows.Forms.Label lblDownSpd;
        private System.Windows.Forms.Label lblUpSpd;
        private System.Windows.Forms.Button btnClose;
        private DoubleBufferedPanel pnlGraph;
        private System.Windows.Forms.Label lblSpeed;

    }
}
