namespace FreeModScan
{
    partial class WriteRegForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.cbDeviceList = new System.Windows.Forms.ComboBox();
            this.cbConnectionList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbRegisterType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.tbRegisterNum = new System.Windows.Forms.NumericUpDown();
            this.btnClearValue = new System.Windows.Forms.Button();
            this.numVal = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.tbRegisterNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVal)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Имя устройства: ";
            // 
            // cbDeviceList
            // 
            this.cbDeviceList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDeviceList.FormattingEnabled = true;
            this.cbDeviceList.Location = new System.Drawing.Point(12, 73);
            this.cbDeviceList.Name = "cbDeviceList";
            this.cbDeviceList.Size = new System.Drawing.Size(342, 21);
            this.cbDeviceList.TabIndex = 14;
            this.cbDeviceList.Text = "Нет устройств";
            // 
            // cbConnectionList
            // 
            this.cbConnectionList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbConnectionList.FormattingEnabled = true;
            this.cbConnectionList.Location = new System.Drawing.Point(12, 25);
            this.cbConnectionList.Name = "cbConnectionList";
            this.cbConnectionList.Size = new System.Drawing.Size(342, 21);
            this.cbConnectionList.TabIndex = 15;
            this.cbConnectionList.Text = "Список подключений пуст";
            this.cbConnectionList.SelectedIndexChanged += new System.EventHandler(this.cbConnectionList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 13;
            this.label1.Text = "Подключение:";
            // 
            // cbRegisterType
            // 
            this.cbRegisterType.FormattingEnabled = true;
            this.cbRegisterType.Items.AddRange(new object[] {
            "01 - Coil",
            "02 - Discrete Input",
            "03 - Holding Register",
            "04 - Input Register"});
            this.cbRegisterType.Location = new System.Drawing.Point(12, 121);
            this.cbRegisterType.Name = "cbRegisterType";
            this.cbRegisterType.Size = new System.Drawing.Size(163, 21);
            this.cbRegisterType.TabIndex = 23;
            this.cbRegisterType.Text = "03 - Holding Register";
            this.cbRegisterType.SelectedIndexChanged += new System.EventHandler(this.cbRegisterType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(188, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Адрес регистра:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Тип регистра:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 157);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "Значение:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(258, 233);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(96, 33);
            this.btnCancel.TabIndex = 30;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApply.Location = new System.Drawing.Point(12, 233);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(240, 33);
            this.btnApply.TabIndex = 29;
            this.btnApply.Text = "Записать";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // tbRegisterNum
            // 
            this.tbRegisterNum.Location = new System.Drawing.Point(191, 121);
            this.tbRegisterNum.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.tbRegisterNum.Name = "tbRegisterNum";
            this.tbRegisterNum.Size = new System.Drawing.Size(163, 20);
            this.tbRegisterNum.TabIndex = 32;
            this.tbRegisterNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnClearValue
            // 
            this.btnClearValue.Image = global::FreeModScan.Resource.erase;
            this.btnClearValue.Location = new System.Drawing.Point(322, 147);
            this.btnClearValue.Name = "btnClearValue";
            this.btnClearValue.Size = new System.Drawing.Size(32, 32);
            this.btnClearValue.TabIndex = 36;
            this.btnClearValue.UseVisualStyleBackColor = true;
            this.btnClearValue.Click += new System.EventHandler(this.btnClearValue_Click);
            // 
            // numVal
            // 
            this.numVal.Location = new System.Drawing.Point(77, 155);
            this.numVal.Maximum = new decimal(new int[] {
            32768,
            0,
            0,
            0});
            this.numVal.Minimum = new decimal(new int[] {
            32768,
            0,
            0,
            -2147483648});
            this.numVal.Name = "numVal";
            this.numVal.Size = new System.Drawing.Size(239, 20);
            this.numVal.TabIndex = 37;
            // 
            // WriteRegForm
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(366, 278);
            this.Controls.Add(this.numVal);
            this.Controls.Add(this.btnClearValue);
            this.Controls.Add(this.tbRegisterNum);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.cbRegisterType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbDeviceList);
            this.Controls.Add(this.cbConnectionList);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WriteRegForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Записать регистр";
            ((System.ComponentModel.ISupportInitialize)(this.tbRegisterNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbDeviceList;
        private System.Windows.Forms.ComboBox cbConnectionList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbRegisterType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.NumericUpDown tbRegisterNum;
        private System.Windows.Forms.Button btnClearValue;
        private System.Windows.Forms.NumericUpDown numVal;
    }
}