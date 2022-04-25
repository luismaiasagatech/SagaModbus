

namespace SagaModbus.Interfaces
{
    public interface IModbusClient
    {

        /// <summary>
        /// Função 01. Requisita os estados das saídas digitais.
        /// 
        /// </summary>
        /// <param name="_addressDevice"></param>
        /// <param name="_firstRegister"></param>
        /// <param name="_quantityRegisters"></param>
        /// <returns></returns>
        bool[] SendReadStatusCoils(int _addressDevice, int _firstRegister, int _quantityRegisters);

        /// <summary>
        /// Função 02. Requisita os estados das entradas digitais.
        /// 
        /// </summary>
        /// <param name="_addressDevice"></param>
        /// <param name="_firstRegister"></param>
        /// <param name="_quantityRegisters"></param>
        /// <returns></returns>
        bool[] SendReadStatusDigitalStatus(int _addressDevice, int _firstRegister, int _quantityRegisters);

        /// <summary>
        /// Função 03. Requisita os conteúdos de saída retentivos analógicos.
        /// 
        /// </summary>
        /// <param name="_addressDevice"></param>
        /// <param name="_firstRegister"></param>
        /// <param name="_quantityRegisters"></param>
        /// <returns></returns>
        int[] SendReadHoldingRegisters(int _addressDevice, int _firstRegister, int _quantityRegisters);

        /// <summary>
        /// Função 04. Requisita os valores das entradas analógicas.
        /// 
        /// </summary>
        /// <param name="_addressDevice"></param>
        /// <param name="_firstRegister"></param>
        /// <param name="_quantityRegisters"></param>
        /// <returns></returns>
        int[] SendReadInputRegisters(int _addressDevice, int _firstRegister, int _quantityRegisters);


        /// <summary>
        /// Função 05. Escreve o status desejado para uma saída digital
        /// 
        /// </summary>
        /// <param name="_addressDevice"></param>
        /// <param name="_firstRegister"></param>
        /// <param name="_statusToWrite"></param>
        /// <returns></returns>
        bool SendForceSingleCoil(int _addressDevice, int _firstRegister, bool _statusToWrite);

        /// <summary>
        /// Função 06. Escreve o status desejado para um conteúdo de saída retentivo analógico.
        /// 
        /// </summary>
        /// <param name="_addressDevice"></param>
        /// <param name="_firstRegister"></param>
        /// <param name="_valueToWrite"></param>
        /// <returns></returns>
        int SendPresetSingleRegister(int _addressDevice, int _firstRegister, int _valueToWrite);

        /// <summary>
        /// Função 15. Escreve o status desejado para múltiplas saídas digitais.
        /// 
        /// </summary>
        /// <param name="_addressDevice"></param>
        /// <param name="_firstRegister"></param>
        /// <param name="_valueToWrite"></param>
        /// <returns></returns>
        bool SendForceMultipleCoils(int _addressDevice, int _firstRegister, bool[] _valueToWrite);

        /// <summary>
        /// Função 16. Escreve o status desejado para múltiplos registros de saída retentivos.
        /// 
        /// </summary>
        /// <param name="_addressDevice"></param>
        /// <param name="_firstRegister"></param>
        /// <param name="_valuesToWrite"></param>
        /// <returns></returns>
        bool SendPresetMultipleRegisters(int _addressDevice, int _firstRegister, int[] _valuesToWrite);
    }
}
