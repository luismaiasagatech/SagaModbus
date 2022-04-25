
using SagaModbus.Interfaces;

namespace SagaModbus.Modbus
{

    public static class ModbusClient
    {
        public static int QuantityAttempts = 3;
        public static IModbusClient GetInstance(ModbusConnection _modbusConnection, ETypeReqModbus _typeReqModbus)
        {

            switch (_typeReqModbus)
            {
                case ETypeReqModbus.TCPIP:
                    {
                        return new ModbusTCPIP(_modbusConnection);
                    };

                case ETypeReqModbus.RTU:
                    {
                        return new ModbusRTU(_modbusConnection);
                    };

                default:
                    return new ModbusRTU(_modbusConnection);
            }
        }
    }
}
