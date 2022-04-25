using System;
using System.Text;


namespace SagaModbus.Modbus
{
    internal enum RegisterOrder
    {
        LowHigh,
        HighLow
    }

    internal static class ConvertersModbus
    {
        //
        // Resumo:
        //     Converts two ModbusRegisters to Float - Example: EasyModbus.ModbusClient.ConvertRegistersToFloat(modbusClient.ReadHoldingRegisters(19,2))
        //
        // Parâmetros:
        //   registers:
        //     Two Register values received from Modbus
        //
        // Devoluções:
        //     Connected float value
        public static float ConvertRegistersToFloat(int[] registers)
        {
            if (registers.Length != 2)
            {
                throw new ArgumentException("Input Array length invalid - Array langth must be '2'");
            }

            int value = registers[1];
            int value2 = registers[0];
            byte[] bytes = BitConverter.GetBytes(value);
            byte[] bytes2 = BitConverter.GetBytes(value2);
            byte[] value3 = new byte[4]
            {
                bytes2[0],
                bytes2[1],
                bytes[0],
                bytes[1]
            };
            return BitConverter.ToSingle(value3, 0);
        }

        //
        // Resumo:
        //     Converts two ModbusRegisters to Float, Registers can by swapped
        //
        // Parâmetros:
        //   registers:
        //     Two Register values received from Modbus
        //
        //   registerOrder:
        //     Desired Word Order (Low Register first or High Register first
        //
        // Devoluções:
        //     Connected float value
        public static float ConvertRegistersToFloat(int[] registers, RegisterOrder registerOrder)
        {
            int[] registers2 = new int[2]
            {
                registers[0],
                registers[1]
            };
            if (registerOrder == RegisterOrder.HighLow)
            {
                registers2 = new int[2]
                {
                    registers[1],
                    registers[0]
                };
            }

            return ConvertRegistersToFloat(registers2);
        }

        //
        // Resumo:
        //     Converts two ModbusRegisters to 32 Bit Integer value
        //
        // Parâmetros:
        //   registers:
        //     Two Register values received from Modbus
        //
        // Devoluções:
        //     Connected 32 Bit Integer value
        public static int ConvertRegistersToInt(int[] registers)
        {
            if (registers.Length != 2)
            {
                throw new ArgumentException("Input Array length invalid - Array langth must be '2'");
            }

            int value = registers[1];
            int value2 = registers[0];
            byte[] bytes = BitConverter.GetBytes(value);
            byte[] bytes2 = BitConverter.GetBytes(value2);
            byte[] value3 = new byte[4]
            {
                bytes2[0],
                bytes2[1],
                bytes[0],
                bytes[1]
            };
            return BitConverter.ToInt32(value3, 0);
        }

        //
        // Resumo:
        //     Converts two ModbusRegisters to 32 Bit Integer Value - Registers can be swapped
        //
        // Parâmetros:
        //   registers:
        //     Two Register values received from Modbus
        //
        //   registerOrder:
        //     Desired Word Order (Low Register first or High Register first
        //
        // Devoluções:
        //     Connecteds 32 Bit Integer value
        public static int ConvertRegistersToInt(int[] registers, RegisterOrder registerOrder)
        {
            int[] registers2 = new int[2]
            {
                registers[0],
                registers[1]
            };
            if (registerOrder == RegisterOrder.HighLow)
            {
                registers2 = new int[2]
                {
                    registers[1],
                    registers[0]
                };
            }

            return ConvertRegistersToInt(registers2);
        }

