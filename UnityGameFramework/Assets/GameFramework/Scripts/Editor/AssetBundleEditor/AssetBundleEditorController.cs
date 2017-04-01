//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    internal sealed partial class AssetBundleEditorController
    {
        private const string ConfigurationName = "GameFramework/Configs/AssetBundleEditor.xml";
        private const string DefaultSourceAssetRootPath = "Assets";
        private readonly AssetBundleCollection m_AssetBundleCollection;
        private readonly List<string> m_SourceAssetSearchPaths;
        private readonly List<string> m_SourceAssetSearchRelativePaths;
        private readonly Dictionary<string, SourceAsset> m_SourceAssets;
        private SourceFolder m_SourceAssetRoot;
        private string m_SourceAssetRootPath;
        private string m_SourceAssetUnionTypeFilter;
        private string m_SourceAssetUnionLabelFilter;
        private string m_SourceAssetExceptTypeFilter;
        private string m_SourceAssetExceptLabelFilter;
        private AssetSorterType m_AssetSorter;

        public AssetBundleEditorController()
        {
            m_AssetBundleCollection = new AssetBundleCollection();

            m_AssetBundleCollection.OnLoadingAssetBundle += delegate (int index, int count)
            {
                if (OnLoadingAssetBundle != null)
                {
                    OnLoadingAssetBundle(index, count);
                }
            };

            m_AssetBundleCollection.OnLoadingAsset += delegate (int index, int count)
            {
                if (OnLoadingAsset != null)
                {
                    OnLoadingAsset(index, count);
                }
            };

            m_AssetBundleCollection.OnLoadCompleted += delegate ()
            {
                if (OnLoadCompleted != null)
                {
                    OnLoadCompleted();
                }
            };

            m_SourceAssetSearchPaths = new List<string>();
            m_SourceAssetSearchRelativePaths = new List<string>();
            m_SourceAssets = new Dictionary<string, SourceAsset>();
            m_SourceAssetRoot = null;
            m_SourceAssetRootPath = null;
            m_SourceAssetUnionTypeFilter = null;
            m_SourceAssetUnionLabelFilter = null;
            m_SourceAssetExceptTypeFilter = null;
            m_SourceAssetExceptLabelFilter = null;
            m_AssetSorter = AssetSorterType.Path;

            SourceAssetRootPath = DefaultSourceAssetRootPath;
        }

        public int AssetBundleCount
        {
            get
            {
                return m_AssetBundleCollection.AssetBundleCount;
            }
        }

        public int AssetCount
        {
            get
            {
                return m_AssetBundleCollection.AssetCount;
            }
        }

        public SourceFolder SourceAssetRoot
        {
            get
            {
                return m_SourceAssetRoot;
            }
        }

        public string SourceAssetRootPath
        {
            get
            {
                return m_SourceAssetRootPath;
            }
            set
            {
                if (m_SourceAssetRootPath == value)
                {
                    return;
                }

                m_SourceAssetRootPath = value.Replace('\\', '/');
                m_SourceAssetRoot = new SourceFolder(m_SourceAssetRootPath, null);
                RefreshSourceAssetSearchPaths();
            }
        }

        public string SourceAssetUnionTypeFilter
        {
            get
            {
                return m_SourceAssetUnionTypeFilter;
            }
            set
            {
                if (m_SourceAssetUnionTypeFilter == value)
                {
                    return;
                }

                m_SourceAssetUnionTypeFilter = value;
            }
        }

        public string SourceAssetUnionLabelFilter
        {
            get
            {
                return m_SourceAssetUnionLabelFilter;
            }
            set
            {
                if (m_SourceAssetUnionLabelFilter == value)
                {
                    return;
                }

                m_SourceAssetUnionLabelFilter = value;
            }
        }

        public string SourceAssetExceptTypeFilter
        {
            get
            {
                return m_SourceAssetExceptTypeFilter;
            }
            set
            {
                if (m_SourceAssetExceptTypeFilter == value)
                {
                    return;
                }

                m_SourceAssetExceptTypeFilter = value;
            }
        }

        public string SourceAssetExceptLabelFilter
        {
            get
            {
                return m_SourceAssetExceptLabelFilter;
            }
            set
            {
                if (m_SourceAssetExceptLabelFilter == value)
                {
                    return;
                }

                m_SourceAssetExceptLabelFilter = value;
            }
        }

        public AssetSorterType AssetSorter
        {
            get
            {
                return m_AssetSorter;
            }
            set
            {
                if (m_AssetSorter == value)
                {
                    return;
                }

                m_AssetSorter = value;
            }
        }

        public event GameFrameworkAction<int, int> OnLoadingAssetBundle = null;

        public event GameFrameworkAction<int, int> OnLoadingAsset = null;

        public event GameFrameworkAction OnLoadCompleted = null;

        public event GameFrameworkAction<SourceAsset[]> OnAssetAssigned = null;

        public event GameFrameworkAction<SourceAsset[]> OnAssetUnassigned = null;

        public bool Load()
        {
            string configurationName = Utility.Path.GetCombinePath(Application.dataPath, ConfigurationName);
            if (!File.Exists(configurationName))
            {
                return false;
            }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(configurationName);
                XmlNode xmlRoot = xmlDocument.SelectSingleNode("UnityGameFramework");
                XmlNode xmlEditor = xmlRoot.SelectSingleNode("AssetBundleEditor");
                XmlNode xmlSettings = xmlEditor.SelectSingleNode("Settings");

                XmlNodeList xmlNodeList = null;
                XmlNode xmlNode = null;

                xmlNodeList = xmlSettings.ChildNodes;
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    xmlNode = xmlNodeList.Item(i);
                    switch (xmlNode.Name)
                    {
                        case "SourceAssetRootPath":
                            SourceAssetRootPath = xmlNode.InnerText;
                            break;
                        case "SourceAssetSearchPaths":
                            m_SourceAssetSearchRelativePaths.Clear();
                            XmlNodeList xmlNodeListInner = xmlNode.ChildNodes;
                            XmlNode xmlNodeInner = null;
                            for (int j = 0; j < xmlNodeListInner.Count; j++)
                            {
                                xmlNodeInner = xmlNodeListInner.Item(j);
                                if (xmlNodeInner.Name != "SourceAssetSearchPath")
                                {
                                    continue;
                                }

                                m_SourceAssetSearchRelativePaths.Add(xmlNodeInner.Attributes.GetNamedItem("RelativePath").Value);
                            }
                            break;
                        case "SourceAssetUnionTypeFilter":
                            SourceAssetUnionTypeFilter = xmlNode.InnerText;
                            break;
                        case "SourceAssetUnionLabelFilter":
                            SourceAssetUnionLabelFilter = xmlNode.InnerText;
                            break;
                        case "SourceAssetExceptTypeFilter":
                            SourceAssetExceptTypeFilter = xmlNode.InnerText;
                            break;
                        case "SourceAssetExceptLabelFilter":
                            SourceAssetExceptLabelFilter = xmlNode.InnerText;
                            break;
                        case "AssetSorter":
                            AssetSorter = (AssetSorterType)Enum.Parse(typeof(AssetSorterType), xmlNode.InnerText);
                            break;
                    }
                }

                RefreshSourceAssetSearchPaths();
            }
            catch
            {
                File.Delete(configurationName);
                return false;
            }

            ScanSourceAssets();

            m_AssetBundleCollection.Load();

            return true;
        }

        public bool Save()
        {
            string configurationName = Utility.Path.GetCombinePath(Application.dataPath, ConfigurationName);
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.AppendChild(xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", null));

                XmlElement xmlRoot = xmlDocument.CreateElement("UnityGameFramework");
                xmlDocument.AppendChild(xmlRoot);

                XmlElement xmlEditor = xmlDocument.CreateElement("AssetBundleEditor");
                xmlRoot.AppendChild(xmlEditor);

                XmlElement xmlSettings = xmlDocument.CreateElement("Settings");
                xmlEditor.AppendChild(xmlSettings);

                XmlElement xmlElement = null;
                XmlAttribute xmlAttribute = null;

                xmlElement = xmlDocument.CreateElement("SourceAssetRootPath");
                xmlElement.InnerText = SourceAssetRootPath.ToString();
                xmlSettings.AppendChild(xmlElement);

                xmlElement = xmlDocument.CreateElement("SourceAssetSearchPaths");
                xmlSettings.AppendChild(xmlElement);

                foreach (string sourceAssetSearchRelativePath in m_SourceAssetSearchRelativePaths)
                {
                    XmlElement xmlElementInner = xmlDocument.CreateElement("SourceAssetSearchPath");
                    xmlAttribute = xmlDocument.CreateAttribute("RelativePath");
                    xmlAttribute.Value = sourceAssetSearchRelativePath;
                    xmlElementInner.Attributes.SetNamedItem(xmlAttribute);
                    xmlElement.AppendChild(xmlElementInner);
                }

                xmlElement = xmlDocument.CreateElement("SourceAssetUnionTypeFilter");
                xmlElement.InnerText = SourceAssetUnionTypeFilter ?? string.Empty;
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("SourceAssetUnionLabelFilter");
                xmlElement.InnerText = SourceAssetUnionLabelFilter ?? string.Empty;
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("SourceAssetExceptTypeFilter");
                xmlElement.InnerText = SourceAssetExceptTypeFilter ?? string.Empty;
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("SourceAssetExceptLabelFilter");
                xmlElement.InnerText = SourceAssetExceptLabelFilter ?? string.Empty;
                xmlSettings.AppendChild(xmlElement);
                xmlElement = xmlDocument.CreateElement("AssetSorter");
                xmlElement.InnerText = AssetSorter.ToString();
                xmlSettings.AppendChild(xmlElement);

                xmlDocument.Save(configurationName);
            }
            catch
            {
                File.Delete(configurationName);
                return false;
            }

            return m_AssetBundleCollection.Save();
        }

        public AssetBundle[] GetAssetBundles()
        {
            return m_AssetBundleCollection.GetAssetBundles();
        }

        public AssetBundle GetAssetBundle(string assetBundleName, string assetBundleVariant)
        {
            return m_AssetBundleCollection.GetAssetBundle(assetBundleName, assetBundleVariant);
        }

        public bool HasAssetBundle(string assetBundleName, string assetBundleVariant)
        {
            return m_AssetBundleCollection.HasAssetBundle(assetBundleName, assetBundleVariant);
        }

        public bool AddAssetBundle(string assetBundleName, string assetBundleVariant, AssetBundleLoadType assetBundleLoadType, bool assetBundlePacked)
        {
            return m_AssetBundleCollection.AddAssetBundle(assetBundleName, assetBundleVariant, assetBundleLoadType, assetBundlePacked);
        }

        public bool RenameAssetBundle(string oldAssetBundleName, string oldAssetBundleVariant, string newAssetBundleName, string newAssetBundleVariant)
        {
            return m_AssetBundleCollection.RenameAssetBundle(oldAssetBundleName, oldAssetBundleVariant, newAssetBundleName, newAssetBundleVariant);
        }

        public bool RemoveAssetBundle(string assetBundleName, string assetBundleVariant)
        {
            Asset[] assetsToRemove = m_AssetBundleCollection.GetAssets(assetBundleName, assetBundleVariant);
            if (m_AssetBundleCollection.RemoveAssetBundle(assetBundleName, assetBundleVariant))
            {
                List<SourceAsset> unassignedSourceAssets = new List<SourceAsset>();
                foreach (Asset asset in assetsToRemove)
                {
                    SourceAsset sourceAsset = GetSourceAsset(asset.Guid);
                    if (sourceAsset != null)
                    {
                        unassignedSourceAssets.Add(sourceAsset);
                    }
                }

                if (OnAssetUnassigned != null)
                {
                    OnAssetUnassigned(unassignedSourceAssets.ToArray());
                }

                return true;
            }

            return false;
        }

        public bool SetAssetBundleLoadType(string assetBundleName, string assetBundleVariant, AssetBundleLoadType assetBundleLoadType)
        {
            return m_AssetBundleCollection.SetAssetBundleLoadType(assetBundleName, assetBundleVariant, assetBundleLoadType);
        }

        public bool SetAssetBundlePacked(string assetBundleName, string assetBundleVariant, bool assetBundlePacked)
        {
            return m_AssetBundleCollection.SetAssetBundlePacked(assetBundleName, assetBundleVariant, assetBundlePacked);
        }

        public int RemoveUnusedAssetBundles()
        {
            List<AssetBundle> assetBundles = new List<AssetBundle>(m_AssetBundleCollection.GetAssetBundles());
            List<AssetBundle> removeAssetBundles = assetBundles.FindAll(assetBundle => GetAssets(assetBundle.Name, assetBundle.Variant).Length <= 0);
            foreach (AssetBundle assetBundle in removeAssetBundles)
            {
                m_AssetBundleCollection.RemoveAssetBundle(assetBundle.Name, assetBundle.Variant);
            }

            return removeAssetBundles.Count;
        }

        public Asset[] GetAssets(string assetBundleName, string assetBundleVariant)
        {
            List<Asset> assets = new List<Asset>(m_AssetBundleCollection.GetAssets(assetBundleName, assetBundleVariant));
            switch (AssetSorter)
            {
                case AssetSorterType.Path:
                    assets.Sort(AssetPathComparer);
                    break;
                case AssetSorterType.Name:
                    assets.Sort(AssetNameComparer);
                    break;
                case AssetSorterType.Guid:
                    assets.Sort(AssetGuidComparer);
                    break;
            }

            return assets.ToArray();
        }

        public Asset GetAsset(string assetGuid)
        {
            return m_AssetBundleCollection.GetAsset(assetGuid);
        }

        public bool AssignAsset(string assetGuid, string assetBundleName, string assetBundleVariant)
        {
            if (m_AssetBundleCollection.AssignAsset(assetGuid, assetBundleName, assetBundleVariant))
            {
                if (OnAssetAssigned != null)
                {
                    OnAssetAssigned(new SourceAsset[] { GetSourceAsset(assetGuid) });
                }

                return true;
            }

            return false;
        }

        public bool UnassignAsset(string assetGuid)
        {
            if (m_AssetBundleCollection.UnassignAsset(assetGuid))
            {
                SourceAsset sourceAsset = GetSourceAsset(assetGuid);
                if (sourceAsset != null)
                {
                    if (OnAssetUnassigned != null)
                    {
                        OnAssetUnassigned(new SourceAsset[] { sourceAsset });
                    }
                }

                return true;
            }

            return false;
        }

        public int RemoveUnknownAssets()
        {
            List<Asset> assets = new List<Asset>(m_AssetBundleCollection.GetAssets());
            List<Asset> removeAssets = assets.FindAll(asset => GetSourceAsset(asset.Guid) == null);
            foreach (Asset asset in removeAssets)
            {
                m_AssetBundleCollection.UnassignAsset(asset.Guid);
            }

            return removeAssets.Count;
        }

        public SourceAsset GetSourceAsset(string assetGuid)
        {
            if (string.IsNullOrEmpty(assetGuid))
            {
                return null;
            }

            SourceAsset sourceAsset = null;
            if (m_SourceAssets.TryGetValue(assetGuid, out sourceAsset))
            {
                return sourceAsset;
            }

            return null;
        }

        public void ScanSourceAssets()
        {
            m_SourceAssets.Clear();
            m_SourceAssetRoot.Clear();

            string[] sourceAssetSearchPaths = m_SourceAssetSearchPaths.ToArray();
            HashSet<string> tempGuids = new HashSet<string>();
            tempGuids.UnionWith(AssetDatabase.FindAssets(SourceAssetUnionTypeFilter, sourceAssetSearchPaths));
            tempGuids.UnionWith(AssetDatabase.FindAssets(SourceAssetUnionLabelFilter, sourceAssetSearchPaths));
            tempGuids.ExceptWith(AssetDatabase.FindAssets(SourceAssetExceptTypeFilter, sourceAssetSearchPaths));
            tempGuids.ExceptWith(AssetDatabase.FindAssets(SourceAssetExceptLabelFilter, sourceAssetSearchPaths));

            string[] assetGuids = new List<string>(tempGuids).ToArray();
            foreach (string assetGuid in assetGuids)
            {
                string fullPath = AssetDatabase.GUIDToAssetPath(assetGuid);
                if (AssetDatabase.IsValidFolder(fullPath))
                {
                    // Skip folder.
                    continue;
                }

                string assetPath = fullPath.Substring(SourceAssetRootPath.Length + 1);
                string[] splitedPath = assetPath.Split('/');
                SourceFolder folder = m_SourceAssetRoot;
                for (int i = 0; i < splitedPath.Length - 1; i++)
                {
                    SourceFolder subFolder = folder.GetFolder(splitedPath[i]);
                    folder = subFolder == null ? folder.AddFolder(splitedPath[i]) : subFolder;
                }

                SourceAsset asset = folder.AddAsset(assetGuid, fullPath, splitedPath[splitedPath.Length - 1]);
                m_SourceAssets.Add(asset.Guid, asset);
            }
        }

        private void RefreshSourceAssetSearchPaths()
        {
            m_SourceAssetSearchPaths.Clear();

            if (string.IsNullOrEmpty(m_SourceAssetRootPath))
            {
                SourceAssetRootPath = DefaultSourceAssetRootPath;
            }

            if (m_SourceAssetSearchRelativePaths.Count > 0)
            {
                foreach (string sourceAssetSearchRelativePath in m_SourceAssetSearchRelativePaths)
                {
                    m_SourceAssetSearchPaths.Add(Utility.Path.GetCombinePath(m_SourceAssetRootPath, sourceAssetSearchRelativePath));
                }
            }
            else
            {
                m_SourceAssetSearchPaths.Add(m_SourceAssetRootPath);
            }
        }

        private int AssetPathComparer(Asset a, Asset b)
        {
            SourceAsset sourceAssetA = GetSourceAsset(a.Guid);
            SourceAsset sourceAssetB = GetSourceAsset(b.Guid);

            if (sourceAssetA != null && sourceAssetB != null)
            {
                return sourceAssetA.Path.CompareTo(sourceAssetB.Path);
            }

            if (sourceAssetA == null && sourceAssetB == null)
            {
                return a.Guid.CompareTo(b.Guid);
            }

            if (sourceAssetA == null)
            {
                return -1;
            }

            if (sourceAssetB == null)
            {
                return 1;
            }

            return 0;
        }

        private int AssetNameComparer(Asset a, Asset b)
        {
            SourceAsset sourceAssetA = GetSourceAsset(a.Guid);
            SourceAsset sourceAssetB = GetSourceAsset(b.Guid);

            if (sourceAssetA != null && sourceAssetB != null)
            {
                return sourceAssetA.Name.CompareTo(sourceAssetB.Name);
            }

            if (sourceAssetA == null && sourceAssetB == null)
            {
                return a.Guid.CompareTo(b.Guid);
            }

            if (sourceAssetA == null)
            {
                return -1;
            }

            if (sourceAssetB == null)
            {
                return 1;
            }

            return 0;
        }

        private int AssetGuidComparer(Asset a, Asset b)
        {
            SourceAsset sourceAssetA = GetSourceAsset(a.Guid);
            SourceAsset sourceAssetB = GetSourceAsset(b.Guid);

            if (sourceAssetA != null && sourceAssetB != null || sourceAssetA == null && sourceAssetB == null)
            {
                return a.Guid.CompareTo(b.Guid);
            }

            if (sourceAssetA == null)
            {
                return -1;
            }

            if (sourceAssetB == null)
            {
                return 1;
            }

            return 0;
        }
    }
}
