using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing.Drawing2D;
using BAL;
using Microsoft.Reporting.WinForms;
using System.IO;

namespace Log_It.Pages
{
    public partial class ReportPage : UserControl
    {
        LogitInstance instance;
        DAL.Device_Config config;
        Reports.DSLogit dt = null;
        public ReportPage()
        {
            InitializeComponent();
        }

        public void RefreshPage(LogitInstance instance, DAL.Device_Config config, DateTime SD, DateTime ED)
        {
            try
            {
                string s = "SELECT * From Log Where DeviceID = '" + config.ID + "' AND ([date_] > '" + SD.ToString("yyyy/MM/dd HH:MM:ss tt") + "' AND [date_] < '" + ED.ToString("yyyy/MM/dd HH:MM:ss tt") + "') Order by date_";
            string max = "SELECT MAX(_Data) As Maximum, MIN(_Data) As Minimumm, AVG(_Data) As Average From Log Where DeviceID = '" + config.ID + "' AND ([date_] > '" + SD.ToString("yyyy/MM/dd HH:MM:ss tt") + "' AND [date_] < '" + ED.ToString("yyyy/MM/dd HH:MM:ss tt") + "')";
            string maxdate = "SELECT MAX(date_) As Maximum, MIN(date_) As Minimumm From Log Where DeviceID = '" + config.ID + "' AND ([date_] > '" + SD.ToString("yyyy/MM/dd HH:MM:ss tt") + "' AND [date_] < '" + ED.ToString("yyyy/MM/dd HH:MM:ss tt") + "')";
            string hasRow = "SELECT COUNT(DeviceID) AS ROW  From Log Where DeviceID = '" + config.ID + "' AND ([date_] > '" + SD.ToString("yyyy/MM/dd HH:MM:ss tt") + "' AND [date_] < '" + ED.ToString("yyyy/MM/dd HH:MM:ss tt") + "')";

            this.instance = instance;
            this.config = config;
            labelid.Text = config.Port_No.ToString();
            labellocation.Text = config.Location;
            labelinstrument.Text = config.Instrument;
            label1interval.Text = config.Interval.ToString();
            labelstarttime.Text = SD.ToString();
            labelendtime.Text = ED.ToString();
            labeltemcaldate.Text = config.dateofCalibration.ToString();
            decimal tulimit, tllimit, hulimit = 0, hllimit = 0, charthigh, chartlow;




            switch (config.Device_Type)
            {
                case 1:
                    labelsuffix.Text = "C";
                    labeldevicetype.Text = "Temperature";
                    break;
                case 2:

                    labelsuffix.Text = "%";
                    labeldevicetype.Text = "Humidity";
                    break;
                case 3:

                    labelsuffix.Text = "Pa";
                    labeldevicetype.Text = "Differential Pressure";
                    break;
            }

            SqlConnection Conn = new SqlConnection(instance.ConntectionLink);
            SqlDataReader reader = null;
                SqlCommand cmd = new SqlCommand(hasRow, Conn)
                {
                    CommandType = CommandType.Text
                };
                Conn.Open();
            reader = cmd.ExecuteReader();


            while (reader.Read())
            {
                if (Convert.ToInt32(reader[0]) <= 0)
                {
                    MessageBox.Show("No record in selected dates");
                    Conn.Close();
                    return;
                }
            }
            Conn.Close();


                cmd = new SqlCommand("Select * from [Company]", Conn)
                {
                    CommandType = CommandType.Text
                };
                Conn.Open();
            reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                labelCompany.Text = reader[1].ToString();
                labelDepartment.Text = reader[2].ToString();
            }
            Conn.Close();

            reader = null;
                cmd = new SqlCommand(maxdate, Conn)
                {
                    CommandType = CommandType.Text
                };
                Conn.Open();
            reader = cmd.ExecuteReader();
            DateTime dtstart = DateTime.Now;
            DateTime dtend = DateTime.Now;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    if (reader[0] != null)
                    {
                        dtstart = (DateTime)reader[0];

                    }
                    if (reader[0] != null)
                    {
                        dtend = (DateTime)reader[1];

                    }


                    dtend = (DateTime)reader[1];
                }
                TimeSpan ts = dtstart - dtend;
                label1cycle.Text = ts.Days + "d " + ts.Hours + "h " + ts.Minutes + "m";

            }


            Conn.Close();

            dt =  new Reports.DSLogit();

