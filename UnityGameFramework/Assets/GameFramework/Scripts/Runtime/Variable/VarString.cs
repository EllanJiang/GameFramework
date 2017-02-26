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
    /// string 变量类。
    /// </summary>
    public class VarString : Variable<string>
    {
        /// <summary>
        /// 初始化 string 变量类的新实例。
        /// </summary>
        public VarString()
        {

        }

        /// <summary>
        /// 初始化 string 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarString(string value)
            : base(value)
        {

        }

        /// <summary>
        /// 从 string 到 string 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarString(string value)
        {
            return new VarString(value);
        }

        /// <summary>
        /// 从 string 变量类到 string 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator string(VarString value)
        {
            return value.Value;
        }
    }
}
