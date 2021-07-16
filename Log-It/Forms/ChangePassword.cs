using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log_It.Forms
{
    public partial class ChangePassword : Form
    {
        BAL.LogitInstance instance;
        DAL.User user;

        public ChangePassword(BAL.LogitInstance instance, DAL.User user)
        {
            InitializeComponent();
            this.instance = instance;
            this.user = user;
            labelUserid.Text = user.User_Name;
        }

        private bool CheckPassword(string password, string pattern)
        {
            string MatchEmailPattern = pattern;

            if (password != null) return Regex.IsMatch(password, MatchEmailPattern);
            else return false;
        }

        private void buttoncancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxoldpassword.Text != BAL.Authentication.GetDec(user.Password))
                {
                    MessageBox.Show("Old Password is not correct");
                    return;
                }
                if (!CheckPassword(textBoxnewpassword.Text, "(?=^[^\\s]{6,}$)(?=.*\\d)(?=.*[a-zA-Z])"))
                {
                    MessageBox.Show("Password must be six characters including letter and number");
                    return;
                }
                if (textBoxnewpassword.Text == BAL.Authentication.GetDec(user.Password))
                {
                    MessageBox.Show("New password should be different from old");
                    textBoxnewpassword.Clear();
                    textBoxconfirmpassword.Clear();
                    textBoxnewpassword.Focus();
                    return;
                }
                if (textBoxnewpassword.Text != textBoxconfirmpassword.Text)
                {
                    MessageBox.Show("Password not matched");
                    textBoxconfirmpassword.Clear();
                    textBoxconfirmpassword.Focus();
                    return;
                }

                instance.UserInstance.Password = BAL.Authentication.GetEc(textBoxnewpassword.Text);
                instance.DataLink.sp_Update_User(this.instance.UserInstance.Id, instance.UserInstance.User_Name, BAL.Authentication.GetEc(textBoxnewpassword.Text), instance.UserInstance.Role, instance.UserInstance.Full_Name, instance.UserInstance.Full_Name, instance.UserInstance.Authority, instance.UserInstance.Email,
                    instance.UserInstance.Phone, instance.UserInstance.Department, true, instance.UserInstance.Signature, DateTime.Now.AddDays((int)instance.UserInstance.Password_Expiry), instance.UserInstance.Password_Expiry, user.sms_notification, user.email_notification, instance.UserInstance.Title);
                int i = instance.RefresUsers;
                Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Change Properties ", instance.UserInstance.Full_Name);
                MessageBox.Show("Your password has been changed");
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            catch (Exception)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                //Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");
            }
        }
    }
}
