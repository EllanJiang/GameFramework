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
    /// long 变量类。
    /// </summary>
    public class VarLong : Variable<long>
    {
        /// <summary>
        /// 初始化 long 变量类的新实例。
        /// </summary>
        public VarLong()
        {

        }

        /// <summary>
        /// 初始化 long 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarLong(long value)
            : base(value)
        {

        }

        /// <summary>
        /// 从 long 到 long 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarLong(long value)
        {
            return new VarLong(value);
        }

        /// <summary>
        /// 从 long 变量类到 long 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator long(VarLong value)
        {
            return value.Value;
        }
    }
}
