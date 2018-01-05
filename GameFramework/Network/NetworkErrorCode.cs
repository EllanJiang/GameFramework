//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Network
{
    /// <summary>
    /// 网络错误码。
    /// </summary>
    public enum NetworkErrorCode
    {
        /// <summary>
        /// 地址族错误。
        /// </summary>
        AddressFamilyError,

        /// <summary>
        /// Socket 错误。
        /// </summary>
        SocketError,

        /// <summary>
        /// 序列化错误。
        /// </summary>
        SerializeError,

        /// <summary>
        /// 反序列化消息包头错误。
        /// </summary>
        DeserializePacketHeaderError,

        /// <summary>
        /// 反序列化消息包错误。
        /// </summary>
        DeserializePacketError,

        /// <summary>
        /// 连接错误。
        /// </summary>
        ConnectError,

        /// <summary>
        /// 发送错误。
        /// </summary>
        SendError,

        /// <summary>
        /// 接收错误。
        /// </summary>
        ReceiveError,
    }
}
