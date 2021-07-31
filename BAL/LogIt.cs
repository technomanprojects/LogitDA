using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using System.Linq;



namespace BAL
{
    public delegate void LogIts(Guid DeviceID, string ChannelId, string Port_No, double _data);
    public delegate void LogItlastRecord(Guid Id, DateTime dt);
    public delegate void RealTimesS(uint Index, double values);
    public delegate void RealTimesD(string DeviceID, uint Index, double values);
    public delegate void RealTimesChart(LogIt logItObject);
    public delegate void AlarmConditions(bool isLimitExceed);
    public delegate void IsAlaramCondition(LogIt.Parameters P, string name, double values, bool isactive, uint Index, string read, string remakr, List<string> Email, List<string> SMS);
    public delegate void InsertAcknoDelegateTMP(Guid id, Guid Device_ID, string Location, string Instrument, string Event_Type, DateTime Event_DateTime, string Event, string Value);
 
    //[DebuggerNonUserCode()]
    public class LogIt : System.ComponentModel.Component
    {
        public static event LogIts Logging;
        public static event RealTimesChart RealTime;

        public static event LogItlastRecord LastRecord;

        public static uint index = 0;
        private bool rhActive = true;
        private string sDType = null;
        private string sDeviceID = null;
        private string sLocation = null;
        private string sInstrument = null;
        private int iInterval = 0;
        private bool isLoggingEnable = false;
        private bool isDeviceEnable = true;
        private bool isAlarmEnable = true;
        public bool isLogged = true;
        private DateTime dtLastScan;
        private DateTime dtTimeOut = DateTime.Now;
        private Parameters[] parameter;
        private System.Windows.Forms.Timer LoggingTimer = new System.Windows.Forms.Timer();
        
        private static System.Windows.Forms.Timer SMS_Timer = new System.Windows.Forms.Timer();
        public static event EventHandler SendAlarmSMS;
        public static event EventHandler SendAlarmCondition;
        private static DateTime AlarmStartTiming = new DateTime();
        private static TimeSpan AlarmDuration = new TimeSpan();
        private static List<LogIt> deviceCreated = new List<LogIt>();
        public delegate void BarAlaramCondition(uint Index, double values, bool isactive);
   

        [DllImport("kernel32.dll", EntryPoint = "Beep", SetLastError = true)]
        static extern int Beep(uint dwFreq, uint dwDuration);
        //Clsmgt dm = new Clsmgt();

        #region Properties

        public string Channedl_ID
        { get; set; }


       


        public bool RhActive
        {
            get { return rhActive; }
            set { rhActive = value; }
        }

        public string DeviceType
        {
            get { return sDType; }
            set { sDType = value; }
        }
        public string Port_No
        {
            get { return sDeviceID; }
            set { sDeviceID = value; }
        }
        public string Location
        {
            get { return sLocation; }
            set { sLocation = value; }
        }
        public string Instrument
        {
            get { return sInstrument; }
            set { sInstrument = value; }
        }
        public int Interval
        {
            get { return iInterval; }
            set
            {
                iInterval = value;
            }
        }

        public bool IsDeviceEnable
        {
            get { return isDeviceEnable; }
            set { isDeviceEnable = value; }
        }
        public bool IsLoggingEnable
        {
            get { return isLoggingEnable; }
            set { isLoggingEnable = value; }
        }
        public bool IsAlarmEnable
        {
            get { return isAlarmEnable; }
            set { isAlarmEnable = value; }
        }

        public DateTime LastScan
        {
            get { return dtLastScan; }
        }

        public Parameters[] Parameter
        {
            get
            {
                return parameter;
            }
            set
            {
                parameter = value;
            }
        }

        public static sbyte Time { get; set; }

        #endregion

        protected override void Dispose(bool disposing)
        {
            foreach (Parameters p in parameter)
            {
                if (p != null)
                {
                    p.Dispose();
                }
               
            }
            LoggingTimer.Stop();
            
            deviceCreated.Clear();
            base.Dispose(disposing);
        }
         
        static LogIt()
        {
            AlarmStartTiming = DateTime.Now;
          
        }

