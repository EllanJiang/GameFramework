//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 实例化资源回调函数集。
    /// </summary>
    public sealed class InstantiateAssetCallbacks
    {
        private readonly InstantiateAssetSuccessCallback m_InstantiateAssetSuccessCallback;
        private readonly InstantiateAssetFailureCallback m_InstantiateAssetFailureCallback;
        private readonly InstantiateAssetUpdateCallback m_InstantiateAssetUpdateCallback;
        private readonly InstantiateAssetDependencyAssetCallback m_InstantiateAssetDependencyAssetCallback;

        /// <summary>
        /// 初始化实例化资源回调函数集的新实例。
        /// </summary>
        /// <param name="instantiateAssetSuccessCallback">实例化资源成功回调函数。</param>
        public InstantiateAssetCallbacks(InstantiateAssetSuccessCallback instantiateAssetSuccessCallback)
            : this(instantiateAssetSuccessCallback, null, null, null)
        {

        }

        /// <summary>
        /// 初始化实例化资源回调函数集的新实例。
        /// </summary>
        /// <param name="instantiateAssetSuccessCallback">实例化资源成功回调函数。</param>
        /// <param name="instantiateAssetFailureCallback">实例化资源失败回调函数。</param>
        public InstantiateAssetCallbacks(InstantiateAssetSuccessCallback instantiateAssetSuccessCallback, InstantiateAssetFailureCallback instantiateAssetFailureCallback)
            : this(instantiateAssetSuccessCallback, instantiateAssetFailureCallback, null, null)
        {

        }

        /// <summary>
        /// 初始化实例化资源回调函数集的新实例。
        /// </summary>
        /// <param name="instantiateAssetSuccessCallback">实例化资源成功回调函数。</param>
        /// <param name="instantiateAssetUpdateCallback">实例化资源更新回调函数。</param>
        public InstantiateAssetCallbacks(InstantiateAssetSuccessCallback instantiateAssetSuccessCallback, InstantiateAssetUpdateCallback instantiateAssetUpdateCallback)
            : this(instantiateAssetSuccessCallback, null, instantiateAssetUpdateCallback, null)
        {

        }

        /// <summary>
        /// 初始化实例化资源回调函数集的新实例。
        /// </summary>
        /// <param name="instantiateAssetSuccessCallback">实例化资源成功回调函数。</param>
        /// <param name="instantiateAssetDependencyAssetCallback">实例化资源时实例化依赖资源回调函数。</param>
        public InstantiateAssetCallbacks(InstantiateAssetSuccessCallback instantiateAssetSuccessCallback, InstantiateAssetDependencyAssetCallback instantiateAssetDependencyAssetCallback)
            : this(instantiateAssetSuccessCallback, null, null, instantiateAssetDependencyAssetCallback)
        {

        }

        /// <summary>
        /// 初始化实例化资源回调函数集的新实例。
        /// </summary>
        /// <param name="instantiateAssetSuccessCallback">实例化资源成功回调函数。</param>
        /// <param name="instantiateAssetFailureCallback">实例化资源失败回调函数。</param>
        /// <param name="instantiateAssetUpdateCallback">实例化资源更新回调函数。</param>
        public InstantiateAssetCallbacks(InstantiateAssetSuccessCallback instantiateAssetSuccessCallback, InstantiateAssetFailureCallback instantiateAssetFailureCallback, InstantiateAssetUpdateCallback instantiateAssetUpdateCallback)
            : this(instantiateAssetSuccessCallback, instantiateAssetFailureCallback, instantiateAssetUpdateCallback, null)
        {

        }

        /// <summary>
        /// 初始化实例化资源回调函数集的新实例。
        /// </summary>
        /// <param name="instantiateAssetSuccessCallback">实例化资源成功回调函数。</param>
        /// <param name="instantiateAssetFailureCallback">实例化资源失败回调函数。</param>
        /// <param name="instantiateAssetDependencyAssetCallback">实例化资源时实例化依赖资源回调函数。</param>
        public InstantiateAssetCallbacks(InstantiateAssetSuccessCallback instantiateAssetSuccessCallback, InstantiateAssetFailureCallback instantiateAssetFailureCallback, InstantiateAssetDependencyAssetCallback instantiateAssetDependencyAssetCallback)
            : this(instantiateAssetSuccessCallback, instantiateAssetFailureCallback, null, instantiateAssetDependencyAssetCallback)
        {

        }

        /// <summary>
        /// 初始化实例化资源回调函数集的新实例。
        /// </summary>
        /// <param name="instantiateAssetSuccessCallback">实例化资源成功回调函数。</param>
        /// <param name="instantiateAssetFailureCallback">实例化资源失败回调函数。</param>
        /// <param name="instantiateAssetUpdateCallback">实例化资源更新回调函数。</param>
        /// <param name="instantiateAssetDependencyAssetCallback">实例化资源时实例化依赖资源回调函数。</param>
        public InstantiateAssetCallbacks(InstantiateAssetSuccessCallback instantiateAssetSuccessCallback, InstantiateAssetFailureCallback instantiateAssetFailureCallback, InstantiateAssetUpdateCallback instantiateAssetUpdateCallback, InstantiateAssetDependencyAssetCallback instantiateAssetDependencyAssetCallback)
        {
            if (instantiateAssetSuccessCallback == null)
            {
                throw new GameFrameworkException("Instantiate asset success callback is invalid.");
            }

            m_InstantiateAssetSuccessCallback = instantiateAssetSuccessCallback;
            m_InstantiateAssetFailureCallback = instantiateAssetFailureCallback;
            m_InstantiateAssetUpdateCallback = instantiateAssetUpdateCallback;
            m_InstantiateAssetDependencyAssetCallback = instantiateAssetDependencyAssetCallback;
        }

        /// <summary>
        /// 获取实例化资源成功回调函数。
        /// </summary>
        public InstantiateAssetSuccessCallback InstantiateAssetSuccessCallback
        {
            get
            {
                return m_InstantiateAssetSuccessCallback;
            }
        }

        /// <summary>
        /// 获取实例化资源失败回调函数。
        /// </summary>
        public InstantiateAssetFailureCallback InstantiateAssetFailureCallback
        {
            get
            {
                return m_InstantiateAssetFailureCallback;
            }
        }

        /// <summary>
        /// 获取实例化资源更新回调函数。
        /// </summary>
        public InstantiateAssetUpdateCallback InstantiateAssetUpdateCallback
        {
            get
            {
                return m_InstantiateAssetUpdateCallback;
            }
        }

        /// <summary>
        /// 获取实例化资源时实例化依赖资源回调函数。
        /// </summary>
        public InstantiateAssetDependencyAssetCallback InstantiateAssetDependencyAssetCallback
        {
            get
            {
                return m_InstantiateAssetDependencyAssetCallback;
            }
        }
    }
}
