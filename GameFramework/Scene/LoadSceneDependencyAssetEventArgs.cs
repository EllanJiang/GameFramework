//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Scene
{
    /// <summary>
    /// 加载场景时加载依赖资源事件。
    /// </summary>
    public sealed class LoadSceneDependencyAssetEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载场景时加载依赖资源事件的新实例。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="dependencyAssetName">被加载的依赖资源名称。</param>
        /// <param name="loadedCount">当前已加载依赖资源数量。</param>
        /// <param name="totalCount">总共加载依赖资源数量。</param>
        /// <param name="userData">用户自定义数据。</param>
        public LoadSceneDependencyAssetEventArgs(string sceneAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            SceneAssetName = sceneAssetName;
            DependencyAssetName = dependencyAssetName;
            LoadedCount = loadedCount;
            TotalCount = totalCount;
            UserData = userData;
        }

        /// <summary>
        /// 获取场景资源名称。
        /// </summary>
        public string SceneAssetName
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
    }
}
