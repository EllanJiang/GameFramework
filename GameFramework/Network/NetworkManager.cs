//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Network
{
    /// <summary>
    /// 网络管理器。
    /// </summary>
    internal sealed partial class NetworkManager : GameFrameworkModule, INetworkManager
    {
        private readonly Dictionary<string, NetworkChannel> m_NetworkChannels;
        private readonly EventPool<Packet> m_EventPool;
        private INetworkHelper m_NetworkHelper;

        private EventHandler<NetworkConnectedEventArgs> m_NetworkConnectedEventHandler;
        private EventHandler<NetworkClosedEventArgs> m_NetworkClosedEventHandler;
        private EventHandler<NetworkSendPacketEventArgs> m_NetworkSendPacketEventHandler;
        private EventHandler<NetworkMissHeartBeatEventArgs> m_NetworkMissHeartBeatEventHandler;
        private EventHandler<NetworkErrorEventArgs> m_NetworkErrorEventHandler;
        private EventHandler<NetworkCustomErrorEventArgs> m_NetworkCustomErrorEventHandler;

        /// <summary>
        /// 初始化网络管理器的新实例。
        /// </summary>
        public NetworkManager()
        {
            m_NetworkChannels = new Dictionary<string, NetworkChannel>();
            m_EventPool = new EventPool<Packet>(EventPoolMode.Default);
            m_NetworkHelper = null;

            m_NetworkConnectedEventHandler = null;
            m_NetworkClosedEventHandler = null;
            m_NetworkSendPacketEventHandler = null;
            m_NetworkMissHeartBeatEventHandler = null;
            m_NetworkErrorEventHandler = null;
            m_NetworkCustomErrorEventHandler = null;
        }

        /// <summary>
        /// 获取网络频道数量。
        /// </summary>
        public int NetworkChannelCount
        {
            get
            {
                return m_NetworkChannels.Count;
            }
        }

        /// <summary>
        /// 网络连接成功事件。
        /// </summary>
        public event EventHandler<NetworkConnectedEventArgs> NetworkConnected
        {
            add
            {
                m_NetworkConnectedEventHandler += value;
            }
            remove
            {
                m_NetworkConnectedEventHandler -= value;
            }
        }

        /// <summary>
        /// 网络连接关闭事件。
        /// </summary>
        public event EventHandler<NetworkClosedEventArgs> NetworkClosed
        {
            add
            {
                m_NetworkClosedEventHandler += value;
            }
            remove
            {
                m_NetworkClosedEventHandler -= value;
            }
        }

        /// <summary>
        /// 发送网络消息包事件。
        /// </summary>
        public event EventHandler<NetworkSendPacketEventArgs> NetworkSendPacket
        {
            add
            {
                m_NetworkSendPacketEventHandler += value;
            }
            remove
            {
                m_NetworkSendPacketEventHandler -= value;
            }
        }

        /// <summary>
        /// 网络心跳包丢失事件。
        /// </summary>
        public event EventHandler<NetworkMissHeartBeatEventArgs> NetworkMissHeartBeat
        {
            add
            {
                m_NetworkMissHeartBeatEventHandler += value;
            }
            remove
            {
                m_NetworkMissHeartBeatEventHandler -= value;
            }
        }

        /// <summary>
        /// 网络错误事件。
        /// </summary>
        public event EventHandler<NetworkErrorEventArgs> NetworkError
        {
            add
            {
                m_NetworkErrorEventHandler += value;
            }
            remove
            {
                m_NetworkErrorEventHandler -= value;
            }
        }

        /// <summary>
        /// 用户自定义网络错误事件。
        /// </summary>
        public event EventHandler<NetworkCustomErrorEventArgs> NetworkCustomError
        {
            add
            {
                m_NetworkCustomErrorEventHandler += value;
            }
            remove
            {
                m_NetworkCustomErrorEventHandler -= value;
            }
        }

        /// <summary>
        /// 网络管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (KeyValuePair<string, NetworkChannel> networkChannel in m_NetworkChannels)
            {
                networkChannel.Value.Update(elapseSeconds, realElapseSeconds);
            }

            m_EventPool.Update(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 关闭并清理网络管理器。
        /// </summary>
        internal override void Shutdown()
        {
            foreach (KeyValuePair<string, NetworkChannel> networkChannel in m_NetworkChannels)
            {
                NetworkChannel nc = networkChannel.Value;
                nc.Close();
                nc.NetworkChannelConnected -= OnNetworkChannelConnected;
                nc.NetworkChannelClosed -= OnNetworkChannelClosed;
                nc.NetworkChannelSended -= OnNetworkChannelSended;
                nc.NetworkChannelReceived -= OnNetworkChannelReceived;
                nc.NetworkChannelMissHeartBeat -= OnNetworkChannelMissHeartBeat;
                nc.NetworkChannelError -= OnNetworkChannelError;
                nc.NetworkChannelCustomError -= OnNetworkChannelCustomError;
            }

            m_NetworkChannels.Clear();
            m_EventPool.Shutdown();
        }

        /// <summary>
        /// 设置网络辅助器。
        /// </summary>
        /// <param name="networkHelper">网络辅助器。</param>
        public void SetNetworkHelper(INetworkHelper networkHelper)
        {
            if (networkHelper == null)
            {
                throw new GameFrameworkException("Network helper is invalid.");
            }

            m_NetworkHelper = networkHelper;
        }

        /// <summary>
        /// 检查是否存在网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>是否存在网络频道。</returns>
        public bool HasNetworkChannel(string name)
        {
            return m_NetworkChannels.ContainsKey(name ?? string.Empty);
        }

        /// <summary>
        /// 获取网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>要获取的网络频道。</returns>
        public INetworkChannel GetNetworkChannel(string name)
        {
            NetworkChannel networkChannel = null;
            if (m_NetworkChannels.TryGetValue(name ?? string.Empty, out networkChannel))
            {
                return networkChannel;
            }

            return null;
        }

        /// <summary>
        /// 获取所有网络频道。
        /// </summary>
        /// <returns>所有网络频道。</returns>
        public INetworkChannel[] GetAllNetworkChannels()
        {
            int index = 0;
            INetworkChannel[] networkChannels = new INetworkChannel[m_NetworkChannels.Count];
            foreach (KeyValuePair<string, NetworkChannel> networkChannel in m_NetworkChannels)
            {
                networkChannels[index++] = networkChannel.Value;
            }

            return networkChannels;
        }

        /// <summary>
        /// 创建网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>要创建的网络频道。</returns>
        public INetworkChannel CreateNetworkChannel(string name)
        {
            if (m_NetworkHelper == null)
            {
                throw new GameFrameworkException("You must set network helper first.");
            }

            if (HasNetworkChannel(name))
            {
                throw new GameFrameworkException(string.Format("Already exist network channel '{0}'.", name ?? string.Empty));
            }

            NetworkChannel networkChannel = new NetworkChannel(name, m_NetworkHelper);
            networkChannel.NetworkChannelConnected += OnNetworkChannelConnected;
            networkChannel.NetworkChannelClosed += OnNetworkChannelClosed;
            networkChannel.NetworkChannelSended += OnNetworkChannelSended;
            networkChannel.NetworkChannelReceived += OnNetworkChannelReceived;
            networkChannel.NetworkChannelMissHeartBeat += OnNetworkChannelMissHeartBeat;
            networkChannel.NetworkChannelError += OnNetworkChannelError;
            networkChannel.NetworkChannelCustomError += OnNetworkChannelCustomError;
            m_NetworkChannels.Add(name, networkChannel);
            return networkChannel;
        }

        /// <summary>
        /// 销毁网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>是否销毁网络频道成功。</returns>
        public bool DestroyNetworkChannel(string name)
        {
            NetworkChannel networkChannel = null;
            if (m_NetworkChannels.TryGetValue(name ?? string.Empty, out networkChannel))
            {
                networkChannel.Close();
                networkChannel.NetworkChannelConnected -= OnNetworkChannelConnected;
                networkChannel.NetworkChannelClosed -= OnNetworkChannelClosed;
                networkChannel.NetworkChannelSended -= OnNetworkChannelSended;
                networkChannel.NetworkChannelReceived -= OnNetworkChannelReceived;
                networkChannel.NetworkChannelMissHeartBeat -= OnNetworkChannelMissHeartBeat;
                networkChannel.NetworkChannelError -= OnNetworkChannelError;
                networkChannel.NetworkChannelCustomError -= OnNetworkChannelCustomError;
                return m_NetworkChannels.Remove(name);
            }

            return false;
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

        private void OnNetworkChannelConnected(NetworkChannel networkChannel, object userData)
        {
            if (m_NetworkConnectedEventHandler != null)
            {
                lock (m_NetworkConnectedEventHandler)
                {
                    m_NetworkConnectedEventHandler(this, new NetworkConnectedEventArgs(networkChannel, userData));
                }
            }
        }

        private void OnNetworkChannelClosed(NetworkChannel networkChannel)
        {
            if (m_NetworkClosedEventHandler != null)
            {
                lock (m_NetworkClosedEventHandler)
                {
                    m_NetworkClosedEventHandler(this, new NetworkClosedEventArgs(networkChannel));
                }
            }
        }

        private void OnNetworkChannelSended(NetworkChannel networkChannel, int bytesSent, object userData)
        {
            if (m_NetworkSendPacketEventHandler != null)
            {
                lock (m_NetworkSendPacketEventHandler)
                {
                    m_NetworkSendPacketEventHandler(this, new NetworkSendPacketEventArgs(networkChannel, bytesSent, userData));
                }
            }
        }

        private void OnNetworkChannelReceived(NetworkChannel networkChannel, Packet packet)
        {
            m_EventPool.Fire(networkChannel, packet);
        }

        private void OnNetworkChannelMissHeartBeat(NetworkChannel networkChannel, int missHeartBeatCount)
        {
            if (m_NetworkMissHeartBeatEventHandler != null)
            {
                lock (m_NetworkMissHeartBeatEventHandler)
                {
                    m_NetworkMissHeartBeatEventHandler(this, new NetworkMissHeartBeatEventArgs(networkChannel, missHeartBeatCount));
                }
            }
        }

        private void OnNetworkChannelError(NetworkChannel networkChannel, NetworkErrorCode errorCode, string errorMessage)
        {
            if (m_NetworkErrorEventHandler != null)
            {
                lock (m_NetworkErrorEventHandler)
                {
                    m_NetworkErrorEventHandler(this, new NetworkErrorEventArgs(networkChannel, errorCode, errorMessage));
                }
            }
        }

        private void OnNetworkChannelCustomError(NetworkChannel networkChannel, object customErrorData)
        {
            if (m_NetworkCustomErrorEventHandler != null)
            {
                lock (m_NetworkCustomErrorEventHandler)
                {
                    m_NetworkCustomErrorEventHandler(this, new NetworkCustomErrorEventArgs(networkChannel, customErrorData));
                }
            }
        }
    }
}
