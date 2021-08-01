using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BAL;
using Technoman.Utilities;
using DAL;

namespace Log_It.Forms
{
    public partial class Authentication : BaseForm
    {
        private readonly LogitInstance Instance;
        private readonly BAL.Authentication aut;
        public User UserInstance { get; set; }
        public Authentication(LogitInstance instance )
        {
            this.Instance = instance;
            InitializeComponent();
            aut = new BAL.Authentication(Instance);
        }

        private bool FormValidation()
        {
            if (textBoxUsername.Text == string.Empty)
            {
                MessageBox.Show("Please enter the User Name.");
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
            if ( aut.IsUserValid(textBoxUsername.Text.ToLower(),textBoxpassword.Text))
            {
                UserInstance = aut.GetUser;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid User name and Password");
                EventClass.WriteLog(Technoman.Utilities.EventLog.Warning, "User try to login failed", textBoxUsername.Text);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Authentication_Load(object sender, EventArgs e)
        {
            textBoxUsername.Focus();
        }
    }
}
