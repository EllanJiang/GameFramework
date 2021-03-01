//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace GameFramework.Download
{
    /// <summary>
    /// 下载管理器接口。
    /// </summary>
    public interface IDownloadManager
    {
        /// <summary>
        /// 获取或设置下载是否被暂停。
        /// </summary>
        bool Paused
        {
            get;
            set;
        }

        /// <summary>
        /// 获取下载代理总数量。
        /// </summary>
        int TotalAgentCount
        {
            get;
        }

        /// <summary>
        /// 获取可用下载代理数量。
        /// </summary>
        int FreeAgentCount
        {
            get;
        }

        /// <summary>
        /// 获取工作中下载代理数量。
        /// </summary>
        int WorkingAgentCount
        {
            get;
        }

        /// <summary>
        /// 获取等待下载任务数量。
        /// </summary>
        int WaitingTaskCount
        {
            get;
        }

        /// <summary>
        /// 获取或设置将缓冲区写入磁盘的临界大小。
        /// </summary>
        int FlushSize
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置下载超时时长，以秒为单位。
        /// </summary>
        float Timeout
        {
            get;
            set;
        }

        /// <summary>
        /// 获取当前下载速度。
        /// </summary>
        float CurrentSpeed
        {
            get;
        }

        /// <summary>
        /// 下载开始事件。
        /// </summary>
        event EventHandler<DownloadStartEventArgs> DownloadStart;

        /// <summary>
        /// 下载更新事件。
        /// </summary>
        event EventHandler<DownloadUpdateEventArgs> DownloadUpdate;

        /// <summary>
        /// 下载成功事件。
        /// </summary>
        event EventHandler<DownloadSuccessEventArgs> DownloadSuccess;

        /// <summary>
        /// 下载失败事件。
        /// </summary>
        event EventHandler<DownloadFailureEventArgs> DownloadFailure;

        /// <summary>
        /// 增加下载代理辅助器。
        /// </summary>
        /// <param name="downloadAgentHelper">要增加的下载代理辅助器。</param>
        void AddDownloadAgentHelper(IDownloadAgentHelper downloadAgentHelper);

        /// <summary>
        /// 根据下载任务的序列编号获取下载任务的信息。
        /// </summary>
        /// <param name="serialId">要获取信息的下载任务的序列编号。</param>
        /// <returns>下载任务的信息。</returns>
        TaskInfo GetDownloadInfo(int serialId);

        /// <summary>
        /// 根据下载任务的标签获取下载任务的信息。
        /// </summary>
        /// <param name="tag">要获取信息的下载任务的标签。</param>
        /// <returns>下载任务的信息。</returns>
        TaskInfo[] GetDownloadInfos(string tag);

        /// <summary>
        /// 根据下载任务的标签获取下载任务的信息。
        /// </summary>
        /// <param name="tag">要获取信息的下载任务的标签。</param>
        /// <param name="results">下载任务的信息。</param>
        void GetDownloadInfos(string tag, List<TaskInfo> results);

        /// <summary>
        /// 获取所有下载任务的信息。
        /// </summary>
        /// <returns>所有下载任务的信息。</returns>
        TaskInfo[] GetAllDownloadInfos();

        /// <summary>
        /// 获取所有下载任务的信息。
        /// </summary>
        /// <param name="results">所有下载任务的信息。</param>
        void GetAllDownloadInfos(List<TaskInfo> results);

        /// <summary>
        /// 增加下载任务。
        /// </summary>
        /// <param name="downloadPath">下载后存放路径。</param>
        /// <param name="downloadUri">原始下载地址。</param>
        /// <returns>新增下载任务的序列编号。</returns>
        int AddDownload(string downloadPath, string downloadUri);

        /// <summary>
        /// 增加下载任务。
        /// </summary>
        /// <param name="downloadPath">下载后存放路径。</param>
        /// <param name="downloadUri">原始下载地址。</param>
        /// <param name="tag">下载任务的标签。</param>
        /// <returns>新增下载任务的序列编号。</returns>
        int AddDownload(string downloadPath, string downloadUri, string tag);

        /// <summary>
        /// 增加下载任务。
        /// </summary>
        /// <param name="downloadPath">下载后存放路径。</param>
        /// <param name="downloadUri">原始下载地址。</param>
        /// <param name="priority">下载任务的优先级。</param>
        /// <returns>新增下载任务的序列编号。</returns>
        int AddDownload(string downloadPath, string downloadUri, int priority);

        /// <summary>
        /// 增加下载任务。
        /// </summary>
        /// <param name="downloadPath">下载后存放路径。</param>
        /// <param name="downloadUri">原始下载地址。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增下载任务的序列编号。</returns>
        int AddDownload(string downloadPath, string downloadUri, object userData);

        /// <summary>
        /// 增加下载任务。
        /// </summary>
        /// <param name="downloadPath">下载后存放路径。</param>
        /// <param name="downloadUri">原始下载地址。</param>
        /// <param name="tag">下载任务的标签。</param>
        /// <param name="priority">下载任务的优先级。</param>
        /// <returns>新增下载任务的序列编号。</returns>
        int AddDownload(string downloadPath, string downloadUri, string tag, int priority);

        /// <summary>
        /// 增加下载任务。
        /// </summary>
        /// <param name="downloadPath">下载后存放路径。</param>
        /// <param name="downloadUri">原始下载地址。</param>
        /// <param name="tag">下载任务的标签。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增下载任务的序列编号。</returns>
        int AddDownload(string downloadPath, string downloadUri, string tag, object userData);

        /// <summary>
        /// 增加下载任务。
        /// </summary>
        /// <param name="downloadPath">下载后存放路径。</param>
        /// <param name="downloadUri">原始下载地址。</param>
        /// <param name="priority">下载任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增下载任务的序列编号。</returns>
        int AddDownload(string downloadPath, string downloadUri, int priority, object userData);

        /// <summary>
        /// 增加下载任务。
        /// </summary>
        /// <param name="downloadPath">下载后存放路径。</param>
        /// <param name="downloadUri">原始下载地址。</param>
        /// <param name="tag">下载任务的标签。</param>
        /// <param name="priority">下载任务的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>新增下载任务的序列编号。</returns>
        int AddDownload(string downloadPath, string downloadUri, string tag, int priority, object userData);

        /// <summary>
        /// 根据下载任务的序列编号移除下载任务。
        /// </summary>
        /// <param name="serialId">要移除下载任务的序列编号。</param>
        /// <returns>是否移除下载任务成功。</returns>
        bool RemoveDownload(int serialId);

        /// <summary>
        /// 根据下载任务的标签移除下载任务。
        /// </summary>
        /// <param name="tag">要移除下载任务的标签。</param>
        /// <returns>移除下载任务的数量。</returns>
        int RemoveDownloads(string tag);

        /// <summary>
        /// 移除所有下载任务。
        /// </summary>
        /// <returns>移除下载任务的数量。</returns>
        int RemoveAllDownloads();
    }
}
