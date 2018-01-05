//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Network
{
    /// <summary>
    /// 网络消息包处理器接口。
    /// </summary>
    public interface IPacketHandler
    {
        /// <summary>
        /// 获取网络消息包协议编号。
        /// </summary>
        int Id
        {
            get;
        }

        /// <summary>
        /// 网络消息包处理函数。
        /// </summary>
        /// <param name="sender">网络消息包源。</param>
        /// <param name="packet">网络消息包内容。</param>
        void Handle(object sender, Packet packet);
    }
}
