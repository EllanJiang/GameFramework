//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Config
{
    /// <summary>
    /// 加载全局配置更新事件。
    /// </summary>
    public sealed class LoadConfigUpdateEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载全局配置更新事件的新实例。
        /// </summary>
        public LoadConfigUpdateEventArgs()
        {
            ConfigAssetName = null;
            LoadType = LoadType.Text;
            Progress = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取全局配置资源名称。
        /// </summary>
        public string ConfigAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取全局配置加载方式。
        /// </summary>
        public LoadType LoadType
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载全局配置进度。
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

        /// <summary>
        /// 创建加载全局配置更新事件。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="loadType">全局配置加载方式。</param>
        /// <param name="progress">加载全局配置进度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的加载全局配置更新事件。</returns>
        public static LoadConfigUpdateEventArgs Create(string configAssetName, LoadType loadType, float progress, object userData)
        {
            LoadConfigUpdateEventArgs loadConfigUpdateEventArgs = ReferencePool.Acquire<LoadConfigUpdateEventArgs>();
            loadConfigUpdateEventArgs.ConfigAssetName = configAssetName;
            loadConfigUpdateEventArgs.LoadType = loadType;
            loadConfigUpdateEventArgs.Progress = progress;
            loadConfigUpdateEventArgs.UserData = userData;
            return loadConfigUpdateEventArgs;
        }

        /// <summary>
        /// 清理加载全局配置更新事件。
        /// </summary>
        public override void Clear()
        {
            ConfigAssetName = null;
            LoadType = LoadType.Text;
            Progress = 0f;
            UserData = null;
        }
    }
}
