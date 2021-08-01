using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Resources;

namespace Log_It.Forms
{
	/// <summary>
	/// Summary description for ConfigurationWizard.
	/// </summary>
	/// 	
	
	public class CWTryAgain : Form
	{
		public event WizardCom SearchAgain;	
		public event WizardCom Finish;

        private Button  btnNext;
		private Panel panel1;
        private Button btnFinish;
		private Label label1;
		private ImageList imageList1;
		private Label label2;


		private IContainer components;

		//MainPage mp;
		public CWTryAgain()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();					

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new Container();
			ResourceManager resources = new ResourceManager(typeof(CWTryAgain));
            this.btnNext = new Button();
            this.btnFinish = new Button();
			this.panel1 = new Panel();
			this.label1 = new Label();
			this.imageList1 = new ImageList(this.components);
			this.label2 = new Label();
			this.SuspendLayout();
			// 
			// btnNext
			// 
			this.btnNext.DialogResult = DialogResult.Cancel;
			this.btnNext.Location = new Point(328, 328);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new Size(80, 24);
			this.btnNext.TabIndex = 3;
			this.btnNext.Text = "&Search Again";
			this.btnNext.Click += new System.EventHandler(this.BtnNext_Click);
			// 
			// btnFinish
			// 
			this.btnFinish.Location = new Point(416, 328);
			this.btnFinish.Name = "btnFinish";
			this.btnFinish.Size = new Size(72, 24);
			this.btnFinish.TabIndex = 4;
			this.btnFinish.Text = "&Finish";
			this.btnFinish.Click += new EventHandler(this.BtnFinish_Click);
			// 
			// panel1
			// 
			this.panel1.BackColor = Color.White;
			this.panel1.BackgroundImage = ((Image)(resources.GetObject("panel1.BackgroundImage")));
			this.panel1.Location = new Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(512, 56);
			this.panel1.TabIndex = 7;
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Location = new Point(120, 104);
			this.label1.Name = "label1";
			this.label1.Size = new Size(368, 40);
			this.label1.TabIndex = 8;
			this.label1.Text = "The wizard could not find any new device connected to your system. Please verify " +
				"that device is attached to the system properly.";
			// 
			// imageList1
			// 
			this.imageList1.ImageSize = new Size(24, 24);
			this.imageList1.ImageStream = ((ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.Color.Transparent;
			this.label2.ImageIndex = 0;
			this.label2.ImageList = this.imageList1;
			this.label2.Location = new Point(80, 96);
			this.label2.Name = "label2";
			this.label2.Size = new Size(40, 32);
			this.label2.TabIndex = 9;
			// 
			// CWTryAgain
			// 
			this.AcceptButton = this.btnFinish;
			this.AutoScaleBaseSize = new Size(5, 13);
			this.BackColor = SystemColors.Control;
			this.CancelButton = this.btnNext;
			this.ClientSize = new Size(508, 362);
			this.ControlBox = false;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.btnFinish);
			this.Controls.Add(this.btnNext);
			this.FormBorderStyle = FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CWTryAgain";
			this.ShowInTaskbar = false;
			this.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "Configuration Wizard: Try Again";
			this.ResumeLayout(false);

		}
        #endregion

        private void Mp_NextPage(int i)
        {
            Console.WriteLine(i);
        }

        private void BtnNext_Click(object sender, EventArgs e)
		{
            SearchAgain?.Invoke();
        }

		private void BtnFinish_Click(object sender, EventArgs e)
		{
            Finish?.Invoke();
            this.Hide();
		}					
	}	
}
