//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
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
        /// 状态错误。
        /// </summary>
        StatusError,

        /// <summary>
        /// 序列化错误。
        /// </summary>
        SerializeError,

        /// <summary>
        /// 反序列化错误。
        /// </summary>
        DeserializeError,

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

        /// <summary>
        /// 消息包头错误。
        /// </summary>
        HeaderError,

        /// <summary>
        /// 消息包长度错误。
        /// </summary>
        OutOfRangeError,

        /// <summary>
        /// 消息包流错误。
        /// </summary>
        StreamError,
    }
}
