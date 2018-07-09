//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Download;
using System;
using System.IO;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
    {
        /// <summary>
        /// 版本资源列表处理器。
        /// </summary>
        private sealed class VersionListProcessor
        {
            private readonly ResourceManager m_ResourceManager;
            private IDownloadManager m_DownloadManager;
            private int m_VersionListLength;
            private int m_VersionListHashCode;
            private int m_VersionListZipLength;
            private int m_VersionListZipHashCode;

            public GameFrameworkAction<string, string> VersionListUpdateSuccess;
            public GameFrameworkAction<string, string> VersionListUpdateFailure;

            /// <summary>
            /// 初始化版本资源列表处理器的新实例。
            /// </summary>
            /// <param name="resourceManager">资源管理器。</param>
            public VersionListProcessor(ResourceManager resourceManager)
            {
                m_ResourceManager = resourceManager;
                m_DownloadManager = null;
                m_VersionListLength = 0;
                m_VersionListHashCode = 0;
                m_VersionListZipLength = 0;
                m_VersionListZipHashCode = 0;

                VersionListUpdateSuccess = null;
                VersionListUpdateFailure = null;
            }

            /// <summary>
            /// 关闭并清理版本资源列表处理器。
            /// </summary>
            public void Shutdown()
            {
                if (m_DownloadManager != null)
                {
                    m_DownloadManager.DownloadSuccess -= OnDownloadSuccess;
                    m_DownloadManager.DownloadFailure -= OnDownloadFailure;
                }
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
                m_DownloadManager.DownloadSuccess += OnDownloadSuccess;
                m_DownloadManager.DownloadFailure += OnDownloadFailure;
            }

            /// <summary>
            /// 检查版本资源列表。
            /// </summary>
            /// <param name="latestInternalResourceVersion">最新的内部资源版本号。</param>
            /// <returns>检查版本资源列表结果。</returns>
            public CheckVersionListResult CheckVersionList(int latestInternalResourceVersion)
            {
                string applicableGameVersion = null;
                int internalResourceVersion = 0;

                string versionListFileName = Utility.Path.GetCombinePath(m_ResourceManager.m_ReadWritePath, Utility.Path.GetResourceNameWithSuffix(VersionListFileName));
                if (!File.Exists(versionListFileName))
                {
                    Log.Debug("Latest internal resource version is '{0}', local resource version is not exist.", latestInternalResourceVersion.ToString());
                    return CheckVersionListResult.NeedUpdate;
                }

                FileStream fileStream = null;
                try
                {
                    fileStream = new FileStream(versionListFileName, FileMode.Open, FileAccess.Read);
                    using (BinaryReader binaryReader = new BinaryReader(fileStream))
                    {
                        fileStream = null;
                        char[] header = binaryReader.ReadChars(3);
                        if (header[0] != VersionListHeader[0] || header[1] != VersionListHeader[1] || header[2] != VersionListHeader[2])
                        {
                            Log.Debug("Latest internal resource version is '{0}', local resource version is invalid.", latestInternalResourceVersion.ToString());
                            return CheckVersionListResult.NeedUpdate;
                        }

                        byte listVersion = binaryReader.ReadByte();

                        if (listVersion == 0)
                        {
                            byte[] encryptBytes = binaryReader.ReadBytes(4);
                            applicableGameVersion = Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), encryptBytes));
                            internalResourceVersion = binaryReader.ReadInt32();
                        }
                        else
                        {
                            throw new GameFrameworkException("Version list version is invalid.");
                        }
                    }
                }
                catch
                {
                    Log.Debug("Latest internal resource version is '{0}', local resource version is invalid.", latestInternalResourceVersion.ToString());
                    return CheckVersionListResult.NeedUpdate;
                }
                finally
                {
                    if (fileStream != null)
                    {
                        fileStream.Dispose();
                        fileStream = null;
                    }
                }

                if (internalResourceVersion != latestInternalResourceVersion)
                {
                    Log.Debug("Applicable game version is '{0}', latest internal resource version is '{1}', local resource version is '{2}'.", applicableGameVersion, latestInternalResourceVersion.ToString(), internalResourceVersion.ToString());
                    return CheckVersionListResult.NeedUpdate;
                }

                Log.Debug("Applicable game version is '{0}', latest internal resource version is '{1}', local resource version is up-to-date.", applicableGameVersion, latestInternalResourceVersion.ToString());
                return CheckVersionListResult.Updated;
            }

            /// <summary>
            /// 更新版本资源列表。
            /// </summary>
            /// <param name="versionListLength">版本资源列表大小。</param>
            /// <param name="versionListHashCode">版本资源列表哈希值。</param>
            /// <param name="versionListZipLength">版本资源列表压缩后大小。</param>
            /// <param name="versionListZipHashCode">版本资源列表压缩后哈希值。</param>
            public void UpdateVersionList(int versionListLength, int versionListHashCode, int versionListZipLength, int versionListZipHashCode)
            {
                if (m_DownloadManager == null)
                {
                    throw new GameFrameworkException("You must set download manager first.");
                }

                m_VersionListLength = versionListLength;
                m_VersionListHashCode = versionListHashCode;
                m_VersionListZipLength = versionListZipLength;
                m_VersionListZipHashCode = versionListZipHashCode;
                string versionListFileName = Utility.Path.GetResourceNameWithSuffix(VersionListFileName);
                string latestVersionListFileName = Utility.Path.GetResourceNameWithCrc32AndSuffix(VersionListFileName, m_VersionListHashCode);
                string localVersionListFilePath = Utility.Path.GetCombinePath(m_ResourceManager.m_ReadWritePath, versionListFileName);
                string latestVersionListFileUri = Utility.Path.GetRemotePath(m_ResourceManager.m_UpdatePrefixUri, latestVersionListFileName);
                m_DownloadManager.AddDownload(localVersionListFilePath, latestVersionListFileUri, this);
            }

            private void OnDownloadSuccess(object sender, DownloadSuccessEventArgs e)
            {
                VersionListProcessor versionListProcessor = e.UserData as VersionListProcessor;
                if (versionListProcessor == null || versionListProcessor != this)
                {
                    return;
                }

                byte[] bytes = File.ReadAllBytes(e.DownloadPath);
                if (m_VersionListZipLength != bytes.Length)
                {
                    string errorMessage = string.Format("Latest version list zip length error, need '{0}', downloaded '{1}'.", m_VersionListZipLength.ToString(), bytes.Length.ToString());
                    OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData));
                    return;
                }

                int hashCode = Utility.Converter.GetInt32(Utility.Verifier.GetCrc32(bytes));
                if (m_VersionListZipHashCode != hashCode)
                {
                    string errorMessage = string.Format("Latest version list zip hash code error, need '{0}', downloaded '{1}'.", m_VersionListZipHashCode.ToString("X8"), hashCode.ToString("X8"));
                    OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData));
                    return;
                }

                try
                {
                    bytes = Utility.Zip.Decompress(bytes);
                }
                catch (Exception exception)
                {
                    string errorMessage = string.Format("Unable to decompress latest version list '{0}' with error message '{1}'.", e.DownloadPath, exception.Message);
                    OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData));
                    return;
                }

                if (bytes == null)
                {
                    string errorMessage = string.Format("Unable to decompress latest version list '{0}'.", e.DownloadPath);
                    OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData));
                    return;
                }

                if (m_VersionListLength != bytes.Length)
                {
                    string errorMessage = string.Format("Latest version list length error, need '{0}', downloaded '{1}'.", m_VersionListLength.ToString(), bytes.Length.ToString());
                    OnDownloadFailure(this, new DownloadFailureEventArgs(e.SerialId, e.DownloadPath, e.DownloadUri, errorMessage, e.UserData));
                    return;
                }

                File.WriteAllBytes(e.DownloadPath, bytes);

                if (VersionListUpdateSuccess != null)
                {
                    VersionListUpdateSuccess(e.DownloadPath, e.DownloadUri);
                }
            }

            private void OnDownloadFailure(object sender, DownloadFailureEventArgs e)
            {
                VersionListProcessor versionListProcessor = e.UserData as VersionListProcessor;
                if (versionListProcessor == null || versionListProcessor != this)
                {
                    return;
                }

                if (File.Exists(e.DownloadPath))
                {
                    File.Delete(e.DownloadPath);
                }

                if (VersionListUpdateFailure != null)
                {
                    VersionListUpdateFailure(e.DownloadUri, e.ErrorMessage);
                }
            }
        }
    }
}
