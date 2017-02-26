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
    /// 资源更新改变事件。
    /// </summary>
    public sealed class ResourceUpdateChangedEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化资源更新改变事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public ResourceUpdateChangedEventArgs(GameFramework.Resource.ResourceUpdateChangedEventArgs e)
        {
            Name = e.Name;
            DownloadPath = e.DownloadPath;
            DownloadUri = e.DownloadUri;
            CurrentLength = e.CurrentLength;
            ZipLength = e.ZipLength;
        }

        /// <summary>
        /// 获取资源更新改变事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.ResourceUpdateChanged;
            }
        }

        /// <summary>
        /// 获取资源名称。
        /// </summary>
        public string Name
        {
            get;
            private set;
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

        /// <summary>
        /// 获取当前下载大小。
        /// </summary>
        public int CurrentLength
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取压缩包大小。
        /// </summary>
        public int ZipLength
        {
            get;
            private set;
        }
    }
}
