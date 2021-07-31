using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log_It.Pages.TaskPanel
{
    public partial class DeptTask : TaskControl
    {
        public delegate void DeptAdd();
        public event DeptAdd AddDept;

        public delegate void DeptModfied();
        public event DeptModfied ModifiedDept;

        public delegate void DeptDelete();
        public event DeptDelete DeleteDept;
        int deptID;
        BAL.LogitInstance instance;       

        public DeptTask(int id, BAL.LogitInstance instance)
        {

            this.deptID = id;             
            this.instance = instance;
            InitializeComponent();
           
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Log_It.Forms.AddDepartment form = new Forms.AddDepartment(true,instance,deptID);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (AddDept != null)
                {
                    AddDept();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {


            if (ModifiedDept != null)
            {
                ModifiedDept();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (DeleteDept != null)
            {
                DeleteDept();
            }
        }
    }
}
