//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源校验开始事件。
    /// </summary>
    public sealed class ResourceVerifyStartEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化资源校验开始事件的新实例。
        /// </summary>
        public ResourceVerifyStartEventArgs()
        {
            Count = 0;
            TotalLength = 0L;
        }

        /// <summary>
        /// 获取要校验资源的数量。
        /// </summary>
        public int Count
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取要校验资源的总大小。
        /// </summary>
        public long TotalLength
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建资源校验开始事件。
        /// </summary>
        /// <param name="count">要校验资源的数量。</param>
        /// <param name="totalLength">要校验资源的总大小。</param>
        /// <returns>创建的资源校验开始事件。</returns>
        public static ResourceVerifyStartEventArgs Create(int count, long totalLength)
        {
            ResourceVerifyStartEventArgs resourceVerifyStartEventArgs = ReferencePool.Acquire<ResourceVerifyStartEventArgs>();
            resourceVerifyStartEventArgs.Count = count;
            resourceVerifyStartEventArgs.TotalLength = totalLength;
            return resourceVerifyStartEventArgs;
        }

        /// <summary>
        /// 清理资源校验开始事件。
        /// </summary>
        public override void Clear()
        {
            Count = 0;
            TotalLength = 0L;
        }
    }
}
