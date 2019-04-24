namespace FreeModScan
{
    partial class ConnectionForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbConnType = new System.Windows.Forms.ComboBox();
            this.tbPCPort = new System.Windows.Forms.TextBox();
            this.labelForPCPort = new System.Windows.Forms.Label();
            this.tbIp = new System.Windows.Forms.TextBox();
            this.labelForIp = new System.Windows.Forms.Label();
            this.tbComPort = new System.Windows.Forms.TextBox();
            this.labelForCOMPort = new System.Windows.Forms.Label();
            this.tbConnectionName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbDelayRead = new System.Windows.Forms.TextBox();
            this.tbDelayWrite = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbComParams = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbDataBits = new System.Windows.Forms.ComboBox();
            this.cbStopBits = new System.Windows.Forms.ComboBox();
            this.cbParity = new System.Windows.Forms.ComboBox();
            this.cbBaudrate = new System.Windows.Forms.ComboBox();
            this.btnAddAndConnect = new System.Windows.Forms.Button();
            this.btnAddConnection = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.gbComParams.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cbConnType);
            this.groupBox1.Controls.Add(this.tbPCPort);
            this.groupBox1.Controls.Add(this.labelForPCPort);
            this.groupBox1.Controls.Add(this.tbIp);
            this.groupBox1.Controls.Add(this.labelForIp);
            this.groupBox1.Controls.Add(this.tbComPort);
            this.groupBox1.Controls.Add(this.labelForCOMPort);
            this.groupBox1.Controls.Add(this.tbConnectionName);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(486, 124);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Подключение";
            // 
            // cbConnType
            // 
            this.cbConnType.FormattingEnabled = true;
            this.cbConnType.Items.AddRange(new object[] {
            "Удалённый TCP/IP сервер",
            "COM"});
            this.cbConnType.Location = new System.Drawing.Point(161, 19);
            this.cbConnType.Name = "cbConnType";
            this.cbConnType.Size = new System.Drawing.Size(241, 21);
            this.cbConnType.TabIndex = 0;
            this.cbConnType.SelectedIndexChanged += new System.EventHandler(this.cbDevice_SelectedIndexChanged);
            // 
            // tbPCPort
            // 
            this.tbPCPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPCPort.Location = new System.Drawing.Point(161, 98);
            this.tbPCPort.Name = "tbPCPort";
            this.tbPCPort.Size = new System.Drawing.Size(241, 20);
            this.tbPCPort.TabIndex = 1;
            // 
            // labelForPCPort
            // 
            this.labelForPCPort.AutoSize = true;
            this.labelForPCPort.Location = new System.Drawing.Point(120, 101);
            this.labelForPCPort.Name = "labelForPCPort";
            this.labelForPCPort.Size = new System.Drawing.Size(35, 13);
            this.labelForPCPort.TabIndex = 1;
            this.labelForPCPort.Text = "Порт:";
            // 
            // tbIp
            // 
            this.tbIp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbIp.Location = new System.Drawing.Point(161, 72);
            this.tbIp.Name = "tbIp";
            this.tbIp.Size = new System.Drawing.Size(241, 20);
            this.tbIp.TabIndex = 1;
            // 
            // labelForIp
            // 
            this.labelForIp.AutoSize = true;
            this.labelForIp.Location = new System.Drawing.Point(73, 75);
            this.labelForIp.Name = "labelForIp";
            this.labelForIp.Size = new System.Drawing.Size(82, 13);
            this.labelForIp.TabIndex = 1;
            this.labelForIp.Text = "IP-адрес/Хост:";
            // 
            // tbComPort
            // 
            this.tbComPort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbComPort.Location = new System.Drawing.Point(161, 72);
            this.tbComPort.Name = "tbComPort";
            this.tbComPort.Size = new System.Drawing.Size(241, 20);
            this.tbComPort.TabIndex = 1;
            // 
            // labelForCOMPort
            // 
            this.labelForCOMPort.AutoSize = true;
            this.labelForCOMPort.Location = new System.Drawing.Point(95, 75);
            this.labelForCOMPort.Name = "labelForCOMPort";
            this.labelForCOMPort.Size = new System.Drawing.Size(60, 13);
            this.labelForCOMPort.TabIndex = 1;
            this.labelForCOMPort.Text = "COM-порт:";
            // 
            // tbConnectionName
            // 
            this.tbConnectionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbConnectionName.Location = new System.Drawing.Point(161, 46);
            this.tbConnectionName.Name = "tbConnectionName";
            this.tbConnectionName.Size = new System.Drawing.Size(241, 20);
            this.tbConnectionName.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(53, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(102, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Имя подключения:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(56, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Тип подключения:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.tbDelayRead);
            this.groupBox2.Controls.Add(this.tbDelayWrite);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(317, 142);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(181, 133);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Дополнительные параметры";
            // 
            // tbDelayRead
            // 
            this.tbDelayRead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDelayRead.Location = new System.Drawing.Point(6, 56);
            this.tbDelayRead.Name = "tbDelayRead";
            this.tbDelayRead.Size = new System.Drawing.Size(169, 20);
            this.tbDelayRead.TabIndex = 1;
            this.tbDelayRead.Text = "1000";
            // 
            // tbDelayWrite
            // 
            this.tbDelayWrite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDelayWrite.Location = new System.Drawing.Point(6, 82);
            this.tbDelayWrite.Name = "tbDelayWrite";
            this.tbDelayWrite.Size = new System.Drawing.Size(169, 20);
            this.tbDelayWrite.TabIndex = 1;
            this.tbDelayWrite.Text = "1000";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Таймауты чтения и записи, мс";
            // 
            // gbComParams
            // 
            this.gbComParams.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbComParams.Controls.Add(this.label5);
            this.gbComParams.Controls.Add(this.label4);
            this.gbComParams.Controls.Add(this.label3);
            this.gbComParams.Controls.Add(this.label2);
            this.gbComParams.Controls.Add(this.cbDataBits);
            this.gbComParams.Controls.Add(this.cbStopBits);
            this.gbComParams.Controls.Add(this.cbParity);
            this.gbComParams.Controls.Add(this.cbBaudrate);
            this.gbComParams.Location = new System.Drawing.Point(12, 142);
            this.gbComParams.Name = "gbComParams";
            this.gbComParams.Size = new System.Drawing.Size(299, 133);
            this.gbComParams.TabIndex = 0;
            this.gbComParams.TabStop = false;
            this.gbComParams.Text = "Параметры COM-порта";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(47, 103);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Длина слова:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(61, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Стоп-биты:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(65, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Чётность:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Скорость порта (бод):";
            // 
            // cbDataBits
            // 
            this.cbDataBits.FormattingEnabled = true;
            this.cbDataBits.Items.AddRange(new object[] {
            "7",
            "8"});
            this.cbDataBits.Location = new System.Drawing.Point(129, 100);
            this.cbDataBits.Name = "cbDataBits";
            this.cbDataBits.Size = new System.Drawing.Size(164, 21);
            this.cbDataBits.TabIndex = 0;
            // 
            // cbStopBits
            // 
            this.cbStopBits.FormattingEnabled = true;
            this.cbStopBits.Location = new System.Drawing.Point(129, 73);
            this.cbStopBits.Name = "cbStopBits";
            this.cbStopBits.Size = new System.Drawing.Size(164, 21);
            this.cbStopBits.TabIndex = 0;
            this.cbStopBits.SelectedIndexChanged += new System.EventHandler(this.cbStopBits_SelectedIndexChanged);
            // 
            // cbParity
            // 
            this.cbParity.FormattingEnabled = true;
            this.cbParity.Location = new System.Drawing.Point(129, 46);
            this.cbParity.Name = "cbParity";
            this.cbParity.Size = new System.Drawing.Size(164, 21);
            this.cbParity.TabIndex = 0;
            // 
            // cbBaudrate
            // 
            this.cbBaudrate.FormattingEnabled = true;
            this.cbBaudrate.Items.AddRange(new object[] {
            "110",
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "56000",
            "57600",
            "115200",
            "128000",
            "230400",
            "256000",
            "460800",
            "921600"});
            this.cbBaudrate.Location = new System.Drawing.Point(129, 19);
            this.cbBaudrate.Name = "cbBaudrate";
            this.cbBaudrate.Size = new System.Drawing.Size(164, 21);
            this.cbBaudrate.TabIndex = 0;
            // 
            // btnAddAndConnect
            // 
            this.btnAddAndConnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddAndConnect.Location = new System.Drawing.Point(12, 284);
            this.btnAddAndConnect.Name = "btnAddAndConnect";
            this.btnAddAndConnect.Size = new System.Drawing.Size(155, 23);
            this.btnAddAndConnect.TabIndex = 1;
            this.btnAddAndConnect.Text = "Добавить и подключить";
            this.btnAddAndConnect.UseVisualStyleBackColor = true;
            this.btnAddAndConnect.Click += new System.EventHandler(this.btnAddAndConnect_Click);
            // 
            // btnAddConnection
            // 
            this.btnAddConnection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddConnection.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAddConnection.Location = new System.Drawing.Point(296, 284);
            this.btnAddConnection.Name = "btnAddConnection";
            this.btnAddConnection.Size = new System.Drawing.Size(98, 23);
            this.btnAddConnection.TabIndex = 1;
            this.btnAddConnection.Text = "Добавить";
            this.btnAddConnection.UseVisualStyleBackColor = true;
            this.btnAddConnection.Click += new System.EventHandler(this.btnAddConnection_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(400, 284);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(98, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Отмена";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // ConnectionForm
            // 
            this.AcceptButton = this.btnAddConnection;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(510, 319);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnAddConnection);
            this.Controls.Add(this.btnAddAndConnect);
            this.Controls.Add(this.gbComParams);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConnectionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Добавить подключение";
            this.Load += new System.EventHandler(this.ConnectionForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.gbComParams.ResumeLayout(false);
            this.gbComParams.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbDelayWrite;
        private System.Windows.Forms.TextBox tbConnectionName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbComParams;
        private System.Windows.Forms.Button btnAddAndConnect;
        private System.Windows.Forms.Button btnAddConnection;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbDataBits;
        private System.Windows.Forms.ComboBox cbStopBits;
        private System.Windows.Forms.ComboBox cbParity;
        private System.Windows.Forms.ComboBox cbConnType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbComPort;
        private System.Windows.Forms.Label labelForCOMPort;
        private System.Windows.Forms.TextBox tbPCPort;
        private System.Windows.Forms.Label labelForPCPort;
        private System.Windows.Forms.TextBox tbDelayRead;
        private System.Windows.Forms.TextBox tbIp;
        private System.Windows.Forms.Label labelForIp;
        public System.Windows.Forms.ComboBox cbBaudrate;
    }
}