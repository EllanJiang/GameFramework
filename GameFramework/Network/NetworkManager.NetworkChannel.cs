//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
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
            private const float DefaultHeartBeatInterval = 30f;

            private readonly string m_Name;
            private readonly EventPool<Packet> m_EventPool;
            private readonly INetworkChannelHelper m_NetworkChannelHelper;
            private NetworkType m_NetworkType;
            private bool m_ResetHeartBeatElapseSecondsWhenReceivePacket;
            private float m_HeartBeatInterval;
            private Socket m_Socket;
            private readonly ReceiveState m_ReceiveState;
            private readonly HeartBeatState m_HeartBeatState;
            private bool m_Active;
            private bool m_Disposed;

            public GameFrameworkAction<NetworkChannel, object> NetworkChannelConnected;
            public GameFrameworkAction<NetworkChannel> NetworkChannelClosed;
            public GameFrameworkAction<NetworkChannel, int, object> NetworkChannelSended;
            public GameFrameworkAction<NetworkChannel, int> NetworkChannelMissHeartBeat;
            public GameFrameworkAction<NetworkChannel, NetworkErrorCode, string> NetworkChannelError;
            public GameFrameworkAction<NetworkChannel, object> NetworkChannelCustomError;

            /// <summary>
            /// 初始化网络频道的新实例。
            /// </summary>
            /// <param name="name">网络频道名称。</param>
            /// <param name="networkChannelHelper">网络频道辅助器。</param>
            public NetworkChannel(string name, INetworkChannelHelper networkChannelHelper)
            {
                m_Name = name ?? string.Empty;
                m_EventPool = new EventPool<Packet>(EventPoolMode.Default);
                m_NetworkChannelHelper = networkChannelHelper;
                m_NetworkType = NetworkType.Unknown;
                m_ResetHeartBeatElapseSecondsWhenReceivePacket = false;
                m_HeartBeatInterval = DefaultHeartBeatInterval;
                m_Socket = null;
                m_ReceiveState = new ReceiveState();
                m_HeartBeatState = new HeartBeatState();
                m_Active = false;
                m_Disposed = false;

                NetworkChannelConnected = null;
                NetworkChannelClosed = null;
                NetworkChannelSended = null;
                NetworkChannelMissHeartBeat = null;
                NetworkChannelError = null;
                NetworkChannelCustomError = null;

                networkChannelHelper.Initialize(this);
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
                        throw new GameFrameworkException("You must connect first.");
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
                        throw new GameFrameworkException("You must connect first.");
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
                        throw new GameFrameworkException("You must connect first.");
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
                        throw new GameFrameworkException("You must connect first.");
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
                        throw new GameFrameworkException("You must connect first.");
                    }

                    return m_Socket.ReceiveBufferSize;
                }
                set
                {
                    if (m_Socket == null)
                    {
                        throw new GameFrameworkException("You must connect first.");
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
                        throw new GameFrameworkException("You must connect first.");
                    }

                    return m_Socket.SendBufferSize;
                }
                set
                {
                    if (m_Socket == null)
                    {
                        throw new GameFrameworkException("You must connect first.");
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

                m_EventPool.Update(elapseSeconds, realElapseSeconds);

                if (m_HeartBeatInterval > 0f)
                {
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

                    if (sendHeartBeat && m_NetworkChannelHelper.SendHeartBeat())
                    {
                        if (missHeartBeatCount > 0 && NetworkChannelMissHeartBeat != null)
                        {
                            NetworkChannelMissHeartBeat(this, missHeartBeatCount);
                        }
                    }
                }
            }

            /// <summary>
            /// 关闭网络频道。
            /// </summary>
            public void Shutdown()
            {
                Close();
                m_EventPool.Shutdown();
                m_NetworkChannelHelper.Shutdown();
            }

            /// <summary>
            /// 注册网络消息包处理函数。
            /// </summary>
            /// <param name="handler">要注册的网络消息包处理函数。</param>
            public void RegisterHandler(IPacketHandler handler)
            {
                if (handler == null)
                {
                    throw new GameFrameworkException("Packet handler is invalid.");
                }

                m_EventPool.Subscribe(handler.Id, handler.Handle);
            }

            /// <summary>
            /// 连接到远程主机。
            /// </summary>
            /// <param name="ipAddress">远程主机的 IP 地址。</param>
            /// <param name="port">远程主机的端口号。</param>
            public void Connect(IPAddress ipAddress, int port)
            {
                Connect(ipAddress, port, null);
            }

            /// <summary>
            /// 连接到远程主机。
            /// </summary>
            /// <param name="ipAddress">远程主机的 IP 地址。</param>
            /// <param name="port">远程主机的端口号。</param>
            /// <param name="userData">用户自定义数据。</param>
            public void Connect(IPAddress ipAddress, int port, object userData)
            {
                if (m_Socket != null)
                {
                    Close();
                    m_Socket = null;
                }

                switch (ipAddress.AddressFamily)
                {
                    case AddressFamily.InterNetwork:
                        m_NetworkType = NetworkType.IPv4;
                        break;
                    case AddressFamily.InterNetworkV6:
                        m_NetworkType = NetworkType.IPv6;
                        break;
                    default:
                        string errorMessage = string.Format("Not supported address family '{0}'.", ipAddress.AddressFamily.ToString());
                        if (NetworkChannelError != null)
                        {
                            NetworkChannelError(this, NetworkErrorCode.AddressFamilyError, errorMessage);
                            return;
                        }

                        throw new GameFrameworkException(errorMessage);
                }

                m_Socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                if (m_Socket == null)
                {
                    string errorMessage = "Initialize network channel failure.";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.SocketError, errorMessage);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                m_ReceiveState.PrepareForPacketHeader(m_NetworkChannelHelper.PacketHeaderLength);

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

                m_EventPool.Clear();

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

                    if (NetworkChannelClosed != null)
                    {
                        NetworkChannelClosed(this);
                    }
                }
            }

            /// <summary>
            /// 向远程主机发送消息包。
            /// </summary>
            /// <param name="buffer">消息包流。</param>
            public void Send(byte[] buffer)
            {
                Send(buffer, 0, buffer.Length, null);
            }

            /// <summary>
            /// 向远程主机发送消息包。
            /// </summary>
            /// <param name="buffer">消息包流。</param>
            /// <param name="userData">用户自定义数据。</param>
            public void Send(byte[] buffer, object userData)
            {
                Send(buffer, 0, buffer.Length, userData);
            }

            /// <summary>
            /// 向远程主机发送消息包。
            /// </summary>
            /// <param name="buffer">消息包流。</param>
            /// <param name="offset">要发送消息包的偏移。</param>
            /// <param name="size">要发送消息包的长度。</param>
            public void Send(byte[] buffer, int offset, int size)
            {
                Send(buffer, offset, size, null);
            }

            /// <summary>
            /// 向远程主机发送消息包。
            /// </summary>
            /// <param name="buffer">消息包流。</param>
            /// <param name="offset">要发送消息包的偏移。</param>
            /// <param name="size">要发送消息包的长度。</param>
            /// <param name="userData">用户自定义数据。</param>
            public void Send(byte[] buffer, int offset, int size, object userData)
            {
                if (m_Socket == null)
                {
                    string errorMessage = "You must connect first.";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.SocketError, errorMessage);
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
            /// 向远程主机发送消息包。
            /// </summary>
            /// <typeparam name="T">消息包类型。</typeparam>
            /// <param name="packet">要发送的消息包。</param>
            public void Send<T>(T packet) where T : Packet
            {
                Send(packet, null);
            }

            /// <summary>
            /// 向远程主机发送消息包。
            /// </summary>
            /// <typeparam name="T">消息包类型。</typeparam>
            /// <param name="packet">要发送的消息包。</param>
            /// <param name="userData">用户自定义数据。</param>
            public void Send<T>(T packet, object userData) where T : Packet
            {
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

                byte[] buffer = null;
                try
                {
                    buffer = m_NetworkChannelHelper.Serialize(packet);
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

                if (buffer == null || buffer.Length <= 0)
                {
                    string errorMessage = "Serialized packet is invalid.";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.SerializeError, errorMessage);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                Send(buffer, userData);
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

            private void Receive()
            {
                try
                {
                    m_Socket.BeginReceive(m_ReceiveState.Stream.GetBuffer(), (int)m_ReceiveState.Stream.Position, (int)(m_ReceiveState.Stream.Length - m_ReceiveState.Stream.Position), SocketFlags.None, ReceiveCallback, m_Socket);
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

            private bool ProcessPacketHeader()
            {
                try
                {
                    object customErrorData = null;
                    IPacketHeader packetHeader = m_NetworkChannelHelper.DeserializePacketHeader(m_ReceiveState.Stream, out customErrorData);

                    if (customErrorData != null && NetworkChannelCustomError != null)
                    {
                        NetworkChannelCustomError(this, customErrorData);
                    }

                    if (packetHeader == null)
                    {
                        string errorMessage = "Packet header is invalid.";
                        if (NetworkChannelError != null)
                        {
                            NetworkChannelError(this, NetworkErrorCode.DeserializePacketHeaderError, errorMessage);
                            return false;
                        }

                        throw new GameFrameworkException(errorMessage);
                    }

                    m_ReceiveState.PrepareForPacket(packetHeader);
                    if (packetHeader.PacketLength <= 0)
                    {
                        ProcessPacket();
                    }
                }
                catch (Exception exception)
                {
                    m_Active = false;
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.DeserializePacketHeaderError, exception.ToString());
                        return false;
                    }

                    throw;
                }

                return true;
            }

            private bool ProcessPacket()
            {
                lock (m_HeartBeatState)
                {
                    m_HeartBeatState.Reset(m_ResetHeartBeatElapseSecondsWhenReceivePacket);
                }

                try
                {
                    object customErrorData = null;
                    Packet packet = m_NetworkChannelHelper.DeserializePacket(m_ReceiveState.PacketHeader, m_ReceiveState.Stream, out customErrorData);

                    if (customErrorData != null && NetworkChannelCustomError != null)
                    {
                        NetworkChannelCustomError(this, customErrorData);
                    }

                    if (packet != null)
                    {
                        m_EventPool.Fire(this, packet);
                    }

                    m_ReceiveState.PrepareForPacketHeader(m_NetworkChannelHelper.PacketHeaderLength);
                }
                catch (Exception exception)
                {
                    m_Active = false;
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.DeserializePacketError, exception.ToString());
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

                m_ReceiveState.Stream.Position += bytesReceived;
                if (m_ReceiveState.Stream.Position < m_ReceiveState.Stream.Length)
                {
                    Receive();
                    return;
                }

                m_ReceiveState.Stream.Position = 0L;

                bool processSuccess = false;
                if (m_ReceiveState.PacketHeader != null)
                {
                    processSuccess = ProcessPacket();
                }
                else
                {
                    processSuccess = ProcessPacketHeader();
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
