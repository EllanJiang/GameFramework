//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.WebRequest;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// Web 请求组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Web Request")]
    public sealed class WebRequestComponent : GameFrameworkComponent
    {
        private IWebRequestManager m_WebRequestManager = null;
        private EventComponent m_EventComponent = null;

        [SerializeField]
        private Transform m_InstanceRoot = null;

        [SerializeField]
        private string m_WebRequestAgentHelperTypeName = "UnityGameFramework.Runtime.UnityWebRequestAgentHelper";

        [SerializeField]
        private WebRequestAgentHelperBase m_CustomWebRequestAgentHelper = null;

        [SerializeField]
        private int m_WebRequestAgentHelperCount = 1;

        [SerializeField]
        private float m_Timeout = 30f;

        /// <summary>
        /// 获取 Web 请求代理总数量。
        /// </summary>
        public int TotalAgentCount
        {
            get
            {
                return m_WebRequestManager.TotalAgentCount;
            }
        }

        /// <summary>
        /// 获取可用 Web 请求代理数量。
        /// </summary>
        public int FreeAgentCount
        {
            get
            {
                return m_WebRequestManager.FreeAgentCount;
            }
        }

        /// <summary>
        /// 获取工作中 Web 请求代理数量。
        /// </summary>
        public int WorkingAgentCount
        {
            get
            {
                return m_WebRequestManager.WorkingAgentCount;
            }
        }

        /// <summary>
        /// 获取等待 Web 请求数量。
        /// </summary>
        public int WaitingTaskCount
        {
            get
            {
                return m_WebRequestManager.WaitingTaskCount;
            }
        }

        /// <summary>
        /// 获取或设置 Web 请求超时时长，以秒为单位。
        /// </summary>
        public float Timeout
        {
            get
            {
                return m_WebRequestManager.Timeout;
            }
            set
            {
                m_WebRequestManager.Timeout = m_Timeout = value;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_WebRequestManager = GameFrameworkEntry.GetModule<IWebRequestManager>();
            if (m_WebRequestManager == null)
            {
                Log.Fatal("Web request manager is invalid.");
                return;
            }

            m_WebRequestManager.Timeout = m_Timeout;
            m_WebRequestManager.WebRequestStart += OnWebRequestStart;
            m_WebRequestManager.WebRequestSuccess += OnWebRequestSuccess;
            m_WebRequestManager.WebRequestFailure += OnWebRequestFailure;
        }

        private void Start()
        {
            m_EventComponent = GameEntry.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            if (m_InstanceRoot == null)
            {
                m_InstanceRoot = (new GameObject("Web Request Agent Instances")).transform;
                m_InstanceRoot.SetParent(gameObject.transform);
                m_InstanceRoot.localScale = Vector3.one;
            }

            for (int i = 0; i < m_WebRequestAgentHelperCount; i++)
            {
                AddWebRequestAgentHelper(i);
            }
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri)
        {
            return AddWebRequest(webRequestUri, null, null, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData)
        {
            return AddWebRequest(webRequestUri, postData, null, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, null);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, object userData)
        {
            return AddWebRequest(webRequestUri, null, null, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, byte[] postData, object userData)
        {
            return AddWebRequest(webRequestUri, postData, null, userData);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        public int AddWebRequest(string webRequestUri, WWWForm wwwForm, object userData)
        {
            return AddWebRequest(webRequestUri, null, wwwForm, userData);
        }

        /// <summary>
        /// 移除 Web 请求任务。
        /// </summary>
        /// <param name="serialId">要移除 Web 请求任务的序列编号。</param>
        /// <returns>是否移除 Web 请求任务成功。</returns>
        public bool RemoveWebRequest(int serialId)
        {
            return m_WebRequestManager.RemoveWebRequest(serialId);
        }

        /// <summary>
        /// 移除所有 Web 请求任务。
        /// </summary>
        public void RemoveAllWebRequests()
        {
            m_WebRequestManager.RemoveAllWebRequests();
        }

        /// <summary>
        /// 增加 Web 请求代理辅助器。
        /// </summary>
        /// <param name="index">Web 请求代理辅助器索引。</param>
        private void AddWebRequestAgentHelper(int index)
        {
            WebRequestAgentHelperBase webRequestAgentHelper = Helper.CreateHelper(m_WebRequestAgentHelperTypeName, m_CustomWebRequestAgentHelper, index);
            if (webRequestAgentHelper == null)
            {
                Log.Error("Can not create web request agent helper.");
                return;
            }

            webRequestAgentHelper.name = string.Format("Web Request Agent Helper - {0}", index.ToString());
            Transform transform = webRequestAgentHelper.transform;
            transform.SetParent(m_InstanceRoot);
            transform.localScale = Vector3.one;

            m_WebRequestManager.AddWebRequestAgentHelper(webRequestAgentHelper);
        }

        /// <summary>
        /// 增加 Web 请求任务。
        /// </summary>
        /// <param name="webRequestUri">Web 请求地址。</param>
        /// <param name="postData">要发送的数据流。</param>
        /// <param name="wwwForm">WWW 表单。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增 Web 请求任务的序列编号。</returns>
        private int AddWebRequest(string webRequestUri, byte[] postData, WWWForm wwwForm, object userData)
        {
            return m_WebRequestManager.AddWebRequest(webRequestUri, postData, new WWWFormInfo(wwwForm, userData));
        }

        private void OnWebRequestStart(object sender, GameFramework.WebRequest.WebRequestStartEventArgs e)
        {
            m_EventComponent.Fire(this, new WebRequestStartEventArgs(e));
        }

        private void OnWebRequestSuccess(object sender, GameFramework.WebRequest.WebRequestSuccessEventArgs e)
        {
            m_EventComponent.Fire(this, new WebRequestSuccessEventArgs(e));
        }

        private void OnWebRequestFailure(object sender, GameFramework.WebRequest.WebRequestFailureEventArgs e)
        {
            Log.Warning("Web request failure, web request serial id '{0}', web request uri '{1}', error message '{2}'.", e.SerialId.ToString(), e.WebRequestUri, e.ErrorMessage);
            m_EventComponent.Fire(this, new WebRequestFailureEventArgs(e));
        }
    }
}
