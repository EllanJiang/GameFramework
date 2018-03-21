//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;
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
            private readonly Queue<Packet> m_SendPacketPool;
            private readonly EventPool<Packet> m_ReceivePacketPool;
            private readonly INetworkChannelHelper m_NetworkChannelHelper;
            private NetworkType m_NetworkType;
            private bool m_ResetHeartBeatElapseSecondsWhenReceivePacket;
            private float m_HeartBeatInterval;
            private Socket m_Socket;
            private readonly SendState m_SendState;
            private readonly ReceiveState m_ReceiveState;
            private readonly HeartBeatState m_HeartBeatState;
            private bool m_Active;
            private bool m_Disposed;

            public GameFrameworkAction<NetworkChannel, object> NetworkChannelConnected;
            public GameFrameworkAction<NetworkChannel> NetworkChannelClosed;
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
                m_SendPacketPool = new Queue<Packet>();
                m_ReceivePacketPool = new EventPool<Packet>(EventPoolMode.Default);
                m_NetworkChannelHelper = networkChannelHelper;
                m_NetworkType = NetworkType.Unknown;
                m_ResetHeartBeatElapseSecondsWhenReceivePacket = false;
                m_HeartBeatInterval = DefaultHeartBeatInterval;
                m_Socket = null;
                m_SendState = new SendState();
                m_ReceiveState = new ReceiveState();
                m_HeartBeatState = new HeartBeatState();
                m_Active = false;
                m_Disposed = false;

                NetworkChannelConnected = null;
                NetworkChannelClosed = null;
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
            /// 要发送的消息包数量。
            /// </summary>
            public int SendPacketCount
            {
                get
                {
                    return m_SendPacketPool.Count;
                }
            }

            /// <summary>
            /// 已接收未处理的消息包数量。
            /// </summary>
            public int ReceivePacketCount
            {
                get
                {
                    return m_ReceivePacketPool.Count;
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

                ProcessSend();
                m_ReceivePacketPool.Update(elapseSeconds, realElapseSeconds);

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
                m_ReceivePacketPool.Shutdown();
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

                m_ReceivePacketPool.Subscribe(handler.Id, handler.Handle);
            }

            /// <summary>
            /// 设置默认事件处理函数。
            /// </summary>
            /// <param name="handler">要设置的默认事件处理函数。</param>
            public void SetDefaultHandler(EventHandler<Packet> handler)
            {
                m_ReceivePacketPool.SetDefaultHandler(handler);
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
                    m_Socket.BeginConnect(ipAddress, port, ConnectCallback, new ConnectState(m_Socket, userData));
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
                lock (this)
                {
                    if (m_Socket == null)
                    {
                        return;
                    }

                    lock (m_SendPacketPool)
                    {
                        m_SendPacketPool.Clear();
                    }

                    m_ReceivePacketPool.Clear();

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
            }

            /// <summary>
            /// 向远程主机发送消息包。
            /// </summary>
            /// <typeparam name="T">消息包类型。</typeparam>
            /// <param name="packet">要发送的消息包。</param>
            public void Send<T>(T packet) where T : Packet
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

                lock (m_SendPacketPool)
                {
                    m_SendPacketPool.Enqueue(packet);
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

            private void Send()
            {
                try
                {
                    m_Socket.BeginSend(m_SendState.GetPacketBytes(), m_SendState.Offset, m_SendState.Length, SocketFlags.None, SendCallback, m_Socket);
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

            private void ProcessSend()
            {
                if (m_SendPacketPool.Count <= 0)
                {
                    return;
                }

                if (!m_SendState.IsFree)
                {
                    return;
                }

                Packet packet = null;
                lock (m_SendPacketPool)
                {
                    packet = m_SendPacketPool.Dequeue();
                }

                byte[] packetBytes = null;
                try
                {
                    packetBytes = m_NetworkChannelHelper.Serialize(packet);
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

                if (packetBytes == null || packetBytes.Length <= 0)
                {
                    string errorMessage = "Serialized packet is invalid.";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.SerializeError, errorMessage);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                m_SendState.SetPacket(packetBytes);
                Send();
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
                        m_ReceivePacketPool.Fire(this, packet);
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
                ConnectState socketUserData = (ConnectState)ar.AsyncState;
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
                Socket socket = (Socket)ar.AsyncState;
                try
                {
                    m_SendState.Offset += socket.EndSend(ar);
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

                if (m_SendState.Offset < m_SendState.Length)
                {
                    Send();
                    return;
                }

                m_SendState.Reset();
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
