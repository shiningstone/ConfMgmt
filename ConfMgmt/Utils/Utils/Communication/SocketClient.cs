using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Linq;

namespace Utils.Communication
{
    public class SocketClient
    {
        public Action<byte[]> OnReceived;
        public Action OnConnected;
        public Action OnDisconnected;

        public int BuffSize = 2048;

        private readonly string Ip;
        private readonly int Port;
        public SocketClient(string strIp, int port)
        {
            Ip = strIp;
            Port = port;
        }

        private Logger _log = new Logger("SocketClient");
        bool IsConnected;
        Socket _socket;
        Thread _recvThread;

        public ErrInfo Connect()
        {
            try
            {
                Connect(Ip, Port);

                _recvThread = new Thread(new ThreadStart(Receive));
                _recvThread.IsBackground = true;
                _recvThread.Name = "SocketClient.Recv";
                _recvThread.Start();

                return ErrInfo.Ok;
            }
            catch (Exception ex)
            {
                _log.Debug($"连接失败({Ip}:{Port}): {ex}");
                return new ErrInfo(ErrCode.Fail, $"连接失败({Ip}:{Port})");
            }
        }

        private void Connect(string ip, int port)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(IPAddress.Parse(ip), Port);
            _log.Debug($"连接({ip}:{Port})成功");

            IsConnected = true;
            OnConnected?.Invoke();
        }

        private void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[BuffSize];
                    int r = _socket.Receive(buffer);
                    if (r == 0)
                    {
                        break;
                    }
                    else
                    {
                        var length = buffer.ToList().FindIndex(x => x == 0);
                        var msg = new byte[length];
                        Array.Copy(buffer, msg, length);
                        _log.Debug($"Receive({Port}) {string.Join(" ", msg.Select(x => $"{x:X2}"))}");
                        OnReceived?.Invoke(msg);
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("接收服务端发送的消息出错:" + ex.ToString());

                IsConnected = false;
                _socket = null;
                _recvThread = null;
                OnDisconnected?.Invoke();
            }
        }

        private ErrInfo _Send(byte[] buff)
        {
            if (!IsConnected)
            {
                Connect(Ip, Port);
            }

            _log.Debug($"Send({Port}): {string.Join(" ", buff.ToList().Select(x => $"{x:X2}"))}");
            int send = _socket.Send(buff);
            return new ErrInfo(ErrCode.Ok);
        }

        public ErrInfo Send(byte[] buff)
        {
            for (int i = 0; i < 1; i++)
            {
                try
                {
                    return _Send(buff);
                }
                catch (Exception ex)
                {
                    _log.Warn($"发送消息出错({i + 1}):" + ex.Message);
                    IsConnected = false;
                }
            }

            return new ErrInfo(ErrCode.Fail, $"");
        }

        public void Close(object sender, EventArgs e)
        {
            _socket.Close();
            _recvThread.Abort();
        }
    }
}