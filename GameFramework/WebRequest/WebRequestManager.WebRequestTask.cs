//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.WebRequest
{
    internal partial class WebRequestManager
    {
        /// <summary>
        /// Web 请求任务。
        /// </summary>
        private sealed class WebRequestTask : ITask
        {
            private static int s_Serial = 0;

            private readonly int m_SerialId;
            private readonly int m_Priority;
            private bool m_Done;
            private WebRequestTaskStatus m_Status;
            private readonly string m_WebRequestUri;
            private readonly byte[] m_PostData;
            private readonly float m_Timeout;
            private readonly object m_UserData;

            /// <summary>
            /// 初始化 Web 请求任务的新实例。
            /// </summary>
            /// <param name="webRequestUri">要发送的远程地址。</param>
            /// <param name="postData">要发送的数据流。</param>
            /// <param name="priority">Web 请求任务的优先级。</param>
            /// <param name="timeout">下载超时时长，以秒为单位。</param>
            /// <param name="userData">用户自定义数据。</param>
            public WebRequestTask(string webRequestUri, byte[] postData, int priority, float timeout, object userData)
            {
                m_SerialId = s_Serial++;
                m_Priority = priority;
                m_Done = false;
                m_Status = WebRequestTaskStatus.Todo;
                m_WebRequestUri = webRequestUri;
                m_PostData = postData;
                m_Timeout = timeout;
                m_UserData = userData;
            }

            /// <summary>
            /// 获取 Web 请求任务的序列编号。
            /// </summary>
            public int SerialId
            {
                get
                {
                    return m_SerialId;
                }
            }

            /// <summary>
            /// 获取 Web 请求任务的优先级。
            /// </summary>
            public int Priority
            {
                get
                {
                    return m_Priority;
                }
            }

            /// <summary>
            /// 获取或设置 Web 请求任务是否完成。
            /// </summary>
            public bool Done
            {
                get
                {
                    return m_Done;
                }
                set
                {
                    m_Done = value;
                }
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
            /// 获取要发送的数据流。
            /// </summary>
            public byte[] GetPostData()
            {
                return m_PostData;
            }
        }
    }
}
