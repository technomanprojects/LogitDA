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
    public partial class DeviceTask : TaskControl
    {
        public delegate void DeviceAdd();
        public event DeviceAdd AddDevice;

        public delegate void DeviceAdded();
        public event DeviceAdded AddedDevice;


        public delegate void DeviceModfied();
        public event DeviceModfied ModifiedDevice;

        public delegate void DeviceDelete();
        public event DeviceDelete DeleteDevice;

        private readonly int user;
        private readonly BAL.LogitInstance instance;

        public DeviceTask(int id, BAL.LogitInstance instance)
        {
            this.user = id;
            this.instance = instance;
            InitializeComponent();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ModifiedDevice?.Invoke();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            DeleteDevice?.Invoke();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Forms.AcquisitionAddForm form = new Forms.AcquisitionAddForm(Guid.NewGuid(), instance, true);

            if (form.ShowDialog() == DialogResult.OK)
            {
                AddedDevice?.Invoke();
            }
        }
    }
}
