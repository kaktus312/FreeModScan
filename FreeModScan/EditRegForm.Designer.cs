namespace FreeModScan
{
    partial class EditRegForm
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
            this.cbRepresent = new System.Windows.Forms.ComboBox();
            this.cbDataType = new System.Windows.Forms.ComboBox();
            this.cbRegisterType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbA = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbB = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.cbRegState = new System.Windows.Forms.CheckBox();
            this.tbRegisterNum = new System.Windows.Forms.NumericUpDown();
            this.cbByteOrder = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cbUseMults = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.tbRegisterNum)).BeginInit();
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
            this.cbConnectionList.Size = new System.Drawing.Size(186, 21);
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
            // cbRepresent
            // 
            this.cbRepresent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRepresent.FormattingEnabled = true;
            this.cbRepresent.Items.AddRange(new object[] {
            "Dec",
            "Bin",
            "Hex"});
            this.cbRepresent.Location = new System.Drawing.Point(272, 113);
            this.cbRepresent.Name = "cbRepresent";
            this.cbRepresent.Size = new System.Drawing.Size(82, 21);
            this.cbRepresent.TabIndex = 24;
            this.cbRepresent.Text = "Dec";
            // 
            // cbDataType
            // 
            this.cbDataType.FormattingEnabled = true;
            this.cbDataType.Items.AddRange(new object[] {
            "Int",
            "Float",
            "swFloat",
            "Double",
            "swDouble"});
            this.cbDataType.Location = new System.Drawing.Point(154, 113);
            this.cbDataType.Name = "cbDataType";
            this.cbDataType.Size = new System.Drawing.Size(112, 21);
            this.cbDataType.TabIndex = 25;
            this.cbDataType.Text = "swDouble";
            // 
            // cbRegisterType
            // 
            this.cbRegisterType.FormattingEnabled = true;
            this.cbRegisterType.Items.AddRange(new object[] {
            "01 - Coil",
            "02 - Discrete Input",
            "03 - Holding Register",
            "04 - Input Register"});
            this.cbRegisterType.Location = new System.Drawing.Point(12, 113);
            this.cbRegisterType.Name = "cbRegisterType";
            this.cbRegisterType.Size = new System.Drawing.Size(139, 21);
            this.cbRegisterType.TabIndex = 23;
            this.cbRegisterType.Text = "03 - Holding Register";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 145);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 19;
            this.label4.Text = "Адрес регистра:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(269, 97);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 20;
            this.label6.Text = "Представление:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(151, 97);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Тип данных:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Тип регистра:";
            // 
            // tbA
            // 
            this.tbA.Location = new System.Drawing.Point(141, 197);
            this.tbA.Name = "tbA";
            this.tbA.Size = new System.Drawing.Size(79, 20);
            this.tbA.TabIndex = 28;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(122, 200);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 13);
            this.label7.TabIndex = 19;
            this.label7.Text = "А:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(229, 200);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "B:";
            // 
            // tbB
            // 
            this.tbB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbB.Location = new System.Drawing.Point(248, 197);
            this.tbB.Name = "tbB";
            this.tbB.Size = new System.Drawing.Size(79, 20);
            this.tbB.TabIndex = 28;
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
            this.btnApply.Text = "Применить";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // cbRegState
            // 
            this.cbRegState.AutoSize = true;
            this.cbRegState.Checked = true;
            this.cbRegState.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRegState.Location = new System.Drawing.Point(213, 27);
            this.cbRegState.Name = "cbRegState";
            this.cbRegState.Size = new System.Drawing.Size(89, 17);
            this.cbRegState.TabIndex = 31;
            this.cbRegState.Text = "Опрашивать";
            this.cbRegState.UseVisualStyleBackColor = true;
            // 
            // tbRegisterNum
            // 
            this.tbRegisterNum.Location = new System.Drawing.Point(12, 162);
            this.tbRegisterNum.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.tbRegisterNum.Name = "tbRegisterNum";
            this.tbRegisterNum.Size = new System.Drawing.Size(207, 20);
            this.tbRegisterNum.TabIndex = 32;
            this.tbRegisterNum.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbByteOrder
            // 
            this.cbByteOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbByteOrder.FormattingEnabled = true;
            this.cbByteOrder.Items.AddRange(new object[] {
            "Big-Endian",
            " Little-endian"});
            this.cbByteOrder.Location = new System.Drawing.Point(225, 161);
            this.cbByteOrder.Name = "cbByteOrder";
            this.cbByteOrder.Size = new System.Drawing.Size(129, 21);
            this.cbByteOrder.TabIndex = 34;
            this.cbByteOrder.Text = "Big-Endian";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(222, 145);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(92, 13);
            this.label10.TabIndex = 33;
            this.label10.Text = "Порядок байтов:";
            // 
            // cbUseMults
            // 
            this.cbUseMults.AutoSize = true;
            this.cbUseMults.Location = new System.Drawing.Point(12, 199);
            this.cbUseMults.Name = "cbUseMults";
            this.cbUseMults.Size = new System.Drawing.Size(107, 17);
            this.cbUseMults.TabIndex = 35;
            this.cbUseMults.Text = "Коэффициенты:";
            this.cbUseMults.UseVisualStyleBackColor = true;
            this.cbUseMults.CheckedChanged += new System.EventHandler(this.cbUseMults_CheckedChanged);
            // 
            // EditRegForm
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(366, 278);
            this.Controls.Add(this.cbUseMults);
            this.Controls.Add(this.cbByteOrder);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tbRegisterNum);
            this.Controls.Add(this.cbRegState);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.tbB);
            this.Controls.Add(this.tbA);
            this.Controls.Add(this.cbRepresent);
            this.Controls.Add(this.cbDataType);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbRegisterType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbDeviceList);
            this.Controls.Add(this.cbConnectionList);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "EditRegForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Изменить регистр";
            ((System.ComponentModel.ISupportInitialize)(this.tbRegisterNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbDeviceList;
        private System.Windows.Forms.ComboBox cbConnectionList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbRepresent;
        private System.Windows.Forms.ComboBox cbDataType;
        private System.Windows.Forms.ComboBox cbRegisterType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbA;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbB;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.CheckBox cbRegState;
        private System.Windows.Forms.NumericUpDown tbRegisterNum;
        private System.Windows.Forms.ComboBox cbByteOrder;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox cbUseMults;
    }
}