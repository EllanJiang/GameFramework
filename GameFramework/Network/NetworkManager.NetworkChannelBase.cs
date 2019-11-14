//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace GameFramework.Network
{
    internal sealed partial class NetworkManager : GameFrameworkModule, INetworkManager
    {
        /// <summary>
        /// 网络频道基类。
        /// </summary>
        private abstract class NetworkChannelBase : INetworkChannel, IDisposable
        {
            private const float DefaultHeartBeatInterval = 30f;

            private readonly string m_Name;
            protected readonly Queue<Packet> m_SendPacketPool;
            protected readonly EventPool<Packet> m_ReceivePacketPool;
            protected readonly INetworkChannelHelper m_NetworkChannelHelper;
            protected AddressFamily m_AddressFamily;
            protected bool m_ResetHeartBeatElapseSecondsWhenReceivePacket;
            protected float m_HeartBeatInterval;
            protected Socket m_Socket;
            protected readonly SendState m_SendState;
            protected readonly ReceiveState m_ReceiveState;
            protected readonly HeartBeatState m_HeartBeatState;
            protected int m_SentPacketCount;
            protected int m_ReceivedPacketCount;
            protected bool m_Active;
            private bool m_Disposed;

            public GameFrameworkAction<NetworkChannelBase, object> NetworkChannelConnected;
            public GameFrameworkAction<NetworkChannelBase> NetworkChannelClosed;
            public GameFrameworkAction<NetworkChannelBase, int> NetworkChannelMissHeartBeat;
            public GameFrameworkAction<NetworkChannelBase, NetworkErrorCode, SocketError, string> NetworkChannelError;
            public GameFrameworkAction<NetworkChannelBase, object> NetworkChannelCustomError;

            /// <summary>
            /// 初始化网络频道基类的新实例。
            /// </summary>
            /// <param name="name">网络频道名称。</param>
            /// <param name="networkChannelHelper">网络频道辅助器。</param>
            public NetworkChannelBase(string name, INetworkChannelHelper networkChannelHelper)
            {
                m_Name = name ?? string.Empty;
                m_SendPacketPool = new Queue<Packet>();
                m_ReceivePacketPool = new EventPool<Packet>(EventPoolMode.Default);
                m_NetworkChannelHelper = networkChannelHelper;
                m_AddressFamily = AddressFamily.Unknown;
                m_ResetHeartBeatElapseSecondsWhenReceivePacket = false;
                m_HeartBeatInterval = DefaultHeartBeatInterval;
                m_Socket = null;
                m_SendState = new SendState();
                m_ReceiveState = new ReceiveState();
                m_HeartBeatState = new HeartBeatState();
                m_SentPacketCount = 0;
                m_ReceivedPacketCount = 0;
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
            /// 获取网络频道所使用的 Socket。
            /// </summary>
            public Socket Socket
            {
                get
                {
                    return m_Socket;
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
            /// 获取网络服务类型。
            /// </summary>
            public abstract ServiceType ServiceType
            {
                get;
            }

            /// <summary>
            /// 获取网络地址类型。
            /// </summary>
            public AddressFamily AddressFamily
            {
                get
                {
                    return m_AddressFamily;
                }
            }

            /// <summary>
            /// 获取要发送的消息包数量。
            /// </summary>
            public int SendPacketCount
            {
                get
                {
                    return m_SendPacketPool.Count;
                }
            }

            /// <summary>
            /// 获取累计发送的消息包数量。
            /// </summary>
            public int SentPacketCount
            {
                get
                {
                    return m_SentPacketCount;
                }
            }

            /// <summary>
            /// 获取已接收未处理的消息包数量。
            /// </summary>
            public int ReceivePacketCount
            {
                get
                {
                    return m_ReceivePacketPool.EventCount;
                }
            }

            /// <summary>
            /// 获取累计已接收的消息包数量。
            /// </summary>
            public int ReceivedPacketCount
            {
                get
                {
                    return m_ReceivedPacketCount;
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
            /// 获取丢失心跳的次数。
            /// </summary>
            public int MissHeartBeatCount
            {
                get
                {
                    return m_HeartBeatState.MissHeartBeatCount;
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
            /// 获取心跳等待时长，以秒为单位。
            /// </summary>
            public float HeartBeatElapseSeconds
            {
                get
                {
                    return m_HeartBeatState.HeartBeatElapseSeconds;
                }
            }

            /// <summary>
            /// 网络频道轮询。
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
            /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
            public virtual void Update(float elapseSeconds, float realElapseSeconds)
            {
            }

            /// <summary>
            /// 关闭网络频道。
            /// </summary>
            public virtual void Shutdown()
            {
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
            public abstract void Connect(IPAddress ipAddress, int port, object userData);

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

                    m_SentPacketCount = 0;
                    m_ReceivedPacketCount = 0;

                    lock (m_SendPacketPool)
                    {
                        m_SendPacketPool.Clear();
                    }

                    m_ReceivePacketPool.Clear();

                    lock (m_HeartBeatState)
                    {
                        m_HeartBeatState.Reset(true);
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
                        NetworkChannelError(this, NetworkErrorCode.SendError, SocketError.Success, errorMessage);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                if (!m_Active)
                {
                    string errorMessage = "Socket is not active.";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.SendError, SocketError.Success, errorMessage);
                        return;
                    }

                    throw new GameFrameworkException(errorMessage);
                }

                if (packet == null)
                {
                    string errorMessage = "Packet is invalid.";
                    if (NetworkChannelError != null)
                    {
                        NetworkChannelError(this, NetworkErrorCode.SendError, SocketError.Success, errorMessage);
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
                    m_SendState.Dispose();
                    m_ReceiveState.Dispose();
                }

                m_Disposed = true;
            }
        }
    }
}
