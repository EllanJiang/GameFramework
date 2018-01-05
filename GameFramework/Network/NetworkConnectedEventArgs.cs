//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Network
{
    /// <summary>
    /// 网络连接成功事件。
    /// </summary>
    public sealed class NetworkConnectedEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化网络连接成功事件的新实例。
        /// </summary>
        /// <param name="networkChannel">网络频道。</param>
        /// <param name="userData">用户自定义数据。</param>
        public NetworkConnectedEventArgs(INetworkChannel networkChannel, object userData)
        {
            NetworkChannel = networkChannel;
            UserData = userData;
        }

        /// <summary>
        /// 获取网络频道。
        /// </summary>
        public INetworkChannel NetworkChannel
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }
    }
}
