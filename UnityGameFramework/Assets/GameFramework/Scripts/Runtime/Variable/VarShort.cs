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
    /// short 变量类。
    /// </summary>
    public class VarShort : Variable<short>
    {
        /// <summary>
        /// 初始化 short 变量类的新实例。
        /// </summary>
        public VarShort()
        {

        }

        /// <summary>
        /// 初始化 short 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarShort(short value)
            : base(value)
        {

        }

        /// <summary>
        /// 从 short 到 short 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarShort(short value)
        {
            return new VarShort(value);
        }

        /// <summary>
        /// 从 short 变量类到 short 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator short(VarShort value)
        {
            return value.Value;
        }
    }
}
