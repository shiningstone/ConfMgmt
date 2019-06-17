using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace Utils
{
    public class ComPort
    {
        public static int FailCommandRepeat = 1;
        //share SerialPort with CommandPort
        public static Dictionary<string, object> lockDict = new Dictionary<string, object>();

        private Logger _log = new Logger("ComPort");
        private readonly SerialPort _serialPort;
        private bool IsRecv { get; set; }
        public string Name;

        static ComPort()
        {
            try
            {
                FailCommandRepeat = Int32.Parse(ConfigurationManager.AppSettings["FailCommandRepeat"]);
            }
            catch (Exception ex)
            {
                new Logger("ComPort").Warn("Failed to read FailCommandRepeat", ex);
            }
        }
        public ComPort(string name, int baudrate = 19200, StopBits stopBits = StopBits.One, Parity parity = Parity.None)
        {
            Name = name;
#if VIRT_COM
            Name = "Virt" + Name;
#endif

            if (!Name.Contains("Virt"))
            {
                _serialPort = new SerialPort
                {
                    PortName = name,
                    BaudRate = baudrate,
                    DataBits = 8,
                    StopBits = stopBits,
                    Parity = parity,
                    ReadTimeout = 5000,
                };
                _serialPort.DataReceived += ComDevice_DataReceived;
            }

            if (false == lockDict.ContainsKey(Name))
            {
                lockDict[Name] = new object();
            }
        }
        public void Config(int baudrate = 19200, StopBits stopBits = StopBits.One, Parity parity = Parity.None)
        {
            if (_serialPort != null)
            {
                lock (lockDict[Name])
                {
                    if (_serialPort.BaudRate != baudrate || _serialPort.StopBits != stopBits || _serialPort.Parity != parity)
                    {
                        _serialPort.BaudRate = baudrate;
                        _serialPort.StopBits = stopBits;
                        _serialPort.Parity = parity;

                        _log.Info($"{Name} Set BaudRate({baudrate}), StopBits({stopBits}), Parity({parity})");
                    }
                }
            }
        }

        public void ComDevice_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            IsRecv = true;
        }
        public byte[] Query(byte[] cmd, int sleep = 0)
        {
            return Query(cmd, 0, sleep);
        }
        public byte[] Read()
        {
            return Query(new byte[] { }, 0, 500);
        }
        public string Query(string cmd, int sleepMs)
        {
            lock (lockDict[Name])
            {
                try
                {
                    if (_serialPort != null && !_serialPort.IsOpen)
                    {
                        _serialPort?.Open();
                    }

                    _log.Debug($"{Name} Send: {cmd}");
                    Reset();
                    _serialPort?.WriteLine(cmd);

                    Thread.Sleep(sleepMs);

                    List<int> buf = new List<int>();
                    while (_serialPort.BytesToRead > 0)
                    {
                        buf.Add(_serialPort.ReadByte());
                    }
                    string ret = System.Text.Encoding.ASCII.GetString(buf.Select(x => (byte)x).ToArray());
                    _log.Debug($"{Name} Recv: {ret}");

                    return ret;
                }
                catch (Exception e)
                {
                    _log.Error(Name + "failed to query", e);
                    return "N/A";
                }
                finally
                {
                    _serialPort?.Close();
                }
            }
        }
        /* NOTE : this interface only applied to the command system which the length of response is determined */
        public byte[] Query(byte[] cmd, int responseMsgLength = 0, int waitTransferDone = 200, int timeOut = 5000)
        {
            lock (lockDict[Name])
            {
                for (var tryNo = 0; tryNo < FailCommandRepeat; tryNo++)
                {
                    byte[] result;
                    if (Send(cmd, responseMsgLength, out result, waitTransferDone, timeOut))
                    {
                        return result;
                    }
                }
            }

            return new byte[0];
        }

        private void Reset()
        {
            _serialPort?.DiscardOutBuffer();
            _serialPort?.DiscardInBuffer();
            IsRecv = false;
        }
        private bool Send(byte[] cmd, int responseMsgLength, out byte[] result, int waitTransferDone, int timeOut)
        {
            result = new byte[0];

            try
            {
                if (_serialPort!=null && !_serialPort.IsOpen)
                {
                    _serialPort?.Open();
                }
                
                _log.Debug(Name + " Send(" + cmd.Length + "): " + Calc.ToHexString(cmd));
                Reset();
                _serialPort?.Write(cmd, 0, cmd.Length);

                var recvFlag = WaitResponse(responseMsgLength, out result, waitTransferDone, timeOut);
                _log.Debug(Name + " Recv(" + result.Length + "): " + Calc.ToHexString(result));

                return recvFlag;
            }
            catch (Exception e)
            {
                _log.Error(Name + "failed to query", e);
                return false;
            }
            finally
            {
                _serialPort?.Close();
            }
        }

        private bool WaitResponse(int responseMsgLength, out byte[] result, int waitTransferDone, int timeOut)
        {
            if (_serialPort == null)
            {
                result = new byte[0];
                return true;
            }

            const int waitStepMs = 20;
            var silentTime = 0;
            var buffer = new List<byte>();

            do
            {
                Thread.Sleep(waitStepMs);

                if (IsRecv && _serialPort.BytesToRead > 0)
                {
                    if (CompleteRead(responseMsgLength, waitTransferDone, buffer))
                    {
                        result = buffer.ToArray();
                        return true;
                    }

                    continue;
                }

                silentTime += waitStepMs;
                if (silentTime < timeOut) continue;

                result = buffer.ToArray();
                _log.Error($"{Name} receive timeout({timeOut}ms)");
                return false;
            } while (true);
        }

        private bool CompleteRead(int responseMsgLength, int waitTransferDone, List<byte> buffer)
        {
            if (responseMsgLength == 0)
            {
                Thread.Sleep(waitTransferDone);
            }

            var reDatas = new byte[_serialPort.BytesToRead];
            _serialPort.Read(reDatas, 0, reDatas.Length);
            buffer.AddRange(reDatas);

            return (responseMsgLength == 0) ||
                (responseMsgLength > 0 && buffer.Count >= responseMsgLength);
        }
    }
}
