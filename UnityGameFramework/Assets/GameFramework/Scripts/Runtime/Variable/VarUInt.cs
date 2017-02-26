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
    /// uint 变量类。
    /// </summary>
    public class VarUInt : Variable<uint>
    {
        /// <summary>
        /// 初始化 uint 变量类的新实例。
        /// </summary>
        public VarUInt()
        {

        }

        /// <summary>
        /// 初始化 uint 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarUInt(uint value)
            : base(value)
        {

        }

        /// <summary>
        /// 从 uint 到 uint 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarUInt(uint value)
        {
            return new VarUInt(value);
        }

        /// <summary>
        /// 从 uint 变量类到 uint 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator uint(VarUInt value)
        {
            return value.Value;
        }
    }
}
