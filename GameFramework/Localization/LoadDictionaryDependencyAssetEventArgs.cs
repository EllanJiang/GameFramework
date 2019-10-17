//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Localization
{
    /// <summary>
    /// 加载字典时加载依赖资源事件。
    /// </summary>
    public sealed class LoadDictionaryDependencyAssetEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载字典时加载依赖资源事件的新实例。
        /// </summary>
        public LoadDictionaryDependencyAssetEventArgs()
        {
            DictionaryAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 获取字典资源名称。
        /// </summary>
        public string DictionaryAssetName
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
        /// 创建加载字典时加载依赖资源事件。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="dependencyAssetName">被加载的依赖资源名称。</param>
        /// <param name="loadedCount">当前已加载依赖资源数量。</param>
        /// <param name="totalCount">总共加载依赖资源数量。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的加载字典时加载依赖资源事件。</returns>
        public static LoadDictionaryDependencyAssetEventArgs Create(string dictionaryAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            LoadDictionaryDependencyAssetEventArgs loadDictionaryDependencyAssetEventArgs = ReferencePool.Acquire<LoadDictionaryDependencyAssetEventArgs>();
            loadDictionaryDependencyAssetEventArgs.DictionaryAssetName = dictionaryAssetName;
            loadDictionaryDependencyAssetEventArgs.DependencyAssetName = dependencyAssetName;
            loadDictionaryDependencyAssetEventArgs.LoadedCount = loadedCount;
            loadDictionaryDependencyAssetEventArgs.TotalCount = totalCount;
            loadDictionaryDependencyAssetEventArgs.UserData = userData;
            return loadDictionaryDependencyAssetEventArgs;
        }

        /// <summary>
        /// 清理加载字典时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            DictionaryAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}
