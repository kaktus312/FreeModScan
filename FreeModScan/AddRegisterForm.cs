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
    public partial class AddRegisterForm : Form
    {
        //BindingList<Connection> _conns;

        public AddRegisterForm(BindingList<Connection> conns, int currConIndex, int currDevIndex)
            : this(conns)
        {
            cbConnectionList.SelectedIndex = currConIndex;
            cbDeviceList.SelectedIndex = currDevIndex;
        }

        public AddRegisterForm(BindingList<Connection> conns)
        {
            InitializeComponent();
            //_conns = conns;
            cbConnectionList.DataSource=conns;
            cbConnectionList.DisplayMember = "ConnName";
            cbConnectionList.ValueMember = "ConnName";
            cbConnectionList.SelectedIndex = 0;

            cbDataType.DataSource =  Enum.GetNames(typeof(Register.DataType));
            cbRepresent.DataSource = Enum.GetNames(typeof(Register.Representation));
            cbByteOrder.DataSource = Enum.GetNames(typeof(Register.ByteOrder));

            cbConnectionList.DataSource = MainForm._conns;
            cbConnectionList.DisplayMember = "ConnName";
            cbByteOrder.SelectedIndex = (int)Register.ByteOrder.BIGENDIAN;
        }

        private void cbConnectionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbDeviceList.DataSource = MainForm._conns[MainForm.currConn].Devices;
            cbDeviceList.DisplayMember = "deviceName";
            cbDeviceList.ValueMember = "deviceName";
            cbDeviceList.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Connection selectedConn = _conns[cbConnectionList.SelectedIndex];
            Connection selectedConn = MainForm._conns[MainForm.currConn];

            Device selectedDev = selectedConn.Devices[MainForm.currDevice];
            MessageBox.Show(((Register.RegType)cbRegisterType.SelectedIndex+1).ToString());
            selectedDev.addRegisters(tbRegisterList.Text, cbRegisterType.SelectedIndex + 1, cbDataType.SelectedIndex, cbByteOrder.SelectedIndex);
            //MainForm.addRegisters(selectedDev, tbRegisterList.Text, cbRegisterType.SelectedIndex+1, cbDataType.SelectedIndex, cbByteOrder.SelectedIndex);
        }

    }
}
