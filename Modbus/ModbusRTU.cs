using SagaModbus.Modbus.Utils;
using SagaModbus.Interfaces;
using System;

namespace SagaModbus.Modbus
{
    enum ERTUHeader
    {
        ADDR_DEVICE = 0,
        FUNCTION = 1,
    }

    internal class ModbusRTU : IModbusClient
    {
        private readonly int QuantityAttempts = 1;
        private readonly ModbusConnection ModbusConnection;
        private readonly ETypeReqModbus TypeReqModbus = ETypeReqModbus.RTU;

        public ModbusRTU(ModbusConnection _modbusConnection)
        {
            ModbusConnection = _modbusConnection;
            QuantityAttempts = ModbusClient.QuantityAttempts;
        }


        internal bool IsValidRTU(byte[] buffer, byte[] response)
        {
            bool addressDevice = buffer[(int)ERTUHeader.ADDR_DEVICE] == response[(int)ERTUHeader.ADDR_DEVICE];
            bool function = buffer[(int)ERTUHeader.FUNCTION] == response[(int)ERTUHeader.FUNCTION];
            bool checkSum = CheckSum.IsMathCheckSum(response);

            return addressDevice && function && checkSum;
        }


        public bool[] SendReadStatusCoils(int _addressDevice, int _firstRegister, int _quantityRegisters)
        {

            try
            {
                for (int i = 0; i < QuantityAttempts; i++)
                {
                    var (buffer, sizeBufferExpected) = MountProtocolsModbus.FC_01(
                        _addressDevice,
                        _firstRegister,
                        _quantityRegisters);

                    byte[] response = ModbusConnection.SendRequest(buffer, sizeBufferExpected, TypeReqModbus);

                    if (response != null && IsValidRTU(buffer, response))
                    {
                        int quantityCoilsBytes = (sizeBufferExpected - 5) == response[(int)ERTUHeader.FUNCTION + 1] ? response[(int)ERTUHeader.FUNCTION + 1] : 0;

                        if (quantityCoilsBytes > 0)
                        {
                            bool[] statusCoils = new bool[quantityCoilsBytes * 8];

                            for (int y = 0; y < quantityCoilsBytes; y++)
                            {
                                for (int z = 0; z < 8; z++)
                                {
                                    statusCoils[y * 8 + z] = BitsOperators.BitRead(response[(int)ERTUHeader.FUNCTION + 2 + y], z);
                                }
                            }

                            return statusCoils;
                        }
                    }
                }

                throw new Exception("Failed to read status coils FC 01 - ModbusRTU");
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
                        _addressDevice,
                        _firstRegister,
                        _quantityRegisters);

                    byte[] response = ModbusConnection.SendRequest(buffer, sizeBufferExpected, TypeReqModbus);

                    if (response != null && IsValidRTU(buffer, response))
                    {
                        int quantityDiscreteInputsBytes = (sizeBufferExpected - 5) == response[(int)ERTUHeader.FUNCTION + 1] ? response[(int)ERTUHeader.FUNCTION + 1] : 0;

                        if (quantityDiscreteInputsBytes > 0)
                        {
                            bool[] statusDiscreteInputs = new bool[quantityDiscreteInputsBytes * 8];

                            for (int y = 0; y < quantityDiscreteInputsBytes; y++)
                            {
                                for (int z = 0; z < 8; z++)
                                {
                                    statusDiscreteInputs[y * 8 + z] = BitsOperators.BitRead(response[(int)ERTUHeader.FUNCTION + 2 + y], z);
                                }
                            }

                            return statusDiscreteInputs;
                        }
                    }
                }

                throw new Exception("Failed to read digital inputs FC 02 - ModbusRTU");
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
                        _addressDevice,
                        _firstRegister,
                        _quantityRegisters);

                    byte[] response = ModbusConnection.SendRequest(buffer, sizeBufferExpected, TypeReqModbus);

