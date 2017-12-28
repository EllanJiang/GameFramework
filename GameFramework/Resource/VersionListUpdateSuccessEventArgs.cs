//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 版本资源列表更新成功事件。
    /// </summary>
    public sealed class VersionListUpdateSuccessEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化版本资源列表更新成功事件的新实例。
        /// </summary>
        /// <param name="downloadPath">资源下载后存放路径。</param>
        /// <param name="downloadUri">资源下载地址。</param>
        public VersionListUpdateSuccessEventArgs(string downloadPath, string downloadUri)
        {
            DownloadPath = downloadPath;
            DownloadUri = downloadUri;
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
