//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源校验成功事件。
    /// </summary>
    public sealed class ResourceVerifySuccessEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化资源校验成功事件的新实例。
        /// </summary>
        public ResourceVerifySuccessEventArgs()
        {
            Name = null;
            Length = 0;
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
        /// 获取资源大小。
        /// </summary>
        public int Length
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建资源校验成功事件。
        /// </summary>
        /// <param name="name">资源名称。</param>
        /// <param name="length">资源大小。</param>
        /// <returns>创建的资源校验成功事件。</returns>
        public static ResourceVerifySuccessEventArgs Create(string name, int length)
        {
            ResourceVerifySuccessEventArgs resourceVerifySuccessEventArgs = ReferencePool.Acquire<ResourceVerifySuccessEventArgs>();
            resourceVerifySuccessEventArgs.Name = name;
            resourceVerifySuccessEventArgs.Length = length;
            return resourceVerifySuccessEventArgs;
        }

        /// <summary>
        /// 清理资源校验成功事件。
        /// </summary>
        public override void Clear()
        {
            Name = null;
            Length = 0;
        }
    }
}
