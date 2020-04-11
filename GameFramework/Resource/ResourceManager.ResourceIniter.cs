//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

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

                m_ResourceManager.m_ResourceHelper.LoadBytes(Utility.Path.GetRemotePath(Path.Combine(m_ResourceManager.m_ReadOnlyPath, Utility.Path.GetResourceNameWithSuffix(VersionListFileName))), ParsePackageVersionList);
            }

            /// <summary>
            /// 解析资源包版本资源列表。
            /// </summary>
            /// <param name="fileUri">版本资源列表文件路径。</param>
            /// <param name="bytes">要解析的数据。</param>
            /// <param name="errorMessage">错误信息。</param>
            private void ParsePackageVersionList(string fileUri, byte[] bytes, string errorMessage)
            {
                if (bytes == null || bytes.Length <= 0)
                {
                    throw new GameFrameworkException(Utility.Text.Format("Package version list '{0}' is invalid, error message is '{1}'.", fileUri, string.IsNullOrEmpty(errorMessage) ? "<Empty>" : errorMessage));
                }

                MemoryStream memoryStream = null;
                try
                {
                    memoryStream = new MemoryStream(bytes, false);
                    PackageVersionList versionList = m_ResourceManager.m_PackageVersionListSerializer.Deserialize(memoryStream);
                    if (!versionList.IsValid)
                    {
                        throw new GameFrameworkException("Deserialize package version list failure.");
                    }

                    PackageVersionList.Asset[] assets = versionList.GetAssets();
                    PackageVersionList.Resource[] resources = versionList.GetResources();
                    PackageVersionList.ResourceGroup[] resourceGroups = versionList.GetResourceGroups();
                    m_ResourceManager.m_ApplicableGameVersion = versionList.ApplicableGameVersion;
                    m_ResourceManager.m_InternalResourceVersion = versionList.InternalResourceVersion;
                    m_ResourceManager.m_AssetInfos = new Dictionary<string, AssetInfo>(assets.Length);
                    m_ResourceManager.m_ResourceInfos = new Dictionary<ResourceName, ResourceInfo>(resources.Length, new ResourceNameComparer());
                    ResourceGroup defaultResourceGroup = m_ResourceManager.GetOrAddResourceGroup(string.Empty);

                    foreach (PackageVersionList.Resource resource in resources)
                    {
                        ResourceName resourceName = new ResourceName(resource.Name, resource.Variant);
                        int[] assetIndexes = resource.GetAssetIndexes();
                        foreach (int assetIndex in assetIndexes)
                        {
                            PackageVersionList.Asset asset = assets[assetIndex];
                            int[] dependencyAssetIndexes = asset.GetDependencyAssetIndexes();
                            string[] dependencyAssetNames = new string[dependencyAssetIndexes.Length];
                            int index = 0;
                            foreach (int dependencyAssetIndex in dependencyAssetIndexes)
                            {
                                dependencyAssetNames[index++] = assets[dependencyAssetIndex].Name;
                            }

                            if (resource.Variant == null || resource.Variant == m_CurrentVariant)
                            {
                                m_ResourceManager.m_AssetInfos.Add(asset.Name, new AssetInfo(asset.Name, resourceName, dependencyAssetNames));
                            }
                        }

                        if (resource.Variant == null || resource.Variant == m_CurrentVariant)
                        {
                            m_ResourceManager.m_ResourceInfos.Add(resourceName, new ResourceInfo(resourceName, (LoadType)resource.LoadType, resource.Length, resource.HashCode, true));
                            defaultResourceGroup.AddResource(resourceName, resource.Length, resource.Length);
                        }
                    }

                    foreach (PackageVersionList.ResourceGroup resourceGroup in resourceGroups)
                    {
                        ResourceGroup group = m_ResourceManager.GetOrAddResourceGroup(resourceGroup.Name);
                        int[] resourceIndexes = resourceGroup.GetResourceIndexes();
                        foreach (int resourceIndex in resourceIndexes)
                        {
                            if (resources[resourceIndex].Variant == null || resources[resourceIndex].Variant == m_CurrentVariant)
                            {
                                group.AddResource(new ResourceName(resources[resourceIndex].Name, resources[resourceIndex].Variant), resources[resourceIndex].Length, resources[resourceIndex].Length);
                            }
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

                    throw new GameFrameworkException(Utility.Text.Format("Parse package version list exception '{0}'.", exception.ToString()), exception);
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
        }
    }
}
