//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;
using GameFramework.Network;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 用户自定义网络错误事件。
    /// </summary>
    public sealed class NetworkCustomErrorEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化用户自定义网络错误事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public NetworkCustomErrorEventArgs(GameFramework.Network.NetworkCustomErrorEventArgs e)
        {
            NetworkChannel = e.NetworkChannel;
            CustomErrorData = e.CustomErrorData;
        }

        /// <summary>
        /// 获取用户自定义网络错误事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.NetworkCustomError;
            }
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
        /// 获取用户自定义错误数据。
        /// </summary>
        public object CustomErrorData
        {
            get;
            private set;
        }
    }
}
