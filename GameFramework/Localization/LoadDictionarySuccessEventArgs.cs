//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Localization
{
    /// <summary>
    /// 加载字典成功事件。
    /// </summary>
    public sealed class LoadDictionarySuccessEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载字典成功事件的新实例。
        /// </summary>
        public LoadDictionarySuccessEventArgs()
        {
            DictionaryAssetName = null;
            Duration = 0f;
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
        /// 获取加载持续时间。
        /// </summary>
        public float Duration
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
        /// 创建加载字典成功事件。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="duration">加载持续时间。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的加载字典成功事件。</returns>
        public static LoadDictionarySuccessEventArgs Create(string dictionaryAssetName, float duration, object userData)
        {
            LoadDictionarySuccessEventArgs loadDictionarySuccessEventArgs = ReferencePool.Acquire<LoadDictionarySuccessEventArgs>();
            loadDictionarySuccessEventArgs.DictionaryAssetName = dictionaryAssetName;
            loadDictionarySuccessEventArgs.Duration = duration;
            loadDictionarySuccessEventArgs.UserData = userData;
            return loadDictionarySuccessEventArgs;
        }

        /// <summary>
        /// 清理加载字典成功事件。
        /// </summary>
        public override void Clear()
        {
            DictionaryAssetName = null;
            Duration = 0f;
            UserData = null;
        }
    }
}
