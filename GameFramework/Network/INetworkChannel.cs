//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Net;

namespace GameFramework.Network
{
    /// <summary>
    /// 网络频道接口。
    /// </summary>
    public interface INetworkChannel
    {
        /// <summary>
        /// 获取网络频道名称。
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// 获取是否已连接。
        /// </summary>
        bool Connected
        {
            get;
        }

        /// <summary>
        /// 获取网络类型。
        /// </summary>
        NetworkType NetworkType
        {
            get;
        }

        /// <summary>
        /// 获取本地终结点的 IP 地址。
        /// </summary>
        IPAddress LocalIPAddress
        {
            get;
        }

        /// <summary>
        /// 获取本地终结点的端口号。
        /// </summary>
        int LocalPort
        {
            get;
        }

        /// <summary>
        /// 获取远程终结点的 IP 地址。
        /// </summary>
        IPAddress RemoteIPAddress
        {
            get;
        }

        /// <summary>
        /// 获取远程终结点的端口号。
        /// </summary>
        int RemotePort
        {
            get;
        }

        /// <summary>
        /// 获取数据包头长度。
        /// </summary>
        int PacketHeaderLength
        {
            get;
        }

        /// <summary>
        /// 获取数据包最大字节数。
        /// </summary>
        int MaxPacketLength
        {
            get;
        }

        /// <summary>
        /// 获取或设置当收到消息包时是否重置心跳流逝时间。
        /// </summary>
        bool ResetHeartBeatElapseSecondsWhenReceivePacket
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置心跳间隔时长，以秒为单位。
        /// </summary>
        float HeartBeatInterval
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置接收缓冲区字节数。
        /// </summary>
        int ReceiveBufferSize
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置发送缓冲区字节数。
        /// </summary>
        int SendBufferSize
        {
            get;
            set;
        }

        /// <summary>
        /// 连接到远程主机。
        /// </summary>
        /// <param name="ipAddress">远程主机的 IP 地址。</param>
        /// <param name="port">远程主机的端口号。</param>
        void Connect(IPAddress ipAddress, int port);

        /// <summary>
        /// 连接到远程主机。
        /// </summary>
        /// <param name="ipAddress">远程主机的 IP 地址。</param>
        /// <param name="port">远程主机的端口号。</param>
        /// <param name="maxPacketLength">数据包最大字节数。</param>
        void Connect(IPAddress ipAddress, int port, int maxPacketLength);

        /// <summary>
        /// 连接到远程主机。
        /// </summary>
        /// <param name="ipAddress">远程主机的 IP 地址。</param>
        /// <param name="port">远程主机的端口号。</param>
        /// <param name="userData">用户自定义数据。</param>
        void Connect(IPAddress ipAddress, int port, object userData);

        /// <summary>
        /// 连接到远程主机。
        /// </summary>
        /// <param name="ipAddress">远程主机的 IP 地址。</param>
        /// <param name="port">远程主机的端口号。</param>
        /// <param name="packetHeaderLength">数据包头长度。</param>
        /// <param name="maxPacketLength">数据包最大字节数。</param>
        void Connect(IPAddress ipAddress, int port, int packetHeaderLength, int maxPacketLength);

        /// <summary>
        /// 连接到远程主机。
        /// </summary>
        /// <param name="ipAddress">远程主机的 IP 地址。</param>
        /// <param name="port">远程主机的端口号。</param>
        /// <param name="maxPacketLength">数据包最大字节数。</param>
        /// <param name="userData">用户自定义数据。</param>
        void Connect(IPAddress ipAddress, int port, int maxPacketLength, object userData);

        /// <summary>
        /// 连接到远程主机。
        /// </summary>
        /// <param name="ipAddress">远程主机的 IP 地址。</param>
        /// <param name="port">远程主机的端口号。</param>
        /// <param name="packetHeaderLength">数据包头长度。</param>
        /// <param name="maxPacketLength">数据包最大字节数。</param>
        /// <param name="userData">用户自定义数据。</param>
        void Connect(IPAddress ipAddress, int port, int packetHeaderLength, int maxPacketLength, object userData);

        /// <summary>
        /// 关闭网络频道。
        /// </summary>
        void Close();

        /// <summary>
        /// 向远程主机发送数据包。
        /// </summary>
        /// <param name="buffer">数据包流。</param>
        void Send(byte[] buffer);

        /// <summary>
        /// 向远程主机发送数据包。
        /// </summary>
        /// <param name="buffer">数据包流。</param>
        /// <param name="userData">用户自定义数据。</param>
        void Send(byte[] buffer, object userData);

        /// <summary>
        /// 向远程主机发送数据包。
        /// </summary>
        /// <param name="buffer">数据包流。</param>
        /// <param name="offset">要发送数据包的偏移。</param>
        /// <param name="size">要发送数据包的长度。</param>
        void Send(byte[] buffer, int offset, int size);

        /// <summary>
        /// 向远程主机发送数据包。
        /// </summary>
        /// <param name="buffer">数据包流。</param>
        /// <param name="offset">要发送数据包的偏移。</param>
        /// <param name="size">要发送数据包的长度。</param>
        /// <param name="userData">用户自定义数据。</param>
        void Send(byte[] buffer, int offset, int size, object userData);

        /// <summary>
        /// 向远程主机发送数据包。
        /// </summary>
        /// <typeparam name="T">数据包类型。</typeparam>
        /// <param name="packet">要发送的数据包。</param>
        void Send<T>(T packet) where T : Packet;

        /// <summary>
        /// 向远程主机发送数据包。
        /// </summary>
        /// <typeparam name="T">数据包类型。</typeparam>
        /// <param name="packet">要发送的数据包。</param>
        /// <param name="userData">用户自定义数据。</param>
        void Send<T>(T packet, object userData) where T : Packet;
    }
}
