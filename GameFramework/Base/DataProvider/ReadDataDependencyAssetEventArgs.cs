//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework
{
    /// <summary>
    /// 读取数据时加载依赖资源事件。
    /// </summary>
    public sealed class ReadDataDependencyAssetEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化读取数据时加载依赖资源事件的新实例。
        /// </summary>
        public ReadDataDependencyAssetEventArgs()
        {
            DataAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }

        /// <summary>
        /// 获取内容资源名称。
        /// </summary>
        public string DataAssetName
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
        /// 创建读取数据时加载依赖资源事件。
        /// </summary>
        /// <param name="dataAssetName">内容资源名称。</param>
        /// <param name="dependencyAssetName">被加载的依赖资源名称。</param>
        /// <param name="loadedCount">当前已加载依赖资源数量。</param>
        /// <param name="totalCount">总共加载依赖资源数量。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的读取数据时加载依赖资源事件。</returns>
        public static ReadDataDependencyAssetEventArgs Create(string dataAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            ReadDataDependencyAssetEventArgs loadDataDependencyAssetEventArgs = ReferencePool.Acquire<ReadDataDependencyAssetEventArgs>();
            loadDataDependencyAssetEventArgs.DataAssetName = dataAssetName;
            loadDataDependencyAssetEventArgs.DependencyAssetName = dependencyAssetName;
            loadDataDependencyAssetEventArgs.LoadedCount = loadedCount;
            loadDataDependencyAssetEventArgs.TotalCount = totalCount;
            loadDataDependencyAssetEventArgs.UserData = userData;
            return loadDataDependencyAssetEventArgs;
        }

        /// <summary>
        /// 清理读取数据时加载依赖资源事件。
        /// </summary>
        public override void Clear()
        {
            DataAssetName = null;
            DependencyAssetName = null;
            LoadedCount = 0;
            TotalCount = 0;
            UserData = null;
        }
    }
}
