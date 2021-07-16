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
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Log_It.Pages
{
    public partial class DBMaintenance : ControlPage
    {

        public delegate void PowerStatusChanged();
        public event PowerStatusChanged CreatedbBackupManually;
        BAL.LogitInstance instance;

        public DBMaintenance(BAL.LogitInstance instance)
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (CreatedbBackupManually != null)
            {
                CreatedbBackupManually();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
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
                    instance.SystemProperties.backuplocation = files + @"\";
                    label7.Text = instance.SystemProperties.backuplocation;
                    instance.DataLink.SubmitChanges();
                }
            }
            catch (Exception m)
            {

                MessageBox.Show(m.Message);
            }


        }
    }
}
