//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 加载资源代理辅助器更新事件。
    /// </summary>
    public sealed class LoadResourceAgentHelperUpdateEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载资源代理辅助器更新事件的新实例。
        /// </summary>
        public LoadResourceAgentHelperUpdateEventArgs()
        {
            Type = LoadResourceProgress.Unknown;
            Progress = 0f;
        }

        /// <summary>
        /// 获取进度类型。
        /// </summary>
        public LoadResourceProgress Type
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取进度。
        /// </summary>
        public float Progress
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建加载资源代理辅助器更新事件。
        /// </summary>
        /// <param name="type">进度类型。</param>
        /// <param name="progress">进度。</param>
        /// <returns>创建的加载资源代理辅助器更新事件。</returns>
        public static LoadResourceAgentHelperUpdateEventArgs Create(LoadResourceProgress type, float progress)
        {
            LoadResourceAgentHelperUpdateEventArgs loadResourceAgentHelperUpdateEventArgs = ReferencePool.Acquire<LoadResourceAgentHelperUpdateEventArgs>();
            loadResourceAgentHelperUpdateEventArgs.Type = type;
            loadResourceAgentHelperUpdateEventArgs.Progress = progress;
            return loadResourceAgentHelperUpdateEventArgs;
        }

        /// <summary>
        /// 清理加载资源代理辅助器更新事件。
        /// </summary>
        public override void Clear()
        {
            Type = LoadResourceProgress.Unknown;
            Progress = 0f;
        }
    }
}
