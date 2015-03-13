namespace TCPRelayWindow
{
    partial class AdvancedSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedSettingsForm));
            this.lblFromApp = new System.Windows.Forms.Label();
            this.lblToRemote = new System.Windows.Forms.Label();
            this.chbSendBufferApp = new System.Windows.Forms.CheckBox();
            this.lblSendBuffer = new System.Windows.Forms.Label();
            this.lblRecvBuffer = new System.Windows.Forms.Label();
            this.chbReceiveBufferApp = new System.Windows.Forms.CheckBox();
            this.chbReceiveBufferRemote = new System.Windows.Forms.CheckBox();
            this.chbSendBufferRemote = new System.Windows.Forms.CheckBox();
            this.numSendBufferApp = new System.Windows.Forms.NumericUpDown();
            this.numReceiveBufferApp = new System.Windows.Forms.NumericUpDown();
            this.numReceiveBufferRemote = new System.Windows.Forms.NumericUpDown();
            this.numSendBufferRemote = new System.Windows.Forms.NumericUpDown();
            this.chbNoDelayApp = new System.Windows.Forms.CheckBox();
            this.lblNoDelay = new System.Windows.Forms.Label();
            this.chbNoDelayRemote = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.numConnTimeout = new System.Windows.Forms.NumericUpDown();
            this.numInternalBufferSize = new System.Windows.Forms.NumericUpDown();
            this.lblInternalBuffer = new System.Windows.Forms.Label();
            this.lblBindToAddress = new System.Windows.Forms.Label();
            this.cbxAddresses = new System.Windows.Forms.ComboBox();
            this.chbConnTimeout = new System.Windows.Forms.CheckBox();
            this.lblConnTimeout = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numSendBufferApp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReceiveBufferApp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReceiveBufferRemote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSendBufferRemote)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConnTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInternalBufferSize)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFromApp
            // 
            this.lblFromApp.AutoSize = true;
            this.lblFromApp.Location = new System.Drawing.Point(178, 9);
            this.lblFromApp.Name = "lblFromApp";
            this.lblFromApp.Size = new System.Drawing.Size(85, 13);
            this.lblFromApp.TabIndex = 0;
            this.lblFromApp.Text = "From Application";
            // 
            // lblToRemote
            // 
            this.lblToRemote.AutoSize = true;
            this.lblToRemote.Location = new System.Drawing.Point(300, 9);
            this.lblToRemote.Name = "lblToRemote";
            this.lblToRemote.Size = new System.Drawing.Size(60, 13);
            this.lblToRemote.TabIndex = 1;
            this.lblToRemote.Text = "To Remote";
            // 
            // chbSendBufferApp
            // 
            this.chbSendBufferApp.AutoSize = true;
            this.chbSendBufferApp.Location = new System.Drawing.Point(160, 27);
            this.chbSendBufferApp.Name = "chbSendBufferApp";
            this.chbSendBufferApp.Size = new System.Drawing.Size(15, 14);
            this.chbSendBufferApp.TabIndex = 1;
            this.chbSendBufferApp.UseVisualStyleBackColor = true;
            this.chbSendBufferApp.CheckedChanged += new System.EventHandler(this.chbSendBufferApp_CheckedChanged);
            // 
            // lblSendBuffer
            // 
            this.lblSendBuffer.AutoSize = true;
            this.lblSendBuffer.Location = new System.Drawing.Point(12, 27);
            this.lblSendBuffer.Name = "lblSendBuffer";
            this.lblSendBuffer.Size = new System.Drawing.Size(109, 13);
            this.lblSendBuffer.TabIndex = 3;
            this.lblSendBuffer.Text = "Send Buffer Size (KB)";
            // 
            // lblRecvBuffer
            // 
            this.lblRecvBuffer.AutoSize = true;
            this.lblRecvBuffer.Location = new System.Drawing.Point(12, 53);
            this.lblRecvBuffer.Name = "lblRecvBuffer";
            this.lblRecvBuffer.Size = new System.Drawing.Size(124, 13);
            this.lblRecvBuffer.TabIndex = 4;
            this.lblRecvBuffer.Text = "Receive Buffer Size (KB)";
            // 
            // chbReceiveBufferApp
            // 
            this.chbReceiveBufferApp.AutoSize = true;
            this.chbReceiveBufferApp.Location = new System.Drawing.Point(160, 52);
            this.chbReceiveBufferApp.Name = "chbReceiveBufferApp";
            this.chbReceiveBufferApp.Size = new System.Drawing.Size(15, 14);
            this.chbReceiveBufferApp.TabIndex = 5;
            this.chbReceiveBufferApp.UseVisualStyleBackColor = true;
            this.chbReceiveBufferApp.CheckedChanged += new System.EventHandler(this.chbReceiveBufferApp_CheckedChanged);
            // 
            // chbReceiveBufferRemote
            // 
            this.chbReceiveBufferRemote.AutoSize = true;
            this.chbReceiveBufferRemote.Location = new System.Drawing.Point(282, 52);
            this.chbReceiveBufferRemote.Name = "chbReceiveBufferRemote";
            this.chbReceiveBufferRemote.Size = new System.Drawing.Size(15, 14);
            this.chbReceiveBufferRemote.TabIndex = 7;
            this.chbReceiveBufferRemote.UseVisualStyleBackColor = true;
            this.chbReceiveBufferRemote.CheckedChanged += new System.EventHandler(this.chbReceiveBufferRemote_CheckedChanged);
            // 
            // chbSendBufferRemote
            // 
            this.chbSendBufferRemote.AutoSize = true;
            this.chbSendBufferRemote.Location = new System.Drawing.Point(282, 26);
            this.chbSendBufferRemote.Name = "chbSendBufferRemote";
            this.chbSendBufferRemote.Size = new System.Drawing.Size(15, 14);
            this.chbSendBufferRemote.TabIndex = 3;
            this.chbSendBufferRemote.UseVisualStyleBackColor = true;
            this.chbSendBufferRemote.CheckedChanged += new System.EventHandler(this.chbSendBufferRemote_CheckedChanged);
            // 
            // numSendBufferApp
            // 
            this.numSendBufferApp.Location = new System.Drawing.Point(181, 25);
            this.numSendBufferApp.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
            this.numSendBufferApp.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numSendBufferApp.Name = "numSendBufferApp";
            this.numSendBufferApp.Size = new System.Drawing.Size(65, 20);
            this.numSendBufferApp.TabIndex = 2;
            this.numSendBufferApp.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // numReceiveBufferApp
            // 
            this.numReceiveBufferApp.Location = new System.Drawing.Point(181, 50);
            this.numReceiveBufferApp.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
            this.numReceiveBufferApp.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numReceiveBufferApp.Name = "numReceiveBufferApp";
            this.numReceiveBufferApp.Size = new System.Drawing.Size(65, 20);
            this.numReceiveBufferApp.TabIndex = 6;
            this.numReceiveBufferApp.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // numReceiveBufferRemote
            // 
            this.numReceiveBufferRemote.Location = new System.Drawing.Point(303, 49);
            this.numReceiveBufferRemote.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
            this.numReceiveBufferRemote.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numReceiveBufferRemote.Name = "numReceiveBufferRemote";
            this.numReceiveBufferRemote.Size = new System.Drawing.Size(65, 20);
            this.numReceiveBufferRemote.TabIndex = 8;
            this.numReceiveBufferRemote.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // numSendBufferRemote
            // 
            this.numSendBufferRemote.Location = new System.Drawing.Point(303, 24);
            this.numSendBufferRemote.Maximum = new decimal(new int[] {
            16384,
            0,
            0,
            0});
            this.numSendBufferRemote.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numSendBufferRemote.Name = "numSendBufferRemote";
            this.numSendBufferRemote.Size = new System.Drawing.Size(65, 20);
            this.numSendBufferRemote.TabIndex = 4;
            this.numSendBufferRemote.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            // 
            // chbNoDelayApp
            // 
            this.chbNoDelayApp.AutoSize = true;
            this.chbNoDelayApp.Location = new System.Drawing.Point(160, 77);
            this.chbNoDelayApp.Name = "chbNoDelayApp";
            this.chbNoDelayApp.Size = new System.Drawing.Size(15, 14);
            this.chbNoDelayApp.TabIndex = 9;
            this.chbNoDelayApp.UseVisualStyleBackColor = true;
            // 
            // lblNoDelay
            // 
            this.lblNoDelay.AutoSize = true;
            this.lblNoDelay.Location = new System.Drawing.Point(12, 78);
            this.lblNoDelay.Name = "lblNoDelay";
            this.lblNoDelay.Size = new System.Drawing.Size(87, 13);
            this.lblNoDelay.TabIndex = 13;
            this.lblNoDelay.Text = "Enable No Delay";
            // 
            // chbNoDelayRemote
            // 
            this.chbNoDelayRemote.AutoSize = true;
            this.chbNoDelayRemote.Location = new System.Drawing.Point(282, 77);
            this.chbNoDelayRemote.Name = "chbNoDelayRemote";
            this.chbNoDelayRemote.Size = new System.Drawing.Size(15, 14);
            this.chbNoDelayRemote.TabIndex = 10;
            this.chbNoDelayRemote.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(226, 208);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 15;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(314, 208);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // numConnTimeout
            // 
            this.numConnTimeout.Location = new System.Drawing.Point(181, 108);
            this.numConnTimeout.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numConnTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numConnTimeout.Name = "numConnTimeout";
            this.numConnTimeout.Size = new System.Drawing.Size(65, 20);
            this.numConnTimeout.TabIndex = 12;
            this.numConnTimeout.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // numInternalBufferSize
            // 
            this.numInternalBufferSize.Location = new System.Drawing.Point(181, 135);
            this.numInternalBufferSize.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numInternalBufferSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numInternalBufferSize.Name = "numInternalBufferSize";
            this.numInternalBufferSize.Size = new System.Drawing.Size(65, 20);
            this.numInternalBufferSize.TabIndex = 13;
            this.numInternalBufferSize.Value = new decimal(new int[] {
            64,
            0,
            0,
            0});
            // 
            // lblInternalBuffer
            // 
            this.lblInternalBuffer.AutoSize = true;
            this.lblInternalBuffer.Location = new System.Drawing.Point(12, 137);
            this.lblInternalBuffer.Name = "lblInternalBuffer";
            this.lblInternalBuffer.Size = new System.Drawing.Size(119, 13);
            this.lblInternalBuffer.TabIndex = 16;
            this.lblInternalBuffer.Text = "Internal Buffer Size (KB)";
            // 
            // lblBindToAddress
            // 
            this.lblBindToAddress.AutoSize = true;
            this.lblBindToAddress.Location = new System.Drawing.Point(12, 164);
            this.lblBindToAddress.Name = "lblBindToAddress";
            this.lblBindToAddress.Size = new System.Drawing.Size(81, 13);
            this.lblBindToAddress.TabIndex = 18;
            this.lblBindToAddress.Text = "Bind to Address";
            // 
            // cbxAddresses
            // 
            this.cbxAddresses.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxAddresses.FormattingEnabled = true;
            this.cbxAddresses.Location = new System.Drawing.Point(181, 161);
            this.cbxAddresses.Name = "cbxAddresses";
            this.cbxAddresses.Size = new System.Drawing.Size(208, 21);
            this.cbxAddresses.TabIndex = 14;
            // 
            // chbConnTimeout
            // 
            this.chbConnTimeout.AutoSize = true;
            this.chbConnTimeout.Location = new System.Drawing.Point(160, 111);
            this.chbConnTimeout.Name = "chbConnTimeout";
            this.chbConnTimeout.Size = new System.Drawing.Size(15, 14);
            this.chbConnTimeout.TabIndex = 11;
            this.chbConnTimeout.UseVisualStyleBackColor = true;
            this.chbConnTimeout.CheckedChanged += new System.EventHandler(this.chbConnTimeoutRemote_CheckedChanged);
            // 
            // lblConnTimeout
            // 
            this.lblConnTimeout.AutoSize = true;
            this.lblConnTimeout.Location = new System.Drawing.Point(12, 110);
            this.lblConnTimeout.Name = "lblConnTimeout";
            this.lblConnTimeout.Size = new System.Drawing.Size(131, 13);
            this.lblConnTimeout.TabIndex = 14;
            this.lblConnTimeout.Text = "Connection Timeout (sec.)";
            // 
            // AdvancedSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(401, 243);
            this.Controls.Add(this.lblBindToAddress);
            this.Controls.Add(this.cbxAddresses);
            this.Controls.Add(this.numInternalBufferSize);
            this.Controls.Add(this.lblInternalBuffer);
            this.Controls.Add(this.numConnTimeout);
            this.Controls.Add(this.chbConnTimeout);
            this.Controls.Add(this.lblConnTimeout);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chbNoDelayRemote);
            this.Controls.Add(this.lblNoDelay);
            this.Controls.Add(this.chbNoDelayApp);
            this.Controls.Add(this.numReceiveBufferRemote);
            this.Controls.Add(this.numSendBufferRemote);
            this.Controls.Add(this.numReceiveBufferApp);
            this.Controls.Add(this.numSendBufferApp);
            this.Controls.Add(this.chbReceiveBufferRemote);
            this.Controls.Add(this.chbSendBufferRemote);
            this.Controls.Add(this.chbReceiveBufferApp);
            this.Controls.Add(this.lblRecvBuffer);
            this.Controls.Add(this.lblSendBuffer);
            this.Controls.Add(this.chbSendBufferApp);
            this.Controls.Add(this.lblToRemote);
            this.Controls.Add(this.lblFromApp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdvancedSettingsForm";
            this.Text = "Advanced Settings";
            ((System.ComponentModel.ISupportInitialize)(this.numSendBufferApp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReceiveBufferApp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReceiveBufferRemote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSendBufferRemote)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConnTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numInternalBufferSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFromApp;
        private System.Windows.Forms.Label lblToRemote;
        private System.Windows.Forms.CheckBox chbSendBufferApp;
        private System.Windows.Forms.Label lblSendBuffer;
        private System.Windows.Forms.Label lblRecvBuffer;
        private System.Windows.Forms.CheckBox chbReceiveBufferApp;
        private System.Windows.Forms.CheckBox chbReceiveBufferRemote;
        private System.Windows.Forms.CheckBox chbSendBufferRemote;
        private System.Windows.Forms.NumericUpDown numSendBufferApp;
        private System.Windows.Forms.NumericUpDown numReceiveBufferApp;
        private System.Windows.Forms.NumericUpDown numReceiveBufferRemote;
        private System.Windows.Forms.NumericUpDown numSendBufferRemote;
        private System.Windows.Forms.CheckBox chbNoDelayApp;
        private System.Windows.Forms.Label lblNoDelay;
        private System.Windows.Forms.CheckBox chbNoDelayRemote;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.NumericUpDown numConnTimeout;
        private System.Windows.Forms.NumericUpDown numInternalBufferSize;
        private System.Windows.Forms.Label lblInternalBuffer;
        private System.Windows.Forms.Label lblBindToAddress;
        private System.Windows.Forms.ComboBox cbxAddresses;
        private System.Windows.Forms.CheckBox chbConnTimeout;
        private System.Windows.Forms.Label lblConnTimeout;
    }
}