using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Local
{
    public class Database
    {
        private OleDbConnection conn = new OleDbConnection();
        private static Database instance = null;
        private int totalDevices = 0;
        private int totalChannel = 0;
        private string usrname = "";
        string path = null;
        string dbName = "\\DAPlotter.mdb";
        private Database(string Path)
		{
            
            path = Path + dbName;

			    conn.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=" 
				    + path + ";Jet OLEDB:Database Password=pchart;";
			    this.CreateBackup();
			GetTotalDevice();			
		}

        public void CreateBackup()
        {
            string filepath = path + dbName;
            DateTime dtCurrent = DateTime.Now;
            DateTime dtOld = Directory.GetCreationTime(filepath);
            string dirpath = path + "\\Backups\\" + dtCurrent.ToString("MMMM yyyy");
            DateTime tempy = new DateTime(2006, 01, 01);
            if (CheckDatabaseDate() && (!Directory.Exists(dirpath)))
            {
                dirpath = path + "\\Backups\\" + dtOld.ToString("MMMM yyyy");
                Directory.CreateDirectory(dirpath);
                File.Copy(path + "\\Plotter.mdb", dirpath + dbName, true);
                this.DeleteRecord();
                File.SetCreationTime(path + dbName, dtCurrent);
                File.SetCreationTime(dirpath + dbName, dtOld);
            }
        }

        private void DeleteRecord()
        {
            conn.Open();
            using (OleDbCommand cmdDelete = conn.CreateCommand())
            {
                cmdDelete.CommandText = "DELETE FROM RECODE1";
                cmdDelete.ExecuteNonQuery();
                cmdDelete.CommandText = "DELETE FROM RECODE2";
                cmdDelete.ExecuteNonQuery();
            }
            conn.Close();
        }

        private bool CheckDatabaseDate()
        {
            DateTime dtCurrent = DateTime.Now;
            DateTime dtOld = File.GetCreationTime(path + dbName);
            if (dtCurrent.Year > dtOld.Year)
            {
                return true;
            }
            else if (dtCurrent.Year == dtOld.Year)
            {
                if ((dtCurrent.Month - dtOld.Month) >= 1)
                    return true;
            }
            return false;
        }

        public void GetTotalDevice()
        {
            OleDbDataReader rdr;
            conn.Open();
            using (OleDbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = "SELECT COUNT(CHANNELID) FROM CONFIG";
                rdr = cmd.ExecuteReader();
                rdr.Read();
                totalDevices = rdr.GetInt32(0);
                rdr.Close();
            }
            conn.Close();
        }
    }
}
