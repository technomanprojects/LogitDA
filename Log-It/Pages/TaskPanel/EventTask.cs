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
    public partial class EventTask : TaskControl
    {
        public delegate void RefreshPage();
        public event RefreshPage RefreshControl;

        public delegate void PrintPage();
        public event PrintPage PrintP;

        public EventTask()
        {
            InitializeComponent();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (RefreshControl != null)
            {
                RefreshControl();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (PrintP != null)
            {
                PrintP();
            }
        }
    }
}
