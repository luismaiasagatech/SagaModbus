using SagaModbus.Modbus.Utils;
using SagaModbus.Interfaces;
using System;

namespace SagaModbus.Modbus
{
    enum EMBAPHeader : int
    {
        TRANS_ID_HIGH = 0,
        TRANS_ID_LOW = 1,
        PROTOCOL_ID_HIGH = 2,
        PROTOCOL_ID_LOW = 3,
        MSG_LEN_HIGH = 4,
        MSG_LEN_LOW = 5,
        UINT_ID = 6,
        FUNCTION = 7
    }

    internal class ModbusTCPIP : IModbusClient
    {
        private readonly int QuantityAttempts = 1;
        private readonly ModbusConnection ModbusConnection;
        private readonly ETypeReqModbus TypeReqModbus = ETypeReqModbus.TCPIP;

        public ModbusTCPIP(ModbusConnection _modbusConnection)
        {
            ModbusConnection = _modbusConnection;
            QuantityAttempts = ModbusClient.QuantityAttempts;
        }

        private bool IsValidMBAPHeader(byte[] buffer, byte[] response)
        {
            bool transactionID = (buffer[(int)EMBAPHeader.TRANS_ID_HIGH] << 8 | buffer[(int)EMBAPHeader.TRANS_ID_LOW]) == (response[(int)EMBAPHeader.TRANS_ID_HIGH] << 8 | response[(int)EMBAPHeader.TRANS_ID_LOW]);
            bool protocolID = (buffer[(int)EMBAPHeader.PROTOCOL_ID_HIGH] << 8 | buffer[(int)EMBAPHeader.PROTOCOL_ID_LOW]) == (response[(int)EMBAPHeader.PROTOCOL_ID_HIGH] << 8 | response[(int)EMBAPHeader.PROTOCOL_ID_LOW]);
            bool uintID = buffer[(int)EMBAPHeader.UINT_ID] == response[(int)EMBAPHeader.UINT_ID];
            bool function = buffer[(int)EMBAPHeader.FUNCTION] == response[(int)EMBAPHeader.FUNCTION];

            return transactionID && protocolID && uintID && function;
        }

