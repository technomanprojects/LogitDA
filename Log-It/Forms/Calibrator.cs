using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log_It.Forms
{
    public partial class Calibrator : Form
    {
        BAL.LogitInstance instance;
        List<DAL.Device_Config> devicelist;
        DAL.Device_Config config;

        public Calibrator(BAL.LogitInstance instance)
        {
            InitializeComponent();
            this.instance = instance;

            foreach (var item in instance.DataLink.Device_Configs.Where(x => x.Active == true && x.IsRowActive == true))
            {
                deviceEntityComboBox1.Items.Add(item);

            }
            devicelist = instance.DataLink.Device_Configs.Where(x => x.Active == true && x.IsRowActive == true).ToList();
        }



        private void comboBoxtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (comboBoxtype.SelectedIndex == -1 || comboBoxtype.Text == string.Empty)
            //{
            //    return;
            //}

            ////DAL.Device_Config config = devicelist.SingleOrDefault(x => x.Device_Id == Convert.ToInt16(comboBoxdevices.Text));
            //if (comboBoxtype.Text == "Temperature")
            //{
            //     limit = config.LimitTables.SingleOrDefault(x => x.Device_type == 1);
            //    labelupperlimit.Text = limit.Upper_Limit.ToString();
            //    labellowerlimit.Text = limit.Lower_Limit.ToString();
            //    textBoxoffset.Text = limit.ofset.ToString();
            //    labeldatetime.Text = limit.dateofcalibrate.ToString();
            //}
            //if (comboBoxtype.Text == "Humidity")
            //{
            //    limit = config.LimitTables.SingleOrDefault(x => x.Device_type == 2);
            //    labelupperlimit.Text = limit.Upper_Limit.ToString();
            //    labellowerlimit.Text = limit.Lower_Limit.ToString();
            //    textBoxoffset.Text = limit.ofset.ToString();
            //    labeldatetime.Text = limit.dateofcalibrate.ToString();
            //}  
        }

        private void comboBoxdevices_SelectedValueChanged(object sender, EventArgs e)
        {
            //if (comboBoxdevices.SelectedIndex == -1 )
            //{
            //    return;
            //}
            //config = devicelist.SingleOrDefault(x => x.Device_Id == Convert.ToInt16(comboBoxdevices.Text));
            //labellocation.Text = config.Location;
            //labelinstrument.Text = config.Instrument;
            //if (comboBoxtype.Items.Count > 0)
            //{
            //    comboBoxtype.Items.Clear();
            //    comboBoxtype.Text = string.Empty;
            //    labelupperlimit.Text = string.Empty;
            //    labellowerlimit.Text = string.Empty;
            //    textBoxoffset.Text = string.Empty;
            //    labeldatetime.Text = string.Empty;

            //}
            //comboBoxtype.Items.Add("Temperature");
            //if (config.Rh_Active == true)
            //{
            //    comboBoxtype.Items.Add("Humidity");

            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonsave_Click(object sender, EventArgs e)
        {
            try 
	{	        
		   if (deviceEntityComboBox1.SelectedEntity != null)
            {
                if (textBoxoffset.Text != deviceEntityComboBox1.SelectedEntity.Offset.ToString())
                {
                    instance.Device_Configes.SingleOrDefault(x => x.ID == deviceEntityComboBox1.SelectedEntity.ID).Offset= Convert.ToDouble(textBoxoffset.Text);
                
                    instance.DataLink.SubmitChanges();
                    
                     lblLocation.Text = string.Empty;
                lblChannelID.Text =string.Empty;
                lblPortNo.Text = string.Empty;
                lblDeviceType.Text =string.Empty;
                textBoxoffset.Text = string.Empty;
                lblUpperLimit.Text = string.Empty;
                lbllower.Text = string.Empty;
                lblDatetime.Text = string.Empty;           
                    MessageBox.Show("Date has been saved");
                }
            }
	}
	catch (Exception m)
	{
		
		MessageBox.Show(m.Message);
	}
         
           
        }

        private void deviceEntityComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (deviceEntityComboBox1.SelectedEntity != null)
            {
                lblLocation.Text = deviceEntityComboBox1.SelectedEntity.Location;
                lblChannelID.Text = deviceEntityComboBox1.SelectedEntity.Channel_id;
                lblPortNo.Text = deviceEntityComboBox1.SelectedEntity.Port_No.ToString();

                //lblDeviceType.Text = deviceEntityComboBox1.SelectedEntity.devDevice_Enum.Type;
                textBoxoffset.Text = deviceEntityComboBox1.SelectedEntity.Offset.ToString();
                lblUpperLimit.Text = deviceEntityComboBox1.SelectedEntity.Upper_Limit.ToString();
                lbllower.Text = deviceEntityComboBox1.SelectedEntity.Lower_Limit.ToString();
                lblDatetime.Text = deviceEntityComboBox1.SelectedEntity.dateofCalibration.ToString();
            }
        }
    }
}