        //
        // Resumo:
        //     Convert four 16 Bit Registers to 64 Bit Integer value Register Order "LowHigh":
        //     Reg0: Low Word.....Reg3: High Word, "HighLow": Reg0: High Word.....Reg3: Low
        //     Word
        //
        // Parâmetros:
        //   registers:
        //     four Register values received from Modbus
        //
        // Devoluções:
        //     64 bit value
        public static long ConvertRegistersToLong(int[] registers)
        {
            if (registers.Length != 4)
            {
                throw new ArgumentException("Input Array length invalid - Array langth must be '4'");
            }

            int value = registers[3];
            int value2 = registers[2];
            int value3 = registers[1];
            int value4 = registers[0];
            byte[] bytes = BitConverter.GetBytes(value);
            byte[] bytes2 = BitConverter.GetBytes(value2);
            byte[] bytes3 = BitConverter.GetBytes(value3);
            byte[] bytes4 = BitConverter.GetBytes(value4);
            byte[] value5 = new byte[8]
            {
                bytes4[0],
                bytes4[1],
                bytes3[0],
                bytes3[1],
                bytes2[0],
                bytes2[1],
                bytes[0],
                bytes[1]
            };
            return BitConverter.ToInt64(value5, 0);
        }

        //
        // Resumo:
        //     Convert four 16 Bit Registers to 64 Bit Integer value - Registers can be swapped
        //
        // Parâmetros:
        //   registers:
        //     four Register values received from Modbus
        //
        //   registerOrder:
        //     Desired Word Order (Low Register first or High Register first
        //
        // Devoluções:
        //     Connected 64 Bit Integer value
        public static long ConvertRegistersToLong(int[] registers, RegisterOrder registerOrder)
        {
            if (registers.Length != 4)
            {
                throw new ArgumentException("Input Array length invalid - Array langth must be '4'");
            }

            int[] registers2 = new int[4]
            {
                registers[0],
                registers[1],
                registers[2],
                registers[3]
            };
            if (registerOrder == RegisterOrder.HighLow)
            {
                registers2 = new int[4]
                {
                    registers[3],
                    registers[2],
                    registers[1],
                    registers[0]
                };
            }

            return ConvertRegistersToLong(registers2);
        }

        //
        // Resumo:
        //     Convert four 16 Bit Registers to 64 Bit double prec. value Register Order "LowHigh":
        //     Reg0: Low Word.....Reg3: High Word, "HighLow": Reg0: High Word.....Reg3: Low
        //     Word
        //
        // Parâmetros:
        //   registers:
        //     four Register values received from Modbus
        //
        // Devoluções:
        //     64 bit value
        public static double ConvertRegistersToDouble(int[] registers)
        {
            if (registers.Length != 4)
            {
                throw new ArgumentException("Input Array length invalid - Array langth must be '4'");
            }

            int value = registers[3];
            int value2 = registers[2];
            int value3 = registers[1];
            int value4 = registers[0];
            byte[] bytes = BitConverter.GetBytes(value);
            byte[] bytes2 = BitConverter.GetBytes(value2);
            byte[] bytes3 = BitConverter.GetBytes(value3);
            byte[] bytes4 = BitConverter.GetBytes(value4);
            byte[] value5 = new byte[8]
            {
                bytes4[0],
                bytes4[1],
                bytes3[0],
                bytes3[1],
                bytes2[0],
                bytes2[1],
                bytes[0],
                bytes[1]
            };
            return BitConverter.ToDouble(value5, 0);
        }

        //
        // Resumo:
        //     Convert four 16 Bit Registers to 64 Bit double prec. value - Registers can be
        //     swapped
        //
        // Parâmetros:
        //   registers:
        //     four Register values received from Modbus
        //
        //   registerOrder:
        //     Desired Word Order (Low Register first or High Register first
        //
        // Devoluções:
        //     Connected double prec. float value
        public static double ConvertRegistersToDouble(int[] registers, RegisterOrder registerOrder)
        {
            if (registers.Length != 4)
            {
                throw new ArgumentException("Input Array length invalid - Array langth must be '4'");
            }

            int[] registers2 = new int[4]
            {
                registers[0],
                registers[1],
                registers[2],
                registers[3]
            };
            if (registerOrder == RegisterOrder.HighLow)
            {
                registers2 = new int[4]
                {
                    registers[3],
                    registers[2],
                    registers[1],
                    registers[0]
                };
            }

            return ConvertRegistersToDouble(registers2);
        }

