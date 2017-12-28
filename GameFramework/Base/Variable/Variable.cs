//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework
{
    /// <summary>
    /// 变量。
    /// </summary>
    public abstract class Variable
    {
        /// <summary>
        /// 初始化变量的新实例。
        /// </summary>
        protected Variable()
        {

        }

        /// <summary>
        /// 获取变量类型。
        /// </summary>
        public abstract Type Type
        {
            get;
        }

        /// <summary>
        /// 获取变量值。
        /// </summary>
        /// <returns>变量值。</returns>
        public abstract object GetValue();

        /// <summary>
        /// 设置变量值。
        /// </summary>
        /// <param name="value">变量值。</param>
        public abstract void SetValue(object value);

        /// <summary>
        /// 重置变量值。
        /// </summary>
        public abstract void Reset();
    }
}
