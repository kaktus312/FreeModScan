using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace FreeModScan
{
    public class Device:IEquatable<Device>
    {

        string _deviceName;
        public string deviceName{ get {return _deviceName; } set {_deviceName=value; } }

        byte _deviceAdress;
        public byte deviceAdress { get { return _deviceAdress; } set { _deviceAdress = value; } }

        public BindingList<Register> Registers = new BindingList<Register>();
       
        public delegate void DeviceEventHandler(Device d);
        public event DeviceEventHandler Create;
        public event DeviceEventHandler StateChanged;
        public event DeviceEventHandler Delete;

        public delegate void DeviceErrorHandler(Exception err);
        public event DeviceErrorHandler Error;

        public Device()
        {
            //для сериализации (при сохранении в XML) необходим конструктор без параметров 
            Registers.ListChanged += new ListChangedEventHandler(Registers_ListChanged);

        }

        public Device(byte adress, string name):this(){
            _deviceAdress = adress;
            _deviceName = name;

        }

        bool _status=true;    //Статус (опрашивать/не опрашивать) 
        public bool Status { get { return _status; } set { _status = value; } }


        private void Registers_ListChanged(object sender, ListChangedEventArgs e)
        {
            Register reg = null;
            int regsNum = Registers.Count();//поличество подключений после изменения

            if (e.NewIndex != regsNum)
                reg = Registers[e.NewIndex];//объект с которым производились манипуляции

            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    //Add item here.
                    reg.OnCreate();
                    MessageBox.Show("Register Added");
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

        //Методы On.....() - для запуска события из внешних классов
        public void OnCreate()
        {
            if (Create != null) Create(this);
        }
        public void OnStateChanged()
        {
            if (StateChanged != null) StateChanged(this);
        }
        public void OnDelete()
        {
            if (Delete != null) Delete(this);
        }

        public void OnError(Exception e)
        {
            if (Error != null) Error(e);
        }

        public void addRegisters(string regString, int rType, int dType, int bOrder = 1)
        {
            MatchCollection matches = Regex.Matches(regString, "((?<s>\\d+)-(?<e>\\d+))|(?<r>\\d+)");

            foreach (Match match in matches)
            {
                Register tmpReg = null;
                int startIndex = 0;
                int endIndex = -1;

                if (match.Groups["r"].Value != "")
                {
                    //MessageBox.Show(uint.Parse(match.Groups["r"].Value).ToString());
                    startIndex = endIndex = int.Parse(match.Groups["r"].Value);
                }

                if ((match.Groups["s"].Value != "") && (match.Groups["e"].Value != ""))
                {
                    startIndex = int.Parse(match.Groups["s"].Value);
                    endIndex = int.Parse(match.Groups["e"].Value);
                }

                for (int i = startIndex; i <= endIndex; i++)
                {
                    tmpReg = new Register(deviceName, (uint)i, (Register.RegType)rType, (Register.DataType)dType, (Register.ByteOrder)bOrder);
                    tmpReg.Create += new Register.RegisterEventHandler(RegisterAdded);//подписываемся на событие Создания
                    //MessageBox.Show(i.ToString());
                    //dev.Registers.Add(tmpReg);
                    Registers.Add(tmpReg);
                }
            }
        }

        private void RegisterAdded(Register r)
        {
            MainForm.console.Add("Class Device: register added\n");
            //MessageBox.Show("Register was Added");
        }
    }
}
