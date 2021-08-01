namespace Log_It.Forms
{
    partial class ChangePassword
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
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttoncancel = new System.Windows.Forms.Button();
            this.textBoxconfirmpassword = new System.Windows.Forms.TextBox();
            this.textBoxnewpassword = new System.Windows.Forms.TextBox();
            this.textBoxoldpassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelUserid = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonOk
            // 
            this.buttonOk.Location = new System.Drawing.Point(174, 155);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(75, 23);
            this.buttonOk.TabIndex = 12;
            this.buttonOk.Text = "Ok";
            this.buttonOk.UseVisualStyleBackColor = true;
            this.buttonOk.Click += new System.EventHandler(this.ButtonOk_Click);
            // 
            // buttoncancel
            // 
            this.buttoncancel.Location = new System.Drawing.Point(255, 156);
            this.buttoncancel.Name = "buttoncancel";
            this.buttoncancel.Size = new System.Drawing.Size(75, 23);
            this.buttoncancel.TabIndex = 14;
            this.buttoncancel.Text = "Cancel";
            this.buttoncancel.UseVisualStyleBackColor = true;
            this.buttoncancel.Click += new System.EventHandler(this.Buttoncancel_Click);
            // 
            // textBoxconfirmpassword
            // 
            this.textBoxconfirmpassword.Location = new System.Drawing.Point(122, 110);
            this.textBoxconfirmpassword.Name = "textBoxconfirmpassword";
            this.textBoxconfirmpassword.PasswordChar = '•';
            this.textBoxconfirmpassword.Size = new System.Drawing.Size(208, 20);
            this.textBoxconfirmpassword.TabIndex = 10;
            // 
            // textBoxnewpassword
            // 
            this.textBoxnewpassword.Location = new System.Drawing.Point(122, 78);
            this.textBoxnewpassword.Name = "textBoxnewpassword";
            this.textBoxnewpassword.PasswordChar = '•';
            this.textBoxnewpassword.Size = new System.Drawing.Size(208, 20);
            this.textBoxnewpassword.TabIndex = 8;
            // 
            // textBoxoldpassword
            // 
            this.textBoxoldpassword.Location = new System.Drawing.Point(122, 46);
            this.textBoxoldpassword.Name = "textBoxoldpassword";
            this.textBoxoldpassword.PasswordChar = '•';
            this.textBoxoldpassword.Size = new System.Drawing.Size(208, 20);
            this.textBoxoldpassword.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Confirm Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "New Password:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Old Password:";
            // 
            // labelUserid
            // 
            this.labelUserid.AutoSize = true;
            this.labelUserid.Location = new System.Drawing.Point(119, 18);
            this.labelUserid.Name = "labelUserid";
            this.labelUserid.Size = new System.Drawing.Size(0, 13);
            this.labelUserid.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "User ID:";
            // 
            // ChangePassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 204);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttoncancel);
            this.Controls.Add(this.textBoxconfirmpassword);
            this.Controls.Add(this.textBoxnewpassword);
            this.Controls.Add(this.textBoxoldpassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelUserid);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ChangePassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Change Password";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttoncancel;
        private System.Windows.Forms.TextBox textBoxconfirmpassword;
        private System.Windows.Forms.TextBox textBoxnewpassword;
        private System.Windows.Forms.TextBox textBoxoldpassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelUserid;
        private System.Windows.Forms.Label label1;
    }
}