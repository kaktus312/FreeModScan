using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;

namespace FreeModScan
{
    public class Device:IEquatable<Device>
    {
        string _deviceName;
        public string deviceName{ get {return _deviceName; } set {_deviceName=value; } }

        byte _deviceAdress;
        public byte deviceAdress { get { return _deviceAdress; } set { _deviceAdress = value; } }

        public BindingList<Register> Registers = new BindingList<Register>();

        public Device(byte adress, string name){
            _deviceAdress = adress;
            _deviceName = name;
            Registers.ListChanged += new ListChangedEventHandler(Registers_ListChanged);
        }

        bool _status=true;    //Статус (опрашивать/не опрашивать) 
        public bool Status { get { return _status; } set { _status = value; } }


        private void Registers_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    //Add item here.
                    //MessageBox.Show("Register Added");
                    //MessageBox.Show(Registers.Last().Val.ToString("X2"));
                    break;

                case ListChangedType.ItemChanged:
                    //Change node associated with this item
                    MessageBox.Show("Register State changed");
                    break;

                case ListChangedType.ItemMoved:
                    //Parent changed.
                    MessageBox.Show("Register moved");
                    break;

                case ListChangedType.ItemDeleted:
                    MessageBox.Show("Register deleted");
                    break;

                case ListChangedType.Reset:
                    //This reset all data and control need to refill all data.
                    MessageBox.Show("Register reset");
                    break;
            }
        }

        public bool Equals(Device obj)
        {
            return this.deviceAdress.Equals(obj.deviceAdress) && this.deviceName.Equals(obj.deviceName);
        }

    }
}
