//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    /// <summary>
    /// 资源包。
    /// </summary>
    internal sealed class AssetBundle
    {
        private readonly List<Asset> m_Assets;

        private AssetBundle(string name, string variant, AssetBundleLoadType loadType, bool packed)
        {
            m_Assets = new List<Asset>();

            Name = name;
            Variant = variant;
            Type = AssetBundleType.Unknown;
            LoadType = loadType;
            Packed = packed;
        }

        public string Name
        {
            get;
            private set;
        }

        public string Variant
        {
            get;
            private set;
        }

        public string FullName
        {
            get
            {
                return Variant != null ? string.Format("{0}.{1}", Name, Variant) : Name;
            }
        }

        public AssetBundleType Type
        {
            get;
            private set;
        }

        public AssetBundleLoadType LoadType
        {
            get;
            private set;
        }

        public bool Packed
        {
            get;
            private set;
        }

        public static AssetBundle Create(string name, string variant, AssetBundleLoadType loadType, bool packed)
        {
            return new AssetBundle(name, variant, loadType, packed);
        }

        public Asset[] GetAssets()
        {
            return m_Assets.ToArray();
        }

        public void Rename(string name, string variant)
        {
            Name = name;
            Variant = variant;
        }

        public void SetLoadType(AssetBundleLoadType loadType)
        {
            LoadType = loadType;
        }

        public void SetPacked(bool packed)
        {
            Packed = packed;
        }

        public void AssignAsset(Asset asset, bool isScene)
        {
            if (asset.AssetBundle != null)
            {
                asset.AssetBundle.Unassign(asset);
            }

            Type = isScene ? AssetBundleType.Scene : AssetBundleType.Asset;
            asset.SetAssetBundle(this);
            m_Assets.Add(asset);
            m_Assets.Sort(AssetComparer);
        }

        public void Unassign(Asset asset)
        {
            asset.SetAssetBundle(null);
            m_Assets.Remove(asset);
            if (m_Assets.Count <= 0)
            {
                Type = AssetBundleType.Unknown;
            }
        }

        public void Clear()
        {
            foreach (Asset asset in m_Assets)
            {
                asset.SetAssetBundle(null);
            }

            m_Assets.Clear();
        }

        private int AssetComparer(Asset a, Asset b)
        {
            return a.Guid.CompareTo(b.Guid);
        }
    }
}
