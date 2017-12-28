//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
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
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="errorMessage">错误信息。</param>
        /// <param name="userData">用户自定义数据。</param>
        public LoadDictionaryFailureEventArgs(string dictionaryAssetName, string errorMessage, object userData)
        {
            DictionaryAssetName = dictionaryAssetName;
            ErrorMessage = errorMessage;
            UserData = userData;
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
    }
}
