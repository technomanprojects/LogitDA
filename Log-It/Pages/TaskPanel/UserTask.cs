using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log_It.Pages.TaskPanel
{
    public partial class UserTask : TaskControl
    {
        public delegate void UserAdd();
        public event UserAdd AddUser;

        public delegate void UserModfied();
        public event UserModfied ModifiedUser;

        public delegate void UserDelete();
        public event UserDelete DeleteUser;

        private readonly int user;
        private readonly BAL.LogitInstance instance;

        public UserTask(int id, BAL.LogitInstance instance)
        {
            user = id;
            this.instance = instance;
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Forms.UserForm form = new Forms.UserForm(0,instance,true);
            if (form.ShowDialog() == DialogResult.OK)
            {
                AddUser?.Invoke();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ModifiedUser?.Invoke();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            DeleteUser?.Invoke();
        }
    }
}
