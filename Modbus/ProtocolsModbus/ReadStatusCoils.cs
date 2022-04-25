using SagaModbus.Modbus.Utils;
using System;


namespace SagaModbus.Modbus.ProtocolsModbus
{
    internal static class ReadStatusCoils
    {
        private static readonly byte FunctionModbus = 0x01;

        //Modbus RTU
        public static Tuple<byte[], int> MountData(int _addressDevice, int _firstRegister, int _quantityRegisters)
        {
            int quantityCoils = (int)((_quantityRegisters / 8M) > 1M ? Math.Ceiling(_quantityRegisters / 8M) : 1M);
            int sizeBufferExpected = 5 + quantityCoils;


            byte[] buffer = {
                BitsOperators.LowByte(_addressDevice),
                FunctionModbus,
                BitsOperators.HighByte(_firstRegister),
                BitsOperators.LowByte(_firstRegister),
                BitsOperators.HighByte(_quantityRegisters),
                BitsOperators.LowByte(_quantityRegisters),
            };

            CheckSum.MountBuffer(ref buffer);

            return new Tuple<byte[], int>(buffer, sizeBufferExpected);
        }

        //Modbus TCPIP
        public static Tuple<byte[], int> MountData(int _transactionID, int _addressDevice, int _firstRegister, int _quantityRegisters)
        {
            int quantityCoils = (int)((_quantityRegisters / 8M) > 1M ? Math.Ceiling(_quantityRegisters / 8M) : 1M);
            int sizeBufferExpected = 6 + 3 + quantityCoils;


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
                BitsOperators.HighByte(_quantityRegisters),
                BitsOperators.LowByte(_quantityRegisters),
            };

            return new Tuple<byte[], int>(buffer, sizeBufferExpected);
        }
    }
}

