namespace Log_It.Forms
{
    partial class Calibrator
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
            this.textBoxoffset = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblPortNo = new System.Windows.Forms.Label();
            this.lblDeviceType = new System.Windows.Forms.Label();
            this.lblUpperLimit = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbllower = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.lblDatetime = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonsave = new System.Windows.Forms.Button();
            this.deviceEntityComboBox1 = new Log_It.CustomControls.DeviceEntityComboBox(this.components);
            this.lblChannelID = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblLocation = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxoffset
            // 
            this.textBoxoffset.Location = new System.Drawing.Point(146, 221);
            this.textBoxoffset.Name = "textBoxoffset";
            this.textBoxoffset.Size = new System.Drawing.Size(171, 20);
            this.textBoxoffset.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Serial No.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Port No.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Device Type";
            // 
            // lblPortNo
            // 
            this.lblPortNo.AutoSize = true;
            this.lblPortNo.Location = new System.Drawing.Point(146, 93);
            this.lblPortNo.Name = "lblPortNo";
            this.lblPortNo.Size = new System.Drawing.Size(0, 13);
            this.lblPortNo.TabIndex = 4;
            // 
            // lblDeviceType
            // 
            this.lblDeviceType.AutoSize = true;
            this.lblDeviceType.Location = new System.Drawing.Point(146, 119);
            this.lblDeviceType.Name = "lblDeviceType";
            this.lblDeviceType.Size = new System.Drawing.Size(0, 13);
            this.lblDeviceType.TabIndex = 8;
            // 
            // lblUpperLimit
            // 
            this.lblUpperLimit.AutoSize = true;
            this.lblUpperLimit.Location = new System.Drawing.Point(146, 143);
            this.lblUpperLimit.Name = "lblUpperLimit";
            this.lblUpperLimit.Size = new System.Drawing.Size(0, 13);
            this.lblUpperLimit.TabIndex = 9;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(21, 143);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 10;
            this.label8.Text = "Upper Limit:";
            // 
            // lbllower
            // 
            this.lbllower.AutoSize = true;
            this.lbllower.Location = new System.Drawing.Point(146, 169);
            this.lbllower.Name = "lbllower";
            this.lbllower.Size = new System.Drawing.Size(0, 13);
            this.lbllower.TabIndex = 11;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(21, 169);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "Lower Limit:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(21, 224);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "Offset:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(244, 260);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 14;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblDatetime
            // 
            this.lblDatetime.AutoSize = true;
            this.lblDatetime.Location = new System.Drawing.Point(146, 197);
            this.lblDatetime.Name = "lblDatetime";
            this.lblDatetime.Size = new System.Drawing.Size(0, 13);
            this.lblDatetime.TabIndex = 15;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(21, 197);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(120, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Last Date of Calibration:";
            // 
            // buttonsave
            // 
            this.buttonsave.Location = new System.Drawing.Point(163, 260);
            this.buttonsave.Name = "buttonsave";
            this.buttonsave.Size = new System.Drawing.Size(75, 23);
            this.buttonsave.TabIndex = 3;
            this.buttonsave.Text = "Save";
            this.buttonsave.UseVisualStyleBackColor = true;
            this.buttonsave.Click += new System.EventHandler(this.buttonsave_Click);
            // 
            // deviceEntityComboBox1
            // 
            this.deviceEntityComboBox1.FormattingEnabled = true;
            this.deviceEntityComboBox1.Location = new System.Drawing.Point(146, 12);
            this.deviceEntityComboBox1.Name = "deviceEntityComboBox1";
            this.deviceEntityComboBox1.Size = new System.Drawing.Size(173, 21);
            this.deviceEntityComboBox1.TabIndex = 17;
            this.deviceEntityComboBox1.SelectedIndexChanged += new System.EventHandler(this.deviceEntityComboBox1_SelectedIndexChanged);
            // 
            // lblChannelID
            // 
            this.lblChannelID.AutoSize = true;
            this.lblChannelID.Location = new System.Drawing.Point(146, 68);
            this.lblChannelID.Name = "lblChannelID";
            this.lblChannelID.Size = new System.Drawing.Size(0, 13);
            this.lblChannelID.TabIndex = 20;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Channel ID:";
            // 
            // lblLocation
            // 
            this.lblLocation.AutoSize = true;
            this.lblLocation.Location = new System.Drawing.Point(146, 42);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(0, 13);
            this.lblLocation.TabIndex = 18;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(21, 42);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 13);
            this.label12.TabIndex = 19;
            this.label12.Text = "Location:";
            // 
            // Calibrator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(338, 294);
            this.Controls.Add(this.lblChannelID);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblLocation);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.deviceEntityComboBox1);
            this.Controls.Add(this.lblDatetime);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lbllower);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.lblUpperLimit);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.lblDeviceType);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblPortNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonsave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxoffset);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Calibrator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Calibrator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxoffset;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonsave;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblPortNo;
        private System.Windows.Forms.Label lblDeviceType;
        private System.Windows.Forms.Label lblUpperLimit;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbllower;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lblDatetime;
        private System.Windows.Forms.Label label6;
        private CustomControls.DeviceEntityComboBox deviceEntityComboBox1;
        private System.Windows.Forms.Label lblChannelID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label label12;
    }
}