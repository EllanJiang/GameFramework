//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Localization
{
    /// <summary>
    /// 加载字典失败事件。
    /// </summary>
    public sealed class LoadDictionaryFailureEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载字典失败事件的新实例。
        /// </summary>
        public LoadDictionaryFailureEventArgs()
        {
            DictionaryAssetName = null;
            ErrorMessage = null;
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
        /// 创建加载字典失败事件。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的加载字典失败事件。</returns>
        public static LoadDictionaryFailureEventArgs Create(string dictionaryAssetName, string errorMessage, object userData)
        {
            LoadDictionaryFailureEventArgs loadDictionaryFailureEventArgs = ReferencePool.Acquire<LoadDictionaryFailureEventArgs>();
            loadDictionaryFailureEventArgs.DictionaryAssetName = dictionaryAssetName;
            loadDictionaryFailureEventArgs.ErrorMessage = errorMessage;
            loadDictionaryFailureEventArgs.UserData = userData;
            return loadDictionaryFailureEventArgs;
        }

        /// <summary>
        /// 清理加载字典失败事件。
        /// </summary>
        public override void Clear()
        {
            DictionaryAssetName = null;
            ErrorMessage = null;
            UserData = null;
        }
    }
}
