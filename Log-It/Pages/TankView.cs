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
    public partial class TankView : UserControl
    {
        public delegate void FormClose();
        public event FormClose Close;

        private bool tanksCreated = false;
        private CustomControls.BarPack[] tanks = null;
        private const int PerLineControls = 8;
        private static TankView instance = null;
        public static TankView Instance()
        {
            if (instance == null)
                instance = new TankView();

            return instance;
        }
        public TankView()
        {
            InitializeComponent();
        }

        public void CreateTanks(int n)
        {
            try
            {

                if (this.tanksCreated) return;
                this.tanks = new CustomControls.BarPack[n];
                int index = 0;
                int noOfLines = (int)Math.Ceiling((float)n / PerLineControls);
                Panel[] panels = new Panel[noOfLines];
                Panel p;
                CustomControls.BarPack tank;
                for (int i = 0; i < noOfLines; i++)
                {
                    p = new Panel
                    {
                        Height = 250,
                        Dock = DockStyle.Top
                    };
                    panels[i] = p;
                    for (int j = 0; j < PerLineControls; j++)
                    {
                        tank = new CustomControls.BarPack
                        {
                            Dock = DockStyle.Left
                        };
                        Controls.Add(tank);
                        tanks[index] = tank;
                        index++;
                        if (index >= n) break;

                    }
                }
                for (int i = noOfLines - 1; i >= 0; i--)
                    Controls.Add(panels[i]);
                this.tanksCreated = true;
            }
            catch (Exception m)
            {
                var st = new StackTrace();
                var sf = st.GetFrame(0);

                var currentMethodName = sf.GetMethod();
                Technoman.Utilities.EventClass.ErrorLog(Technoman.Utilities.EventLog.Error, m.Message + " Method Name: " + currentMethodName, "System");
            }
        }

        public void ApplyFont(Font f)
        {
            foreach (Control c in this.tanks)
                c.Font = f;

        }

        public CustomControls.BarPack[] Tanks
        {
            get
            {
                return this.tanks;
            }
        }

        private void TankView_Load(object sender, EventArgs e)
        {

        }

        private void TankView_VisibleChanged(object sender, EventArgs e)
        {

            Close?.Invoke();
        }
    }
}
