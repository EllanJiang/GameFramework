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
    /// UnityEngine.Quaternion 变量类。
    /// </summary>
    public class VarQuaternion : Variable<Quaternion>
    {
        /// <summary>
        /// 初始化 UnityEngine.Quaternion 变量类的新实例。
        /// </summary>
        public VarQuaternion()
        {

        }

        /// <summary>
        /// 初始化 UnityEngine.Quaternion 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarQuaternion(Quaternion value)
            : base(value)
        {

        }

        /// <summary>
        /// 从 UnityEngine.Quaternion 到 UnityEngine.Quaternion 变量类的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator VarQuaternion(Quaternion value)
        {
            return new VarQuaternion(value);
        }

        /// <summary>
        /// 从 UnityEngine.Quaternion 变量类到 UnityEngine.Quaternion 的隐式转换。
        /// </summary>
        /// <param name="value">值。</param>
        public static implicit operator Quaternion(VarQuaternion value)
        {
            return value.Value;
        }
    }
}
