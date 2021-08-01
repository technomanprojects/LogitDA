using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log_It.Forms
{
    public partial class SystemSetting : Form
    {
        bool DBverified;
        string servername;
        string userlogin;
        string password;
        public string ConnectionStringDb;

        public SystemSetting()
        {
            try
            {
                InitializeComponent();

                SqlDataSourceEnumerator obj = SqlDataSourceEnumerator.Instance;
                DataTable table = obj.GetDataSources();
                if (table != null)
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        this.cmbServers.Items.Add(table.Rows[i][0].ToString() + @"\" + table.Rows[i][1].ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void applicationProperties1_Load(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection Conn = new SqlConnection("Data Source=" + cmbServers.Text + ";Initial Catalog=master;Persist Security Info=True;User ID=" + textBoxlogin.Text + ";Password=" + textBoxpassword.Text);
                SqlCommand testCMD = new SqlCommand("sp_spaceused", Conn);
                SqlCommand cmd = new SqlCommand("select @@version AS 'Server Name' ", Conn);
                testCMD.CommandType = CommandType.StoredProcedure;
                cmd.CommandType = CommandType.Text;
                Conn.Open();
                SqlDataReader reader = testCMD.ExecuteReader();
                if (reader.HasRows)
                {
                    MessageBox.Show("Test Connection Sucessfull");
                    DBverified = true;
                    servername = cmbServers.Text;
                    userlogin = textBoxlogin.Text;
                    password = textBoxpassword.Text;
                }
                Conn.Close();

            }
            catch (Exception)
            {

                MessageBox.Show("Test Connection Fail");
                DBverified = false;
            }


        }

        private void SystemSetting_Load(object sender, EventArgs e)
        {
            comboBoxParity.SelectedIndex = 0;
            comboBoxStopBit.SelectedIndex = 0;
            comboBoxUnit.SelectedIndex = 0;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            
            if (!DBverified)
            {
                MessageBox.Show("Please verifiy the Database connection");
                return;
            }
            if (textBoxCompany.Text == string.Empty)
            {
                MessageBox.Show("Please Enter the company name");
                textBoxCompany.Focus();
                return;

            }
            if (textBoxDepartment.Text == string.Empty)
            {
                MessageBox.Show("Please Enter the department name");
                textBoxDepartment.Focus();
                return;
            }
            if (textBoxCom.Text == string.Empty)
            {
                MessageBox.Show("Please enter the port number");
                textBoxCom.Focus();
                return;
            }
            if (textBoxBaudRate.Text == string.Empty)
            {
                MessageBox.Show("Please enter the Baud Rate");
                textBoxBaudRate.Focus();
                return;
            }

            try
            {
                SqlConnection Conn = new SqlConnection("Data Source=" + servername + ";Initial Catalog=master;Persist Security Info=True;User ID=" + userlogin + ";Password=" + password);
                SqlCommand testCMD = new SqlCommand("create database PlotterDA", Conn);


                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                int i = testCMD.ExecuteNonQuery();

                Conn.Close();
                Conn = new SqlConnection("Data Source=" + servername + ";Initial Catalog=PlotterDA;Persist Security Info=True;User ID=" + userlogin + ";Password=" + password);
                ConnectionStringDb = BAL.Authentication.GetEc( Conn.ConnectionString);

                testCMD = new SqlCommand("CREATE TABLE  [dbo].[Alaram_Log](	[ID] [uniqueidentifier] NOT NULL,	[Device_ID] [int] NOT NULL,	[Device_Type] [nvarchar](50) NULL,	[Description] [nvarchar](50) NULL,	[Time] [datetime] NULL,	[_data] [decimal](18, 0) NULL, CONSTRAINT [PK_Alaram_Log] PRIMARY KEY CLUSTERED (	[ID] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("CREATE TABLE [dbo].[Company](	[Id] [uniqueidentifier] NOT NULL,	[Company_Name] [nvarchar](50) NOT NULL,	[Department] [nvarchar](50) NULL, CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("CREATE TABLE [dbo].[Device_Config](	[ID] [uniqueidentifier] NOT NULL,	[Channel_id] [nvarchar](50) NULL,	[Port_No] [int] NULL,	[Location] [nvarchar](50) NULL,	[Instrument] [nvarchar](50) NULL,	[Interval] [int] NULL,	[Active] [bit] NULL,	[Alaram] [bit] NULL,	[Last_Record] [datetime] NULL,	[CreatedBy] [nvarchar](50) NULL,	[CreateDateTime] [datetime] NULL,	[ModifiedBy] [nvarchar](50) NULL,	[ModifiedDateTime] [datetime] NULL,	[IsRowActive] [bit] NULL,	[Rh_Active] [bit] NULL,	[Upper_Limit] [int] NULL,	[Lower_Limit] [int] NULL,	[Upper_Range] [int] NULL,	[Lower_Range] [int] NULL,	[Device_Type] [int] NULL,	[Offset] [float] NULL,	[dateofCalibration] [datetime] NULL,	[Lower] [int] NULL,	[higher] [int] NULL, CONSTRAINT [PK_Device_Config] PRIMARY KEY CLUSTERED (	[ID] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();
                Conn.Close();

             

                Conn.Close();

                testCMD = new SqlCommand("CREATE TABLE [dbo].[Device_Enum](	[ID] [int] NOT NULL,	[Type] [nchar](10) NULL, CONSTRAINT [PK_Device_Enum] PRIMARY KEY CLUSTERED (	[ID] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("CREATE TABLE [dbo].[Email](	[ID] [uniqueidentifier] NOT NULL,	[EmailID] [nvarchar](100) NULL, CONSTRAINT [PK_Email] PRIMARY KEY CLUSTERED (	[ID] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();


                testCMD = new SqlCommand("CREATE TABLE [dbo].[EventLog](	[ID] [uniqueidentifier] NOT NULL,	[DateTime] [datetime] NULL,	[UserName] [nvarchar](50) NULL,	[EventName] [nvarchar](50) NULL,	[MessageLog] [nvarchar](max) NULL, CONSTRAINT [PK_EventLog] PRIMARY KEY CLUSTERED (	[ID] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();


               



                testCMD = new SqlCommand("CREATE TABLE [dbo].[Log](	[ID] [uniqueidentifier] NOT NULL,	[DeviceID] [uniqueidentifier] NULL,	[Channel_ID] [nvarchar](50) NULL,	[Port_Id] [nvarchar](50) NOT NULL,	[_Data] [float] NULL,	[date_] [datetime] NULL, CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED (	[ID] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("CREATE TABLE [dbo].[ofsetAuditRecord](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Device_Id] [int] NULL,	[ofset] [float] NOT NULL,	[createdby] [nvarchar](50) NULL,	[createddatetime] [datetime] NULL, CONSTRAINT [PK_ofsetAuditRecord_1] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();


                testCMD = new SqlCommand("CREATE TABLE [dbo].[Roles](	[Id] [int] NOT NULL,	[RoleName] [nvarchar](50) NULL, CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("CREATE TABLE [dbo].[SYSProperties](	[ID] [int] NOT NULL,	[Unit] [nvarchar](50) NULL,	[Signature] [bit] NULL,	[Automatic_Sign] [bit] NULL,	[Port] [int] NULL,	[BaudRate] [nvarchar](50) NULL,	[DataBit] [nvarchar](50) NULL,	[Parity] [nvarchar](50) NULL,	[StopBit] [nvarchar](50) NULL,	[RTS] [bit] NULL,	[DTS] [bit] NULL,	[D_Type] [int] NULL,	[D_Name] [nvarchar](50) NULL,	[Number_Devices] [int] NULL, CONSTRAINT [PK_SYSProperties] PRIMARY KEY CLUSTERED (	[ID] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("CREATE TABLE [dbo].[User](	[Id] [int] IDENTITY(1,1) NOT NULL,	[User_Name] [nvarchar](50) NOT NULL,	[Password] [nvarchar](2000) NULL,	[Role] [int] NULL,	[CreatedBy] [nvarchar](50) NULL,	[CreateDateTime] [datetime] NULL,	[Active] [bit] NULL,	[ModefiedBy] [nvarchar](50) NULL,	[ModifiedDateTime] [datetime] NULL,	[IsRowEnable] [bit] NULL,	[Full_Name] [nvarchar](max) NULL,	[Authority] [nvarchar](50) NULL,	[Description] [nvarchar](max) NULL,	[Email] [nvarchar](50) NULL, CONSTRAINT [PK_User_1] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY] ", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                //

                testCMD = new SqlCommand("ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_Roles] FOREIGN KEY([Role]) REFERENCES [dbo].[Roles] ([Id]) ", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("ALTER TABLE [dbo].[User] CHECK CONSTRAINT [FK_User_Roles] CREATE TABLE [dbo].[Group]( [ID] [int] IDENTITY(1,1) NOT NULL,	[Group_Name] [nvarchar](50) NULL, CONSTRAINT [PK_Group] PRIMARY KEY CLUSTERED (	[ID] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();




                testCMD = new SqlCommand("CREATE TABLE [dbo].[Group_Devices](	[Id] [int] IDENTITY(1,1) NOT NULL,	[Group_Id] [int] NOT NULL,	[Device_Id] [int] NOT NULL, CONSTRAINT [PK_Group_Devices] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY]", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("CREATE TABLE [dbo].[Group_User](	[Id] [int] IDENTITY(1,1) NOT NULL,	[User_Id] [int] NOT NULL,	[Group_ID] [int] NOT NULL, CONSTRAINT [PK_Group_User] PRIMARY KEY CLUSTERED (	[Id] ASC)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]) ON [PRIMARY] ", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                //GO 

                testCMD = new SqlCommand("ALTER TABLE [dbo].[Group_User]  WITH CHECK ADD  CONSTRAINT [FK_Group_User_Group] FOREIGN KEY([Group_ID]) REFERENCES [dbo].[Group] ([ID])", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("ALTER TABLE [dbo].[Group_User] CHECK CONSTRAINT [FK_Group_User_Group]", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("ALTER TABLE [dbo].[Group_User]  WITH CHECK ADD  CONSTRAINT [FK_Group_User_User] FOREIGN KEY([User_Id])REFERENCES [dbo].[User] ([Id])", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("ALTER TABLE [dbo].[Group_User] CHECK CONSTRAINT [FK_Group_User_User] ", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("ALTER TABLE [dbo].[Device_Config]  WITH CHECK ADD  CONSTRAINT [FK_Device_Config_Device_Enum1] FOREIGN KEY([Device_Type]) REFERENCES [dbo].[Device_Enum] ([ID])", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();
                Conn.Close();

                testCMD = new SqlCommand("ALTER TABLE [dbo].[Device_Config] CHECK CONSTRAINT [FK_Device_Config_Device_Enum1]", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("INSERT INto Device_Enum(ID,Type) Values(1,'Temp')", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("INSERT INto Device_Enum(ID,Type) Values(2,'Rh')", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("INSERT INto Device_Enum(ID,Type) Values(3,'Pressure')", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("INSERT INTO Roles(Id,RoleName) Values(0,'Owner')", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("INSERT INTO Roles(Id,RoleName) Values(1,'Administrator')", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();
                testCMD = new SqlCommand("INSERT INTO Roles(Id,RoleName) Values(2,'User')", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

               

                Conn.Close();
                int s1 = 0;
                int s2 = 0;
                int s3 = 0;
                int s4 = 0;
                if (checkBoxSignLine.Checked)
                {
                    s1 = 1;
                }
                else
                {
                    s1 = 0;
                }
                if (checkBoxSignLogged.Checked)
                {
                    s2 = 1;
                }
                else
                {
                    s2 = 0;
                }
                if (checkBoxRTS.Checked)
                {
                    s3 = 1;
                }
                else
                {
                    s3 = 0;
                }
                if (checkBoxDTS.Checked)
                {
                    s4 = 1;
                }
                else
                {
                    s4 = 0;
                }
                testCMD = new SqlCommand("INSERT INTO SYSProperties([ID],[Unit],[Signature],[Automatic_Sign],[Port],[BaudRate],[DataBit],[Parity],[StopBit],[RTS],[DTS],[D_Type],[D_Name],[Number_Devices])VALUES ("
                    + 0 + ",'" + comboBoxUnit.Text + "'," + s1 + "," + s2 + "," + Convert.ToInt32(textBoxCom.Text) + ",'" + textBoxBaudRate.Text + "','" + textBoxDataBit.Text + "','" + comboBoxParity.Text + "','" + comboBoxStopBit.Text + "'," + s3 + "," + s4 + "," + 2 + ",'Aosong'," + Convert.ToInt32(numericUpDown1.Value) + ")", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("INSERT INTO Company ([ID],[Company_Name],[Department]) VALUES ('" + Guid.NewGuid() + "','" + textBoxCompany.Text + "','" + textBoxDepartment.Text + "')", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();

                Conn.Close();

                testCMD = new SqlCommand("INSERT INTO [User] ([User_Name],[Password],[Role],[CreatedBy],[CreateDateTime],[Active],[ModefiedBy],[ModifiedDateTime],[IsRowEnable],[Full_Name],[Authority],[Description],[Email]) VALUES ('" +
                    textBoxuserName.Text.ToLower() + "','" + BAL.Authentication.GetEc(textBoxloginpassword.Text) + "','" + 0 + "','System Admin',GETDATE(),1,'NULL',NULL," + 1 + ",'System Administrator','Owner','NULL','NULL')", Conn);
                testCMD.CommandType = CommandType.Text;
                Conn.Open();
                i = testCMD.ExecuteNonQuery();
                Conn.Close();

                //RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\TMSSetting");
                //if (key == null)
                //{
                //    key = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\TMSSetting");
                //    key.SetValue("A1", 1);
                //}

                MessageBox.Show("Configuration has been saved");
                this.DialogResult = DialogResult.OK;
                this.Close();

            }
            catch (Exception m)
            {
                labelstatus.Text = m.Message;
                MessageBox.Show(m.Message);
                this.DialogResult = DialogResult.Cancel;
            }
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void textBoxCom_KeyPress(object sender, KeyPressEventArgs e)
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

        private void numericUpDown1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
