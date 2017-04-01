//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEditor;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    /// <summary>
    /// 资源。
    /// </summary>
    internal sealed class Asset
    {
        private Asset(string guid, AssetBundle assetBundle)
        {
            Guid = guid;
            AssetBundle = assetBundle;
        }

        public string Guid
        {
            get;
            private set;
        }

        public string Name
        {
            get
            {
                return AssetDatabase.GUIDToAssetPath(Guid);
            }
        }

        public AssetBundle AssetBundle
        {
            get;
            private set;
        }

        public static Asset Create(string guid)
        {
            return new Asset(guid, null);
        }

        public static Asset Create(string guid, AssetBundle assetBundle)
        {
            return new Asset(guid, assetBundle);
        }

        public void SetAssetBundle(AssetBundle assetBundle)
        {
            AssetBundle = assetBundle;
        }
    }
}
