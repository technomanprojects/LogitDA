using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BAL;
using DAL;
using Microsoft.Win32;
using System.Diagnostics;
using System.Threading;
using System.Net.Mail;
using System.Net;

namespace Log_It.Pages
{
    public enum D_Type
    {
        Aosong =1,
        Acquisition=2
    }

    public partial class ApplicationProperties : ControlPage
    {
        public BAL.LogitInstance instance { get; set; }
        public bool systemset = false;
        string username;
        public D_Type d_Type =  D_Type.Acquisition;

               public delegate void RefreshProperties();
        public event RefreshProperties RefresPageProperties;
        public ApplicationProperties(BAL.LogitInstance instance,string username )
        {
            InitializeComponent( );
            d_Type = D_Type.Acquisition;
            this.instance = instance;
            this.username = username;
            this.RefreshPage();
        }

        public override void RefreshPage()
        {
            try
            {
                base.RefreshPage();
                if (instance.DataLink.SYSProperties.Count() > 0)
                {
                    SYSProperty Sysproperties = instance.DataLink.SYSProperties.SingleOrDefault(x => x.ID == 1);
                    textBoxIPAddress.Text = Sysproperties.IP_Address.ToString();
                    comboBoxUnit.Text = Sysproperties.Unit;
                    checkBoxSignLine.Checked = (bool)Sysproperties.Signature;
                    checkBoxSignLogged.Checked = (bool)Sysproperties.Automatic_Sign;
                    textBoxIPAddress.Text = Sysproperties.IP_Address.ToString();
                    textBoxBaudRate.Text = Sysproperties.Port1.ToString();
                    textBoxDataBit.Text = Sysproperties.Port2.ToString();
                 
                    if (Sysproperties.D_Type == 1)
                    {
                        d_Type = D_Type.Aosong;
                        radioButton1.Checked = true;
                        numericUpDown1.Value =(decimal) Sysproperties.Number_Devices;
                       // radioButton1_CheckedChanged(radioButton1, null);

                    }
                    if (Sysproperties.D_Type == 2)
                    {
                        d_Type = D_Type.Acquisition;
                        radioButton2.Checked = true;
                        numericUpDown2.Value = (decimal)Sysproperties.Number_Devices;
                    }
                    checkBoxAlert.Checked = (bool)Sysproperties.Acknowledged;
                    checkBoxWifiAlarm.Checked = (bool)Sysproperties.AlarmEnable;
                    textBoxAlarmInterval.Text = Sysproperties.Alert_Interval.ToString();
                    if (checkBoxWifiAlarm.Checked)
                    {
                        textBoxAlarmIP.Text = Sysproperties.Alarm_IP;
                        textBoxAlarmPort.Text = Sysproperties.Alarm_Port;
                       
                    }
                    chbEmail.Checked = (bool)Sysproperties.Email;
                    if (chbEmail.Checked)
                    {
                        textBoxEmailID.Text = Sysproperties.EmailID;
                        textBoxEmailPassword.Text = Sysproperties.EmailPassword;
                        textBoxEmailPort.Text = Sysproperties.EmailPort;
                        textBoxEmailSMTP.Text = Sysproperties.EmailSMTP;
                    }
                    chbSMS.Checked = (bool)Sysproperties.SMS;
                    if (chbSMS.Checked)
                    {
                        if (Sysproperties.WebLink == true)
                        {                            
                            textBoxSMSID.Text = Sysproperties.SMSID;
                            textBoxSMSPassword.Text = Sysproperties.SMSPassword;
                            textBoxSMSSecret.Text = Sysproperties.SMSSecret;
                            textBoxSMSToken.Text = Sysproperties.SMSToken;
                        }
                    }

                    DAL.Company company = instance.DataLink.Companies.SingleOrDefault();
                    if (company != null)
                    {
                     
                        textBoxCompany.Text = company.Company_Name;


                    }


                    systemset = true;


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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            if (System.Text.RegularExpressions.Regex.IsMatch(t.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                t.Text = t.Text.Remove(t.Text.Length - 1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!systemset)
                {
                    SYSProperty Systproperties = new SYSProperty();

                    Systproperties.Unit = comboBoxUnit.Text;
                    Systproperties.Signature = checkBoxSignLine.Checked;
                    Systproperties.Automatic_Sign = checkBoxSignLogged.Checked;
                    Systproperties.IP_Address= textBoxIPAddress.Text;
                    Systproperties.Port1= textBoxBaudRate.Text;
                    Systproperties.Port2= textBoxDataBit.Text;
                    

                    Systproperties.Acknowledged = checkBoxAlert.Checked;
                    Systproperties.Alert_Interval = Convert.ToInt32(textBoxAlarmInterval.Text); 
                    if (checkBoxWifiAlarm.Checked)
                    {
                        Systproperties.Alarm_IP = textBoxAlarmIP.Text;
                        Systproperties.Alert_Interval = Convert.ToInt32(textBoxAlarmInterval.Text);
                    }
                    else
                    {
                        Systproperties.Alarm_Port = string.Empty;
                        Systproperties.Alarm_IP = string.Empty;
                    }

                    Systproperties.Email = chbEmail.Checked;
                    if (chbEmail.Checked)
                    {
                        Systproperties.EmailID = textBoxEmailID.Text;
                        Systproperties.EmailPassword = textBoxEmailPassword.Text;
                        Systproperties.EmailPort = textBoxEmailPort.Text;
                        Systproperties.EmailSMTP = textBoxEmailSMTP.Text;
                    }
                    else
                    {
                        Systproperties.EmailID = string.Empty;
                        Systproperties.EmailPassword = string.Empty;
                        Systproperties.EmailPort = null;
                        Systproperties.EmailSMTP = string.Empty;
                    }

                    Systproperties.SMS = chbSMS.Checked;
                    if (chbSMS.Checked)
                    {
                        Systproperties.SMSID = textBoxSMSID.Text;
                        Systproperties.SMSPassword = textBoxSMSPassword.Text;
                        Systproperties.SMSSecret = textBoxSMSSecret.Text;
                        Systproperties.SMSToken = textBoxSMSToken.Text;

                    }
                    instance.DataLink.SYSProperties.InsertOnSubmit(Systproperties);

                    DAL.Company company = new Company();
                    company.Id = Guid.NewGuid();
                    company.Company_Name = textBoxCompany.Text;
                  
                    instance.DataLink.Companies.InsertOnSubmit(company);

                    instance.DataLink.SubmitChanges();

                    //RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\TLSSetting");
                    //if (key == null)
                    //{
                    //    key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\TLSSetting");
                    //    key.SetValue("A1", 1);
                    //}


                    Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, " System properties has updated", instance.UserInstance.Full_Name);
                    MessageBox.Show("Data has been saved!");
                }
                else
                {
                    SYSProperty Systproperties = instance.DataLink.SYSProperties.SingleOrDefault(x => x.ID == 1);


                    Systproperties.Unit = comboBoxUnit.Text;
                    Systproperties.Signature = checkBoxSignLine.Checked;
                    Systproperties.Automatic_Sign = checkBoxSignLogged.Checked;
                    Systproperties.IP_Address = textBoxIPAddress.Text;
                    Systproperties.Port1 = textBoxBaudRate.Text;
                    Systproperties.Port2 = textBoxDataBit.Text;
                  
                    Systproperties.Power_Acknowledged = false;
                    Systproperties.Alert_Interval = Convert.ToInt32(textBoxAlarmInterval.Text);
                    Systproperties.Power_SMS = false;
                    Systproperties.AlarmEnable = checkBoxWifiAlarm.Checked;
                    if (checkBoxWifiAlarm.Checked)
                    {

                        Systproperties.Alarm_IP = textBoxAlarmIP.Text;
                        Systproperties.Alarm_Port = textBoxAlarmPort.Text;
                        
                    }
                    else
                    {
                        Systproperties.Alarm_Port = string.Empty;                        
                        Systproperties.Alarm_IP = string.Empty;
                    }
                    Systproperties.Number_Devices = (int)numericUpDown2.Value;
                    Systproperties.Email = chbEmail.Checked;
                    if (chbEmail.Checked)
                    {
                        if (textBoxEmailID.Text == string.Empty)
                        {

                            MessageBox.Show("Please provide User Name.");
                            return;
                        }
                        if (textBoxEmailPassword.Text == string.Empty)
                        {

                            MessageBox.Show("Please provide Email Password.");
                            return;
                        }
                        if (textBoxEmailSMTP.Text == string.Empty)
                        {

                            MessageBox.Show("Please provide SMTP.");
                            return;
                        }
                        if (textBoxEmailPort.Text == string.Empty)
                        {

                            MessageBox.Show("Please provide SMTP Port.");
                            return;
                        }




                        Systproperties.EmailID = textBoxEmailID.Text;
                        Systproperties.EmailPassword = textBoxEmailPassword.Text;
                        Systproperties.EmailPort = textBoxEmailPort.Text;
                        Systproperties.EmailSMTP = textBoxEmailSMTP.Text;
                    }
                    else
                    {
                        Systproperties.EmailID = string.Empty;
                        Systproperties.EmailPassword = string.Empty;
                        Systproperties.EmailPort = string.Empty;
                        Systproperties.EmailSMTP = string.Empty;
                    }
                    Systproperties.SMS = chbSMS.Checked;
                    if (chbSMS.Checked)
                    {



                        if (textBoxSMSID.Text == string.Empty)
                        {

                            MessageBox.Show("Please provide SMS User ID.");
                            return;
                        }
                        if (textBoxSMSPassword.Text == string.Empty)
                        {

                            MessageBox.Show("Please provide SMS Password.");
                            return;
                        }
                        if (textBoxSMSSecret.Text == string.Empty)
                        {

                            MessageBox.Show("Please provide SMS API Secret code.");
                            return;
                        }
                        if (textBoxSMSToken.Text == string.Empty)
                        {

                            MessageBox.Show("Please provide SMS API Token.");
                            return;
                        }

                        Systproperties.SMSID = textBoxSMSID.Text;
                        Systproperties.SMSPassword = textBoxSMSPassword.Text;
                        Systproperties.SMSSecret = textBoxSMSSecret.Text;
                        Systproperties.SMSToken = textBoxSMSToken.Text;
                        Systproperties.GSM = false;







                      


                       
                    }
                    else
                    {
                        Systproperties.SMSID = string.Empty;
                        Systproperties.SMSPassword = string.Empty;
                        Systproperties.SMSSecret = string.Empty;
                        Systproperties.SMSToken = string.Empty;                       
                        Systproperties.GSM = false;
                        Systproperties.WebLink = false;
                    }
                    DAL.Company company = instance.DataLink.Companies.SingleOrDefault();
                    if (company != null)
                    {
                        company.Company_Name = textBoxCompany.Text;
                    }


                    instance.DataLink.SubmitChanges();
                    Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "System properties has updated", instance.UserInstance.Full_Name);
                    MessageBox.Show("Data has been saved!");
                    RefresPageProperties();
                }
            }

            catch (Exception)
            {

                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                //Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");

            }
        

        
        }


     

        private void chbSMS_CheckedChanged(object sender, EventArgs e)
        {
              groupBoxSMS.Enabled = chbSMS.Checked;
                       
        }

        private void chbEmail_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxEmail.Enabled = chbEmail.Checked;
        }

