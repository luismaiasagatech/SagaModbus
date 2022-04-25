using SagaModbus.Interfaces;
using SagaModbus.Modbus;
using System;
using System.Threading;

namespace SagaModbus
{
    internal class ExampleModbus
    {
        private ModbusConnection modbusConn;
        private ETypeReqModbus typeRequestModbus = ETypeReqModbus.TCPIP;

        public void ToggleStatusSingleCoil()
        {
            while (true)
            {
                try
                {
                    if (modbusConn.StatusConnection() == false)
                    {
                        ModbusConnection modbusConn = new ModbusConnection("127.0.0.1", 502, 1000, 500, 1000);
                        modbusConn.OpenConnection();
                    }
                    else
                    {
                        IModbusClient modbusClient = ModbusClient.GetInstance(modbusConn, typeRequestModbus);

                        bool[] statusCoils = modbusClient.SendReadStatusCoils(0x01, 0x0000, 0x0001);
                        bool currentState = modbusClient.SendForceSingleCoil(0x01, 0x0000, !statusCoils[0]);
                    }
                }
                catch (Exception)
                {

                    throw;
                }

                Thread.Sleep(1000);
            }
        }
    }
}
