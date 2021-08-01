using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using BAL;

namespace Log_It.Pages
{
    public partial class DeviceConfigPage : ControlPage
    {
        public delegate void SetID(Guid id);
        public event SetID IDSet;

        private readonly LogitInstance instance;
        public DeviceConfigPage(LogitInstance instance)
        {
            this.instance = instance;
            InitializeComponent();
            RefreshPage();
        }
        public override void RefreshPage()
        {
            try
            {
                base.RefreshPage();
                if (instance.DataLink.Device_Configs.Count() > 0)
                {
                    bindingSource1.DataSource = instance.DataLink.Device_Configs.Where(x => x.Active == true).OrderBy(c => c.Port_No).ThenBy(n => n.Device_Type);

                    dataGridView1.DataSource = bindingSource1;
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[6].Visible = false;
                    dataGridView1.Columns[7].Visible = false;
                    dataGridView1.Columns[8].Visible = false;
                    dataGridView1.Columns[9].Visible = false;
                    dataGridView1.Columns[10].Visible = false;
                    dataGridView1.Columns[11].Visible = false;
                    dataGridView1.Columns[12].Visible = false;
                    dataGridView1.Columns[13].Visible = false;
                    dataGridView1.Columns[14].Visible = false;
                    dataGridView1.Columns[19].Visible = false;
                    dataGridView1.Columns[20].Visible = false;
                    dataGridView1.Columns[21].Visible = false;
                    dataGridView1.Columns[22].Visible = false;
                    dataGridView1.Columns[23].Visible = false;
                    dataGridView1.Columns[24].Visible = false;
                    dataGridView1.Columns[26].Visible = false;
                    dataGridView1.Columns[27].Visible = false;

                    dataGridView1.Columns[1].HeaderText = "Channel ID";
                    dataGridView1.Columns[2].HeaderText = "Port ID";
                    dataGridView1.Refresh();
                }
            }
            catch (Exception m)
            {

                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");

            }
           
        }

        private void DataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                IDSet?.Invoke((Guid)dataGridView1.Rows[e.RowIndex].Cells[0].Value);
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");
            }
        }
    }
}
