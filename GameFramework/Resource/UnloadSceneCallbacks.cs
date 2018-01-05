//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 卸载场景回调函数集。
    /// </summary>
    public sealed class UnloadSceneCallbacks
    {
        private readonly UnloadSceneSuccessCallback m_UnloadSceneSuccessCallback;
        private readonly UnloadSceneFailureCallback m_UnloadSceneFailureCallback;

        /// <summary>
        /// 初始化卸载场景回调函数集的新实例。
        /// </summary>
        /// <param name="unloadSceneSuccessCallback">卸载场景成功回调函数。</param>
        public UnloadSceneCallbacks(UnloadSceneSuccessCallback unloadSceneSuccessCallback)
            : this(unloadSceneSuccessCallback, null)
        {

        }

        /// <summary>
        /// 初始化卸载场景回调函数集的新实例。
        /// </summary>
        /// <param name="unloadSceneSuccessCallback">卸载场景成功回调函数。</param>
        /// <param name="unloadSceneFailureCallback">卸载场景失败回调函数。</param>
        public UnloadSceneCallbacks(UnloadSceneSuccessCallback unloadSceneSuccessCallback, UnloadSceneFailureCallback unloadSceneFailureCallback)
        {
            if (unloadSceneSuccessCallback == null)
            {
                throw new GameFrameworkException("Unload scene success callback is invalid.");
            }

            m_UnloadSceneSuccessCallback = unloadSceneSuccessCallback;
            m_UnloadSceneFailureCallback = unloadSceneFailureCallback;
        }

        /// <summary>
        /// 获取卸载场景成功回调函数。
        /// </summary>
        public UnloadSceneSuccessCallback UnloadSceneSuccessCallback
        {
            get
            {
                return m_UnloadSceneSuccessCallback;
            }
        }

        /// <summary>
        /// 获取卸载场景失败回调函数。
        /// </summary>
        public UnloadSceneFailureCallback UnloadSceneFailureCallback
        {
            get
            {
                return m_UnloadSceneFailureCallback;
            }
        }
    }
}
