//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace GameFramework.Network
{
    internal partial class NetworkManager
    {
        /// <summary>
        /// 网络频道。
        /// </summary>
        private sealed partial class NetworkChannel : INetworkChannel, IDisposable
        {
            private const int DefaultPacketHeaderLength = 4;
            private const int DefaultMaxPacketLength = 1024 * 32;
            private const float DefaultHeartBeatInterval = 30f;

            private readonly string m_Name;
            private readonly INetworkHelper m_NetworkHelper;
            private NetworkType m_NetworkType;
            private int m_PacketHeaderLength;
            private int m_MaxPacketLength;
            private bool m_ResetHeartBeatElapseSecondsWhenReceivePacket;
            private float m_HeartBeatInterval;
            private Socket m_Socket;
            private ReceiveState m_ReceiveState;
            private readonly HeartBeatState m_HeartBeatState;
            private bool m_Active;
            private bool m_Disposed;

            public GameFrameworkAction<NetworkChannel, object> NetworkChannelConnected;
            public GameFrameworkAction<NetworkChannel> NetworkChannelClosed;
            public GameFrameworkAction<NetworkChannel, int, object> NetworkChannelSended;
            public GameFrameworkAction<NetworkChannel, Packet> NetworkChannelReceived;
            public GameFrameworkAction<NetworkChannel, int> NetworkChannelMissHeartBeat;
            public GameFrameworkAction<NetworkChannel, NetworkErrorCode, string> NetworkChannelError;
            public GameFrameworkAction<NetworkChannel, object> NetworkChannelCustomError;

            /// <summary>
            /// 初始化网络频道的新实例。
            /// </summary>
            /// <param name="name">网络频道名称。</param>
            /// <param name="networkHelper">网络辅助器。</param>
            public NetworkChannel(string name, INetworkHelper networkHelper)
            {
                m_Name = name ?? string.Empty;
                m_NetworkHelper = networkHelper;
                m_NetworkType = NetworkType.Unknown;
                m_PacketHeaderLength = DefaultPacketHeaderLength;
                m_MaxPacketLength = DefaultMaxPacketLength;
                m_ResetHeartBeatElapseSecondsWhenReceivePacket = false;
                m_HeartBeatInterval = DefaultHeartBeatInterval;
                m_Socket = null;
                m_ReceiveState = null;
                m_HeartBeatState = new HeartBeatState();
                m_Active = false;
                m_Disposed = false;

                NetworkChannelConnected = null;
                NetworkChannelClosed = null;
                NetworkChannelSended = null;
                NetworkChannelReceived = null;
                NetworkChannelMissHeartBeat = null;
                NetworkChannelError = null;
                NetworkChannelCustomError = null;
            }

            /// <summary>
            /// 获取网络频道名称。
            /// </summary>
            public string Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// 获取是否已连接。
            /// </summary>
            public bool Connected
            {
                get
                {
                    if (m_Socket != null)
                    {
                        return m_Socket.Connected;
                    }

                    return false;
                }
            }

            /// <summary>
            /// 获取网络类型。
            /// </summary>
            public NetworkType NetworkType
            {
                get
                {
                    return m_NetworkType;
                }
            }

            /// <summary>
            /// 获取本地终结点的 IP 地址。
            /// </summary>
            public IPAddress LocalIPAddress
            {
                get
                {
                    if (m_Socket == null)
                    {
                        throw new GameFrameworkException("You must initialize network channel first.");
                    }

                    IPEndPoint ipEndPoint = (IPEndPoint)m_Socket.LocalEndPoint;
                    if (ipEndPoint == null)
                    {
                        throw new GameFrameworkException("Local end point is invalid.");
                    }

                    return ipEndPoint.Address;
                }
            }

            /// <summary>
            /// 获取本地终结点的端口号。
            /// </summary>
            public int LocalPort
            {
                get
                {
                    if (m_Socket == null)
                    {
                        throw new GameFrameworkException("You must initialize network channel first.");
                    }

                    IPEndPoint ipEndPoint = (IPEndPoint)m_Socket.LocalEndPoint;
                    if (ipEndPoint == null)
                    {
                        throw new GameFrameworkException("Local end point is invalid.");
                    }

                    return ipEndPoint.Port;
                }
            }

            /// <summary>
            /// 获取远程终结点的 IP 地址。
            /// </summary>
            public IPAddress RemoteIPAddress
            {
                get
                {
                    if (m_Socket == null)
                    {
                        throw new GameFrameworkException("You must initialize network channel first.");
                    }

                    IPEndPoint ipEndPoint = (IPEndPoint)m_Socket.RemoteEndPoint;
                    if (ipEndPoint == null)
                    {
                        throw new GameFrameworkException("Remote end point is invalid.");
                    }

                    return ipEndPoint.Address;
                }
            }

            /// <summary>
            /// 获取远程终结点的端口号。
            /// </summary>
            public int RemotePort
            {
                get
                {
                    if (m_Socket == null)
                    {
                        throw new GameFrameworkException("You must initialize network channel first.");
                    }

                    IPEndPoint ipEndPoint = (IPEndPoint)m_Socket.RemoteEndPoint;
                    if (ipEndPoint == null)
                    {
                        throw new GameFrameworkException("Remote end point is invalid.");
                    }

                    return ipEndPoint.Port;
                }
            }

            /// <summary>
            /// 获取数据包头长度。
            /// </summary>
            public int PacketHeaderLength
            {
                get
                {
                    return m_PacketHeaderLength;
                }
            }

            /// <summary>
            /// 获取数据包最大字节数。
            /// </summary>
            public int MaxPacketLength
            {
                get
                {
                    return m_MaxPacketLength;
                }
            }

            /// <summary>
            /// 获取或设置当收到消息包时是否重置心跳流逝时间。
            /// </summary>
            public bool ResetHeartBeatElapseSecondsWhenReceivePacket
            {
                get
                {
                    return m_ResetHeartBeatElapseSecondsWhenReceivePacket;
                }
                set
                {
                    m_ResetHeartBeatElapseSecondsWhenReceivePacket = value;
                }
            }

            /// <summary>
            /// 获取或设置心跳间隔时长，以秒为单位。
            /// </summary>
            public float HeartBeatInterval
            {
                get
                {
                    return m_HeartBeatInterval;
                }
                set
                {
                    m_HeartBeatInterval = value;
                }
            }

            /// <summary>
            /// 获取或设置接收缓冲区字节数。
            /// </summary>
            public int ReceiveBufferSize
            {
                get
                {
                    if (m_Socket == null)
                    {
                        throw new GameFrameworkException("You must initialize network channel first.");
                    }

                    return m_Socket.ReceiveBufferSize;
                }
                set
                {
                    if (m_Socket == null)
                    {
                        throw new GameFrameworkException("You must initialize network channel first.");
                    }

                    m_Socket.ReceiveBufferSize = value;
                }
            }

            /// <summary>
            /// 获取或设置发送缓冲区字节数。
            /// </summary>
            public int SendBufferSize
            {
                get
                {
                    if (m_Socket == null)
                    {
                        throw new GameFrameworkException("You must initialize network channel first.");
                    }

                    return m_Socket.SendBufferSize;
                }
                set
                {
                    if (m_Socket == null)
                    {
                        throw new GameFrameworkException("You must initialize network channel first.");
                    }

                    m_Socket.SendBufferSize = value;
                }
            }

            /// <summary>
            /// 网络频道轮询。
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
            /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (m_Socket == null || !m_Active)
                {
                    return;
                }

                if (m_HeartBeatInterval < 0f)
                {
                    return;
                }

                bool sendHeartBeat = false;
                int missHeartBeatCount = 0;
                lock (m_HeartBeatState)
                {
                    m_HeartBeatState.HeartBeatElapseSeconds += realElapseSeconds;
                    if (m_HeartBeatState.HeartBeatElapseSeconds >= m_HeartBeatInterval)
                    {
                        sendHeartBeat = true;
                        missHeartBeatCount = m_HeartBeatState.MissHeartBeatCount;
                        m_HeartBeatState.HeartBeatElapseSeconds = 0f;
                        m_HeartBeatState.MissHeartBeatCount++;
                    }
                }

                if (sendHeartBeat && m_NetworkHelper.SendHeartBeat(this))
                {
                    if (missHeartBeatCount > 0 && NetworkChannelMissHeartBeat != null)
                    {
                        NetworkChannelMissHeartBeat(this, missHeartBeatCount);
                    }
                }
            }

            /// <summary>
            /// 连接到远程主机。
            /// </summary>
            /// <param name="ipAddress">远程主机的 IP 地址。</param>
            /// <param name="port">远程主机的端口号。</param>
            public void Connect(IPAddress ipAddress, int port)
            {
                Connect(ipAddress, port, DefaultPacketHeaderLength, DefaultMaxPacketLength, null);
            }

            /// <summary>
            /// 连接到远程主机。
            /// </summary>
            /// <param name="ipAddress">远程主机的 IP 地址。</param>
            /// <param name="port">远程主机的端口号。</param>
            /// <param name="maxPacketLength">数据包最大字节数。</param>
            public void Connect(IPAddress ipAddress, int port, int maxPacketLength)
            {
                Connect(ipAddress, port, DefaultPacketHeaderLength, maxPacketLength, null);
            }

            /// <summary>
            /// 连接到远程主机。
            /// </summary>
            /// <param name="ipAddress">远程主机的 IP 地址。</param>
            /// <param name="port">远程主机的端口号。</param>
            /// <param name="userData">用户自定义数据。</param>
            public void Connect(IPAddress ipAddress, int port, object userData)
            {
                Connect(ipAddress, port, DefaultPacketHeaderLength, DefaultMaxPacketLength, userData);
            }

            /// <summary>
            /// 连接到远程主机。
            /// </summary>
            /// <param name="ipAddress">远程主机的 IP 地址。</param>
            /// <param name="port">远程主机的端口号。</param>
            /// <param name="packetHeaderLength">数据包头长度。</param>
            /// <param name="maxPacketLength">数据包最大字节数。</param>
            public void Connect(IPAddress ipAddress, int port, int packetHeaderLength, int maxPacketLength)
            {
                Connect(ipAddress, port, DefaultPacketHeaderLength, DefaultMaxPacketLength, null);
            }

            /// <summary>
            /// 连接到远程主机。
            /// </summary>
            /// <param name="ipAddress">远程主机的 IP 地址。</param>
            /// <param name="port">远程主机的端口号。</param>
            /// <param name="maxPacketLength">数据包最大字节数。</param>
            /// <param name="userData">用户自定义数据。</param>
            public void Connect(IPAddress ipAddress, int port, int maxPacketLength, object userData)
            {
                Connect(ipAddress, port, DefaultPacketHeaderLength, maxPacketLength, userData);
            }

            /// <summary>
            /// 连接到远程主机。
            /// </summary>
            /// <param name="ipAddress">远程主机的 IP 地址。</param>
            /// <param name="port">远程主机的端口号。</param>
            /// <param name="packetHeaderLength">数据包头长度。</param>
            /// <param name="maxPacketLength">数据包最大字节数。</param>
            /// <param name="userData">用户自定义数据。</param>
            public void Connect(IPAddress ipAddress, int port, int packetHeaderLength, int maxPacketLength, object userData)
            {
                Initialize(ipAddress.AddressFamily, packetHeaderLength, maxPacketLength);

                if (m_Socket == null)
                {
                    string errorMessage = "Initialize network channel failure.";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.StatusError, errorMessage);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                try
                {
                    m_Socket.BeginConnect(ipAddress, port, ConnectCallback, new SocketUserData(m_Socket, userData));
                }
                catch (Exception exception)
                {
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.ConnectError, exception.Message);
                        return;
                    }

                    throw;
                }
            }

            /// <summary>
            /// 关闭连接并释放所有相关资源。
            /// </summary>
            public void Close()
            {
                if (m_Socket == null)
                {
                    return;
                }

                m_Active = false;
                try
                {
                    m_Socket.Shutdown(SocketShutdown.Both);
                }
                catch
                {
                }
                finally
                {
                    m_Socket.Close();
                    m_Socket = null;
                    m_ReceiveState = null;

                    if (NetworkChannelClosed != null)
                    {
                        NetworkChannelClosed(this);
                    }
                }
            }

            /// <summary>
            /// 向远程主机发送数据包。
            /// </summary>
            /// <param name="buffer">数据包流。</param>
            public void Send(byte[] buffer)
            {
                Send(buffer, 0, buffer.Length, null);
            }

            /// <summary>
            /// 向远程主机发送数据包。
            /// </summary>
            /// <param name="buffer">数据包流。</param>
            /// <param name="userData">用户自定义数据。</param>
            public void Send(byte[] buffer, object userData)
            {
                Send(buffer, 0, buffer.Length, userData);
            }

            /// <summary>
            /// 向远程主机发送数据包。
            /// </summary>
            /// <param name="buffer">数据包流。</param>
            /// <param name="offset">要发送数据包的偏移。</param>
            /// <param name="size">要发送数据包的长度。</param>
            public void Send(byte[] buffer, int offset, int size)
            {
                Send(buffer, offset, size, null);
            }

            /// <summary>
            /// 向远程主机发送数据包。
            /// </summary>
            /// <param name="buffer">数据包流。</param>
            /// <param name="offset">要发送数据包的偏移。</param>
            /// <param name="size">要发送数据包的长度。</param>
            /// <param name="userData">用户自定义数据。</param>
            public void Send(byte[] buffer, int offset, int size, object userData)
            {
                if (m_Socket == null)
                {
                    string errorMessage = "You must initialize network channel first.";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.StatusError, errorMessage);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                try
                {
                    m_Socket.BeginSend(buffer, offset, size, SocketFlags.None, SendCallback, new SocketUserData(m_Socket, userData));
                }
                catch (Exception exception)
                {
                    m_Active = false;
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.SendError, exception.Message);
                        return;
                    }

                    throw;
                }
            }

            /// <summary>
            /// 向远程主机发送数据包。
            /// </summary>
            /// <typeparam name="T">数据包类型。</typeparam>
            /// <param name="packet">要发送的数据包。</param>
            public void Send<T>(T packet) where T : Packet
            {
                Send(packet, null);
            }

            /// <summary>
            /// 向远程主机发送数据包。
            /// </summary>
            /// <typeparam name="T">数据包类型。</typeparam>
            /// <param name="packet">要发送的数据包。</param>
            /// <param name="userData">用户自定义数据。</param>
            public void Send<T>(T packet, object userData) where T : Packet
            {
                if (m_Socket == null)
                {
                    string errorMessage = "You must initialize network channel first.";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.StatusError, errorMessage);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                if (packet == null)
                {
                    string errorMessage = "Packet is invalid.";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.SendError, errorMessage);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                try
                {
                    int length = 0;
                    int packetLength = 0;
                    byte[] packetBuffer = new byte[m_MaxPacketLength];
                    using (MemoryStream memoryStream = new MemoryStream(packetBuffer, true))
                    {
                        memoryStream.Seek(m_PacketHeaderLength, SeekOrigin.Begin);
                        m_NetworkHelper.Serialize(this, memoryStream, packet);
                        length = (int)memoryStream.Position;
                    }

                    packetLength = length - m_PacketHeaderLength;
                    if (m_PacketHeaderLength == 4)
                    {
                        Utility.Converter.GetBytes(packetLength).CopyTo(packetBuffer, 0);
                    }
                    else if (m_PacketHeaderLength == 2)
                    {
                        Utility.Converter.GetBytes((ushort)packetLength).CopyTo(packetBuffer, 0);
                    }
                    else
                    {
                        packetBuffer[0] = (byte)packetLength;
                    }

                    Send(packetBuffer, 0, length, userData);
                }
                catch (Exception exception)
                {
                    m_Active = false;
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.SerializeError, exception.ToString());
                        return;
                    }

                    throw;
                }
            }

            /// <summary>
            /// 释放资源。
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// 释放资源。
            /// </summary>
            /// <param name="disposing">释放资源标记。</param>
            private void Dispose(bool disposing)
            {
                if (m_Disposed)
                {
                    return;
                }

                if (disposing)
                {
                    Close();
                }

                m_Disposed = true;
            }

            private void Initialize(AddressFamily addressFamily, int packetHeaderLength, int maxPacketLength)
            {
                if (m_Socket != null)
                {
                    Close();
                    m_Socket = null;
                }

                if (packetHeaderLength != 1 || packetHeaderLength != 2 || packetHeaderLength != 4)
                {
                    throw new GameFrameworkException("Packet header length is invalid, you can only use 1, 2 or 4.");
                }

                if (maxPacketLength <= 0)
                {
                    throw new GameFrameworkException("Max packet length is invalid.");
                }

                m_PacketHeaderLength = packetHeaderLength;
                m_MaxPacketLength = maxPacketLength;

                switch (addressFamily)
                {
                    case AddressFamily.InterNetwork:
                        m_NetworkType = NetworkType.IPv4;
                        break;
                    case AddressFamily.InterNetworkV6:
                        m_NetworkType = NetworkType.IPv6;
                        break;
                    default:
                        throw new GameFrameworkException(string.Format("Not supported address family '{0}'.", addressFamily.ToString()));
                }

                m_Socket = new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);
                m_ReceiveState = new ReceiveState(maxPacketLength);
                m_ReceiveState.Reset(m_PacketHeaderLength);
            }

            private void Receive()
            {
                try
                {
                    m_Socket.BeginReceive(m_ReceiveState.GetBuffer(), m_ReceiveState.ReceivedLength, m_ReceiveState.Length - m_ReceiveState.ReceivedLength, SocketFlags.None, ReceiveCallback, m_Socket);
                }
                catch (Exception exception)
                {
                    m_Active = false;
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.ReceiveError, exception.Message);
                        return;
                    }

                    throw;
                }
            }

            private bool Process()
            {
                if (m_ReceiveState.ReceivedLength != m_ReceiveState.Length)
                {
                    throw new GameFrameworkException(string.Format("Receive length '{0}' is not equal to length '{1}'.", m_ReceiveState.ReceivedLength.ToString(), m_ReceiveState.Length.ToString()));
                }

                if (m_ReceiveState.Length < m_PacketHeaderLength)
                {
                    throw new GameFrameworkException(string.Format("Length '{0}' is smaller than length header.", m_ReceiveState.Length.ToString()));
                }

                if (m_ReceiveState.Length == m_PacketHeaderLength)
                {
                    int packetLength = m_PacketHeaderLength == 4 ? Utility.Converter.GetInt32(m_ReceiveState.GetBuffer()) : (m_PacketHeaderLength == 2 ? Utility.Converter.GetUInt16(m_ReceiveState.GetBuffer()) : m_ReceiveState.GetBuffer()[0]);
                    if (packetLength <= 0)
                    {
                        string errorMessage = string.Format("Packet length '{0}' is invalid.", packetLength.ToString());
                        if (NetworkChannelError != null)
                        {
                            NetworkChannelError(this, NetworkErrorCode.HeaderError, errorMessage);
                            return false;
                        }

                        throw new GameFrameworkException(errorMessage);
                    }

                    m_ReceiveState.Length += packetLength;
                    if (m_ReceiveState.Length > m_ReceiveState.BufferSize)
                    {
                        string errorMessage = string.Format("Length '{0}' is larger than buffer size '{1}'.", m_ReceiveState.Length.ToString(), m_ReceiveState.BufferSize.ToString());
                        if (NetworkChannelError != null)
                        {
                            NetworkChannelError(this, NetworkErrorCode.OutOfRangeError, errorMessage);
                            return false;
                        }

                        throw new GameFrameworkException(errorMessage);
                    }

                    return true;
                }

                lock (m_HeartBeatState)
                {
                    m_HeartBeatState.Reset(m_ResetHeartBeatElapseSecondsWhenReceivePacket);
                }

                Packet packet = null;
                try
                {
                    int packetLength = m_ReceiveState.Length - m_PacketHeaderLength;
                    object customErrorData = null;
                    using (MemoryStream memoryStream = new MemoryStream(m_ReceiveState.GetBuffer(), m_PacketHeaderLength, packetLength, false))
                    {
                        lock (m_NetworkHelper)
                        {
                            packet = m_NetworkHelper.Deserialize(this, memoryStream, out customErrorData);
                        }
                    }

                    m_ReceiveState.Reset(m_PacketHeaderLength);
                    if (packet == null)
                    {
                        if (NetworkChannelCustomError != null)
                        {
                            NetworkChannelCustomError(this, customErrorData);
                        }
                    }
                    else
                    {
                        if (NetworkChannelReceived != null)
                        {
                            NetworkChannelReceived(this, packet);
                        }
                    }
                }
                catch (Exception exception)
                {
                    m_Active = false;
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.DeserializeError, exception.ToString());
                        return false;
                    }

                    throw;
                }

                return true;
            }

            private void ConnectCallback(IAsyncResult ar)
            {
                SocketUserData socketUserData = (SocketUserData)ar.AsyncState;
                try
                {
                    socketUserData.Socket.EndConnect(ar);
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (Exception exception)
                {
                    m_Active = false;
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.ConnectError, exception.Message);
                        return;
                    }

                    throw;
                }

                m_Active = true;
                lock (m_HeartBeatState)
                {
                    m_HeartBeatState.Reset(true);
                }

                if (NetworkChannelConnected != null)
                {
                    NetworkChannelConnected(this, socketUserData.UserData);
                }

                Receive();
            }

            private void SendCallback(IAsyncResult ar)
            {
                SocketUserData socketUserData = (SocketUserData)ar.AsyncState;
                int bytesSent = 0;
                try
                {
                    bytesSent = socketUserData.Socket.EndSend(ar);
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (Exception exception)
                {
                    m_Active = false;
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.SendError, exception.Message);
                        return;
                    }

                    throw;
                }

                if (NetworkChannelSended != null)
                {
                    NetworkChannelSended(this, bytesSent, socketUserData.UserData);
                }
            }

            private void ReceiveCallback(IAsyncResult ar)
            {
                Socket socket = (Socket)ar.AsyncState;
                int bytesReceived = 0;
                try
                {
                    bytesReceived = socket.EndReceive(ar);
                }
                catch (ObjectDisposedException)
                {
                    return;
                }
                catch (Exception exception)
                {
                    m_Active = false;
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.ReceiveError, exception.Message);
                        return;
                    }

                    throw;
                }

                if (bytesReceived <= 0)
                {
                    Close();
                    return;
                }

                m_ReceiveState.ReceivedLength += bytesReceived;
                if (m_ReceiveState.ReceivedLength < m_ReceiveState.Length)
                {
                    Receive();
                    return;
                }

                bool processSuccess = false;
                try
                {
                    processSuccess = Process();
                }
                catch (Exception exception)
                {
                    m_Active = false;
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.StreamError, exception.Message);
                        return;
                    }

                    throw;
                }

                if (processSuccess)
                {
                    Receive();
                    return;
                }
            }
        }
    }
}
