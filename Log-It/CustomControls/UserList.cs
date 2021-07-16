using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Log_It.CustomControls
{
    public partial class UserList : ComboBox
    {
        EntityObjCollection items;
        DAL.User selectedEntity;
        
        public UserList()
        {
            InitializeComponent();
        }
        public DAL.User SelectedEntity
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

            base.OnSelectedIndexChanged(e);
        }

        protected override void OnClick(EventArgs e)
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




            base.OnClick(e);
        }

        protected override void OnSelectedValueChanged(EventArgs e)
        {
            base.OnSelectedValueChanged(e);
        }

        public UserList(System.ComponentModel.IContainer container)
        {
            container.Add(this);

            InitializeComponent();

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

        public class EntityObjCollection
        {
            Dictionary<string, DAL.User> entityDictionary = new Dictionary<string, DAL.User>();
            ComboBox listBox;
            public EntityObjCollection(ComboBox listBox)
            {
                this.listBox = listBox;
            }

            public int Add(DAL.User masterBaseEntity)
            {
                int result = -1;
                try
                {
                    if (!entityDictionary.ContainsValue(masterBaseEntity))
                    {
                        entityDictionary.Add(masterBaseEntity.User_Name, masterBaseEntity);
                        result = listBox.Items.Add(masterBaseEntity.User_Name);
                        listBox.Refresh();
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(((Form)listBox.Parent), e.Message);
                }
                return result;
            }

            public void Remove(DAL.User masterBaseEntity)
            {
                try
                {
                    if (masterBaseEntity != null)
                    {
                        entityDictionary.Remove(masterBaseEntity.User_Name);
                        listBox.Items.Remove(masterBaseEntity.User_Name);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(((Form)listBox.Parent), e.Message);
                }
            }



            public DAL.User this[string name]
            {
                get
                {
                    DAL.User masterBaseEntity = null;
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

            public bool Contains(DAL.User masterBaseEntity)
            {
                return entityDictionary.ContainsKey(masterBaseEntity.User_Name);
            }
        }
    }
}
