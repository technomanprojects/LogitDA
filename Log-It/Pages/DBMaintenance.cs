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
using Log_It.Forms;
using DAL;

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
                    SYSProperty Systproperties = instance.DataLink.SYSProperties.SingleOrDefault(x => x.ID == 1);
                    String mess = string.Empty;
                    bool check = false;
                    if (Systproperties.backuplocation != files + @"\")
                    {
                        mess = Systproperties.backuplocation + " to " + files + @"\";
                        check = true;
                    }
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
                    if (check == true)
                    {
                        Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Database Backup Location " + mess.ToString() + " has changed.", instance.UserInstance.Full_Name);
                    }

                }
            }
            catch (Exception m)
            {

                MessageBox.Show(m.Message);
            }


        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog file = new OpenFileDialog())
                {
                    file.Filter = "Backup File (*.bak)|*.bak|All files (*.*)|*.*";
                    if (file.ShowDialog() == DialogResult.OK)
                    {
                        DialogResult result = MessageBox.Show("Do you want to Delete old Database", "Confirmation", MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            String filePath = file.FileName;

                            String query = "IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'" + Path.GetFileNameWithoutExtension(filePath) + "') DROP DATABASE " + Path.GetFileNameWithoutExtension(filePath) + " RESTORE DATABASE " + Path.GetFileNameWithoutExtension(filePath) + " FROM DISK = '" + filePath + "'";
                            SqlConnection Conn = new SqlConnection(instance.ConntectionLink);
                            SqlCommand cmd = new SqlCommand(query, Conn);
                            if (Conn.State == ConnectionState.Closed)
                            {
                                Conn.Open();
                            }

                            cmd.ExecuteNonQuery();
                            Conn.Close();
                            MessageBox.Show("Restore Database Successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "Database has Restored Sucessfully.", instance.UserInstance.Full_Name);
                        }
                        else
                        {
                            MessageBox.Show("Database Restore Process cancel", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
