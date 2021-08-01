using BAL;
using DAL;
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
    public partial class AddDepartment : BaseForm
    {
        private readonly LogitInstance instance;
        private readonly bool isnew;
        private readonly int deptid;
        public AddDepartment(bool isnew, LogitInstance instance,int id)
        {
          
            this.instance = instance;
            this.deptid = id;
            this.isnew = isnew;
            InitializeComponent();
            if (!isnew)
            {
                this.Text = "Edit Department";
                   Department editdept = instance.DataLink.Departments.SingleOrDefault(x => x.Department_Id == id);
                   if (editdept != null)
                   {
                       textBoxDepartment.Text = editdept.Department_Name;
                       textBox1.Text = editdept.Department_Description;
                   }
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {

            switch (isnew)
            {
                case true:
                    Department dept = new Department()
                    {
                        Department_Name = textBoxDepartment.Text,
                        Department_Description = textBox1.Text
                    };
                    instance.DataLink.Departments.InsertOnSubmit(dept);

                    instance.DataLink.SubmitChanges();

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    break;

                case false:


                    Department editdept = instance.DataLink.Departments.SingleOrDefault(x => x.Department_Id == deptid);

                    editdept.Department_Name = textBoxDepartment.Text;
                    editdept.Department_Description = textBox1.Text;

                  

                    instance.DataLink.SubmitChanges();

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                    break;


            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void textBoxDepartment_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsControl(e.KeyChar) || char.IsLetter(e.KeyChar) || char.IsWhiteSpace(e.KeyChar)))
            {
                e.Handled = true;
            }
        }
    }
}
