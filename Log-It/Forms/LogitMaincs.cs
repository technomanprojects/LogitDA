using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BAL;
using Classes;
using Log_It.CustomControls;
using System.Diagnostics;
using Log_It.Pages;
using System.IO;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using System.Net.Sockets;
using Log_It.Pages.TaskPanel;
using DAL;
using Timer = System.Windows.Forms.Timer;

// Syed Rafiuddin
namespace Log_It.Forms
{

    
    public partial class LogitMaincs : Form, IMessageFilter
    {
        private TaskControl[] taskPanel;
        private ControlPage[] Pages;
        private PageControlEnum displayMode;
        private TankView tk;
        private TVView tvv;
        private TVControl[] tvc;
        private readonly bool signal = false;
        private readonly LogitInstance instance;
        private DAL.User userIntance;
        private readonly WizardController wc;
        private LogIt[] channel;
        private BarPack[] bp;
        public bool isCalibratorOn;
        private readonly Logit_Device logit_device;
        private DateTime backuplast;
        private readonly Thread t1;
        private Task<string> task;
        private CancellationTokenSource cancellationTokenSource; //Declare a cancellation token source
        private CancellationToken cancellationToken;
        private bool isRun = false;
        private int UserID;
        private int DeptID;
        private Guid _DeviceID;        
        public string LastValue;
        private string _result = "";
        private List<string> SMSUser;
        private List<string> EmailUser;
        private readonly Timer mTimer;
        private TcpClient client = null;
        private NetworkStream netStream = null;
        public LogitMaincs(LogitInstance instance, User userIntance)
        {
            try
            {
                InitializeComponent();
                logit_device = new Logit_Device(instance);
                LogIt.Logging += LogIt_Logging;
                LogIt.LastRecord += LogIt_LastRecord;
                mTimer = new Timer
                {
                    Interval = 10000//instance.SystemProperties.s
                };
                mTimer.Tick += LogoutUser;
                mTimer.Enabled = true;
                Application.AddMessageFilter(this);
                this.instance = instance;
                this.userIntance = userIntance;
                toolStripStatusUser.Text = "Login User: " + userIntance.User_Name;
                //toolStripStatusComPort.Text = "Communication Port: " + sp.PortName;
                LogIt.Parameters.Output1 += new RealTimesS(Parameters_Output1);
                LogIt.RealTime += new RealTimesChart(LogIt_RealTime);
                LogIt.Parameters.outofLimits += Parameters_outofLimits;
                LogIt.Parameters.InsertAckTMP += Parameters_InsertAckTMP;
                LogIt.Parameters.BarAlaram += Parameters_BarAlaram;
                LogIt.SendAlarmCondition += LogIt_SendAlarmCondition;
                CheckForIllegalCrossThreadCalls = false;

                if (instance.DataLink.SYSProperties.Single().D_Type == 2)
                {
                    
                }
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        void LogIt_SendAlarmCondition(object sender, EventArgs e)
        {
            
        }

        private void Parameters_BarAlaram(uint Index, double values, bool isactive)
        {
            try
            {
                if (bp != null)
                {
                    bp[Index].AlaramActive = isactive;
                }
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private async void Parameters_InsertAckTMP(Guid id, Guid Device_ID, string Location, string Instrument, string Event_Type, DateTime Event_DateTime, string Event, string Value)
        {
            try
            {
                await Task.Run(() =>
                {
                    instance.DataLink.Insert_Acknowladge_TMP(id, Device_ID, Location, Instrument, Event_Type, Event_DateTime, "Device Name: " + "ALERT! Instrument: " + Instrument + " " + Event_Type + " is " + Event + " (" + Value+ ")",0);

                    //SqlConnection con = new SqlConnection(instance.ConntectionLink);
                    //SqlDataReader rd;


                    //using (con)
                    //{
                    //    if (instance.SystemProperties.Acknowledged == true)
                    //    {




                    //        SqlCommand cmd = new SqlCommand("Insert_Acknowladge_TMP", con); // Read user-> stored procedure name
                    //        cmd.CommandType = CommandType.StoredProcedure;
                    //        cmd.Parameters.Add(new SqlParameter("@ID", id));
                    //        cmd.Parameters.Add(new SqlParameter("@Device_ID", Device_ID));
                    //        cmd.Parameters.Add(new SqlParameter("@CombineID", CombineID));
                    //        cmd.Parameters.Add(new SqlParameter("@Location", Location));
                    //        cmd.Parameters.Add(new SqlParameter("@Instrument", Instrument));
                    //        cmd.Parameters.Add(new SqlParameter("@Event_Type", Event_Type));
                    //        cmd.Parameters.Add(new SqlParameter("@Event_DateTime", Event_DateTime));
                    //        cmd.Parameters.Add(new SqlParameter("@Event", " Device Name: " + "ALERT! Instrument: " + Instrument + " " + Event_Type + " is " + Event + " (" + Data.ToString() + ")"));
                    //        cmd.Parameters.Add(new SqlParameter("@DeviceID", DeviceID));
                    //        cmd.Parameters.Add(new SqlParameter("@Channel_ID", Channel_ID));
                    //        cmd.Parameters.Add(new SqlParameter("@Data", Data));
                    //        cmd.Parameters.Add(new SqlParameter("@Current_Source", Current_Source));
                    //        cmd.Parameters.Add(new SqlParameter("@Voltage", Voltage));
                    //        con.Open();
                    //        rd = cmd.ExecuteReader();
                    //        rd.Close();
                    //    }
                    //}

                });

            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private async void Parameters_outofLimits(LogIt.Parameters p, string name, double values, bool isactive, uint Index, string read, string remakr, List<string> Email, List<string> SMS)
        {
            try
            {
                Debug.Print("Send Alerts.  " + DateTime.Now.ToString());

                if (instance.SystemProperties.Email == true)
                {
                    bool isconnected = true;//  await isInternetConnected();
                    if (!isconnected)
                    {
                        toolStripStatusLabel1.Text = "No internet connect";
                    }
                    if (isactive)
                    {
                        if (isconnected)
                        {
                            bool b = await SendMail(DateTime.Now, name, p.Name, values.ToString(), remakr, Email);
                        }
                    }

                    if (!isactive)
                    {
                        if (isconnected)
                        {
                            bool b = await SendMail(DateTime.Now, name, p.Name, values.ToString(), remakr, Email);
                        }
                    }
                }

                if (instance.SystemProperties.SMS == true)
                {
                    if (instance.SystemProperties.WebLink == true)
                    {
                        bool isconnected = true;// await isInternetConnected();
                        if (!isconnected)
                        {
                            toolStripStatusLabel1.Text = "No internet connect";
                        }
                        if (isactive)
                        {
                            if (isconnected)
                            {
                                if (SMS != null)
                                {
                                    if (p.Name == "Temperature")
                                    {
                                        read += " C";
                                    }
                                    if (p.Name == "Humidity")
                                    {
                                        read += " %";
                                    }
                                    string status = (string)remakr.Clone();

                                    string body = @"Date: " + DateTime.Now.ToString() + "\r\n" + "Time: " + DateTime.Now.ToShortTimeString() + "\r\nInstrument: " + name + "\r\n" + p.Name + "  = " + read + " \r\nDescription: \r\nALERT! " + p.Name + " is " + status;

                                    SMSComponent smscomponent = new SMSComponent(instance.SystemProperties);
                                    smscomponent.send(SMS, body);
                                    smscomponent.Dispose();
                                }
                            }
                        }

                        if (!isactive)
                        {
                            if (isconnected)
                            {
                                string status = (string)remakr.Clone();

                                if (SMS != null)
                                {
                                    if (SMS.Count > 0)
                                    {
                                        if (p.Name == "Temperature")
                                        {
                                            read += " C";
                                        }
                                        if (p.Name == "Humidity")
                                        {
                                            read += " %";
                                        }
                                        //string status = (string)remakr.Clone();

                                        string body = @"Date: " + DateTime.Now.ToString() + "\r\n" + "Time: " + DateTime.Now.ToShortTimeString() + "\r\nInstrument: " + name + "\r\n" + p.Name + "  = " + read + " \r\nDescription: \r\nALERT! " + p.Name + " is " + status;

                                        SMSComponent smscomponent = new SMSComponent(instance.SystemProperties);
                                        smscomponent.send(SMS, body);
                                        smscomponent.Dispose();
                                    }
                                }
                            }
                        }
                    }                   
                }
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private async Task<bool> SendMail(DateTime lastRecord, string device, string status, string read, string remakr, List<string> Email)
        {
            try
            {
                await Task.Run(() =>
                {
                    string status1 = (string)remakr.Clone();

                    if (status == "Temperature")
                    {
                        read += " C";
                    }
                    if (status == "Humidity")
                    {
                        read += " %";
                    }

                    string smtpAddress = instance.SystemProperties.EmailSMTP; //ConfigurationManager.AppSettings["SMTP"].ToString();
                    int portNumber = Convert.ToInt32(instance.SystemProperties.EmailPort);// ConfigurationManager.AppSettings["Port"].ToString());
                    bool enableSSL = true;
                    string emailFromAddress = instance.SystemProperties.EmailID;// ConfigurationManager.AppSettings["UserID"].ToString(); //Sender Email Address  
                    string password = instance.SystemProperties.EmailPassword;// ConfigurationManager.AppSettings["Password"].ToString(); //Sender Password  
                    // string emailToAddress = "it@technoman.biz"; //Receiv Email Address  
                    string subject = "Alert! " + device;
                    string body = @"<html><body> <p>Date & Time  : " + lastRecord.ToString() + "<br/>Instrument: <b>" + device + "</b><br/> " + status + "  = <b>" + read + "</b><br/><br/> Description: <br/> ALERT! " + status + " is <b>" + status1 + "</b><br/><br/>This is a system notification email. Please don't reply <br/><br/>Thanks & Regards, <br/>Logit System ";

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(emailFromAddress);
                        if (Email != null)
                        {
                            if (Email.Count > 0)
                            {
                                foreach (var item in Email)
                                {
                                    mail.To.Add(item);
                                }
                            }
                            else
                            {
                                mail.Dispose();
                                toolStripStatusLabel1.Text = "No any Email Address!";
                                return;
                            }
                        }
                        else
                        {
                            mail.Dispose();
                            toolStripStatusLabel1.Text = "No any Email Address!";
                            return;
                        }

                        mail.Subject = subject;
                        mail.Body = body;
                        mail.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                        {
                            try
                            {
                                smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                                smtp.EnableSsl = enableSSL;
                                toolStripStatusLabel1.Text = "Email is Sending";
                                smtp.Send(mail);
                                toolStripStatusLabel1.Text = "Email has been Sent";
                            }
                            catch (Exception m)
                            {
                                toolStripStatusLabel1.Text = "Email Send Fail";
                                var st = new StackTrace();
                                var sf = st.GetFrame(0);

                                var currentMethodName = sf.GetMethod();
                                Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");
                            }
                        }
                    }
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void LogIt_LastRecord(Guid Id, DateTime dt)
        {
            DAL.Device_Config config = instance.Device_Configes.SingleOrDefault(x => x.ID == Id);
            config.Last_Record = dt;
            if (instance.DataLink.Connection.State == ConnectionState.Closed)
            {
                instance.DataLink.Connection.Open();
            }
            instance.DataLink.SubmitChanges();
        }
        
        private void LogitMaincs_Load(object sender, EventArgs e)
        {
            try
            {
                sideNavItem2.Checked = true;

                taskPanel = new TaskControl[6];
                taskPanel[0] = new HomeTask(instance);
                taskPanel[1] = new AppTask();

                taskPanel[2] = new DatabaseTask();
                 
                EventTask eventtask = new EventTask();
                eventtask.RefreshControl += Eventtask_RefreshControl;
                eventtask.PrintP += Eventtask_PrintP;
                taskPanel[3] = eventtask;

                UserTask usertask = new UserTask(0, instance);
                usertask.AddUser += Usertask_AddUser;
                usertask.ModifiedUser += Usertask_ModifiedUser;
                usertask.DeleteUser += Usertask_DeleteUser;
                taskPanel[4] = usertask;

                
                DeptTask depttask = new DeptTask(0, instance);
                depttask.AddDept += Depttask_AddDept;
                depttask.ModifiedDept += Depttask_ModifiedDept;
                depttask.DeleteDept += Depttask_DeleteDept;
                taskPanel[5] = depttask;



                Pages = new ControlPage[8];
                HomePage home = new HomePage();
                home.PageIndex += Home_PageIndex;
                Pages[0] = home;

                ApplicationProperties propertiespage = new ApplicationProperties(instance, userIntance.User_Name);
                propertiespage.RefresPageProperties += Propertiespage_RefresPageProperties;
                Pages[1] = propertiespage;
                //if (userIntance.Role != 1)
                //{
                //    Log_It.Pages.ControlPage page = (Log_It.Pages.ApplicationProperties)Pages[1];
                //    page.Enabled = false;
                //}
                DBMaintenance dbMaintenance = new DBMaintenance(instance);
                dbMaintenance.CreatedbBackupManually += DbMaintenance_CreatedbBackupManually;
                Pages[2] = dbMaintenance;

                Pages[3] = new Eventpage(instance);

                UserPage userpage = new UserPage(instance);
                userpage.IDSet +=Userpage_IDSet;
                Pages[4] = userpage;

                DeptPage deptpage = new DeptPage(instance);
                deptpage.IDSet += Deptpage_IDSet;

                Pages[5] = deptpage;
                


                DeviceConfigPage devicepage = new DeviceConfigPage(instance);
                devicepage.IDSet += Devicepage_IDSet;
                Pages[6] = devicepage;



                AcknowledgePage ack_pages = new AcknowledgePage(instance);
                
                Pages[7] = ack_pages;
  
                foreach (var item in Pages)
                {
                    item.Dock = DockStyle.Fill;
                }
                foreach (var item in taskPanel)
                {
                    item.Dock = DockStyle.Fill;
                }

                panelControl.Controls.Add(Pages[0]);
                paneltask.Controls.Add(taskPanel[0]);
                displayMode = PageControlEnum.Home;
                treeView1.ExpandAll();
                if ( CreateLogItObjects())
                {
                     RunTask();
                }

                if (userIntance.Role == 3)
                {
                    sideNavItem2.Visible = false;
                    sideNavItem2.Checked = false;

                    sideNavItem3.Checked = true;
                    if (panelControl.Controls.Count > 0)
                    {
                        panelControl.Controls.Clear();
                    }
                    panelControl.Controls.Add(Pages[5]);
                    //paneltask.Controls.cAdd(taskPanel[0]);
                    displayMode = PageControlEnum.DeviceConfigPage;

                }  
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }

        }

        private void Depttask_DeleteDept()
        {
            try
            {
                if (DeptID > -1)
                {
                    DialogResult r = MessageBox.Show("Are your sure you want to delete select Department", "Delete department", MessageBoxButtons.YesNo);
                    if (r == DialogResult.No)
                    {
                        return;
                    }
                    Department _dept = instance.DataLink.Departments.SingleOrDefault(x => x.Department_Id == DeptID);
                    string dptname = _dept.Department_Name;
                    if (instance.DataLink.Device_Configs.SingleOrDefault(x => x.Department_Id == DeptID) == null)
                    {
                        MessageBox.Show("Sorry you can not delete this department because it's related to some devices.");
                        Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modfiy, "Department (" + _dept.Department_Name + ") Failed to Delete by " + instance.UserInstance.Full_Name, instance.UserInstance.Full_Name);

                    }
                    if (instance.DataLink.Users.SingleOrDefault(x => x.Department_Id == DeptID) == null)
                    {
                        MessageBox.Show("Sorry you can not delete this department because it's related to some Users.");
                        Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modfiy, "Department (" + _dept.Department_Name + ") Failed to Delete by " + instance.UserInstance.Full_Name, instance.UserInstance.Full_Name);

                    }

                    instance.DataLink.Departments.DeleteOnSubmit(_dept);
                    instance.DataLink.SubmitChanges();

                    MessageBox.Show("Department " + dptname + " has sucessfully deleted.");
                    DeptPage dp = (DeptPage)Pages[5];
                    dp.RefreshPage();
                    Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modfiy, "Department (" + _dept.Department_Name + ") Delete by " + instance.UserInstance.Full_Name, instance.UserInstance.Full_Name);
                }
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private void Depttask_ModifiedDept()
        {
            try
            {
                AddDepartment form = new AddDepartment(false, instance, DeptID);
                form.ShowDialog();
                DeptPage deptpage = (DeptPage)Pages[5];
                deptpage.RefreshPage();
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private void Depttask_AddDept()
        {
            DeptPage deptpage = (DeptPage)Pages[5];
            deptpage.RefreshPage();
        }

        private void Deptpage_IDSet(int id)
        {
            DeptID = id;
        }

        private void DbMaintenance_CreatedbBackupManually()
        {
            CreateBackup();
            MessageBox.Show("Backup has been done");
            Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Information, "Backup created Manually. ", instance.UserInstance.Full_Name);
        }

        private void CreateBackup()
        {
            try
            {
                string backuplocation = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Data\";
                if (!Directory.Exists(backuplocation))
                {
                    Directory.CreateDirectory(backuplocation);
                    var directoryInfo = new DirectoryInfo(backuplocation);
                    var directorySecurity = directoryInfo.GetAccessControl();
                    var currentUserIdentity = WindowsIdentity.GetCurrent();
                    var fileSystemRule = new FileSystemAccessRule("EveryOne",
                                                                  FileSystemRights.FullControl,
                                                                  InheritanceFlags.ObjectInherit |
                                                                  InheritanceFlags.ContainerInherit,
                                                                  PropagationFlags.None,
                                                                  AccessControlType.Allow);

                    directorySecurity.AddAccessRule(fileSystemRule);
                    directoryInfo.SetAccessControl(directorySecurity);
                }

                string DataBase = instance.DataLink.Connection.Database;
                string userid = BAL.Authentication.GetDec(instance.SystemProperties.idlog);
                string password = BAL.Authentication.GetDec(instance.SystemProperties.passlog);
                string serverInstance = instance.SystemProperties.serverinstance;
                if (instance.SystemProperties.backuplocation != null)
                {
                    backuplocation = instance.SystemProperties.backuplocation;
                }
                try
                {
                    SqlConnection Conn = new SqlConnection(instance.ConntectionLink);
                    SqlCommand cmd = new SqlCommand("Create_dbBackup", Conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };//INSERT INTO Eventlog (ID, DateTime,UserName, EventName, MessageLog) VALUES ('" + Guid.NewGuid() + "'," + (DateTime)DateTime.Now + ",'" + username + "','" + log.ToString() + "','" + Message + "')", Conn);
                    if (Conn.State == ConnectionState.Closed)
                    {
                        Conn.Open();
                    }
                    cmd.Parameters.Add(new SqlParameter("@dbName", DataBase));
                    cmd.Parameters.Add(new SqlParameter("@location", backuplocation + @"\" + DataBase + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + ".bak"));

                    cmd.ExecuteNonQuery();
                    Conn.Close();

                }
                catch (Exception m)
                {
                    toolStripStatusLabel1.Text = m.Message;
                }
               
                backuplast = DateTime.Now;
                Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Information, "Backup has been done", "System");
            }
            catch (Exception m)
            {

                var st = new StackTrace();
                var sf = st.GetFrame(0);
                MessageBox.Show(m.InnerException.Message);
                _ = sf.GetMethod();
                //Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");
            }
        }
        
        private void Parameters_outofLimits(uint Index, double values, bool isactive)
        {
            if (bp != null)
            {
                bp[Index].AlaramActive = isactive;
            }
        }
       
        /// <summary>
        /// Aosong Config Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cf_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (CreateLogItObjects())
            {
                RunTask();
            }
            if (panelControl.Controls.Count > 0)
            {
                panelControl.Controls.Clear();
            }
            panelControl.Controls.Add(Pages[5]);
            ControlPage page = (ControlPage)panelControl.Controls[0];
            page.RefreshPage();
        }

        private void Parameters_Output1(uint Index, double values)
        {
            if (bp != null)
                bp[Index].Value = (float)values;
        }

        private void LogIt_Logging(Guid DeviceID, string ChannelId, string Port_No, double _data)
        {
            try
            {
                Log log = new Log()
                {
                    Channel_ID = ChannelId,
                    DeviceID = DeviceID,
                    Port_Id = Port_No,
                    _Data = Convert.ToDouble(_data.ToString("00.00")),
                    date_ = Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/dd HH:mm:00")),
                };
                logit_device.InsertRecord(log);
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
                //MessageBox.Show(m.Message);
            }
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }
        
     
        public bool CreateLogItObjects()
        {
            try
            {
                int Length = logit_device.GetAllDevices().Count(x => x.Active == true && x.IsRowActive == true);
                if (Length > 0)
                {
                    channel = new LogIt[Length];
                    tvc = new TVControl[channel.Length];
                    int index = 0;
                    foreach (Device_Config item in logit_device.GetAllDevices().OrderBy(x => x.Port_No))
                    {
                        EmailUser = new List<string>();
                        SMSUser = new List<string>();

                        IQueryable<User> users = instance.DataLink.Users.Where(x => x.Active == true && x.IsRowEnable == true && x.Role != 0);
                        if (users != null)
                        {
                            IQueryable<User> users1 = users.Where(y => y.email_notification == true || y.sms_notification == true);

                            foreach (var items in users1)
                            {
                                if (!EmailUser.Contains(items.Email))
                                {
                                    if (items.Email != string.Empty || items.Email != "")
                                    {
                                        EmailUser.Add(items.Email);
                                    }
                                }
                                if (!SMSUser.Contains(items.Phone))
                                {
                                    if (items.Phone != string.Empty || items.Phone != "")
                                    {
                                        SMSUser.Add(items.Phone);
                                        toolStripStatusLabel1.Text = items.Phone;
                                    }
                                }
                            }
                        }

                        channel[index] = new LogIt(item, (int)instance.SystemProperties.Alert_Interval, EmailUser, SMSUser);
                        index++;
                    }
                    isRun = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
                return false;
                //MessageBox.Show(m.Message);
            }
        }

        private void DestroyLogItObject()
        {
            try
            {
                if (channel != null)
                {
                    foreach (LogIt logObj in channel)
                    {
                        logObj.Dispose();
                    }
                    channel = null;
                    LogIt.index = 0;
                }
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }
        
        private void Tk_close()
        {
            tk = null;
            //throw new NotImplementedException();
        }
        
        private void Propertiespage_RefresPageProperties()
        {
            cancellationTokenSource.Cancel();
            DestroyLogItObject();
            if (CreateLogItObjects())
            {
                RunTask();
            }
        }

        private void Devicepage_IDSet(Guid id)
        {
            _DeviceID = id;
        }

        private void Devicetask_AddedDevice()
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel();
            }
            DestroyLogItObject();

            if (CreateLogItObjects())
            {
                RunTask();
            }
            if (panelControl.Controls.Count > 0)
            {
                panelControl.Controls.Clear();
            }
            panelControl.Controls.Add(Pages[6]);
            ControlPage page = (ControlPage)panelControl.Controls[0];

            page.RefreshPage();          
        }

        private void Devicetask_DeleteDevice()
        {
            try
            {
                if (_DeviceID != null)
                {
                    if (instance.DataLink.Device_Configs.SingleOrDefault(x => x.ID == _DeviceID && x.Active == true && x.IsRowActive == true) == null)
                    {
                        return;
                    }
                    DialogResult r = MessageBox.Show("Are your sure you want to delete select Device", "Delete device", MessageBoxButtons.YesNo);
                    if (r == DialogResult.No)
                    {
                        return;
                    }
                    if (cancellationTokenSource != null)
                    {
                        cancellationTokenSource.Cancel();
                    }
                    DestroyLogItObject();
                    Device_Config config = instance.DataLink.Device_Configs.SingleOrDefault(x => x.ID == _DeviceID);
                    config.Active = false;
                    instance.DataLink.SubmitChanges();
                    DeviceConfigPage userpage = (DeviceConfigPage)Pages[6];
                    userpage.RefreshPage();
                    Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modfiy, "Device Location : ( "+ config.Location  +" )  Delete by " + instance.UserInstance.Full_Name, instance.UserInstance.Full_Name);
                    if (CreateLogItObjects())
                    {
                        RunTask();
                    }
                }
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private void Devicetask_ModifiedDevice()
        {
            try
            {
                if (_DeviceID != null)
                {

                    if (instance.DataLink.Device_Configs.SingleOrDefault(x => x.ID == _DeviceID && x.Active == true && x.IsRowActive == true) == null)
                    {
                        return;
                    }
                    if (cancellationTokenSource != null)
                    {
                        cancellationTokenSource.Cancel();
                    }
                    DestroyLogItObject();

                    AcquisitionAddForm form = new AcquisitionAddForm(_DeviceID, instance, false);
                    if (form.ShowDialog() ==  DialogResult.OK)
                    {
                        DeviceConfigPage devicepage = (DeviceConfigPage)Pages[6];
                        devicepage.RefreshPage();
                    }
                    if (panelControl.Controls.Count > 0)
                    {
                        panelControl.Controls.Clear();
                    }
                    panelControl.Controls.Add(Pages[6]);
                    ControlPage page = (ControlPage)panelControl.Controls[0];
                    page.RefreshPage();
                    if (CreateLogItObjects())
                    {
                        RunTask();
                    }
                    displayMode = PageControlEnum.DeviceConfigPage;
                }
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private void Usertask_AddUser()
        {
            try
            {
                UserPage userpage = (UserPage)Pages[4];
                userpage.RefreshPage();
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private void Usertask_DeleteUser()
        {
            try
            {
                if (UserID > -1)
                {
                    DialogResult r = MessageBox.Show("Are your sure you want to delete select user", "Delete user", MessageBoxButtons.YesNo);
                    if (r == DialogResult.No)
                    {
                        return;
                    }
                    User _user = instance.DataLink.Users.SingleOrDefault(x => x.Id == UserID);
                    _user.Active = false;
                    instance.DataLink.SubmitChanges();
                    UserPage userpage = (UserPage)Pages[4];
                    userpage.RefreshPage();
                    Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modfiy, "User  (" + _user.Full_Name +  ") Delete by " + instance.UserInstance.Full_Name, instance.UserInstance.Full_Name);
                }
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private void Usertask_ModifiedUser()
        {
            try
            {
                UserForm form = new UserForm(UserID, instance, false);
                form.ShowDialog();
                UserPage userpage = (UserPage)Pages[4];
                userpage.RefreshPage();
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private void Userpage_IDSet(int id)
        {
            UserID = id;
        }

        private void Eventtask_PrintP()
        {

            try
            {
                Eventpage eventpage = (Eventpage)Pages[3];
               // eventpage.PrintDoc();
                EventFilterForm form = new EventFilterForm(instance);
                form.ShowDialog(); 
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private void Eventtask_RefreshControl()
        {
            try
            {
                Eventpage eventpage = (Eventpage)Pages[3];
                eventpage.Refresh();
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }
        private void Home_PageIndex(int i)
        {
            try
            {
                if (panelControl.Controls.Count > 0)
                {
                    panelControl.Controls.Clear();
                    paneltask.Controls.Clear();
                }
                paneltask.Controls.Add(taskPanel[i]);

                panelControl.Controls.Add(Pages[i]);
                ControlPage page = (ControlPage)panelControl.Controls[0];
                page.RefreshPage();
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }
        
        private void Tvv_Closed(object sender, EventArgs e)
        {
            tvv = null;
        }

        private void Send2Channels(Guid id, double temperature, double humidity)
        {
            try
            {
                if (channel != null)
                {
                    LogIt log = channel.SingleOrDefault(p => p.ID == id);
                    log.Parameter[0].ParameterValue = temperature;
                    //if (log.RhActive)
                    //{
                    //    log.Parameter[1].ParameterValue = humidity;
                    //}
                    log.Parameter[0].SensorID = id.ToString();
                    log.isLogged = false;
                    log.LaunchRealTime();
                }
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private void LogitMaincs_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancellationTokenSource.Cancel();
            Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Shutdown, "System Shutdown", userIntance.Full_Name);
        }

        private async void RunTask()
        {
            try
            {
                int numericValue = 100;//Capture the user input
                object[] arrObjects = new object[] { numericValue };//Declare the array of objects
                //serialPort1.Open();
                //Because Cancellation tokens cannot be reused after they have been canceled,
                //we need to create a new cancellation token before each start
                cancellationTokenSource = new CancellationTokenSource();
                cancellationToken = cancellationTokenSource.Token;

                task = new Task<string>(new Func<object, string>(Run), arrObjects, cancellationToken);//Declare and initialize the task


                //lblStatus.Text = "Started Calculation...";//Set the status label to signal
                ////starting the operation
                //btnStart.Enabled = false; //Disable the Start button

                task.Start();//Start the execution of the task
                await task;// wait for the task to finish, without blocking the main thread

                //if (!task.IsFaulted)
                //{
                //    //textBox1.Text = string.Empty;
                //    //textBox1.Text = task.Result.ToString();//at this point,
                //    ////the task has finished its background work, and we can take the result
                //    //lblStatus.Text = "Completed.";//Signal the completion of the task
                //}

                //btnStart.Enabled = true; //Re-enable the Start button
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }
        public void GetData(string ipAddress,int index, int slaveID)
        {
            try
            {
                byte[] byteBuffer = Encoding.ASCII.GetBytes("#0" + slaveID.ToString() + '\r');
                if (client == null)
                {
                      client = new TcpClient();
                }
                while (!client.Connected)
                {
                    try
                    {
                        client.Connect(ipAddress, index);    
                    }
                    catch (Exception)
                    {
                        toolStripStatusComPort.Text = "Network disconnect.";
                        continue;
                    }
                }
                
                netStream = client.GetStream();
                netStream.Write(byteBuffer, 0, byteBuffer.Length);

                int totalBytesRcvd = 0; // Total bytes received so far
                int bytesRcvd = 0;
                Thread.Sleep(2000);
                //netStream.ReadTimeout = 5000;
                byteBuffer = new byte[114];
                while (totalBytesRcvd < byteBuffer.Length)
                {
                    bytesRcvd = netStream.Read(byteBuffer, totalBytesRcvd, byteBuffer.Length - totalBytesRcvd);
                    //if () == 114)
                    //{

                    //    break;
                    //}
                    totalBytesRcvd += bytesRcvd;
                }
                LastValue = Encoding.ASCII.GetString(byteBuffer, 0, totalBytesRcvd);
                          }
            catch (Exception m)
            {
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message, "");
            }
            finally
            {
                netStream.Close();
                client.Close();
                client = null;
            }
        }
        
        public new void Close()
        {
            if (client.Connected)
            {
                netStream.Close();
                client.Close();
            }
        }

        private List<DeviceList> dlist;

        private string Run(object id)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Close();
                    return string.Empty;
                }
                 if (instance.Device_Configes.Count(x => x.Active == true && x.IsRowActive == true) > 0)
                {

                    dlist = new List<DeviceList>(instance.Device_Configes.Where(x => x.Active == true && x.IsRowActive == true).Count());

                    foreach (var item in instance.DataLink.Device_Configs.Where(x => x.Active == true && x.IsRowActive == true).OrderBy(x => x.Port_No).ToList())
                    {
                        DeviceList d = new DeviceList();
                        d.ID = item.ID;
                        d.Channel = Convert.ToInt32(item.Channel_id);
                        d.Port = Convert.ToInt32(item.Port_No);
                        d.higher = (int)item.higher;
                        d.Lower = (int)item.Lower;
                        d.E_Port = item.E_Port;
                        dlist.Add(d);
                    }
                 }
                 int[] ports = new int[2] { Convert.ToInt32(instance.SystemProperties.Port1), Convert.ToInt32(instance.SystemProperties.Port2) };


                while (isRun)
                {
                    int index = 0;
                    
                    for (int k = 0; k < instance.SystemProperties.Number_Devices; k++)
                    {
                        GetData(instance.SystemProperties.IP_Address,ports[k],0);
                        if (cancellationToken.IsCancellationRequested)
                        {
                            base.Close();
                            break;
                        }

                        string lastValue = LastValue;
                       
                        if (lastValue == null || lastValue == string.Empty)
                        {
                            continue;
                        }
                        if (lastValue != null)
                        {
                           Debug.Print(lastValue.ToString());
                        }

                        if (lastValue.StartsWith(">"))
                        {
                            lastValue = lastValue.Remove(0, 1).Trim();
                        }
                        string str2 = string.Copy(lastValue);

                        string[] split = str2.Split(new char[] { '+', '-' });

                        List<string> list = new List<string>();

                        try
                        {
                            index = str2.IndexOf('+');
                            if (index != -1)
                            {  
                                if (dlist.Count > 0)
                                {
                                    for (int i = 0; i < dlist.Count; i++)
                                    {
                                        if (Convert.ToDecimal(split[i + 1]) > 4)
                                        {
                                            DeviceList dl = dlist.SingleOrDefault(b => b.E_Port == ports[k].ToString() && b.Channel == 0 && b.Port == i);
                                            if (dl != null)
                                            {
                                                decimal Pvhigh = Convert.ToDecimal(dl.higher);
                                                decimal Pvlow = Convert.ToDecimal(dl.Lower);
                                                decimal Ihigh = 20;
                                                decimal Ilow = 4;
                                                decimal pvt = Pvhigh - Pvlow;
                                                decimal ivt = Ihigh - Ilow;
                                                pvt = pvt / ivt;
                                                //decimal dt = r.Next(10, 20);
                                                decimal resutl = Convert.ToDecimal(split[i + 1]);
                                                double ofset = 0.003;
                                                resutl -= Convert.ToDecimal(ofset);
                                                decimal d = resutl - 4;
                                                decimal pv = (pvt * d) + Pvlow;
                                                //pv = pv / 100;
                                                Debug.Print(pv.ToString());
                                                list.Add(split[i + 1] + "," + index);
                                                Send2Channels(dl.ID, Convert.ToDouble(pv), 0.0);
                                            }
                                        }
                                    }
                                }
                            }
                            LastValue = string.Empty;
                        }
                        catch (Exception )
                        {
                            break;
                        }
                        int count = list.Count;
                    }
                }
                return string.Empty;
            }
            catch (Exception m)
            {
                toolStripStatusLabel1.Text = m.Message;
                isRun = false;
                return string.Empty;
            }
        }
         
        private void LogIt_RealTime(LogIt logItObject)
        {
            try
            {
                if (tvv != null)
                {
                    tvv.RealTimeData(logItObject);
                }
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }
        
        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (panelControl.Controls.Count > 0)
                {
                    panelControl.Controls.Clear();
                    paneltask.Controls.Clear();
                }
                switch (e.Node.Tag)
                {
                    case "Option":
                        panelControl.Controls.Add(Pages[0]);
                        paneltask.Controls.Add(taskPanel[0]);
                        displayMode = PageControlEnum.Home;
                        break;
                    default:
                        {
                            panelControl.Controls.Add(Pages[e.Node.Index + 1]);

                            ControlPage page = (ControlPage)panelControl.Controls[0];
                            page.RefreshPage();

                            paneltask.Controls.Add(taskPanel[e.Node.Index + 1]);
                            displayMode = (PageControlEnum)e.Node.Index + 1;
                            break;
                        }
                }
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private void SideNavItem3_Click(object sender, EventArgs e)
        {
            try
            {
                if (panelControl.Controls.Count > 0)
                {
                    panelControl.Controls.Clear();
                    panelControl.Controls.Add(Pages[6]);
                    ControlPage page = (ControlPage)panelControl.Controls[0];
                    page.RefreshPage();
                    displayMode = PageControlEnum.DeviceConfigPage;
                }
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private void SideNavItem2_Click(object sender, EventArgs e)
        {
            try
            {
                if (panelControl.Controls.Count > 0)
                {
                    panelControl.Controls.Clear();
                    panelControl.Controls.Add(Pages[0]);
                }
                if (paneltask.Controls.Count > 0)
                {
                    paneltask.Controls.Clear();
                    paneltask.Controls.Add(taskPanel[0]);
                }
                displayMode = PageControlEnum.Home;
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            try
            {
                tk = new TankView();
                tk.Close += Tk_close;

                tk.Dock = DockStyle.Fill;
                //int count = 0;

                foreach (var item in logit_device.GetAllDevices())
                {
                    //if (item.Rh_Active == true)
                    //{
                    //    count = count + 2;
                    //}
                    //else
                    //{
                    //    count++;
                    //}
                }

                tk.CreateTanks(logit_device.GetAllDevices().Count());
                tk.ApplyFont(new Font("Americana BT", 9));
                bp = tk.Tanks;

                int i = 0;
                IQueryable<Device_Config> config = logit_device.GetAllDevices();
                IOrderedQueryable<Device_Config> orderConfig = config.OrderBy(x => x.Port_No);
                foreach (var item in orderConfig)
                {
                    bp[i].Caption = item.Location;
                    //DAL.LimitTable templimit = item.LimitTables.SingleOrDefault();
                    switch (item.Device_Type)
                    {
                        case 1:
                            bp[i].Unit = "°C";

                            break;
                        case 2:
                            bp[i].Unit = "%";

                            break;
                        case 3:
                            bp[i].Unit = "Pa";

                            break;
                        default:
                            break;
                    }

                    bp[i].LLimit = (float)item.Lower_Limit;
                    bp[i].ULimit = (float)item.Upper_Limit;
                    bp[i].Min = (float)item.Lower_Range;
                    bp[i].Max = (float)item.Upper_Range;
                    bp[i].picTimeOut.Tag = item.Port_No.ToString();

                  
                    i++;
                }
                if (panelControl.Controls.Count > 0)
                {

                    if (panelControl.Controls[0] is TankView)
                    {
                    }
                    else
                    {
                        if (panelControl.Controls[0] is TVView tvView)
                        {
                            tvView.Dispose();
                        }

                        if (panelControl.Controls.Count > 0 && panelControl.Controls[0] is AcknowledgePage)
                        {
                            AcknowledgePage ackpage = (AcknowledgePage)panelControl.Controls[0];
                            ackpage.Dispose();
                        }
                        panelControl.Controls.Clear();
                    }
                    if (panelControl.Controls.Count > 0)
                    {
                        if (panelControl.Controls[0] is TVView tvView)
                        {
                            tvView.Dispose();
                        }
                    }
                    if (panelControl.Controls.Count > 0)
                    {
                        if (panelControl.Controls[0] is AcknowledgePage ackpage)
                        {
                            ackpage.Dispose();
                        }
                    }
                    panelDeviceTask.Controls.Clear();
                }
                displayMode = PageControlEnum.TankView;
                panelControl.Controls.Add(tk);
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                tvv = TVView.Instance(channel, logit_device, instance);
                tvv.ClientSize = new Size(Size.Width - 50, 1000);
                tvv.Close += Tvv_close;
                try
                {
                    tvc = tvv.TVs;
                    if (panelControl.Controls.Count > 0)
                    {
                        if (panelControl.Controls[0] is TVView)
                        {

                        }
                        else
                        {
                            if (panelControl.Controls[0] is TankView tnView)
                            {
                                tnView.Dispose();
                            }
                            if (panelControl.Controls.Count > 0)
                            {
                                if (panelControl.Controls[0] is AcknowledgePage ackpage)
                                {
                                    ackpage.Dispose();
                                }
                            }
                            panelControl.Controls.Clear();
                        }
                        if (panelControl.Controls.Count > 0)
                        {
                            if (panelControl.Controls[0] is TankView tnView)
                            {
                                tnView.Dispose();
                            }
                        }
                        if (panelControl.Controls.Count > 0)
                        {
                            if (panelControl.Controls[0] is AcknowledgePage ackpage)
                            {
                                ackpage.Dispose();
                            }
                        }
                      panelDeviceTask.Controls.Clear();
                    }
                    displayMode = PageControlEnum.TVView;
                    panelControl.Controls.Add(tvv);
                }
                catch (Exception m)
                {
                    MessageBox.Show(m.Message);
                }
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private void Tvv_close()
        {
            tvv = null;
        }
        /// <summary>
        /// Acquisition Config Event
        /// </summary>
        void frm_close()
        {
            //CreateLogItObjects();
            //if (panelControl.Controls.Count > 0)
            //{

            //    panelControl.Controls.Clear();
            //}
            //panelControl.Controls.Add(Pages[5]);
            //Log_It.Pages.ControlPage page = (Log_It.Pages.ControlPage)panelControl.Controls[0];

            //page.RefreshPage();
            //RunTask();
        }

        private void SideNavItem4_Click(object sender, EventArgs e)
        {
            ReportTask t = new ReportTask(instance);
            t.EventDevice += EventDevice;
            if (panelControl.Controls.Count > 0)
            {
                panelControl.Controls.Clear();
            }
            t.Dock = DockStyle.Fill;
            panelReport.Controls.Add(t);
        }

        private void EventDevice(DAL.Device_Config Config, DateTime SD, DateTime ED)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                ReportPage Rp = new ReportPage();
                Rp.RefreshPage(instance, Config, SD, ED);
                Rp.Dock = DockStyle.Fill;
                if (panelControl.Controls.Count > 0)
                {
                    panelControl.Controls.Clear();
                }
                panelControl.Controls.Add(Rp); 
                Cursor.Current = Cursors.Default;
            }
            catch (Exception)
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void DeviceTask_AddDevice()
        {
            throw new NotImplementedException();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    //tvc = tvv.TVs;

                    AcknowledgePage ackPage = new AcknowledgePage(instance);
                    if (panelControl.Controls.Count > 0)
                    {
                        if (panelControl.Controls.Count > 0)
                        {
                            if (panelControl.Controls[0] is TankView tnView)
                            {
                                tnView.Dispose();
                            }
                        }
                        if (panelControl.Controls.Count > 0)
                        {
                            if (panelControl.Controls[0] is TVView ackpage)
                            {
                                ackpage.Dispose();
                            }
                        }
                        panelDeviceTask.Controls.Clear();
                        panelControl.Controls.Clear();
                    }
                    //displayMode = PageControlEnum.TVView;
                    panelControl.Controls.Add(ackPage);
                    ackPage.Dock = DockStyle.Fill;
                }
                catch (Exception m)
                {

                    MessageBox.Show(m.Message);
                }
            }
            catch (Exception)
            {

                //var st = new StackTrace();
                //var sf = st.GetFrame(0);

                //var currentMethodName = sf.GetMethod();
                //Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");

            }
        }
    
        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                char[] buffer = new char[115];
                //this.serialPort1.ReadTimeout = 10000;
                string str = "";

                //str = this.serialPort1.ReadTo("\r");// Read(buffer, 0, 125);

                //for (int i = 0; i < num; i++)
                //{
                //    str = str + buffer[i];
                //}
                if (!str.Contains("\r"))
                {
                    _result = _result + str;
                    LastValue = str;
                }
                else
                {
                    LastValue = _result;
                    _result = "";
                }
               
                
                //this.serialPort1.DiscardInBuffer();
            }

        }

        public object Object { get; set; }

        private void Button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                //if (userIntance.Role != (int)DAL.RoleEnum.Administrator)
                //{
                //    MessageBox.Show("You can not proceed." + "\r\n"
                //        + "Only Administrators are allowed to Configure the system.", "Access Violation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}
                if (panelDeviceTask.Controls.Count > 0)
                {
                    panelDeviceTask.Controls.Clear();
                }

                if (panelControl.Controls.Count > 0)
                {
                    panelControl.Controls.Clear();
                    panelControl.Controls.Add(Pages[6]);
                    ControlPage page = (ControlPage)panelControl.Controls[0];
                    page.RefreshPage();
                    displayMode = PageControlEnum.DeviceConfigPage;

                }

                DeviceTask devicetask = new DeviceTask(0, instance);
                devicetask.ModifiedDevice += Devicetask_ModifiedDevice;
                devicetask.DeleteDevice += Devicetask_DeleteDevice;                
                devicetask.AddedDevice += Devicetask_AddedDevice;
                panelDeviceTask.Controls.Add(devicetask);

            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }

        private void ChangePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangePassword cp = new ChangePassword(instance, instance.UserInstance);
            cp.ShowDialog();
        }

        public bool PreFilterMessage(ref Message m)
        {
            // Monitor message for keyboard and mouse messages
            bool active = m.Msg == 0x100 || m.Msg == 0x101;  // WM_KEYDOWN/UP
            active = active || m.Msg == 0xA0 || m.Msg == 0x200;  // WM_(NC)MOUSEMOVE
            active = active || m.Msg == 0x10;  // WM_CLOSE, in case dialog closes
            if (active)
            {
                if (mTimer.Enabled)
                    mTimer.Enabled = false;
                mTimer.Start();
            }
            return false;
        }

        private void LogoutUser(object sender, EventArgs e)
        {
            try
            {
                // No activity, logout user
                mTimer.Enabled = false;
                Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.AutoLogout, "System Auto Logout", userIntance.Full_Name);
                Application.Exit();

            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName.Name, "System");
            }
        }
    }
}

public class DeviceList
{
    public Guid ID { get; set; }
    public int Channel { get; set; }
    public int Port
    { get; set; }
    public int higher
    { get; set; }
    public int Lower
    { get; set; }

    public string E_Port { get; set; }
}









