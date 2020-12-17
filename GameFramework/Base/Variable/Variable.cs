//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework
{
    /// <summary>
    /// 变量。
    /// </summary>
    public abstract class Variable : IReference
    {
        /// <summary>
        /// 初始化变量的新实例。
        /// </summary>
        public Variable()
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
        /// 清理变量值。
        /// </summary>
        public abstract void Clear();
    }
}
