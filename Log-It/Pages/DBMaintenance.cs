using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using BAL;
using System.Data.SqlClient;

namespace Log_It.Pages
{
    public partial class DBMaintenance : ControlPage
    {
        public delegate void PowerStatusChanged();
        public event PowerStatusChanged CreatedbBackupManually;
        private readonly LogitInstance instance;

        public DBMaintenance(LogitInstance instance)
        {
            InitializeComponent();
            this.instance = instance;
            try
            {
                label7.Text = instance.SystemProperties.backuplocation;
                SqlConnection Conn = new SqlConnection(instance.DataLink.Connection.ConnectionString);
                SqlCommand testCMD = new SqlCommand("sp_spaceused", Conn);
                SqlCommand cmd = new SqlCommand("select @@version AS 'Server Name' ", Conn);
                testCMD.CommandType = CommandType.StoredProcedure;
                cmd.CommandType = CommandType.Text;
                Conn.Open();
                SqlDataReader reader = testCMD.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        labelDBName.Text = reader["database_name"].ToString();
                        labelFileSize.Text = reader["database_size"].ToString();
                    }
                }
                Conn.Close();
                Conn.Open();

                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        labelServerType.Text = r["Server Name"].ToString();
                    }
                }
                Conn.Close();
            }
            catch (Exception m)
            {
                MessageBox.Show(m.Message);
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            CreatedbBackupManually?.Invoke();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string mess = string.Empty;
                FolderBrowserDialog fb = new FolderBrowserDialog();
                if (fb.ShowDialog() == DialogResult.OK)
                {
                    string files = fb.SelectedPath;
                    Directory.CreateDirectory(files);
                    var directoryInfo = new DirectoryInfo(files);
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
                    if (instance.SystemProperties.backuplocation != files + @"\")
                    {
                        mess = instance.SystemProperties.backuplocation + " to " + files + @"\";
                    }
                    instance.SystemProperties.backuplocation = files + @"\";
                    label7.Text = instance.SystemProperties.backuplocation;
                    instance.DataLink.SubmitChanges();
                    Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Databaase Location " + mess.ToString() + " has updated successfully.", instance.UserInstance.Full_Name);

                }
            }
            catch (Exception m)
            {
                MessageBox.Show(m.Message);
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog fb = new OpenFileDialog())
                {
                    fb.Title = "Select Database File";
                    fb.Filter = "Bak files (*.bak)|*.bak|All files (*.*)|*.*";
                    fb.DefaultExt = "bak";
                    if (fb.ShowDialog() == DialogResult.OK)
                    {
                        string filepath = fb.FileName;
                        string filename = Path.GetFileNameWithoutExtension(filepath);
                        string query = "IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'" + filename + "') DROP DATABASE " + filename + " RESTORE DATABASE " + filename + " FROM DISK = '" + fb.FileName + "'";
                        SqlConnection Conn = new SqlConnection(instance.DataLink.Connection.ConnectionString);
                        SqlCommand testCMD = new SqlCommand(query, Conn);
                        Conn.Open();
                        testCMD.ExecuteNonQuery();
                        Conn.Close();
                        MessageBox.Show("Databaase restore successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Databaase restore successfully.", instance.UserInstance.Full_Name);
                    }
                }
            }
            catch (Exception m)
            {
                MessageBox.Show(m.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
