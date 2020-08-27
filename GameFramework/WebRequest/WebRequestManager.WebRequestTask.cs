//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.WebRequest
{
    internal sealed partial class WebRequestManager : GameFrameworkModule, IWebRequestManager
    {
        /// <summary>
        /// Web 请求任务。
        /// </summary>
        private sealed class WebRequestTask : TaskBase
        {
            private static int s_Serial = 0;

            private WebRequestTaskStatus m_Status;
            private string m_WebRequestUri;
            private byte[] m_PostData;
            private float m_Timeout;
            private object m_UserData;

            public WebRequestTask()
            {
                m_Status = WebRequestTaskStatus.Todo;
                m_WebRequestUri = null;
                m_PostData = null;
                m_Timeout = 0f;
                m_UserData = null;
            }

            /// <summary>
            /// 获取或设置 Web 请求任务的状态。
            /// </summary>
            public WebRequestTaskStatus Status
            {
                get
                {
                    return m_Status;
                }
                set
                {
                    m_Status = value;
                }
            }

            /// <summary>
            /// 获取要发送的远程地址。
            /// </summary>
            public string WebRequestUri
            {
                get
                {
                    return m_WebRequestUri;
                }
            }

            /// <summary>
            /// 获取 Web 请求超时时长，以秒为单位。
            /// </summary>
            public float Timeout
            {
                get
                {
                    return m_Timeout;
                }
            }

            /// <summary>
            /// 获取用户自定义数据。
            /// </summary>
            public object UserData
            {
                get
                {
                    return m_UserData;
                }
            }

            /// <summary>
            /// 获取 Web 请求任务的描述。
            /// </summary>
            public override string Description
            {
                get
                {
                    return m_WebRequestUri;
                }
            }

            /// <summary>
            /// 创建 Web 请求任务。
            /// </summary>
            /// <param name="webRequestUri">要发送的远程地址。</param>
            /// <param name="postData">要发送的数据流。</param>
            /// <param name="priority">Web 请求任务的优先级。</param>
            /// <param name="timeout">下载超时时长，以秒为单位。</param>
            /// <param name="userData">用户自定义数据。</param>
            /// <returns>创建的 Web 请求任务。</returns>
            public static WebRequestTask Create(string webRequestUri, byte[] postData, int priority, float timeout, object userData)
            {
                WebRequestTask webRequestTask = ReferencePool.Acquire<WebRequestTask>();
                webRequestTask.Initialize(++s_Serial, priority);
                webRequestTask.m_WebRequestUri = webRequestUri;
                webRequestTask.m_PostData = postData;
                webRequestTask.m_Timeout = timeout;
                webRequestTask.m_UserData = userData;
                return webRequestTask;
            }

            /// <summary>
            /// 清理 Web 请求任务。
            /// </summary>
            public override void Clear()
            {
                base.Clear();
                m_Status = WebRequestTaskStatus.Todo;
                m_WebRequestUri = null;
                m_PostData = null;
                m_Timeout = 0f;
                m_UserData = null;
            }

            /// <summary>
            /// 获取要发送的数据流。
            /// </summary>
            public byte[] GetPostData()
            {
                return m_PostData;
            }
        }
    }
}