                SqlCommand cmdAck = new SqlCommand("SELECT *  FROM [PlotterDA].[dbo].[Acknowledge]  where Device_ID = '" + config.ID + "' AND (Event_DateTime >=' " + SD.ToString("yyyy/MM/dd HH:MM:ss tt") + "'  AND Event_DateTime <= '" + ED.ToString("yyyy/MM/dd HH:MM:ss tt") + "' ) Order By Event_DateTime ASC", Conn)
                {
                    CommandType = CommandType.Text
                }; // Read user-> stored procedure name
                Conn.Open();
            SqlDataReader readerAck = cmdAck.ExecuteReader();
            while (readerAck.Read())
            {
                Reports.DSLogit.AcknowladgeRow arow = dt.Acknowladge.NewAcknowladgeRow();
                arow.Device_ID = readerAck["ID"].ToString();
                arow.Ack_DateTime = readerAck["Ack_DateTime"].ToString();
                arow.Comment = readerAck["Ack_Comments"].ToString();
                arow.Event = readerAck["Event"].ToString();
                arow.Event_DateTime = readerAck["Event_DateTime"].ToString();
                arow.Event_Type = readerAck["Event_Type"].ToString();
                arow.User = readerAck["Ack_User"].ToString();
                arow.Location = readerAck["Location"].ToString();
                arow.Instrument = readerAck["Instrument"].ToString();
                dt.Acknowladge.AddAcknowladgeRow(arow);
            }
            readerAck.Close();
            Conn.Close();

            Reports.DSLogit.DeviceInformationRow drow = dt.DeviceInformation.NewDeviceInformationRow();
            drow.Device_ID = labelid.Text;
            drow.Location = labellocation.Text;
            drow.Instrument = labelinstrument.Text;
            drow.Logging_Interval = label1interval.Text;
            drow.Loggin_cycle = label1cycle.Text;
            drow.Device_Type = labeldevicetype.Text;

            drow.Max_Limit = (int)config.Upper_Limit;
            drow.Min_Limit = (int)config.Lower_Limit;
            dt.DeviceInformation.AddDeviceInformationRow(drow);

            Reports.DSLogit.CompanyInfoRow companyrow = dt.CompanyInfo.NewCompanyInfoRow();
            companyrow.CompanyName = labelCompany.Text;
            companyrow.Department = labelDepartment.Text;
            dt.CompanyInfo.AddCompanyInfoRow(companyrow);

            Reports.DSLogit.UserInformationRow userrow = dt.UserInformation.NewUserInformationRow();
            userrow.FullName = instance.UserInstance.Full_Name;
            userrow.UserName = instance.UserInstance.User_Name;
            dt.UserInformation.AddUserInformationRow(userrow);


            Reports.DSLogit.StatisticsRow statiscticRow = dt.Statistics.NewStatisticsRow();


                cmd = new SqlCommand(s, Conn)
                {
                    CommandType = CommandType.Text
                };
                Conn.Open();
            reader = cmd.ExecuteReader();
            int i = 0;
            List<MKT> mkt = new List<MKT>();

