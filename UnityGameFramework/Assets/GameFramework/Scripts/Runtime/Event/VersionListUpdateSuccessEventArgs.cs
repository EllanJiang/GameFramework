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
    /// 版本资源列表更新成功事件。
    /// </summary>
    public sealed class VersionListUpdateSuccessEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化版本资源列表更新成功事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public VersionListUpdateSuccessEventArgs(GameFramework.Resource.VersionListUpdateSuccessEventArgs e)
        {
            DownloadPath = e.DownloadPath;
            DownloadUri = e.DownloadUri;
        }

        /// <summary>
        /// 获取版本资源列表更新成功事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.VersionListUpdateSuccess;
            }
        }

        /// <summary>
        /// 获取资源下载后存放路径。
        /// </summary>
        public string DownloadPath
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取下载地址。
        /// </summary>
        public string DownloadUri
        {
            get;
            private set;
        }
    }
}
