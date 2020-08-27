﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework.Download
{
    /// <summary>
    /// 下载代理辅助器接口。
    /// </summary>
    public interface IDownloadAgentHelper
    {
        /// <summary>
        /// 下载代理辅助器更新数据流事件。
        /// </summary>
        event EventHandler<DownloadAgentHelperUpdateBytesEventArgs> DownloadAgentHelperUpdateBytes;

        /// <summary>
        /// 下载代理辅助器更新数据大小事件。
        /// </summary>
        event EventHandler<DownloadAgentHelperUpdateLengthEventArgs> DownloadAgentHelperUpdateLength;

        /// <summary>
        /// 下载代理辅助器完成事件。
        /// </summary>
        event EventHandler<DownloadAgentHelperCompleteEventArgs> DownloadAgentHelperComplete;

        /// <summary>
        /// 下载代理辅助器错误事件。
        /// </summary>
        event EventHandler<DownloadAgentHelperErrorEventArgs> DownloadAgentHelperError;

        /// <summary>
        /// 通过下载代理辅助器下载指定地址的数据。
        /// </summary>
        /// <param name="downloadUri">下载地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        void Download(string downloadUri, object userData);

        /// <summary>
        /// 通过下载代理辅助器下载指定地址的数据。
        /// </summary>
        /// <param name="downloadUri">下载地址。</param>
        /// <param name="fromPosition">下载数据起始位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        void Download(string downloadUri, int fromPosition, object userData);

        /// <summary>
        /// 通过下载代理辅助器下载指定地址的数据。
        /// </summary>
        /// <param name="downloadUri">下载地址。</param>
        /// <param name="fromPosition">下载数据起始位置。</param>
        /// <param name="toPosition">下载数据结束位置。</param>
        /// <param name="userData">用户自定义数据。</param>
        void Download(string downloadUri, int fromPosition, int toPosition, object userData);

        /// <summary>
        /// 重置下载代理辅助器。
        /// </summary>
        void Reset();
    }
}
