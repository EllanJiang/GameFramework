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
    /// ushort 变量类。
    /// </summary>
    public class VarUShort : Variable<ushort>
    {
        /// <summary>
        /// 初始化 ushort 变量类的新实例。
        /// </summary>
        public VarUShort()
        {

        }

        /// <summary>
        /// 初始化 ushort 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarUShort(ushort value)
            : base(value)
        {

        }

        /// <summary>
        /// 从 ushort 到 ushort 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarUShort(ushort value)
        {
            return new VarUShort(value);
        }

        /// <summary>
        /// 从 short 变量类到 short 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator ushort(VarUShort value)
        {
            return value.Value;
        }
    }
}
