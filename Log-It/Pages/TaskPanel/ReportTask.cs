using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace Log_It.Pages.TaskPanel
{
    public partial class ReportTask : UserControl
    {

        public delegate void DeviceEvent(DAL.Device_Config Config,DateTime SD,DateTime ED);
        public event DeviceEvent EventDevice;

        private readonly BAL.LogitInstance Instance;
        public ReportTask(BAL.LogitInstance Instance)
        {
            InitializeComponent();
            this.Instance = Instance;

            dateTimeInputfrom.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dateTimeInputto.ResetValue();
            dateTimeInputto.Value = DateTime.Now;
            this.Instance = Instance;

            if (Instance.DataLink.Device_Configs.Count() > 0)
            {
                foreach (var item in Instance.DataLink.Device_Configs.Where(p => p.Active == true && p.IsRowActive == true).OrderBy(x => x.Port_No))
                {
                    entityListBox1.Items.Add(item);
                }
            }
        }

        private void entityListBox1_DoubleClick(object sender, EventArgs e)
        {
            if (Instance.SystemProperties.Automatic_Sign != true)
            {
                Forms.Authentication auth = new Forms.Authentication(Instance);
                if (auth.ShowDialog() == DialogResult.OK)
                {
                    Instance.UserReport = auth.UserInstance;
                    GenerateReport();
                }
                else
                {
                    return;
                }
            }
            else
            {
                Instance.UserReport = Instance.UserInstance;
                GenerateReport();
            }
            

            //if (EventDevice!= null &&  entityListBox1.SelectedEntity != null)
            //{
            //    string str = dateTimeFrom.Value.ToString("MM/dd/yyyy 00:00:00");
            //    string etr = dateTimeTo.Value.ToString("MM/dd/yyyy 23:59:59");

            //    EventDevice(entityListBox1.SelectedEntity, Convert.ToDateTime(str),Convert.ToDateTime(etr));            
            //}
        }
        private void GenerateReport()
        {
            if (EventDevice != null && entityListBox1.SelectedItem != null)
            {
                //en-GB date format dd/MM/yyyy
                //en-US date format MM/dd/yyyy
                string str = string.Empty;
                string etr = string.Empty;
                string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;

                if (sysFormat == "MM/dd/yyyy")
                {
                    CultureInfo us = new CultureInfo("en-US");
                    _ = us.DateTimeFormat.ShortDatePattern;
                    str = dateTimeInputfrom.Value.ToString("MM/dd/yyyy h:mm:ss tt");
                    etr = dateTimeInputto.Value.ToString("MM/dd/yyyy h:mm:ss tt");
                }

                if (sysFormat == "dd/MM/yyyy")
                {
                    CultureInfo us = new CultureInfo("en-GB");
                    _ = us.DateTimeFormat.ShortDatePattern;
                    str = dateTimeInputfrom.Value.ToString("dd/MM/yyyy h:mm:ss tt");
                    etr = dateTimeInputto.Value.ToString("dd/MM/yyyy h:mm:ss tt");
                }

                DateTime startdate = Convert.ToDateTime(str);
                DateTime EndDate = Convert.ToDateTime(etr);

                TimeSpan dt = EndDate.Subtract(startdate).Duration();

                //DAL.Device_Config config = 
                EventDevice(entityListBox1.SelectedEntity, Convert.ToDateTime(str), Convert.ToDateTime(etr));
            }
        }
    }
}
