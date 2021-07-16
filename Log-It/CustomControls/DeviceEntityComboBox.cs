using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log_It.CustomControls
{
    public partial class DeviceEntityComboBox : ComboBox
    {
        EntityObjCollection items;
        DAL.Device_Config selectedEntity;

        public DeviceEntityComboBox()
        {
            InitializeComponent();
        }

        public DAL.Device_Config SelectedEntity
        {
            get { return selectedEntity; }
        }


        public new int SelectedIndex
        {
            get { return base.SelectedIndex; }
            set { base.SelectedIndex = value; }
        }
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            if (this.SelectedItem != null)
            {
                selectedEntity = items[SelectedIndex];
                base.OnSelectedIndexChanged(e);
            }
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            if (this.SelectedItem != null)
            {

                base.OnDoubleClick(e);
            }

        }

        protected override void OnSelectedValueChanged(EventArgs e)
        {
            if (this.SelectedItem != null)
            {

                selectedEntity = items[SelectedIndex];
                base.OnSelectedValueChanged(e);
            }
            
        }
        public new EntityObjCollection Items
        {
            get
            {
                if (items == null)
                {
                    items = new EntityObjCollection(this);
                }
                return items;
            }
        }        

        public DeviceEntityComboBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }



           public class EntityObjCollection
        {
            Dictionary<string, DAL.Device_Config> entityDictionary = new Dictionary<string, DAL.Device_Config>();
            ComboBox listBox;
            public EntityObjCollection(ComboBox listBox)
            {
                this.listBox = listBox;          
                
            }
               
            public int Add(DAL.Device_Config masterBaseEntity)
            {
                int result = -1;
                try
                {
                    if (!entityDictionary.ContainsValue(masterBaseEntity))
                    {
                        entityDictionary.Add(masterBaseEntity.Instrument , masterBaseEntity);
                        result = listBox.Items.Add(masterBaseEntity.Instrument);
                        listBox.Refresh();
                    }                   
                }
                catch (Exception e)
                {                   
                    MessageBox.Show(((Form)listBox.Parent), e.Message);
                }
                return result;
            }

            public void Remove(DAL.Device_Config masterBaseEntity)
            {
                try
                {
                    if (masterBaseEntity != null)
                    {
                        entityDictionary.Remove(masterBaseEntity.Instrument);
                        listBox.Items.Remove(masterBaseEntity.Location);    
                    }                    
                }
                catch (Exception e)
                {
                    MessageBox.Show(((Form)listBox.Parent), e.Message);
                }
            }

            public DAL.Device_Config this[int Index]
            {
                get
                {
                    DAL.Device_Config masterBaseEntity = null;
                    try
                    {
                        string a = listBox.Items[Index].ToString();

                        masterBaseEntity = entityDictionary[a];
                    }
                    catch (Exception m)
                    {
                    }
                    return masterBaseEntity;
                }
            }

   

            public DAL.Device_Config this[string name]
            {
                get
                {
                    DAL.Device_Config masterBaseEntity = null;
                    try
                    {
                        masterBaseEntity = entityDictionary[name];
                    }
                    catch (Exception)
                    {
                    }
                    return masterBaseEntity;
                }
            }

            public void Clear()
            {
                entityDictionary.Clear();
                listBox.Items.Clear();                
            }

            public int Count
            {
                get { return listBox.Items.Count; }
            
            }

            public EntityObjCollection GetSelectedAccount()
            {
                //AccountCollection accountCollection = new AccountCollection();
                //foreach (Account account in entityDictionary.Values)
                //{
                //    accountCollection.Add(account);
                //}
                return null; // accountCollection;
            }

            public bool Contains(DAL.Device_Config masterBaseEntity)
            {
                return entityDictionary.ContainsKey(masterBaseEntity.Instrument);
            }
        }
    }
}
