//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Config
{
    /// <summary>
    /// 加载全局配置成功事件。
    /// </summary>
    public sealed class LoadConfigSuccessEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载全局配置成功事件的新实例。
        /// </summary>
        public LoadConfigSuccessEventArgs()
        {
            ConfigAssetName = null;
            Duration = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取全局配置资源名称。
        /// </summary>
        public string ConfigAssetName
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
        /// 创建加载全局配置成功事件。
        /// </summary>
        /// <param name="configAssetName">全局配置资源名称。</param>
        /// <param name="duration">加载持续时间。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的加载全局配置成功事件。</returns>
        public static LoadConfigSuccessEventArgs Create(string configAssetName, float duration, object userData)
        {
            LoadConfigSuccessEventArgs loadConfigSuccessEventArgs = ReferencePool.Acquire<LoadConfigSuccessEventArgs>();
            loadConfigSuccessEventArgs.ConfigAssetName = configAssetName;
            loadConfigSuccessEventArgs.Duration = duration;
            loadConfigSuccessEventArgs.UserData = userData;
            return loadConfigSuccessEventArgs;
        }

        /// <summary>
        /// 清理加载全局配置成功事件。
        /// </summary>
        public override void Clear()
        {
            ConfigAssetName = null;
            Duration = 0f;
            UserData = null;
        }
    }
}
