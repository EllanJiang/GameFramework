//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源应用失败事件。
    /// </summary>
    public sealed class ResourceApplyFailureEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化资源应用失败事件的新实例。
        /// </summary>
        public ResourceApplyFailureEventArgs()
        {
            Name = null;
            ResourcePackPath = null;
            ErrorMessage = null;
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
        /// 获取资源包路径。
        /// </summary>
        public string ResourcePackPath
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建资源应用失败事件。
        /// </summary>
        /// <param name="name">资源名称。</param>
        /// <param name="resourcePackPath">资源包路径。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <returns>创建的资源应用失败事件。</returns>
        public static ResourceApplyFailureEventArgs Create(string name, string resourcePackPath, string errorMessage)
        {
            ResourceApplyFailureEventArgs resourceApplyFailureEventArgs = ReferencePool.Acquire<ResourceApplyFailureEventArgs>();
            resourceApplyFailureEventArgs.Name = name;
            resourceApplyFailureEventArgs.ResourcePackPath = resourcePackPath;
            resourceApplyFailureEventArgs.ErrorMessage = errorMessage;
            return resourceApplyFailureEventArgs;
        }

        /// <summary>
        /// 清理资源应用失败事件。
        /// </summary>
        public override void Clear()
        {
            Name = null;
            ResourcePackPath = null;
            ErrorMessage = null;
        }
    }
}