        public LogIt(string DeviceType, string DeviceID, string Location,
            string Instrument, int Interval, DateTime LastScan,
            bool AlarmEnable, bool LoggingEnable, bool RhActive)
        {
            this.DeviceType = DeviceType;

            this.Port_No = DeviceID;
            this.Location = Location;
            this.Instrument = Instrument;
            this.Interval = Interval;
            this.dtLastScan = LastScan;
            this.IsAlarmEnable = AlarmEnable;
            this.isLoggingEnable = LoggingEnable;
            this.rhActive = RhActive;

            if (this.sDType == "01")
            {
                parameter = new Parameters[2];
                parameter[0] = new Parameters("Temperature", sDeviceID, AlarmEnable,0);
                parameter[1] = new Parameters("Humidity", sDeviceID, AlarmEnable,0);
                parameter[0].Index = index++;
                parameter[1].Index = index++;

            }
            else if (this.sDType == "02")
            {
                parameter = new Parameters[1];
                parameter[0] = new Parameters("Temperature", sDeviceID, AlarmEnable,0);
                parameter[0].Index = index++;
            }
            LoggingTimer.Tick += new EventHandler(LoggingTimer_Tick);
            LoggingTimer_Tick(this, new EventArgs());
            LoggingTimer.Interval = 5000;
            LoggingTimer.Start();
            //Parameters.OutOfLimit += new AlarmConditions(Parameters_OutOfLimit);
            LogIt.deviceCreated.Add(this);
           
        }

        public LogIt(DAL.Device_Config Device, int interval, List<string> Email, List<string> SMS)
        {
            this.ID = Device.ID; ;
            this.Port_No = Device.Port_No.ToString();
            this.Location = Device.Location;
            this.Instrument = Device.Instrument;
            this.Interval = (int)Device.Interval;
            this.Channedl_ID = Device.Channel_id;

            this.DeviceType = Device.Device_Type.ToString();
          
            if (Device.Last_Record != null )
            {
                this.dtLastScan = Device.Last_Record.Value;
           
            }
            this.isLoggingEnable = (bool)Device.Active;
           
            parameter = new Parameters[1];




            switch (Device.Device_Type)
                {
                    case 1:
                        parameter[0] = new Parameters("Temperature", Device.Device_Type.ToString(), true,interval);

                         break;
                    case 2:

                         parameter[0] = new Parameters("Humidity", Device.Device_Type.ToString(), true, interval);
                          
                        
                        
                        break;
                    case 3:
                        parameter[0] = new Parameters("Pressure", Device.Device_Type.ToString(), true, interval);
                      
                        
                        break;

                   
                }
                parameter[0].Index = index;
                parameter[0].SMS = SMS;
                parameter[0].Email = Email;
                parameter[0].Device_Type = (int)Device.Device_Type;
                parameter[0].LowerLimit = (double)Device.Lower_Limit;
                parameter[0].UpperLimit = (double)Device.Upper_Limit;
                parameter[0].LowerRange = (double)Device.Lower_Range;
                parameter[0].UpperRange = (double)Device.Upper_Range;
                parameter[0].GUIDID = Device.ID;
                if (Device.Offset!= null)
                {
                    parameter[0].Offset = (double)Device.Offset;

                }
                parameter[0]._logit = this;
                index++;
               
            
            LoggingTimer.Tick += new EventHandler(LoggingTimer_Tick);
            LoggingTimer_Tick(this, new EventArgs());
            LoggingTimer.Interval = 5000;
            LoggingTimer.Start();
            LogIt.deviceCreated.Add(this);
            

        }
        
        private static void AlarmTimer_Tick(object sender, EventArgs e)
        {
            //bool result = false;
            //List<string> AlarmList = new List<string>();
            //foreach (LogIt logitObject in LogIt.deviceCreated)
            //{
            //    foreach (Parameters p in logitObject.parameter)
            //    {
            //        if (p!= null)
            //        {
            //            result = result | p.OutofLimit;
            //            if (p.OutofLimit)
            //            {
            //                //SendAlarmSMS(sender, e); 
                         
            //            }
            //        }
                    
            //    }
            //}
            //if (result)
            //{
            //    //Beep(1000, 2000);

            //    AlarmDuration = DateTime.Now.Subtract(AlarmStartTiming);                    // This is only for Roche               
            //        if ((AlarmDuration.TotalMinutes >= Time) && (SendAlarmCondition != null))   // This is only for Roche
            //    {
            //        sender = AlarmList;
            //        SendAlarmCondition(sender, e);                                          // This is general.
                  
            //    }
            //}
            //else
            //{
            //    AlarmStartTiming = DateTime.Now;
            //  //  SendAlarmSMS(sender, e);                                                       // This is general.
            //}
        }
          

