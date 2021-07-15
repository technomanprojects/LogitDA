﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BAL;
using System.Diagnostics;

namespace Log_It.Pages
{
    public partial class TVView : UserControl
    {
        public delegate void FormClose();
        public event FormClose close;

        Log_It.CustomControls.TVControl tv;
        Logit_Device config;
        private static TVView instance = null;
        private BAL.LogitInstance Dbinstance = null;
        private Log_It.CustomControls.TVControl[] tvs = null;

        public TVView(object o, BAL.Logit_Device config, BAL.LogitInstance Dbinstance)
        {
            InitializeComponent();
            this.config = config;
            this.Dbinstance = Dbinstance;
            this.CreateTVObjects(o);
        }
        public static TVView Instance(object o, BAL.Logit_Device config, BAL.LogitInstance Dbinstance)
        {
            if (instance == null)
                instance = new TVView(o, config, Dbinstance);
            return instance;
        }

        public Log_It.CustomControls.TVControl CreateTV(string caption)
        {
            try
            {

                tv = new Log_It.CustomControls.TVControl(Dbinstance);
                System.Windows.Forms.TabPage page = new TabPage(caption);
                page.Controls.Add(tv);
                tab.TabPages.Add(page);
                page.BringToFront();
                tab.BringToFront();
               
            }
            catch (Exception m)
            {

                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");

            }
            return tv;
        }
        public Log_It.CustomControls.TVControl[] TVs
        {
            get
            {
                return this.tvs;
            }

        }
        private void CreateTVObjects(object o)
        {
            try
            {

                LogIt[] channel = (LogIt[])o;
                if (channel != null)
                {
                    tvs = new Log_It.CustomControls.TVControl[channel.Length];
                    for (int i = 0; i < channel.Length; i++)
                    {
                        tvs[i] = CreateTV(channel[i].Location);
                        tvs[i].logitObj = channel[i];
                        tvs[i].Chart.Parameters = (Log_It.CustomControls.Chart.NumberSquence)Enum.Parse(typeof(Log_It.CustomControls.Chart.NumberSquence), channel[i].DeviceType);
                        switch ( tvs[i].Chart.Parameters)
                        {
                            case Log_It.CustomControls.Chart.NumberSquence.Temperature:
                                tvs[i].TypeChart = CustomControls.Chart.NumberSquence.Temperature;

                                
                                tvs[i].TankT.Unit = "°C";
                                break;
                            case Log_It.CustomControls.Chart.NumberSquence.Humidity:
                                tvs[i].TankT.Unit = "%";
                                tvs[i].TypeChart = CustomControls.Chart.NumberSquence.Humidity;
                                break;
                            case Log_It.CustomControls.Chart.NumberSquence.Pressure:
                                tvs[i].TankT.Unit = "Pa";
                                tvs[i].TypeChart = CustomControls.Chart.NumberSquence.Pressure;
                                break;                         
                        }    
                    }

                    for (int i = 0; i < config.GetAllDevices().Count(); i++)
                    {
                        tvs[i].TempLowerLimit = (float)channel[i].Parameter[0].LowerLimit;
                        tvs[i].TempUpperLimit = (float)channel[i].Parameter[0].UpperLimit;
                        tvs[i].TempLowerRange = (float)channel[i].Parameter[0].LowerRange;
                        tvs[i].TempUpperRange = (float)channel[i].Parameter[0].UpperRange;
                        tvs[i].TankT.Caption = channel[i].Location;
                        tvs[i].TankT.Tag = channel[i].Port_No.ToString();
                     
                    }
                }
            }
            catch (Exception m)
            {

                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");

            }
        }

        private System.Windows.Forms.TabControl.TabPageCollection TabPages
        {
            get
            {
                return this.tab.TabPages;
            }

        }

        public void RealTimeData(LogIt logItObject)
        {
            try
            {

                for (int i = 0; i < tvs.Length; i++)
                {
                    if (logItObject.Port_No == tvs[i].logitObj.Port_No)
                    {
                        tvs[i].RealTimeData(logItObject);
                        Console.WriteLine(i);
                    }

                }
            }
            catch (Exception m)
            {

                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");

            }
        }

        private void tab_VisibleChanged(object sender, EventArgs e)
        {
            
        }

        private void TVView_VisibleChanged(object sender, EventArgs e)
        {
            try
            {

                instance = null;
                tv = null;

                if (close != null)
                {

                    close();

                }
            }
            catch (Exception m)
            {

                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");

            }
        }


    }
}