            double sum = 0;
            while (reader.Read())
            {



                Reports.DSLogit.LogsTableRow row = dt.LogsTable.NewLogsTableRow();
                i++;
                row.Point = i.ToString();
                    MKT mk = new MKT
                    {
                        data = Convert.ToDouble(reader["_Data"]),
                        _date = Convert.ToDateTime(reader["date_"])
                    };
                    if (config.Device_Type == 1)
                {
                    mk.kvdata = Convert.ToDouble(mk.data + 273.15);
                    double d = (0.008314472 * mk.kvdata);
                    mk.expdata = Math.Exp(-83.14472 / d);
                    sum += mk.expdata;
                }
                mkt.Add(mk);
                row.Channel_ID = reader["Channel_ID"].ToString();
                row.TempData = reader["_Data"].ToString();
                row.Date_Time = reader["date_"].ToString();
                dt.LogsTable.AddLogsTableRow(row);

            }
            if (reader.HasRows)
            {
                if (config.Device_Type == 0)
                {
                    sum /= mkt.Count;
                    sum = Math.Log(sum);
                    sum /= -1;
                    sum = 10000 / sum;
                    sum -= 273.15;
                    labelTMKT.Text = sum.ToString("##.00");
                    statiscticRow.MKT = labelTMKT.Text;
                }
                dataGridView1.DataSource = dt;
                dataGridView1.DataMember = "LogsTable";
                dataGridView1.Columns[0].Width = 60;
                dataGridView1.Columns[1].Visible = false;

                dataGridView1.Columns[2].HeaderText = labelsuffix.Text;
                dataGridView1.Columns[2].Width = 120;


                dataGridView1.Columns[4].HeaderText = "Date Time";
                dataGridView1.Columns[4].Width = 180;
                dataGridView1.Columns[3].Visible = false;


                chart1.ChartAreas[0].AxisX.Title = "Date Time";
                chart1.ChartAreas[0].AxisX.LabelStyle.Format = "MM/dd/yyyy hh:mm:ss tt";
                chart1.Series[0].IsXValueIndexed = true;
                chart1.Series[0].XValueType = ChartValueType.DateTime;


                chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.LightBlue;
                chart1.ChartAreas[0].AxisY.LabelStyle.IsEndLabelVisible = false;
                chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.LightSalmon;

                chart1.ChartAreas[0].AxisY.Minimum = 0;
                chart1.ChartAreas[0].AxisY.Maximum = 100;
                chart1.ChartAreas[0].AxisY.Interval = 20;

                chart1.ChartAreas[0].AxisY2.Minimum = 0;
                chart1.ChartAreas[0].AxisY2.Maximum = 100;
                chart1.ChartAreas[0].AxisY2.Interval = 20;
                chart1.ChartAreas[0].AxisY2.MajorGrid.LineColor = Color.LightSalmon;



                foreach (MKT item in mkt)
                {
                    chart1.Series[0].Points.AddXY(item._date, item.data);
                    //chart1.Series[1].Points.AddXY(item._date, item.Rhdata);
                }


                foreach (MKT item in mkt)
                {
                    chart1.Series[0].Points.AddXY(item._date, item.data);
                }



                Conn.Close();

                    cmd = new SqlCommand(max, Conn)
                    {
                        CommandType = CommandType.Text
                    };
                    Conn.Open();
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    labelTmax.Text = reader[0].ToString();
                    labelTmin.Text = reader[1].ToString();
                    decimal stravg = Convert.ToDecimal(reader[2]);
                    labelTavg.Text = Decimal.Round(stravg, 2, MidpointRounding.AwayFromZero).ToString();

                }
                statiscticRow.Max = labelTmax.Text;
                statiscticRow.Min = labelTmin.Text;
                statiscticRow.Avg = labelTavg.Text;
                statiscticRow.DateCalibration = labeltemcaldate.Text;


                Conn.Close();
                dt.Statistics.AddStatisticsRow(statiscticRow);


                labeldatapoints.Text = i.ToString();


                this.reportViewer1.LocalReport.ReportEmbeddedResource = "Log_It.Reports.LogitReport.rdlc";

                reportViewer1.LocalReport.SubreportProcessing += LocalReport_SubreportProcessing;


                ReportDataSource datasourcelog = new ReportDataSource("DataSet1", dt.Tables["UserInformation"]);
                ReportDataSource datasourcelog2 = new ReportDataSource("DataSet2", dt.Tables["CompanyInfo"]);
                ReportDataSource datasourcelog3 = new ReportDataSource("DataSet3", dt.Tables["DeviceInformation"]);
                ReportDataSource datasourcelog4 = new ReportDataSource("DataSet4", dt.Tables["Statistics"]);
                ReportDataSource datasourcelog5 = new ReportDataSource("DataSet5", dt.Tables["LogsTable"]);
                ReportDataSource datasourcelog6 = new ReportDataSource("DataSet6", dt.Tables["DeviceRh"]);
                ReportDataSource datasourcelog7 = new ReportDataSource("DataSet7", dt.Tables["StatisticsRh"]);
                ReportDataSource datasourcelog8 = new ReportDataSource("DataSet8", dt.Tables["Approval"]);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(datasourcelog);
                reportViewer1.LocalReport.DataSources.Add(datasourcelog2);
                reportViewer1.LocalReport.DataSources.Add(datasourcelog3);
                reportViewer1.LocalReport.DataSources.Add(datasourcelog4);
                reportViewer1.LocalReport.DataSources.Add(datasourcelog5);
                reportViewer1.LocalReport.DataSources.Add(datasourcelog6);
                reportViewer1.LocalReport.DataSources.Add(datasourcelog7);
                reportViewer1.LocalReport.DataSources.Add(datasourcelog8);

