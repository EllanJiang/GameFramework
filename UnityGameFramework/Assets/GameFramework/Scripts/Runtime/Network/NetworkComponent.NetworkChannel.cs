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
            private const int DefaultPacketHeaderLength = 4;
            private const int DefaultMaxPacketLength = 1024 * 32;
            private const float DefaultHeartBeatInterval = 30f;

            private INetworkChannel m_NetworkChannel;

            [SerializeField]
            private string m_Name = string.Empty;

            [SerializeField]
            private string m_IPString = string.Empty;

            [SerializeField]
            private int m_Port = 0;

            [SerializeField]
            private int m_PacketHeaderLength = DefaultPacketHeaderLength;

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
            /// 获取或设置远程主机的 IP 地址字符串。
            /// </summary>
            public string IPString
            {
                get
                {
                    return m_IPString;
                }
                set
                {
                    m_IPString = value;
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

            public void Connect(object userData)
            {
                if (string.IsNullOrEmpty(m_IPString))
                {
                    Log.Warning("IP string is invalid.");
                    return;
                }

                IPAddress ipAddress = null;
                if (!IPAddress.TryParse(m_IPString, out ipAddress))
                {
                    Log.Warning("IP string '{0}' is invalid.", m_IPString);
                    return;
                }

                m_NetworkChannel.Connect(ipAddress, m_Port, m_PacketHeaderLength, m_MaxPacketLength, userData);
            }
        }
    }
}
