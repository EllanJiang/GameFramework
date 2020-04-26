//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.DataTable
{
    /// <summary>
    /// 加载数据表更新事件。
    /// </summary>
    public sealed class LoadDataTableUpdateEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载数据表更新事件的新实例。
        /// </summary>
        public LoadDataTableUpdateEventArgs()
        {
            DataTableAssetName = null;
            Progress = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取数据表资源名称。
        /// </summary>
        public string DataTableAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载数据表进度。
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
        /// 创建加载数据表更新事件。
        /// </summary>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="progress">加载数据表进度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的加载数据表更新事件。</returns>
        public static LoadDataTableUpdateEventArgs Create(string dataTableAssetName, float progress, object userData)
        {
            LoadDataTableUpdateEventArgs loadDataTableUpdateEventArgs = ReferencePool.Acquire<LoadDataTableUpdateEventArgs>();
            loadDataTableUpdateEventArgs.DataTableAssetName = dataTableAssetName;
            loadDataTableUpdateEventArgs.Progress = progress;
            loadDataTableUpdateEventArgs.UserData = userData;
            return loadDataTableUpdateEventArgs;
        }

        /// <summary>
        /// 清理加载数据表更新事件。
        /// </summary>
        public override void Clear()
        {
            DataTableAssetName = null;
            Progress = 0f;
            UserData = null;
        }
    }
}
