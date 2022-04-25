using System.Collections.Generic;
using System.Linq;
using System;

namespace SagaModbus.Modbus.Utils
{
    internal static class CheckSum
    {
        internal static ushort Compute(byte[] buffer)
        {
            ushort crc = 0xFFFF;
            for (int pos = 0; pos < buffer.Length; pos++)
            {
                crc ^= (ushort)buffer[pos];    // XOR byte into least sig. byte of crc

                for (int i = 8; i != 0; i--)
                {    // Loop over each bit
                    if ((crc & 0x0001) != 0)
                    {      // If the LSB is set
                        crc >>= 1;                    // Shift right and XOR 0xA001
                        crc ^= 0xA001;
                    }
                    else                            // Else LSB is not set
                        crc >>= 1;                    // Just shift right
                }
            }

            return (ushort)((ushort)(crc << 8) | (crc >> 8));
        }

        internal static void MountBuffer(ref byte[] buffer)
        {
            ushort checkSum = Compute(buffer);

            List<byte> bufferList = buffer.ToList();
            bufferList.Add(BitsOperators.HighByte(checkSum));
            bufferList.Add(BitsOperators.LowByte(checkSum));

            buffer = bufferList.ToArray();
        }


        internal static bool IsMathCheckSum(byte[] buffer)
        {
            ushort checkSum = (ushort)((ushort)buffer[buffer.Length - 2] << 8 | buffer[buffer.Length - 1]);

            byte[] bufferWithoutCheckSum = new byte[buffer.Length - 2];
            Array.Copy(buffer, bufferWithoutCheckSum, buffer.Length - 2);

            ushort checkSumComputed = Compute(bufferWithoutCheckSum);

            return checkSum == checkSumComputed;
        }
    }
}