        private void LoggingTimer_Tick(object sender, EventArgs e)
        {
            DateTime dt1 = dtLastScan.AddMinutes(iInterval);
            dt1 = dt1.AddSeconds(-dt1.Second);
            DateTime dt2 = DateTime.Now;
            TimeSpan ts = dt1.Subtract(dt2);
            if (ts.TotalMinutes <= 0)
            {
                if (Logging != null && !(isLogged))
                {

                    Logging(parameter[0].GUIDID, Channedl_ID, Port_No, parameter[0].ParameterValue);
                  

                    if (LastRecord != null)
                    {
                        dtLastScan = DateTime.Now;
                        LastRecord(this.ID, dtLastScan);
                    }

                    isLogged = true;
                }
            }
        }
        public void ExplicitLogging()
        {
              Logging(parameter[0].GUIDID, Channedl_ID, Port_No, parameter[0].ParameterValue);

            dtLastScan = DateTime.Now;
        }

        public void LaunchRealTime()
        {
            if (RealTime != null)
                RealTime(this);
        }

        //[DebuggerNonUserCode()]
        public class Parameters : System.ComponentModel.Component
        {
            public static event RealTimesS Output1;
            public static event AlarmConditions OutOfLimit;
            public static event IsAlaramCondition outofLimits;
            public static event InsertAcknoDelegateTMP InsertAckTMP;
            public static event BarAlaramCondition BarAlaram;
            internal BAL.LogIt _logit;
            Stopwatch limitsw;
            public bool isMailSend;
            #region Fields
            private Guid ID;
            private string name = "";
            private string sDeiveID = "";
            private double paravalue = 0;
            private double lowerlimit = 0;
            private double upperlimit = 0;
            private double lowerrange = 0;
            private double upperrange = 0;
            private double offset = 0;
            private bool isoutlimit = false;
            private bool isalarmenable = false;
            private uint index = 0;
            private DateTime newTime;
            private DateTime lastTime = DateTime.Now;
            private int _interval = 0;
            #endregion

            #region Properties
            public string Name
            {
                get { return name; }
            }
            public int Device_Type
            { get; set; }

            public Guid GUIDID { get; set; }

            public string Limite { get; set; }

            public string LocationObj { get; set; }
            public string Instrument { get; set; }

