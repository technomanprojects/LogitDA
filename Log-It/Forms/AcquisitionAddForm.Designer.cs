namespace Log_It.Forms
{
    partial class AcquisitionAddForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AcquisitionAddForm));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxPort = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxDeviceUL = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.textBoxdeviceLL = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
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
            this.label2 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.textBoxChannelID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.deptComboBox1 = new Log_It.CustomControls.DeptComboBox(this.components);
            this.comboBoxNetwork = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxTemp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(360, 444);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(279, 444);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Ok";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.comboBoxNetwork);
            this.groupBox1.Controls.Add(this.deptComboBox1);
            this.groupBox1.Controls.Add(this.comboBoxPort);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label11);
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
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.textBoxChannelID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(438, 426);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Detail";
            // 
            // comboBoxPort
            // 
            this.comboBoxPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPort.FormattingEnabled = true;
            this.comboBoxPort.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15"});
            this.comboBoxPort.Location = new System.Drawing.Point(99, 125);
            this.comboBoxPort.Name = "comboBoxPort";
            this.comboBoxPort.Size = new System.Drawing.Size(324, 21);
            this.comboBoxPort.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBoxDeviceUL);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.textBoxdeviceLL);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Location = new System.Drawing.Point(74, 346);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(349, 61);
            this.groupBox2.TabIndex = 32;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Device Limits";
            // 
            // textBoxDeviceUL
            // 
            this.textBoxDeviceUL.Location = new System.Drawing.Point(265, 26);
            this.textBoxDeviceUL.Name = "textBoxDeviceUL";
            this.textBoxDeviceUL.Size = new System.Drawing.Size(56, 20);
            this.textBoxDeviceUL.TabIndex = 2;
            this.textBoxDeviceUL.Text = "100";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(185, 29);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(36, 13);
            this.label14.TabIndex = 17;
            this.label14.Text = "Upper";
            // 
            // textBoxdeviceLL
            // 
            this.textBoxdeviceLL.Location = new System.Drawing.Point(86, 26);
            this.textBoxdeviceLL.Name = "textBoxdeviceLL";
            this.textBoxdeviceLL.Size = new System.Drawing.Size(56, 20);
            this.textBoxdeviceLL.TabIndex = 0;
            this.textBoxdeviceLL.Text = "0";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 29);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(36, 13);
            this.label15.TabIndex = 15;
            this.label15.Text = "Lower";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 101);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(71, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Device Type:";
            // 
            // comboBoxType
            // 
            this.comboBoxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxType.FormattingEnabled = true;
            this.comboBoxType.Items.AddRange(new object[] {
            "Temp",
            "Rh",
            "Pressure"});
            this.comboBoxType.Location = new System.Drawing.Point(99, 98);
            this.comboBoxType.Name = "comboBoxType";
            this.comboBoxType.Size = new System.Drawing.Size(324, 21);
            this.comboBoxType.TabIndex = 1;
            // 
            // checkBoxAlaram
            // 
            this.checkBoxAlaram.AutoSize = true;
            this.checkBoxAlaram.Location = new System.Drawing.Point(339, 211);
            this.checkBoxAlaram.Name = "checkBoxAlaram";
            this.checkBoxAlaram.Size = new System.Drawing.Size(52, 17);
            this.checkBoxAlaram.TabIndex = 6;
            this.checkBoxAlaram.Text = "Alarm";
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
            this.groupBoxTemp.Location = new System.Drawing.Point(74, 244);
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
            this.textBoxTUR.Text = "100";
            this.textBoxTUR.TextChanged += new System.EventHandler(this.textBoxTLL_TextChanged);
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
            this.textBoxTLR.Text = "0";
            this.textBoxTLR.TextChanged += new System.EventHandler(this.textBoxTLL_TextChanged);
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
            this.textBoxTUL.Text = "100";
            this.textBoxTUL.TextChanged += new System.EventHandler(this.textBoxTLL_TextChanged);
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
            this.textBoxTLL.Text = "0";
            this.textBoxTLL.TextChanged += new System.EventHandler(this.textBoxTLL_TextChanged);
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
            this.label6.Location = new System.Drawing.Point(222, 213);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(39, 13);
            this.label6.TabIndex = 30;
            this.label6.Text = "Minute";
            // 
            // numericInterval
            // 
            this.numericInterval.Location = new System.Drawing.Point(99, 209);
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
            this.label5.Location = new System.Drawing.Point(12, 213);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 28;
            this.label5.Text = "Interval:";
            // 
            // textBoxInstrument
            // 
            this.textBoxInstrument.Location = new System.Drawing.Point(99, 180);
            this.textBoxInstrument.Name = "textBoxInstrument";
            this.textBoxInstrument.Size = new System.Drawing.Size(324, 20);
            this.textBoxInstrument.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 183);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 26;
            this.label4.Text = "Serial No:";
            // 
            // textBoxLocation
            // 
            this.textBoxLocation.Location = new System.Drawing.Point(99, 153);
            this.textBoxLocation.Name = "textBoxLocation";
            this.textBoxLocation.Size = new System.Drawing.Size(324, 20);
            this.textBoxLocation.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 24;
            this.label3.Text = "Location:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 129);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 22;
            this.label2.Text = "Port ID:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 19);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(72, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "Network Port:";
            // 
            // textBoxChannelID
            // 
            this.textBoxChannelID.Location = new System.Drawing.Point(99, 72);
            this.textBoxChannelID.Name = "textBoxChannelID";
            this.textBoxChannelID.Size = new System.Drawing.Size(324, 20);
            this.textBoxChannelID.TabIndex = 0;
            this.textBoxChannelID.Text = "1";
            this.textBoxChannelID.TextChanged += new System.EventHandler(this.textBoxTLL_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Channel ID:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(11, 42);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 13);
            this.label13.TabIndex = 20;
            this.label13.Text = "Department:";
            // 
            // deptComboBox1
            // 
            this.deptComboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.deptComboBox1.FormattingEnabled = true;
            this.deptComboBox1.Location = new System.Drawing.Point(99, 42);
            this.deptComboBox1.Name = "deptComboBox1";
            this.deptComboBox1.SelectedEntity = null;
            this.deptComboBox1.Size = new System.Drawing.Size(324, 21);
            this.deptComboBox1.TabIndex = 33;
            // 
            // comboBoxNetwork
            // 
            this.comboBoxNetwork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxNetwork.FormattingEnabled = true;
            this.comboBoxNetwork.Location = new System.Drawing.Point(99, 16);
            this.comboBoxNetwork.Name = "comboBoxNetwork";
            this.comboBoxNetwork.Size = new System.Drawing.Size(324, 21);
            this.comboBoxNetwork.TabIndex = 34;
            // 
            // AcquisitionAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 473);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AcquisitionAddForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Setup ";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBoxTemp.ResumeLayout(false);
            this.groupBoxTemp.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.ComboBox comboBoxType;
        public System.Windows.Forms.CheckBox checkBoxAlaram;
        private System.Windows.Forms.GroupBox groupBoxTemp;
        public System.Windows.Forms.TextBox textBoxTUR;
        private System.Windows.Forms.Label label10;
        public System.Windows.Forms.TextBox textBoxTLR;
        private System.Windows.Forms.Label label9;
        public System.Windows.Forms.TextBox textBoxTUL;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.TextBox textBoxTLL;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.NumericUpDown numericInterval;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox textBoxInstrument;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox textBoxLocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox textBoxChannelID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.TextBox textBoxDeviceUL;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.TextBox textBoxdeviceLL;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.ComboBox comboBoxPort;
        private CustomControls.DeptComboBox deptComboBox1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox comboBoxNetwork;
    }
}