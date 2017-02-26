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
    /// float 变量类。
    /// </summary>
    public class VarFloat : Variable<float>
    {
        /// <summary>
        /// 初始化 float 变量类的新实例。
        /// </summary>
        public VarFloat()
        {

        }

        /// <summary>
        /// 初始化 float 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarFloat(float value)
            : base(value)
        {

        }

        /// <summary>
        /// 从 float 到 float 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarFloat(float value)
        {
            return new VarFloat(value);
        }

        /// <summary>
        /// 从 float 变量类到 float 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator float(VarFloat value)
        {
            return value.Value;
        }
    }
}
