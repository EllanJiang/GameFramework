//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Localization
{
    /// <summary>
    /// 加载字典更新事件。
    /// </summary>
    public sealed class LoadDictionaryUpdateEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载字典更新事件的新实例。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="progress">加载字典进度。</param>
        /// <param name="userData">用户自定义数据。</param>
        public LoadDictionaryUpdateEventArgs(string dictionaryAssetName, float progress, object userData)
        {
            DictionaryAssetName = dictionaryAssetName;
            Progress = progress;
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
        /// 获取加载字典进度。
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
    }
}
