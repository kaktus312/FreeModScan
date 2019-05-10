using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FreeModScan
{
    public partial class ConnectionForm : Form
    {
        Connection conn;
        BindingList<Connection> refConns;

        public ConnectionForm(ref BindingList<Connection> conns)//вызывается в случае создания подключения
        {
            InitializeComponent();
            refConns = conns;
            this.Text = "Добавить подключение";
        }

        public ConnectionForm(ref Connection conn)//вызывается в случае редактирования подключения
        {
            InitializeComponent();
            this.conn = conn;
            this.Text = "Редактировать подключение";
            tbConnectionName.Text = conn.ConnName;
            tbComPort.Text = conn.Port.PortName;
            tbDelayRead.Text = conn.ReadTimeout.ToString();
            tbDelayWrite.Text = conn.WriteTimeout.ToString();
            btnAddAndConnect.Visible = false;
            btnAddConnection.Text = "Сохранить";
        }

        private void cbDevice_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cbConnType.SelectedIndex){
                case 0:
                    tbComPort.Visible = labelForCOMPort.Visible = false;
                    gbComParams.Enabled = false;
                    tbIp.Visible = tbPCPort.Visible = labelForIp.Visible = labelForPCPort.Visible = true;                   
                    break;
                default:
                    tbComPort.Visible = labelForCOMPort.Visible = true;
                    tbIp.Visible = tbPCPort.Visible = labelForIp.Visible = labelForPCPort.Visible = false;
                    gbComParams.Enabled = true;
                    if (conn==null) {
                        
                        cbBaudrate.SelectedIndex = 6;
                        cbParity.SelectedIndex = 0;
                        cbStopBits.SelectedIndex = 1;
                        cbDataBits.SelectedIndex = 1;
                        tbComPort.Text = cbConnType.Text;
                        tbConnectionName.Text ="WS-N01-" + cbDataBits.Text;//TODO автоматическая генерация имени соединения
                    } else {
                        cbBaudrate.Text = conn.Port.BaudRate.ToString();
                        cbParity.Text = conn.Port.Parity.ToString();
                        cbStopBits.Text = conn.Port.StopBits.ToString();
                        cbDataBits.Text = conn.Port.DataBits.ToString();
                    }

                    break;
            }
        }

        private void ConnectionForm_Load(object sender, EventArgs e)
        {
            //Заполняем поля параметров соединения формы
            cbConnType.Items.AddRange(SerialPort.GetPortNames());
            cbParity.Items.AddRange(Enum.GetNames(typeof(Parity)));
            cbStopBits.Items.AddRange(Enum.GetNames(typeof(StopBits)));
            //cbHandshake.Items.AddRange(Enum.GetNames(typeof(Handshake)));
            cbConnType.SelectedIndex = (conn == null) ? 0 : conn.ConnType;
        }
        
        private Connection CreateConnection()//Создание нового подключения
        {
            Connection tmpC = null;
            if (cbConnType.SelectedIndex==0)
            {
                MessageBox.Show("Создаём подключение TCP/IP");
            } else
            {
                string portName = tbComPort.Text;
                int baudRate = Convert.ToInt32(cbBaudrate.SelectedItem);
                Parity parity = (Parity)cbParity.SelectedIndex;
                StopBits stopBits = (StopBits)cbStopBits.SelectedIndex;
                int dataBits = Convert.ToInt32(cbDataBits.SelectedItem);

                SerialPort sp = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
                tmpC = new Connection(cbConnType.SelectedIndex, tbConnectionName.Text, sp, 
                                                    Convert.ToUInt32(tbDelayRead.Text), Convert.ToUInt32(tbDelayWrite.Text));
            }
            return tmpC;
        }
        
        private void btnAddConnection_Click(object sender, EventArgs e)
        {
            if (conn == null)
            {
                refConns.Add(CreateConnection());
            } else
            {
                conn.ConnType = cbConnType.SelectedIndex;
                conn.ConnName = tbConnectionName.Text;
                conn.Port.PortName = tbComPort.Text;
                conn.Port.BaudRate = Convert.ToInt32(cbBaudrate.Text);
                conn.Port.StopBits = (StopBits)Enum.Parse(typeof(StopBits), cbStopBits.Text);
                conn.Port.Parity = (Parity)Enum.Parse(typeof(Parity), cbParity.Text);
                conn.Port.DataBits = Convert.ToInt32(cbDataBits.Text);
                conn.WriteTimeout = Convert.ToUInt32(tbDelayWrite.Text);
                conn.ReadTimeout = Convert.ToUInt32(tbDelayRead.Text);
            }
            this.Close();
        }

        private void btnAddAndConnect_Click(object sender, EventArgs e)
        {
            refConns.Add(CreateConnection());       //создаём новое подключение
            
            foreach (Connection tmp in refConns)
                if ((tmp != null) && (!tmp.status))
                    tmp.Open();                         //подключаем все доступные    
            this.Close();
        }

        private void cbStopBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStopBits.Text == "None")
            {
                MessageBox.Show("Не поддерживается", "Ошибка");
                cbStopBits.SelectedIndex = 1;
            }
        }
    }
}
