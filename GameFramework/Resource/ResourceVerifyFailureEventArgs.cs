//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源校验失败事件。
    /// </summary>
    public sealed class ResourceVerifyFailureEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化资源校验失败事件的新实例。
        /// </summary>
        public ResourceVerifyFailureEventArgs()
        {
            Name = null;
        }

        /// <summary>
        /// 获取资源名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建资源校验失败事件。
        /// </summary>
        /// <param name="name">资源名称。</param>
        /// <returns>创建的资源校验失败事件。</returns>
        public static ResourceVerifyFailureEventArgs Create(string name)
        {
            ResourceVerifyFailureEventArgs resourceVerifyFailureEventArgs = ReferencePool.Acquire<ResourceVerifyFailureEventArgs>();
            resourceVerifyFailureEventArgs.Name = name;
            return resourceVerifyFailureEventArgs;
        }

        /// <summary>
        /// 清理资源校验失败事件。
        /// </summary>
        public override void Clear()
        {
            Name = null;
        }
    }
}
