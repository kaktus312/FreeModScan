using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
using System.ComponentModel;
using System.Diagnostics;

namespace FreeModScan
{
    public class Connection
    {
        SerialPort _port;
        public SerialPort Port { get { return _port; } set { _port = value; } }

        string _connName;
        public string ConnName { get { return _connName; } set { _connName = value; } }

        uint _readTimeout;
        public uint ReadTimeout { get { return _readTimeout; } set { _readTimeout = value; } }

        uint _writeTimeout;
        public uint WriteTimeout { get { return _writeTimeout; } set { _writeTimeout = value; } }

        bool _status;
        public bool status { get { return _status; } }

        public string statusString { get { return (_status) ? "Подключено" : "Отключено"; } }

        uint _queriesNumRCV;
        public uint QueriesNumRCV { get { return _queriesNumRCV; } set { _queriesNumRCV = value; } }

        uint _queriesNumSND;
        public uint QueriesNumSND { get { return _queriesNumSND; } set { _queriesNumSND = value; } }


        //long _queryTime;
        //public long QueryTime { get { return sw.ElapsedMilliseconds/100; } }

        int _connType;
        public int ConnType { get { return _connType; } set { _connType = value; } }

        public BindingList<Device> Devices = new BindingList<Device>();

        List<byte[]> requests = new List<byte[]>();
        int buffSize = 0;//требуемый размер буфера для полного ответа на текущий запрос
        int totalRCV;//общая длина ответа, которая должна равнятся buffSize при приёме в несколько этапов

        public Stopwatch sw;
        public Connection() {
            //для сериализации (при сохранении в XML) необходим конструктор без параметров 
        }
        public Connection(int ConnType, string ConName, SerialPort sp, uint readTout, uint writeTout)
        {
            this.ConnType = ConnType;
            this.ConnName = ConName;
            Port = sp;
            WriteTimeout = writeTout;
            ReadTimeout = readTout;
            Devices.ListChanged += new ListChangedEventHandler(Devices_ListChanged);
            Port.DataReceived += new SerialDataReceivedEventHandler(spDataReceived);
            sw = new Stopwatch();
        }

        private void Devices_ListChanged(object sender, ListChangedEventArgs e)
        {
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    //Add item here.
                    MessageBox.Show("Device Added");
                    break;

                case ListChangedType.ItemChanged:
                    //Change node associated with this item
                    MessageBox.Show("State changed");
                    break;

                case ListChangedType.ItemMoved:
                    //Parent changed.
                    MessageBox.Show("ItemMoved");
                    break;

                case ListChangedType.ItemDeleted:
                    MessageBox.Show("ItemDeleted");
                    break;

                case ListChangedType.Reset:
                    //This reset all data and control need to refill all data.
                    MessageBox.Show("Reset");
                    break;
            }
        }

        public void Open()
        {
            try
            {
                _port.Open();
                if (_port.IsOpen)
                {
                    _status = true;
                    //MessageBox.Show("Соединение установлено");
                    MainForm.console.Add("** " + DateTime.Now.ToString() + " - " + ConnName + " : Соединение по " + Port.PortName + " установлено. **" + "\n");
                }
            }
            catch (Exception err)
            {
                //MessageBox.Show(err.Message);
                MainForm.console.Add("** " + DateTime.Now.ToString() + " - " + ConnName + "-" + Port.PortName + " : " + err.Message + " **" + "\n");
            }
        }

        public void Close()
        {
            _port.Close();
            if (!_port.IsOpen)
            {
                _status = false;
                //MessageBox.Show("Соединение отключено");
                MainForm.console.Add("** " + DateTime.Now.ToString() + " - " + ConnName + " : Соединение по " + Port.PortName + " отключено. **" + "\n");
            }

        }

