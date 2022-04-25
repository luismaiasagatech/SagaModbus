


using SagaModbus.Modbus.Utils;
using System;
using System.Linq;

namespace SagaModbus.Modbus.ProtocolsModbus
{
    internal class ForceMultipleCoils
    {
        private static readonly byte FunctionModbus = 0x0F;

        //Modbus RTU
        public static Tuple<byte[], int> MountData(int _addressDevice, int _firstRegister, bool[] _statusToWrite)
        {
            int sizeBufferExpected = 8;
            int quantityBytes = (int)(Math.Ceiling(_statusToWrite.Length / 8.0m) > 1m ? Math.Ceiling(_statusToWrite.Length / 8.0m) : 1m);

            byte[] dataToWrite = new byte[quantityBytes];

            int currentBytePos = 0;

            for (int i = 0; i < _statusToWrite.Length; i++)
            {
                if ((i % 8) == 0 && i != 0)
                {
                    ++currentBytePos;
                }

                dataToWrite[currentBytePos] |= (byte)(Convert.ToByte(_statusToWrite[i]) << (i - currentBytePos * 8));
            }


            byte[] buffer = {
                BitsOperators.LowByte(_addressDevice),
                FunctionModbus,
                BitsOperators.HighByte(_firstRegister),
                BitsOperators.LowByte(_firstRegister),
                BitsOperators.HighByte(_statusToWrite.Length),
                BitsOperators.LowByte(_statusToWrite.Length),
                (byte)quantityBytes,
            };

            buffer.ToList().AddRange(dataToWrite.ToList());

            CheckSum.MountBuffer(ref buffer);

            return new Tuple<byte[], int>(buffer, sizeBufferExpected);
        }

        //Modbus TCPIP
        public static Tuple<byte[], int> MountData(int _transactionID, int _addressDevice, int _firstRegister, bool[] _statusToWrite)
        {
            int sizeBufferExpected = 12;

            int quantityBytes = (int)(Math.Ceiling(_statusToWrite.Length / 8.0m) > 1m ? Math.Ceiling(_statusToWrite.Length / 8.0m) : 1m);

            byte[] dataToWrite = new byte[quantityBytes];

            int currentBytePos = 0;

            for (int i = 0; i < _statusToWrite.Length; i++)
            {
                if ((i % 8) == 0 && i != 0)
                {
                    ++currentBytePos;
                }

                dataToWrite[currentBytePos] |= (byte)(Convert.ToByte(_statusToWrite[i]) << (i - currentBytePos * 8));
            }


            byte[] buffer = {
                BitsOperators.HighByte(_transactionID),
                BitsOperators.LowByte(_transactionID),
                0x00,
                0x00,
                0x00,
                0x06,
                BitsOperators.LowByte(_addressDevice),
                FunctionModbus,
                BitsOperators.HighByte(_firstRegister),
                BitsOperators.LowByte(_firstRegister),
                BitsOperators.HighByte(_statusToWrite.Length),
                BitsOperators.LowByte(_statusToWrite.Length),
                (byte)quantityBytes,
            };

            buffer.ToList().AddRange(dataToWrite.ToList());

            return new Tuple<byte[], int>(buffer, sizeBufferExpected);
        }
    }
}
