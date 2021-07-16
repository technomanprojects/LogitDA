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
        int user;
        BAL.LogitInstance instance;
        bool isNew;

        public DeviceTask(int id, BAL.LogitInstance instance)
        {
            this.user = id;
            this.instance = instance;
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (ModifiedDevice != null)
            {
                ModifiedDevice();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (DeleteDevice != null)
            {
                DeleteDevice();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            Log_It.Forms.AcquisitionAddForm form = new Forms.AcquisitionAddForm(Guid.NewGuid(), instance, true);

            if (form.ShowDialog() == DialogResult.OK)
            {

                if (AddedDevice != null)
                {
                    AddedDevice();
                }
            }
        }

        void form_close()
        {
            if (AddedDevice != null)
            {
                AddedDevice();
            }
        }
    }
}