        private void spDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buff = new byte[buffSize];
            //Console.Out.WriteLine(DateTime.Now + " << [" + Port.BytesToRead + "]");
            //TODO предусмотреть прием ответа частями
            if (Port.BytesToRead >= buffSize)
            {
                //ответ полный, проверяем контрольную сумму и анализируем ответ
                Port.Read(buff, 0, buffSize);
                Console.Out.WriteLine(DateTime.Now + " << " + BitConverter.ToString(buff));
                MainForm.console.Add(DateTime.Now + " >> [" + BitConverter.ToString(buff) + "] \n");
                byte[] resp = buff.Take(buffSize - 2).ToArray();//без 2-х последних байтов ответа (CRC16)
                byte[] crc = buff.Skip(buffSize - 2).Take(2).ToArray();//2 последних байта ответа (CRC16)
                ushort crcRCV = BitConverter.ToUInt16(crc, 0);
                ushort _crc16 = MainForm.CRC.ComputeChecksum(resp);
                Console.Out.WriteLine(DateTime.Now + " << " + crcRCV + " / " + _crc16);
                
                if (crcRCV != _crc16)
                    return;
                if (resp[2] != 0x28)
                    return;//TODO сделать анализ ошибок modbus
                sw.Stop();
                Register.RegType rT = (Register.RegType)BitConverter.ToInt16(resp, 1);
                var tmpColl = Devices[MainForm.currDevice].Registers.Where(r => r.Type == rT);
                int index = 3;//первые 3 байта ответа - номер устройства, номер команды, признак ошибки/корректного ответа - пропускаем
                int skip = 0;
                foreach (Register r in tmpColl)
                {
                    if (skip > 0)
                    {
                        r.Status = false;
                        skip--;
                    }
                    else
                    {
                        r.Status = true;
                        int bytesNum;
                        switch (r.dataType)
                        {
                            case Register.DataType.Int16:
                            default:
                                bytesNum = 2;
                                break;
                            case Register.DataType.Int32:
                            case Register.DataType.Float:
                                bytesNum = 4;
                                break;
                            case Register.DataType.Int64:
                            case Register.DataType.Double:
                                bytesNum = 8;
                                break;
                        }

                        skip = bytesNum / 2 - 1;//сколько регистров необходимо пропустить для случая, если значение занимает несколько регистров
                        byte[] tmp = buff.Skip(index).Take(bytesNum).ToArray();

                        r.ValArr = tmp;
                        //Console.Out.Write(BitConverter.ToString(r.ValArr)+"\n");
                        index += bytesNum;
                    }

                }

                QueriesNumRCV++;

                totalRCV = 0;
            }
        }


        public void Poll(int ID)
        {
            if (requests.Count <= 0)
                QueryGen(ID);

            foreach (byte[] tmp in requests)
            {
                //Console.Out.WriteLine(DateTime.Now + " >> ");
                if (this.status)
                {
                    sw.Restart();
                    this.Port.Write(tmp, 0, 8);
                    buffSize = BitConverter.ToUInt16(tmp.Skip(4).Take(2).Reverse().ToArray(), 0);//BIG ENDIAN
                    buffSize = buffSize * 2 + 5;
                    //Console.Out.WriteLine(DateTime.Now + " >> " + buffSize.ToString());
                    QueriesNumSND++;
                    Console.Out.WriteLine(DateTime.Now + " >> [" + BitConverter.ToString(tmp) + "]");
                    MainForm.console.Add(DateTime.Now + " >> [" + BitConverter.ToString(tmp) + "] \n");
                }
                spDataReceived();
            }
            //requests.Clear();//или очищать или формировать запросы один раз и перегенерировать их при необходимости
        }

        private void spDataReceived()
        {
            //Эмулятор ответов
            byte[] buff = new byte[45] { 0x01, 0x03, 0x28, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0E, 0x08, 0xA1, 0x09, 0xA2, 0xAB, 0xCD, 0x01, 0x02, 0x03, 0xE8, 0x13, 0x8A, 0x00, 0x00, 0x00, 0x00, 0x74, 0x40 };
            Console.Out.WriteLine(DateTime.Now + " << [" + BitConverter.ToString(buff, 0)+"]");
            MainForm.console.Add(DateTime.Now + " >> [" + BitConverter.ToString(buff) + "] \n");

            if (buff.Count() >= buffSize)
            {
                //ответ полный, проверяем контрольную сумму и анализируем ответ
                byte[] resp = buff.Take(buffSize - 2).ToArray();//без 2-х последних байтов ответа (CRC16)
                byte[] crc = buff.Skip(buffSize - 2).Take(2).ToArray();//2 последних байта ответа (CRC16)
                ushort crcRCV = BitConverter.ToUInt16(crc, 0);
                ushort _crc16 = MainForm.CRC.ComputeChecksum(resp);
                Console.Out.WriteLine(DateTime.Now + " << " + crcRCV + " / " + _crc16);

                if (crcRCV != _crc16)
                    //return;
                if (resp[2] != 0x28)
                    return;//TODO сделать анализ ошибок modbus
                sw.Stop();
                Register.RegType rT = (Register.RegType)BitConverter.ToInt16(resp, 1);
                var tmpColl = Devices[MainForm.currDevice].Registers.Where(r => r.Type == rT);
                int index = 3;//первые 3 байта ответа - номер устройства, номер команды, признак ошибки/корректного ответа - пропускаем
                int skip = 0;
                foreach (Register r in tmpColl)
                {
                    if (skip > 0)
                    {
                        r.Status = false;
                        skip--;
                    }
                    else
                    {
                        r.Status = true;
                        int bytesNum;
                        switch (r.dataType)
                        {
                            case Register.DataType.Int16:
                            default:
                                bytesNum = 2;
                                break;
                            case Register.DataType.Int32:
                            case Register.DataType.Float:
                                bytesNum = 4;
                                break;
                            case Register.DataType.Int64:
                            case Register.DataType.Double:
                                bytesNum = 8;
                                break;
                        }

                        skip = bytesNum / 2 - 1;//сколько регистров необходимо пропустить для случая, если значение занимает несколько регистров

                        byte[] tmp = buff.Skip(index).Take(bytesNum).ToArray();

                        r.ValArr = tmp;
                        //Console.Out.Write(BitConverter.ToString(r.ValArr)+"\n");
                        index += bytesNum;
                    }

                }

                QueriesNumRCV++;

                totalRCV = 0;
            }
        }

        private void QueryGen(int ID)
        {
            //Формат запроса Modbus-RTU: SlaveID+"03"+"Начальный регистр"+"Количество регистров"+"Контрольная сумма" 
            //Например: 01 03 006B 0003 7417
            //где   0x01 - адрес прибора
            //      0х03 - команда (чтение)
            //    0x006B - начальный адрес группы опрашиваемых регисторов (107 в десятичном представлении)
            //    0x0003 - количество опрашиваемых регисторов в группе (3 в десятичном представлении)      
            //    0x7417 - контрольная сумма для предыдущих данных (29719 в десятичном представлении) 

            //1. Определяем адрес нужного прибора 
            //2. Определяем команду
            //3. Определяем минимальный адрес регистра в списке регистров данного устройства
            //4. Определяем количество регистров с учётом типов данных, хранящихся в них
            //5. Рассчитываем контрольную сумму для предыдущего набора данных (п. 1-4)
            //6. Формируем массив байтов для отправки запроса в порт, связанный с текущим соединением
            //7. Анализируем ответ        


            //TODO Формировать запрос с  учётом типа регистра
            foreach (Register.RegType tmp in Enum.GetValues(typeof(Register.RegType)))
            {
                //Console.Out.WriteLine(tmp);
                var tmpColl = Devices[ID].Registers.Where(T => T.Type == tmp);

                int counter = 0;
                uint minAddr = 999999;
                uint maxAddr = 0;
                for (int j = 0; j < tmpColl.Count(); j++)
                {
                    if (counter == 0)
                    {
                        minAddr = (uint)tmpColl.ElementAt(j).Adress;
                        maxAddr = (uint)tmpColl.ElementAt(j).Adress;
                        counter++;
                    }
                    else
                    {
                        long diff = (tmpColl.ElementAt(j).FullAdress - tmpColl.ElementAt(j - 1).FullAdress);
                        if (Math.Abs(diff) == 1)
                        {
                            if (tmpColl.ElementAt(j).Adress < minAddr)
                                minAddr = (uint)tmpColl.ElementAt(j).Adress;
                            if (tmpColl.ElementAt(j).Adress > maxAddr)
                                maxAddr = (uint)tmpColl.ElementAt(j).Adress;
                            counter++;
                        }
                        else
                        {
                            if (counter > 1)
                            {
                                CreateRequest(Devices[ID].deviceAdress, (byte)tmp, counter, minAddr);//для группы регистров с неприрывным диапазоном адресов
                                //CreateRequest(Devices[ID].deviceAdress, (byte)tmp, 1, (uint)tmpColl.ElementAt(j).Adress);//индивидуальный запрос для регистра вне группы
                                //counter = 0;
                                counter = 1;
                                minAddr = (uint)tmpColl.ElementAt(j).Adress;
                                maxAddr = (uint)tmpColl.ElementAt(j).Adress;
                            }
                            else
                            {
                                CreateRequest(Devices[ID].deviceAdress, (byte)tmp, 1, (uint)tmpColl.ElementAt(j - 1).Adress);//индивидуальный запрос для регистра вне группы
                                counter = 1;
                                minAddr = (uint)tmpColl.ElementAt(j).Adress;
                                maxAddr = (uint)tmpColl.ElementAt(j).Adress;
                            }
                        }
                    }
                }

                //формируем один запрос для группы регистров с неприрывным диапазоном адресов
                if (counter >= 1)
                    CreateRequest(Devices[ID].deviceAdress, (byte)tmp, counter, minAddr);
            }
        }

        private void CreateRequest(byte devAdr, byte command, int counter, uint minAddr)
        {
            if (counter > 0)
            {
                byte[] _dataBuff = new byte[6];
                _dataBuff[0] = devAdr;
                _dataBuff[1] = command;
                _dataBuff[2] = (byte)((minAddr) >> 8);
                _dataBuff[3] = (byte)(minAddr);
                _dataBuff[4] = (byte)(counter >> 8);
                _dataBuff[5] = (byte)counter;
                //подсчитываем CRC            
                ushort _crc16 = MainForm.CRC.ComputeChecksum(_dataBuff);
                byte[] _crcBUFF = BitConverter.GetBytes(_crc16);
                requests.Add(_dataBuff.Concat(_crcBUFF).ToArray());
            }
        }

        public ushort CalcCRC16(byte[] data)
        {
            //const ushort polynomial = 0x8005; //0x8005 = 1000 0000 0000 0101 - standard polynom
            const ushort polynomial = 0xA001;   //0xA001 = 1010 0000 0000 0001 - reversed polynom
            ushort crc = 0xffff;                //Обязательно! Инициализация контрольной суммы. Может быть 0x0000 или 0xffff. Для Modbus - 0xffff
            for (ushort i = 0; i < data.Length; i++)
            {
                crc ^= (UInt16)data[i];
                for (byte j = 0; j < 8; j++)
                {
                    if ((crc & 0x0001) == 0)
                        crc >>= 1;
                    else
                        crc = (ushort)((crc >> 1) ^ polynomial);
                }
            }
            return crc;
        }
    }
}
