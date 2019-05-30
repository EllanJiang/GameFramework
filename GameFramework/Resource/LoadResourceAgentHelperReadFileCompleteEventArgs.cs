﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 加载资源代理辅助器异步将资源文件转换为加载对象完成事件。
    /// </summary>
    public sealed class LoadResourceAgentHelperReadFileCompleteEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载资源代理辅助器异步将资源文件转换为加载对象完成事件的新实例。
        /// </summary>
        /// <param name="resource">资源对象。</param>
        public LoadResourceAgentHelperReadFileCompleteEventArgs(object resource)
        {
            Resource = resource;
        }

        /// <summary>
        /// 获取加载对象。
        /// </summary>
        public object Resource
        {
            get;
            private set;
        }
    }
}
