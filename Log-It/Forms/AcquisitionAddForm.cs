using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log_It.Forms
{
    public partial class AcquisitionAddForm : Form
    {
        public delegate void FormClose();
        public event FormClose close;
        BAL.LogitInstance Instance;
        bool isNew;
        Guid Id;

        public AcquisitionAddForm(Guid Id, BAL.LogitInstance Instance, bool isNew)
        {
            this.Instance = Instance;
            this.isNew = isNew;

            InitializeComponent();
            this.Id = Id;
            comboBoxType.SelectedIndex = 0;
            comboBoxNetwork.Items.Clear();
            comboBoxNetwork.Items.Add(Instance.DataLink.SYSProperties.SingleOrDefault(x => x.ID == 1).Port1);
            comboBoxNetwork.Items.Add(Instance.DataLink.SYSProperties.SingleOrDefault(x => x.ID == 1).Port2);
            DAL.Device_Config config = Instance.Device_Configes.SingleOrDefault(x => x.ID == Id && x.Active == true && x.IsRowActive == true);
            List<DAL.Department> dlist = Instance.DataLink.Departments.ToList();

            foreach (var item in dlist)
            {
                deptComboBox1.Items.Add(item);
            }
            if (config != null)
            {
                //checkBoxAlaram.Checked = (bool)config.Alaram;

                comboBoxNetwork.Text = config.E_Port;
                comboBoxPort.Text = config.Channel_id;
                textBoxChannelID.Text = config.Channel_id.ToString();
                textBoxInstrument.Text = config.Instrument;
                numericInterval.Value = Convert.ToDecimal(config.Interval);
                config.IsRowActive = true;
                textBoxLocation.Text = config.Location;
                if (config.Department != null)
                {
                    deptComboBox1.Text = Instance.DataLink.Departments.SingleOrDefault(x => x.Department_Id == config.Department_Id).Department_Name;    
                }
                
                textBoxTLL.Text = config.Lower_Limit.ToString();
                textBoxTUL.Text = config.Upper_Limit.ToString();
                textBoxTLR.Text = config.Lower_Range.ToString();
                textBoxTUR.Text = config.Upper_Range.ToString();
                comboBoxType.SelectedIndex = (int)config.Device_Type - 1;
                textBoxdeviceLL.Text = config.Lower.ToString();
                textBoxDeviceUL.Text = config.higher.ToString();
                checkBoxAlaram.Checked = (bool)config.Alarm;
            }
        }

        private bool Validation()
        {
            try
            {
                if (textBoxChannelID.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Device ID");
                    return false;
                }
                if (comboBoxNetwork.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Network Port");
                    return false;
                }
                if (comboBoxPort.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter port number");
                    return false;
                }
                if (textBoxInstrument.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Serial Number");
                    return false;
                }
                if (!comboBoxType.Items.Contains(comboBoxType.Text))
                {
                    MessageBox.Show("Please select correct type");
                    return false;
                }
                if (Instance.DataLink.Device_Configs.FirstOrDefault(x => x.Active == true && x.Channel_id == textBoxChannelID.Text && x.Port_No == Convert.ToInt32( comboBoxPort.Text)) != null)
                {
                    MessageBox.Show("Channel and Port already configured.");
                    return false;
                }

            }
            catch (Exception m)
            {

                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");

            }
            return true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Validation())
                {
                    return;
                }
                if (isNew)
                {

                    DAL.Device_Config config = new DAL.Device_Config();
                    config.ID = Guid.NewGuid();
                    config.Active = true;
                    config.E_Port = comboBoxNetwork.Text;
                    config.Channel_id = comboBoxPort.Text;
                    config.Port_No = Convert.ToInt32( comboBoxPort.Text);
                    config.CreateDateTime = DateTime.Now;
                    config.CreatedBy = Instance.UserInstance.Full_Name;
                    config.Channel_id = textBoxChannelID.Text;
                    config.Instrument = textBoxInstrument.Text;
                    config.Interval = Convert.ToInt32(numericInterval.Value);
                    config.IsRowActive = true;
                    config.Location = textBoxLocation.Text;
                    config.Last_Record = DateTime.Now;
                    config.Department_Id = deptComboBox1.SelectedEntity.Department_Id;
                    config.Department = deptComboBox1.SelectedEntity;
                    config.Lower_Limit = Convert.ToInt32(textBoxTLL.Text);
                    config.Upper_Limit = Convert.ToInt32(textBoxTUL.Text);
                    config.Lower_Range = Convert.ToInt32(textBoxTLR.Text);
                    config.Upper_Range = Convert.ToInt32(textBoxTUR.Text);
                    config.Device_Type = comboBoxType.SelectedIndex + 1;
                    config.Offset = 0.0f;
                    config.dateofCalibration= DateTime.Now;
                    config.Lower = Convert.ToInt32(textBoxdeviceLL.Text);
                    config.higher = Convert.ToInt32(textBoxDeviceUL.Text);
                    config.Alarm = checkBoxAlaram.Checked;

                    Instance.DataLink.Device_Configs.InsertOnSubmit(config);

                    Instance.DataLink.SubmitChanges();
                    Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modfiy, "Add new Device by" + Instance.UserInstance.Full_Name, Instance.UserInstance.Full_Name);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;

                    MessageBox.Show("Device has been saved");
                    this.Close();
                }
                else
                {
                    DAL.Device_Config config = Instance.Device_Configes.SingleOrDefault(x => x.ID == Id);
                    config.Active = true;
                    
                    config.Channel_id = comboBoxPort.Text;
                    config.E_Port = comboBoxNetwork.Text;
                    config.CreateDateTime = DateTime.Now;
                    config.CreatedBy = Instance.UserInstance.Full_Name;
                    config.Channel_id = textBoxChannelID.Text;
                    config.Instrument = textBoxInstrument.Text;
                    config.Interval = Convert.ToInt32(numericInterval.Value);
                    config.IsRowActive = true;
                    config.Location = textBoxLocation.Text;
                    config.Lower_Limit = Convert.ToInt32(textBoxTLL.Text);
                    config.Upper_Limit = Convert.ToInt32(textBoxTUL.Text);
                    config.Lower_Range = Convert.ToInt32(textBoxTLR.Text);
                    config.Upper_Range = Convert.ToInt32(textBoxTUR.Text);
                    config.Lower = Convert.ToInt32(textBoxdeviceLL.Text);
                    config.higher = Convert.ToInt32(textBoxDeviceUL.Text);
                    config.Department = deptComboBox1.SelectedEntity;
                    config.Device_Type = comboBoxType.SelectedIndex + 1;


                    config.Alarm = checkBoxAlaram.Checked;
                    Instance.DataLink.SubmitChanges();
                    Technoman.Utilities.EventClass.WriteLog(Technoman.Utilities.EventLog.Modfiy, "Modifed Device by " + Instance.UserInstance.Full_Name, Instance.UserInstance.Full_Name);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    MessageBox.Show("Device has been saved");
                    this.Close();
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

        private void textBoxTLL_TextChanged(object sender, EventArgs e)
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
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
            if (close != null)
            {
                close();
            }
        }       
    }
}
