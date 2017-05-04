//------------------------------------------------------------
// Game Framework v2.x
// Copyright © 2014-2016 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Download;
using System;
using System.Collections.Generic;
#if UNITY_5_4_OR_NEWER
using UnityEngine.Networking;
#else
using UnityEngine.Experimental.Networking;
#endif

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 使用 UnityWebRequest 实现的下载代理辅助器。
    /// </summary>
    public class UnityWebRequestDownloadAgentHelper : DownloadAgentHelperBase, IDisposable
    {
        private UnityWebRequest m_UnityWebRequest = null;
        private int m_LastDownloadedSize = 0;
        private bool m_Disposed = false;

        private EventHandler<DownloadAgentHelperUpdateEventArgs> m_DownloadAgentHelperUpdateEventHandler = null;
        private EventHandler<DownloadAgentHelperCompleteEventArgs> m_DownloadAgentHelperCompleteEventHandler = null;
        private EventHandler<DownloadAgentHelperErrorEventArgs> m_DownloadAgentHelperErrorEventHandler = null;

        /// <summary>
        /// 下载代理辅助器更新事件。
        /// </summary>
        public override event EventHandler<DownloadAgentHelperUpdateEventArgs> DownloadAgentHelperUpdate
        {
            add
            {
                m_DownloadAgentHelperUpdateEventHandler += value;
            }
            remove
            {
                m_DownloadAgentHelperUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 下载代理辅助器完成事件。
        /// </summary>
        public override event EventHandler<DownloadAgentHelperCompleteEventArgs> DownloadAgentHelperComplete
        {
            add
            {
                m_DownloadAgentHelperCompleteEventHandler += value;
            }
            remove
            {
                m_DownloadAgentHelperCompleteEventHandler -= value;
            }
        }

        /// <summary>
        /// 下载代理辅助器错误事件。
        /// </summary>
        public override event EventHandler<DownloadAgentHelperErrorEventArgs> DownloadAgentHelperError
        {
            add
            {
                m_DownloadAgentHelperErrorEventHandler += value;
            }
            remove
            {
                m_DownloadAgentHelperErrorEventHandler -= value;
            }
        }

        /// <summary>
        /// 通过下载代理辅助器下载指定地址的数据。
        /// </summary>
        /// <param name="downloadUri">下载地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        public override void Download(string downloadUri, object userData)
        {
            if (m_DownloadAgentHelperUpdateEventHandler == null || m_DownloadAgentHelperCompleteEventHandler == null || m_DownloadAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Download agent helper handler is invalid.");
                return;
            }

            m_UnityWebRequest = UnityWebRequest.Get(downloadUri);
            m_UnityWebRequest.Send();
        }

        /// <summary>
        /// 通过下载代理辅助器下载指定地址的数据。
        /// </summary>
        /// <param name="downloadUri">下载地址。</param>
        /// <param name="fromPosition">下载数据起始位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        public override void Download(string downloadUri, int fromPosition, object userData)
        {
            if (m_DownloadAgentHelperUpdateEventHandler == null || m_DownloadAgentHelperCompleteEventHandler == null || m_DownloadAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Download agent helper handler is invalid.");
                return;
            }

            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Range", string.Format("bytes={0}-", fromPosition.ToString()));
            m_UnityWebRequest = UnityWebRequest.Post(downloadUri, header);
            m_UnityWebRequest.Send();
        }

        /// <summary>
        /// 通过下载代理辅助器下载指定地址的数据。
        /// </summary>
        /// <param name="downloadUri">下载地址。</param>
        /// <param name="fromPosition">下载数据起始位置。</param>
        /// <param name="toPosition">下载数据结束位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        public override void Download(string downloadUri, int fromPosition, int toPosition, object userData)
        {
            if (m_DownloadAgentHelperUpdateEventHandler == null || m_DownloadAgentHelperCompleteEventHandler == null || m_DownloadAgentHelperErrorEventHandler == null)
            {
                Log.Fatal("Download agent helper handler is invalid.");
                return;
            }

            Dictionary<string, string> header = new Dictionary<string, string>();
            header.Add("Range", string.Format("bytes={0}-{1}", fromPosition.ToString(), toPosition.ToString()));
            m_UnityWebRequest = UnityWebRequest.Post(downloadUri, header);
            m_UnityWebRequest.Send();
        }

        /// <summary>
        /// 重置下载代理辅助器。
        /// </summary>
        public override void Reset()
        {
            if (m_UnityWebRequest != null)
            {
                m_UnityWebRequest.Dispose();
                m_UnityWebRequest = null;
            }

            m_LastDownloadedSize = 0;
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
                if (m_UnityWebRequest != null)
                {
                    m_UnityWebRequest.Dispose();
                    m_UnityWebRequest = null;
                }
            }

            m_Disposed = true;
        }

        private void Update()
        {
            if (m_UnityWebRequest == null)
            {
                return;
            }

            if (!m_UnityWebRequest.isDone)
            {
                if (m_LastDownloadedSize < (int)m_UnityWebRequest.downloadedBytes)
                {
                    m_LastDownloadedSize = (int)m_UnityWebRequest.downloadedBytes;
                    m_DownloadAgentHelperUpdateEventHandler(this, new DownloadAgentHelperUpdateEventArgs((int)m_UnityWebRequest.downloadedBytes, null));
                }

                return;
            }

            bool isError = false;
#if UNITY_2017_1_OR_NEWER
            isError = m_UnityWebRequest.isNetworkError;
#else
            isError = m_UnityWebRequest.isError;
#endif
            if (isError)
            {
                m_DownloadAgentHelperErrorEventHandler(this, new DownloadAgentHelperErrorEventArgs(m_UnityWebRequest.error));
            }
            else if (m_UnityWebRequest.downloadHandler.isDone)
            {
                m_DownloadAgentHelperCompleteEventHandler(this, new DownloadAgentHelperCompleteEventArgs((int)m_UnityWebRequest.downloadedBytes, m_UnityWebRequest.downloadHandler.data));
            }
        }
    }
}
