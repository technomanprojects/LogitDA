﻿using System;
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

        private readonly int deptID;
        private readonly BAL.LogitInstance instance;       

        public DeptTask(int id, BAL.LogitInstance instance)
        {
            this.deptID = id;             
            this.instance = instance;
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Forms.AddDepartment form = new Forms.AddDepartment(true,instance,deptID);
            if (form.ShowDialog() == DialogResult.OK)
            {
                AddDept?.Invoke();
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            ModifiedDept?.Invoke();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            DeleteDept?.Invoke();
        }
    }
}
