namespace ParadoxApiDemo
{
    partial class FormPanelSettings
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
            this.cbxConnectionType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cbxSerialComPort = new System.Windows.Forms.ComboBox();
            this.cbxSerialBaudRate = new System.Windows.Forms.ComboBox();
            this.txbIPPort = new System.Windows.Forms.MaskedTextBox();
            this.txbDNSSiteID = new System.Windows.Forms.TextBox();
            this.txtIPModulePassword = new System.Windows.Forms.TextBox();
            this.txbUserPassword = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.ipAddressControl = new IPAddressControlLib.IPAddressControl();
            this.label9 = new System.Windows.Forms.Label();
            this.cbxPanelType = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbxSystemAlarmLanguage = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cbxConnectionType
            // 
            this.cbxConnectionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxConnectionType.FormattingEnabled = true;
            this.cbxConnectionType.Items.AddRange(new object[] {
            "Serial",
            "IP/Static",
            "GPRS Callback",
            "GPRS Private",
            "IP/DNS"});
            this.cbxConnectionType.Location = new System.Drawing.Point(183, 74);
            this.cbxConnectionType.Name = "cbxConnectionType";
            this.cbxConnectionType.Size = new System.Drawing.Size(159, 21);
            this.cbxConnectionType.TabIndex = 0;
            this.cbxConnectionType.SelectedValueChanged += new System.EventHandler(this.cbxConnectionType_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Connection Type";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 205);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "IP Address";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 237);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "IP Port";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Serial Com. Port";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 269);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "IP Module Password";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 173);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "DNS Site ID";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(23, 141);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(114, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Serial Com. Baud Rate";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 301);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "User Password";
            // 
            // cbxSerialComPort
            // 
            this.cbxSerialComPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSerialComPort.FormattingEnabled = true;
            this.cbxSerialComPort.Items.AddRange(new object[] {
            "COM1",
            "COM2",
            "COM3",
            "COM4",
            "COM5",
            "COM6",
            "COM7",
            "COM8",
            "COM9",
            "COM10",
            "COM11",
            "COM12",
            "COM13",
            "COM14",
            "COM15",
            "COM16",
            "COM17",
            "COM18",
            "COM19",
            "COM20",
            "COM21",
            "COM22",
            "COM24"});
            this.cbxSerialComPort.Location = new System.Drawing.Point(183, 106);
            this.cbxSerialComPort.Name = "cbxSerialComPort";
            this.cbxSerialComPort.Size = new System.Drawing.Size(159, 21);
            this.cbxSerialComPort.TabIndex = 9;
            // 
            // cbxSerialBaudRate
            // 
            this.cbxSerialBaudRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSerialBaudRate.FormattingEnabled = true;
            this.cbxSerialBaudRate.Items.AddRange(new object[] {
            "Auto-Detect",
            "921600",
            "115200",
            "57600",
            "38400",
            "19200",
            "9600"});
            this.cbxSerialBaudRate.Location = new System.Drawing.Point(183, 138);
            this.cbxSerialBaudRate.Name = "cbxSerialBaudRate";
            this.cbxSerialBaudRate.Size = new System.Drawing.Size(159, 21);
            this.cbxSerialBaudRate.TabIndex = 10;
            // 
            // txbIPPort
            // 
            this.txbIPPort.Location = new System.Drawing.Point(183, 234);
            this.txbIPPort.Mask = "00009";
            this.txbIPPort.Name = "txbIPPort";
            this.txbIPPort.PromptChar = ' ';
            this.txbIPPort.Size = new System.Drawing.Size(43, 20);
            this.txbIPPort.TabIndex = 12;
            this.txbIPPort.Text = "10000";
            this.txbIPPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txbDNSSiteID
            // 
            this.txbDNSSiteID.Location = new System.Drawing.Point(183, 170);
            this.txbDNSSiteID.Name = "txbDNSSiteID";
            this.txbDNSSiteID.Size = new System.Drawing.Size(159, 20);
            this.txbDNSSiteID.TabIndex = 13;
            // 
            // txtIPModulePassword
            // 
            this.txtIPModulePassword.Location = new System.Drawing.Point(183, 266);
            this.txtIPModulePassword.Name = "txtIPModulePassword";
            this.txtIPModulePassword.Size = new System.Drawing.Size(159, 20);
            this.txtIPModulePassword.TabIndex = 14;
            this.txtIPModulePassword.Text = "paradox";
            // 
            // txbUserPassword
            // 
            this.txbUserPassword.Location = new System.Drawing.Point(183, 298);
            this.txbUserPassword.Name = "txbUserPassword";
            this.txbUserPassword.Size = new System.Drawing.Size(43, 20);
            this.txbUserPassword.TabIndex = 15;
            this.txbUserPassword.Text = "1234";
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(267, 371);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(183, 371);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 17;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // ipAddressControl
            // 
            this.ipAddressControl.AllowInternalTab = false;
            this.ipAddressControl.AutoHeight = true;
            this.ipAddressControl.BackColor = System.Drawing.SystemColors.Window;
            this.ipAddressControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.ipAddressControl.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ipAddressControl.Location = new System.Drawing.Point(183, 202);
            this.ipAddressControl.MinimumSize = new System.Drawing.Size(87, 20);
            this.ipAddressControl.Name = "ipAddressControl";
            this.ipAddressControl.ReadOnly = false;
            this.ipAddressControl.Size = new System.Drawing.Size(159, 20);
            this.ipAddressControl.TabIndex = 18;
            this.ipAddressControl.Text = "...";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(23, 41);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(61, 13);
            this.label9.TabIndex = 20;
            this.label9.Text = "Panel Type";
            // 
            // cbxPanelType
            // 
            this.cbxPanelType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPanelType.FormattingEnabled = true;
            this.cbxPanelType.Items.AddRange(new object[] {
            "Serial",
            "IP/Static",
            "GPRS Callback",
            "IP/DNS"});
            this.cbxPanelType.Location = new System.Drawing.Point(183, 38);
            this.cbxPanelType.Name = "cbxPanelType";
            this.cbxPanelType.Size = new System.Drawing.Size(159, 21);
            this.cbxPanelType.TabIndex = 19;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(25, 333);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(121, 13);
            this.label10.TabIndex = 22;
            this.label10.Text = "System Alarm Language";
            // 
            // cbxSystemAlarmLanguage
            // 
            this.cbxSystemAlarmLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSystemAlarmLanguage.FormattingEnabled = true;
            this.cbxSystemAlarmLanguage.Items.AddRange(new object[] {
            "Serial",
            "IP/Static",
            "GPRS Callback",
            "IP/DNS"});
            this.cbxSystemAlarmLanguage.Location = new System.Drawing.Point(185, 330);
            this.cbxSystemAlarmLanguage.Name = "cbxSystemAlarmLanguage";
            this.cbxSystemAlarmLanguage.Size = new System.Drawing.Size(159, 21);
            this.cbxSystemAlarmLanguage.TabIndex = 21;
            // 
            // FormPanelSettings
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(369, 406);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.cbxSystemAlarmLanguage);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.cbxPanelType);
            this.Controls.Add(this.ipAddressControl);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.txbUserPassword);
            this.Controls.Add(this.txtIPModulePassword);
            this.Controls.Add(this.txbDNSSiteID);
            this.Controls.Add(this.txbIPPort);
            this.Controls.Add(this.cbxSerialBaudRate);
            this.Controls.Add(this.cbxSerialComPort);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbxConnectionType);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "FormPanelSettings";
            this.Text = "Panel Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbxConnectionType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbxSerialComPort;
        private System.Windows.Forms.ComboBox cbxSerialBaudRate;
        private System.Windows.Forms.MaskedTextBox txbIPPort;
        private System.Windows.Forms.TextBox txbDNSSiteID;
        private System.Windows.Forms.TextBox txtIPModulePassword;
        private System.Windows.Forms.TextBox txbUserPassword;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private IPAddressControlLib.IPAddressControl ipAddressControl;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbxPanelType;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cbxSystemAlarmLanguage;
    }
}