                this.reportViewer1.RefreshReport();

            }
            else
            {
                MessageBox.Show("No record in selected dates");
            }
            }
            catch (Exception m)
            {

                MessageBox.Show(m.Message);
            }
        }

        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            ReportDataSource datasource = new ReportDataSource("DataSet1", dt.Tables["Acknowladge"]);


            e.DataSources.Add(datasource);
        }

        private void Chart1_GetToolTipText(object sender, System.Windows.Forms.DataVisualization.Charting.ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                DataPoint dp = e.HitTestResult.Series.Points[i];
                DateTime dt = DateTime.FromOADate(dp.XValue);
                e.Text = string.Format("TimeStamp: {0:F1}, Value: {1:F1}", dt.ToString(), dp.YValues[0]);

            }
        }

        private void Chart1_MouseMove(object sender, MouseEventArgs e)
        {

            //Point mousePoint = new Point(e.X, e.Y);

            //chart1.ChartAreas[0].CursorX.SetCursorPixelPosition(mousePoint, true);
            //chart1.ChartAreas[0].CursorY.SetCursorPixelPosition(mousePoint, true);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            try
            {
                this.chart1.Printing.PrintDocument.DefaultPageSettings.Landscape = true;
                this.pDoc.DefaultPageSettings.Landscape = true;
                dr = this.ppreview.ShowDialog();

               
            }
            catch (System.Exception winex)
            {
                MessageBox.Show(winex.Message, "Print Preview Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     

           
        }

        private void Footer(Graphics g)
        {
            g.PageUnit = GraphicsUnit.Inch;
            float x = 8.0f, y = 8.0f;
            Font f = new Font("MS Reference Sans Serif", 9);
            g.DrawString("Logit Chart By TECHNOMAN", f, Brushes.Black, x, y);
        }
        
        private void ChartHeading(Graphics g, string Heading)
        {
            g.PageUnit = GraphicsUnit.Inch;
            float x = 5.25f, y = 1.75f;
            Font f = new Font("Americana BT", 20, FontStyle.Bold | FontStyle.Underline);
            SizeF s = g.MeasureString(Heading, f);
            g.DrawString(Heading, f, Brushes.Black, x - (s.Width / 2), y);
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult dr;
            try
            {
                dr = ppreview.ShowDialog();
            }
            catch (Exception winex)
            {
                MessageBox.Show(winex.Message, "Print Preview Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }     
        }

        private void PDoc_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {


           this.PrintPageText(sender, e);
            e.Graphics.PageUnit = GraphicsUnit.Display;
            e.Graphics.TranslateTransform(-20, 220);
            e.Graphics.ScaleTransform(1.05f, 0.75f);
         
            this.chart1.Printing.PrintPaint(e.Graphics, new Rectangle(0,0, this.chart1.Width, this.chart1.Height));
            
        }

        private void PrintPageText(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            object[] obj1 = new object[6];
            for (int i = 0; i < obj1.Length; i++)
                obj1[i] = new object();

            obj1[0] = labeldevicetype.Text;
           
            obj1[1] = config.Location;
            obj1[2] = config.Instrument;
            obj1[3] = config.Interval + " minute(s)";
            obj1[4] = true;
            

            object[] obj2 = this.GetChannelParameter();


            DAL.Company company = instance.DataLink.Companies.SingleOrDefault();// Companies.ToArray();
            this.Picture(Application.StartupPath + "\\Log-it_Chart.bmp", e.Graphics);
            this.CDName(company.Company_Name, company.Department, e.Graphics);
            this.PrintDT(e.Graphics);
            this.ChartHeading(e.Graphics, "Log-It Chart");
            this.ChannelDetail(e.Graphics, obj1);
            this.ChartSummary(e.Graphics, obj2);
            //this.Legend(e.Graphics, "Temperature", this.chart1.Series[0].Color, 0);
            //if (logitObj.DeviceType == "01")
            //    this.Legend(e.Graphics, "Humidty", this.chart.ColorHLine, 1);
            this.Footer(e.Graphics);
        }

        private void Legend(Graphics g, string parameter, Color color, int parano)
        {
            g.PageUnit = GraphicsUnit.Inch;
            float x = 6.5f, y = 6.8f;

            Font f = new Font("MS Reference Sans Serif", 10);
            g.DrawString(parameter + ":", f, Brushes.Black, x + 0.2f, y + 0.9f + ((float)parano / 5.0f));

            SizeF s = g.MeasureString("Start Date/Time:i", f);
            g.PageUnit = GraphicsUnit.Pixel;
            SolidBrush sb = new SolidBrush(color);
            g.FillRectangle(sb, (x + 0.2f + s.Width) * g.DpiX, (y + 0.965f + ((float)parano / 5.0f)) * g.DpiY, (0.5f) * g.DpiX, (0.1f) * g.DpiY);
        }

        private void ChartSummary(Graphics g, object[] summary)
        {
            g.PageUnit = GraphicsUnit.Inch;
            float x = 6.5f, y = 6.6f;
            Font f = new Font("MS Reference Sans Serif", 11, FontStyle.Bold | FontStyle.Underline);
            g.DrawString("Chart Summary", f, Brushes.Black, x, y);

            f = new Font("MS Reference Sans Serif", 10);
            g.DrawString("Start Date/Time:", f, Brushes.Black, x + 0.2f, y + 0.3f);
            g.DrawString("End Date/Time:", f, Brushes.Black, x + 0.2f, y + 0.5f);
            g.DrawString("No. of Records:", f, Brushes.Black, x + 0.2f, y + 0.7f);
            g.DrawString("Signature:", f, Brushes.Black, x + 0.2f, y + 0.9f);

            SizeF s = g.MeasureString("Start Date/Time:i", f);

            f = new Font("MS Reference Sans Serif", 9);
            g.DrawString(summary[0].ToString(), f, Brushes.Black, x + 0.2f + s.Width, y + 0.3f);
            g.DrawString(summary[1].ToString(), f, Brushes.Black, x + 0.2f + s.Width, y + 0.5f);
            g.DrawString(summary[2].ToString(), f, Brushes.Black, x + 0.2f + s.Width, y + 0.7f);
            g.DrawString(summary[3].ToString(), f, Brushes.Black, x + 0.2f + s.Width, y + 0.9f);
        }

        private void ChannelDetail(Graphics g, object[] details)
        {
            g.PageUnit = GraphicsUnit.Inch;
            float x = 0.5f, y = 6.6f;
            Font f = new Font("MS Reference Sans Serif", 11, FontStyle.Bold | FontStyle.Underline);
            g.DrawString("Channel Details", f, Brushes.Black, x, y);

            f = new Font("MS Reference Sans Serif", 10);
            g.DrawString("Channel type:", f, Brushes.Black, x + 0.2f, y + 0.3f);
            g.DrawString("Location:", f, Brushes.Black, x + 0.2f, y + 0.5f);
            g.DrawString("Instrument:", f, Brushes.Black, x + 0.2f, y + 0.7f);
            g.DrawString("Logging Interval:", f, Brushes.Black, x + 0.2f, y + 0.9f);
            g.DrawString("Logging On:", f, Brushes.Black, x + 0.2f, y + 1.1f);
            g.DrawString("Alarm On:", f, Brushes.Black, x + 0.2f, y + 1.3f);

            SizeF s = g.MeasureString("Logging Interval:i", f);

            f = new Font("MS Reference Sans Serif", 9);
            g.DrawString(details[0].ToString(), f, Brushes.Black, x + 0.2f + s.Width, y + 0.3f);
            g.DrawString(details[1].ToString(), f, Brushes.Black, x + 0.2f + s.Width, y + 0.5f);
            g.DrawString(details[2].ToString(), f, Brushes.Black, x + 0.2f + s.Width, y + 0.7f);
            g.DrawString(details[3].ToString(), f, Brushes.Black, x + 0.2f + s.Width, y + 0.9f);
            g.DrawString(details[4].ToString(), f, Brushes.Black, x + 0.2f + s.Width, y + 1.1f);
            g.DrawString(details[5].ToString(), f, Brushes.Black, x + 0.2f + s.Width, y + 1.3f);
        }
        
        private void PrintDT(Graphics g)
        {
            g.PageUnit = GraphicsUnit.Inch;
            float x = 0.5f, y = 1.4f;
            string date = DateTime.Now.ToString("dd-MMMM-yyyy");
            string time = DateTime.Now.ToString("hh:mm:ss tt");
            Font f = new Font("Americana BT", 10);
            SizeF s1 = g.MeasureString(date, f);
            g.DrawString(date.ToUpper(), f, Brushes.DarkBlue,
                x, y);
            SizeF s2 = g.MeasureString(time, f);
            g.DrawString(time.ToUpper(), f, Brushes.Black,
                x, s2.Height + y);
        }

        private void CDName(string companyname, string deptname, Graphics g)
        {
            g.PageUnit = GraphicsUnit.Inch;
            float x = 10.0f, y = 0.9f;
            Font f = new Font("Arial", 16, System.Drawing.FontStyle.Bold);
            SizeF s1 = g.MeasureString(companyname, f);
            g.DrawString(companyname, f, Brushes.DarkBlue,
                x - s1.Width, y);
            f = new Font("Arial", 16);
            SizeF s2 = g.MeasureString(deptname, f);
            g.DrawString(deptname, f, Brushes.Black,
                x - s2.Width, s2.Height + y);			
        }

        private void Picture(string path, Graphics g)
        {
            g.PageUnit = GraphicsUnit.Inch;
           g.DrawImage(Properties.Resources.Log_it_Chart1, 0.5f, 0.5f);
        }

        private object[] GetChannelParameter()
        {
            object[] obj2 = new object[4];
            for (int i = 0; i < obj2.Length; i++)
            {
                obj2[i] = new object();
                obj2[i] = string.Empty;
            }

            obj2[0] = this.labelstarttime.Text;
            obj2[1] = this.labelendtime.Text;
            obj2[2] = this.labeldatapoints.Text;
            obj2[3] = this.instance.UserInstance.Full_Name;
      
            return obj2;
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
           
        }

        private void reportViewer1_PrintingBegin(object sender, ReportPrintEventArgs e)
        {

        }

        private void reportViewer1_Print(object sender, ReportPrintEventArgs e)
        {
            

        }

        private void Button2_Click_2(object sender, EventArgs e)
        {
            try
            {
                Reports.DSLogit.ApprovalRow approvedrow= dt.Approval.NewApprovalRow();
                if (!GetApproval("Reviewed By"))
                {
                    return;
                }
                approvedrow.Reviewed = forms.ApprovalUser.Signature;
                if (!GetApproval("Approved By"))
                {
                    return;
                }
                approvedrow.Approved = forms.ApprovalUser.Signature;

                ReportDataSource datasourcelog8 = new ReportDataSource("DataSet8", dt.Tables["Approval"]);

                dt.Approval.AddApprovalRow(approvedrow) ;
                reportViewer1.LocalReport.DataSources.Add(datasourcelog8);


                this.reportViewer1.RefreshReport();


                SaveFileDialog savedialog = new SaveFileDialog();
                savedialog.Filter = "PDF Files|*.pdf";
                string ReportIDT = config.Channel_id + "_T" + labelCompany.Text.Substring(0, 1) + DateTime.Now.Day.ToString("0#") + DateTime.Now.Month.ToString("0#") + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + "T";
                string ReportIDRH = config.Channel_id + "_T" + labelCompany.Text.Substring(0, 1) + DateTime.Now.Day.ToString("0#") + DateTime.Now.Month.ToString("0#") + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + "TRH";

                string filename = string.Empty;
                switch (config.Device_Type)
                {
                    case 1:
                        filename = ReportIDRH;
                        break;
                    case 2:
                        filename = ReportIDT;
                        break;
                    default:
                        break;
                }
                savedialog.FileName = filename;
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = savedialog.FileName;
                    FileInfo fi = new FileInfo(fileName);
                    if (fi.Exists)
                    {
                        fi.Delete();
                    }
                    //Warning[] warnings;
                    //string[] streaminds;
                    //string mimetype, ecoding, filenameExtension;
                    byte[] bytes = reportViewer1.LocalReport.Render("PDF");
                    using (FileStream fs = File.Create(fileName))
                    {
                        fs.Write(bytes, 0, bytes.Length);
                    }
                    MessageBox.Show("File has been export successfully.");
                    //ackReport.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, fileName);

                }
            }
            catch (Exception m)
            {

                MessageBox.Show(m.Message);
            }
        }
        Forms.ApprovalForm forms;

        private bool GetApproval(string s)
        {

            forms = new Forms.ApprovalForm(instance,s);
            if (forms.ShowDialog() == DialogResult.OK)
            {
                return true;
            }
            return false;

        }
    }
    public class MKT
    {
   
        public double data;
        public double kvdata;
        public double expdata;
        public DateTime _date;
    }
}
