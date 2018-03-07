//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Config
{
    /// <summary>
    /// 加载配置更新事件。
    /// </summary>
    public sealed class LoadConfigUpdateEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载配置更新事件的新实例。
        /// </summary>
        /// <param name="configAssetName">配置资源名称。</param>
        /// <param name="progress">加载配置进度。</param>
        /// <param name="userData">用户自定义数据。</param>
        public LoadConfigUpdateEventArgs(string configAssetName, float progress, object userData)
        {
            ConfigAssetName = configAssetName;
            Progress = progress;
            UserData = userData;
        }

        /// <summary>
        /// 获取配置资源名称。
        /// </summary>
        public string ConfigAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载配置进度。
        /// </summary>
        public float Progress
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取用户自定义数据。
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }
    }
}
