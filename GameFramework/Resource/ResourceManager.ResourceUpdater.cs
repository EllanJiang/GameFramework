//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Download;
using System;
using System.Collections.Generic;
using System.IO;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        /// <summary>
        /// 资源更新器。
        /// </summary>
        private sealed partial class ResourceUpdater
        {
            private readonly ResourceManager m_ResourceManager;
            private readonly List<UpdateInfo> m_UpdateWaitingInfo;
            private IDownloadManager m_DownloadManager;
            private bool m_CheckResourcesComplete;
            private bool m_UpdateAllowed;
            private bool m_UpdateComplete;
            private int m_RetryCount;
            private int m_UpdatingCount;

            public GameFrameworkAction<ResourceName, string, string, int, int, int> ResourceUpdateStart;
            public GameFrameworkAction<ResourceName, string, string, int, int> ResourceUpdateChanged;
            public GameFrameworkAction<ResourceName, string, string, int, int> ResourceUpdateSuccess;
            public GameFrameworkAction<ResourceName, string, int, int, string> ResourceUpdateFailure;
            public GameFrameworkAction ResourceUpdateAllComplete;

            /// <summary>
            /// 初始化资源更新器的新实例。
            /// </summary>
            /// <param name="resourceManager">资源管理器。</param>
            public ResourceUpdater(ResourceManager resourceManager)
            {
                m_ResourceManager = resourceManager;
                m_UpdateWaitingInfo = new List<UpdateInfo>();
                m_DownloadManager = null;
                m_CheckResourcesComplete = false;
                m_UpdateAllowed = false;
                m_UpdateComplete = false;
                m_RetryCount = 3;
                m_UpdatingCount = 0;

                ResourceUpdateStart = null;
                ResourceUpdateChanged = null;
                ResourceUpdateSuccess = null;
                ResourceUpdateFailure = null;
                ResourceUpdateAllComplete = null;
            }

            /// <summary>
            /// 获取或设置资源更新重试次数。
            /// </summary>
            public int RetryCount
            {
                get
                {
                    return m_RetryCount;
                }
                set
                {
                    m_RetryCount = value;
                }
            }

            /// <summary>
            /// 获取等待更新队列大小。
            /// </summary>
            public int UpdateWaitingCount
            {
                get
                {
                    return m_UpdateWaitingInfo.Count;
                }
            }

            /// <summary>
            /// 获取正在更新队列大小。
            /// </summary>
            public int UpdatingCount
            {
                get
                {
                    return m_UpdatingCount;
                }
            }

            /// <summary>
            /// 资源更新器轮询。
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
            /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
            public void Update(float elapseSeconds, float realElapseSeconds)
            {
                if (m_UpdateAllowed && !m_UpdateComplete)
                {
                    if (m_UpdateWaitingInfo.Count > 0)
                    {
                        if (m_DownloadManager.FreeAgentCount > 0)
                        {
                            UpdateInfo updateInfo = m_UpdateWaitingInfo[0];
                            m_UpdateWaitingInfo.RemoveAt(0);
                            m_DownloadManager.AddDownload(updateInfo.DownloadPath, updateInfo.DownloadUri, updateInfo);
                            m_UpdatingCount++;
                        }
                    }
                    else if (m_UpdatingCount <= 0)
                    {
                        m_UpdateComplete = true;
                        Utility.Path.RemoveEmptyDirectory(m_ResourceManager.m_ReadWritePath);
                        if (ResourceUpdateAllComplete != null)
                        {
                            ResourceUpdateAllComplete();
                        }
                    }
                }
            }

            /// <summary>
            /// 关闭并清理资源更新器。
            /// </summary>
            public void Shutdown()
            {
                if (m_DownloadManager != null)
                {
                    m_DownloadManager.DownloadStart -= OnDownloadStart;
                    m_DownloadManager.DownloadUpdate -= OnDownloadUpdate;
                    m_DownloadManager.DownloadSuccess -= OnDownloadSuccess;
                    m_DownloadManager.DownloadFailure -= OnDownloadFailure;
                }

                m_UpdateWaitingInfo.Clear();
            }

            /// <summary>
            /// 设置下载管理器。
            /// </summary>
            /// <param name="downloadManager">下载管理器。</param>
            public void SetDownloadManager(IDownloadManager downloadManager)
            {
                if (downloadManager == null)
                {
                    throw new GameFrameworkException("Download manager is invalid.");
                }

                m_DownloadManager = downloadManager;
                m_DownloadManager.DownloadStart += OnDownloadStart;
                m_DownloadManager.DownloadUpdate += OnDownloadUpdate;
                m_DownloadManager.DownloadSuccess += OnDownloadSuccess;
                m_DownloadManager.DownloadFailure += OnDownloadFailure;
            }

            /// <summary>
            /// 增加资源更新。
            /// </summary>
            /// <param name="resourceName">资源名称。</param>
            /// <param name="loadType">资源加载方式。</param>
            /// <param name="length">资源大小。</param>
            /// <param name="hashCode">资源哈希值。</param>
            /// <param name="zipLength">压缩包大小。</param>
            /// <param name="zipHashCode">压缩包哈希值。</param>
            /// <param name="downloadPath">下载后存放路径。</param>
            /// <param name="downloadUri">下载地址。</param>
            /// <param name="retryCount">已重试次数。</param>
            public void AddResourceUpdate(ResourceName resourceName, LoadType loadType, int length, int hashCode, int zipLength, int zipHashCode, string downloadPath, string downloadUri, int retryCount)
            {
                m_UpdateWaitingInfo.Add(new UpdateInfo(resourceName, loadType, length, hashCode, zipLength, zipHashCode, downloadPath, downloadUri, retryCount));
            }

            /// <summary>
            /// 检查资源完成。
            /// </summary>
            /// <param name="needGenerateReadWriteList">是否需要生成读写区资源列表。</param>
            public void CheckResourceComplete(bool needGenerateReadWriteList)
            {
                m_CheckResourcesComplete = true;
                if (needGenerateReadWriteList)
                {
                    GenerateReadWriteList();
                }
            }

            /// <summary>
            /// 更新资源。
            /// </summary>
            public void UpdateResources()
            {
                if (m_DownloadManager == null)
                {
                    throw new GameFrameworkException("You must set download manager first.");
                }

                if (!m_CheckResourcesComplete)
                {
                    throw new GameFrameworkException("You must check resources complete first.");
                }

                m_UpdateAllowed = true;
            }

            private void GenerateReadWriteList()
            {
                string file = Utility.Path.GetCombinePath(m_ResourceManager.m_ReadWritePath, Utility.Path.GetResourceNameWithSuffix(ResourceListFileName));
                string backupFile = null;

                if (File.Exists(file))
                {
                    backupFile = file + BackupFileSuffixName;
                    if (File.Exists(backupFile))
                    {
                        File.Delete(backupFile);
                    }

                    File.Move(file, backupFile);
                }

                FileStream fileStream = null;
                try
                {
                    fileStream = new FileStream(file, FileMode.CreateNew, FileAccess.Write);
                    using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                    {
                        fileStream = null;
                        byte[] encryptCode = new byte[4];
                        Utility.Random.GetRandomBytes(encryptCode);

                        binaryWriter.Write(ReadWriteListHeader);
                        binaryWriter.Write(ReadWriteListVersionHeader);
                        binaryWriter.Write(encryptCode);
                        binaryWriter.Write(m_ResourceManager.m_ReadWriteResourceInfos.Count);
                        foreach (KeyValuePair<ResourceName, ReadWriteResourceInfo> i in m_ResourceManager.m_ReadWriteResourceInfos)
                        {
                            byte[] nameBytes = Utility.Encryption.GetXorBytes(Utility.Converter.GetBytes(i.Key.Name), encryptCode);
                            binaryWriter.Write((byte)nameBytes.Length);
                            binaryWriter.Write(nameBytes);

                            if (i.Key.Variant == null)
                            {
                                binaryWriter.Write((byte)0);
                            }
                            else
                            {
                                byte[] variantBytes = Utility.Encryption.GetXorBytes(Utility.Converter.GetBytes(i.Key.Variant), encryptCode);
                                binaryWriter.Write((byte)variantBytes.Length);
                                binaryWriter.Write(variantBytes);
                            }

                            binaryWriter.Write((byte)i.Value.LoadType);
                            binaryWriter.Write(i.Value.Length);
                            binaryWriter.Write(i.Value.HashCode);
                        }
                    }

                    if (!string.IsNullOrEmpty(backupFile))
                    {
                        File.Delete(backupFile);
                    }
                }
                catch (Exception exception)
                {
                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }

                    if (!string.IsNullOrEmpty(backupFile))
                    {
                        File.Move(backupFile, file);
                    }

                    throw new GameFrameworkException(string.Format("Pack save exception '{0}'.", exception.Message), exception);
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                        fileStream = null;
                    }
                }
            }

            private void OnDownloadStart(object sender, DownloadStartEventArgs e)
            {
                UpdateInfo updateInfo = e.UserData as UpdateInfo;
                if (updateInfo == null)
                {
                    return;
                }

                if (m_DownloadManager == null)
                {
                    throw new GameFrameworkException("You must set download manager first.");
                }

                if (e.CurrentLength > updateInfo.ZipLength)
                {
                    m_DownloadManager.RemoveDownload(e.SerialId);
                    string downloadFile = string.Format("{0}.download", e.DownloadPath);
                    if (File.Exists(downloadFile))
                    {
                        File.Delete(downloadFile);
                    }

                    string errorMessage = string.Format("When download start, downloaded length is larger than zip length, need '{0}', current '{1}'.", updateInfo.ZipLength.ToString(), e.CurrentLength.ToString());
                    OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData));
                    return;
                }

                if (ResourceUpdateStart != null)
                {
                    ResourceUpdateStart(updateInfo.ResourceName, e.DownloadPath, e.DownloadUri, e.CurrentLength, updateInfo.ZipLength, updateInfo.RetryCount);
                }
            }

            private void OnDownloadUpdate(object sender, DownloadUpdateEventArgs e)
            {
                UpdateInfo updateInfo = e.UserData as UpdateInfo;
                if (updateInfo == null)
                {
                    return;
                }

                if (m_DownloadManager == null)
                {
                    throw new GameFrameworkException("You must set download manager first.");
                }

                if (e.CurrentLength > updateInfo.ZipLength)
                {
                    m_DownloadManager.RemoveDownload(e.SerialId);
                    string downloadFile = string.Format("{0}.download", e.DownloadPath);
                    if (File.Exists(downloadFile))
                    {
                        File.Delete(downloadFile);
                    }

                    string errorMessage = string.Format("When download update, downloaded length is larger than zip length, need '{0}', current '{1}'.", updateInfo.ZipLength.ToString(), e.CurrentLength.ToString());
                    OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData));
                    return;
                }

                if (ResourceUpdateChanged != null)
                {
                    ResourceUpdateChanged(updateInfo.ResourceName, e.DownloadPath, e.DownloadUri, e.CurrentLength, updateInfo.ZipLength);
                }
            }

            private void OnDownloadSuccess(object sender, DownloadSuccessEventArgs e)
            {
                UpdateInfo updateInfo = e.UserData as UpdateInfo;
                if (updateInfo == null)
                {
                    return;
                }

                bool zip = (updateInfo.Length != updateInfo.ZipLength || updateInfo.HashCode != updateInfo.ZipHashCode);
                byte[] bytes = File.ReadAllBytes(e.DownloadPath);

                if (updateInfo.ZipLength != bytes.Length)
                {
                    string errorMessage = string.Format("Zip length error, need '{0}', downloaded '{1}'.", updateInfo.ZipLength.ToString(), bytes.Length.ToString());
                    OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData));
                    return;
                }

                if (!zip)
                {
                    byte[] hashBytes = Utility.Converter.GetBytes(updateInfo.HashCode);
                    if (updateInfo.LoadType == LoadType.LoadFromMemoryAndQuickDecrypt)
                    {
                        bytes = Utility.Encryption.GetQuickXorBytes(bytes, hashBytes);
                    }
                    else if (updateInfo.LoadType == LoadType.LoadFromMemoryAndDecrypt)
                    {
                        bytes = Utility.Encryption.GetXorBytes(bytes, hashBytes);
                    }
                }

                int hashCode = Utility.Converter.GetInt32(Utility.Verifier.GetCrc32(bytes));
                if (updateInfo.ZipHashCode != hashCode)
                {
                    string errorMessage = string.Format("Zip hash code error, need '{0}', downloaded '{1}'.", updateInfo.ZipHashCode.ToString("X8"), hashCode.ToString("X8"));
                    OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData));
                    return;
                }

                if (zip)
                {
                    try
                    {
                        bytes = Utility.Zip.Decompress(bytes);
                    }
                    catch (Exception exception)
                    {
                        string errorMessage = string.Format("Unable to decompress from file '{0}' with error message '{1}'.", e.DownloadPath, exception.Message);
                        OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData));
                        return;
                    }

                    if (bytes == null)
                    {
                        string errorMessage = string.Format("Unable to decompress from file '{0}'.", e.DownloadPath);
                        OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData));
                        return;
                    }

                    if (updateInfo.Length != bytes.Length)
                    {
                        string errorMessage = string.Format("Resource length error, need '{0}', downloaded '{1}'.", updateInfo.Length.ToString(), bytes.Length.ToString());
                        OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData));
                        return;
                    }

                    File.WriteAllBytes(e.DownloadPath, bytes);
                }

                m_UpdatingCount--;

                if (m_ResourceManager.m_ResourceInfos.ContainsKey(updateInfo.ResourceName))
                {
                    throw new GameFrameworkException(string.Format("Resource info '{0}' is already exist.", updateInfo.ResourceName.FullName));
                }

                m_ResourceManager.m_ResourceInfos.Add(updateInfo.ResourceName, new ResourceInfo(updateInfo.ResourceName, updateInfo.LoadType, updateInfo.Length, updateInfo.HashCode, false));

                if (m_ResourceManager.m_ReadWriteResourceInfos.ContainsKey(updateInfo.ResourceName))
                {
                    throw new GameFrameworkException(string.Format("Read-write resource info '{0}' is already exist.", updateInfo.ResourceName.FullName));
                }

                m_ResourceManager.m_ReadWriteResourceInfos.Add(updateInfo.ResourceName, new ReadWriteResourceInfo(updateInfo.LoadType, updateInfo.Length, updateInfo.HashCode));

                GenerateReadWriteList();

                if (ResourceUpdateSuccess != null)
                {
                    ResourceUpdateSuccess(updateInfo.ResourceName, e.DownloadPath, e.DownloadUri, updateInfo.Length, updateInfo.ZipLength);
                }
            }

            private void OnDownloadFailure(object sender, DownloadFailureEventArgs e)
            {
                UpdateInfo updateInfo = e.UserData as UpdateInfo;
                if (updateInfo == null)
                {
                    return;
                }

                if (File.Exists(e.DownloadPath))
                {
                    File.Delete(e.DownloadPath);
                }

                if (ResourceUpdateFailure != null)
                {
                    ResourceUpdateFailure(updateInfo.ResourceName, e.DownloadUri, updateInfo.RetryCount, m_RetryCount, e.ErrorMessage);
                }

                if (updateInfo.RetryCount < m_RetryCount)
                {
                    m_UpdatingCount--;
                    UpdateInfo newUpdateInfo = new UpdateInfo(updateInfo.ResourceName, updateInfo.LoadType, updateInfo.Length, updateInfo.HashCode, updateInfo.ZipLength, updateInfo.ZipHashCode, updateInfo.DownloadPath, updateInfo.DownloadUri, updateInfo.RetryCount + 1);
                    if (m_UpdateAllowed)
                    {
                        m_UpdateWaitingInfo.Add(newUpdateInfo);
                    }
                    else
                    {
                        throw new GameFrameworkException("Update state error.");
                    }
                }
            }
        }
    }
}
