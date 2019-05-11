﻿using System;
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
using System.Xml.Serialization;
using System.IO;

namespace FreeModScan
{
    public partial class MainForm : Form
    {
        TextWriter _writer = null;// Это специальный класс TextWriter для вывода сообщений консоли в Textbox

        private const string NO_CONN_ADD_MSG = "Для добавления устройства создайте новое подключение";
        private const string EMPTY_CONN_LIST_MSG = "Список подключений пуст";
        private const string NO_DEV_ADD_MSG = "Для добавления регистров создайте новое устройство или выберите существующее";
        private const string NO_REGS_MSG = "Регистры устройства не определены";
        private const string NO_REGS_SELECT_MSG = "Регистр не выбран";
        private const string NO_ACTIVE_CONN_MSG = "Нет активных подключений";
        private const string MSG_INFO_TITLE = "Информация";

        public static BindingList<Connection> _conns = new BindingList<Connection>();
        //public static int currConn = -1;//текущее соединение
        //public static int currDevice = -1;//текущее устройство
        //public static int currRegister = -1;//текущий регистр
        public static Connection currConn = null;//текущее соединение
        public static Device currDevice = null;//текущее устройство
        public static Register currRegister = null;//текущий регистр

        //public static BindingList<string> console = new BindingList<string>();

        public static CRC16 CRC = new CRC16();

        string filePath = string.Empty;//путь к текущему файлу регистров

        public MainForm()
        {
            InitializeComponent();
            _conns.ListChanged += new ListChangedEventHandler(Connections_ListChanged);
            //console.ListChanged += new ListChangedEventHandler(ConsoleMsg_Changes);
            cbConnectionList.DataSource = _conns;
            cbConnectionList.DisplayMember = "PortName";
            cbConnectionList.ValueMember = "ConnName";

            cbDataType.DataSource = Enum.GetNames(typeof(Register.DataType));
            cbDataType.SelectedIndex = (int)Register.DataType.Int16;
            tslRequestTotalNum.Text = tslAnswerNum.Text = tsLblLogRecordsNum.Text = tslRequestTime.Text = "0";

            Register.Create += new Register.RegisterEventHandler(registerCreated);
            Register.Delete += new Register.RegisterEventHandler(registerDeleted);
            Device.Create += new Device.DeviceEventHandler(deviceAdded);
            Device.Delete += new Device.DeviceEventHandler(deviceDeleted);
            Connection.StateChanged += new Connection.ConnectionEventHandler(ConnStateChanged);

            // Инстанциируем writer
            _writer = new ConsoleStreamWriter(rtbConsole);
            // Перенаправляем выходной поток консоли
            Console.SetOut(_writer);
        }

        private void deviceDeleted(Device d)
        {
            Console.Write("Class MainForm: Device Deleted");
        }

        private void registerCreated(Register r)
        {
            Console.Write("Class MainForm: Register Created");
        }

        private void registerDeleted(Register r)
        {
            Console.Write("Class MainForm: Register Deleted");
        }

        private void Connections_ListChanged(object sender, ListChangedEventArgs e)
        {
            Connection conn = null;
            int connsNum = _conns.Count();//поличество подключений после изменения

            if (e.NewIndex != connsNum)
                conn = _conns[e.NewIndex];//объект с которым производились манипуляции

            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    //Add item here.
                    //currConn = e.NewIndex;
                    currConn = _conns[e.NewIndex];

                    if (tvConnectionTree.Nodes[0].Text == EMPTY_CONN_LIST_MSG)//удаляем текст, отображаемый при отсутствии соединений
                        tvConnectionTree.Nodes.Clear();
                    //tvConnectionTree.Nodes.Add(_conns[currConn].ConnName + " (" + _conns[currConn].Port.PortName + "): " + _conns[currConn].statusString);
                    //console.Add("** " + DateTime.Now + " - " + _conns[currConn].ConnName + " (" + _conns[currConn].Port.PortName + "): Соединение создано**\n");
                    tvConnectionTree.Nodes.Add(currConn.ConnName + " (" + currConn.Port.PortName + "): " +currConn.statusString);
                    Console.Write(currConn.ConnName + " (" + currConn.Port.PortName + "): Соединение создано");

                    //подписываемся на события
                    //conn.StateChanged += new Connection.ConnectionEventHandler(ConnStateChanged);
                    //conn.Delete += new Connection.ConnectionEventHandler(ConnDelete);
                    conn.Error += new Connection.ConnectionErrorHandler(ConnError);

                    //запуск события Создания для других подписчиков
                    conn.OnCreate();
                    break;
                case ListChangedType.PropertyDescriptorChanged:
                    MessageBox.Show("1561");
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
                    if (connsNum == 0)
                    {
                        setConnTreeDefaults();
                        //currConn = -1;
                        currConn = null;
                    }
                    else
                    {
                        tvConnectionTree.Nodes[e.NewIndex].Remove();
                        //if (e.NewIndex == currConn)//если удаляется текущее соединение, то делаем текущим предыдущее
                        //    if (currConn >= connsNum)
                        //        currConn--;
                        if (e.NewIndex == MainForm._conns.IndexOf(currConn))//если удаляется текущее соединение, то делаем текущим предыдущее
                            if (MainForm._conns.IndexOf(currConn) >= connsNum)
                                currConn = MainForm._conns.Last();
                    }
                    break;

                case ListChangedType.Reset:
                    //This reset all data and control need to refill all data.
                    //MessageBox.Show("Reset");
                    setConnTreeDefaults();
                    break;
            }
        }


