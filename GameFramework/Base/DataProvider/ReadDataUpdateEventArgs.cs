//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework
{
    /// <summary>
    /// 读取数据更新事件。
    /// </summary>
    public sealed class ReadDataUpdateEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化读取数据更新事件的新实例。
        /// </summary>
        public ReadDataUpdateEventArgs()
        {
            DataAssetName = null;
            Progress = 0f;
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
        /// 获取读取数据进度。
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
        /// 创建读取数据更新事件。
        /// </summary>
        /// <param name="dataAssetName">内容资源名称。</param>
        /// <param name="progress">读取数据进度。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的读取数据更新事件。</returns>
        public static ReadDataUpdateEventArgs Create(string dataAssetName, float progress, object userData)
        {
            ReadDataUpdateEventArgs loadDataUpdateEventArgs = ReferencePool.Acquire<ReadDataUpdateEventArgs>();
            loadDataUpdateEventArgs.DataAssetName = dataAssetName;
            loadDataUpdateEventArgs.Progress = progress;
            loadDataUpdateEventArgs.UserData = userData;
            return loadDataUpdateEventArgs;
        }

        /// <summary>
        /// 清理读取数据更新事件。
        /// </summary>
        public override void Clear()
        {
            DataAssetName = null;
            Progress = 0f;
            UserData = null;
        }
    }
}
