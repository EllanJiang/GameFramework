//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
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
        /// <param name="type">进度类型。</param>
        /// <param name="progress">进度。</param>
        public LoadResourceAgentHelperUpdateEventArgs(LoadResourceProgress type, float progress)
        {
            Type = type;
            Progress = progress;
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
    }
}
