//------------------------------------------------------------
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
        /// 资源初始化器。
        /// </summary>
        private sealed class ResourceIniter
        {
            private readonly ResourceManager m_ResourceManager;
            private string m_CurrentVariant;

            public GameFrameworkAction ResourceInitComplete;

            /// <summary>
            /// 初始化资源初始化器的新实例。
            /// </summary>
            /// <param name="resourceManager">资源管理器。</param>
            public ResourceIniter(ResourceManager resourceManager)
            {
                m_ResourceManager = resourceManager;
                m_CurrentVariant = null;

                ResourceInitComplete = null;
            }

            /// <summary>
            /// 关闭并清理资源初始化器。
            /// </summary>
            public void Shutdown()
            {
            }

            /// <summary>
            /// 初始化资源。
            /// </summary>
            public void InitResources(string currentVariant)
            {
                m_CurrentVariant = currentVariant;

                if (m_ResourceManager.m_ResourceHelper == null)
                {
                    throw new GameFrameworkException("Resource helper is invalid.");
                }

                if (string.IsNullOrEmpty(m_ResourceManager.m_ReadOnlyPath))
                {
                    throw new GameFrameworkException("Readonly path is invalid.");
                }

                m_ResourceManager.m_ResourceHelper.LoadBytes(Utility.Path.GetRemotePath(m_ResourceManager.m_ReadOnlyPath, Utility.Path.GetResourceNameWithSuffix(VersionListFileName)), ParsePackageList);
            }

            /// <summary>
            /// 解析资源包资源列表。
            /// </summary>
            /// <param name="fileUri">版本资源列表文件路径。</param>
            /// <param name="bytes">要解析的数据。</param>
            /// <param name="errorMessage">错误信息。</param>
            private void ParsePackageList(string fileUri, byte[] bytes, string errorMessage)
            {
                if (bytes == null || bytes.Length <= 0)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Package list '{0}' is invalid, error message is '{1}'.", fileUri, string.IsNullOrEmpty(errorMessage) ? "<Empty>" : errorMessage));
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes, false);
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                    {
                        memoryStream = null;
                        if (binaryReader.ReadChar() != PackageListHeader[0] || binaryReader.ReadChar() != PackageListHeader[1] || binaryReader.ReadChar() != PackageListHeader[2])
                        {
                            throw new GameFrameworkException("Package list header is invalid.");
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
                            ResourceLength[] resourceLengths = new ResourceLength[resourceCount];

                            for (int i = 0; i < resourceCount; i++)
                            {
                                string name = m_ResourceManager.GetEncryptedString(binaryReader, encryptBytes);
                                string variant = m_ResourceManager.GetEncryptedString(binaryReader, encryptBytes);
                                ResourceName resourceName = new ResourceName(name, variant);

                                LoadType loadType = (LoadType)binaryReader.ReadByte();
                                int length = binaryReader.ReadInt32();
                                int hashCode = binaryReader.ReadInt32();
                                byte[] hashCodeBytes = Utility.Converter.GetBytes(hashCode);
                                resourceLengths[i] = new ResourceLength(resourceName, length, length);

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

                                if (variant == null || variant == m_CurrentVariant)
                                {
                                    ProcessResourceInfo(resourceName, loadType, length, hashCode);
                                }
                            }

                            ResourceGroup defaultResourceGroup = m_ResourceManager.GetOrAddResourceGroup(string.Empty);
                            for (int i = 0; i < resourceCount; i++)
                            {
                                if (resourceLengths[i].ResourceName.Variant == null || resourceLengths[i].ResourceName.Variant == m_CurrentVariant)
                                {
                                    defaultResourceGroup.AddResource(resourceLengths[i].ResourceName, resourceLengths[i].Length, resourceLengths[i].ZipLength);
                                }
                            }

                            int resourceGroupCount = binaryReader.ReadInt32();
                            for (int i = 0; i < resourceGroupCount; i++)
                            {
                                string resourceGroupName = m_ResourceManager.GetEncryptedString(binaryReader, encryptBytes);
                                ResourceGroup resourceGroup = m_ResourceManager.GetOrAddResourceGroup(resourceGroupName);
                                int resourceGroupResourceCount = binaryReader.ReadInt32();
                                for (int j = 0; j < resourceGroupResourceCount; j++)
                                {
                                    ushort index = binaryReader.ReadUInt16();
                                    if (index >= resourceCount)
                                    {
                                        throw new GameFrameworkException(Utility.Text.Format("Package index '{0}' is invalid, resource count is '{1}'.", index.ToString(), resourceCount.ToString()));
                                    }

                                    if (resourceLengths[index].ResourceName.Variant == null || resourceLengths[index].ResourceName.Variant == m_CurrentVariant)
                                    {
                                        resourceGroup.AddResource(resourceLengths[index].ResourceName, resourceLengths[index].Length, resourceLengths[index].ZipLength);
                                    }
                                }
                            }
                        }
                        else
                        {
                            throw new GameFrameworkException("Package list version is invalid.");
                        }
                    }

                    ResourceInitComplete();
                }
                catch (Exception exception)
                {
                    if (exception is GameFrameworkException)
                    {
                        throw;
                    }

                    throw new GameFrameworkException(Utility.Text.Format("Parse package list exception '{0}'.", exception.Message), exception);
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

            private void ProcessResourceInfo(ResourceName resourceName, LoadType loadType, int length, int hashCode)
            {
                if (m_ResourceManager.m_ResourceInfos.ContainsKey(resourceName))
                {
                    throw new GameFrameworkException(Utility.Text.Format("Resource info '{0}' is already exist.", resourceName.FullName));
                }

                m_ResourceManager.m_ResourceInfos.Add(resourceName, new ResourceInfo(resourceName, loadType, length, hashCode, true));
            }
        }
    }
}
