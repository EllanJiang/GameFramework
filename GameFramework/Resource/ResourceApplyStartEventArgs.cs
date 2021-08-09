//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源应用开始事件。
    /// </summary>
    public sealed class ResourceApplyStartEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化资源应用开始事件的新实例。
        /// </summary>
        public ResourceApplyStartEventArgs()
        {
            ResourcePackPath = null;
            Count = 0;
            TotalLength = 0L;
        }

        /// <summary>
        /// 获取资源包路径。
        /// </summary>
        public string ResourcePackPath
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取要应用资源的数量。
        /// </summary>
        public int Count
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取要应用资源的总大小。
        /// </summary>
        public long TotalLength
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建资源应用开始事件。
        /// </summary>
        /// <param name="resourcePackPath">资源包路径。</param>
        /// <param name="count">要应用资源的数量。</param>
        /// <param name="totalLength">要应用资源的总大小。</param>
        /// <returns>创建的资源应用开始事件。</returns>
        public static ResourceApplyStartEventArgs Create(string resourcePackPath, int count, long totalLength)
        {
            ResourceApplyStartEventArgs resourceApplyStartEventArgs = ReferencePool.Acquire<ResourceApplyStartEventArgs>();
            resourceApplyStartEventArgs.ResourcePackPath = resourcePackPath;
            resourceApplyStartEventArgs.Count = count;
            resourceApplyStartEventArgs.TotalLength = totalLength;
            return resourceApplyStartEventArgs;
        }

        /// <summary>
        /// 清理资源应用开始事件。
        /// </summary>
        public override void Clear()
        {
            ResourcePackPath = null;
            Count = 0;
            TotalLength = 0L;
        }
    }
}
