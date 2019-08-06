//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Config
{
    /// <summary>
    /// 加载配置成功事件。
    /// </summary>
    public sealed class LoadConfigSuccessEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载配置成功事件的新实例。
        /// </summary>
        public LoadConfigSuccessEventArgs()
        {
            ConfigAssetName = null;
            LoadType = LoadType.Text;
            Duration = 0f;
            UserData = null;
        }

        /// <summary>
        /// 获取配置资源名称。
        /// </summary>
        public string ConfigAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取配置加载方式。
        /// </summary>
        public LoadType LoadType
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
        /// 创建加载配置成功事件。
        /// </summary>
        /// <param name="configAssetName">配置资源名称。</param>
        /// <param name="loadType">配置加载方式。</param>
        /// <param name="duration">加载持续时间。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>创建的加载配置成功事件。</returns>
        public static LoadConfigSuccessEventArgs Create(string configAssetName, LoadType loadType, float duration, object userData)
        {
            LoadConfigSuccessEventArgs loadConfigSuccessEventArgs = ReferencePool.Acquire<LoadConfigSuccessEventArgs>();
            loadConfigSuccessEventArgs.ConfigAssetName = configAssetName;
            loadConfigSuccessEventArgs.LoadType = loadType;
            loadConfigSuccessEventArgs.Duration = duration;
            loadConfigSuccessEventArgs.UserData = userData;
            return loadConfigSuccessEventArgs;
        }

        /// <summary>
        /// 清理加载配置成功事件。
        /// </summary>
        public override void Clear()
        {
            ConfigAssetName = null;
            LoadType = LoadType.Text;
            Duration = 0f;
            UserData = null;
        }
    }
}
