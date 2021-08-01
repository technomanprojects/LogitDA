using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using DAL;

namespace BAL
{
    public enum HardwareType
    {
        AO,
        DT,
        DA
    }
    public class LogitInstance
    {
        private string p;

        public PlotterDataContext DataLink { get; set; }

        public User UserInstance {get;set;}
        public SYSProperty SystemProperties { get; set; }

        public LogitInstance()
        {

            DataLink = new PlotterDataContext();
            _ = DataLink.Connection.ConnectionString;

        }

        public LogitInstance(string connectionstringDb)
        {
            DataLink = new PlotterDataContext();
            //string str = "Data Source=IT-PC\\SQLEXPRESS;Initial Catalog=Plotter;Persist Security Info=True;User ID=sa;Password=reloaded";
            string str = connectionstringDb;
            DataLink.Connection.ConnectionString = str;
            _ = DataLink.Connection.ConnectionString;
            this.ConntectionLink = str;
        }


     

        public IQueryable<User> Users
        {
            get
            {
                DataLink.Refresh(System.Data.Linq.RefreshMode.OverwriteCurrentValues, DataLink.Users);
                return DataLink.Users;
            }
        }
       
        public IQueryable<Email> Emails
        {
            get
            {
                DataLink.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, DataLink.Emails);
                return DataLink.Emails;
            }
        }

        public IQueryable<Company> Companiess
        {
            get
            {
                DataLink.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, DataLink.Companies);
                return DataLink.Companies;
            }
        }
        public IQueryable<Device_Config> Device_Configes
        {
            get
            {
                DataLink.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, DataLink.Device_Configs);
                return DataLink.Device_Configs;
            }
        }
        
        public IQueryable<Log> Logs
        {
            get
            {
                DataLink.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, DataLink.Logs);
                return DataLink.Logs;
            }
        }
        public IQueryable<ofsetAuditRecord> ofsetAuditRecords
        {
            get
            {
                DataLink.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, DataLink.ofsetAuditRecords);
                return DataLink.ofsetAuditRecords;
            }

        }


        public int RefresUsers
        {
            get
            {
                DataLink.Refresh(System.Data.Linq.RefreshMode.KeepCurrentValues, DataLink.Users);

                return 0;
            }
        }

        public string ConntectionLink { get; set; }

        public User UserReport { get; set; }
    }
}