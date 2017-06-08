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
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    /// <summary>
    /// 资源包收集器。
    /// </summary>
    internal sealed class AssetBundleCollection
    {
        private const string ConfigurationName = "GameFramework/Configs/AssetBundleCollection.xml";
        private const string AssetBundleNamePattern = @"^([A-Za-z0-9\._-]+/)*[A-Za-z0-9\._-]+$";
        private const string AssetBundleVariantPattern = @"^[a-z0-9_-]+$";
        private const string PostfixOfScene = ".unity";
        private readonly SortedDictionary<string, AssetBundle> m_AssetBundles;
        private readonly SortedDictionary<string, Asset> m_Assets;

        public AssetBundleCollection()
        {
            m_AssetBundles = new SortedDictionary<string, AssetBundle>();
            m_Assets = new SortedDictionary<string, Asset>();
        }

        public int AssetBundleCount
        {
            get
            {
                return m_AssetBundles.Count;
            }
        }

        public int AssetCount
        {
            get
            {
                return m_Assets.Count;
            }
        }

        public event GameFrameworkAction<int, int> OnLoadingAssetBundle = null;

        public event GameFrameworkAction<int, int> OnLoadingAsset = null;

        public event GameFrameworkAction OnLoadCompleted = null;

        public void Clear()
        {
            m_AssetBundles.Clear();
            m_Assets.Clear();
        }

        public bool Load()
        {
            Clear();

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
                XmlNode xmlCollection = xmlRoot.SelectSingleNode("AssetBundleCollection");
                XmlNode xmlAssetBundles = xmlCollection.SelectSingleNode("AssetBundles");
                XmlNode xmlAssets = xmlCollection.SelectSingleNode("Assets");

                XmlNodeList xmlNodeList = null;
                XmlNode xmlNode = null;
                int count = 0;

                xmlNodeList = xmlAssetBundles.ChildNodes;
                count = xmlNodeList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (OnLoadingAssetBundle != null)
                    {
                        OnLoadingAssetBundle(i, count);
                    }

                    xmlNode = xmlNodeList.Item(i);
                    if (xmlNode.Name != "AssetBundle")
                    {
                        continue;
                    }

                    string assetBundleName = xmlNode.Attributes.GetNamedItem("Name").Value;
                    string assetBundleVariant = xmlNode.Attributes.GetNamedItem("Variant") != null ? xmlNode.Attributes.GetNamedItem("Variant").Value : null;
                    int assetBundleLoadType = 0;
                    if (xmlNode.Attributes.GetNamedItem("LoadType") != null)
                    {
                        int.TryParse(xmlNode.Attributes.GetNamedItem("LoadType").Value, out assetBundleLoadType);
                    }
                    bool assetBundlePacked = false;
                    if (xmlNode.Attributes.GetNamedItem("Packed") != null)
                    {
                        bool.TryParse(xmlNode.Attributes.GetNamedItem("Packed").Value, out assetBundlePacked);
                    }

                    if (!AddAssetBundle(assetBundleName, assetBundleVariant, (AssetBundleLoadType)assetBundleLoadType, assetBundlePacked))
                    {
                        string assetBundleFullName = assetBundleVariant != null ? string.Format("{0}.{1}", assetBundleName, assetBundleVariant) : assetBundleName;
                        Debug.LogWarning(string.Format("Can not add AssetBundle '{0}'.", assetBundleFullName));
                        continue;
                    }
                }

                xmlNodeList = xmlAssets.ChildNodes;
                count = xmlNodeList.Count;
                for (int i = 0; i < count; i++)
                {
                    if (OnLoadingAsset != null)
                    {
                        OnLoadingAsset(i, count);
                    }

                    xmlNode = xmlNodeList.Item(i);
                    if (xmlNode.Name != "Asset")
                    {
                        continue;
                    }

                    string assetGuid = xmlNode.Attributes.GetNamedItem("Guid").Value;
                    string assetBundleName = xmlNode.Attributes.GetNamedItem("AssetBundleName").Value;
                    string assetBundleVariant = xmlNode.Attributes.GetNamedItem("AssetBundleVariant") != null ? xmlNode.Attributes.GetNamedItem("AssetBundleVariant").Value : null;
                    if (!AssignAsset(assetGuid, assetBundleName, assetBundleVariant))
                    {
                        string assetBundleFullName = assetBundleVariant != null ? string.Format("{0}.{1}", assetBundleName, assetBundleVariant) : assetBundleName;
                        Debug.LogWarning(string.Format("Can not assign asset '{0}' to AssetBundle '{1}'.", assetGuid, assetBundleFullName));
                        continue;
                    }
                }

                if (OnLoadCompleted != null)
                {
                    OnLoadCompleted();
                }

                return true;
            }
            catch
            {
                File.Delete(configurationName);
                if (OnLoadCompleted != null)
                {
                    OnLoadCompleted();
                }

                return false;
            }
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

                XmlElement xmlCollection = xmlDocument.CreateElement("AssetBundleCollection");
                xmlRoot.AppendChild(xmlCollection);

                XmlElement xmlAssetBundles = xmlDocument.CreateElement("AssetBundles");
                xmlCollection.AppendChild(xmlAssetBundles);

                XmlElement xmlAssets = xmlDocument.CreateElement("Assets");
                xmlCollection.AppendChild(xmlAssets);

                XmlElement xmlElement = null;
                XmlAttribute xmlAttribute = null;

                foreach (AssetBundle assetBundle in m_AssetBundles.Values)
                {
                    xmlElement = xmlDocument.CreateElement("AssetBundle");
                    xmlAttribute = xmlDocument.CreateAttribute("Name");
                    xmlAttribute.Value = assetBundle.Name;
                    xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    if (assetBundle.Variant != null)
                    {
                        xmlAttribute = xmlDocument.CreateAttribute("Variant");
                        xmlAttribute.Value = assetBundle.Variant;
                        xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    }
                    xmlAttribute = xmlDocument.CreateAttribute("LoadType");
                    xmlAttribute.Value = ((int)assetBundle.LoadType).ToString();
                    xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    xmlAttribute = xmlDocument.CreateAttribute("Packed");
                    xmlAttribute.Value = assetBundle.Packed.ToString();
                    xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    xmlAssetBundles.AppendChild(xmlElement);
                }

                foreach (Asset asset in m_Assets.Values)
                {
                    xmlElement = xmlDocument.CreateElement("Asset");
                    xmlAttribute = xmlDocument.CreateAttribute("Guid");
                    xmlAttribute.Value = asset.Guid;
                    xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    xmlAttribute = xmlDocument.CreateAttribute("AssetBundleName");
                    xmlAttribute.Value = asset.AssetBundle.Name;
                    xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    if (asset.AssetBundle.Variant != null)
                    {
                        xmlAttribute = xmlDocument.CreateAttribute("AssetBundleVariant");
                        xmlAttribute.Value = asset.AssetBundle.Variant;
                        xmlElement.Attributes.SetNamedItem(xmlAttribute);
                    }
                    xmlAssets.AppendChild(xmlElement);
                }

                xmlDocument.Save(configurationName);
                return true;
            }
            catch
            {
                File.Delete(configurationName);
                return false;
            }
        }

        public AssetBundle[] GetAssetBundles()
        {
            return m_AssetBundles.Values.ToArray();
        }

        public AssetBundle GetAssetBundle(string assetBundleName, string assetBundleVariant)
        {
            if (!IsValidAssetBundleName(assetBundleName, assetBundleVariant))
            {
                return null;
            }

            AssetBundle assetBundle = null;
            if (m_AssetBundles.TryGetValue(GetAssetBundleFullName(assetBundleName, assetBundleVariant), out assetBundle))
            {
                return assetBundle;
            }

            return null;
        }

        public bool HasAssetBundle(string assetBundleName, string assetBundleVariant)
        {
            if (!IsValidAssetBundleName(assetBundleName, assetBundleVariant))
            {
                return false;
            }

            return m_AssetBundles.ContainsKey(GetAssetBundleFullName(assetBundleName, assetBundleVariant));
        }

        public bool AddAssetBundle(string assetBundleName, string assetBundleVariant, AssetBundleLoadType assetBundleLoadType, bool assetBundlePacked)
        {
            if (!IsValidAssetBundleName(assetBundleName, assetBundleVariant))
            {
                return false;
            }

            if (!IsAvailableBundleName(assetBundleName, assetBundleVariant, null))
            {
                return false;
            }

            AssetBundle assetBundle = AssetBundle.Create(assetBundleName, assetBundleVariant, assetBundleLoadType, assetBundlePacked);
            m_AssetBundles.Add(assetBundle.FullName, assetBundle);

            return true;
        }

        public bool RenameAssetBundle(string oldAssetBundleName, string oldAssetBundleVariant, string newAssetBundleName, string newAssetBundleVariant)
        {
            if (!IsValidAssetBundleName(oldAssetBundleName, oldAssetBundleVariant) || !IsValidAssetBundleName(newAssetBundleName, newAssetBundleVariant))
            {
                return false;
            }

            AssetBundle assetBundle = GetAssetBundle(oldAssetBundleName, oldAssetBundleVariant);
            if (assetBundle == null)
            {
                return false;
            }

            if (!IsAvailableBundleName(newAssetBundleName, newAssetBundleVariant, assetBundle))
            {
                return false;
            }

            m_AssetBundles.Remove(assetBundle.FullName);
            assetBundle.Rename(newAssetBundleName, newAssetBundleVariant);
            m_AssetBundles.Add(assetBundle.FullName, assetBundle);

            return true;
        }

        public bool RemoveAssetBundle(string assetBundleName, string assetBundleVariant)
        {
            if (!IsValidAssetBundleName(assetBundleName, assetBundleVariant))
            {
                return false;
            }

            AssetBundle assetBundle = GetAssetBundle(assetBundleName, assetBundleVariant);
            if (assetBundle == null)
            {
                return false;
            }

            Asset[] assets = assetBundle.GetAssets();
            assetBundle.Clear();
            m_AssetBundles.Remove(assetBundle.FullName);
            foreach (Asset asset in assets)
            {
                m_Assets.Remove(asset.Guid);
            }

            return true;
        }

        public bool SetAssetBundleLoadType(string assetBundleName, string assetBundleVariant, AssetBundleLoadType assetBundleLoadType)
        {
            if (!IsValidAssetBundleName(assetBundleName, assetBundleVariant))
            {
                return false;
            }

            AssetBundle assetBundle = GetAssetBundle(assetBundleName, assetBundleVariant);
            if (assetBundle == null)
            {
                return false;
            }

            assetBundle.SetLoadType(assetBundleLoadType);

            return true;
        }

        public bool SetAssetBundlePacked(string assetBundleName, string assetBundleVariant, bool assetBundlePacked)
        {
            if (!IsValidAssetBundleName(assetBundleName, assetBundleVariant))
            {
                return false;
            }

            AssetBundle assetBundle = GetAssetBundle(assetBundleName, assetBundleVariant);
            if (assetBundle == null)
            {
                return false;
            }

            assetBundle.SetPacked(assetBundlePacked);

            return true;
        }

        public Asset[] GetAssets()
        {
            return m_Assets.Values.ToArray();
        }

        public Asset[] GetAssets(string assetBundleName, string assetBundleVariant)
        {
            if (!IsValidAssetBundleName(assetBundleName, assetBundleVariant))
            {
                return new Asset[0];
            }

            AssetBundle assetBundle = GetAssetBundle(assetBundleName, assetBundleVariant);
            if (assetBundle == null)
            {
                return new Asset[0];
            }

            return assetBundle.GetAssets();
        }

        public Asset GetAsset(string assetGuid)
        {
            if (string.IsNullOrEmpty(assetGuid))
            {
                return null;
            }

            Asset asset = null;
            if (m_Assets.TryGetValue(assetGuid, out asset))
            {
                return asset;
            }

            return null;
        }

        public bool HasAsset(string assetGuid)
        {
            if (string.IsNullOrEmpty(assetGuid))
            {
                return false;
            }

            return m_Assets.ContainsKey(assetGuid);
        }

        public bool AssignAsset(string assetGuid, string assetBundleName, string assetBundleVariant)
        {
            if (string.IsNullOrEmpty(assetGuid))
            {
                return false;
            }

            if (!IsValidAssetBundleName(assetBundleName, assetBundleVariant))
            {
                return false;
            }

            AssetBundle assetBundle = GetAssetBundle(assetBundleName, assetBundleVariant);
            if (assetBundle == null)
            {
                return false;
            }

            string assetName = AssetDatabase.GUIDToAssetPath(assetGuid);
            if (string.IsNullOrEmpty(assetName))
            {
                return false;
            }

            bool isScene = assetName.EndsWith(PostfixOfScene);
            if (isScene && assetBundle.Type == AssetBundleType.Asset || !isScene && assetBundle.Type == AssetBundleType.Scene)
            {
                return false;
            }

            Asset asset = GetAsset(assetGuid);
            if (asset == null)
            {
                asset = Asset.Create(assetGuid);
                m_Assets.Add(asset.Guid, asset);
            }

            assetBundle.AssignAsset(asset, isScene);

            return true;
        }

        public bool UnassignAsset(string assetGuid)
        {
            if (string.IsNullOrEmpty(assetGuid))
            {
                return false;
            }

            Asset asset = GetAsset(assetGuid);
            if (asset != null)
            {
                asset.AssetBundle.Unassign(asset);
                m_Assets.Remove(asset.Guid);
            }

            return true;
        }

        private bool IsValidAssetBundleName(string assetBundleName, string assetBundleVariant)
        {
            if (string.IsNullOrEmpty(assetBundleName))
            {
                return false;
            }

            if (!Regex.IsMatch(assetBundleName, AssetBundleNamePattern))
            {
                return false;
            }

            if (assetBundleVariant != null && !Regex.IsMatch(assetBundleVariant, AssetBundleVariantPattern))
            {
                return false;
            }

            return true;
        }

        private bool IsAvailableBundleName(string assetBundleName, string assetBundleVariant, AssetBundle selfAssetBundle)
        {
            AssetBundle findAssetBundle = GetAssetBundle(assetBundleName, assetBundleVariant);
            if (findAssetBundle != null)
            {
                return findAssetBundle == selfAssetBundle;
            }

            foreach (AssetBundle assetBundle in m_AssetBundles.Values)
            {
                if (selfAssetBundle != null && assetBundle == selfAssetBundle)
                {
                    continue;
                }

                if (assetBundle.Name == assetBundleName)
                {
                    if (assetBundle.Variant == null && assetBundleVariant != null)
                    {
                        return false;
                    }

                    if (assetBundle.Variant != null && assetBundleVariant == null)
                    {
                        return false;
                    }
                }

                if (assetBundle.Name.Length > assetBundleName.Length
                    && assetBundle.Name.IndexOf(assetBundleName, StringComparison.CurrentCultureIgnoreCase) == 0
                    && assetBundle.Name[assetBundleName.Length] == '/')
                {
                    return false;
                }

                if (assetBundleName.Length > assetBundle.Name.Length
                    && assetBundleName.IndexOf(assetBundle.Name, StringComparison.CurrentCultureIgnoreCase) == 0
                    && assetBundleName[assetBundle.Name.Length] == '/')
                {
                    return false;
                }
            }

            return true;
        }

        private string GetAssetBundleFullName(string assetBundleName, string assetBundleVariant)
        {
            return (!string.IsNullOrEmpty(assetBundleVariant) ? string.Format("{0}.{1}", assetBundleName, assetBundleVariant) : assetBundleName);
        }
    }
}
