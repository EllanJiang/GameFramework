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
    /// sbyte 变量类。
    /// </summary>
    public class VarSByte : Variable<sbyte>
    {
        /// <summary>
        /// 初始化 sbyte 变量类的新实例。
        /// </summary>
        public VarSByte()
        {

        }

        /// <summary>
        /// 初始化 sbyte 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarSByte(sbyte value)
            : base(value)
        {

        }

        /// <summary>
        /// 从 sbyte 到 sbyte 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarSByte(sbyte value)
        {
            return new VarSByte(value);
        }

        /// <summary>
        /// 从 sbyte 变量类到 sbyte 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator sbyte(VarSByte value)
        {
            return value.Value;
        }
    }
}
