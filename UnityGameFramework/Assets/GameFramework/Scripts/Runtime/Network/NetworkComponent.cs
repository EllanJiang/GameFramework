//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Network;
using System.Net;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 网络组件。
    /// </summary>
    [AddComponentMenu("Game Framework/Network")]
    public sealed partial class NetworkComponent : GameFrameworkComponent
    {
        private INetworkManager m_NetworkManager = null;
        private EventComponent m_EventComponent = null;

        [SerializeField]
        private string m_NetworkHelperTypeName = "UnityGameFramework.Runtime.DefaultNetworkHelper";

        [SerializeField]
        private NetworkHelperBase m_CustomNetworkHelper = null;

        [SerializeField]
        private NetworkChannel[] m_NetworkChannels = null;

        /// <summary>
        /// 获取网络频道数量。
        /// </summary>
        public int NetworkChannelCount
        {
            get
            {
                return m_NetworkManager.NetworkChannelCount;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected internal override void Awake()
        {
            base.Awake();

            m_NetworkManager = GameFrameworkEntry.GetModule<INetworkManager>();
            if (m_NetworkManager == null)
            {
                Log.Fatal("Network manager is invalid.");
                return;
            }

            m_NetworkManager.NetworkConnected += OnNetworkConnected;
            m_NetworkManager.NetworkClosed += OnNetworkClosed;
            m_NetworkManager.NetworkSendPacket += OnNetworkSendPacket;
            m_NetworkManager.NetworkMissHeartBeat += OnNetworkMissHeartBeat;
            m_NetworkManager.NetworkError += OnNetworkError;
            m_NetworkManager.NetworkCustomError += OnNetworkCustomError;
        }

        private void Start()
        {
            m_EventComponent = GameEntry.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            NetworkHelperBase networkHelper = Utility.Helper.CreateHelper(m_NetworkHelperTypeName, m_CustomNetworkHelper);
            if (networkHelper == null)
            {
                Log.Error("Can not create network helper.");
                return;
            }

            networkHelper.name = string.Format("Network Helper");
            Transform transform = networkHelper.transform;
            transform.SetParent(this.transform);
            transform.localScale = Vector3.one;

            m_NetworkManager.SetNetworkHelper(networkHelper);

            for (int i = 0; i < m_NetworkChannels.Length; i++)
            {
                m_NetworkChannels[i].SetNetworkChannel(CreateNetworkChannel(m_NetworkChannels[i].Name));
            }
        }

        /// <summary>
        /// 检查是否存在网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>是否存在网络频道。</returns>
        public bool HasNetworkChannel(string name)
        {
            return m_NetworkManager.HasNetworkChannel(name);
        }

        /// <summary>
        /// 获取网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>要获取的网络频道。</returns>
        public INetworkChannel GetNetworkChannel(string name)
        {
            return m_NetworkManager.GetNetworkChannel(name);
        }

        /// <summary>
        /// 获取所有网络频道。
        /// </summary>
        /// <returns>所有网络频道。</returns>
        public INetworkChannel[] GetAllNetworkChannels()
        {
            return m_NetworkManager.GetAllNetworkChannels();
        }

        /// <summary>
        /// 创建网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>要创建的网络频道。</returns>
        public INetworkChannel CreateNetworkChannel(string name)
        {
            return m_NetworkManager.CreateNetworkChannel(name);
        }

        /// <summary>
        /// 销毁网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>是否销毁网络频道成功。</returns>
        public bool DestroyNetworkChannel(string name)
        {
            return m_NetworkManager.DestroyNetworkChannel(name);
        }

        /// <summary>
        /// 设置网络频道的连接参数。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <param name="useIPv6">是否使用 IP 版本 6。</param>
        /// <param name="hostOrIPString">远程主机的名称或 IP 地址字符串。</param>
        /// <param name="port">远程主机的端口号。</param>
        public void SetNetworkChannelConnection(string name, bool useIPv6, string hostOrIPString, int port)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            for (int i = 0; i < m_NetworkChannels.Length; i++)
            {
                if (m_NetworkChannels[i].Name == name)
                {
                    m_NetworkChannels[i].UseIPv6 = useIPv6;
                    m_NetworkChannels[i].HostOrIPString = hostOrIPString;
                    m_NetworkChannels[i].Port = port;
                    return;
                }
            }

            Log.Warning("Can not find network channel named '{0}'.", name);
        }

        /// <summary>
        /// 使用预先设置的数据初始化网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        public void InitNetworkChannel(string name)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            for (int i = 0; i < m_NetworkChannels.Length; i++)
            {
                if (m_NetworkChannels[i].Name == name)
                {
                    m_NetworkChannels[i].Initialize();
                    return;
                }
            }

            Log.Warning("Can not find network channel named '{0}'.", name);
        }

        /// <summary>
        /// 初始化网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <param name="networkType">网络类型。</param>
        public void InitNetworkChannel(string name, NetworkType networkType)
        {
            INetworkChannel networkChannel = m_NetworkManager.GetNetworkChannel(name);
            if (networkChannel == null)
            {
                Log.Warning("Can not find network channel named '{0}'.", name);
                return;
            }

            networkChannel.Initialize(networkType);
        }

        /// <summary>
        /// 初始化网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <param name="maxPacketLength">数据包最大字节数。</param>
        public void InitNetworkChannel(string name, int maxPacketLength)
        {
            INetworkChannel networkChannel = m_NetworkManager.GetNetworkChannel(name);
            if (networkChannel == null)
            {
                Log.Warning("Can not find network channel named '{0}'.", name);
                return;
            }

            networkChannel.Initialize(maxPacketLength);
        }

        /// <summary>
        /// 初始化网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <param name="networkType">网络类型。</param>
        /// <param name="maxPacketLength">数据包最大字节数。</param>
        public void InitNetworkChannel(string name, NetworkType networkType, int maxPacketLength)
        {
            INetworkChannel networkChannel = m_NetworkManager.GetNetworkChannel(name);
            if (networkChannel == null)
            {
                Log.Warning("Can not find network channel named '{0}'.", name);
                return;
            }

            networkChannel.Initialize(networkType, maxPacketLength);
        }

        /// <summary>
        /// 使用预先设置的数据连接网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        public void ConnectNetworkChannel(string name)
        {
            ConnectNetworkChannel(name, null);
        }

        /// <summary>
        /// 使用预先设置的数据连接网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void ConnectNetworkChannel(string name, object userData)
        {
            if (name == null)
            {
                name = string.Empty;
            }

            for (int i = 0; i < m_NetworkChannels.Length; i++)
            {
                if (m_NetworkChannels[i].Name == name)
                {
                    m_NetworkChannels[i].Connect(userData);
                    return;
                }
            }

            Log.Warning("Can not find network channel named '{0}'.", name);
        }

        /// <summary>
        /// 使用预先设置的数据连接网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <param name="hostOrIPString">远程主机的名称或 IP 地址字符串。</param>
        /// <param name="port">远程主机的端口号。</param>
        public void ConnectNetworkChannel(string name, string hostOrIPString, int port)
        {
            ConnectNetworkChannel(name, hostOrIPString, port, null);
        }

        /// <summary>
        /// 使用预先设置的数据连接网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <param name="hostOrIPString">远程主机的名称或 IP 地址字符串。</param>
        /// <param name="port">远程主机的端口号。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void ConnectNetworkChannel(string name, string hostOrIPString, int port, object userData)
        {
            if (string.IsNullOrEmpty(hostOrIPString))
            {
                Log.Warning("Host or IP string is invalid.");
                return;
            }

            INetworkChannel networkChannel = m_NetworkManager.GetNetworkChannel(name);
            if (networkChannel == null)
            {
                Log.Warning("Can not find network channel named '{0}'.", name);
                return;
            }

            IPAddress ipAddress = null;
            if (IPAddress.TryParse(hostOrIPString, out ipAddress))
            {
                networkChannel.Connect(ipAddress, port, userData);
            }
            else
            {
                networkChannel.Connect(hostOrIPString, port, userData);
            }
        }

        /// <summary>
        /// 注册网络消息包处理函数。
        /// </summary>
        /// <param name="handler">要注册的网络消息包处理函数。</param>
        public void RegisterHandler(IPacketHandler handler)
        {
            m_NetworkManager.RegisterHandler(handler);
        }

        private void OnNetworkConnected(object sender, GameFramework.Network.NetworkConnectedEventArgs e)
        {
            m_EventComponent.Fire(this, new NetworkConnectedEventArgs(e));
        }

        private void OnNetworkClosed(object sender, GameFramework.Network.NetworkClosedEventArgs e)
        {
            m_EventComponent.Fire(this, new NetworkClosedEventArgs(e));
        }

        private void OnNetworkSendPacket(object sender, GameFramework.Network.NetworkSendPacketEventArgs e)
        {
            m_EventComponent.Fire(this, new NetworkSendPacketEventArgs(e));
        }

        private void OnNetworkMissHeartBeat(object sender, GameFramework.Network.NetworkMissHeartBeatEventArgs e)
        {
            m_EventComponent.Fire(this, new NetworkMissHeartBeatEventArgs(e));
        }

        private void OnNetworkError(object sender, GameFramework.Network.NetworkErrorEventArgs e)
        {
            m_EventComponent.Fire(this, new NetworkErrorEventArgs(e));
        }

        private void OnNetworkCustomError(object sender, GameFramework.Network.NetworkCustomErrorEventArgs e)
        {
            m_EventComponent.Fire(this, new NetworkCustomErrorEventArgs(e));
        }
    }
}
