//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Network;
using System;
using System.Net;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public partial class NetworkComponent
    {
        /// <summary>
        /// 网络频道。
        /// </summary>
        [Serializable]
        private class NetworkChannel
        {
            private const int DefaultMaxPacketLength = 1024 * 32;
            private const float DefaultHeartBeatInterval = 30f;

            private INetworkChannel m_NetworkChannel;

            [SerializeField]
            private string m_Name = string.Empty;

            [SerializeField]
            private bool m_UseIPv6 = false;

            [SerializeField]
            private string m_HostOrIPString = string.Empty;

            [SerializeField]
            private int m_Port = 0;

            [SerializeField]
            private int m_MaxPacketLength = DefaultMaxPacketLength;

            [SerializeField]
            private bool m_ResetHeartBeatElapseSecondsWhenReceivePacket = false;

            [SerializeField]
            private float m_HeartBeatInterval = DefaultHeartBeatInterval;

            /// <summary>
            /// 初始化网络频道的新实例。
            /// </summary>
            public NetworkChannel()
            {
                m_NetworkChannel = null;
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
            /// 获取或设置是否使用 IP 版本 6。
            /// </summary>
            public bool UseIPv6
            {
                get
                {
                    return m_UseIPv6;
                }
                set
                {
                    m_UseIPv6 = value;
                }
            }

            /// <summary>
            /// 获取或设置远程主机的名称或 IP 地址字符串。
            /// </summary>
            public string HostOrIPString
            {
                get
                {
                    return m_HostOrIPString;
                }
                set
                {
                    m_HostOrIPString = value;
                }
            }

            /// <summary>
            /// 获取或设置远程主机的端口号。
            /// </summary>
            public int Port
            {
                get
                {
                    return m_Port;
                }
                set
                {
                    m_Port = value;
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
            /// 获取当收到消息包时是否重置心跳流逝时间。
            /// </summary>
            public bool ResetHeartBeatElapseSecondsWhenReceivePacket
            {
                get
                {
                    return m_ResetHeartBeatElapseSecondsWhenReceivePacket;
                }
            }

            /// <summary>
            /// 获取心跳间隔时长，以秒为单位。
            /// </summary>
            public float HeartBeatInterval
            {
                get
                {
                    return m_HeartBeatInterval;
                }
            }

            public void SetNetworkChannel(INetworkChannel networkChannel)
            {
                if (networkChannel == null)
                {
                    Log.Error("Network channel is invalid.");
                    return;
                }

                m_NetworkChannel = networkChannel;
                m_NetworkChannel.ResetHeartBeatElapseSecondsWhenReceivePacket = m_ResetHeartBeatElapseSecondsWhenReceivePacket;
                m_NetworkChannel.HeartBeatInterval = m_HeartBeatInterval;
            }

            public void Initialize()
            {
                m_NetworkChannel.Initialize(m_UseIPv6 ? NetworkType.IPv6 : NetworkType.IPv4, m_MaxPacketLength);
            }

            public void Connect(object userData)
            {
                if (string.IsNullOrEmpty(m_HostOrIPString))
                {
                    Log.Warning("Host or IP string is invalid.");
                    return;
                }

                IPAddress ipAddress = null;
                if (IPAddress.TryParse(m_HostOrIPString, out ipAddress))
                {
                    m_NetworkChannel.Connect(ipAddress, m_Port, userData);
                }
                else
                {
                    m_NetworkChannel.Connect(m_HostOrIPString, m_Port, userData);
                }
            }
        }
    }
}
