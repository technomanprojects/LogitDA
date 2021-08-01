using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class P_Databases
    {
        private OleDbConnection conn = new OleDbConnection();
        private readonly string location;
        private readonly string Path;

        public P_Databases(string path)
        {
            Path = path;
            
            location = path + "\\Plotter.mdb";
            conn.ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source="
                + location + ";Jet OLEDB:Database Password=M@$ter321;";
            this.CreateBackup();
        }

        private bool CheckDatabaseDate()
        {
            DateTime dtCurrent = DateTime.Now;
            DateTime dtOld = File.GetCreationTime(location);
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

        public void CreateBackup()
        {
            string filepath = Path;
            DateTime dtCurrent = DateTime.Now;
            DateTime dtOld = Directory.GetCreationTime(filepath);
            string dirpath = Path + "\\Backups\\" + dtCurrent.ToString("MMMM yyyy");
            _ = new DateTime(2006, 01, 01);

            if (CheckDatabaseDate() && (!Directory.Exists(dirpath)))
            {
                dirpath = Path + "\\Backups\\" + dtOld.ToString("MMMM yyyy");
                Directory.CreateDirectory(dirpath);
                File.Copy(Path + "\\Plotter.mdb", dirpath + "\\Plotter.mdb", true);
                //this.DeleteRecord();
                File.SetCreationTime(Path + "\\Plotter.mdb", dtCurrent);
                File.SetCreationTime(dirpath + "\\Plotter.mdb", dtOld);
            }
        }

        public void CWInsertIntoConfig(ConfigPlotter objConfig)
        {
            DateTime lastscan = new DateTime(1899, 12, 31, 23, 59, 59);

            conn.Open();
            using (OleDbCommand cmdInsert = conn.CreateCommand())
            {
                cmdInsert.CommandText =
                    "INSERT INTO CONFIG (CONFIG.ID, CONFIG.Channel_id, CONFIG.Port_No, CONFIG.Location, CONFIG.Instrument, CONFIG.Interval, CONFIG.Upper_Limit, CONFIG.Lower_Limit, CONFIG.Upper_Range, CONFIG.Lower_Range, CONFIG.Device_Type) " +
                    "VALUES('" + objConfig.ID + "','" + objConfig.Channel_id + "','" + objConfig.Port_No + "','" + objConfig.Location + "','" + objConfig.Instrument + "','"
                               + objConfig.Interval + "','" + objConfig.Upper_Limit + "','" + objConfig.Lower_Limit + "','" + objConfig.Upper_Range + "','" + objConfig.Lower_Range + "','" + objConfig.Device_Type + "')";
                cmdInsert.ExecuteNonQuery();
            }
            conn.Close();
        }

        public void InsertRecords(Log InsertData)
        {

            try
            {
                conn.Open();
                using (OleDbCommand cmdInsert = conn.CreateCommand())
                {
                    cmdInsert.CommandText =
                    "INSERT INTO RECODE1 (DeviceID, Channel_ID, Port_Id, _Data, date_ )" +
                    "VALUES('" + InsertData.DeviceID + "','" + InsertData.Channel_ID + "'," + InsertData.Port_Id + ", " + InsertData._Data + ", '" + InsertData.date_ + "')";

                    cmdInsert.ExecuteNonQuery();
                }
                conn.Close();
            }
            catch (Exception)
            {

                conn.Close();
            }

        }

        public class ConfigPlotter
        {
            public string ID;
            public string Channel_id;
            public string Port_No;
            public string Location;
            public string Instrument;
            public string Interval;
            public string Upper_Limit;
            public string Lower_Limit;
            public string Upper_Range;
            public string Lower_Range;
            public string Device_Type;
        }

        public class Log
        {
            public string DeviceID;
            public string Channel_ID;
            public double Port_Id;
            public uint _Data;
            public DateTime date_;
        }
    }
}
