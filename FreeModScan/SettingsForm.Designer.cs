namespace FreeModScan
{
    partial class SettingsForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.tbOptionWindowSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tboptionDelay = new System.Windows.Forms.TextBox();
            this.chbFastRecord = new System.Windows.Forms.CheckBox();
            this.btnOptionsReset = new System.Windows.Forms.Button();
            this.btnOptionsCancel = new System.Windows.Forms.Button();
            this.btnOptionsSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(212, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Размер окна для чтения Holdings/Inputs:";
            // 
            // tbOptionWindowSize
            // 
            this.tbOptionWindowSize.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOptionWindowSize.Location = new System.Drawing.Point(225, 29);
            this.tbOptionWindowSize.Name = "tbOptionWindowSize";
            this.tbOptionWindowSize.Size = new System.Drawing.Size(135, 20);
            this.tbOptionWindowSize.TabIndex = 1;
            this.tbOptionWindowSize.Text = "125";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(217, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Длительность паузы в линии в символах";
            // 
            // tboptionDelay
            // 
            this.tboptionDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tboptionDelay.Location = new System.Drawing.Point(225, 55);
            this.tboptionDelay.Name = "tboptionDelay";
            this.tboptionDelay.Size = new System.Drawing.Size(135, 20);
            this.tboptionDelay.TabIndex = 1;
            this.tboptionDelay.Text = "3.5";
            // 
            // chbFastRecord
            // 
            this.chbFastRecord.AutoSize = true;
            this.chbFastRecord.Location = new System.Drawing.Point(10, 95);
            this.chbFastRecord.Name = "chbFastRecord";
            this.chbFastRecord.Size = new System.Drawing.Size(169, 17);
            this.chbFastRecord.TabIndex = 2;
            this.chbFastRecord.Text = "Быстрая запись в регистры";
            this.chbFastRecord.UseVisualStyleBackColor = true;
            // 
            // btnOptionsReset
            // 
            this.btnOptionsReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOptionsReset.Location = new System.Drawing.Point(10, 135);
            this.btnOptionsReset.Name = "btnOptionsReset";
            this.btnOptionsReset.Size = new System.Drawing.Size(75, 23);
            this.btnOptionsReset.TabIndex = 3;
            this.btnOptionsReset.Text = "Сбросить";
            this.btnOptionsReset.UseVisualStyleBackColor = true;
            this.btnOptionsReset.Click += new System.EventHandler(this.btnOptionsReset_Click);
            // 
            // btnOptionsCancel
            // 
            this.btnOptionsCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOptionsCancel.Location = new System.Drawing.Point(285, 135);
            this.btnOptionsCancel.Name = "btnOptionsCancel";
            this.btnOptionsCancel.Size = new System.Drawing.Size(75, 23);
            this.btnOptionsCancel.TabIndex = 3;
            this.btnOptionsCancel.Text = "Отмена";
            this.btnOptionsCancel.UseVisualStyleBackColor = true;
            this.btnOptionsCancel.Click += new System.EventHandler(this.btnOptionsCancel_Click);
            // 
            // btnOptionsSave
            // 
            this.btnOptionsSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOptionsSave.Location = new System.Drawing.Point(204, 135);
            this.btnOptionsSave.Name = "btnOptionsSave";
            this.btnOptionsSave.Size = new System.Drawing.Size(75, 23);
            this.btnOptionsSave.TabIndex = 3;
            this.btnOptionsSave.Text = "Сохранить";
            this.btnOptionsSave.UseVisualStyleBackColor = true;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 170);
            this.Controls.Add(this.btnOptionsSave);
            this.Controls.Add(this.btnOptionsCancel);
            this.Controls.Add(this.btnOptionsReset);
            this.Controls.Add(this.chbFastRecord);
            this.Controls.Add(this.tboptionDelay);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbOptionWindowSize);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "SettingsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Настройки для текущего сеанса";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbOptionWindowSize;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tboptionDelay;
        private System.Windows.Forms.CheckBox chbFastRecord;
        private System.Windows.Forms.Button btnOptionsReset;
        private System.Windows.Forms.Button btnOptionsCancel;
        private System.Windows.Forms.Button btnOptionsSave;
    }
}