using System;
namespace FreeModScan
{
    public class CRC16
    {
        //const ushort polynomial = 0x8005;//0x8005 = 1000 0000 0000 0101 - standard polynom
        const ushort polynomial = 0xA001;//0xA001 = 1010 0000 0000 0001 - reversed polynom
        
        ushort[] table = new ushort[256];

        public CRC16()
        {
            ushort value;
            ushort temp;
            for (ushort i = 0; i < table.Length; ++i)
            {
                value = 0;
                temp = i;
                for (byte j = 0; j < 8; ++j)
                {
                    if (((value ^ temp) & 0x0001) != 0)
                    {
                        value = (ushort)((value >> 1) ^ polynomial);
                    }
                    else
                    {
                        value >>= 1;
                    }
                    temp >>= 1;
                }
                table[i] = value;
            }
        }


        public ushort ComputeChecksum(byte[] bytes)
        {
            ushort crc = 0xffff; //инициализация контрольной суммы. Может быть 0x0000 или 0xffff. Для Modbus - 0xffff
            for (int i = 0; i < bytes.Length; ++i)
            {
                byte index = (byte)(crc ^ bytes[i]);
                crc = (ushort)((crc >> 8) ^ table[index]);
            }
            return crc;
        }

        public byte[] ComputeChecksumBytes(byte[] bytes)
        {
            ushort crc = ComputeChecksum(bytes);
            return BitConverter.GetBytes(crc);
        }
    }
}