        async void buttontest_Click(object sender, EventArgs e)
        {
            if (chbEmail.Checked && textBoxEmailID.Text == string.Empty)
            {
                MessageBox.Show("Please provide User Name.");
                return;
            }
            if (chbEmail.Checked && textBoxEmailPassword.Text == string.Empty)
            {
                MessageBox.Show("Please provide Email Password.");
                return;
            }
            if (chbEmail.Checked && textBoxEmailSMTP.Text == string.Empty)
            {
                MessageBox.Show("Please provide SMTP.");
                return;
            }
            if (chbEmail.Checked && textBoxEmailPort.Text == string.Empty)
            {
                MessageBox.Show("Please provide SMTP Port.");
                return;
            }
            if (chbEmail.Checked && textBoxtestaddress.Text == string.Empty)
            {
                MessageBox.Show("Please provide Test Email Address.");
                textBoxtestaddress.Focus();
                return;
            }
            bool b = await SendMail();
        }
        async Task<bool> SendMail()
        {
            try
            {
                await Task.Run(() =>
                {

                    string smtpAddress = textBoxEmailSMTP.Text;// instance.SystemProperties.EmailSMTP; //ConfigurationManager.AppSettings["SMTP"].ToString();
                    int portNumber = Convert.ToInt32(textBoxEmailPort.Text);// ConfigurationManager.AppSettings["Port"].ToString());
                    bool enableSSL = true;
                    string emailFromAddress = textBoxEmailID.Text;// instance.SystemProperties.EmailID;// ConfigurationManager.AppSettings["UserID"].ToString(); //Sender Email Address  
                    string password = textBoxEmailPassword.Text;// instance.SystemProperties.EmailPassword;// ConfigurationManager.AppSettings["Password"].ToString(); //Sender Password  
                    string subject = "Test Mail";
                    string body = @"<html><body> <h3> This is the Test Mail from Logit WiFi </h3><br/>Thanks & Regards, <br/>Logit System ";

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(emailFromAddress);
                        mail.To.Add(textBoxtestaddress.Text);



                        mail.Subject = subject;
                        mail.Body = body;
                        mail.IsBodyHtml = true;

                        using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                        {
                            try
                            {

                                smtp.EnableSsl = enableSSL;
                                smtp.UseDefaultCredentials = false;
                                smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                                smtp.Host = smtpAddress;
                                smtp.Port = portNumber;
                                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                                smtp.Send(mail);
                                MessageBox.Show("Email Successfully Sent.");
                            }
                            catch (Exception m)
                            {

                                MessageBox.Show("Delivery failed." + " Message : " + m.Message);
                                //Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");
                            }
                        }
                    }
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (chbSMS.Checked)
            {
               
             
                    if (textBoxSMSID.Text == string.Empty)
                    {

                        MessageBox.Show("Please provide SMS User ID.");
                        return;
                    }
                    if (textBoxSMSPassword.Text == string.Empty)
                    {

                        MessageBox.Show("Please provide SMS Password.");
                        return;
                    }
                    if (textBoxSMSSecret.Text == string.Empty)
                    {

                        MessageBox.Show("Please provide SMS API Secret code.");
                        return;
                    }
                    if (textBoxSMSToken.Text == string.Empty)
                    {

                        MessageBox.Show("Please provide SMS API Token.");
                        return;
                    }
                    if (textBoxSMSTest.Text == string.Empty)
                    {
                        MessageBox.Show("Please provide SMS number.");
                        return;
                    }
                    CustomControls.SMSComponent sms = new CustomControls.SMSComponent(textBoxSMSToken.Text, textBoxSMSSecret.Text);
                    sms.send(textBoxSMSTest.Text, "This is Test SMS from Logit WiFi");
                    sms.Dispose();
                    //CustomControls.SMSComponent sms = new CustomControls.SMSComponent(textBoxSMSToken.Text, );
                    //sms.send(textBoxSMSTest.Text,)

                

            }
        }
        
        public object _object { get; set; }

        private void checkBoxWifiAlarm_CheckedChanged(object sender, EventArgs e)
        {
            textBoxAlarmIP.Enabled = checkBoxWifiAlarm.Checked;
            textBoxAlarmPort.Enabled = checkBoxWifiAlarm.Checked;           
        }
    }
}
