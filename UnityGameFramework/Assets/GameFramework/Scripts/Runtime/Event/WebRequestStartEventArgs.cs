//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// Web 请求开始事件。
    /// </summary>
    public sealed class WebRequestStartEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化 Web 请求开始事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public WebRequestStartEventArgs(GameFramework.WebRequest.WebRequestStartEventArgs e)
        {
            WWWFormInfo wwwFormInfo = (WWWFormInfo)e.UserData;
            SerialId = e.SerialId;
            WebRequestUri = e.WebRequestUri;
            UserData = wwwFormInfo.UserData;
        }

        /// <summary>
        /// 获取 Web 请求开始事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.WebRequestStart;
            }
        }

        /// <summary>
        /// 获取 Web 请求任务的序列编号。
        /// </summary>
        public int SerialId
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取 Web 请求地址。
        /// </summary>
        public string WebRequestUri
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
