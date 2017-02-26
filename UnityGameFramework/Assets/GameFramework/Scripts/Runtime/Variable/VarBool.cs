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
    /// bool 变量类。
    /// </summary>
    public class VarBool : Variable<bool>
    {
        /// <summary>
        /// 初始化 bool 变量类的新实例。
        /// </summary>
        public VarBool()
        {

        }

        /// <summary>
        /// 初始化 bool 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarBool(bool value)
            : base(value)
        {

        }

        /// <summary>
        /// 从 bool 到 bool 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarBool(bool value)
        {
            return new VarBool(value);
        }

        /// <summary>
        /// 从 bool 变量类到 bool 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator bool(VarBool value)
        {
            return value.Value;
        }
    }
}
