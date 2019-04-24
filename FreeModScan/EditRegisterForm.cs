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
    public partial class EditRegisterForm : Form
    {
        BindingList<Connection> _conns;

        public EditRegisterForm(BindingList<Connection> conns, int currConIndex, int currDevIndex)
            : this(conns)
        {
            cbConnectionList.SelectedIndex = currConIndex;
            cbDeviceList.SelectedIndex = currDevIndex;
        }

        public EditRegisterForm(BindingList<Connection> conns)
        {
            InitializeComponent();
            _conns = conns;
            cbConnectionList.DataSource=conns;
            cbConnectionList.DisplayMember = "ConnName";
            cbConnectionList.ValueMember = "ConnName";
            cbConnectionList.SelectedIndex = 0;
        }

        private void cbConnectionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbDeviceList.DataSource = _conns[cbConnectionList.SelectedIndex].Devices;
            cbDeviceList.DisplayMember = "deviceName";
            cbDeviceList.ValueMember = "deviceName";
            cbDeviceList.SelectedIndex = 0;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Connection selectedConn = _conns[cbConnectionList.SelectedIndex];
            Device selectedDev = selectedConn.Devices[cbDeviceList.SelectedIndex];
            MainForm.addRegisters(selectedDev, tbRegisterList.Text, cbRegisterType.SelectedIndex);
        }

    }
}
