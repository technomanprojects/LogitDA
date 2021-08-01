using BAL;
using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Technoman.Utilities;
using EventLog = Technoman.Utilities.EventLog;

namespace Log_It.Forms
{
    public partial class ApprovalForm : BaseForm
    {
        private readonly LogitInstance Instance;
        private readonly BAL.Authentication aut;
        public User ApprovalUser { get; set; }
        public User ReviewedUser { get; set; }
        public ApprovalForm(LogitInstance Instance,string caption)
        {
            this.Instance = Instance;
            InitializeComponent();
            this.Text = caption;
            aut = new BAL.Authentication(Instance);
            List<User> users = Instance.DataLink.Users.Where(x => x.Active == true && x.IsRowEnable == true).ToList();
            foreach (var item in users)
            {
                userList1.Items.Add(item);
            }
        }
        private bool FormValidation()
        {
            if (userList1.Text == string.Empty || userList1.SelectedItem == null)
            {
                MessageBox.Show("Please select the user name.");
                return false;
            }
           
            if (textBoxpassword.Text == string.Empty)
            {
                MessageBox.Show("Please enter the Password.");
                return false;
            }
            return true;
        }
        private void Buttonlogin_Click(object sender, EventArgs e)
        {
            if (!FormValidation())
            {
                return;
            }
            if (aut.IsUserValid(userList1.SelectedItem.ToString().ToLower(), textBoxpassword.Text))
            {
                ApprovalUser = aut.GetUser;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid User name and Password");
                EventClass.WriteLog(EventLog.Warning, "User try to login failed", userList1.SelectedEntity.User_Name.ToLower());
            }
        }
    }
}
