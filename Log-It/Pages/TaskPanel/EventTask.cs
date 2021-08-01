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

        private void Button4_Click(object sender, EventArgs e)
        {
            RefreshControl?.Invoke();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            PrintP?.Invoke();
        }
    }
}
