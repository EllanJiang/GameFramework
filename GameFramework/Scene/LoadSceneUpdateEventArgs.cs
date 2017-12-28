//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Scene
{
    /// <summary>
    /// 加载场景更新事件。
    /// </summary>
    public sealed class LoadSceneUpdateEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化加载场景更新事件的新实例。
        /// </summary>
        /// <param name="sceneAssetName">场景资源名称。</param>
        /// <param name="progress">加载场景进度。</param>
        /// <param name="userData">用户自定义数据。</param>
        public LoadSceneUpdateEventArgs(string sceneAssetName, float progress, object userData)
        {
            SceneAssetName = sceneAssetName;
            Progress = progress;
            UserData = userData;
        }

        /// <summary>
        /// 获取场景资源名称。
        /// </summary>
        public string SceneAssetName
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取加载场景进度。
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