            public double ParameterValue
            {
                get { return paravalue; }
                set
                {
                    paravalue = value + offset;
                    Output1(index, paravalue);
                    newTime = DateTime.Now;
                    double d = newTime.Subtract(lastTime).TotalSeconds;

                    if (d >= 1)
                    {
                        lastTime = DateTime.Now;
                    }
                    if (paravalue >= upperlimit || paravalue <= lowerlimit)
                    {
                        this.isoutlimit = true;
                        if (!limitsw.IsRunning)
                        {
                            limitsw.Start();
                            //System.Diagnostics.Debug.Print("Timer Start");
                        }
                        if (limitsw.Elapsed.Minutes >= _interval)
                        {

                            if (paravalue > upperlimit)
                            {

                                if (!isMailSend)
                                {
                                    outofLimits(this, this._logit.Instrument, paravalue, true, index, paravalue.ToString(), "High", Email, SMS);

                                    Guid id = Guid.NewGuid();

                                    switch (this._logit.DeviceType)
                                    {
                                        case "1":
                                            InsertAckTMP(id, this._logit.ID, this._logit.Location, this._logit.Instrument, "Temperature", DateTime.Now, "High", paravalue.ToString());
                                            break;
                                        case "2":
                                            InsertAckTMP(id, this._logit.ID, this._logit.Location, this._logit.Instrument, "Humidity", DateTime.Now, "High", paravalue.ToString());
                                            break;
                                        case "3":
                                            InsertAckTMP(id, this._logit.ID, this._logit.Location, this._logit.Instrument, "Pressure", DateTime.Now, "High", paravalue.ToString());
                                            break;

                                    }                                    

                                   // AlarmLimit = Guid.NewGuid();
                                   // object[] strArry = new object[] { AlarmLimit, this._DeviceID, this._SensiorID, this.combineID, this.Instrument, this.LocationObj, this.Name, "High", dt_LastRecord };

                                    //duration of alarm
                                   // OutOfLimit(strArry);
                                    isMailSend = true;

                                   //// if (!islogAlaram)
                                   // {
                                   //     islogAlaram = true;
                                   // }
                                }
                            }
                            if (paravalue < lowerlimit)
                            {
                                if (!isMailSend)
                                {
                                    outofLimits(this, this._logit.Instrument, paravalue, true, index, paravalue.ToString(), "High", Email, SMS);

                                    Guid id = Guid.NewGuid();

                                    switch (this._logit.DeviceType)
                                    {
                                        case "1":
                                            InsertAckTMP(id, this._logit.ID, this._logit.Location, this._logit.Instrument, "Temperature", DateTime.Now, "Low", paravalue.ToString());
                                            break;
                                        case "2":
                                                InsertAckTMP(id, this._logit.ID, this._logit.Location, this._logit.Instrument, "Humidity", DateTime.Now, "Low", paravalue.ToString());
                                            break;
                                        case "3":
                                            InsertAckTMP(id, this._logit.ID, this._logit.Location, this._logit.Instrument, "Pressure", DateTime.Now, "Low", paravalue.ToString());
                                            break;

                                    }

                                    // AlarmLimit = Guid.NewGuid();
                                    // object[] strArry = new object[] { AlarmLimit, this._DeviceID, this._SensiorID, this.combineID, this.Instrument, this.LocationObj, this.Name, "High", dt_LastRecord };

                                    //duration of alarm
                                    // OutOfLimit(strArry);
                                    isMailSend = true;

                                    //// if (!islogAlaram)
                                    // {
                                    //     islogAlaram = true;
                                    // }
                                }
                            }

                            limitsw.Restart();
                        }

                        if (BarAlaram != null)
                        {
                            BarAlaram(index, paravalue, true);
                        }
                    }                   
                    else
                    {
                        this.isoutlimit = false;
                        if (paravalue >= lowerlimit && paravalue <= upperlimit)
                        {
                            this.Limite = "Normal";


                            if (isMailSend)
                            {
                                outofLimits(this, this._logit.Instrument, paravalue, false, index, paravalue.ToString(), (string)Limite.Clone(), Email, SMS);
                                Guid id = Guid.NewGuid();
                                switch (this._logit.DeviceType)
                                {
                                    case "1":
                                        InsertAckTMP(id, this._logit.ID, this._logit.Location, this._logit.Instrument, "Temperature", DateTime.Now, "Normal", paravalue.ToString());
                                        break;
                                    case "2":
                                        InsertAckTMP(id, this._logit.ID, this._logit.Location, this._logit.Instrument, "Humidity", DateTime.Now, "Normal", paravalue.ToString());
                                        break;
                                    case "3":
                                        InsertAckTMP(id, this._logit.ID, this._logit.Location, this._logit.Instrument, "Pressure", DateTime.Now, "Normal", paravalue.ToString());
                                        break;

                                }
                                isMailSend = false;

                            }


                            if (limitsw.IsRunning)
                            {
                                limitsw.Stop();
                                System.Diagnostics.Debug.Print("Timer Stop");
                            }



                            if (BarAlaram != null)
                            {
                                BarAlaram(index, paravalue, false);
                            }
                        }

                        this.Limite = string.Empty;
                        //this.isoutlimit = false;
                        //if (OutOfLimit != null)
                        //    OutOfLimit(false);
                        //if (outofLimits != null)
                        //{
                        //    outofLimits(index, paravalue, false);
                        //}
                        //this.Limite = string.Empty;
                    }
                }
            }

          
            public string SensorID { get; set; }
            public double LowerLimit
            {
                get { return lowerlimit; }
                set { lowerlimit = value; }
            }
            public double UpperLimit
            {
                get { return upperlimit; }
                set { upperlimit = value; }
            }
            public double LowerRange
            {
                get { return lowerrange; }
                set { lowerrange = value; }
            }
            public double UpperRange
            {
                get { return upperrange; }
                set { upperrange = value; }
            }
            public double Offset
            {
                get { return offset; }
                set { offset = value; }
            }
            public bool OutofLimit
            {
                get { return isoutlimit; }
            }
            public uint Index
            {
                get
                {
                    return index;
                }
                set
                {
                    index = value;
                }
            }
            #endregion

            #region Methods
            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
            }

            #endregion

            #region EventHandler

            #endregion

            #region Constructor

            public Parameters(string name, string sDeviceID, bool IsAlarmEnable, int interval)
            {
                this._interval = interval;
                this.name = name;
                this.sDeiveID = sDeviceID;
                this.isalarmenable = IsAlarmEnable;
                limitsw = new Stopwatch();
            }
            #endregion


            public List<string> SMS { get; set; }

            public List<string> Email { get; set; }
        }

        public Guid ID { get; set; }
    }
}