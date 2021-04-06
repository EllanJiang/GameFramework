//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源更新全部完成事件。
    /// </summary>
    public sealed class ResourceUpdateAllCompleteEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化资源更新全部完成事件的新实例。
        /// </summary>
        public ResourceUpdateAllCompleteEventArgs()
        {
        }

        /// <summary>
        /// 创建资源更新全部完成事件。
        /// </summary>
        /// <returns>创建的资源更新全部完成事件。</returns>
        public static ResourceUpdateAllCompleteEventArgs Create()
        {
            return ReferencePool.Acquire<ResourceUpdateAllCompleteEventArgs>();
        }

        /// <summary>
        /// 清理资源更新全部完成事件。
        /// </summary>
        public override void Clear()
        {
        }
    }
}
