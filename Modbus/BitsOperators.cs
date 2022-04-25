using System;

namespace SagaModbus.Modbus
{
    internal static class BitsOperators
    {
        public static bool BitRead(byte buffer, int pos)
        {
            if (pos > 7) throw new Exception("Invalid position for read from buffer!");
            return (int)(buffer & (0x01 << (byte)pos)) >= 1;
        }


        public static void BitWrite(ref byte buffer, int pos, bool value)
        {
            if (pos > 7) throw new Exception("Invalid position for write in the buffer!");

            if (value)
                buffer |= (byte)(1 << pos);
            else
                buffer &= (byte)(~(1 << pos));
        }

        public static byte LowByte(int value)
        {
            return (byte)(value & 0xFF);
        }


        public static byte HighByte(int value)
        {
            return (byte)(value >> 8);
        }
    }
}
