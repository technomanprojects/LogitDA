﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using System.Data.Linq;

namespace BAL
{
    public class Logit_Device
    {
        LogitInstance _instance;
        public Logit_Device(LogitInstance instance)
        {
            _instance = instance;
        }
       
        public bool VerificationDeviceid(Guid s)
        {
            if (_instance.DataLink.Device_Configs.Count(x => x.ID  == s) > 0)
            {
                return false;
            }
            
            return true;
        }
        public bool VerificationChannelid(string s)
        {
            if (_instance.DataLink.Device_Configs.Count(x => x.Channel_id == s) > 0)
            {
                return false;
            }

            return true;
        }
        public bool Verificationlocation(string s)
        {
            if (_instance.DataLink.Device_Configs.Count(x => x.Location == s) > 0)
            {
                return false;
            }

            return true;
        }

        public void InsertRecord(DAL.Log log )
        {
            _instance.DataLink.Logs.InsertOnSubmit(log);
            _instance.DataLink.SubmitChanges(ConflictMode.FailOnFirstConflict);
        }

        public int Add(Device_Config device)
        {
            try
            {
                _instance.DataLink.Device_Configs.InsertOnSubmit(device);
                //IQueryable<LimitTable> limits = _instance.DataLink.LimitTables.Where(x => x.Device_id == device.ID);
                //if (limits.Count() > 0)
                //{
                //    foreach (var item in limits)
                //    {
                //        _instance.DataLink.LimitTables.InsertOnSubmit(item);
                //    }
                //}
                _instance.DataLink.SubmitChanges(ConflictMode.FailOnFirstConflict);
                return 0;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public int Update(Device_Config new_device, Device_Config current_device)
        {
            Device_Config _device = _instance.DataLink.Device_Configs.SingleOrDefault(x => x.ID == current_device.ID  && current_device.IsRowActive == true);
            _device.ModifiedBy = new_device.CreatedBy ;
            _device.ModifiedDateTime = DateTime.Now;
            _device.IsRowActive = false;
            this.Add(new_device);
            //_instance.DataLink.Device_Configs.InsertOnSubmit(_device);
            //this.Clone(current_device);
            return 0;
 
        }
        public int Update(Device_Config new_device)
        {
            _instance.DataLink.SubmitChanges();
            //this.Clone(current_device);
            return 0;

        }
        void Clone(Device_Config device)
        {
            try
            {
                Device_Config d = new Device_Config()
                {
                    ID = Guid.NewGuid(),
                    Channel_id = device.Channel_id,
                    Location = device.Location,
                    Active = device.Active,
                     
                    Instrument = device.Instrument,
                    Interval = device.Interval,
                    Last_Record = device.Last_Record,
                    IsRowActive = device.IsRowActive,
                    CreateDateTime = device.CreateDateTime,
                    CreatedBy = device.CreatedBy,
                    dateofCalibration= device.dateofCalibration,
                    Device_Type = device.Device_Type,
                    higher = device.higher,
                    Lower = device.Lower,
                    Lower_Limit = device.Lower_Limit,
                    Lower_Range = device.Lower_Range,
                    ModifiedBy = device.ModifiedBy,
                    ModifiedDateTime = device.ModifiedDateTime,
                    Port_No = device.Port_No,
                   Offset = device.Offset,
                    Upper_Limit = device.Upper_Limit,
                    Upper_Range = device.Upper_Range
                };

            }
            catch (Exception)
            {

                throw;
            }

        }
        public Device_Config GetbyDeviceGUDID(Guid id)
        {
            try
            {
                return _instance.DataLink.Device_Configs.SingleOrDefault(x => x.ID == id && x.IsRowActive == true);
            }
            catch (Exception)
            {
                
                throw;
            }
            
        }
        public Device_Config GetbyDeviceID(int id)
        {
            try
            {
                return _instance.DataLink.Device_Configs.SingleOrDefault(x => x.Port_No == id && x.IsRowActive == true);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public IQueryable<Device_Config> GetAllDevices()
        {
            try
            {
                
                return _instance.DataLink.Device_Configs.Where(x => x.IsRowActive == true && x.Active == true);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IQueryable<Device_Config> GetActiveDevice()
        {
            try
            {
                return _instance.DataLink.Device_Configs.Where(x => x.IsRowActive == false && x.Active == true);
            }
            catch (Exception)
            {

                throw;
            }
        }

     
        public int GetCount()
        {
            return _instance.Device_Configes.Count(x => x.Active == true && x.IsRowActive == true);
        }

        public int DeviceCount()
        {
            try
            {
                if (_instance.DataLink.Device_Configs.Count(x => x.IsRowActive == true) > 0 )
                {
                    return _instance.DataLink.Device_Configs.Count(x => x.IsRowActive == true);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }

    }
}