        //private void ConsoleMsg_Changes(object sender, ListChangedEventArgs e)
        //{
        //    switch (e.ListChangedType)
        //    {
        //        case ListChangedType.ItemAdded:
        //            rtbConsole.Text = console.Last() + rtbConsole.Text;
        //            //rtbConsole.ScrollToCaret;
        //            break;
        //    }
        //}

        private void tsmiExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnConsoleClear_Click(object sender, EventArgs e)
        {
            //console.Clear();
            rtbConsole.Clear();
        }

        private void tsmiLogShow_Click(object sender, EventArgs e)
        {
            openLogDialog.ShowDialog(this);
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
            ConnectionForm connForm = new ConnectionForm(ref _conns);
            if (connForm.ShowDialog(this) == DialogResult.OK)
            {
                //_conns.Last().OnStateChanged += new Connection.ConnectionEventHandler(ConnStateChanged);
                //_conns.Last().OnError += new Connection.ConnectionErrorHandler(ConnError);
                tvConnectionTree.SelectedNode = tvConnectionTree.Nodes[_conns.IndexOf(currConn)];
                tvConnectionTree.Focus();
            }


        }

        private void dataReady()
        {
            //rtbConsole.Text = _conns[currConn] + rtbConsole.Text;
        }

        public void ConnError(Exception e)
        {
            //TreeNode currNode = tvConnectionTree.Nodes[currConn];
            //Connection conn = _conns[currConn];
            TreeNode currNode = tvConnectionTree.Nodes[_conns.IndexOf(currConn)];
            Connection conn = currConn;
            //currNode.ForeColor = Color.DarkRed;
            rtbConsole.Text = "** " + DateTime.Now.ToString() + " - " + conn.ConnName + "-" + conn.Port.PortName + " : " + e.Message + " **" + "\n" + rtbConsole.Text;
            //console.Add("** " + DateTime.Now.ToString() + " - " + conn.ConnName + "-" + conn.Port.PortName + " : " + e.Message + " **" + "\n");
            //MessageBox.Show(conn.ConnName + " - Ошибка. " + e.Message);
        }

