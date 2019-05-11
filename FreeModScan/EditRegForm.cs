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
    public partial class EditRegForm : Form
    {
        Register currReg;

        public EditRegForm()
        {
            InitializeComponent();
            cbDataType.DataSource = Enum.GetNames(typeof(Register.DataType));
            cbRepresent.DataSource = Enum.GetNames(typeof(Register.Representation));
            cbByteOrder.DataSource = Enum.GetNames(typeof(Register.ByteOrder));

            cbConnectionList.DataSource = MainForm._conns;
            cbConnectionList.DisplayMember = "ConnName";

            //currReg = MainForm._conns[MainForm.currConn].Devices[MainForm.currDevice].Registers[MainForm.currRegister];
            currReg = MainForm.currRegister;
            //cbConnectionList.SelectedIndex = MainForm._conns.currConn;
            //cbDeviceList.SelectedIndex = MainForm.currDevice;
            cbRegState.Checked = currReg.Status;
            cbDataType.SelectedIndex = (int) currReg.dataType;
            cbRegisterType.SelectedIndex = (int) currReg.Type-1;
            cbRepresent.SelectedIndex = (int)currReg.Represent;
            tbRegisterNum.Text = (currReg.Adress%100000).ToString();
            cbUseMults.Checked = currReg.UseMults;
            tbA.Enabled = tbB.Enabled = cbUseMults.Checked;
            tbA.Text = currReg.A.ToString();
            tbB.Text = currReg.B.ToString();
            cbByteOrder.SelectedIndex = (int)currReg.byteOrder;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            currReg.Status = cbRegState.Checked;
            currReg.dataType = (Register.DataType) cbDataType.SelectedIndex;
            currReg.Type = (Register.RegType) cbRegisterType.SelectedIndex+1;
            currReg.Represent = (Register.Representation) cbRepresent.SelectedIndex;
            currReg.Adress = uint.Parse(tbRegisterNum.Text);
            currReg.UseMults = cbUseMults.Checked;
            if (cbUseMults.Checked)
            {
                currReg.A = float.Parse(tbA.Text);
                currReg.B = float.Parse(tbB.Text);
            }
            currReg.byteOrder = (Register.ByteOrder)cbByteOrder.SelectedIndex;
            switch (currReg.dataType){
                case Register.DataType.Int16:
                    currReg.ValArr = new byte[2];
                    break;                
                case Register.DataType.Int32:
                case Register.DataType.Float:
                    currReg.ValArr = new byte[4];
                    break;
                case Register.DataType.Int64:
                case Register.DataType.Double:
                default:
                    currReg.ValArr = new byte[8];
                    break;
            }
            

            if (currReg.devName != cbDeviceList.Text) {     //было изменено устройство и/или подключение
                currReg.devName = cbDeviceList.Text;
                Connection destConn = MainForm._conns[cbConnectionList.SelectedIndex];
                Device destDevice = destConn.Devices[cbDeviceList.SelectedIndex];
                destDevice.Registers.Add(currReg);
                //MainForm._conns[MainForm.currConn].Devices[MainForm.currDevice].Registers.RemoveAt(MainForm.currRegister);
            }

            currReg.OnChange();
        }

        private void cbConnectionList_SelectedIndexChanged(object sender, EventArgs e)
        {
                cbDeviceList.DataSource = MainForm._conns[cbConnectionList.SelectedIndex].Devices;
                cbDeviceList.DisplayMember = "deviceName";
                cbDeviceList.SelectedIndex = 0;

        }

        private void cbUseMults_CheckedChanged(object sender, EventArgs e)
        {
            tbA.Enabled = tbB.Enabled = cbUseMults.Checked;
        }
    }
}
