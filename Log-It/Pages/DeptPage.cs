using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace Log_It.Pages
{
    public partial class DeptPage : ControlPage
    {
        public delegate void SetID(int id);
        public event SetID IDSet;
        int width;
        BAL.LogitInstance instance;

        public DeptPage(BAL.LogitInstance instance)
        {
            InitializeComponent();
            this.instance = instance;
            RefreshPage();
        }
        public override void RefreshPage()
        {
            base.RefreshPage();
            int wi = dataGridView1.Size.Width;
            if (instance.DataLink.Departments.Count() > 0)
            {
                bindingSource1.DataSource = instance.DataLink.Departments.OrderByDescending(p => p.Department_Id).Take(50);

                dataGridView1.DataSource = bindingSource1;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Refresh();

                this.dataGridView1.Sort(this.dataGridView1.Columns[1], ListSortDirection.Descending);

            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (IDSet != null)
                {
                    IDSet((int)dataGridView1.Rows[e.RowIndex].Cells[0].Value);
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
    }
}
