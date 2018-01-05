//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

namespace GameFramework.Resource
{
    internal partial class ResourceManager
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
                    throw new GameFrameworkException(string.Format("Package list '{0}' is invalid, error message is '{1}'.", fileUri, string.IsNullOrEmpty(errorMessage) ? "<Empty>" : errorMessage));
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes);
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                    {
                        memoryStream = null;
                        char[] header = binaryReader.ReadChars(3);
                        if (header[0] != PackageListHeader[0] || header[1] != PackageListHeader[1] || header[2] != PackageListHeader[2])
                        {
                            throw new GameFrameworkException("Package list header is invalid.");
                        }

                        byte listVersion = binaryReader.ReadByte();

                        if (listVersion == 0)
                        {
                            byte[] encryptBytes = binaryReader.ReadBytes(4);

                            m_ResourceManager.m_ApplicableGameVersion = Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), encryptBytes));
                            m_ResourceManager.m_InternalResourceVersion = binaryReader.ReadInt32();

                            int resourceCount = binaryReader.ReadInt32();
                            string[] names = new string[resourceCount];
                            string[] variants = new string[resourceCount];
                            int[] lengths = new int[resourceCount];
                            Dictionary<string, string[]> dependencyAssetNamesCollection = new Dictionary<string, string[]>();
                            for (int i = 0; i < resourceCount; i++)
                            {
                                names[i] = Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), encryptBytes));

                                variants[i] = null;
                                byte variantLength = binaryReader.ReadByte();
                                if (variantLength > 0)
                                {
                                    variants[i] = Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(variantLength), encryptBytes));
                                }

                                LoadType loadType = (LoadType)binaryReader.ReadByte();
                                lengths[i] = binaryReader.ReadInt32();
                                int hashCode = binaryReader.ReadInt32();

                                int assetNamesCount = binaryReader.ReadInt32();
                                string[] assetNames = new string[assetNamesCount];
                                for (int j = 0; j < assetNamesCount; j++)
                                {
                                    assetNames[j] = Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), Utility.Converter.GetBytes(hashCode)));

                                    int dependencyAssetNamesCount = binaryReader.ReadInt32();
                                    string[] dependencyAssetNames = new string[dependencyAssetNamesCount];
                                    for (int k = 0; k < dependencyAssetNamesCount; k++)
                                    {
                                        dependencyAssetNames[k] = Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), Utility.Converter.GetBytes(hashCode)));
                                    }

                                    if (variants[i] == null || variants[i] == m_CurrentVariant)
                                    {
                                        dependencyAssetNamesCollection.Add(assetNames[j], dependencyAssetNames);
                                    }
                                }

                                if (variants[i] == null || variants[i] == m_CurrentVariant)
                                {
                                    ResourceName resourceName = new ResourceName(names[i], variants[i]);
                                    ProcessAssetInfo(resourceName, assetNames);
                                    ProcessResourceInfo(resourceName, loadType, lengths[i], hashCode);
                                }
                            }

                            ProcessAssetDependencyInfo(dependencyAssetNamesCollection);

                            ResourceGroup resourceGroupAll = m_ResourceManager.GetResourceGroup(string.Empty);
                            for (int i = 0; i < resourceCount; i++)
                            {
                                resourceGroupAll.AddResource(names[i], variants[i], lengths[i]);
                            }

                            int resourceGroupCount = binaryReader.ReadInt32();
                            for (int i = 0; i < resourceGroupCount; i++)
                            {
                                string groupName = Utility.Converter.GetString(Utility.Encryption.GetXorBytes(binaryReader.ReadBytes(binaryReader.ReadByte()), encryptBytes));
                                ResourceGroup resourceGroup = m_ResourceManager.GetResourceGroup(groupName);
                                int groupResourceCount = binaryReader.ReadInt32();
                                for (int j = 0; j < groupResourceCount; j++)
                                {
                                    ushort versionIndex = binaryReader.ReadUInt16();
                                    if (versionIndex >= resourceCount)
                                    {
                                        throw new GameFrameworkException(string.Format("Package index '{0}' is invalid, resource count is '{1}'.", versionIndex, resourceCount));
                                    }

                                    resourceGroup.AddResource(names[versionIndex], variants[versionIndex], lengths[versionIndex]);
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

                    throw new GameFrameworkException(string.Format("Parse package list exception '{0}'.", exception.Message), exception);
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

            private void ProcessAssetInfo(ResourceName resourceName, string[] assetNames)
            {
                foreach (string assetName in assetNames)
                {
                    m_ResourceManager.m_AssetInfos.Add(assetName, new AssetInfo(assetName, resourceName));
                }
            }

            private void ProcessAssetDependencyInfo(Dictionary<string, string[]> dependencyAssetNamesCollection)
            {
                foreach (KeyValuePair<string, string[]> dependencyAssetNamesCollectionItem in dependencyAssetNamesCollection)
                {
                    List<string> dependencyAssetNames = new List<string>();
                    List<string> scatteredDependencyAssetNames = new List<string>();
                    foreach (string dependencyAssetName in dependencyAssetNamesCollectionItem.Value)
                    {
                        AssetInfo? assetInfo = m_ResourceManager.GetAssetInfo(dependencyAssetName);
                        if (assetInfo.HasValue)
                        {
                            dependencyAssetNames.Add(dependencyAssetName);
                        }
                        else
                        {
                            scatteredDependencyAssetNames.Add(dependencyAssetName);
                        }
                    }

                    m_ResourceManager.m_AssetDependencyInfos.Add(dependencyAssetNamesCollectionItem.Key, new AssetDependencyInfo(dependencyAssetNamesCollectionItem.Key, dependencyAssetNames.ToArray(), scatteredDependencyAssetNames.ToArray()));
                }
            }

            private void ProcessResourceInfo(ResourceName resourceName, LoadType loadType, int length, int hashCode)
            {
                if (m_ResourceManager.m_ResourceInfos.ContainsKey(resourceName))
                {
                    throw new GameFrameworkException(string.Format("Resource info '{0}' is already exist.", resourceName.FullName));
                }

                m_ResourceManager.m_ResourceInfos.Add(resourceName, new ResourceInfo(resourceName, loadType, length, hashCode, true));
            }
        }
    }
}
