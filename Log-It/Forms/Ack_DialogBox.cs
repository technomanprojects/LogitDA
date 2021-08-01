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
    public partial class Ack_DialogBox : Form
    {
        public string Comments { get; set; }
        public Guid ID { get; set; }
        public Ack_DialogBox(Guid id, string type, DateTime dt, string description, string location, string instrument)
        {
            InitializeComponent();
            labelType.Text = type;
            labelDateTime.Text = dt.ToString();
            labelLocation.Text = location;
            labelInstrument.Text = instrument;
            labelDescription.Text = description;
            ID = id;
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            if (textBoxcomments.Text == string.Empty || textBoxcomments.Text == "")
            {
                MessageBox.Show("Please write comments for event ");
            }
            else
            {
                Comments = textBoxcomments.Text;
                this.DialogResult = DialogResult.OK;
            }

        }
    }
}