        public bool[] SendReadStatusCoils(int _addressDevice, int _firstRegister, int _quantityRegisters)
        {
            try
            {
                for (int i = 0; i < QuantityAttempts; i++)
                {
                    var (buffer, sizeBufferExpected) = MountProtocolsModbus.FC_01(
                        _transactionID: ModbusConnection.GetTransactionID(),
                        _addressDevice,
                        _firstRegister,
                        _quantityRegisters);

                    byte[] response = ModbusConnection.SendRequest(buffer, sizeBufferExpected, TypeReqModbus);

                    if (response != null && IsValidMBAPHeader(buffer, response))
                    {
                        bool msgLength = ((uint)response[(int)EMBAPHeader.MSG_LEN_HIGH] << 8 | response[(int)EMBAPHeader.MSG_LEN_LOW]) == (sizeBufferExpected - 6);
                        int quantityCoilsBytes = (sizeBufferExpected - 9) == response[(int)EMBAPHeader.FUNCTION + 1] ? response[(int)EMBAPHeader.FUNCTION + 1] : 0;

                        if (msgLength && quantityCoilsBytes > 0)
                        {
                            bool[] statusCoils = new bool[quantityCoilsBytes * 8];

                            for (int y = 0; y < quantityCoilsBytes; y++)
                            {
                                for (int z = 0; z < 8; z++)
                                {
                                    statusCoils[y * 8 + z] = BitsOperators.BitRead(response[(int)EMBAPHeader.FUNCTION + 2 + y], z);
                                }
                            }

                            bool[] statusCoilsFiltered = new bool[_quantityRegisters];

                            Array.Copy(statusCoils, statusCoilsFiltered, _quantityRegisters);

                            return statusCoilsFiltered;
                        }
                    }
                }

                throw new Exception("Failed to read status coils FC 01 - ModbusTCP/IP");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public bool[] SendReadStatusDigitalStatus(int _addressDevice, int _firstRegister, int _quantityRegisters)
        {
            try
            {
                for (int i = 0; i < QuantityAttempts; i++)
                {
                    var (buffer, sizeBufferExpected) = MountProtocolsModbus.FC_02(
                        _transactionID: ModbusConnection.GetTransactionID(),
                        _addressDevice,
                        _firstRegister,
                        _quantityRegisters);

                    byte[] response = ModbusConnection.SendRequest(buffer, sizeBufferExpected, TypeReqModbus);

                    if (response != null && IsValidMBAPHeader(buffer, response))
                    {
                        bool msgLength = ((uint)response[(int)EMBAPHeader.MSG_LEN_HIGH] << 8 | response[(int)EMBAPHeader.MSG_LEN_LOW]) == (sizeBufferExpected - 6);
                        int quantityDiscreteInputsBytes = (sizeBufferExpected - 9) == response[(int)EMBAPHeader.FUNCTION + 1] ? response[(int)EMBAPHeader.FUNCTION + 1] : 0;

                        if (msgLength && quantityDiscreteInputsBytes > 0)
                        {
                            bool[] statusDiscreteInputs = new bool[quantityDiscreteInputsBytes * 8];

                            for (int y = 0; y < quantityDiscreteInputsBytes; y++)
                            {
                                for (int z = 0; z < 8; z++)
                                {
                                    statusDiscreteInputs[y * 8 + z] = BitsOperators.BitRead(response[(int)EMBAPHeader.FUNCTION + 2 + y], z);
                                }
                            }

                            bool[] statusDiscreteInputsFiltered = new bool[_quantityRegisters];

                            Array.Copy(statusDiscreteInputs, statusDiscreteInputsFiltered, _quantityRegisters);

                            return statusDiscreteInputsFiltered;
                        }
                    }
                }

                throw new Exception("Failed to read status coils FC 01 - ModbusTCP/IP");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public int[] SendReadHoldingRegisters(int _addressDevice, int _firstRegister, int _quantityRegisters)
        {
            try
            {
                for (int i = 0; i < QuantityAttempts; i++)
                {
                    var (buffer, sizeBufferExpected) = MountProtocolsModbus.FC_03(
                        _transactionID: ModbusConnection.GetTransactionID(),
                        _addressDevice,
                        _firstRegister,
                        _quantityRegisters);

                    byte[] response = ModbusConnection.SendRequest(buffer, sizeBufferExpected, TypeReqModbus);

                    if (response != null && IsValidMBAPHeader(buffer, response))
                    {
                        bool msgLength = ((uint)response[(int)EMBAPHeader.MSG_LEN_HIGH] << 8 | response[(int)EMBAPHeader.MSG_LEN_LOW]) == (sizeBufferExpected - 6);
                        int quantityHoldingRegistersBytes = (sizeBufferExpected - 9) == response[(int)EMBAPHeader.FUNCTION + 1] ? response[(int)EMBAPHeader.FUNCTION + 1] : 0;

                        if (msgLength && quantityHoldingRegistersBytes > 0)
                        {
                            int[] holdingRegisters = new int[quantityHoldingRegistersBytes / 2];

                            for (int y = 0; y < holdingRegisters.Length; y++)
                            {
                                holdingRegisters[y] = response[(int)EMBAPHeader.FUNCTION + (y + 1) * 2] << 8 | response[(int)EMBAPHeader.FUNCTION + (y + 1) * 2 + 1];
                            }

                            return holdingRegisters;
                        }
                    }
                }

                throw new Exception("Failed to read status coils FC 03 - ModbusTCP/IP");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int[] SendReadInputRegisters(int _addressDevice, int _firstRegister, int _quantityRegisters)
        {
            try
            {
                for (int i = 0; i < QuantityAttempts; i++)
                {
                    var (buffer, sizeBufferExpected) = MountProtocolsModbus.FC_04(
                        _transactionID: ModbusConnection.GetTransactionID(),
                        _addressDevice,
                        _firstRegister,
                        _quantityRegisters);

                    byte[] response = ModbusConnection.SendRequest(buffer, sizeBufferExpected, TypeReqModbus);

                    if (response != null && IsValidMBAPHeader(buffer, response))
                    {
                        bool msgLength = ((uint)response[(int)EMBAPHeader.MSG_LEN_HIGH] << 8 | response[(int)EMBAPHeader.MSG_LEN_LOW]) == (sizeBufferExpected - 6);
                        int quantityInputRegistersBytes = (sizeBufferExpected - 9) == response[(int)EMBAPHeader.FUNCTION + 1] ? response[(int)EMBAPHeader.FUNCTION + 1] : 0;

                        if (msgLength && quantityInputRegistersBytes > 0)
                        {
                            int[] inputRegisters = new int[quantityInputRegistersBytes / 2];

                            for (int y = 0; y < inputRegisters.Length; y++)
                            {
                                inputRegisters[y] = response[(int)EMBAPHeader.FUNCTION + (y + 1) * 2] << 8 | response[(int)EMBAPHeader.FUNCTION + (y + 1) * 2 + 1];
                            }

                            return inputRegisters;
                        }
                    }
                }

                throw new Exception("Failed to read status coils FC 04 - ModbusTCP/IP");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool SendForceSingleCoil(int _addressDevice, int _firstRegister, bool _statusToWrite)
        {
            try
            {
                for (int i = 0; i < QuantityAttempts; i++)
                {
                    var (buffer, sizeBufferExpected) = MountProtocolsModbus.FC_05(
                        _transactionID: ModbusConnection.GetTransactionID(),
                        _addressDevice,
                        _firstRegister,
                        _statusToWrite);

                    byte[] response = ModbusConnection.SendRequest(buffer, sizeBufferExpected, TypeReqModbus);

                    if (response != null && IsValidMBAPHeader(buffer, response))
                    {
                        bool msgLength = ((uint)response[(int)EMBAPHeader.MSG_LEN_HIGH] << 8 | response[(int)EMBAPHeader.MSG_LEN_LOW]) == 6;
                        bool firstRegister = ((uint)response[(int)EMBAPHeader.FUNCTION + 1] << 8 | response[(int)EMBAPHeader.FUNCTION + 2]) == _firstRegister;

                        if (msgLength && firstRegister)
                        {
                            return response[(int)EMBAPHeader.FUNCTION + 3] > 0;
                        }
                    }
                }

                throw new Exception("Failed to read status coils FC 05 - ModbusTCP/IP");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public int SendPresetSingleRegister(int _addressDevice, int _firstRegister, int _valueToWrite)
        {
            try
            {
                for (int i = 0; i < QuantityAttempts; i++)
                {
                    var (buffer, sizeBufferExpected) = MountProtocolsModbus.FC_06(
                        _transactionID: ModbusConnection.GetTransactionID(),
                        _addressDevice,
                        _firstRegister,
                        _valueToWrite);

                    byte[] response = ModbusConnection.SendRequest(buffer, sizeBufferExpected, TypeReqModbus);

                    if (response != null && IsValidMBAPHeader(buffer, response))
                    {
                        bool msgLength = ((uint)response[(int)EMBAPHeader.MSG_LEN_HIGH] << 8 | response[(int)EMBAPHeader.MSG_LEN_LOW]) == 6;
                        bool firstRegister = ((uint)response[(int)EMBAPHeader.FUNCTION + 1] << 8 | response[(int)EMBAPHeader.FUNCTION + 2]) == _firstRegister;

                        if (msgLength && firstRegister)
                        {
                            return ((int)response[(int)EMBAPHeader.FUNCTION + 3] << 8 | response[(int)EMBAPHeader.FUNCTION + 4]);
                        }
                    }
                }

                throw new Exception("Failed to read status coils FC 06 - ModbusTCP/IP");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public bool SendForceMultipleCoils(int _addressDevice, int _firstRegister, bool[] _valueToWrite)
        {
            try
            {
                for (int i = 0; i < QuantityAttempts; i++)
                {
                    var (buffer, sizeBufferExpected) = MountProtocolsModbus.FC_15(
                        _transactionID: ModbusConnection.GetTransactionID(),
                        _addressDevice,
                        _firstRegister,
                        _valueToWrite);

                    byte[] response = ModbusConnection.SendRequest(buffer, sizeBufferExpected, TypeReqModbus);

                    if (response != null && IsValidMBAPHeader(buffer, response))
                    {
                        bool msgLength = ((uint)response[(int)EMBAPHeader.MSG_LEN_HIGH] << 8 | response[(int)EMBAPHeader.MSG_LEN_LOW]) == 6;
                        bool firstRegister = ((uint)response[(int)EMBAPHeader.FUNCTION + 1] << 8 | response[(int)EMBAPHeader.FUNCTION + 2]) == _firstRegister;
                        bool quantity = ((uint)response[(int)EMBAPHeader.FUNCTION + 3] << 8 | response[(int)EMBAPHeader.FUNCTION + 4]) == _valueToWrite.Length;

                        if (msgLength && firstRegister && quantity)
                        {
                            return true;
                        }
                    }
                }

                throw new Exception("Failed to read status coils FC 15 - ModbusTCP/IP");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public bool SendPresetMultipleRegisters(int _addressDevice, int _firstRegister, int[] _valuesToWrite)
        {
            try
            {
                for (int i = 0; i < QuantityAttempts; i++)
                {
                    var (buffer, sizeBufferExpected) = MountProtocolsModbus.FC_16(
                        _transactionID: ModbusConnection.GetTransactionID(),
                        _addressDevice,
                        _firstRegister,
                        _valuesToWrite);

                    byte[] response = ModbusConnection.SendRequest(buffer, sizeBufferExpected, TypeReqModbus);

                    if (response != null && IsValidMBAPHeader(buffer, response))
                    {
                        bool msgLength = ((uint)response[(int)EMBAPHeader.MSG_LEN_HIGH] << 8 | response[(int)EMBAPHeader.MSG_LEN_LOW]) == 6;
                        bool firstRegister = ((uint)response[(int)EMBAPHeader.FUNCTION + 1] << 8 | response[(int)EMBAPHeader.FUNCTION + 2]) == _firstRegister;
                        bool quantity = ((uint)response[(int)EMBAPHeader.FUNCTION + 3] << 8 | response[(int)EMBAPHeader.FUNCTION + 4]) == _valuesToWrite.Length;

                        if (msgLength && firstRegister && quantity)
                        {
                            return true;
                        }
                    }
                }

                throw new Exception("Failed preset values to multiples registers FC 16 - ModbusTCP/IP");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
