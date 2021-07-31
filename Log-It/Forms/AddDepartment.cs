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
        BAL.LogitInstance instance;
        bool isnew;
        int deptid;
        public AddDepartment(bool isnew, BAL.LogitInstance instance,int id)
        {
          
            this.instance = instance;
            this.deptid = id;
            this.isnew = isnew;
            InitializeComponent();
            if (!isnew)
            {
                this.Text = "Edit Department";
                DAL.Department editdept = instance.DataLink.Departments.SingleOrDefault(x => x.Department_Id == id);
                   if (editdept != null)
                   {

                       textBoxDepartment.Text = editdept.Department_Name;
                       textBox1.Text = editdept.Department_Description;
                   }

                  

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            switch (isnew)
            {
                case true:
                    DAL.Department dept = new DAL.Department()
                    {
                        Department_Name = textBoxDepartment.Text,
                        Department_Description = textBox1.Text
                    };
                    instance.DataLink.Departments.InsertOnSubmit(dept);

                    instance.DataLink.SubmitChanges();

                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                    break;

                case false:


                    DAL.Department editdept = instance.DataLink.Departments.SingleOrDefault(x => x.Department_Id == deptid);

                    editdept.Department_Name = textBoxDepartment.Text;
                    editdept.Department_Description = textBox1.Text;

                  

                    instance.DataLink.SubmitChanges();

                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                    break;


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        
    }
}
