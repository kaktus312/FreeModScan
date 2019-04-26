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
            Float = 4,
            Double = 5
        }

        public enum ByteOrder : byte
        {
            LITTLEENDIAN  = 0,
            BIGENDIAN = 1,
            MIDLITTLEENDIAN = 2,
            MIDBIGENDIAN = 3
        }

        bool _useMults = false;
        public bool UseMults
        {
            get { return _useMults; }
            set
            {
                _useMults = value;
                Represent = (value) ? Representation.Float : Represent;
            }
        }
        string _title = "Default Register Title";
        public string Title { get { return _title; } set { _title = value; } }
        public string devName { get; set; }

        uint _offset;
        public uint Offset { get { return _offset; } set { _offset = value * 100000; } }

        long _adress = 0;
        public long Adress { get { return _adress; } set { _adress = value; } }
        public long FullAdress { get { return Offset + Adress; } }

        float _a = 1;
        public float A { get { return _a; } set { _a = value; } }
        public string strA
        {
            get { return (UseMults == true) ? A.ToString() : ""; }
            set { A = float.Parse(value); }
        }

        float _b = 0;
        public float B { get { return _b; } set { _b = value; } }
        public string strB
        {
            get { return (UseMults == true) ? B.ToString() : ""; }
            set { B = float.Parse(value); }
        }
        bool _status = true;
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
                _represent = (value == DataType.Float) ? Representation.Float : _represent;
                _useMults = (value == DataType.Float) ? true : _useMults;
            }
        }

        ByteOrder _byteOrder = ByteOrder.BIGENDIAN;
        public ByteOrder byteOrder { get { return _byteOrder; } set { _byteOrder = value; } }

        Representation _represent = Representation.DEC;    //представление данных
        public Representation Represent
        {
            get { return _represent; }
            set
            {
                _represent = value;
                _useMults = (value == Representation.Float);
            }
        }
        byte[] _valArr;
        public byte[] ValArr {
            //get { return (BitConverter.IsLittleEndian) ? _valArr.Reverse().ToArray(): _valArr; }
            get { return _valArr; }
            set { _valArr = value; }
        }



        public string stringVal
        {
            get
            {
                if ((!Status) || (ValArr == null))
                    return "-";

                dynamic tmp;

                var ba = RotateVal();
                int byteNum = ba.Count();

                switch (dataType)
                {
                    case DataType.Int16:
                    default:
                        tmp = BitConverter.ToInt16(ba, 0);
                        break;
                    case DataType.Int32:
                        tmp = BitConverter.ToInt32(ba, 0);
                        break;
                    case DataType.Int64:
                        tmp = BitConverter.ToInt64(ba, 0);
                        break;
                    case DataType.Float:
                        tmp = BitConverter.ToSingle(ba, 0);                            
                        break;
                    case DataType.Double:
                        tmp = BitConverter.ToDouble(ba, 0);
                        break;
                }

                if (_useMults)
                    tmp = tmp * A + B;

                return ConvertVal(tmp);
            }
        }

        byte[] RotateVal() 
        {
            int numBytes = ValArr.Count();
            int step = numBytes / 2;
            byte[] ba = new byte[numBytes];
            if (byteOrder == ByteOrder.MIDLITTLEENDIAN)
                for (int i = 0; i < numBytes; i += step)
                {
                    byte[] tmp = ValArr.Skip(i).Take(step).Reverse().ToArray();
                    Array.Copy(tmp, 0, ba, i, step);
                }
            if (byteOrder == ByteOrder.MIDBIGENDIAN)
                for (int i = 0; i < numBytes; i += step)
                    Array.Copy(ValArr, i, ba, (numBytes - (step + i)), step);
            if (byteOrder == ByteOrder.LITTLEENDIAN)
                ba = ValArr;
            if (byteOrder == ByteOrder.BIGENDIAN)
                ba = ValArr.Reverse().ToArray();
            return ba;
        }

        string ConvertVal(dynamic num)
        {
            string s = "???";
            byte[] ba = BitConverter.GetBytes(num);
            switch (Represent)
            {
                case Representation.HEX:
                    ba = ba.Reverse().ToArray();
                    s = BitConverter.ToString(ba, 0);
                    break;
                case Representation.BIN:
                    s = "";
                    ba = ba.Reverse().ToArray();
                    foreach (byte tmpByte in ba)
                        s += Convert.ToString(tmpByte, 2).PadLeft(8, '0') + " ";
                    break;
                case Representation.OCT:
                    s = Convert.ToString(num, 8);
                    break;
                case Representation.DEC:
                    s = Convert.ToString(num, 10);
                    break;
                case Representation.Float:
                    s = ((num < 0.000001) || (num > Int32.MaxValue)) ? num.ToString("E5") : num.ToString("F2");
                    break;
            }
            return s;
        }


        public Register()
        {
        }

        public Register(uint regNum)
            : this()
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
