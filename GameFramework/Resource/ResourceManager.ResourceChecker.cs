﻿//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GameFramework.Resource
{
    internal sealed partial class ResourceManager : GameFrameworkModule, IResourceManager
    {
        /// <summary>
        /// 资源检查器。
        /// </summary>
        private sealed partial class ResourceChecker
        {
            private readonly ResourceManager m_ResourceManager;
            private readonly Dictionary<ResourceName, CheckInfo> m_CheckInfos;
            private string m_CurrentVariant;
            private bool m_VersionListReady;
            private bool m_ReadOnlyListReady;
            private bool m_ReadWriteListReady;

            public GameFrameworkAction<ResourceName, LoadType, int, int, int, int> ResourceNeedUpdate;
            public GameFrameworkAction<int, int, long, long> ResourceCheckComplete;

            /// <summary>
            /// 初始化资源检查器的新实例。
            /// </summary>
            /// <param name="resourceManager">资源管理器。</param>
            public ResourceChecker(ResourceManager resourceManager)
            {
                m_ResourceManager = resourceManager;
                m_CheckInfos = new Dictionary<ResourceName, CheckInfo>();
                m_CurrentVariant = null;
                m_VersionListReady = false;
                m_ReadOnlyListReady = false;
                m_ReadWriteListReady = false;

                ResourceNeedUpdate = null;
                ResourceCheckComplete = null;
            }

            /// <summary>
            /// 关闭并清理资源检查器。
            /// </summary>
            public void Shutdown()
            {
                m_CheckInfos.Clear();
            }

            public void CheckResources(string currentVariant)
            {
                m_CurrentVariant = currentVariant;

                TryRecoverReadWriteList();

                if (m_ResourceManager.m_ResourceHelper == null)
                {
                    throw new GameFrameworkException("Resource helper is invalid.");
                }

                if (string.IsNullOrEmpty(m_ResourceManager.m_ReadOnlyPath))
                {
                    throw new GameFrameworkException("Readonly path is invalid.");
                }

                if (string.IsNullOrEmpty(m_ResourceManager.m_ReadWritePath))
                {
                    throw new GameFrameworkException("Read-write path is invalid.");
                }

                m_ResourceManager.m_ResourceHelper.LoadBytes(Utility.Path.GetRemotePath(m_ResourceManager.m_ReadWritePath, Utility.Path.GetResourceNameWithSuffix(VersionListFileName)), ParseVersionList);
                m_ResourceManager.m_ResourceHelper.LoadBytes(Utility.Path.GetRemotePath(m_ResourceManager.m_ReadOnlyPath, Utility.Path.GetResourceNameWithSuffix(ResourceListFileName)), ParseReadOnlyList);
                m_ResourceManager.m_ResourceHelper.LoadBytes(Utility.Path.GetRemotePath(m_ResourceManager.m_ReadWritePath, Utility.Path.GetResourceNameWithSuffix(ResourceListFileName)), ParseReadWriteList);
            }

            private void SetVersionInfo(ResourceName resourceName, LoadType loadType, int length, int hashCode, int zipLength, int zipHashCode)
            {
                GetOrAddCheckInfo(resourceName).SetVersionInfo(loadType, length, hashCode, zipLength, zipHashCode);
            }

            private void SetReadOnlyInfo(ResourceName resourceName, LoadType loadType, int length, int hashCode)
            {
                GetOrAddCheckInfo(resourceName).SetReadOnlyInfo(loadType, length, hashCode);
            }

            private void SetReadWriteInfo(ResourceName resourceName, LoadType loadType, int length, int hashCode)
            {
                GetOrAddCheckInfo(resourceName).SetReadWriteInfo(loadType, length, hashCode);
            }

            private CheckInfo GetOrAddCheckInfo(ResourceName resourceName)
            {
                CheckInfo checkInfo = null;
                if (m_CheckInfos.TryGetValue(resourceName, out checkInfo))
                {
                    return checkInfo;
                }

                checkInfo = new CheckInfo(resourceName);
                m_CheckInfos.Add(checkInfo.ResourceName, checkInfo);

                return checkInfo;
            }

            private void RefreshCheckInfoStatus()
            {
                if (!m_VersionListReady || !m_ReadOnlyListReady || !m_ReadWriteListReady)
                {
                    return;
                }

                int removedCount = 0;
                int updateCount = 0;
                long updateTotalLength = 0L;
                long updateTotalZipLength = 0L;
                foreach (KeyValuePair<ResourceName, CheckInfo> checkInfo in m_CheckInfos)
                {
                    CheckInfo ci = checkInfo.Value;
                    ci.RefreshStatus(m_CurrentVariant);

                    if (ci.Status == CheckInfo.CheckStatus.StorageInReadOnly)
                    {
                        ProcessResourceInfo(ci.ResourceName, ci.LoadType, ci.Length, ci.HashCode, true);
                    }
                    else if (ci.Status == CheckInfo.CheckStatus.StorageInReadWrite)
                    {
                        ProcessResourceInfo(ci.ResourceName, ci.LoadType, ci.Length, ci.HashCode, false);
                    }
                    else if (ci.Status == CheckInfo.CheckStatus.NeedUpdate)
                    {
                        updateCount++;
                        updateTotalLength += ci.Length;
                        updateTotalZipLength += ci.ZipLength;

                        ResourceNeedUpdate(ci.ResourceName, ci.LoadType, ci.Length, ci.HashCode, ci.ZipLength, ci.ZipHashCode);
                    }
                    else if (ci.Status == CheckInfo.CheckStatus.Disuse || ci.Status == CheckInfo.CheckStatus.Unavailable)
                    {
                        // Do nothing.
                    }
                    else
                    {
                        throw new GameFrameworkException(Utility.Text.Format("Check resources '{0}' error with unknown status.", ci.ResourceName.FullName));
                    }

                    if (ci.NeedRemove)
                    {
                        removedCount++;

                        string path = Utility.Path.GetCombinePath(m_ResourceManager.m_ReadWritePath, Utility.Path.GetResourceNameWithSuffix(ci.ResourceName.FullName));
                        File.Delete(path);

                        if (!m_ResourceManager.m_ReadWriteResourceInfos.ContainsKey(ci.ResourceName))
                        {
                            throw new GameFrameworkException(Utility.Text.Format("Resource '{0}' is not exist in read-write list.", ci.ResourceName.FullName));
                        }

                        m_ResourceManager.m_ReadWriteResourceInfos.Remove(ci.ResourceName);
                    }
                }

                ResourceCheckComplete(removedCount, updateCount, updateTotalLength, updateTotalZipLength);
            }

            /// <summary>
            /// 尝试恢复读写区资源列表。
            /// </summary>
            /// <returns>是否恢复成功。</returns>
            private bool TryRecoverReadWriteList()
            {
                string file = Utility.Path.GetCombinePath(m_ResourceManager.m_ReadWritePath, Utility.Path.GetResourceNameWithSuffix(ResourceListFileName));
                string backupFile = file + BackupFileSuffixName;

                try
                {
                    if (!File.Exists(backupFile))
                    {
                        return false;
                    }

                    if (File.Exists(file))
                    {
                        File.Delete(file);
                    }

                    File.Move(backupFile, file);
                }
                catch
                {
                    return false;
                }

                return true;
            }

            /// <summary>
            /// 解析版本资源列表。
            /// </summary>
            /// <param name="fileUri">版本资源列表文件路径。</param>
            /// <param name="bytes">要解析的数据。</param>
            /// <param name="errorMessage">错误信息。</param>
            private void ParseVersionList(string fileUri, byte[] bytes, string errorMessage)
            {
                if (m_VersionListReady)
                {
                    throw new GameFrameworkException("Version list has been parsed.");
                }

                if (bytes == null || bytes.Length <= 0)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Version list '{0}' is invalid, error message is '{1}'.", fileUri, string.IsNullOrEmpty(errorMessage) ? "<Empty>" : errorMessage));
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes, false);
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                    {
                        memoryStream = null;
                        if (binaryReader.ReadChar() != VersionListHeader[0] || binaryReader.ReadChar() != VersionListHeader[1] || binaryReader.ReadChar() != VersionListHeader[2])
                        {
                            throw new GameFrameworkException("Version list header is invalid.");
                        }

                        byte listVersion = binaryReader.ReadByte();

                        if (listVersion == 0)
                        {
                            byte[] encryptBytes = binaryReader.ReadBytes(4);

                            m_ResourceManager.m_ApplicableGameVersion = m_ResourceManager.GetEncryptedString(binaryReader, encryptBytes);
                            m_ResourceManager.m_InternalResourceVersion = binaryReader.ReadInt32();

                            int assetCount = binaryReader.ReadInt32();
                            m_ResourceManager.m_AssetInfos = new Dictionary<string, AssetInfo>(assetCount);
                            int resourceCount = binaryReader.ReadInt32();
                            m_ResourceManager.m_ResourceInfos = new Dictionary<ResourceName, ResourceInfo>(resourceCount, new ResourceNameComparer());

                            for (int i = 0; i < resourceCount; i++)
                            {
                                string name = m_ResourceManager.GetEncryptedString(binaryReader, encryptBytes);
                                string variant = m_ResourceManager.GetEncryptedString(binaryReader, encryptBytes);
                                ResourceName resourceName = new ResourceName(name, variant);

                                LoadType loadType = (LoadType)binaryReader.ReadByte();
                                int length = binaryReader.ReadInt32();
                                int hashCode = binaryReader.ReadInt32();
                                byte[] hashCodeBytes = Utility.Converter.GetBytes(hashCode);
                                int zipLength = binaryReader.ReadInt32();
                                int zipHashCode = binaryReader.ReadInt32();

                                int assetNamesCount = binaryReader.ReadInt32();
                                for (int j = 0; j < assetNamesCount; j++)
                                {
                                    string assetName = m_ResourceManager.GetEncryptedString(binaryReader, hashCodeBytes);

                                    int dependencyAssetNamesCount = binaryReader.ReadInt32();
                                    string[] dependencyAssetNames = new string[dependencyAssetNamesCount];
                                    for (int k = 0; k < dependencyAssetNamesCount; k++)
                                    {
                                        dependencyAssetNames[k] = m_ResourceManager.GetEncryptedString(binaryReader, hashCodeBytes);
                                    }

                                    if (variant == null || variant == m_CurrentVariant)
                                    {
                                        m_ResourceManager.m_AssetInfos.Add(assetName, new AssetInfo(assetName, resourceName, dependencyAssetNames));
                                    }
                                }

                                SetVersionInfo(resourceName, loadType, length, hashCode, zipLength, zipHashCode);
                            }
                        }
                        else
                        {
                            throw new GameFrameworkException("Version list version is invalid.");
                        }
                    }

                    m_VersionListReady = true;
                    RefreshCheckInfoStatus();
                }
                catch (Exception exception)
                {
                    if (exception is GameFrameworkException)
                    {
                        throw;
                    }

                    throw new GameFrameworkException(Utility.Text.Format("Parse version list exception '{0}'.", exception.Message), exception);
                }
                finally
                {
                    if (memoryStream != null)
                    {
                        memoryStream.Dispose();
                        memoryStream = null;
                    }
                }
            }

            /// <summary>
            /// 解析只读区资源列表。
            /// </summary>
            /// <param name="fileUri">只读区资源列表文件路径。</param>
            /// <param name="bytes">要解析的数据。</param>
            /// <param name="errorMessage">错误信息。</param>
            private void ParseReadOnlyList(string fileUri, byte[] bytes, string errorMessage)
            {
                if (m_ReadOnlyListReady)
                {
                    throw new GameFrameworkException("Readonly list has been parsed.");
                }

                if (bytes == null || bytes.Length <= 0)
                {
                    m_ReadOnlyListReady = true;
                    RefreshCheckInfoStatus();
                    return;
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes, false);
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                    {
                        memoryStream = null;
                        if (binaryReader.ReadChar() != ReadOnlyListHeader[0] || binaryReader.ReadChar() != ReadOnlyListHeader[1] || binaryReader.ReadChar() != ReadOnlyListHeader[2])
                        {
                            throw new GameFrameworkException("Readonly list header is invalid.");
                        }

                        byte listVersion = binaryReader.ReadByte();

                        if (listVersion == 0)
                        {
                            byte[] encryptBytes = binaryReader.ReadBytes(4);

                            int resourceCount = binaryReader.ReadInt32();
                            for (int i = 0; i < resourceCount; i++)
                            {
                                string name = m_ResourceManager.GetEncryptedString(binaryReader, encryptBytes);
                                string variant = m_ResourceManager.GetEncryptedString(binaryReader, encryptBytes);

                                LoadType loadType = (LoadType)binaryReader.ReadByte();
                                int length = binaryReader.ReadInt32();
                                int hashCode = binaryReader.ReadInt32();

                                SetReadOnlyInfo(new ResourceName(name, variant), loadType, length, hashCode);
                            }
                        }
                        else
                        {
                            throw new GameFrameworkException("Readonly list version is invalid.");
                        }
                    }

                    m_ReadOnlyListReady = true;
                    RefreshCheckInfoStatus();
                }
                catch (Exception exception)
                {
                    if (exception is GameFrameworkException)
                    {
                        throw;
                    }

                    throw new GameFrameworkException(Utility.Text.Format("Parse readonly list exception '{0}'.", exception.Message), exception);
                }
                finally
                {
                    if (memoryStream != null)
                    {
                        memoryStream.Dispose();
                        memoryStream = null;
                    }
                }
            }

            /// <summary>
            /// 解析读写区资源列表。
            /// </summary>
            /// <param name="fileUri">读写区资源列表文件路径。</param>
            /// <param name="bytes">要解析的数据。</param>
            /// <param name="errorMessage">错误信息。</param>
            private void ParseReadWriteList(string fileUri, byte[] bytes, string errorMessage)
            {
                if (m_ReadWriteListReady)
                {
                    throw new GameFrameworkException("Read-write list has been parsed.");
                }

                if (bytes == null || bytes.Length <= 0)
                {
                    m_ReadWriteListReady = true;
                    RefreshCheckInfoStatus();
                    return;
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes, false);
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                    {
                        memoryStream = null;
                        if (binaryReader.ReadChar() != ReadWriteListHeader[0] || binaryReader.ReadChar() != ReadWriteListHeader[1] || binaryReader.ReadChar() != ReadWriteListHeader[2])
                        {
                            throw new GameFrameworkException("Read-write list header is invalid.");
                        }

                        byte listVersion = binaryReader.ReadByte();

                        if (listVersion == 0)
                        {
                            byte[] encryptBytes = binaryReader.ReadBytes(4);

                            int resourceCount = binaryReader.ReadInt32();
                            for (int i = 0; i < resourceCount; i++)
                            {
                                string name = m_ResourceManager.GetEncryptedString(binaryReader, encryptBytes);
                                string variant = m_ResourceManager.GetEncryptedString(binaryReader, encryptBytes);

                                LoadType loadType = (LoadType)binaryReader.ReadByte();
                                int length = binaryReader.ReadInt32();
                                int hashCode = binaryReader.ReadInt32();

                                SetReadWriteInfo(new ResourceName(name, variant), loadType, length, hashCode);

                                ResourceName resourceName = new ResourceName(name, variant);
                                if (m_ResourceManager.m_ReadWriteResourceInfos.ContainsKey(resourceName))
                                {
                                    throw new GameFrameworkException(Utility.Text.Format("Read-write resource info '{0}' is already exist.", resourceName.FullName));
                                }

                                m_ResourceManager.m_ReadWriteResourceInfos.Add(resourceName, new ReadWriteResourceInfo(loadType, length, hashCode));
                            }
                        }
                        else
                        {
                            throw new GameFrameworkException("Read-write list version is invalid.");
                        }
                    }

                    m_ReadWriteListReady = true;
                    RefreshCheckInfoStatus();
                }
                catch (Exception exception)
                {
                    if (exception is GameFrameworkException)
                    {
                        throw;
                    }

                    throw new GameFrameworkException(Utility.Text.Format("Parse read-write list exception '{0}'.", exception.Message), exception);
                }
                finally
                {
                    if (memoryStream != null)
                    {
                        memoryStream.Dispose();
                        memoryStream = null;
                    }
                }
            }

            private void ProcessResourceInfo(ResourceName resourceName, LoadType loadType, int length, int hashCode, bool storageInReadOnly)
            {
                if (m_ResourceManager.m_ResourceInfos.ContainsKey(resourceName))
                {
                    throw new GameFrameworkException(Utility.Text.Format("Resource info '{0}' is already exist.", resourceName.FullName));
                }

                m_ResourceManager.m_ResourceInfos.Add(resourceName, new ResourceInfo(resourceName, loadType, length, hashCode, storageInReadOnly));
            }
        }
    }
}
