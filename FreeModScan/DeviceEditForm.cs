using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeModScan
{
    public partial class DeviceEditForm : Form
    {
        private Device dev;
        private Connection conn;

        public DeviceEditForm(Connection conn)
        {  
            InitializeComponent();
            this.Text = "Добавить устройство";
            this.conn = conn;
        }

        public DeviceEditForm(ref Device d)
        {
            InitializeComponent();
            this.Text = "Изменение параметров устройств";
            dev = d;
            nUpDnDevicAdress.Value = d.deviceAdress;
            tbDeviceName.Text = d.deviceName;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (dev == null){
                dev = new Device((byte)nUpDnDevicAdress.Value, tbDeviceName.Text);
                conn.Devices.Add(dev);
            } 
            dev.deviceAdress = (byte)nUpDnDevicAdress.Value;
            dev.deviceName = tbDeviceName.Text;
        }
    }
}
