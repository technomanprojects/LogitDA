using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using BAL;
using DAL;
using System.Net.Mail;
using System.Diagnostics;
using System.Net;
using Log_It.CustomControls;
using System.Text.RegularExpressions;

namespace Log_It.Pages
{
    public enum D_Type
    {
        Aosong =1,
        Acquisition=2
    }

    public partial class ApplicationProperties : ControlPage
    {
        public LogitInstance Instance { get; set; }
        public bool systemset = false;
        private readonly string username;
        public D_Type d_Type =  D_Type.Acquisition;

        public delegate void RefreshProperties();
        public event RefreshProperties RefresPageProperties;
        public ApplicationProperties(LogitInstance instance,string username )
        {
            InitializeComponent( );
            d_Type = D_Type.Acquisition;
            this.Instance = instance;
            this.username = username;

            this.RefreshPage();
        }

        public override void RefreshPage()
        {
            try
            {
                base.RefreshPage();
                if (Instance.DataLink.SYSProperties.Count() > 0)
                {
                    SYSProperty Sysproperties = Instance.DataLink.SYSProperties.SingleOrDefault(x => x.ID == 1);
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

                    Company company = Instance.DataLink.Companies.SingleOrDefault();
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

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox t = (TextBox)sender;
            if (Regex.IsMatch(t.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                t.Text = t.Text.Remove(t.Text.Length - 1);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                String mess = string.Empty;
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
                    Instance.DataLink.SYSProperties.InsertOnSubmit(Systproperties);

                    Company company = new Company
                    {
                        Id = Guid.NewGuid(),
                        Company_Name = textBoxCompany.Text
                    };

                    Instance.DataLink.Companies.InsertOnSubmit(company);

                    Instance.DataLink.SubmitChanges();

                    //RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\TLSSetting");
                    //if (key == null)
                    //{
                    //    key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\TLSSetting");
                    //    key.SetValue("A1", 1);
                    //}


                    Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, " System properties has updated", Instance.UserInstance.Full_Name);
                    MessageBox.Show("Data has been saved!");
                }
                else
                {
                    SYSProperty Systproperties = Instance.DataLink.SYSProperties.SingleOrDefault(x => x.ID == 1);
                    if (Systproperties.Unit != comboBoxUnit.Text)
                    {
                        mess += ", Unit " + Systproperties.Unit + " to " + comboBoxUnit.Text;
                    }
                    if (Systproperties.Signature != (bool)checkBoxSignLine.Checked)
                    {
                        mess += ", Signature " + Systproperties.Signature + " to " + checkBoxSignLine.Checked;
                    }
                    if (Systproperties.Automatic_Sign != (bool)checkBoxSignLogged.Checked)
                    {
                        mess += ", Automatic Signature " + Systproperties.Automatic_Sign + " to " + checkBoxSignLogged.Checked;
                    }
                    if (Systproperties.IP_Address != textBoxIPAddress.Text)
                    {
                        mess += ", IP Address " + Systproperties.IP_Address + " to " + textBoxIPAddress.Text;
                    }
                    if (Systproperties.Port1 != textBoxBaudRate.Text)
                    {
                        mess += ", Port 1 " + Systproperties.Port1 + " to " + textBoxBaudRate.Text;
                    }
                    if (Systproperties.Port2 != textBoxDataBit.Text)
                    {
                        mess += ", Port 2 " + Systproperties.Port2 + " to " + textBoxDataBit.Text;
                    }
                    if (Systproperties.Alert_Interval != Convert.ToInt32(textBoxAlarmInterval.Text))
                    {
                        mess += ", Alarm Interval " + Systproperties.Alert_Interval + " to " + textBoxAlarmInterval.Text;
                    }
                    if (Systproperties.AlarmEnable != (bool)checkBoxWifiAlarm.Checked)
                    {
                        mess += ", Alarm Enable " + Systproperties.AlarmEnable + " to " + checkBoxWifiAlarm.Checked;
                    }
                    if (Systproperties.Acknowledged != (bool)checkBoxAlert.Checked)
                    {
                        mess += ", Acknowledge " + Systproperties.Acknowledged + " to " + checkBoxAlert.Checked;
                    }

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
                        if (Systproperties.Alarm_IP != textBoxAlarmIP.Text)
                        {
                            mess += ", Alarm IP " + Systproperties.Alarm_IP + " to " + textBoxAlarmIP.Text;
                        }
                        if (Systproperties.Alarm_Port != textBoxAlarmPort.Text)
                        {
                            mess += ", Alarm Port " + Systproperties.Alarm_Port + " to " + textBoxAlarmPort.Text;
                        }
                        Systproperties.Alarm_IP = textBoxAlarmIP.Text;
                        Systproperties.Alarm_Port = textBoxAlarmPort.Text;
                        
                    }
                    else
                    {
                        Systproperties.Alarm_Port = string.Empty;                        
                        Systproperties.Alarm_IP = string.Empty;
                    }
                    if (Systproperties.Number_Devices != (int)numericUpDown2.Value)
                    {
                        mess += ", Number Device " + Systproperties.Number_Devices + " to " + (int)numericUpDown2.Value;
                    }
                    if (Systproperties.Email != (bool)chbEmail.Checked)
                    {
                        mess += ", Email Enable " + Systproperties.Email + " to " + chbEmail.Checked;
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
                        if (Systproperties.EmailID != textBoxEmailID.Text)
                        {
                            mess += ", Email ID " + Systproperties.EmailID + " to " + textBoxEmailID.Text;
                        }
                        if (Systproperties.EmailPassword != textBoxEmailPassword.Text)
                        {
                            mess += ", Email Password " + Systproperties.EmailPassword + " to " + textBoxEmailPassword.Text;
                        }
                        if (Systproperties.EmailPort != textBoxEmailPort.Text)
                        {
                            mess += ", Email Port " + Systproperties.EmailPort + " to " + textBoxEmailPort.Text;
                        }
                        if (Systproperties.EmailSMTP != textBoxEmailSMTP.Text)
                        {
                            mess += ", Email SMTP " + Systproperties.EmailSMTP + " to " + textBoxEmailSMTP.Text;
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
                    if (Systproperties.SMS != (bool)chbSMS.Checked)
                    {
                        mess += ", SMS " + Systproperties.SMS + " to " + chbSMS.Checked;
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
                        if (Systproperties.SMSID != textBoxSMSID.Text)
                        {
                            mess += ", SMS ID " + Systproperties.SMSID + " to " + textBoxSMSID.Text;
                        }
                        if (Systproperties.SMSPassword != textBoxSMSPassword.Text)
                        {
                            mess += ", SMS Password " + Systproperties.SMSPassword + " to " + textBoxSMSPassword.Text;
                        }
                        if (Systproperties.SMSSecret != textBoxSMSSecret.Text)
                        {
                            mess += ", SMS Secret " + Systproperties.SMSSecret + " to " + textBoxSMSSecret.Text;
                        }
                        if (Systproperties.SMSToken != textBoxSMSToken.Text)
                        {
                            mess += ", SMS Token " + Systproperties.SMSToken + " to " + textBoxSMSToken.Text;
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
                    Company company = Instance.DataLink.Companies.SingleOrDefault();
                    if (company != null)
                    {
                        if (company.Company_Name != textBoxCompany.Text)
                        {
                            mess += ", Company Name " + company.Company_Name + " to " + textBoxCompany.Text;
                        }
                        company.Company_Name = textBoxCompany.Text;
                    }
                    Instance.DataLink.SubmitChanges();
                    Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modify, "System properties" + mess.Remove(0, 1) + " has updated", Instance.UserInstance.Full_Name);
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
     
        private void ChbSMS_CheckedChanged(object sender, EventArgs e)
        {
              groupBoxSMS.Enabled = chbSMS.Checked;                      
        }

        private void ChbEmail_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxEmail.Enabled = chbEmail.Checked;
        }

        private async void Buttontest_Click(object sender, EventArgs e)
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
            _ = await SendMail();
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

        private void Button2_Click(object sender, EventArgs e)
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
                    SMSComponent sms = new SMSComponent(textBoxSMSToken.Text, textBoxSMSSecret.Text);
                    sms.send(textBoxSMSTest.Text, "This is Test SMS from Logit WiFi");
                    sms.Dispose();
                    //CustomControls.SMSComponent sms = new CustomControls.SMSComponent(textBoxSMSToken.Text, );
                    //sms.send(textBoxSMSTest.Text,)                

            }
        }
        
        public object _object { get; set; }

        private void CheckBoxWifiAlarm_CheckedChanged(object sender, EventArgs e)
        {
            textBoxAlarmIP.Enabled = checkBoxWifiAlarm.Checked;
            textBoxAlarmPort.Enabled = checkBoxWifiAlarm.Checked;           
        }

        private void textBoxtestaddress_Validated(object sender, EventArgs e)
        {
            string email = textBoxtestaddress.Text;
            var regex = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
            bool isValid = Regex.IsMatch(email, regex, RegexOptions.IgnoreCase);
            if (!isValid)
            {
                MessageBox.Show("Enter correct email address");
            }
        }

        private void textBoxSMSTest_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBoxIPAddress_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBoxBaudRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBoxDataBit_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBoxAlarmIP_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBoxAlarmPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void textBoxAlarmInterval_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void numericUpDown2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void panelHome_Paint(object sender, PaintEventArgs e)
        {
            //comboBoxUnit.SelectedIndex = 0;
        }
    }
}
