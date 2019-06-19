using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FreeModScan
{
    public partial class WriteRegForm : Form
    {
        Register currReg;

        public WriteRegForm()
        {
            InitializeComponent();

            cbConnectionList.DataSource = MainForm._conns;
            cbConnectionList.DisplayMember = "ConnName";

            currReg = MainForm.currRegister;

            cbRegisterType.SelectedIndex = (int) currReg.Type-1;

            tbRegisterNum.Text = (currReg.Adress%100000).ToString();


            numVal.Text = currReg.A.ToString();

        }

        private void btnApply_Click(object sender, EventArgs e)
        {

            byte command = 0x00;
            switch ((Register.RegType) cbRegisterType.SelectedIndex+1)
            {
                case Register.RegType.COIL:
                    command = 0x05;
                    break;
                case Register.RegType.INPUT_REGISTER:
                    command = 0x06;
                    break;
                default:
                    return;
            }

            MainForm.currConn.CreateRequest(MainForm.currDevice.deviceAdress, command, (uint)numVal.Value, (uint)currReg.Adress);          
        }

        private void cbConnectionList_SelectedIndexChanged(object sender, EventArgs e)
        {
                cbDeviceList.DataSource = MainForm._conns[cbConnectionList.SelectedIndex].Devices;
                cbDeviceList.DisplayMember = "deviceName";
                cbDeviceList.SelectedIndex = 0;

        }

        private void btnClearValue_Click(object sender, EventArgs e)
        {
            numVal.Value=0;
        }

        private void cbRegisterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selItem = cbRegisterType.SelectedIndex+1;
            if ((selItem ==(int) Register.RegType.DIGITAL_INPUT) || (selItem ==(int) Register.RegType.HOLDING_REGISTER))
                MessageBox.Show("Выбранный тип регистра только для чтения", "Ошибка");
        }

    }
}
