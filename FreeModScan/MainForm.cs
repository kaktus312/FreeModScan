using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace FreeModScan
{
    public partial class MainForm : Form
    {
        private const string NO_CONN_ADD_MSG = "Для добавления устройства создайте новое подключение";
        private const string EMPTY_CONN_LIST_MSG = "Список подключений пуст";
        private const string NO_DEV_ADD_MSG = "Для добавления регистров создайте новое устройство или выберите существующее";
        private const string NO_REGS_MSG = "Регистры устройства не определены";
        private const string NO_REGS_SELECT_MSG = "Регистр не выбран";
        private const string NO_ACTIVE_CONN_MSG =  "Нет активных подключений";
        private const string MSG_INFO_TITLE = "Информация";

        public static BindingList<Connection> _conns = new BindingList<Connection>();
        public static int currConn = -1;//текущее соединение
        public static int currDevice = -1;//текущее устройство
        public static int currRegister = -1;//текущий регистр

        public static BindingList<string> console = new BindingList<string>();

        public static CRC16 CRC = new CRC16();

        public MainForm()
        {
            InitializeComponent();
            _conns.ListChanged += new ListChangedEventHandler(Connections_ListChanged);
            console.ListChanged += new ListChangedEventHandler(ConsoleMsg_Changes);
            cbConnectionList.DataSource = _conns;
            cbConnectionList.DisplayMember = "PortName";
            cbConnectionList.ValueMember = "ConnName";
            
            PollTimer.Interval = int.Parse(tsTbPollInterval.Text);

            cbDataType.DataSource = Enum.GetNames(typeof(Register.DataType));
            cbDataType.SelectedIndex = (int) Register.DataType.Int16;
        }

        private void Connections_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    //Add item here.
                    if (tvConnectionTree.Nodes[0].Text == EMPTY_CONN_LIST_MSG)
                        tvConnectionTree.Nodes.Clear();

                    Connection conn = _conns.Last();
                    tvConnectionTree.Nodes.Add(conn.ConnName + " - " + conn.Port.PortName + " - " + conn.statusString);
                    currConn = e.NewIndex;

                    break;

                case ListChangedType.ItemChanged:
                    //Change node associated with this item
                    MessageBox.Show("State changed");
                    break;

                case ListChangedType.ItemMoved:
                    //Parent changed.
                    MessageBox.Show("Parent changed");
                    break;

                case ListChangedType.ItemDeleted:
                    //Item removed
                    //MessageBox.Show("Deleted");
                    if (_conns.Count==0) 
                        setConnTreeDefaults();
                    else
                        tvConnectionTree.Nodes[e.NewIndex].Remove();
                    break;

                case ListChangedType.Reset:
                    //This reset all data and control need to refill all data.
                    //MessageBox.Show("Reset");
                    setConnTreeDefaults();
                    break;
            }
        }

        private void ConsoleMsg_Changes(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    rtbConsole.Text = console.Last() + rtbConsole.Text;
                    //rtbConsole.ScrollToCaret;
                    break;
            }
        }

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnConsoleClear_Click(object sender, EventArgs e)
        {
            console.Clear();
            rtbConsole.Clear();
        }

        private void tsmiLogShow_Click(object sender, EventArgs e)
        {
            openLogDialog.ShowDialog(this);
        }

        private void tsmiMapOpen_Click(object sender, EventArgs e)
        {
            openMapDialog.ShowDialog(this);
        }

        private void tsmiMapSaveAs_Click(object sender, EventArgs e)
        {
            saveFileDialog.ShowDialog(this);
        }

        private void tsmiCut_Click(object sender, EventArgs e)
        {

        }

        private void tsmiCopy_Click(object sender, EventArgs e)
        {
            //Clipboard.SetText();
        }

        private void tsMiOptions_Click(object sender, EventArgs e)
        {
            SettingsForm form = new SettingsForm();
            form.ShowDialog(this);
        }

        private void ConnectionAddForm_Show(object sender, EventArgs e)
        {
            ConnectionForm connForm = new ConnectionForm();
            connForm.ShowDialog(this);
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            ConnectAll();
        }

        public void ConnectAll()
        {
            foreach (Connection conn in _conns)
            {
                if ((conn != null) && (!conn.status))
                    conn.Open();
                tvConnectionStateUpdate(conn);
                //rtbConsole.Text += "** " + DateTime.Now.ToString() + " - " + _conns[currConn].ConnName + " : Соединение COM подключено. **"+"\n";
            }
        }

        private void tvConnectionStateUpdate(Connection conn)
        {
            TreeNode currNode = tvConnectionTree.Nodes[_conns.IndexOf(conn)];
            currNode.Text = conn.ConnName + " - " + conn.Port.PortName + " - " + conn.statusString;
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            DisconnectAll();
        }

        public void DisconnectAll()
        {
            foreach (Connection conn in _conns)
            {
                if ((conn != null) && (conn.status))
                    conn.Close();
                tvConnectionStateUpdate(conn);
                //rtbConsole.Text += "** " + DateTime.Now.ToString() + " - " + _conns[currConn].ConnName + " : Соединение COM отключено. **" + "\n";
            }
        }

        private void tsMiMul_CheckStateChanged(object sender, EventArgs e)
        {
            dgvTable.Columns["MulACol"].Visible = (tsMiMul.Checked) ? true : false;
            dgvTable.Columns["MulBCol"].Visible = (tsMiMul.Checked) ? true : false;

        }

        private void tsmiFormat_CheckStateChanged(object sender, EventArgs e)
        {
            dgvTable.Columns["FormatCol"].Visible = (tsmiFormat.Checked) ? true : false;
        }

        private void tsmiAdress_CheckStateChanged(object sender, EventArgs e)
        {
            dgvTable.Columns["AdressCol"].Visible = (tsmiAdress.Checked) ? true : false;
        }

        private void tsmiDevice_CheckStateChanged(object sender, EventArgs e)
        {
            dgvTable.Columns["DeviceCol"].Visible = (tsmiDevice.Checked) ? true : false;
        }

        private void btnCellAddSide_Click(object sender, EventArgs e)
        {
            if (_conns.Count > 0)
            {
                
                Connection currCon = _conns[cbConnectionList.SelectedIndex];
                TreeNode currNode = tvConnectionTree.Nodes[cbConnectionList.SelectedIndex];

                byte devAdress = byte.Parse(tbDeviceId.Text);
                Device dev = new Device(devAdress, tbDeviceName.Text);

                int devId = currCon.Devices.IndexOf(dev);
                MessageBox.Show("Device ID=" + devId.ToString());

                //if (currCon.Devices.Contains(dev))
                if (devId>=0)
                {
                    MessageBox.Show("Device Exists"+ devId.ToString());
                    dev = currCon.Devices[devId];
                } else
                {
                    currCon.Devices.Add(dev);
                    currNode.Nodes.Add(tbDeviceId.Text + ": " + tbDeviceName.Text);
                    
                }

                addRegisters(dev, tbRegisterList.Text, cbRegisterType.SelectedIndex+1, cbDataType.SelectedIndex);

                if (!currNode.IsExpanded)
                    currNode.Expand();
                tvConnectionTree.SelectedNode = currNode.LastNode;
                dgvTable.Rows[0].Cells[0].Selected = true;
                currDevice = currNode.LastNode.Index;
                currRegister = 0;
            }
            else
            {
                MessageBox.Show(NO_CONN_ADD_MSG, MSG_INFO_TITLE);
            }
        }

        static public void addRegisters(Device dev, string regString, int rType, int dType, int bOrder=0)
        {
            MatchCollection matches = Regex.Matches(regString, "((?<s>\\d+)-(?<e>\\d+))|(?<r>\\d+)");

            foreach (Match match in matches)
            {
                Register tmpReg = null;
                if (match.Groups["r"].Value != "")
                {
                    //MessageBox.Show(uint.Parse(match.Groups["r"].Value).ToString());
                    uint i = uint.Parse(match.Groups["r"].Value);

                    tmpReg = new Register(dev.deviceName, i, (Register.RegType)rType, (Register.DataType)dType, (Register.ByteOrder)bOrder);
                    if  (tmpReg!=null) dev.Registers.Add(tmpReg);
                }
                if ((match.Groups["s"].Value != "") && (match.Groups["e"].Value != ""))
                {
                    for (uint i = uint.Parse(match.Groups["s"].Value); i <= uint.Parse(match.Groups["e"].Value); i++)
                    {
                         tmpReg = new Register(dev.deviceName, i, (Register.RegType)rType, (Register.DataType)dType, (Register.ByteOrder)bOrder);
                        //MessageBox.Show(i.ToString());
                        dev.Registers.Add(tmpReg);
                    }
                }

            }
        }

        private void удалитьВсеПодключенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_conns.Count > 0)
            {
                DialogResult res = MessageBox.Show("Удаление всех подключений повлечёт удаление всех регистров и устройств. Продолжить?",
                                 "Удаление всех устройств", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    _conns.Clear();
                    cbConnectionList.Text = tvConnectionTree.SelectedNode.Text;
                }
                    
            }
        }

        private void setConnTreeDefaults()
        {
            tvConnectionTree.Nodes.Clear();
            tvConnectionTree.Nodes.Add(EMPTY_CONN_LIST_MSG);
            cbConnectionList.Text = EMPTY_CONN_LIST_MSG;
            tbDeviceName.Text = "devName";
            tbDeviceId.Text = Convert.ToString(1);
            tvConnectionTree.SelectedNode = tvConnectionTree.Nodes[0];            
        }

        private void подключитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_conns.Count > 0)
            {
                int currConn = 0;
                if (tvConnectionTree.SelectedNode.Level==0)
                    currConn = tvConnectionTree.SelectedNode.Index;
                    
                if (tvConnectionTree.SelectedNode.Level == 1)
                     currConn = tvConnectionTree.SelectedNode.Parent.Index;

                _conns[currConn].Open();
                tvConnectionStateUpdate(_conns[currConn]);
                //if (_conns[currConn].Port.IsOpen)
                //    rtbConsole.Text += "** " + DateTime.Now.ToString() + " - " + _conns[currConn].ConnName + " : Соединение COM подключено. **" + "\n";
                //else
                //    rtbConsole.Text += "** " + DateTime.Now.ToString() + " - " + _conns[currConn].ConnName + " : Соединение COM неподключено. **" + "\n";
            }
                

        }

        private void отключитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_conns.Count > 0)
            {
                int currConn = 0;
                if (tvConnectionTree.SelectedNode.Level == 0)
                    currConn = tvConnectionTree.SelectedNode.Index;

                if (tvConnectionTree.SelectedNode.Level == 1)
                    currConn = tvConnectionTree.SelectedNode.Parent.Index;

                _conns[currConn].Close();
                tvConnectionStateUpdate(_conns[currConn]);
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_conns.Count > 0)
            {
                
                int lvl = tvConnectionTree.SelectedNode.Level;
                int selectedNodeIndex = tvConnectionTree.SelectedNode.Index;
                if (lvl > 0)
                {                  
                    TreeNode ParentNode = tvConnectionTree.SelectedNode.Parent;
                    MessageBox.Show("Удаляем устройство #" + selectedNodeIndex + " в соединении " + ParentNode.Text);
                    _conns.ElementAt(ParentNode.Index).Devices.RemoveAt(selectedNodeIndex);
                    ParentNode.Nodes[selectedNodeIndex].Remove();
                }
                else
                {
                    _conns.RemoveAt(selectedNodeIndex);
                    cbConnectionList.Text = tvConnectionTree.SelectedNode.Text;
                }

            } else
            {
                
            }
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_conns.Count > 0)
            {
                int selectedNodeIndex = tvConnectionTree.SelectedNode.Index;
                if (tvConnectionTree.SelectedNode.Level==0) {
                    Connection currConn = _conns[selectedNodeIndex];
                    ConnectionForm connForm = new ConnectionForm(ref currConn);
                    DialogResult res = connForm.ShowDialog(this);
                    if (res == DialogResult.OK)
                    {
                        MessageBox.Show("Обновляем параметры соединения");
                        tvConnectionTree.SelectedNode.Text = currConn.ConnName + " - " + currConn.Port.PortName + " - " + currConn.statusString;
                    }
                } else {
                    TreeNode ParentNode = tvConnectionTree.SelectedNode.Parent;
                    Device currDev = _conns.ElementAt(ParentNode.Index).Devices[selectedNodeIndex];
                    DeviceEditForm devEditForm = new DeviceEditForm(ref currDev);
                    DialogResult res = devEditForm.ShowDialog(this);
                    if (res==DialogResult.OK){
                        MessageBox.Show("Обновляем параметры устройства");
                        tvConnectionTree.SelectedNode.Text = currDev.deviceAdress + ": " + currDev.deviceName;
                    }
                }

            }
        }

        private void tvConnectionTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_conns.Count > 0) {

                if (e.Node.Level == 0) {
                    currConn = e.Node.Index;
                    currDevice = 0;
                    cbConnectionList.SelectedIndex = e.Node.Index;
                    tbDeviceId.Text = Convert.ToString(1);
                    tbDeviceName.Text = "devName";
                    
                }
                
                if (e.Node.Level == 1)
                {
                    currDevice = e.Node.Index;
                    currConn = e.Node.Parent.Index;
                    int selectedNodeIndex = tvConnectionTree.SelectedNode.Index;
                    TreeNode ParentNode = tvConnectionTree.SelectedNode.Parent;
                    Device currDev = _conns.ElementAt(ParentNode.Index).Devices[selectedNodeIndex];
                    tbDeviceName.Text = currDev.deviceName;
                    tbDeviceId.Text = currDev.deviceAdress.ToString();
                    //MessageBox.Show("Отображаем карту регистров");
                    dgvTable.AutoGenerateColumns = false;//использовать до подключения источника данных к таблице
                    dgvTable.DataSource = currDev.Registers;
                    dgvTable.Columns["RegisterCol"].DataPropertyName = "Title";
                    dgvTable.Columns["DeviceCol"].DataPropertyName = "devName";
                    dgvTable.Columns["AdressCol"].DataPropertyName = "FullAdress";
                    dgvTable.Columns["FormatCol"].DataPropertyName = "dataType";
                    dgvTable.Columns["RepresentCol"].DataPropertyName = "Represent";
                    dgvTable.Columns["ByteOrderCol"].DataPropertyName = "byteOrder";
                    dgvTable.Columns["MulACol"].DataPropertyName = "A";
                    dgvTable.Columns["MulBCol"].DataPropertyName = "B";
                    
                    //TODO: Реализовать "сборку" значения из соседних регистров согласно его типу
                    dgvTable.Columns["ValCol"].DataPropertyName = "stringVal";
                    
                    
                }

            }            
        }

        private void добавитьУстройствоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            if (_conns.Count > 0) {   
                int selectedConn = (tvConnectionTree.SelectedNode.Level == 0)? tvConnectionTree.SelectedNode.Index:tvConnectionTree.SelectedNode.Parent.Index;
                Connection currConn = _conns[selectedConn];
                TreeNode currNode = tvConnectionTree.Nodes[selectedConn];
                DeviceEditForm devEditForm = new DeviceEditForm(currConn);
                DialogResult res = devEditForm.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    Device currDev = currConn.Devices.Last();
                    currNode.Nodes.Add(currDev.deviceAdress + ": " + currDev.deviceName);
                    if (!currNode.IsExpanded)
                        currNode.Expand();
                }
            }
            else
            {
                MessageBox.Show(NO_CONN_ADD_MSG, MSG_INFO_TITLE);
            }
        }

        private void добавитьЯчейкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addRegisterDialog();

        }

        private void addRegisterDialog()
        {
            if (_conns.Count < 0)
            {
                MessageBox.Show(NO_CONN_ADD_MSG, MSG_INFO_TITLE);
                return;
            }
            if (tvConnectionTree.SelectedNode == null)
            {
                MessageBox.Show(NO_DEV_ADD_MSG, MSG_INFO_TITLE);
                return;
            }
            int connIndex = (tvConnectionTree.SelectedNode.Level == 0) ? tvConnectionTree.SelectedNode.Index : tvConnectionTree.SelectedNode.Parent.Index;
            int devIndex = (tvConnectionTree.SelectedNode.Level == 1) ? tvConnectionTree.SelectedNode.Index : -1;
            if (devIndex < 0)
            {
                MessageBox.Show(NO_DEV_ADD_MSG, MSG_INFO_TITLE);
                return;
            }

            Connection currConn = _conns[connIndex];

            Device currDev = currConn.Devices[devIndex];
            MessageBox.Show(currDev.deviceName, MSG_INFO_TITLE);

            AddRegisterForm eRegForm = new AddRegisterForm(_conns, connIndex, devIndex);
            DialogResult res = eRegForm.ShowDialog(this);
        }

        private void btnTableClear_Click(object sender, EventArgs e)
        {
            ClearAllRegisters();
        }

        private void dgvTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //MessageBox.Show(e.RowIndex.ToString());
            currRegister = e.RowIndex;
            string devName = (string) dgvTable.Rows[e.RowIndex].Cells[1].Value;
            EditRegForm regForm = new EditRegForm();
            DialogResult res = regForm.ShowDialog(this);
            if (res == DialogResult.OK)
                dgvRowUpdate();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            addRegisterDialog();
        }

        private void tsmi_DelCurrRegister_Click(object sender, EventArgs e)
        {
            //DataGridViewSelectedCellCollection selCells = dgvTable.SelectedCells;
            DataGridViewSelectedRowCollection selRows = dgvTable.SelectedRows;
            //if (dgvTable.SelectedCells.Count > 0)
            //foreach (DataGridViewCell tmpCell in dgvTable.SelectedCells)
            //    _conns[currConn].Devices[currDevice].Registers.RemoveAt(tmpCell.RowIndex);
            if (dgvTable.SelectedRows.Count > 0)
                foreach (DataGridViewRow tmpRow in dgvTable.SelectedRows)
                    _conns[currConn].Devices[currDevice].Registers.RemoveAt(tmpRow.Index);
            if (dgvTable.SelectedCells.Count>0)
                currRegister = dgvTable.SelectedCells[0].RowIndex;
            else
            {
                currRegister = -1;
            }
            //MessageBox.Show(currRegister.ToString());
        }

        private void btnTableUpdate_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Update");
            dgvFullUpdate();
        }

        private void dgvTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            currRegister = e.RowIndex;
            //MessageBox.Show(currRegister.ToString());
        }

        private void tsmi_DelAllRegisters_Click(object sender, EventArgs e)
        {
            ClearAllRegisters();

        }

        private static void ClearAllRegisters()
        {
            if ((_conns.Count > 0)  && (_conns[currConn].Devices.Count>0) && (_conns[currConn].Devices[currDevice].Registers.Count > 0))
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить все регистры устройства?",
                                    "Удаление всех регистров устройства", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    _conns[currConn].Devices[currDevice].Registers.Clear();
                    currRegister = -1;
                }
            }
            else
            {
                MessageBox.Show(NO_REGS_MSG, MSG_INFO_TITLE);
            }
        }

        private void tsmi_RegisterProperty_Click(object sender, EventArgs e)
        {
            if (currRegister > 0)
            {
                EditRegForm regForm = new EditRegForm();
                DialogResult res = regForm.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    dgvRowUpdate();
                }
            } else
            {
                MessageBox.Show(NO_REGS_SELECT_MSG,MSG_INFO_TITLE);
            }

        }

        private void dgvRowUpdate()
        {
            try
            {
            for (int i = 0; i < dgvTable.ColumnCount; i++)
                dgvTable.UpdateCellValue(i, currRegister);
            }
            catch { }

        }

        private void dgvFullUpdate()
        {
            try
            {
                for (int i = 0; i < dgvTable.RowCount; i++)
                for (int j = 0; j < dgvTable.ColumnCount; j++)
                    dgvTable.UpdateCellValue(j, i);
        }
            catch { }
        }

        private void dgvTable_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTable.SelectedRows.Count > 0)
                currRegister = dgvTable.SelectedRows[0].Index;
            if (dgvTable.SelectedCells.Count > 0)
                currRegister = dgvTable.SelectedCells[0].RowIndex;
            //MessageBox.Show(currRegister.ToString());
        }

        private void tvConnectionTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (_conns.Count > 0)
            {
                if (e.Node.Level == 0)
                {
                    currConn = e.Node.Index;
                    currDevice = -1;
                    dgvTable.DataSource = null;
                }

                if (e.Node.Level == 1)
                {
                    currDevice = e.Node.Index;
                    currConn = e.Node.Parent.Index;
                }
            }
            else
            {
                currConn = -1;
                currDevice = -1;
                currRegister = -1;
            }
        }

        private void cbConnectionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            currConn = cbConnectionList.SelectedIndex;
        }

        private void tsmiByteOrder_CheckStateChanged(object sender, EventArgs e)
        {
            dgvTable.Columns["ByteOrderCol"].Visible = (tsmiByteOrder.Checked) ? true : false;
        }

        private void tsmiRepresent_CheckStateChanged(object sender, EventArgs e)
        {
            dgvTable.Columns["RepresentCol"].Visible = (tsmiRepresent.Checked);
        }

        private void tsBtnDec_Click(object sender, EventArgs e)
        {
            if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            {
                _conns[currConn].Devices[currDevice].Registers[currRegister].Represent = Register.Representation.DEC;
                dgvRowUpdate();
            }
        }
        
        private void tsBtnHex_Click(object sender, EventArgs e)
        {
            if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            {
                _conns[currConn].Devices[currDevice].Registers[currRegister].Represent = Register.Representation.HEX;
                dgvRowUpdate();
            }

        }

        private void tsBtnBin_Click(object sender, EventArgs e)
        {
            if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            {
                _conns[currConn].Devices[currDevice].Registers[currRegister].Represent = Register.Representation.BIN;
                dgvRowUpdate();
            }
        }

        private void tsBtnOct_Click(object sender, EventArgs e)
        {
            if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            {
                _conns[currConn].Devices[currDevice].Registers[currRegister].Represent = Register.Representation.OCT;
                dgvRowUpdate();
            }
        }

        private void tsBtnFloat_Click_1(object sender, EventArgs e)
        {
            if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            {
                _conns[currConn].Devices[currDevice].Registers[currRegister].Represent = Register.Representation.Float;
                dgvRowUpdate();
            }
        }

        private void tsBtnWord_Click(object sender, EventArgs e)
        {
            if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            {
                _conns[currConn].Devices[currDevice].Registers[currRegister].dataType = Register.DataType.Int16;
                dgvRowUpdate();
            }
        }

        private void tsBtnDWord_Click(object sender, EventArgs e)
        {
            if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            {
                _conns[currConn].Devices[currDevice].Registers[currRegister].dataType = Register.DataType.Int32;
                dgvRowUpdate();
            }
        }

        private void tsBtnIEEE754_Click(object sender, EventArgs e)
        {
            if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            {
                _conns[currConn].Devices[currDevice].Registers[currRegister].dataType = Register.DataType.Int64;
                dgvRowUpdate();
            }
        }

        private void tsBtnFloat_Click(object sender, EventArgs e)
        {
            if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            {
                _conns[currConn].Devices[currDevice].Registers[currRegister].dataType = Register.DataType.Float;
                dgvRowUpdate();
            }
        }

        private void tsBtnBE_Click(object sender, EventArgs e)
        {
            if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            {
                _conns[currConn].Devices[currDevice].Registers[currRegister].byteOrder = Register.ByteOrder.BIGENDIAN;
                dgvRowUpdate();
            }
        }

        private void tsBtnLE_Click(object sender, EventArgs e)
        {
            if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            {
                _conns[currConn].Devices[currDevice].Registers[currRegister].byteOrder = Register.ByteOrder.LITTLEENDIAN;
                dgvRowUpdate();
            }
        }

        private void tsBtnStartPoll_Click(object sender, EventArgs e)
        {
            if (_conns[currConn].status){
            PollTimer.Enabled = !PollTimer.Enabled;
            tsBtnStartPoll.Text = (PollTimer.Enabled) ? "Остановить опрос" : "Начать опрос";
            } else {
                MessageBox.Show(NO_ACTIVE_CONN_MSG, MSG_INFO_TITLE);
            }

        }

        private void PollTimer_Tick(object sender, EventArgs e)
        {

            if (_conns[currConn].status == true)
            {
                foreach (Device d in _conns[currConn].Devices)
                    if (d.Status)
                    {
                        //время предыдущего запроса
                        double queryTime = 0;
                        if (_conns[currConn].sw.IsRunning)
                        {
                            _conns[currConn].sw.Stop();
                            queryTime = _conns[currConn].sw.ElapsedMilliseconds;
                        }
                        else
                            queryTime = _conns[currConn].sw.ElapsedMilliseconds;

                        tslRequestTime.Text = queryTime.ToString();

                        //количество запросов
                        tslRequestTotalNum.Text = _conns[currConn].QueriesNumSND.ToString();
                        tslAnswerNum.Text = _conns[currConn].QueriesNumRCV.ToString();
                        
                        //новый запрос
                        _conns[currConn].Poll(_conns[currConn].Devices.IndexOf(d));
                       
                        //обновление колонки значений
                        try
                        {
                            for (int i = 0; i < dgvTable.RowCount; i++)
                                    dgvTable.UpdateCellValue((dgvTable.ColumnCount-1), i);
                        }
                        catch { }
                    }
            }
        }

        private void tsTbPollInterval_KeyPress(object sender, KeyPressEventArgs e)
        {
            //Console.Out.WriteLine(Convert.ToInt32(e.KeyChar));
            if (Convert.ToInt32(e.KeyChar) == 13)
                if (int.Parse(tsTbPollInterval.Text) > 100)
                PollTimer.Interval = int.Parse(tsTbPollInterval.Text);
        }

        private void tsTbPollInterval_Leave(object sender, EventArgs e)
        {
            if (int.Parse(tsTbPollInterval.Text) > 100)
                PollTimer.Interval = int.Parse(tsTbPollInterval.Text);
        }

        private void tsMiHelp_Click(object sender, EventArgs e)
        {
            Help.ShowHelp(this, "Help.chm");
        }
    }
}