        public void ConnStateChanged(Connection c)
        {
            //TreeNode currNode = tvConnectionTree.Nodes[currConn];
            TreeNode currNode = tvConnectionTree.Nodes[_conns.IndexOf(currConn)];
            currNode.Text = c.ConnName + " - " + c.Port.PortName + " - " + c.statusString;
            //rtbConsole.Text = "** " + DateTime.Now.ToString() + " - " + conn.ConnName + " : Соединение по " + conn.Port.PortName + " " + conn.statusString + ". **" + "\n" + rtbConsole.Text;
            //console.Add("** " + DateTime.Now.ToString() + " - " + c.ConnName + " : Соединение по " + c.Port.PortName + " " + c.statusString + ". **" + "\n");
            Console.Write(c.ConnName + " ("+c.Port.PortName+"): Соединение " + c.statusString);
            //currNode.ForeColor = (conn.status)? Color.Black : Color.DarkGray;
            //MessageBox.Show(conn.ConnName + " - " + conn.statusString);
            //_conns.Last().OnDataReady += new Connection.PollEventHandler(dataReady);
            var tmpColl = _conns.Where(tmpConn => c.status == true);
            if (tmpColl.Count() <= 0)
            {
                PollTimer.Enabled = false;
                tsBtnStartPoll.Text = "Начать опрос";
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            foreach (Connection tmp in _conns)
                if ((tmp != null) && (!tmp.status))
                    tmp.Open();
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            foreach (Connection tmp in _conns)
            {
                if ((tmp != null) && (tmp.status))
                    tmp.Close();
            }
        }

        private void tsMiMul_CheckStateChanged(object sender, EventArgs e)
        {
            dgvTable.Columns["MulACol"].Visible =
            dgvTable.Columns["MulBCol"].Visible =
            dgvTable.Columns["k"].Visible = tsMiMul.Checked;

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

            if (_conns.Count <= 0)
            {
                MessageBox.Show(NO_CONN_ADD_MSG, MSG_INFO_TITLE);
                return;
            }

            Connection currCon = _conns[cbConnectionList.SelectedIndex];

            byte devAdress = byte.Parse(tbDeviceId.Text);
            Device dev = new Device(devAdress, tbDeviceName.Text);
            //dev.Create += new Device.DeviceEventHandler(deviceAdded);//подписка на событие Создания устройства

            int devId = currCon.Devices.IndexOf(dev);
            MessageBox.Show("Device ID=" + devId.ToString());

            if (devId >= 0)//устройство существует, не добавляем нового
            {
                MessageBox.Show("Device Exists" + devId.ToString());
                dev = currCon.Devices[devId];
            }
            else
                currCon.Devices.Add(dev);

            //добавляем регистры
            dev.addRegisters(tbRegisterList.Text, cbRegisterType.SelectedIndex + 1, cbDataType.SelectedIndex);
            //addRegisters(dev, tbRegisterList.Text, cbRegisterType.SelectedIndex + 1, cbDataType.SelectedIndex);
            //dgvTable.Rows[0].Cells[0].Selected = true;

        }

        //public void addRegisters(Device dev, string regString, int rType, int dType, int bOrder = 1)
        //{
        //    MatchCollection matches = Regex.Matches(regString, "((?<s>\\d+)-(?<e>\\d+))|(?<r>\\d+)");

        //    foreach (Match match in matches)
        //    {
        //        Register tmpReg = null;
        //        int startIndex = 0;
        //        int endIndex = -1;

        //        if (match.Groups["r"].Value != "")
        //        {
        //            //MessageBox.Show(uint.Parse(match.Groups["r"].Value).ToString());
        //            startIndex = endIndex = int.Parse(match.Groups["r"].Value);
        //        }

        //        if ((match.Groups["s"].Value != "") && (match.Groups["e"].Value != ""))
        //        {
        //            startIndex = int.Parse(match.Groups["s"].Value);
        //            endIndex = int.Parse(match.Groups["e"].Value);
        //        }

        //        for (int i = startIndex; i <= endIndex; i++)
        //        {
        //            tmpReg = new Register(dev.deviceName, (uint)i, (Register.RegType)rType, (Register.DataType)dType, (Register.ByteOrder)bOrder);
        //            tmpReg.Create += new Register.RegisterEventHandler(RegisterAdded);//подписываемся на событие Создания
        //            //MessageBox.Show(i.ToString());
        //            dev.Registers.Add(tmpReg);
        //        }
        //    }
        //}

        //public void RegisterAdded(Register r)
        //{
        //    rtbConsole.Text = "Register was Added" + rtbConsole.Text;
        //    MessageBox.Show("Register was Added");
        //}

        private void удалитьВсеПодключенияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_conns.Count > 0)
            {
                DialogResult res = MessageBox.Show("Удаление всех подключений повлечёт удаление всех регистров и устройств. Продолжить?",
                                 "Удаление всех устройств", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    PollTimer.Enabled = false;
                    //tsBtnStartPoll.Text = (PollTimer.Enabled) ? "Остановить опрос" : "Начать опрос";
                    //currConn = currDevice = currRegister = -1;
                    currConn = null;
                    currDevice = null;
                    currRegister = null;
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
                if (tvConnectionTree.SelectedNode.Level == 0)
                    currConn = tvConnectionTree.SelectedNode.Index;

                if (tvConnectionTree.SelectedNode.Level == 1)
                    currConn = tvConnectionTree.SelectedNode.Parent.Index;

                _conns[currConn].Open();
                //tvConnectionStateUpdate(_conns[currConn]);
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
                //tvConnectionStateUpdate(_conns[currConn]);
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
                    //MessageBox.Show("Удаляем устройство #" + selectedNodeIndex + " в соединении " + ParentNode.Text);
                    _conns.ElementAt(ParentNode.Index).Devices.RemoveAt(selectedNodeIndex);
                    currDevice.OnDelete();
                    ParentNode.Nodes[selectedNodeIndex].Remove();
                    currDevice = null;
                    currRegister = null;
                }
                else
                {
                    //if (selectedNodeIndex == currConn)
                    if (selectedNodeIndex == _conns.IndexOf(currConn))
                    {
                        PollTimer.Enabled = false;
                        tsBtnStartPoll.Text = (PollTimer.Enabled) ? "Остановить опрос" : "Начать опрос";
                    }
                    //_conns[currConn].OnDelete();//запуск события Удаления соединения для прочих подписчиков
                    currConn.OnDelete();//запуск события Удаления соединения для прочих подписчиков
                    Console.Write( currConn.ConnName + " (" + currConn.Port.PortName + "): Соединение удалено");
                    _conns.RemoveAt(selectedNodeIndex);

                    cbConnectionList.Text = tvConnectionTree.SelectedNode.Text;

                    currConn = null;
                    currDevice = null;
                    currRegister = null;
                }

            }
            else
            {
                Console.Write("**Список подключений пуст**\n");
            }
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tvConnectionTree.Refresh();
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_conns.Count > 0)
            {
                int selectedNodeIndex = tvConnectionTree.SelectedNode.Index;
                if (tvConnectionTree.SelectedNode.Level == 0)
                {
                    Connection currConn = _conns[selectedNodeIndex];
                    ConnectionForm connForm = new ConnectionForm(ref currConn);
                    DialogResult res = connForm.ShowDialog(this);
                    if (res == DialogResult.OK)
                    {
                        MessageBox.Show("Обновляем параметры соединения");
                        tvConnectionTree.SelectedNode.Text = currConn.ConnName + " - " + currConn.Port.PortName + " - " + currConn.statusString;
                    }
                }
                else
                {
                    TreeNode ParentNode = tvConnectionTree.SelectedNode.Parent;
                    Device currDev = _conns.ElementAt(ParentNode.Index).Devices[selectedNodeIndex];
                    DeviceEditForm devEditForm = new DeviceEditForm(ref currDev);
                    DialogResult res = devEditForm.ShowDialog(this);
                    if (res == DialogResult.OK)
                    {
                        MessageBox.Show("Обновляем параметры устройства");
                        tvConnectionTree.SelectedNode.Text = currDev.deviceAdress + ": " + currDev.deviceName;
                    }
                }

            }
        }

        private void tvConnectionTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (_conns.Count > 0)
            {

                if (e.Node.Level == 0)
                {
                    //currConn = e.Node.Index;
                    //currDevice = -1;
                    currConn = _conns[e.Node.Index];
                    currDevice = null;
                    cbConnectionList.SelectedIndex = e.Node.Index;
                    tbDeviceId.Text = Convert.ToString(1);
                    tbDeviceName.Text = "devName";

                }

                if (e.Node.Level == 1)
                {
                    //currDevice = e.Node.Index;
                    //currConn = e.Node.Parent.Index;
                    currConn = _conns[e.Node.Parent.Index];
                    currDevice = currConn.Devices[e.Node.Index];
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
                    dgvTable.Columns["k"].DataPropertyName = "UseMults";
                    dgvTable.Columns["MulACol"].DataPropertyName = "strA";
                    dgvTable.Columns["MulBCol"].DataPropertyName = "strB";
                    dgvTable.Columns["ValCol"].DataPropertyName = "stringVal";
                }

            }
        }

        private void tsmiAddDevice_Click(object sender, EventArgs e)
        {

            if (_conns.Count <= 0)
            {
                MessageBox.Show(NO_CONN_ADD_MSG, MSG_INFO_TITLE);
                return;
            }

            int selectedConn = (tvConnectionTree.SelectedNode.Level == 0) ? tvConnectionTree.SelectedNode.Index : tvConnectionTree.SelectedNode.Parent.Index;
            Connection currConn = _conns[selectedConn];
            TreeNode currNode = tvConnectionTree.Nodes[selectedConn];
            DeviceEditForm devEditForm = new DeviceEditForm(currConn);
            DialogResult res = devEditForm.ShowDialog(this);
            //if (res == DialogResult.OK)
            //{
            //    Device currDev = currConn.Devices.Last();
            //}
        }

        internal void deviceAdded(Device d)
        {
            MessageBox.Show("Bingo!");

            TreeNode currNode = tvConnectionTree.Nodes[cbConnectionList.SelectedIndex];
            currNode.Nodes.Add(d.deviceAdress + ": " + d.deviceName);
            if (!currNode.IsExpanded)
                currNode.Expand();
            tvConnectionTree.SelectedNode = currNode.LastNode;
            tvConnectionTree.Focus();
            //currDevice = currNode.LastNode.Index;
            //currRegister = 0;
            //console.Add("** " + DateTime.Now.ToString() + " - " + _conns[currConn].ConnName + " (" + _conns[currConn].Port.PortName + "): Добавлено устройство " + d.deviceName + " **" + "\n");
            currDevice = currConn.Devices[currNode.LastNode.Index];
            currRegister = (currDevice.Registers.Count()>0)?currDevice.Registers.First():null;
            Console.Write(currConn.ConnName + " (" + currConn.Port.PortName + "): Устройство " + d.deviceName + " добавлено");
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
            //currRegister = e.RowIndex;
            currRegister = currDevice.Registers[e.RowIndex];
            string devName = (string)dgvTable.Rows[e.RowIndex].Cells[1].Value;
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
            DataGridViewSelectedRowCollection selRows = dgvTable.SelectedRows;
            if (selRows.Count == 0)
            {
                 dgvTable.CurrentRow.Selected = true;
                selRows = dgvTable.SelectedRows;
            }

            foreach (DataGridViewRow tmpRow in selRows)
            {
                Register tmpR = (Register)tmpRow.DataBoundItem;
                tmpR.OnDelete();
                currDevice.Registers.Remove(tmpR);//или вариант ниже
                //dgvTable.Rows.RemoveAt(tmpRow.Index);//или вариант ниже
                //_conns[currConn].Devices[currDevice].Registers.RemoveAt(tmpRow.Index);
                dgvTable.Update();//необходимо для исключения возникновения ошибки при удалении строк, видимых на экране
            }

            if (dgvTable.SelectedCells.Count > 0)
                //currRegister = dgvTable.SelectedCells[0].RowIndex;
                currRegister = currDevice.Registers[dgvTable.SelectedCells[0].RowIndex];
            else
            {
                //currRegister = -1;
                currRegister = null;
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
            //currRegister = e.RowIndex;
            currRegister = currDevice.Registers[e.RowIndex];
            //MessageBox.Show(currRegister.ToString());

        }

        private void tsmi_DelAllRegisters_Click(object sender, EventArgs e)
        {
            ClearAllRegisters();

        }

        private void ClearAllRegisters()
        {
            if (currDevice != null)
            {
                DialogResult res = MessageBox.Show("Вы действительно хотите удалить все регистры устройства?",
                                   "Удаление всех регистров устройства", MessageBoxButtons.YesNo);
                if (res == DialogResult.Yes)
                {
                    currRegister = null;
                    dgvTable.DataSource=null;
                    currDevice.OnDeleteAll();
                }

            } else
            {
                MessageBox.Show("Выберите устройство", MSG_INFO_TITLE);
            }
            //if ((_conns.Count > 0) && (_conns[currConn].Devices.Count > 0) && (_conns[currConn].Devices[currDevice].Registers.Count > 0))
            //if (currDevice.Registers.Count > 0)
            //{
            //DialogResult res = MessageBox.Show("Вы действительно хотите удалить все регистры устройства?",
            //                    "Удаление всех регистров устройства", MessageBoxButtons.YesNo);
            //if (res == DialogResult.Yes)
            //{
            //    //_conns[currConn].Devices[currDevice].Registers.Clear();
            //    //currRegister = -1;
            //    //_conns[currConn].ClearRequests();
            //    currDevice.Registers.Clear();
            //    dgvTable.Update();
            //    currRegister = null;
            //    currConn.ClearRequests();
            //}
            //}
            //else
            //{
            //    MessageBox.Show(NO_REGS_MSG, MSG_INFO_TITLE);
            //}
        }

        private void tsmi_RegisterProperty_Click(object sender, EventArgs e)
        {
            //if (currRegister > 0)
            if (currRegister !=null)
            {
                EditRegForm regForm = new EditRegForm();
                DialogResult res = regForm.ShowDialog(this);
                if (res == DialogResult.OK)
                {
                    
                    dgvRowUpdate();
                }
            }
            else
            {
                MessageBox.Show(NO_REGS_SELECT_MSG, MSG_INFO_TITLE);
            }

        }

        private void dgvRowUpdate()
        {
            try
            {
                for (int i = 0; i < dgvTable.ColumnCount; i++)
                    //dgvTable.UpdateCellValue(i, currRegister);
                    dgvTable.UpdateCellValue(i, currDevice.Registers.IndexOf(currRegister));
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
                //currRegister = dgvTable.SelectedRows[0].Index;
                currRegister = currDevice.Registers[dgvTable.SelectedRows[0].Index];
            if (dgvTable.SelectedCells.Count > 0)
                //currRegister = dgvTable.SelectedCells[0].RowIndex;
                currRegister = currDevice.Registers[dgvTable.SelectedCells[0].RowIndex];
            //MessageBox.Show(currRegister.ToString());
        }

        private void tvConnectionTree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (_conns.Count > 0)
            {
                if (e.Node.Level == 0)
                {
                    //currConn = e.Node.Index;
                    //currDevice = -1;
                    currConn = _conns[e.Node.Index];
                    currDevice = null;
                    dgvTable.DataSource = null;
                }

                if (e.Node.Level == 1)
                {
                    //currDevice = e.Node.Index;
                    //currConn = e.Node.Parent.Index;
                    currConn = _conns[e.Node.Parent.Index];
                    currDevice = currConn.Devices[e.Node.Index];
                    dgvTable.DataSource = currDevice.Registers;
                }
            }
            else
            {
                //currConn = -1;
                //currDevice = -1;
                //currRegister = -1;
                currConn = null;
                currDevice = null;
                currRegister = null;
            }
        }

        private void cbConnectionList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //currConn = cbConnectionList.SelectedIndex;
            currConn = _conns[cbConnectionList.SelectedIndex];
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
            //if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            if (currRegister != null)
            {
                //_conns[currConn].Devices[currDevice].Registers[currRegister].Represent = Register.Representation.DEC;
                currRegister.Represent = Register.Representation.DEC;
                dgvRowUpdate();
            }
        }

        private void tsBtnHex_Click(object sender, EventArgs e)
        {
            //if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            if (currRegister != null)
            {
                //_conns[currConn].Devices[currDevice].Registers[currRegister].Represent = Register.Representation.HEX;
                currRegister.Represent = Register.Representation.HEX;
                dgvRowUpdate();
            }

        }

        private void tsBtnBin_Click(object sender, EventArgs e)
        {
            //if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            //if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            if (currRegister != null)
            {
                //_conns[currConn].Devices[currDevice].Registers[currRegister].Represent = Register.Representation.BIN;
                currRegister.Represent = Register.Representation.BIN;
                dgvRowUpdate();
            }
        }

        private void tsBtnOct_Click(object sender, EventArgs e)
        {
            //if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            if (currRegister != null)
            {
                //_conns[currConn].Devices[currDevice].Registers[currRegister].Represent = Register.Representation.OCT;
                currRegister.Represent = Register.Representation.OCT;
                dgvRowUpdate();
            }
        }

        private void tsBtnFloat_Click_1(object sender, EventArgs e)
        {
            //if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            if (currRegister !=null)
            {
                //_conns[currConn].Devices[currDevice].Registers[currRegister].Represent = Register.Representation.Float;
                dgvRowUpdate();
            }
        }

        private void tsBtnInt16_Click(object sender, EventArgs e)
        {
            //if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            if (currRegister != null)
            {
                //_conns[currConn].Devices[currDevice].Registers[currRegister].dataType = Register.DataType.Int16;
                //_conns[currConn].Devices[currDevice].Registers[currRegister].ValArr = new byte[2];
                currRegister.dataType = Register.DataType.Int16;
                currRegister.ValArr = new byte[2];
                dgvRowUpdate();
            }
        }

        private void tsBtnInt32_Click(object sender, EventArgs e)
        {
            //if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            if (currRegister != null)
            {
                //_conns[currConn].Devices[currDevice].Registers[currRegister].dataType = Register.DataType.Int32;
                //_conns[currConn].Devices[currDevice].Registers[currRegister].ValArr = new byte[4];
                currRegister.dataType = Register.DataType.Int32;
                currRegister.ValArr = new byte[4];
                dgvRowUpdate();
            }
        }

        private void tsBtnInt64_Click(object sender, EventArgs e)
        {
            //if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            if (currRegister !=null)
            {
                //_conns[currConn].Devices[currDevice].Registers[currRegister].dataType = Register.DataType.Int64;
                //_conns[currConn].Devices[currDevice].Registers[currRegister].ValArr = new byte[8];
                currRegister.dataType = Register.DataType.Int64;
                currRegister.ValArr = new byte[8];
                dgvRowUpdate();
            }
        }

        private void tsBtnFloat_Click(object sender, EventArgs e)
        {
            //if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            if (currRegister !=null)
            {
                //_conns[currConn].Devices[currDevice].Registers[currRegister].dataType = Register.DataType.Float;
                //_conns[currConn].Devices[currDevice].Registers[currRegister].ValArr = new byte[8];
                currRegister.dataType = Register.DataType.Float;
                currRegister.ValArr = new byte[8];
                dgvRowUpdate();
            }
        }

        private void tsBtnBE_Click(object sender, EventArgs e)
        {
            //if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            if (currRegister != null)
            {
                //_conns[currConn].Devices[currDevice].Registers[currRegister].byteOrder = Register.ByteOrder.BIGENDIAN;
                currRegister.byteOrder = Register.ByteOrder.BIGENDIAN;
                dgvRowUpdate();
            }
        }

        private void tsBtnLE_Click(object sender, EventArgs e)
        {
            //if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            if (currRegister != null)
            {
                //_conns[currConn].Devices[currDevice].Registers[currRegister].byteOrder = Register.ByteOrder.LITTLEENDIAN;
                currRegister.byteOrder = Register.ByteOrder.LITTLEENDIAN;
                dgvRowUpdate();
            }
        }
        private void tsBtnMidLE_Click(object sender, EventArgs e)
        {
            //if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            if (currRegister !=null)
            {
                //_conns[currConn].Devices[currDevice].Registers[currRegister].byteOrder = Register.ByteOrder.MIDLITTLEENDIAN;
                currRegister.byteOrder = Register.ByteOrder.MIDLITTLEENDIAN;
                dgvRowUpdate();
            }
        }
        private void tsBtnMidBE_Click(object sender, EventArgs e)
        {
            //if ((currConn >= 0) && (currDevice >= 0) && (currRegister >= 0))
            if (currRegister != null)
            {
                //_conns[currConn].Devices[currDevice].Registers[currRegister].byteOrder = Register.ByteOrder.MIDBIGENDIAN;
                currRegister.byteOrder = Register.ByteOrder.MIDBIGENDIAN;
                dgvRowUpdate();
            }
        }
        private void tsBtnStartPoll_Click(object sender, EventArgs e)
        {
            //if ((_conns.Count > 0) && (_conns[currConn].status))
            if ((_conns.Count > 0) && (currConn.status))
            {
                //_conns[currConn].UpdateRequests();
                currConn.UpdateRequests();
                PollTimer.Interval = int.Parse(tsTbPollInterval.Text);
                PollTimer.Enabled = !PollTimer.Enabled;
                tsBtnStartPoll.Text = (PollTimer.Enabled) ? "Остановить опрос" : "Начать опрос";

            }
            else
            {
                //if (_conns[currConn].status)
                if (currConn.status)
                    tsBtnStartPoll.Text = "Начать опрос";
                else
                    MessageBox.Show(NO_ACTIVE_CONN_MSG, MSG_INFO_TITLE);
            }

        }

        private void PollTimer_Tick(object sender, EventArgs e)
        {

            //if (_conns[currConn].status == true)
            if (currConn.status == true)
            {
                //foreach (Device d in _conns[currConn].Devices)
                foreach (Device d in currConn.Devices)
                    if (d.Status)
                    {
                        //время предыдущего запроса
                        double queryTime = 0;
                        //if (_conns[currConn].sw.IsRunning)
                        if (currConn.sw.IsRunning)
                        {
                            //_conns[currConn].sw.Stop();
                            //queryTime = _conns[currConn].sw.ElapsedMilliseconds;
                            currConn.sw.Stop();
                            queryTime = currConn.sw.ElapsedMilliseconds;
                        }
                        else
                            //queryTime = _conns[currConn].sw.ElapsedMilliseconds;
                            queryTime = currConn.sw.ElapsedMilliseconds;

                        tslRequestTime.Text = queryTime.ToString();

                        //количество запросов
                        //tslRequestTotalNum.Text = _conns[currConn].QueriesNumSND.ToString();
                        //tslAnswerNum.Text = _conns[currConn].QueriesNumRCV.ToString();
                        tslRequestTotalNum.Text = currConn.QueriesNumSND.ToString();
                        tslAnswerNum.Text = currConn.QueriesNumRCV.ToString();

                        //новый запрос
                        //_conns[currConn].Poll(_conns[currConn].Devices.IndexOf(d));
                        //_conns[currConn].Poll();
                        currConn.Poll();

                        //обновление колонки значений
                        try
                        {
                            for (int i = 0; i < dgvTable.RowCount; i++)
                                dgvTable.UpdateCellValue((dgvTable.ColumnCount - 1), i);
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

        private void dgvTable_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if ((dgvTable.Columns["k"] != null) && (e.ColumnIndex == dgvTable.Columns["k"].Index))
            {
                dgvTable.UpdateCellValue(dgvTable.Columns["MulACol"].Index, e.RowIndex);
                dgvTable.UpdateCellValue(dgvTable.Columns["MulBCol"].Index, e.RowIndex);

            }
        }

        private void dgvTable_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvTable_CellValidated(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvTable_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void tsmiMapSave_Click(object sender, EventArgs e)
        {
            string path = string.Empty;
            if (filePath != string.Empty)
            {
                path = filePath;
            }
            else
            {
                if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
                    path = filePath = saveFileDialog.FileName;
                else
                    return;
            }
            XmlSerializer ser = new XmlSerializer(typeof(BindingList<Device>));
            using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
            //{ ser.Serialize(file, _conns[currConn].Devices); }
            { ser.Serialize(file, currConn.Devices); }

        }

        private void tsmiMapSaveAs_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                //if (currConn < 0)
                if (currConn == null)
                {
                    MessageBox.Show(EMPTY_CONN_LIST_MSG, MSG_INFO_TITLE);
                    return;
                }
                string path = filePath = saveFileDialog.FileName;
                XmlSerializer ser = new XmlSerializer(typeof(BindingList<Device>));
                using (FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None))
                //{ ser.Serialize(file, _conns[currConn].Devices); }
                { ser.Serialize(file, currConn.Devices); }
            }
        }

        private void tsmiMapOpen_Click(object sender, EventArgs e)
        {
            if (openMapDialog.ShowDialog(this) == DialogResult.OK)
            {
                //if (currConn < 0)
                if (currConn == null)
                {
                    MessageBox.Show(EMPTY_CONN_LIST_MSG, MSG_INFO_TITLE);
                    return;
                }
                tvConnectionTree.BeginUpdate();
                BindingList<Device> tmpList;
                XmlSerializer ser = new XmlSerializer(typeof(BindingList<Device>));
                string path = openMapDialog.FileName;
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    tmpList = (BindingList<Device>)ser.Deserialize(file);
                }
                //TreeNode currNode = tvConnectionTree.Nodes[currConn];
                TreeNode currNode = tvConnectionTree.Nodes[_conns.IndexOf(currConn)];

                foreach (Device device in tmpList)
                {
                    //_conns[currConn].Devices.Add(device);
                    currConn.Devices.Add(device);
                    TreeNode node = new TreeNode(device.deviceAdress + ":" + device.deviceName);
                    currNode.Nodes.Add(node);

                }
                tvConnectionTree.ExpandAll();
                tvConnectionTree.EndUpdate();
            }
        }

        private void tsmiNewMap_Click(object sender, EventArgs e)
        {
            PollTimer.Enabled = false;
            dgvTable.DataSource = null;
            _conns.Clear();
            //currConn = -1;//текущее соединение
            //currDevice = -1;//текущее устройство
            //currRegister = -1;//текущий регистр            
            currConn = null;//текущее соединение
            currDevice = null;//текущее устройство
            currRegister = null;//текущий регистр
        }

        private void cMSConnTree_Opening(object sender, CancelEventArgs e)
        {
            if (_conns.Count() > 0)
            {
                cMSConnTree.Items["tsmiAddDevice"].Enabled =
                cMSConnTree.Items["tsmiEdit"].Enabled =
                cMSConnTree.Items["tsmiDel"].Enabled =
                cMSConnTree.Items["tsmiDelAll"].Enabled = true;

                //cMSConnTree.Items["tsmiAddRegisters"].Enabled = (currDevice >= 0);
                cMSConnTree.Items["tsmiAddRegisters"].Enabled = (currDevice != null);

                TreeNode selectedConn = (tvConnectionTree.SelectedNode.Level == 0) ?
                tvConnectionTree.SelectedNode : tvConnectionTree.SelectedNode.Parent;
                    if (_conns[selectedConn.Index].status)
                {
                    cMSConnTree.Items["tsmiDisconnect"].Enabled = true;
                    cMSConnTree.Items["tsmiConnect"].Enabled = false;
                }
                else
                {
                    cMSConnTree.Items["tsmiDisconnect"].Enabled = false;
                    cMSConnTree.Items["tsmiConnect"].Enabled = true;
                }
            }
            else
            {
                cMSConnTree.Items["tsmiAddDevice"].Enabled =
                cMSConnTree.Items["tsmiEdit"].Enabled =
                cMSConnTree.Items["tsmiDel"].Enabled =
                cMSConnTree.Items["tsmiConnect"].Enabled =
                cMSConnTree.Items["tsmiDisconnect"].Enabled =
                cMSConnTree.Items["tsmiAddRegisters"].Enabled =
                cMSConnTree.Items["tsmiDelAll"].Enabled = false;
            }
        }


    }
}