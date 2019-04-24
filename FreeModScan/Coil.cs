using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeModScan
{
    class Coil : Register
    {
        RegType _type = RegType.COIL;    //тип регистра
        uint _offset = 100001;
        string _title = "Default Coil Register Title";       //Имя регистра
        public Coil(string dName, uint regNum, DataType dType, ByteOrder bOrder) : base(dName, regNum, dType) { }

    }

}
