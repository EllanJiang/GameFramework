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
    /// UnityEngine.Material 变量类。
    /// </summary>
    public class VarMaterial : Variable<Material>
    {
        /// <summary>
        /// 初始化 UnityEngine.Material 变量类的新实例。
        /// </summary>
        public VarMaterial()
        {

        }

        /// <summary>
        /// 初始化 UnityEngine.Material 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarMaterial(Material value)
            : base(value)
        {

        }

        /// <summary>
        /// 从 UnityEngine.Material 到 UnityEngine.Material 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarMaterial(Material value)
        {
            return new VarMaterial(value);
        }

        /// <summary>
        /// 从 UnityEngine.Material 变量类到 UnityEngine.Material 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Material(VarMaterial value)
        {
            return value.Value;
        }
    }
}
