using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log_It.Forms
{
    public partial class frmViewLogging : Form
    {
        private readonly string DeviceType = string.Empty;
        private readonly string ReportPath = string.Empty;
        private readonly string DatabasePath = string.Empty;
        private string startdt = string.Empty;
        private string enddt = string.Empty;
        private readonly string DeviceID = string.Empty;
        public string SelectionFormula = string.Empty;
        private readonly DateTime SelectedDate = DateTime.Now; 

        public frmViewLogging()
        {
            InitializeComponent();
        }

        private void RadioToday_CheckedChanged(object sender, EventArgs e)
        {
            if (this.radioToday.Checked)
            {
                this.startdt = this.SelectedDate.ToString("MM/dd/yyyy 00:00:00");
                this.enddt = this.SelectedDate.ToString("MM/dd/yyyy 23:59:59");
            }
            else if (this.radioThisMonth.Checked)
            {
                int maxDay = DateTime.DaysInMonth(this.SelectedDate.Year, this.SelectedDate.Month);
                this.startdt = this.SelectedDate.ToString("MM/01/yyyy 00:00:00");
                this.enddt = this.SelectedDate.ToString("MM/" + maxDay.ToString() + "/yyyy 23:59:59");
            }
            else if (this.radioYesterday.Checked)
            {
                this.startdt = this.SelectedDate.AddDays(-1).ToString("MM/dd/yyyy 00:00:00");
                this.enddt = this.SelectedDate.AddDays(-1).ToString("MM/dd/yyyy 23:59:59");
            }
            else if (this.radioLastDays.Checked)
            {
                this.startdt = this.SelectedDate.AddDays(Convert.ToDouble(this.daysChanger.Text) * -1).ToString("MM/dd/yyyy 00:00:00");
                this.enddt = this.SelectedDate.AddDays(Convert.ToDouble(this.daysChanger.Text) * -1).ToString("MM/dd/yyyy 23:59:59");
            }
            else if (this.radioCustom.Checked)
            {
                groupBox1.Enabled = true;
                 string st = dateTimeInputfrom.Value.ToString("MM/dd/yyyy h:mm:ss tt");
                //string st = String.Format("{0:t tt}", this.timeSettingfrom.label1.Text);
                 string est = dateTimeInputto.Value.ToString("MM/dd/yyyy h:mm:ss tt");
     
                this.startdt = st;
                this.enddt = est;
            }			
        }

        private string Str2DateTime(string dt)
        {
            string temp = dt.Substring(6, 4);
            temp += "," + dt.Substring(0, 2);
            temp += "," + dt.Substring(3, 2);
            temp += "," + dt.Substring(11, 2);
            temp += "," + dt.Substring(14, 2);
            temp += "," + dt.Substring(17, 2);
            return temp;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            this.RadioToday_CheckedChanged(sender, e);
            //AND [date_]  >= '10/01/2018 12:00' AND  [date_] <= '10/27/2018 11:59'
            this.SelectionFormula = " AND [date_]  >= '" + this.startdt + "' AND  [date_] <= ' " + this.enddt + "' ORDER BY [date_]";
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void DaysChanger_TextChanged(object sender, EventArgs e)
        {
            this.radioLastDays.Checked = true;
        }

        private void FrmViewLogging_Load(object sender, EventArgs e)
        {
            this.dateTimeInputfrom.Value  = DateTime.Now;
            this.dateTimeInputto.Value= DateTime.Now; 			
        }

        private void daysChanger_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar)))
            {
                e.Handled = true;
            }
        }
    }
}
