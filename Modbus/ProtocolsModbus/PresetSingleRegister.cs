using SagaModbus.Modbus.Utils;
using System;


namespace SagaModbus.Modbus.ProtocolsModbus
{
    internal static class PresetSingleRegister
    {
        private static readonly byte FunctionModbus = 0x06;

        //Modbus RTU
        public static Tuple<byte[], int> MountData(int _addressDevice, int _firstRegister, int _valueToWrite)
        {
            int sizeBufferExpected = 8;


            byte[] buffer = {
                BitsOperators.LowByte(_addressDevice),
                FunctionModbus,
                BitsOperators.HighByte(_firstRegister),
                BitsOperators.LowByte(_firstRegister),
                BitsOperators.HighByte(_valueToWrite),
                BitsOperators.LowByte(_valueToWrite),
            };

            CheckSum.MountBuffer(ref buffer);

            return new Tuple<byte[], int>(buffer, sizeBufferExpected);
        }

        //Modbus TCPIP
        public static Tuple<byte[], int> MountData(int _transactionID, int _addressDevice, int _firstRegister, int _valueToWrite)
        {
            int sizeBufferExpected = 12;


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
                BitsOperators.HighByte(_valueToWrite),
                BitsOperators.LowByte(_valueToWrite),
            };

            return new Tuple<byte[], int>(buffer, sizeBufferExpected);
        }
    }
}
