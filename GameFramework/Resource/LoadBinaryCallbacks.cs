//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 加载二进制资源回调函数集。
    /// </summary>
    public sealed class LoadBinaryCallbacks
    {
        private readonly LoadBinarySuccessCallback m_LoadBinarySuccessCallback;
        private readonly LoadBinaryFailureCallback m_LoadBinaryFailureCallback;

        /// <summary>
        /// 初始化加载二进制资源回调函数集的新实例。
        /// </summary>
        /// <param name="loadBinarySuccessCallback">加载二进制资源成功回调函数。</param>
        public LoadBinaryCallbacks(LoadBinarySuccessCallback loadBinarySuccessCallback)
            : this(loadBinarySuccessCallback, null)
        {
        }

        /// <summary>
        /// 初始化加载二进制资源回调函数集的新实例。
        /// </summary>
        /// <param name="loadBinarySuccessCallback">加载二进制资源成功回调函数。</param>
        /// <param name="loadBinaryFailureCallback">加载二进制资源失败回调函数。</param>
        public LoadBinaryCallbacks(LoadBinarySuccessCallback loadBinarySuccessCallback, LoadBinaryFailureCallback loadBinaryFailureCallback)
        {
            if (loadBinarySuccessCallback == null)
            {
                throw new GameFrameworkException("Load binary success callback is invalid.");
            }

            m_LoadBinarySuccessCallback = loadBinarySuccessCallback;
            m_LoadBinaryFailureCallback = loadBinaryFailureCallback;
        }

        /// <summary>
        /// 获取加载二进制资源成功回调函数。
        /// </summary>
        public LoadBinarySuccessCallback LoadBinarySuccessCallback
        {
            get
            {
                return m_LoadBinarySuccessCallback;
            }
        }

        /// <summary>
        /// 获取加载二进制资源失败回调函数。
        /// </summary>
        public LoadBinaryFailureCallback LoadBinaryFailureCallback
        {
            get
            {
                return m_LoadBinaryFailureCallback;
            }
        }
    }
}
