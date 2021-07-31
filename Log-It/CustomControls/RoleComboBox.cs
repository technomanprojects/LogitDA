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

    public partial class RoleComboBox : ComboBox
    {
        EntityObjCollection items;
        DAL.Role selectedEntity;

        public RoleComboBox()
        {
            InitializeComponent();
        }

        public DAL.Role SelectedEntity
        {
            get { return selectedEntity; }
            set { selectedEntity = value; }
        }




        public new int SelectedIndex
        {
            get { return base.SelectedIndex; }
            set { base.SelectedIndex = value; }
        }
          protected override void OnSelectedIndexChanged(EventArgs e)
        {

            base.OnSelectedIndexChanged(e);
        }

          protected override void OnSelectionChangeCommitted(EventArgs e)
          {


              if (this.SelectedItem == null)
              {
                  return;
              }

              string s = this.SelectedItem.ToString();

              if (items.Count > 0)
              {
                  selectedEntity = items[s];
              }
              base.OnSelectionChangeCommitted(e);

          }


        protected override void OnSelectedValueChanged(EventArgs e)
        {
            base.OnSelectedValueChanged(e);
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



        public RoleComboBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public class EntityObjCollection
        {
            Dictionary<string, DAL.Role > entityDictionary = new Dictionary<string, DAL.Role>();
            ComboBox listBox;
            public EntityObjCollection(ComboBox listBox)
            {
                this.listBox = listBox;
            }

            public int Add(DAL.Role masterBaseEntity)
            {
                int result = -1;
                try
                {
                    if (!entityDictionary.ContainsValue(masterBaseEntity))
                    {
                        entityDictionary.Add(masterBaseEntity.RoleName , masterBaseEntity);
                        result = listBox.Items.Add(masterBaseEntity.RoleName);
                        listBox.Refresh();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(((Form)listBox.Parent), e.Message);
                }
                return result;
            }

            public void Remove(DAL.Department masterBaseEntity)
            {
                try
                {
                    if (masterBaseEntity != null)
                    {
                        entityDictionary.Remove(masterBaseEntity.Department_Name);
                        listBox.Items.Remove(masterBaseEntity.Department_Name);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(((Form)listBox.Parent), e.Message);
                }
            }



            public DAL.Role this[string name]
            {
                get
                {
                    DAL.Role masterBaseEntity = null;
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

            public bool Contains(DAL.Role masterBaseEntity)
            {
                return entityDictionary.ContainsKey(masterBaseEntity.RoleName);
            }
        }
    }
}

