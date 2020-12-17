//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework
{
    /// <summary>
    /// 读取数据失败事件。
    /// </summary>
    public sealed class ReadDataFailureEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化读取数据失败事件的新实例。
        /// </summary>
        public ReadDataFailureEventArgs()
        {
            DataAssetName = null;
            ErrorMessage = null;
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
        /// 创建读取数据失败事件。
        /// </summary>
        /// <param name="dataAssetName">内容资源名称。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的读取数据失败事件。</returns>
        public static ReadDataFailureEventArgs Create(string dataAssetName, string errorMessage, object userData)
        {
            ReadDataFailureEventArgs loadDataFailureEventArgs = ReferencePool.Acquire<ReadDataFailureEventArgs>();
            loadDataFailureEventArgs.DataAssetName = dataAssetName;
            loadDataFailureEventArgs.ErrorMessage = errorMessage;
            loadDataFailureEventArgs.UserData = userData;
            return loadDataFailureEventArgs;
        }

        /// <summary>
        /// 清理读取数据失败事件。
        /// </summary>
        public override void Clear()
        {
            DataAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
