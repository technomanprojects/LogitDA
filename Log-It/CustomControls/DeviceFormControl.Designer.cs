namespace Log_It.CustomControls
{
    partial class DeviceFormControl
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label1lastRecord = new System.Windows.Forms.Label();
            this.comboBoxType = new System.Windows.Forms.ComboBox();
            this.checkBoxAlaram = new System.Windows.Forms.CheckBox();
            this.groupBoxTemp = new System.Windows.Forms.GroupBox();
            this.textBoxTUR = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.textBoxTLR = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxTUL = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxTLL = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.numericInterval = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxInstrument = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxLocation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxChannelID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxDeviceID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBoxTemp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label1lastRecord);
            this.groupBox1.Controls.Add(this.comboBoxType);
            this.groupBox1.Controls.Add(this.checkBoxAlaram);
            this.groupBox1.Controls.Add(this.groupBoxTemp);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.numericInterval);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.textBoxInstrument);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.textBoxLocation);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textBoxChannelID);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.textBoxDeviceID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(9, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(438, 297);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Detail";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 56);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Device Type:";
            // 
            // label1lastRecord
            // 
            this.label1lastRecord.AutoSize = true;
            this.label1lastRecord.Location = new System.Drawing.Point(6, 251);
            this.label1lastRecord.Name = "label1lastRecord";
            this.label1lastRecord.Size = new System.Drawing.Size(41, 13);
            this.label1lastRecord.TabIndex = 35;
            this.label1lastRecord.Text = "label16";
            this.label1lastRecord.Visible = false;
            // 
            // comboBoxType
            // 
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "Temp",
            "Rh",
            "Pressure"});
            this.comboBoxType.Location = new System.Drawing.Point(99, 53);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(324, 21);
            this.comboBoxType.TabIndex = 1;
            this.comboBoxType.Text = "Temp";
            // 
            // checkBoxAlaram
            // 
            this.checkBoxAlaram.AutoSize = true;
            this.checkBoxAlaram.Location = new System.Drawing.Point(339, 166);
            this.checkBoxAlaram.Name = "checkBoxAlaram";
            this.checkBoxAlaram.Size = new System.Drawing.Size(58, 17);
            this.checkBoxAlaram.TabIndex = 6;
            this.checkBoxAlaram.Text = "Alaram";
            this.checkBoxAlaram.UseVisualStyleBackColor = true;
            // 
            // groupBoxTemp
            // 
            this.groupBoxTemp.Controls.Add(this.textBoxTUR);
            this.groupBoxTemp.Controls.Add(this.label10);
            this.groupBoxTemp.Controls.Add(this.textBoxTLR);
            this.groupBoxTemp.Controls.Add(this.label9);
            this.groupBoxTemp.Controls.Add(this.textBoxTUL);
            this.groupBoxTemp.Controls.Add(this.label8);
            this.groupBoxTemp.Controls.Add(this.textBoxTLL);
            this.groupBoxTemp.Controls.Add(this.label7);
            this.groupBoxTemp.Location = new System.Drawing.Point(74, 199);
            this.groupBoxTemp.Name = "groupBoxTemp";
            this.groupBoxTemp.Size = new System.Drawing.Size(349, 85);
            this.groupBoxTemp.TabIndex = 31;
            this.groupBoxTemp.TabStop = false;
            this.groupBoxTemp.Text = "Limits";
            // 
            // textBoxTUR
            // 
            this.textBoxTUR.Location = new System.Drawing.Point(265, 52);
            this.textBoxTUR.Name = "textBoxTUR";
            this.textBoxTUR.Size = new System.Drawing.Size(56, 20);
            this.textBoxTUR.TabIndex = 3;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(185, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 13);
            this.label10.TabIndex = 21;
            this.label10.Text = "Upper Range:";
            // 
            // textBoxTLR
            // 
            this.textBoxTLR.Location = new System.Drawing.Point(86, 52);
            this.textBoxTLR.Name = "textBoxTLR";
            this.textBoxTLR.Size = new System.Drawing.Size(56, 20);
            this.textBoxTLR.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 55);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(74, 13);
            this.label9.TabIndex = 19;
            this.label9.Text = "Lower Range:";
            // 
            // textBoxTUL
            // 
            this.textBoxTUL.Location = new System.Drawing.Point(265, 26);
            this.textBoxTUL.Name = "textBoxTUL";
            this.textBoxTUL.Size = new System.Drawing.Size(56, 20);
            this.textBoxTUL.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(185, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 17;
            this.label8.Text = "Upper Limit:";
            // 
            // textBoxTLL
            // 
            this.textBoxTLL.Location = new System.Drawing.Point(86, 26);
            this.textBoxTLL.Name = "textBoxTLL";
            this.textBoxTLL.Size = new System.Drawing.Size(56, 20);
            this.textBoxTLL.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Lower Limit:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(222, 166);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "Minute";
            // 
            // numericInterval
            // 
            this.numericInterval.Location = new System.Drawing.Point(99, 164);
            this.numericInterval.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numericInterval.Name = "numericInterval";
            this.numericInterval.Size = new System.Drawing.Size(117, 20);
            this.numericInterval.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 166);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Interval:";
            // 
            // textBoxInstrument
            // 
            this.textBoxInstrument.Location = new System.Drawing.Point(99, 135);
            this.textBoxInstrument.Name = "textBoxInstrument";
            this.textBoxInstrument.Size = new System.Drawing.Size(324, 20);
            this.textBoxInstrument.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Serial No:";
            // 
            // textBoxLocation
            // 
            this.textBoxLocation.Location = new System.Drawing.Point(99, 108);
            this.textBoxLocation.Name = "textBoxLocation";
            this.textBoxLocation.Size = new System.Drawing.Size(324, 20);
            this.textBoxLocation.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Location:";
            // 
            // textBoxChannelID
            // 
            this.textBoxChannelID.Location = new System.Drawing.Point(99, 81);
            this.textBoxChannelID.Name = "textBoxChannelID";
            this.textBoxChannelID.Size = new System.Drawing.Size(324, 20);
            this.textBoxChannelID.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Port ID:";
            // 
            // textBoxDeviceID
            // 
            this.textBoxDeviceID.Location = new System.Drawing.Point(99, 27);
            this.textBoxDeviceID.Name = "textBoxDeviceID";
            this.textBoxDeviceID.Size = new System.Drawing.Size(324, 20);
            this.textBoxDeviceID.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Device ID:";
            // 
            // DeviceFormControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "DeviceFormControl";
            this.Size = new System.Drawing.Size(460, 321);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxTemp.ResumeLayout(false);
            this.groupBoxTemp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1lastRecord;
        private System.Windows.Forms.GroupBox groupBoxTemp;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.CheckBox checkBoxAlaram;
        public System.Windows.Forms.TextBox textBoxTUR;
        public System.Windows.Forms.TextBox textBoxTLR;
        public System.Windows.Forms.TextBox textBoxTUL;
        public System.Windows.Forms.TextBox textBoxTLL;
        public System.Windows.Forms.NumericUpDown numericInterval;
        public System.Windows.Forms.TextBox textBoxInstrument;
        public System.Windows.Forms.TextBox textBoxLocation;
        public System.Windows.Forms.TextBox textBoxChannelID;
        public System.Windows.Forms.TextBox textBoxDeviceID;
        public System.Windows.Forms.ComboBox comboBoxType;

    }
}
