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
    public partial class HomeTask : TaskControl
    {
        private readonly BAL.LogitInstance instance;
        public HomeTask(BAL.LogitInstance instance)
        {
            InitializeComponent();
            if (instance.UserInstance.Authority == "Owner")
            {
                button1.Visible = true;
            }
           
            this.instance = instance;
        }

        private void LinkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            linkLabel1.LinkVisited = true;
            System.Diagnostics.Process.Start("http://www.technoman.biz");  
        }

        private void LinkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Application.Run(new Forms.Splash());
        }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            Forms.Calibrator cl = new Forms.Calibrator(instance);
            cl.ShowDialog();
        }
    }
}
