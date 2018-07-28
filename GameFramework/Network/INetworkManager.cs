//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Network
{
    /// <summary>
    /// 网络管理器接口。
    /// </summary>
    public interface INetworkManager
    {
        /// <summary>
        /// 获取网络频道数量。
        /// </summary>
        int NetworkChannelCount
        {
            get;
        }

        /// <summary>
        /// 网络连接成功事件。
        /// </summary>
        event EventHandler<NetworkConnectedEventArgs> NetworkConnected;

        /// <summary>
        /// 网络连接关闭事件。
        /// </summary>
        event EventHandler<NetworkClosedEventArgs> NetworkClosed;

        /// <summary>
        /// 网络心跳包丢失事件。
        /// </summary>
        event EventHandler<NetworkMissHeartBeatEventArgs> NetworkMissHeartBeat;

        /// <summary>
        /// 网络错误事件。
        /// </summary>
        event EventHandler<NetworkErrorEventArgs> NetworkError;

        /// <summary>
        /// 用户自定义网络错误事件。
        /// </summary>
        event EventHandler<NetworkCustomErrorEventArgs> NetworkCustomError;

        /// <summary>
        /// 检查是否存在网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>是否存在网络频道。</returns>
        bool HasNetworkChannel(string name);

        /// <summary>
        /// 获取网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>要获取的网络频道。</returns>
        INetworkChannel GetNetworkChannel(string name);

        /// <summary>
        /// 获取所有网络频道。
        /// </summary>
        /// <returns>所有网络频道。</returns>
        INetworkChannel[] GetAllNetworkChannels();

        /// <summary>
        /// 获取所有网络频道。
        /// </summary>
        /// <param name="results">所有网络频道。</param>
        void GetAllNetworkChannels(List<INetworkChannel> results);

        /// <summary>
        /// 创建网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <param name="networkChannelHelper">网络频道辅助器。</param>
        /// <returns>要创建的网络频道。</returns>
        INetworkChannel CreateNetworkChannel(string name, INetworkChannelHelper networkChannelHelper);

        /// <summary>
        /// 销毁网络频道。
        /// </summary>
        /// <param name="name">网络频道名称。</param>
        /// <returns>是否销毁网络频道成功。</returns>
        bool DestroyNetworkChannel(string name);
    }
}
