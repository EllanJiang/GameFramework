//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    internal partial class AssetBundleEditor
    {
        private sealed class AssetBundleFolder
        {
            private static Texture s_CachedIcon = null;

            private readonly List<AssetBundleFolder> m_Folders;
            private readonly List<AssetBundleItem> m_Items;

            public AssetBundleFolder(string name, AssetBundleFolder folder)
            {
                m_Folders = new List<AssetBundleFolder>();
                m_Items = new List<AssetBundleItem>();

                Name = name;
                Folder = folder;
            }

            public string Name
            {
                get;
                private set;
            }

            public AssetBundleFolder Folder
            {
                get;
                private set;
            }

            public string FromRootPath
            {
                get
                {
                    return Folder == null ? string.Empty : (Folder.Folder == null ? Name : string.Format("{0}/{1}", Folder.FromRootPath, Name));
                }
            }

            public int Depth
            {
                get
                {
                    return Folder != null ? Folder.Depth + 1 : 0;
                }
            }

            public static Texture Icon
            {
                get
                {
                    if (s_CachedIcon == null)
                    {
                        s_CachedIcon = AssetDatabase.GetCachedIcon("Assets");
                    }

                    return s_CachedIcon;
                }
            }

            public void Clear()
            {
                m_Folders.Clear();
                m_Items.Clear();
            }

            public AssetBundleFolder[] GetFolders()
            {
                return m_Folders.ToArray();
            }

            public AssetBundleFolder GetFolder(string name)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new GameFrameworkException("AssetBundle folder name is invalid.");
                }

                foreach (AssetBundleFolder folder in m_Folders)
                {
                    if (folder.Name == name)
                    {
                        return folder;
                    }
                }

                return null;
            }

            public AssetBundleFolder AddFolder(string name)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new GameFrameworkException("AssetBundle folder name is invalid.");
                }

                AssetBundleFolder folder = GetFolder(name);
                if (folder != null)
                {
                    throw new GameFrameworkException("AssetBundle folder is already exist.");
                }

                folder = new AssetBundleFolder(name, this);
                m_Folders.Add(folder);

                return folder;
            }

            public AssetBundleItem[] GetItems()
            {
                return m_Items.ToArray();
            }

            public AssetBundleItem GetItem(string name)
            {
                if (string.IsNullOrEmpty(name))
                {
                    throw new GameFrameworkException("AssetBundle item name is invalid.");
                }

                foreach (AssetBundleItem item in m_Items)
                {
                    if (item.Name == name)
                    {
                        return item;
                    }
                }

                return null;
            }

            public void AddItem(string name, AssetBundle assetBundle)
            {
                AssetBundleItem item = GetItem(name);
                if (item != null)
                {
                    throw new GameFrameworkException("AssetBundle item is already exist.");
                }

                item = new AssetBundleItem(name, assetBundle, this);
                m_Items.Add(item);
                m_Items.Sort(AssetBundleItemComparer);
            }

            private int AssetBundleItemComparer(AssetBundleItem a, AssetBundleItem b)
            {
                return a.Name.CompareTo(b.Name);
            }
        }
    }
}
