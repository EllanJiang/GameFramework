//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// byte 变量类。
    /// </summary>
    public class VarByte : Variable<byte>
    {
        /// <summary>
        /// 初始化 byte 变量类的新实例。
        /// </summary>
        public VarByte()
        {

        }

        /// <summary>
        /// 初始化 byte 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarByte(byte value)
            : base(value)
        {

        }

        /// <summary>
        /// 从 byte 到 byte 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarByte(byte value)
        {
            return new VarByte(value);
        }

        /// <summary>
        /// 从 byte 变量类到 byte 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator byte(VarByte value)
        {
            return value.Value;
        }
    }
}
