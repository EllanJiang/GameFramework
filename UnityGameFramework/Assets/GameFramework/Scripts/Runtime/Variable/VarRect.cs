//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// UnityEngine.Rect 变量类。
    /// </summary>
    public class VarRect : Variable<Rect>
    {
        /// <summary>
        /// 初始化 UnityEngine.Rect 变量类的新实例。
        /// </summary>
        public VarRect()
        {

        }

        /// <summary>
        /// 初始化 UnityEngine.Rect 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarRect(Rect value)
            : base(value)
        {

        }

        /// <summary>
        /// 从 UnityEngine.Rect 到 UnityEngine.Rect 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarRect(Rect value)
        {
            return new VarRect(value);
        }

        /// <summary>
        /// 从 UnityEngine.Rect 变量类到 UnityEngine.Rect 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Rect(VarRect value)
        {
            return value.Value;
        }
    }
}
