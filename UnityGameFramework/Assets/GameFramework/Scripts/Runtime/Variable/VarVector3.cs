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
    /// UnityEngine.Vector3 变量类。
    /// </summary>
    public class VarVector3 : Variable<Vector3>
    {
        /// <summary>
        /// 初始化 UnityEngine.Vector3 变量类的新实例。
        /// </summary>
        public VarVector3()
        {

        }

        /// <summary>
        /// 初始化 UnityEngine.Vector3 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarVector3(Vector3 value)
            : base(value)
        {

        }

        /// <summary>
        /// 从 UnityEngine.Vector3 到 UnityEngine.Vector3 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarVector3(Vector3 value)
        {
            return new VarVector3(value);
        }

        /// <summary>
        /// 从 UnityEngine.Vector3 变量类到 UnityEngine.Vector3 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Vector3(VarVector3 value)
        {
            return value.Value;
        }
    }
}