        //
        // Resumo:
        //     Converts float to two ModbusRegisters - Example: modbusClient.WriteMultipleRegisters(24,
        //     EasyModbus.ModbusClient.ConvertFloatToTwoRegisters((float)1.22));
        //
        // Parâmetros:
        //   floatValue:
        //     Float value which has to be converted into two registers
        //
        // Devoluções:
        //     Register values
        public static int[] ConvertFloatToRegisters(float floatValue)
        {
            byte[] bytes = BitConverter.GetBytes(floatValue);
            byte[] value = new byte[4]
            {
                bytes[2],
                bytes[3],
                0,
                0
            };
            byte[] value2 = new byte[4]
            {
                bytes[0],
                bytes[1],
                0,
                0
            };
            return new int[2]
            {
                BitConverter.ToInt32(value2, 0),
                BitConverter.ToInt32(value, 0)
            };
        }

        //
        // Resumo:
        //     Converts float to two ModbusRegisters Registers - Registers can be swapped
        //
        // Parâmetros:
        //   floatValue:
        //     Float value which has to be converted into two registers
        //
        //   registerOrder:
        //     Desired Word Order (Low Register first or High Register first
        //
        // Devoluções:
        //     Register values
        public static int[] ConvertFloatToRegisters(float floatValue, RegisterOrder registerOrder)
        {
            int[] array = ConvertFloatToRegisters(floatValue);
            int[] result = array;
            if (registerOrder == RegisterOrder.HighLow)
            {
                result = new int[2]
                {
                    array[1],
                    array[0]
                };
            }

            return result;
        }

        //
        // Resumo:
        //     Converts 32 Bit Value to two ModbusRegisters
        //
        // Parâmetros:
        //   intValue:
        //     Int value which has to be converted into two registers
        //
        // Devoluções:
        //     Register values
        public static int[] ConvertIntToRegisters(int intValue)
        {
            byte[] bytes = BitConverter.GetBytes(intValue);
            byte[] value = new byte[4]
            {
                bytes[2],
                bytes[3],
                0,
                0
            };
            byte[] value2 = new byte[4]
            {
                bytes[0],
                bytes[1],
                0,
                0
            };
            return new int[2]
            {
                BitConverter.ToInt32(value2, 0),
                BitConverter.ToInt32(value, 0)
            };
        }

        //
        // Resumo:
        //     Converts 32 Bit Value to two ModbusRegisters Registers - Registers can be swapped
        //
        // Parâmetros:
        //   intValue:
        //     Double value which has to be converted into two registers
        //
        //   registerOrder:
        //     Desired Word Order (Low Register first or High Register first
        //
        // Devoluções:
        //     Register values
        public static int[] ConvertIntToRegisters(int intValue, RegisterOrder registerOrder)
        {
            int[] array = ConvertIntToRegisters(intValue);
            int[] result = array;
            if (registerOrder == RegisterOrder.HighLow)
            {
                result = new int[2]
                {
                    array[1],
                    array[0]
                };
            }

            return result;
        }

        //
        // Resumo:
        //     Converts 64 Bit Value to four ModbusRegisters
        //
        // Parâmetros:
        //   longValue:
        //     long value which has to be converted into four registers
        //
        // Devoluções:
        //     Register values
        public static int[] ConvertLongToRegisters(long longValue)
        {
            byte[] bytes = BitConverter.GetBytes(longValue);
            byte[] value = new byte[4]
            {
                bytes[6],
                bytes[7],
                0,
                0
            };
            byte[] value2 = new byte[4]
            {
                bytes[4],
                bytes[5],
                0,
                0
            };
            byte[] value3 = new byte[4]
            {
                bytes[2],
                bytes[3],
                0,
                0
            };
            byte[] value4 = new byte[4]
            {
                bytes[0],
                bytes[1],
                0,
                0
            };
            return new int[4]
            {
                BitConverter.ToInt32(value4, 0),
                BitConverter.ToInt32(value3, 0),
                BitConverter.ToInt32(value2, 0),
                BitConverter.ToInt32(value, 0)
            };
        }

