using BAL;
using DAL;
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
    public partial class UserForm : Form
    {
        private readonly bool isNew;
        private readonly int userid;
        private readonly User user;
        private readonly LogitInstance instace;
        private readonly User tempuser = null;

        public UserForm(int id, LogitInstance instace, bool isNew)
        {
            this.isNew = isNew;
            this.instace = instace;
            InitializeComponent();
            try
            {
                List<Department> dlist = instace.DataLink.Departments.ToList();

                foreach (var item in dlist)
                {
                    deptComboBox1.Items.Add(item);
                }

                List<Role> rlist = instace.DataLink.Roles.Skip(1).ToList();
                foreach (var item in rlist)
                {
                    roleComboBox1.Items.Add(item);
                }
                if (id > 0)
                {
                    this.userid = id;
                    user = instace.Users.SingleOrDefault(x => x.Id == id);

                }
                if (user != null)
                {
                    checkBoxActive.Visible = true;
                    this.Clone(user, out tempuser);
                    textBoxUserName.Text = user.User_Name.ToLower();
                    textBoxUserName.Enabled = false;
                    textBoxFullName.Text = user.Full_Name;
                    deptComboBox1.Text = user.Department.Department_Name;
                    textBoxPassword.Text = BAL.Authentication.GetDec(user.Password);
                    textBoxSignature.Enabled = false;
                    textBoxSignature.Text = user.Signature;
                    deptComboBox1.Text = user.Authority;
                    textBoxTitle.Text = user.Title;
                    txtPhone.Text = user.Phone;
                    checkBoxEmail.Checked = (bool)user.email_notification;

                    textBoxEmail.Enabled = (bool)user.email_notification;
                    checkBoxSMS.Checked = (bool)user.sms_notification;
                    txtPhone.Enabled = (bool)user.sms_notification;
                    if (user.Email != null)
                    {
                        textBoxEmail.Text = user.Email;
                    }
                    if (user.Phone != null)
                    {
                        txtPhone.Text = user.Phone;
                    }
                    if (!isNew)
                    {
                        textBoxUserName.Enabled = false;
                    }
                    textBoxexpires.Text = user.Password_Expiry.ToString();
                    roleComboBox1.Text = user.Role1.RoleName;
                    checkBoxActive.Checked = (bool)user.Active;
                }
                else
                {
                    textBoxEmail.Enabled = checkBoxEmail.Checked;
                    txtPhone.Enabled = checkBoxSMS.Checked;
                }
            }
            catch (Exception)
            {

                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                // Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");

            }
        }

        private void Clone(User user, out User tempuser)
        {
            tempuser = new User();
            tempuser.Active = user.Active;
            tempuser.Authority = user.Authority;
            tempuser.CreateDateTime = user.CreateDateTime;
            tempuser.CreatedBy = user.CreatedBy;
            tempuser.Department = user.Department;
            tempuser.Full_Name = user.Full_Name;
            tempuser.Id = user.Id;
            tempuser.ModefiedBy = user.ModefiedBy;
            tempuser.ModifiedDateTime = user.ModifiedDateTime;
            tempuser.Department = user.Department;
            tempuser.Password = user.Password;
            tempuser.Role1 = user.Role1;
            tempuser.User_Name = user.User_Name;
            tempuser.Email = user.Email;
            tempuser.Phone = user.Phone;
            tempuser.sms_notification = user.sms_notification;
            tempuser.Title = user.Title;
            tempuser.Password_Expiry = user.Password_Expiry;
            tempuser.email_notification = user.email_notification;

        }

        private bool Validation(bool isnew)
        {
            try
            {
                if (textBoxFullName.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter full name");
                    return false;
                }
                if (textBoxUserName.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter user name");
                    return false;
                }
                if (instace.Users.SingleOrDefault(x => x.User_Name == textBoxUserName.Text) != null && isnew)
                {
                    MessageBox.Show("User aready exists, please enter different user name.");
                    return false;
                }
                if (textBoxPassword.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter password");
                    return false;

                }
                if (!CheckPassword(textBoxPassword.Text, "(?=^[^\\s]{6,}$)(?=.*\\d)(?=.*[a-zA-Z])"))
                {
                    MessageBox.Show("Password must be six characters including letter and number");
                    return false;
                }

                if (textBoxEmail.Text != string.Empty)
                {
                    if (!CheckPassword(textBoxEmail.Text, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$"))
                    {
                        MessageBox.Show("Email Address not valid");
                        return false;
                    }
                }
                if (roleComboBox1.SelectedEntity == null)
                {
                    MessageBox.Show("Please assign user Role");
                        return false;
                }

                if (deptComboBox1.SelectedEntity == null)
                {
                    MessageBox.Show("Please select Department");
                        return false;
                }

            }
            catch (Exception m)
            {

                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");

            }
            return true;

        }

        private bool CheckPassword(string password, string pattern)
        {
            string MatchEmailPattern = pattern;

            if (password != null) return Regex.IsMatch(password, MatchEmailPattern);
            else return false;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (isNew )
                {
                    if (Validation(true))
                    {
                        User user = new User();
                        user.Active = true;
                        user.Authority = roleComboBox1.SelectedEntity.RoleName;
                        user.CreateDateTime = DateTime.Now;
                        user.Department = deptComboBox1.SelectedEntity;
                        user.Full_Name = textBoxFullName.Text;
                        user.IsRowEnable = true;
                        user.Password = BAL.Authentication.GetEc(textBoxPassword.Text);
                        user.User_Name = textBoxUserName.Text;
                        user.CreatedBy = instace.UserInstance.Full_Name;
                        user.Department = deptComboBox1.SelectedEntity;
                        user.Role1 = roleComboBox1.SelectedEntity;
                        user.Email = textBoxEmail.Text;
                        user.Phone = txtPhone.Text;

                        DateTime dt = DateTime.Now.AddDays(Convert.ToInt32(textBoxexpires.Text));
                        instace.DataLink.Insert_User( textBoxUserName.Text.ToLower(), BAL.Authentication.GetEc(textBoxPassword.Text), user.Role, instace.UserInstance.Full_Name, textBoxFullName.Text, user.Authority, textBoxEmail.Text,
                            txtPhone.Text,deptComboBox1.SelectedEntity.Department_Id, textBoxSignature.Text, dt, Convert.ToInt16(textBoxexpires.Text), checkBoxSMS.Checked, checkBoxEmail.Checked, textBoxTitle.Text);
                        int a = instace.RefresUsers;
                        this.DialogResult = DialogResult.OK;
                        Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Add new user ", instace.UserInstance.Full_Name);
                    }
                }
                else
                {
                    if (this.Validation(false))
                    {
                        bool ischange = false;
                        if (user.Active != (bool)checkBoxActive.Checked)
                        {
                            ischange = true;
                            Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, user.Full_Name + "has disabled", instace.UserInstance.Full_Name);
                            user.Active = (bool)checkBoxActive.Checked;
                        
                        }
                        if (user.Full_Name != textBoxFullName.Text)
                        {
                            Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Change Properties of Full Name : from " + tempuser.Full_Name + " to " + textBoxFullName.Text, instace.UserInstance.Full_Name);
                            user.Full_Name = textBoxFullName.Text;
                            ischange = true;
                        }

                        if (user.User_Name != textBoxUserName.Text)
                        {
                            Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Change Properties of User Name : from " + tempuser.User_Name + " to " + textBoxUserName.Text, instace.UserInstance.Full_Name);
                            user.User_Name = textBoxUserName.Text;
                            ischange = true;
                        }
                        if (user.Email != textBoxEmail.Text)
                        {
                            Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Change Properties of Email address : from " + tempuser.Email + " to " + textBoxEmail.Text, instace.UserInstance.Full_Name);
                            user.Email = textBoxEmail.Text;
                            // instace.DataLink.sp_Update_Email(tempuser.Email, textBoxEmail.Text);
                            ischange = true;
                        }
                        if (user.sms_notification != checkBoxSMS.Checked)
                        {
                            Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Change Properties of SMS Notification : from " + tempuser.sms_notification.ToString() + " to " + checkBoxSMS.Checked.ToString(), instace.UserInstance.Full_Name);
                            user.sms_notification = checkBoxSMS.Checked;
                            ischange = true;
                        }
                        if (user.Title != textBoxTitle.Text)
                        {
                            Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Change Properties of User Title : from " + tempuser.Title + " to " + textBoxTitle.Text, instace.UserInstance.Full_Name);
                            user.Title = textBoxTitle.Text;
                            ischange = true;
                        }
                        if (user.Password_Expiry != Convert.ToInt32(textBoxexpires.Text))
                        {
                            Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Change Properties of Password Expiry : from " + tempuser.Password_Expiry + " to " + textBoxexpires.Text, instace.UserInstance.Full_Name);
                            user.Password_Expiry = Convert.ToInt32(textBoxexpires.Text);
                            ischange = true;
                        }
                        if (user.email_notification != checkBoxEmail.Checked)
                        {
                            Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Change Properties of Email Notification : from " + tempuser.email_notification.ToString() + " to " + checkBoxEmail.Checked.ToString(), instace.UserInstance.Full_Name);
                            user.email_notification = checkBoxEmail.Checked;
                            ischange = true;
                        }
                        if (user.Phone != txtPhone.Text)
                        {
                            Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Change Properties of Phone number address : from " + tempuser.Phone + " to " + txtPhone.Text, instace.UserInstance.Full_Name);
                            user.Phone = txtPhone.Text;
                            ischange = true;
                        }


                        if (user.Password != BAL.Authentication.GetEc(textBoxPassword.Text))
                        {
                            Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Change Properties ", instace.UserInstance.Full_Name);
                            user.Password = BAL.Authentication.GetEc(textBoxPassword.Text);
                            ischange = true;
                        }


                        if (user.Role1 != roleComboBox1.SelectedEntity)
                        {
                            Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Change Properties of Role : from " + tempuser.Authority + " to " + roleComboBox1.SelectedEntity.RoleName , instace.UserInstance.Full_Name);
                            user.Authority = roleComboBox1.SelectedEntity.RoleName;
                            ischange = true;
                        }

                        if (user.Department != deptComboBox1.SelectedEntity)
                        {
                            Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Change Properties of Department: from " + tempuser.Department.Department_Name + " to " + deptComboBox1.SelectedEntity.Department_Name, instace.UserInstance.Full_Name);
                            user.Department = deptComboBox1.SelectedEntity;
                            ischange = true;
                        }
                        if (user.Signature != textBoxSignature.Text)
                        {
                            Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Change Properties of Signature: from " + tempuser.Signature + " to " + textBoxSignature.Text, instace.UserInstance.Full_Name);
                            user.Signature = textBoxSignature.Text;
                            ischange = true;
                        }
                        if (ischange)
                        {
                            user.IsRowEnable = true;
                            user.ModefiedBy = instace.UserInstance.Full_Name;
                            user.ModifiedDateTime = DateTime.Now;

                            instace.DataLink.sp_Update_User(user.Id, user.User_Name, user.Password, roleComboBox1.SelectedEntity.Id, instace.UserInstance.Full_Name, user.Full_Name,user.Authority,
                                  user.Email, user.Phone, deptComboBox1.SelectedEntity.Department_Id, user.Active, user.Signature, user.Password_expiry_date, Convert.ToInt16(textBoxexpires.Text), user.sms_notification, user.email_notification, user.Title);
                            int a = instace.RefresUsers;

                            //Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "User Modified ", instace.UserInstance.Full_Name);
                            this.DialogResult = DialogResult.OK;
                        }
                    }
                }
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e) => this.DialogResult = DialogResult.Cancel;

        private void checkBoxEmail_CheckedChanged(object sender, EventArgs e)
        {
            textBoxEmail.Enabled = checkBoxEmail.Checked;
            textBoxEmail.Clear();
        }

        private void checkBoxSMS_CheckedChanged(object sender, EventArgs e)
        {
            txtPhone.Enabled = checkBoxSMS.Checked;
            txtPhone.Clear();
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBoxEmail_Validated(object sender, EventArgs e)
        {
            string email = textBoxEmail.Text;
            var regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            bool isValid = Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
            if (!isValid)
            {
                MessageBox.Show("Enter correct email address");
            }
        }

        private void textBoxFullName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBoxexpires_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            checkBoxActive.Visible = false;
        }
    }
}