                    if (response != null && IsValidRTU(buffer, response))
                    {
                        int quantityHoldingRegistersBytes = (sizeBufferExpected - 5) == response[(int)ERTUHeader.FUNCTION + 1] ? response[(int)ERTUHeader.FUNCTION + 1] : 0;

                        if (quantityHoldingRegistersBytes > 0)
                        {
                            int[] holdingRegisters = new int[quantityHoldingRegistersBytes / 2];

                            for (int y = 0; y < holdingRegisters.Length; y++)
                            {
                                holdingRegisters[y] = response[(int)ERTUHeader.FUNCTION + (y + 1) * 2] << 8 | response[(int)ERTUHeader.FUNCTION + (y + 1) * 2 + 1];
                            }

                            return holdingRegisters;
                        }
                    }
                }

                throw new Exception("Failed to single content retentive FC 03 - ModbusRTU");
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
                        _addressDevice,
                        _firstRegister,
                        _quantityRegisters);

                    byte[] response = ModbusConnection.SendRequest(buffer, sizeBufferExpected, TypeReqModbus);

                    if (response != null && IsValidRTU(buffer, response))
                    {
                        int quantityInputRegistersBytes = (sizeBufferExpected - 5) == response[(int)ERTUHeader.FUNCTION + 1] ? response[(int)ERTUHeader.FUNCTION + 1] : 0;

                        if (quantityInputRegistersBytes > 0)
                        {
                            int[] inputRegisters = new int[quantityInputRegistersBytes / 2];

                            for (int y = 0; y < inputRegisters.Length; y++)
                            {
                                inputRegisters[y] = response[(int)ERTUHeader.FUNCTION + (y + 1) * 2] << 8 | response[(int)ERTUHeader.FUNCTION + (y + 1) * 2 + 1];
                            }

                            return inputRegisters;
                        }
                    }
                }

                throw new Exception("Failed to read analog inputs FC 04 - ModbusRTU");
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
                        _addressDevice,
                        _firstRegister,
                        _statusToWrite);

                    byte[] response = ModbusConnection.SendRequest(buffer, sizeBufferExpected, TypeReqModbus);

                    if (response != null && IsValidRTU(buffer, response))
                    {
                        bool firstRegister = (response[(int)ERTUHeader.FUNCTION + 1] << 8 | response[(int)ERTUHeader.FUNCTION + 2]) == _firstRegister;

                        if (firstRegister)
                        {
                            return response[(int)ERTUHeader.FUNCTION + 3] > 0;
                        }
                    }
                }

                throw new Exception("Failed to write in single coil FC 05 - ModbusRTU");
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
                        _addressDevice,
                        _firstRegister,
                        _valueToWrite);

                    byte[] response = ModbusConnection.SendRequest(buffer, sizeBufferExpected, TypeReqModbus);

                    if (response != null && IsValidRTU(buffer, response))
                    {
                        bool firstRegister = (response[(int)ERTUHeader.FUNCTION + 1] << 8 | response[(int)ERTUHeader.FUNCTION + 2]) == _firstRegister;

                        if (firstRegister)
                        {
                            return ((int)response[(int)ERTUHeader.FUNCTION + 3] << 8 | response[(int)ERTUHeader.FUNCTION + 4]);
                        }
                    }
                }

                throw new Exception("Failed to preset single register FC 06 - ModbusRTU");
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
                        _addressDevice,
                        _firstRegister,
                        _valueToWrite);

                    byte[] response = ModbusConnection.SendRequest(buffer, sizeBufferExpected, TypeReqModbus);

                    if (response != null && IsValidRTU(buffer, response))
                    {
                        bool firstRegister = ((uint)response[(int)ERTUHeader.FUNCTION + 1] << 8 | response[(int)ERTUHeader.FUNCTION + 2]) == _firstRegister;
                        bool quantity = ((uint)response[(int)ERTUHeader.FUNCTION + 3] << 8 | response[(int)ERTUHeader.FUNCTION + 4]) == _valueToWrite.Length;

                        if (firstRegister && quantity)
                        {
                            return true;
                        }
                    }
                }

                throw new Exception("Failed to write status into coils FC 15 - ModbusTCP/IP");
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
                        _addressDevice,
                        _firstRegister,
                        _valuesToWrite);

                    byte[] response = ModbusConnection.SendRequest(buffer, sizeBufferExpected, TypeReqModbus);

                    if (response != null && IsValidRTU(buffer, response))
                    {
                        bool firstRegister = ((uint)response[(int)ERTUHeader.FUNCTION + 1] << 8 | response[(int)ERTUHeader.FUNCTION + 2]) == _firstRegister;
                        bool quantity = ((uint)response[(int)ERTUHeader.FUNCTION + 3] << 8 | response[(int)ERTUHeader.FUNCTION + 4]) == _valuesToWrite.Length;

                        if (firstRegister && quantity)
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