        //
        // Resumo:
        //     Converts 64 Bit Value to four ModbusRegisters - Registers can be swapped
        //
        // Parâmetros:
        //   longValue:
        //     long value which has to be converted into four registers
        //
        //   registerOrder:
        //     Desired Word Order (Low Register first or High Register first
        //
        // Devoluções:
        //     Register values
        public static int[] ConvertLongToRegisters(long longValue, RegisterOrder registerOrder)
        {
            int[] array = ConvertLongToRegisters(longValue);
            int[] result = array;
            if (registerOrder == RegisterOrder.HighLow)
            {
                result = new int[4]
                {
                    array[3],
                    array[2],
                    array[1],
                    array[0]
                };
            }

            return result;
        }

        //
        // Resumo:
        //     Converts 64 Bit double prec Value to four ModbusRegisters
        //
        // Parâmetros:
        //   doubleValue:
        //     double value which has to be converted into four registers
        //
        // Devoluções:
        //     Register values
        public static int[] ConvertDoubleToRegisters(double doubleValue)
        {
            byte[] bytes = BitConverter.GetBytes(doubleValue);
            byte[] value = new byte[4]
            {
                bytes[6],
                bytes[7],
                0,
                0
            };
            byte[] value2 = new byte[4]
            {
                bytes[4],
                bytes[5],
                0,
                0
            };
            byte[] value3 = new byte[4]
            {
                bytes[2],
                bytes[3],
                0,
                0
            };
            byte[] value4 = new byte[4]
            {
                bytes[0],
                bytes[1],
                0,
                0
            };
            return new int[4]
            {
                BitConverter.ToInt32(value4, 0),
                BitConverter.ToInt32(value3, 0),
                BitConverter.ToInt32(value2, 0),
                BitConverter.ToInt32(value, 0)
            };
        }

        //
        // Resumo:
        //     Converts 64 Bit double prec. Value to four ModbusRegisters - Registers can be
        //     swapped
        //
        // Parâmetros:
        //   doubleValue:
        //     double value which has to be converted into four registers
        //
        //   registerOrder:
        //     Desired Word Order (Low Register first or High Register first
        //
        // Devoluções:
        //     Register values
        public static int[] ConvertDoubleToRegisters(double doubleValue, RegisterOrder registerOrder)
        {
            int[] array = ConvertDoubleToRegisters(doubleValue);
            int[] result = array;
            if (registerOrder == RegisterOrder.HighLow)
            {
                result = new int[4]
                {
                    array[3],
                    array[2],
                    array[1],
                    array[0]
                };
            }

            return result;
        }

        //
        // Resumo:
        //     Converts 16 - Bit Register values to String
        //
        // Parâmetros:
        //   registers:
        //     Register array received via Modbus
        //
        //   offset:
        //     First Register containing the String to convert
        //
        //   stringLength:
        //     number of characters in String (must be even)
        //
        // Devoluções:
        //     Converted String
        public static string ConvertRegistersToString(int[] registers, int offset, int stringLength)
        {
            byte[] array = new byte[stringLength];
            byte[] array2 = new byte[2];
            checked
            {
                for (int i = 0; i < unchecked(stringLength / 2); i++)
                {
                    array2 = BitConverter.GetBytes(registers[offset + i]);
                    array[i * 2] = array2[0];
                    array[i * 2 + 1] = array2[1];
                }

                return Encoding.Default.GetString(array);
            }
        }

        //
        // Resumo:
        //     Converts a String to 16 - Bit Registers
        //
        // Parâmetros:
        //   registers:
        //     Register array received via Modbus
        //
        // Devoluções:
        //     Converted String
        public static int[] ConvertStringToRegisters(string stringToConvert)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(stringToConvert);
            checked
            {
                int[] array = new int[unchecked(stringToConvert.Length / 2) + unchecked(stringToConvert.Length % 2)];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = bytes[i * 2];
                    if (i * 2 + 1 < bytes.Length)
                    {
                        array[i] |= bytes[i * 2 + 1] << 8;
                    }
                }

                return array;
            }
        }

    }
}
