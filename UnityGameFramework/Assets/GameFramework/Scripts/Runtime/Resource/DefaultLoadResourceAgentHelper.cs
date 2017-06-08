//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Resource;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 默认加载资源代理辅助器。
    /// </summary>
    public class DefaultLoadResourceAgentHelper : LoadResourceAgentHelperBase, IDisposable
    {
        private string m_FileFullPath = null;
        private string m_BytesFullPath = null;
        private int m_LoadType = 0;
        private string m_ResourceChildName = null;
        private bool m_Disposed = false;
        private WWW m_WWW = null;
        private AssetBundleCreateRequest m_FileAssetBundleCreateRequest = null;
        private AssetBundleCreateRequest m_BytesAssetBundleCreateRequest = null;
        private AssetBundleRequest m_AssetBundleRequest = null;
        private AsyncOperation m_AsyncOperation = null;

        private EventHandler<LoadResourceAgentHelperUpdateEventArgs> m_LoadResourceAgentHelperUpdateEventHandler = null;
        private EventHandler<LoadResourceAgentHelperReadFileCompleteEventArgs> m_LoadResourceAgentHelperReadFileCompleteEventHandler = null;
        private EventHandler<LoadResourceAgentHelperReadBytesCompleteEventArgs> m_LoadResourceAgentHelperReadBytesCompleteEventHandler = null;
        private EventHandler<LoadResourceAgentHelperParseBytesCompleteEventArgs> m_LoadResourceAgentHelperParseBytesCompleteEventHandler = null;
        private EventHandler<LoadResourceAgentHelperLoadCompleteEventArgs> m_LoadResourceAgentHelperLoadCompleteEventHandler = null;
        private EventHandler<LoadResourceAgentHelperErrorEventArgs> m_LoadResourceAgentHelperErrorEventHandler = null;

        /// <summary>
        /// 加载资源代理辅助器异步加载资源更新事件。
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperUpdateEventArgs> LoadResourceAgentHelperUpdate
        {
            add
            {
                m_LoadResourceAgentHelperUpdateEventHandler += value;
            }
            remove
            {
                m_LoadResourceAgentHelperUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载资源代理辅助器异步读取资源文件完成事件。
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperReadFileCompleteEventArgs> LoadResourceAgentHelperReadFileComplete
        {
            add
            {
                m_LoadResourceAgentHelperReadFileCompleteEventHandler += value;
            }
            remove
            {
                m_LoadResourceAgentHelperReadFileCompleteEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载资源代理辅助器异步读取资源二进制流完成事件。
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperReadBytesCompleteEventArgs> LoadResourceAgentHelperReadBytesComplete
        {
            add
            {
                m_LoadResourceAgentHelperReadBytesCompleteEventHandler += value;
            }
            remove
            {
                m_LoadResourceAgentHelperReadBytesCompleteEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载资源代理辅助器异步将资源二进制流转换为加载对象完成事件。
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperParseBytesCompleteEventArgs> LoadResourceAgentHelperParseBytesComplete
        {
            add
            {
                m_LoadResourceAgentHelperParseBytesCompleteEventHandler += value;
            }
            remove
            {
                m_LoadResourceAgentHelperParseBytesCompleteEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载资源代理辅助器异步加载资源完成事件。
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperLoadCompleteEventArgs> LoadResourceAgentHelperLoadComplete
        {
            add
            {
                m_LoadResourceAgentHelperLoadCompleteEventHandler += value;
            }
            remove
            {
                m_LoadResourceAgentHelperLoadCompleteEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载资源代理辅助器错误事件。
        /// </summary>
        public override event EventHandler<LoadResourceAgentHelperErrorEventArgs> LoadResourceAgentHelperError
        {
            add
            {
                m_LoadResourceAgentHelperErrorEventHandler += value;
            }
            remove
            {
                m_LoadResourceAgentHelperErrorEventHandler -= value;
            }
        }

        /// <summary>
        /// 通过加载资源代理辅助器开始异步读取资源文件。
        /// </summary>
        /// <param name="fullPath">要加载资源的完整路径名。</param>
        public override void ReadFile(string fullPath)
        {
            if (m_LoadResourceAgentHelperReadFileCompleteEventHandler == null || m_LoadResourceAgentHelperUpdateEventHandler == null || m_LoadResourceAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Load resource agent helper handler is invalid.");
                return;
            }

            m_FileFullPath = fullPath;
            m_FileAssetBundleCreateRequest = AssetBundle.LoadFromFileAsync(m_FileFullPath);
        }

        /// <summary>
        /// 通过加载资源代理辅助器开始异步读取资源二进制流。
        /// </summary>
        /// <param name="fullPath">要加载资源的完整路径名。</param>
        /// <param name="loadType">资源加载方式。</param>
        public override void ReadBytes(string fullPath, int loadType)
        {
            if (m_LoadResourceAgentHelperReadBytesCompleteEventHandler == null || m_LoadResourceAgentHelperUpdateEventHandler == null || m_LoadResourceAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Load resource agent helper handler is invalid.");
                return;
            }

            m_BytesFullPath = fullPath;
            m_LoadType = loadType;
            m_WWW = new WWW(Utility.Path.GetRemotePath(fullPath));
        }

        /// <summary>
        /// 通过加载资源代理辅助器开始异步将资源二进制流转换为加载对象。
        /// </summary>
        /// <param name="bytes">要加载资源的二进制流。</param>
        public override void ParseBytes(byte[] bytes)
        {
            if (m_LoadResourceAgentHelperParseBytesCompleteEventHandler == null || m_LoadResourceAgentHelperUpdateEventHandler == null || m_LoadResourceAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Load resource agent helper handler is invalid.");
                return;
            }

            m_BytesAssetBundleCreateRequest = AssetBundle.LoadFromMemoryAsync(bytes);
        }

        /// <summary>
        /// 通过加载资源代理辅助器开始异步加载资源。
        /// </summary>
        /// <param name="resource">资源。</param>
        /// <param name="resourceChildName">要加载的子资源名。</param>
        /// <param name="isScene">要加载的资源是否是场景。</param>
        public override void LoadAsset(object resource, string resourceChildName, bool isScene)
        {
            if (m_LoadResourceAgentHelperLoadCompleteEventHandler == null || m_LoadResourceAgentHelperUpdateEventHandler == null || m_LoadResourceAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Load resource agent helper handler is invalid.");
                return;
            }

            AssetBundle assetBundle = resource as AssetBundle;
            if (assetBundle == null)
            {
                m_LoadResourceAgentHelperErrorEventHandler(this, new LoadResourceAgentHelperErrorEventArgs(LoadResourceStatus.TypeError, "Can not load asset bundle from loaded resource which is not an asset bundle."));
                return;
            }

            if (string.IsNullOrEmpty(resourceChildName))
            {
                m_LoadResourceAgentHelperErrorEventHandler(this, new LoadResourceAgentHelperErrorEventArgs(LoadResourceStatus.ChildAssetError, "Can not load asset from asset bundle which child name is invalid."));
                return;
            }

            m_ResourceChildName = resourceChildName;
            if (isScene)
            {
                int sceneNamePosition = resourceChildName.IndexOf('.');
                string sceneName = sceneNamePosition > 0 ? resourceChildName.Substring(0, sceneNamePosition) : resourceChildName;
                m_AsyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            }
            else
            {
                m_AssetBundleRequest = assetBundle.LoadAssetAsync(resourceChildName);
            }
        }

        /// <summary>
        /// 重置加载资源代理辅助器。
        /// </summary>
        public override void Reset()
        {
            m_FileFullPath = null;
            m_BytesFullPath = null;
            m_LoadType = 0;
            m_ResourceChildName = null;

            if (m_WWW != null)
            {
                m_WWW.Dispose();
                m_WWW = null;
            }

            m_FileAssetBundleCreateRequest = null;
            m_BytesAssetBundleCreateRequest = null;
            m_AssetBundleRequest = null;
            m_AsyncOperation = null;
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放资源。
        /// </summary>
        /// <param name="disposing">释放资源标记。</param>
        private void Dispose(bool disposing)
        {
            if (m_Disposed)
            {
                return;
            }

            if (disposing)
            {
                if (m_WWW != null)
                {
                    m_WWW.Dispose();
                    m_WWW = null;
                }
            }

            m_Disposed = true;
        }

        private void Update()
        {
            UpdateWWW();
            UpdateFileAssetBundleCreateRequest();
            UpdateBytesAssetBundleCreateRequest();
            UpdateAssetBundleRequest();
            UpdateAsyncOperation();
        }

        private void UpdateWWW()
        {
            if (m_WWW != null)
            {
                if (m_WWW.isDone)
                {
                    if (string.IsNullOrEmpty(m_WWW.error))
                    {
                        m_LoadResourceAgentHelperReadBytesCompleteEventHandler(this, new LoadResourceAgentHelperReadBytesCompleteEventArgs(m_WWW.bytes, m_LoadType));
                        m_WWW.Dispose();
                        m_WWW = null;
                        m_BytesFullPath = null;
                        m_LoadType = 0;
                    }
                    else
                    {
                        m_LoadResourceAgentHelperErrorEventHandler(this, new LoadResourceAgentHelperErrorEventArgs(LoadResourceStatus.NotExist, string.Format("Can not load asset bundle '{0}' with error message '{1}'.", m_BytesFullPath, m_WWW.error)));
                    }
                }
                else
                {
                    m_LoadResourceAgentHelperUpdateEventHandler(this, new LoadResourceAgentHelperUpdateEventArgs(LoadResourceProgress.ReadBundle, m_WWW.progress));
                }
            }
        }

        private void UpdateFileAssetBundleCreateRequest()
        {
            if (m_FileAssetBundleCreateRequest != null)
            {
                if (m_FileAssetBundleCreateRequest.isDone)
                {
                    AssetBundle assetBundle = m_FileAssetBundleCreateRequest.assetBundle;
                    if (assetBundle != null)
                    {
                        AssetBundleCreateRequest oldFileAssetBundleCreateRequest = m_FileAssetBundleCreateRequest;
                        m_LoadResourceAgentHelperReadFileCompleteEventHandler(this, new LoadResourceAgentHelperReadFileCompleteEventArgs(assetBundle));
                        if (m_FileAssetBundleCreateRequest == oldFileAssetBundleCreateRequest)
                        {
                            m_FileAssetBundleCreateRequest = null;
                        }
                    }
                    else
                    {
                        m_LoadResourceAgentHelperErrorEventHandler(this, new LoadResourceAgentHelperErrorEventArgs(LoadResourceStatus.NotExist, string.Format("Can not load asset bundle from file '{0}' which is not a valid asset bundle.", m_FileFullPath)));
                    }
                }
                else
                {
                    m_LoadResourceAgentHelperUpdateEventHandler(this, new LoadResourceAgentHelperUpdateEventArgs(LoadResourceProgress.LoadBundle, m_FileAssetBundleCreateRequest.progress));
                }
            }
        }

        private void UpdateBytesAssetBundleCreateRequest()
        {
            if (m_BytesAssetBundleCreateRequest != null)
            {
                if (m_BytesAssetBundleCreateRequest.isDone)
                {
                    AssetBundle assetBundle = m_BytesAssetBundleCreateRequest.assetBundle;
                    if (assetBundle != null)
                    {
                        AssetBundleCreateRequest oldBytesAssetBundleCreateRequest = m_BytesAssetBundleCreateRequest;
                        m_LoadResourceAgentHelperParseBytesCompleteEventHandler(this, new LoadResourceAgentHelperParseBytesCompleteEventArgs(assetBundle));
                        if (m_BytesAssetBundleCreateRequest == oldBytesAssetBundleCreateRequest)
                        {
                            m_BytesAssetBundleCreateRequest = null;
                        }
                    }
                    else
                    {
                        m_LoadResourceAgentHelperErrorEventHandler(this, new LoadResourceAgentHelperErrorEventArgs(LoadResourceStatus.NotExist, "Can not load asset bundle from memory which is not a valid asset bundle."));
                    }
                }
                else
                {
                    m_LoadResourceAgentHelperUpdateEventHandler(this, new LoadResourceAgentHelperUpdateEventArgs(LoadResourceProgress.LoadBundle, m_BytesAssetBundleCreateRequest.progress));
                }
            }
        }

        private void UpdateAssetBundleRequest()
        {
            if (m_AssetBundleRequest != null)
            {
                if (m_AssetBundleRequest.isDone)
                {
                    if (m_AssetBundleRequest.asset != null)
                    {
                        m_LoadResourceAgentHelperLoadCompleteEventHandler(this, new LoadResourceAgentHelperLoadCompleteEventArgs(m_AssetBundleRequest.asset));
                        m_ResourceChildName = null;
                        m_AssetBundleRequest = null;
                    }
                    else
                    {
                        m_LoadResourceAgentHelperErrorEventHandler(this, new LoadResourceAgentHelperErrorEventArgs(LoadResourceStatus.ChildAssetError, string.Format("Can not load asset '{0}' from asset bundle which is not exist.", m_ResourceChildName)));
                    }
                }
                else
                {
                    m_LoadResourceAgentHelperUpdateEventHandler(this, new LoadResourceAgentHelperUpdateEventArgs(LoadResourceProgress.LoadAsset, m_AssetBundleRequest.progress));
                }
            }
        }

        private void UpdateAsyncOperation()
        {
            if (m_AsyncOperation != null)
            {
                if (m_AsyncOperation.isDone)
                {
                    if (m_AsyncOperation.allowSceneActivation)
                    {
                        m_LoadResourceAgentHelperLoadCompleteEventHandler(this, new LoadResourceAgentHelperLoadCompleteEventArgs(new SceneAsset()));
                        m_ResourceChildName = null;
                        m_AsyncOperation = null;
                    }
                    else
                    {
                        m_LoadResourceAgentHelperErrorEventHandler(this, new LoadResourceAgentHelperErrorEventArgs(LoadResourceStatus.SceneAssetError, string.Format("Can not load scene asset '{0}' from asset bundle.", m_ResourceChildName)));
                    }
                }
                else
                {
                    m_LoadResourceAgentHelperUpdateEventHandler(this, new LoadResourceAgentHelperUpdateEventArgs(LoadResourceProgress.LoadScene, m_AsyncOperation.progress));
                }
            }
        }
    }
}
