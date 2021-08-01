using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Technoman.Utilities
{
    public enum EventLog
    {
        Modfiy,
        Startup,
        Login,
        Logout,
        Shutdown,
        Error,
        Warning,
        Timeout,
        Alarm,
        System,
        Modify,
        Information,
        AutoLogout
    }

    public static  class EventClass
    {
        public static string connectionstring = string.Empty;
        public static string fileName = Application.StartupPath + "\\LogError.txt";
        static readonly object _objectError = new object();
        public static  void WriteLog(EventLog log, string Message, string username)
        {
            SqlConnection Conn = new SqlConnection(connectionstring);
            Message = Message.Replace("'", "!");
            SqlCommand cmd = new SqlCommand("INSERT INTO Eventlog (ID, DateTime,UserName, EventName, MessageLog) VALUES ('" + Guid.NewGuid() + "','" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "','" + username + "','" + log.ToString() + "','" + Message + "')", Conn)
            {
                CommandType = CommandType.Text
            };
            Conn.Open();
            cmd.ExecuteNonQuery();
            Conn.Close();
        }
        public static void ErrorLog(EventLog log, string data, string username)
        {
            Monitor.Enter(_objectError);
            try
            {
                using (StreamWriter writer = File.AppendText(fileName))
                {
                    writer.WriteLine(DateTime.Now.ToString() + " " + data + '\n');
                    writer.Close();
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                Monitor.Exit(_objectError);
            }

        }
    }
}
