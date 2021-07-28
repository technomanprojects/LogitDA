using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using Log_It.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using System.Xml;
using System.Security.AccessControl;
using System.IO;
using System.Text;

namespace Log_It
{
    static class Program
    {
        static bool isOk;
        static System.IO.Ports.SerialPort spt;
        public static string fileName = Application.StartupPath + "\\Log.txt";
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {

                Thread t = new Thread(new ThreadStart(splashscreen));
                t.Start();
                t.Name = "T1";
                Thread.Sleep(1000);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                DAL.P_Databases dt = new P_Databases(Application.StartupPath);
                DAL.P_Databases.Log d = new P_Databases.Log()
                {
                    DeviceID = Guid.NewGuid().ToString(),
                    Channel_ID = "Hello",
                    Port_Id = 12.0,
                    _Data = 12,
                    date_ = DateTime.Now
                };
                dt.InsertRecords(d);


                if (!System.IO.File.Exists(Application.StartupPath + "\\LogitSetting.xml"))
                {
                    SystemSetting ss = new SystemSetting();
                    if (ss.ShowDialog() == DialogResult.OK)
                    {
                        XmlTextWriter textWriter = new XmlTextWriter(Application.StartupPath + "\\LogitSetting.xml", null);
                        // Opens the document  
                        textWriter.WriteStartDocument();

                        // Write next element  
                        textWriter.WriteStartElement("ConnectionStringDb");
                        textWriter.WriteString(ss.ConnectionStringDb);
                        textWriter.WriteEndElement();
                        textWriter.WriteEndDocument();
                        // close writer  
                        textWriter.Close();
                    }
                    else
                    {
                        if (ss.DialogResult == DialogResult.Cancel)
                        {
                            return;
                        }
                    }
                }

                while (t.IsAlive)
                {

                }

                if (isOk && System.IO.File.Exists(Application.StartupPath + "\\LogitSetting.xml"))
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(Application.StartupPath + "\\LogitSetting.xml");
                    string connection = xmlDocument.GetElementsByTagName("ConnectionStringDb").Item(0).InnerText;
                    if (!File.Exists(fileName))
                    {
                        FileSecurity fSecurity = new FileSecurity();
                        fSecurity.AddAccessRule(new FileSystemAccessRule("EveryOne", FileSystemRights.FullControl, AccessControlType.Allow));

                        using (FileStream fs = File.Create(fileName, 1024, FileOptions.WriteThrough, fSecurity))
                        {
                            // Add some text to file    
                            Byte[] title = new UTF8Encoding(true).GetBytes("Error Log" + Environment.NewLine);
                            fs.Write(title, 0, title.Length);
                            fs.Close();
                            File.SetAttributes(fileName, FileAttributes.Archive | FileAttributes.Hidden);
                        }
                    }



                    //dt.InsertRecords()
                    BAL.LogitInstance instance = new BAL.LogitInstance(BAL.Authentication.GetDec(connection));
                    Technoman.Utilities.EventClass.connectionstring = instance.DataLink.Connection.ConnectionString;
                    Authentication authe = new Authentication(instance);
                    if (authe.ShowDialog() == DialogResult.OK)
                    {
                        instance.UserInstance = authe.UserInstance;
                        instance.SystemProperties = SetCOMPort(instance);
                        Technoman.Utilities.EventClass.connectionstring = instance.DataLink.Connection.ConnectionString;
                        Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Login, "User Login ", authe.UserInstance.Full_Name);
                        DAL.RoleEnum roleEnum = (DAL.RoleEnum)Enum.Parse(typeof(DAL.RoleEnum), instance.UserInstance.Role.ToString());
                        System.IO.Ports.SerialPort sp = new System.IO.Ports.SerialPort("COM" + instance.SystemProperties.Port.ToString(), Convert.ToInt32(instance.SystemProperties.BaudRate));
                        switch (roleEnum)
                        {
                            case RoleEnum.Owner:
                                Application.Run(new LogitMaincs(instance, authe.UserInstance));
                                break;
                            case RoleEnum.Administrator:
                            case RoleEnum.Power:
                            case RoleEnum.User:
                                if (instance.UserInstance.Password_expiry_date <= DateTime.Now)
                                {
                                    if (MessageBox.Show("Your Password has expired and must be changed.", "Password Expire", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                                    {
                                        return;
                                    }
                                    ChangePassword cp = new ChangePassword(instance, instance.UserInstance);
                                    if (cp.ShowDialog() == DialogResult.Cancel)
                                    {
                                        return;
                                    }
                                }             
                        Application.Run(new LogitMaincs(instance, authe.UserInstance));
                                break;
                            default:
                                break;
                        }

                        
                    }
                }
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");
               MessageBox.Show(m.Message);
            }


            //Application.Run(new Form1());
        }

        private static DAL.SYSProperty SetCOMPort(BAL.LogitInstance xml)
        {
            try
            {
                return xml.DataLink.SYSProperties.FirstOrDefault();

            }
            catch (Exception)
            {

                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                //Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");
                return null;
            }



        }

        public static void splashscreen()
        {
           // Application.Run(new Splash());
            Splash sp = new Splash();
            
            sp.ShowDialog();
            spt = sp.SP;
            isOk = sp.isok;
        }
    }
}
