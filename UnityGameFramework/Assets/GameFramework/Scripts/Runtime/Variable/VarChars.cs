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
    /// char[] 变量类。
    /// </summary>
    public class VarChars : Variable<char[]>
    {
        /// <summary>
        /// 初始化 char[] 变量类的新实例。
        /// </summary>
        public VarChars()
        {

        }

        /// <summary>
        /// 初始化 char[] 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarChars(char[] value)
            : base(value)
        {

        }

        /// <summary>
        /// 从 char[] 到 char[] 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarChars(char[] value)
        {
            return new VarChars(value);
        }

        /// <summary>
        /// 从 char[] 变量类到 char[] 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator char[] (VarChars value)
        {
            return value.Value;
        }
    }
}
