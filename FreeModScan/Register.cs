using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FreeModScan
{
    public class Register
    {
        public enum RegType : byte
        {
            COIL = 1,
            DIGITAL_INPUT = 2,
            HOLDING_REGISTER = 3,
            INPUT_REGISTER = 4
        }

        public enum Representation : byte   //определять в форме без изменения регистров???
        {
            DEC = 0,
            HEX = 1,
            BIN = 2,
            OCT = 3,
            Float = 4
        }

        public enum DataType : byte
        {
            Int16 = 0,
            Int32 = 1,
            Int64 = 3,
            Float = 4
        }

        public enum ByteOrder : byte
        {
            LITTLEENDIAN = 0,
            BIGENDIAN = 1
        }

        bool _useMults;
        public bool UseMults { get { return _useMults; } set { _useMults = value; } }


        string _title = "Default Register Title";       //Имя регистра
        public string Title { get { return _title; } set { _title = value; } }

        string _devName;                                //Имя устройства
        public string devName { get { return _devName; } set { _devName = value; } }

        uint _offset;
        public uint Offset { get { return _offset; } set { _offset = value * 100000; } }

        long _adress = 0;                                //Адрес регистра
        public long Adress { get { return _adress; } set { _adress = value; } }
        public long FullAdress { get { return Offset + _adress; } }

        //TODO текстовый возврат множителей для возможности их скрытия когда не используются или не предусмотрены для данного типа данных
        float _a = 1;                                //Множитель A
        public float A
        {
            get { return _a; }
            set
            {
                _a = value;
                //float.TryParse(value.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out _a);
            }
        }

        float _b = 0;                                //Коэффициент B
        public float B {
            get { return _b; }
            set { _b = value; }
        }

        bool _status = true;    //Статус (опрашивать/не опрашивать) 
        public bool Status { get { return _status; } set { _status = value; } }

        RegType _type = RegType.HOLDING_REGISTER;    //тип регистра
        public RegType Type { get { return _type; } set { _type = value; Offset = (uint)value; } }

        DataType _dataType = DataType.Int16;           //тип данных регистра
        public DataType dataType
        {
            get { return _dataType; }
            set
            {
                _dataType = value;
                _represent = (value == DataType.Float) ? Representation.Float : Representation.DEC;
            }
        }

        ByteOrder _byteOrder = ByteOrder.LITTLEENDIAN;           //расположение байтов 
        public ByteOrder byteOrder { get { return _byteOrder; } set { _byteOrder = value; } }

        Representation _represent = Representation.DEC;    //представление данных
        public Representation Represent { get { return _represent; } set { _represent = value; } }

        byte[] _val; //Значение
        public byte[] ValArr
        {
            get
            {
                return _val;
            }
            set
            {
                _val = value;
            }
        }

        //TODO Обдумать хранение значения регистра (регистров) в виде массива байтов
        public string stringVal
        {
            get
            {
                string s = "-";
                if ((!Status) || (ValArr == null))
                    return s;
                var ba = (byteOrder == ByteOrder.BIGENDIAN) ? ValArr.Reverse().ToArray() : ValArr;


                //switch (dataType)
                //{
                //    case DataType.Int16:
                //    default:
                //        Int16 tmp = BitConverter.ToInt16(ba, 0);
                //    case DataType.Int32:
                //        Int32 tmp = BitConverter.ToInt32(ba, 0);
                //    case DataType.Int64:
                //        Int64 tmp = BitConverter.ToInt64(ba, 0);
                //    case DataType.Float:
                //        Int16 tmp = BitConverter.ToInt64(ba, 0);
                //}



                switch (Represent)
                {
                    case Representation.HEX:
                        //s = tmp.ToString("X2");
                        //s = Convert.ToString((Int16)tmp, 16);
                        s = BitConverter.ToString(ba, 0);
                        break;
                    case Representation.BIN:
                        //s = Convert.ToString((Int16)tmp, 2);
                        s = "";
                        foreach (byte tmpByte in ba)
                            s += Convert.ToString(tmpByte, 2).PadLeft(8, '0') + " ";
                        break;
                    case Representation.OCT:
                        //s = Convert.ToString(tmp, 8);
                        break;
                    case Representation.DEC:
                        if (ba.Count() == 8)
                        {
                            Int64 tmp = BitConverter.ToInt64(ba.Reverse().ToArray(), 0);
                            s = Convert.ToString(tmp, 10);
                            break;
                        }
                        if (ba.Count() == 4)
                        {
                            Int32 tmp = BitConverter.ToInt32(ba.Reverse().ToArray(), 0);
                            s = Convert.ToString(tmp, 10);
                            break;
                        }
                        if (ba.Count() == 2)
                        {
                            Int16 tmp = BitConverter.ToInt16(ba.Reverse().ToArray(), 0);
                            s = Convert.ToString(tmp, 10);
                            break;
                        }
                        else
                        {
                            s = "???";
                        }
                        break;
                    case Representation.Float:
                        s = Val.ToString("F2");
                        break;
                }
                return s;
            }
        }

        public float Val
        {
            get
            {
                //TODO сделать варианты для тип данных float и продумать другие
                Int16 tmp = BitConverter.ToInt16(ValArr.Reverse().ToArray(), 0);
                return tmp * A + B;
            }
        }

        public Register()
        {
        }

        public Register(uint regNum) : this()
        {
            Adress = regNum;
        }

        public Register(string dName, uint regNum)
            : this(regNum)
        {
            devName = dName;
        }

        public Register(string dName, uint regNum, RegType rType)
        {
            devName = dName;
            Type = rType;
            //Offset = (uint)rType;
            Adress = regNum;
        }

        public Register(string dName, uint regNum, RegType rType, DataType dType)
            : this(dName, regNum, rType)
        {
            dataType = dType;
        }

        public Register(string dName, uint regNum, RegType rType, DataType dType, ByteOrder bOrder)
            : this(dName, regNum, rType, dType)
        {
            byteOrder = bOrder;
        }

        public Register(string dName, uint regNum, RegType rType, DataType dType, ByteOrder bOrder, Representation repr)
            : this(dName, regNum, rType, dType, bOrder)
        {
            Represent = repr;
        }

        public Register(string dName, uint regNum, DataType dType)
        {
            devName = dName;
            dataType = dType;
            Adress = regNum;
        }
        public Register(string dName, uint regNum, DataType dType, ByteOrder bOrder)
            : this(dName, regNum, dType)
        {
            byteOrder = bOrder;
        }

    }
}
