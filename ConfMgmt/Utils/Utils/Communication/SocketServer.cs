using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Linq;

namespace Utils.Communication
{
    public partial class SocketServer
    {
        public Action<string> OnConnected;
        public Action<byte[]> OnReceived;

        public int BuffSize = 2048;

        private Logger _log = new Logger("SocketServer");
        private readonly string ServerIp = "127.0.0.1";
        private readonly int ServerPort = 10000;
        private readonly int MaxConnection = 10;

        public SocketServer(string ip, int port, int allowConnection = 10)
        {
            ServerIp = ip;
            ServerPort = port;
            MaxConnection = allowConnection;
        }

        Socket _listenSocket;
        Thread _listenThread;
        Thread _recvThread;

        class Connection
        {
            public Socket socket;
            public Thread thread;
            public void Close()
            {
                socket.Close();
                thread.Abort();
            }
        }
        Dictionary<string, Connection> _connections = new Dictionary<string, Connection>();

        public void Initialize()
        {
            try
            {
                _listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ip = IPAddress.Parse(ServerIp);
                IPEndPoint point = new IPEndPoint(ip, ServerPort);
                _listenSocket.Bind(point);
                _listenSocket.Listen(MaxConnection);
                _log.Debug($"Bind({point})");
            }
            catch (Exception ex)
            {
                _log.Error($"Initialize({ServerIp}:{ServerPort}) failed", ex);
                throw new Exception($"SocketServer initialize failed", ex);
            }

            //创建listen线程
            _listenThread = new Thread(new ParameterizedThreadStart(StartListen));
            _listenThread.IsBackground = true;
            _listenThread.Name = "SocketServer.Listen";
            _listenThread.Start(_listenSocket);
        }
        private void StartListen(object obj)
        {
            Socket socketWatch = obj as Socket;
            while (true)
            {
                var connection = socketWatch.Accept();

                string ip = connection.RemoteEndPoint.ToString();
                if (_connections.ContainsKey(ip))
                {
                    _log.Warn($"重复连接({ip})");

                    _connections[ip].Close();
                    _connections.Remove(ip);
                }

                _log.Debug($"Accept({ip})");
                OnConnected?.Invoke(ip);

                _recvThread = new Thread(new ParameterizedThreadStart(Receive));
                _recvThread.IsBackground = true;
                _recvThread.Name = "Sockect.Recv";
                _recvThread.Start(connection);

                _connections[ip] = new Connection() {
                    socket = connection,
                    thread = _recvThread,
                };
            }
        }
        private void Receive(object obj)
        {
            Socket socketSend = obj as Socket;
            try
            {
                while (true)
                {
                    byte[] buffer = new byte[BuffSize];
                    int count = socketSend.Receive(buffer);
                    if (count == 0)//count 表示客户端关闭，要退出循环
                    {
                        _log.Debug($"Receive({ServerPort}) count 0");
                        break;
                    }
                    else
                    {
                        var length = buffer.ToList().FindIndex(x => x == 0);
                        var msg = new byte[length];
                        Array.Copy(buffer, msg, length);
                        _log.Debug($"Receive({ServerPort}) {string.Join(" ", msg.Select(x => $"{x:X2}"))}");

                        try
                        {
                            OnReceived?.Invoke(msg);
                        }
                        catch (Exception ex)
                        {
                            _log.Warn($"Failed to handle {string.Join(" ", msg.Select(x => $"{x:X2}"))}", ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Warn($"This should only be seen during program exit", ex);
                _recvThread = null;
            }
        }

        public ErrInfo Send(byte[] bytes)
        {
            try
            {
                foreach (var ip in _connections.Keys)
                {
                    _log.Debug($"Send({ip}){Environment.NewLine}");
                    _connections[ip].socket.Send(bytes);
                }

                return ErrInfo.Ok;
            }
            catch (Exception ex)
            {
                string errInfo = $"Send failed: {Environment.NewLine}{string.Join(" ", bytes.Select(x => $"0x{x:X2}"))}{Environment.NewLine}{ex}";
                _log.Error(errInfo);
                return new ErrInfo(ErrCode.Fail, $"Send failed");
            }
        }

        public void Close()
        {
            _listenSocket?.Close();
            _listenThread?.Abort();

            foreach (var connection in _connections.Values)
            {
                connection?.Close();
            }
        }
    }
}