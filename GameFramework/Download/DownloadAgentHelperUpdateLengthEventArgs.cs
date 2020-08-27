//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Download
{
    /// <summary>
    /// 下载代理辅助器更新数据大小事件。
    /// </summary>
    public sealed class DownloadAgentHelperUpdateLengthEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化下载代理辅助器更新数据大小事件的新实例。
        /// </summary>
        public DownloadAgentHelperUpdateLengthEventArgs()
        {
            DeltaLength = 0;
        }

        /// <summary>
        /// 获取下载的增量数据大小。
        /// </summary>
        public int DeltaLength
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建下载代理辅助器更新数据大小事件。
        /// </summary>
        /// <param name="deltaLength">下载的增量数据大小。</param>
        /// <returns>创建的下载代理辅助器更新数据大小事件。</returns>
        public static DownloadAgentHelperUpdateLengthEventArgs Create(int deltaLength)
        {
            if (deltaLength <= 0)
            {
                throw new GameFrameworkException("Delta length is invalid.");
            }

            DownloadAgentHelperUpdateLengthEventArgs downloadAgentHelperUpdateLengthEventArgs = ReferencePool.Acquire<DownloadAgentHelperUpdateLengthEventArgs>();
            downloadAgentHelperUpdateLengthEventArgs.DeltaLength = deltaLength;
            return downloadAgentHelperUpdateLengthEventArgs;
        }

        /// <summary>
        /// 清理下载代理辅助器更新数据大小事件。
        /// </summary>
        public override void Clear()
        {
            DeltaLength = 0;
        }
    }
}
