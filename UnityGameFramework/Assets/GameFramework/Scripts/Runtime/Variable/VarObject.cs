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
    /// object 变量类。
    /// </summary>
    public class VarObject : Variable<object>
    {
        /// <summary>
        /// 初始化 object 变量类的新实例。
        /// </summary>
        public VarObject()
        {

        }

        /// <summary>
        /// 初始化 object 变量类的新实例。
        /// </summary>
        /// <param name="value">值。</param>
        public VarObject(object value)
            : base(value)
        {

        }
    }
}
