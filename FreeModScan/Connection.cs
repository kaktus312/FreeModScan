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

        public Queue<byte[]> requests = new Queue<byte[]>();
        int buffSize = 0;//требуемый размер буфера для полного ответа на текущий запрос
        int totalRCV;//общая длина ответа, которая должна равнятся buffSize при приёме в несколько этапов

        public Stopwatch sw;

        public bool dataProcessed=false;

        public delegate void ConnectionEventHandler(Connection c);
        public static event ConnectionEventHandler Create;
        public static event ConnectionEventHandler StateChanged;
        public static event ConnectionEventHandler BeforeDelete;
        public static event ConnectionEventHandler Delete;

        public delegate void ConnectionErrorHandler(Exception err);
        public static event ConnectionErrorHandler Error;




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
            Register.Delete += new Register.RegisterEventHandler(RegisterDeleted);
            sw = new Stopwatch();
        }

        private void RegisterDeleted(Register r)
        {
            /*
             * 1. Остановить опрос
             * 2. Обновить запрос в соответствии с новым набором регистров
             * 3. Запустить опрос
             * Аналогично - для случая добавления регистра
             */
            Console.Write("Class Connection: Register Deleted");
        }

        private void Devices_ListChanged(object sender, ListChangedEventArgs e)
        {
            Device dev = null;
            int devsNum = Devices.Count();//поличество подключений после изменения

            if (e.NewIndex != devsNum)
                dev = Devices[e.NewIndex];//объект с которым производились манипуляции
           
            switch (e.ListChangedType)
            {
                case ListChangedType.ItemAdded:
                    //Add item here.
                    MessageBox.Show("Device Added");
                    dev.Registers.ListChanged += new ListChangedEventHandler(RegistersListChanged);
                    //запуск события Создания для других подписчиков
                    dev.OnCreate();
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

        private void RegistersListChanged(object sender, ListChangedEventArgs e)
        {
            //UpdateRequests();
           //MessageBox.Show("Bingo2!");//не выводить Messagebox иначе - ошибка при удалении строк в dgvTable
        }

        //Методы On.....() - для запуска события из внешних классов
        public void OnCreate() {
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

        public void Open()
        {
            try
            {
                _port.Open();
                if (_port.IsOpen)
                {
                    _status = true;
                    OnStateChanged();
                }
            }
            catch (Exception err)
            {
                OnError(err);
            }
        }

        public void Close()
        {
            try
            {
                _port.Close();
                if (!_port.IsOpen)
                {
                    _status = false;
                    OnStateChanged();
                }
            }
            catch (Exception err)
            {
                OnError(err);
            }

        }

        private void spDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //try
            //{
            lock (MainForm.locker)
            {

            
            byte[] buff = new byte[buffSize];
                //byte[] buff = new byte[Port.BytesToRead];
                //Console.Out.WriteLine(DateTime.Now + " << [" + Port.BytesToRead + "]");
                //Port.Read(buff, 0, Port.BytesToRead);
                //Console.Out.WriteLine(DateTime.Now + " << " + BitConverter.ToString(buff));


                //byte[] resp = buff.Take(buffSize - 2).ToArray();//без 2-х последних байтов ответа (CRC16)
                //byte[] crc = buff.Skip(buffSize - 2).Take(2).ToArray();//2 последних байта ответа (CRC16)
                //TODO предусмотреть прием ответа частями
                if (Port.BytesToRead >= buffSize)
                {
                    //ответ полный, проверяем контрольную сумму и анализируем ответ
                    Port.Read(buff, 0, buffSize);
                    Console.Write(" << " + BitConverter.ToString(buff));

                    byte[] resp = buff.Take(buffSize - 2).ToArray();//без 2-х последних байтов ответа (CRC16)
                    byte[] crc = buff.Skip(buffSize - 2).Take(2).ToArray();//2 последних байта ответа (CRC16)
                    ushort crcRCV = BitConverter.ToUInt16(crc, 0);
                    ushort _crc16 = MainForm.CRC.ComputeChecksum(resp);

                    if (crcRCV != _crc16)
                        return;

                    sw.Stop();
                    Register.RegType rT = (Register.RegType)BitConverter.ToInt16(resp, 1);
                    var tmpColl = MainForm.currDevice.Registers.Where(r => r.Type == rT);
                    int index = 3;//первые 3 байта ответа - номер устройства, номер команды, признак ошибки/корректного ответа - пропускаем
                    int skip = 0;
                    int skipRegs = 0;
                    int startAdrr = 0;
                    foreach (Register r in tmpColl)
                    {
                        int bytesNum = Convert.ToInt32(r.ByteNum());
                        int adr = (int) r.Adress;
                        if ((adr - startAdrr) <= (skipRegs))
                        {
                            r.Status = false;
                        }
                        else
                        { 
                            r.Status = true;
                            skip = (int) (r.Adress - 1) * 2 + 3;//сколько регистров необходимо пропустить для случая, если значение занимает несколько регистров
                            byte[] tmp = buff.Skip(skip).Take(bytesNum).ToArray();
                            r.ValArr = tmp;
                            skipRegs = bytesNum/2-1;
                            startAdrr = adr;
                        } 
                    }

                    QueriesNumRCV++;

                    totalRCV = 0;
                    dataProcessed = true;
                }
            }
            //} catch(Exception exept)
            //{
            //    Console.Write(exept.Message);
            //}
            //OnDataReady();
        }

        private void spDataReceived()
        {
            //Эмулятор ответов
            byte[] buff = new byte[45] { 0x01, 0x03, 0x28, 0x00, 0x00, 0x00, 0x0E, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x0E, 0x08, 0xA1, 0x09, 0xA2, 0xAB, 0xCD, 0x01, 0x02, 0x03, 0xE8, 0x13, 0x8A, 0x00, 0x00, 0x00, 0x00, 0x74, 0x40 };
            Console.Out.WriteLine(DateTime.Now + " << [" + BitConverter.ToString(buff, 0)+"]");
            Console.Write(" >> [" + BitConverter.ToString(buff) + "]");

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
                //var tmpColl = Devices[MainForm.currDevice].Registers.Where(r => r.Type == rT);
                var tmpColl = MainForm.currDevice.Registers.Where(r => r.Type == rT);
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
                        int bytesNum = Convert.ToInt32(r.ByteNum());
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

        public void Poll()
        {
            //if (requests.Count <= 0)
            //    QueryGen(ID);
            
            //foreach (byte[] tmp in requests)
            //{
                //Console.Out.WriteLine(DateTime.Now + " >> ");
                if (this.status)
                {
                byte[] tmp = requests.Dequeue();
                //byte[] tmp = requests.Peek();
                sw.Restart();

                    buffSize = BitConverter.ToUInt16(tmp.Skip(4).Take(2).Reverse().ToArray(), 0);//BIG ENDIAN
                    buffSize = buffSize * 2 + 5;
                    Console.Out.WriteLine(DateTime.Now + " >> " + buffSize.ToString());
                    this.Port.Write(tmp, 0, 8);
                requests.Enqueue(tmp);
                dataProcessed = false;
                    QueriesNumSND++;
                    Console.Out.WriteLine(DateTime.Now + " >> [" + BitConverter.ToString(tmp) + "]");
                    Console.Write(" >> [" + BitConverter.ToString(tmp) + "]");
                }
                
            //}
            //spDataReceived();
            //requests.Clear();//или очищать или формировать запросы один раз и перегенерировать их при необходимости
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
                var tmpColl = Devices[ID].Registers.Where(T => T.Type == tmp);//TODO реализовать опрос регистров других типов

                uint counter = 0;
                uint minAddr = 999999;
                uint maxAddr = 0;
                int numRegs = tmpColl.Count();
                //int sh = 0;
                for (int j = 0; j < numRegs; j++)
                {
                    uint regSize = tmpColl.ElementAt(j).RegSize();
                    long adr = tmpColl.ElementAt(j).Adress;
                    //long delta = adr - maxAddr;
                    //if (Math.Abs(delta) == 1)
                    //{
                        if (adr < minAddr)
                            minAddr = (uint)(adr-1);
                    if (adr > maxAddr)
                        maxAddr = (uint)(adr - 1)+ regSize;//TODO Организовать изменение запроса при изменении типа регистра

                    counter += regSize;
                    //}
                    //else
                    //{
                    //    if ((delta > 1) && (counter >= 1))
                    //    {
                    //        CreateRequest(Devices[ID].deviceAdress, (byte)tmp, counter, minAddr); //создаём запрос для группы регистров с неприрывным диапазоном адресов
                    //        minAddr = (uint)(adr - 1);
                    //        maxAddr = minAddr+regSize;
                    //        counter = regSize;
                    //    }
                    //}
                }

                //формируем один запрос для группы регистров с неприрывным диапазоном адресов
                if (counter >= 1)
                    CreateRequest(Devices[ID].deviceAdress, (byte)tmp, maxAddr, minAddr);
            }
        }
       //private void QueryGen(int ID)
       // {
       //     //Формат запроса Modbus-RTU: SlaveID+"03"+"Начальный регистр"+"Количество регистров"+"Контрольная сумма" 
       //     //Например: 01 03 006B 0003 7417
       //     //где   0x01 - адрес прибора
       //     //      0х03 - команда (чтение)
       //     //    0x006B - начальный адрес группы опрашиваемых регисторов (107 в десятичном представлении)
       //     //    0x0003 - количество опрашиваемых регисторов в группе (3 в десятичном представлении)      
       //     //    0x7417 - контрольная сумма для предыдущих данных (29719 в десятичном представлении) 

       //     //1. Определяем адрес нужного прибора 
       //     //2. Определяем команду
       //     //3. Определяем минимальный адрес регистра в списке регистров данного устройства
       //     //4. Определяем количество регистров с учётом типов данных, хранящихся в них
       //     //5. Рассчитываем контрольную сумму для предыдущего набора данных (п. 1-4)
       //     //6. Формируем массив байтов для отправки запроса в порт, связанный с текущим соединением
       //     //7. Анализируем ответ        


       //     //TODO Формировать запрос с  учётом типа регистра
       //     foreach (Register.RegType tmp in Enum.GetValues(typeof(Register.RegType)))
       //     {
       //         //Console.Out.WriteLine(tmp);
       //         var tmpColl = Devices[ID].Registers.Where(T => T.Type == tmp);//TODO реализовать опрос регистров других типов

       //         uint counter = 0;
       //         uint minAddr = 999999;
       //         uint maxAddr = 0;
       //         int numRegs = tmpColl.Count();
       //         //int sh = 0;
       //         for (int j = 0; j < numRegs; j++)
       //         {
       //             uint regSize = tmpColl.ElementAt(j).RegSize();
       //             long adr = tmpColl.ElementAt(j).Adress;
       //             long delta = adr - maxAddr;
       //             if (Math.Abs(delta) == 1)
       //             {
       //                 if (adr < minAddr)
       //                     minAddr = (uint)(adr-1);
       //                 maxAddr += regSize;
       //                 counter += regSize;
       //             }
       //             else
       //             {
       //                 if ((delta > 1) && (counter >= 1))
       //                 {
       //                     CreateRequest(Devices[ID].deviceAdress, (byte)tmp, counter, minAddr); //создаём запрос для группы регистров с неприрывным диапазоном адресов
       //                     minAddr = (uint)(adr - 1);
       //                     maxAddr = minAddr+regSize;
       //                     counter = regSize;
       //                 }
       //             }
       //         }

       //         //формируем один запрос для группы регистров с неприрывным диапазоном адресов
       //         if (counter >= 1)
       //             CreateRequest(Devices[ID].deviceAdress, (byte)tmp, counter, minAddr);
       //     }
       // }

        public void ClearRequests()
        {
            this.requests.Clear();
        }

        public void UpdateRequests()
        {
            ClearRequests();
            for (int i = 0; i < Devices.Count(); i++)
                QueryGen(i);
        }
        
        private void CreateRequest(byte devAdr, byte command, uint counter, uint minAddr)
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
                requests.Enqueue(_dataBuff.Concat(_crcBUFF).ToArray());
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
