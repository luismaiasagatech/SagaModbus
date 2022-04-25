using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;

namespace SagaModbus.Modbus
{
    public class ModbusConnection
    {

        private static volatile int TransactionID = 0;

        private static volatile object ReadWriteLock = new object();
        private int PortService { get; set; } = 10000;
        private IPAddress IpAddressDevice { get; set; } = IPAddress.None;
        public int TimeOutConnection { get; set; } = 500;
        public int TimeOutSend { get; set; } = 500;
        public int TimeOutReceive { get; set; } = 500;

        private TcpClient TcpClientService { get; set; } = new TcpClient();
        private NetworkStream NetworkStreamService { get; set; } = null;

        public ModbusConnection(string _ipAddressDevice, int _portService, int _timeOutConnection = 500, int _timeOutSend = 500, int _timeOutReceive = 500)
        {
            var (matched, ipAddressDevice) = IsValidIpAddress(_ipAddressDevice);
            try
            {
                if (matched)
                {
                    IpAddressDevice = IPAddress.Parse(ipAddressDevice);
                }
                else
                {
                    this.IpAddressDevice = Dns.GetHostEntry(_ipAddressDevice).AddressList[0];
                }

                if (_timeOutConnection < 300 || _timeOutConnection > 5000)
                {
                    throw new Exception("Timeout value should is between 300 and 5000 ms.");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }


            TimeOutSend = _timeOutSend;
            TimeOutReceive = _timeOutReceive;
            TimeOutConnection = _timeOutConnection;

            this.PortService = _portService;
        }


        private Tuple<bool, string> IsValidIpAddress(string _ipAddressDevice)
        {
            Match mathRegex = Regex.Match(_ipAddressDevice, @"^\d{1,3}.\d{1,3}.\d{1,3}.\d{1,3}$");

            return new Tuple<bool, string>(mathRegex.Success, mathRegex.Success ? mathRegex.Value : "");
        }

        public bool OpenConnection()
        {
            try
            {
                if (TcpClientService.Connected)
                {
                    CloseConnection();
                }

                //Permite apenas que um cliente use uma porta específica. Deve ser definido antes de tentar se conectar a um hospedeiro.
                TcpClientService.ExclusiveAddressUse = true;

                TcpClientService.SendBufferSize = 8192;
                TcpClientService.ReceiveBufferSize = 8192;

                //Determina a quantidade de tempo que o método Read bloqueará até que possa receber dados.
                TcpClientService.SendTimeout = TimeOutSend;
                TcpClientService.ReceiveTimeout = TimeOutReceive;

                //TcpClientService.LingerState = new LingerOption(true, 1);

                if (TcpClientService.ConnectAsync(IpAddressDevice, this.PortService).Wait(TimeOutConnection))
                {
                    if (TcpClientService.Connected)
                    {
                        NetworkStreamService = TcpClientService.GetStream();
                        return true;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Open connection: {e.Message}");
                return false;
            }
        }

        internal int GetTransactionID()
        {
            return TransactionID;
        }

        public bool StatusConnection()
        {
            if (TcpClientService != null)
            {
                return TcpClientService.Connected;
            }

            return false;
        }

        public void CloseConnection()
        {
            try
            {
                if (TcpClientService != null)
                {
                    if (NetworkStreamService != null)
                    {
                        NetworkStreamService.Close(300);
                        NetworkStreamService.Dispose();
                    }

                    TcpClientService.Close();
                    TcpClientService.Dispose();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Close connection: {e.Message}");
            }

        }

        internal byte[] SendRequest(byte[] _requestToSend, int _sizeBufferExpected, ETypeReqModbus _typeReqModbus)
        {
            //int quantityAttempts = 0;
            byte[] response = new byte[_sizeBufferExpected];

            lock (ReadWriteLock)
            {
                try
                {
                    using (NetworkStreamService = new NetworkStream(TcpClientService.Client))
                    {

                        if (NetworkStreamService.CanWrite)
                        {
                            NetworkStreamService.Write(_requestToSend, 0, _requestToSend.Length);

                            NetworkStreamService.Flush();

                            if (_typeReqModbus == ETypeReqModbus.TCPIP)
                            {
                                TransactionID = ++TransactionID > 0xFFFF ? 0 : TransactionID;
                            }
                        }

                        Thread.Sleep(5);

                        if (NetworkStreamService.CanRead)
                        {
                            if (NetworkStreamService.Read(response, 0, _sizeBufferExpected) == _sizeBufferExpected)
                            {
                                return response;
                            }
                        }

                    }

                    return null;
                }

                catch (IOException ioException)
                {
                    if (ioException.InnerException.GetType() == typeof(SocketException))
                    {
                        var error = (SocketException)ioException.InnerException;

                        //Perda de conexão ou não está conectado
                        //error.SocketErrorCode == SocketError.Shutdown
                        if (error.SocketErrorCode == SocketError.NotConnected)
                        {
                            OpenConnection();
                        }
                        //Uma chamada de bloqueio está em uso, mesmo quando se usa Threads em paralelo
                        else if (error.SocketErrorCode == SocketError.InProgress)
                        {
                            Thread.Sleep(100);
                        }
                    }

                    return null;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Write and read: {e.Message}");
                    return null;
                }
            }
        }
    }
}
