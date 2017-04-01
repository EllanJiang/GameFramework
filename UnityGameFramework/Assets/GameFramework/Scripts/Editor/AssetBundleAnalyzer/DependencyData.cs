//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    internal sealed class DependencyData
    {
        private List<AssetBundle> m_DependencyAssetBundles;
        private List<Asset> m_DependencyAssets;
        private List<string> m_ScatteredDependencyAssetNames;

        public DependencyData()
        {
            m_DependencyAssetBundles = new List<AssetBundle>();
            m_DependencyAssets = new List<Asset>();
            m_ScatteredDependencyAssetNames = new List<string>();
        }

        public int DependencyAssetBundleCount
        {
            get
            {
                return m_DependencyAssetBundles.Count;
            }
        }

        public int DependencyAssetCount
        {
            get
            {
                return m_DependencyAssets.Count;
            }
        }

        public int ScatteredDependencyAssetCount
        {
            get
            {
                return m_ScatteredDependencyAssetNames.Count;
            }
        }

        public void AddDependencyAsset(Asset asset)
        {
            if (!m_DependencyAssetBundles.Contains(asset.AssetBundle))
            {
                m_DependencyAssetBundles.Add(asset.AssetBundle);
            }

            m_DependencyAssets.Add(asset);
        }

        public void AddScatteredDependencyAsset(string dependencyAssetName)
        {
            m_ScatteredDependencyAssetNames.Add(dependencyAssetName);
        }

        public AssetBundle[] GetDependencyAssetBundles()
        {
            return m_DependencyAssetBundles.ToArray();
        }

        public Asset[] GetDependencyAssets()
        {
            return m_DependencyAssets.ToArray();
        }

        public string[] GetScatteredDependencyAssetNames()
        {
            return m_ScatteredDependencyAssetNames.ToArray();
        }

        public void RefreshData()
        {
            m_DependencyAssetBundles.Sort(DependencyAssetBundlesComparer);
            m_DependencyAssets.Sort(DependencyAssetsComparer);
            m_ScatteredDependencyAssetNames.Sort();
        }

        private int DependencyAssetBundlesComparer(AssetBundle a, AssetBundle b)
        {
            return a.FullName.CompareTo(b.FullName);
        }

        private int DependencyAssetsComparer(Asset a, Asset b)
        {
            return a.Name.CompareTo(b.Name);
        }
    }
}
