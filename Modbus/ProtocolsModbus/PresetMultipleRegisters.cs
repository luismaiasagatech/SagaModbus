using SagaModbus.Modbus.Utils;
using System.Linq;
using System;

namespace SagaModbus.Modbus.ProtocolsModbus
{
    internal class PresetMultipleRegisters
    {
        private static readonly byte FunctionModbus = 0x10;

        //Modbus RTU
        public static Tuple<byte[], int> MountData(int _addressDevice, int _firstRegister, int[] _valuesToWrite)
        {
            int sizeBufferExpected = 8;
            int quantityBytes = _valuesToWrite.Length * 2;

            byte[] dataToWrite = new byte[quantityBytes];

            for (int i = 0; i < _valuesToWrite.Length; i++)
            {
                dataToWrite[i * 2] = BitsOperators.HighByte(_valuesToWrite[i]);
                dataToWrite[(i * 2) + 1] = BitsOperators.LowByte(_valuesToWrite[i]);
            }


            byte[] buffer = {
                BitsOperators.LowByte(_addressDevice),
                FunctionModbus,
                BitsOperators.HighByte(_firstRegister),
                BitsOperators.LowByte(_firstRegister),
                BitsOperators.HighByte(_valuesToWrite.Length),
                BitsOperators.LowByte(_valuesToWrite.Length),
                (byte)quantityBytes,
            };

            buffer.ToList().AddRange(dataToWrite.ToList());

            CheckSum.MountBuffer(ref buffer);

            return new Tuple<byte[], int>(buffer, sizeBufferExpected);
        }

        //Modbus TCPIP
        public static Tuple<byte[], int> MountData(int _transactionID, int _addressDevice, int _firstRegister, int[] _valuesToWrite)
        {
            int sizeBufferExpected = 12;
            int quantityBytes = _valuesToWrite.Length * 2;

            byte[] dataToWrite = new byte[quantityBytes];

            for (int i = 0; i < _valuesToWrite.Length; i++)
            {
                dataToWrite[i * 2] = BitsOperators.HighByte(_valuesToWrite[i]);
                dataToWrite[(i * 2) + 1] = BitsOperators.LowByte(_valuesToWrite[i]);
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
                BitsOperators.HighByte(_valuesToWrite.Length),
                BitsOperators.LowByte(_valuesToWrite.Length),
                (byte)quantityBytes,
            };

            buffer.ToList().AddRange(dataToWrite.ToList());

            return new Tuple<byte[], int>(buffer, sizeBufferExpected);
        }
    }
}
