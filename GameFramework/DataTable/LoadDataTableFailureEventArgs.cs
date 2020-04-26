//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.DataTable
{
    /// <summary>
    /// 加载数据表失败事件。
    /// </summary>
    public sealed class LoadDataTableFailureEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载数据表失败事件的新实例。
        /// </summary>
        public LoadDataTableFailureEventArgs()
        {
            DataTableAssetName = null;
            ErrorMessage = null;
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
        /// 获取错误信息。
        /// </summary>
        public string ErrorMessage
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
        /// 创建加载数据表失败事件。
        /// </summary>
        /// <param name="dataTableAssetName">数据表资源名称。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的加载数据表失败事件。</returns>
        public static LoadDataTableFailureEventArgs Create(string dataTableAssetName, string errorMessage, object userData)
        {
            LoadDataTableFailureEventArgs loadDataTableFailureEventArgs = ReferencePool.Acquire<LoadDataTableFailureEventArgs>();
            loadDataTableFailureEventArgs.DataTableAssetName = dataTableAssetName;
            loadDataTableFailureEventArgs.ErrorMessage = errorMessage;
            loadDataTableFailureEventArgs.UserData = userData;
            return loadDataTableFailureEventArgs;
        }

        /// <summary>
        /// 清理加载数据表失败事件。
        /// </summary>
        public override void Clear()
        {
            DataTableAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
