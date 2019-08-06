//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Config
{
    /// <summary>
    /// 加载配置时加载依赖资源事件。
    /// </summary>
    public sealed class LoadConfigDependencyAssetEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载配置时加载依赖资源事件的新实例。
        /// </summary>
        public LoadConfigDependencyAssetEventArgs()
        {
            ConfigAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
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
        /// 获取被加载的依赖资源名称。
        /// </summary>
        public string DependencyAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取当前已加载依赖资源数量。
        /// </summary>
        public int LoadedCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取总共加载依赖资源数量。
        /// </summary>
        public int TotalCount
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
        /// 创建加载配置时加载依赖资源事件。
        /// </summary>
        /// <param name="configAssetName">配置资源名称。</param>
        /// <param name="dependencyAssetName">被加载的依赖资源名称。</param>
        /// <param name="loadedCount">当前已加载依赖资源数量。</param>
        /// <param name="totalCount">总共加载依赖资源数量。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的加载配置时加载依赖资源事件。</returns>
        public static LoadConfigDependencyAssetEventArgs Create(string configAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            LoadConfigDependencyAssetEventArgs loadConfigDependencyAssetEventArgs = ReferencePool.Acquire<LoadConfigDependencyAssetEventArgs>();
            loadConfigDependencyAssetEventArgs.ConfigAssetName = configAssetName;
            loadConfigDependencyAssetEventArgs.DependencyAssetName = dependencyAssetName;
            loadConfigDependencyAssetEventArgs.LoadedCount = loadedCount;
            loadConfigDependencyAssetEventArgs.TotalCount = totalCount;
            loadConfigDependencyAssetEventArgs.UserData = userData;
            return loadConfigDependencyAssetEventArgs;
        }

        /// <summary>
        /// 清理加载配置时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            ConfigAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}
