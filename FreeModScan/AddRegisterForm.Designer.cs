namespace FreeModScan
{
    partial class AddRegisterForm
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
            this.btnAdd = new System.Windows.Forms.Button();
            this.tbRegisterList = new System.Windows.Forms.TextBox();
            this.cbDataType = new System.Windows.Forms.ComboBox();
            this.cbRegisterType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbConnectionList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbDeviceList = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.cbRepresent = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbByteOrder = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnAdd.Location = new System.Drawing.Point(12, 243);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(170, 33);
            this.btnAdd.TabIndex = 19;
            this.btnAdd.Text = "Добавить";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // tbRegisterList
            // 
            this.tbRegisterList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tbRegisterList.Location = new System.Drawing.Point(15, 206);
            this.tbRegisterList.Name = "tbRegisterList";
            this.tbRegisterList.Size = new System.Drawing.Size(271, 20);
            this.tbRegisterList.TabIndex = 18;
            this.tbRegisterList.Text = "Список адресов регистров: 1, 2-6, 7, 8-10";
            // 
            // cbDataType
            // 
            this.cbDataType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDataType.FormattingEnabled = true;
            this.cbDataType.Items.AddRange(new object[] {
            "Int",
            "Float",
            "swFloat",
            "Double",
            "swDouble"});
            this.cbDataType.Location = new System.Drawing.Point(157, 121);
            this.cbDataType.Name = "cbDataType";
            this.cbDataType.Size = new System.Drawing.Size(129, 21);
            this.cbDataType.TabIndex = 17;
            this.cbDataType.Text = "swDouble";
            // 
            // cbRegisterType
            // 
            this.cbRegisterType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRegisterType.FormattingEnabled = true;
            this.cbRegisterType.Items.AddRange(new object[] {
            "01 - Coil",
            "02 - Discrete Input",
            "03 - Holding Register",
            "04 - Input Register"});
            this.cbRegisterType.Location = new System.Drawing.Point(12, 121);
            this.cbRegisterType.Name = "cbRegisterType";
            this.cbRegisterType.Size = new System.Drawing.Size(129, 21);
            this.cbRegisterType.TabIndex = 16;
            this.cbRegisterType.Text = "03 - Holding Register";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Тип регистра:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Имя устройства: ";
            // 
            // cbConnectionList
            // 
            this.cbConnectionList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbConnectionList.FormattingEnabled = true;
            this.cbConnectionList.Location = new System.Drawing.Point(12, 25);
            this.cbConnectionList.Name = "cbConnectionList";
            this.cbConnectionList.Size = new System.Drawing.Size(276, 21);
            this.cbConnectionList.TabIndex = 11;
            this.cbConnectionList.Text = "Список подключений пуст";
            this.cbConnectionList.SelectedIndexChanged += new System.EventHandler(this.cbConnectionList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Подключение:";
            // 
            // cbDeviceList
            // 
            this.cbDeviceList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDeviceList.FormattingEnabled = true;
            this.cbDeviceList.Location = new System.Drawing.Point(12, 73);
            this.cbDeviceList.Name = "cbDeviceList";
            this.cbDeviceList.Size = new System.Drawing.Size(276, 21);
            this.cbDeviceList.TabIndex = 11;
            this.cbDeviceList.Text = "Нет устройств";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 190);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(102, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Адреса регистров:";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(188, 243);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(98, 33);
            this.btnCancel.TabIndex = 20;
            this.btnCancel.Text = "Отмена";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(157, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Тип данных:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 149);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Представление:";
            // 
            // cbRepresent
            // 
            this.cbRepresent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbRepresent.FormattingEnabled = true;
            this.cbRepresent.Items.AddRange(new object[] {
            "Dec",
            "Bin",
            "Hex"});
            this.cbRepresent.Location = new System.Drawing.Point(15, 165);
            this.cbRepresent.Name = "cbRepresent";
            this.cbRepresent.Size = new System.Drawing.Size(129, 21);
            this.cbRepresent.TabIndex = 17;
            this.cbRepresent.Text = "Dec";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(157, 149);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Порядок байтов:";
            // 
            // cbByteOrder
            // 
            this.cbByteOrder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbByteOrder.FormattingEnabled = true;
            this.cbByteOrder.Items.AddRange(new object[] {
            "Big-Endian",
            " Little-endian"});
            this.cbByteOrder.Location = new System.Drawing.Point(157, 165);
            this.cbByteOrder.Name = "cbByteOrder";
            this.cbByteOrder.Size = new System.Drawing.Size(129, 21);
            this.cbByteOrder.TabIndex = 17;
            this.cbByteOrder.Text = "Big-Endian";
            // 
            // AddRegisterForm
            // 
            this.AcceptButton = this.btnAdd;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(300, 288);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.tbRegisterList);
            this.Controls.Add(this.cbByteOrder);
            this.Controls.Add(this.cbRepresent);
            this.Controls.Add(this.cbDataType);
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
            this.Name = "AddRegisterForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Добавить регистры";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox tbRegisterList;
        private System.Windows.Forms.ComboBox cbDataType;
        private System.Windows.Forms.ComboBox cbRegisterType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbConnectionList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDeviceList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbRepresent;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbByteOrder;
    }
}