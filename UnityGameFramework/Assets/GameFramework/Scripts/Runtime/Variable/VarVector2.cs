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
    /// UnityEngine.Vector2 变量类。
    /// </summary>
    public class VarVector2 : Variable<Vector2>
    {
        /// <summary>
        /// 初始化 UnityEngine.Vector2 变量类的新实例。
        /// </summary>
        public VarVector2()
        {

        }

        /// <summary>
        /// 初始化 UnityEngine.Vector2 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarVector2(Vector2 value)
            : base(value)
        {

        }

        /// <summary>
        /// 从 UnityEngine.Vector2 到 UnityEngine.Vector2 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarVector2(Vector2 value)
        {
            return new VarVector2(value);
        }

        /// <summary>
        /// 从 UnityEngine.Vector2 变量类到 UnityEngine.Vector2 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Vector2(VarVector2 value)
        {
            return value.Value;
        }
    }
}
