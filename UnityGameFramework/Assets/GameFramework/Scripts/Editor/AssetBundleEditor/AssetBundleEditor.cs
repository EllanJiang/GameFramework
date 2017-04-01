//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    /// <summary>
    /// 资源包编辑器。
    /// </summary>
    internal sealed partial class AssetBundleEditor : EditorWindow
    {
        private AssetBundleEditorController m_Controller = null;
        private MenuState m_MenuState = MenuState.Normal;
        private AssetBundle m_SelectedAssetBundle = null;
        private AssetBundleFolder m_AssetBundleRoot = null;
        private HashSet<string> m_ExpandedAssetBundleFolderNames = null;
        private HashSet<Asset> m_SelectedAssetsInSelectedAssetBundle = null;
        private HashSet<SourceFolder> m_ExpandedSourceFolders = null;
        private HashSet<SourceAsset> m_SelectedSourceAssets = null;
        private Texture m_MissingSourceAssetIcon = null;

        private HashSet<SourceFolder> m_CachedSelectedSourceFolders = null;
        private HashSet<SourceFolder> m_CachedUnselectedSourceFolders = null;
        private HashSet<SourceFolder> m_CachedAssignedSourceFolders = null;
        private HashSet<SourceFolder> m_CachedUnassignedSourceFolders = null;
        private HashSet<SourceAsset> m_CachedAssignedSourceAssets = null;
        private HashSet<SourceAsset> m_CachedUnassignedSourceAssets = null;

        private Vector2 m_AssetBundlesViewScroll = Vector2.zero;
        private Vector2 m_AssetBundleViewScroll = Vector2.zero;
        private Vector2 m_SourceAssetsViewScroll = Vector2.zero;
        private string m_InputAssetBundleName = null;
        private string m_InputAssetBundleVariant = null;
        private bool m_HideAssignedSourceAssets = false;
        private int m_CurrentAssetBundleContentCount = 0;
        private int m_CurrentAssetBundleRowOnDraw = 0;
        private int m_CurrentSourceRowOnDraw = 0;

        [MenuItem("Game Framework/AssetBundle Tools/AssetBundle Editor", false, 32)]
        private static void Open()
        {
            AssetBundleEditor window = GetWindow<AssetBundleEditor>(true, "AssetBundle Editor", true);
            window.minSize = new Vector2(1400f, 600f);
        }

        private void OnEnable()
        {
            m_Controller = new AssetBundleEditorController();
            m_Controller.OnLoadingAssetBundle += OnLoadingAssetBundle;
            m_Controller.OnLoadingAsset += OnLoadingAsset;
            m_Controller.OnLoadCompleted += OnLoadCompleted;
            m_Controller.OnAssetAssigned += OnAssetAssigned;
            m_Controller.OnAssetUnassigned += OnAssetUnassigned;

            m_MenuState = MenuState.Normal;
            m_SelectedAssetBundle = null;
            m_AssetBundleRoot = new AssetBundleFolder("AssetBundles", null);
            m_ExpandedAssetBundleFolderNames = new HashSet<string>();
            m_SelectedAssetsInSelectedAssetBundle = new HashSet<Asset>();
            m_ExpandedSourceFolders = new HashSet<SourceFolder>();
            m_SelectedSourceAssets = new HashSet<SourceAsset>();
            m_MissingSourceAssetIcon = EditorGUIUtility.IconContent("console.warnicon.sml").image;

            m_CachedSelectedSourceFolders = new HashSet<SourceFolder>();
            m_CachedUnselectedSourceFolders = new HashSet<SourceFolder>();
            m_CachedAssignedSourceFolders = new HashSet<SourceFolder>();
            m_CachedUnassignedSourceFolders = new HashSet<SourceFolder>();
            m_CachedAssignedSourceAssets = new HashSet<SourceAsset>();
            m_CachedUnassignedSourceAssets = new HashSet<SourceAsset>();

            m_AssetBundlesViewScroll = Vector2.zero;
            m_AssetBundleViewScroll = Vector2.zero;
            m_SourceAssetsViewScroll = Vector2.zero;
            m_InputAssetBundleName = null;
            m_InputAssetBundleVariant = null;
            m_HideAssignedSourceAssets = false;
            m_CurrentAssetBundleContentCount = 0;
            m_CurrentAssetBundleRowOnDraw = 0;
            m_CurrentSourceRowOnDraw = 0;

            if (m_Controller.Load())
            {
                Debug.Log("Load configuration success.");
            }
            else
            {
                Debug.LogWarning("Load configuration failure.");
            }

            EditorUtility.DisplayProgressBar("Prepare AssetBundle Editor", "Processing...", 0f);
            RefreshAssetBundleTree();
            EditorUtility.ClearProgressBar();
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.Width(position.width), GUILayout.Height(position.height));
            {
                GUILayout.Space(2f);
                EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.25f));
                {
                    GUILayout.Space(5f);
                    EditorGUILayout.LabelField(string.Format("AssetBundle List ({0})", m_Controller.AssetBundleCount.ToString()), EditorStyles.boldLabel);
                    EditorGUILayout.BeginHorizontal("box", GUILayout.Height(position.height - 52f));
                    {
                        DrawAssetBundlesView();
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(5f);
                        DrawAssetBundlesMenu();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.25f));
                {
                    GUILayout.Space(5f);
                    EditorGUILayout.LabelField(string.Format("AssetBundle Content ({0})", m_CurrentAssetBundleContentCount.ToString()), EditorStyles.boldLabel);
                    EditorGUILayout.BeginHorizontal("box", GUILayout.Height(position.height - 52f));
                    {
                        DrawAssetBundleView();
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(5f);
                        DrawAssetBundleMenu();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.5f - 16f));
                {
                    GUILayout.Space(5f);
                    EditorGUILayout.LabelField("Asset List", EditorStyles.boldLabel);
                    EditorGUILayout.BeginHorizontal("box", GUILayout.Height(position.height - 52f));
                    {
                        DrawSourceAssetsView();
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Space(5f);
                        DrawSourceAssetsMenu();
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                GUILayout.Space(5f);
            }
            EditorGUILayout.EndHorizontal();
        }

        private void DrawAssetBundlesView()
        {
            m_CurrentAssetBundleRowOnDraw = 0;
            m_AssetBundlesViewScroll = EditorGUILayout.BeginScrollView(m_AssetBundlesViewScroll);
            {
                DrawAssetBundleFolder(m_AssetBundleRoot);
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawAssetBundleFolder(AssetBundleFolder assetBundleFolder)
        {
            bool expand = IsExpandedAssetBundleFolder(assetBundleFolder);
            EditorGUILayout.BeginHorizontal();
            {
                if (expand != EditorGUI.Foldout(new Rect(18f + 14f * assetBundleFolder.Depth, 20f * m_CurrentAssetBundleRowOnDraw + 2f, int.MaxValue, 14f), expand, string.Empty, true))
                {
                    expand = !expand;
                    SetExpandedAssetBundleFolder(assetBundleFolder, expand);
                }

                GUI.DrawTexture(new Rect(32f + 14f * assetBundleFolder.Depth, 20f * m_CurrentAssetBundleRowOnDraw + 1f, 16f, 16f), AssetBundleFolder.Icon);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(40f + 14f * assetBundleFolder.Depth), GUILayout.Height(18f));
                EditorGUILayout.LabelField(assetBundleFolder.Name);
            }
            EditorGUILayout.EndHorizontal();

            m_CurrentAssetBundleRowOnDraw++;

            if (expand)
            {
                foreach (AssetBundleFolder subAssetBundleFolder in assetBundleFolder.GetFolders())
                {
                    DrawAssetBundleFolder(subAssetBundleFolder);
                }

                foreach (AssetBundleItem assetBundleItem in assetBundleFolder.GetItems())
                {
                    DrawAssetBundleItem(assetBundleItem);
                }
            }
        }

        private void DrawAssetBundleItem(AssetBundleItem assetBundleItem)
        {
            EditorGUILayout.BeginHorizontal();
            {
                string title = assetBundleItem.Name;
                if (assetBundleItem.AssetBundle.Packed)
                {
                    title = "[Packed] " + title;
                }

                float emptySpace = position.width;
                if (EditorGUILayout.Toggle(m_SelectedAssetBundle == assetBundleItem.AssetBundle, GUILayout.Width(emptySpace - 12f)))
                {
                    ChangeSelectedAssetBundle(assetBundleItem.AssetBundle);
                }
                else if (m_SelectedAssetBundle == assetBundleItem.AssetBundle)
                {
                    ChangeSelectedAssetBundle(null);
                }

                GUILayout.Space(-emptySpace + 24f);
                GUI.DrawTexture(new Rect(32f + 14f * assetBundleItem.Depth, 20f * m_CurrentAssetBundleRowOnDraw + 1f, 16f, 16f), assetBundleItem.Icon);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(26f + 14f * assetBundleItem.Depth), GUILayout.Height(18f));
                EditorGUILayout.LabelField(title);
            }
            EditorGUILayout.EndHorizontal();
            m_CurrentAssetBundleRowOnDraw++;
        }

        private void DrawAssetBundlesMenu()
        {
            switch (m_MenuState)
            {
                case MenuState.Normal:
                    DrawAssetBundlesMenu_Normal();
                    break;
                case MenuState.Add:
                    DrawAssetBundlesMenu_Add();
                    break;
                case MenuState.Rename:
                    DrawAssetBundlesMenu_Rename();
                    break;
                case MenuState.Remove:
                    DrawAssetBundlesMenu_Remove();
                    break;
            }
        }

        private void DrawAssetBundlesMenu_Normal()
        {
            if (GUILayout.Button("Add", GUILayout.Width(65f)))
            {
                m_MenuState = MenuState.Add;
                m_InputAssetBundleName = null;
                m_InputAssetBundleVariant = null;
                GUI.FocusControl(null);
            }
            EditorGUI.BeginDisabledGroup(m_SelectedAssetBundle == null);
            {
                if (GUILayout.Button("Rename", GUILayout.Width(65f)))
                {
                    m_MenuState = MenuState.Rename;
                    m_InputAssetBundleName = m_SelectedAssetBundle != null ? m_SelectedAssetBundle.Name : null;
                    m_InputAssetBundleVariant = m_SelectedAssetBundle != null ? m_SelectedAssetBundle.Variant : null;
                    GUI.FocusControl(null);
                }
                if (GUILayout.Button("Remove", GUILayout.Width(65f)))
                {
                    m_MenuState = MenuState.Remove;
                }
                if (m_SelectedAssetBundle == null)
                {
                    EditorGUILayout.EnumPopup(AssetBundleLoadType.LoadFromFile);
                }
                else
                {
                    AssetBundleLoadType loadType = (AssetBundleLoadType)EditorGUILayout.EnumPopup(m_SelectedAssetBundle.LoadType);
                    if (loadType != m_SelectedAssetBundle.LoadType)
                    {
                        SetAssetBundleLoadType(loadType);
                    }
                }
                bool packed = EditorGUILayout.ToggleLeft("Packed", m_SelectedAssetBundle != null && m_SelectedAssetBundle.Packed, GUILayout.Width(65f));
                if (m_SelectedAssetBundle != null && packed != m_SelectedAssetBundle.Packed)
                {
                    SetAssetBundlePacked(packed);
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        private void DrawAssetBundlesMenu_Add()
        {
            GUI.SetNextControlName("NewAssetBundleNameTextField");
            m_InputAssetBundleName = EditorGUILayout.TextField(m_InputAssetBundleName);
            GUI.SetNextControlName("NewAssetBundleVariantTextField");
            m_InputAssetBundleVariant = EditorGUILayout.TextField(m_InputAssetBundleVariant, GUILayout.Width(60f));

            if (GUI.GetNameOfFocusedControl() == "NewAssetBundleNameTextField" || GUI.GetNameOfFocusedControl() == "NewAssetBundleVariantTextField")
            {
                if (Event.current.isKey && Event.current.keyCode == KeyCode.Return)
                {
                    EditorUtility.DisplayProgressBar("Add AssetBundle", "Processing...", 0f);
                    AddAssetBundle(m_InputAssetBundleName, m_InputAssetBundleVariant, true);
                    EditorUtility.ClearProgressBar();
                    Repaint();
                }
            }

            if (GUILayout.Button("Add", GUILayout.Width(50f)))
            {
                EditorUtility.DisplayProgressBar("Add AssetBundle", "Processing...", 0f);
                AddAssetBundle(m_InputAssetBundleName, m_InputAssetBundleVariant, true);
                EditorUtility.ClearProgressBar();
            }

            if (GUILayout.Button("Back", GUILayout.Width(50f)))
            {
                m_MenuState = MenuState.Normal;
            }
        }

        private void DrawAssetBundlesMenu_Rename()
        {
            if (m_SelectedAssetBundle == null)
            {
                m_MenuState = MenuState.Normal;
            }

            GUI.SetNextControlName("RenameAssetBundleNameTextField");
            m_InputAssetBundleName = EditorGUILayout.TextField(m_InputAssetBundleName);
            GUI.SetNextControlName("RenameAssetBundleVariantTextField");
            m_InputAssetBundleVariant = EditorGUILayout.TextField(m_InputAssetBundleVariant, GUILayout.Width(60f));

            if (GUI.GetNameOfFocusedControl() == "RenameAssetBundleNameTextField" || GUI.GetNameOfFocusedControl() == "RenameAssetBundleVariantTextField")
            {
                if (Event.current.isKey && Event.current.keyCode == KeyCode.Return)
                {
                    EditorUtility.DisplayProgressBar("Rename AssetBundle", "Processing...", 0f);
                    RenameAssetBundle(m_SelectedAssetBundle, m_InputAssetBundleName, m_InputAssetBundleVariant);
                    EditorUtility.ClearProgressBar();
                    Repaint();
                }
            }

            if (GUILayout.Button("OK", GUILayout.Width(50f)))
            {
                EditorUtility.DisplayProgressBar("Rename AssetBundle", "Processing...", 0f);
                RenameAssetBundle(m_SelectedAssetBundle, m_InputAssetBundleName, m_InputAssetBundleVariant);
                EditorUtility.ClearProgressBar();
            }

            if (GUILayout.Button("Back", GUILayout.Width(50f)))
            {
                m_MenuState = MenuState.Normal;
            }
        }

        private void DrawAssetBundlesMenu_Remove()
        {
            if (m_SelectedAssetBundle == null)
            {
                m_MenuState = MenuState.Normal;
            }

            GUILayout.Label(string.Format("Remove '{0}' ?", m_SelectedAssetBundle.FullName));

            if (GUILayout.Button("Yes", GUILayout.Width(50f)))
            {
                EditorUtility.DisplayProgressBar("Remove AssetBundle", "Processing...", 0f);
                RemoveAssetBundle();
                EditorUtility.ClearProgressBar();
                m_MenuState = MenuState.Normal;
            }

            if (GUILayout.Button("No", GUILayout.Width(50f)))
            {
                m_MenuState = MenuState.Normal;
            }
        }

        private void DrawAssetBundleView()
        {
            m_AssetBundleViewScroll = EditorGUILayout.BeginScrollView(m_AssetBundleViewScroll);
            {
                if (m_SelectedAssetBundle != null)
                {
                    int index = 0;
                    Asset[] assets = m_Controller.GetAssets(m_SelectedAssetBundle.Name, m_SelectedAssetBundle.Variant);
                    m_CurrentAssetBundleContentCount = assets.Length;
                    foreach (Asset asset in assets)
                    {
                        SourceAsset sourceAsset = m_Controller.GetSourceAsset(asset.Guid);
                        string assetName = sourceAsset != null ? (m_Controller.AssetSorter == AssetSorterType.Path ? sourceAsset.Path : (m_Controller.AssetSorter == AssetSorterType.Name ? sourceAsset.Name : sourceAsset.Guid)) : asset.Guid;
                        EditorGUILayout.BeginHorizontal();
                        {
                            float emptySpace = position.width;
                            bool select = IsSelectedAssetInSelectedAssetBundle(asset);
                            if (select != EditorGUILayout.Toggle(select, GUILayout.Width(emptySpace - 12f)))
                            {
                                select = !select;
                                SetSelectedAssetInSelectedAssetBundle(asset, select);
                            }

                            GUILayout.Space(-emptySpace + 24f);
                            GUI.DrawTexture(new Rect(20f, 20f * (index++) + 1f, 16f, 16f), (sourceAsset != null ? sourceAsset.Icon : m_MissingSourceAssetIcon));
                            EditorGUILayout.LabelField(string.Empty, GUILayout.Width(14f), GUILayout.Height(18f));
                            EditorGUILayout.LabelField(assetName);
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                else
                {
                    m_CurrentAssetBundleContentCount = 0;
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawAssetBundleMenu()
        {
            if (GUILayout.Button("All", GUILayout.Width(50f)) && m_SelectedAssetBundle != null)
            {
                Asset[] assets = m_Controller.GetAssets(m_SelectedAssetBundle.Name, m_SelectedAssetBundle.Variant);
                foreach (Asset asset in assets)
                {
                    SetSelectedAssetInSelectedAssetBundle(asset, true);
                }
            }
            if (GUILayout.Button("None", GUILayout.Width(50f)))
            {
                m_SelectedAssetsInSelectedAssetBundle.Clear();
            }
            m_Controller.AssetSorter = (AssetSorterType)EditorGUILayout.EnumPopup(m_Controller.AssetSorter, GUILayout.Width(60f));
            GUILayout.Label(string.Empty);
            EditorGUI.BeginDisabledGroup(m_SelectedAssetBundle == null || m_SelectedAssetsInSelectedAssetBundle.Count <= 0);
            {
                if (GUILayout.Button(string.Format("{0} >>", m_SelectedAssetsInSelectedAssetBundle.Count.ToString()), GUILayout.Width(80f)))
                {
                    foreach (Asset asset in m_SelectedAssetsInSelectedAssetBundle)
                    {
                        UnassignAsset(asset);
                    }

                    m_SelectedAssetsInSelectedAssetBundle.Clear();
                }
            }
            EditorGUI.EndDisabledGroup();
        }

        private void DrawSourceAssetsView()
        {
            m_CurrentSourceRowOnDraw = 0;
            m_SourceAssetsViewScroll = EditorGUILayout.BeginScrollView(m_SourceAssetsViewScroll);
            {
                DrawSourceFolder(m_Controller.SourceAssetRoot);
            }
            EditorGUILayout.EndScrollView();
        }

        private void DrawSourceAssetsMenu()
        {
            HashSet<SourceAsset> selectedSourceAssets = GetSelectedSourceAssets();
            EditorGUI.BeginDisabledGroup(m_SelectedAssetBundle == null || selectedSourceAssets.Count <= 0);
            {
                if (GUILayout.Button(string.Format("<< {0}", selectedSourceAssets.Count.ToString()), GUILayout.Width(80f)))
                {
                    foreach (SourceAsset sourceAsset in selectedSourceAssets)
                    {
                        AssignAsset(sourceAsset, m_SelectedAssetBundle);
                    }

                    m_SelectedSourceAssets.Clear();
                    m_CachedSelectedSourceFolders.Clear();
                }
            }
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(selectedSourceAssets.Count <= 0);
            {
                if (GUILayout.Button(string.Format("<<< {0}", selectedSourceAssets.Count.ToString()), GUILayout.Width(80f)))
                {
                    int index = 0;
                    int count = selectedSourceAssets.Count;
                    foreach (SourceAsset sourceAsset in selectedSourceAssets)
                    {
                        EditorUtility.DisplayProgressBar("Add AssetBundles", string.Format("{0}/{1} processing...", (++index).ToString(), count.ToString()), (float)index / count);
                        int dotIndex = sourceAsset.FromRootPath.IndexOf('.');
                        string assetBundleName = dotIndex > 0 ? sourceAsset.FromRootPath.Substring(0, dotIndex) : sourceAsset.FromRootPath;
                        AddAssetBundle(assetBundleName, null, false);
                        AssetBundle assetBundle = m_Controller.GetAssetBundle(assetBundleName, null);
                        if (assetBundle == null)
                        {
                            continue;
                        }

                        AssignAsset(sourceAsset, assetBundle);
                    }

                    EditorUtility.DisplayProgressBar("Add AssetBundles", "Complete processing...", 1f);
                    RefreshAssetBundleTree();
                    EditorUtility.ClearProgressBar();
                    m_SelectedSourceAssets.Clear();
                    m_CachedSelectedSourceFolders.Clear();
                }
            }
            EditorGUI.EndDisabledGroup();
            bool hideAssignedSourceAssets = EditorGUILayout.ToggleLeft("Hide Assigned", m_HideAssignedSourceAssets, GUILayout.Width(100f));
            if (hideAssignedSourceAssets != m_HideAssignedSourceAssets)
            {
                m_HideAssignedSourceAssets = hideAssignedSourceAssets;
                m_CachedSelectedSourceFolders.Clear();
                m_CachedUnselectedSourceFolders.Clear();
                m_CachedAssignedSourceFolders.Clear();
                m_CachedUnassignedSourceFolders.Clear();
            }

            GUILayout.Label(string.Empty);
            if (GUILayout.Button("Clean", GUILayout.Width(80f)))
            {
                EditorUtility.DisplayProgressBar("Clean", "Processing...", 0f);
                CleanAssetBundle();
                EditorUtility.ClearProgressBar();
            }
            if (GUILayout.Button("Save", GUILayout.Width(80f)))
            {
                EditorUtility.DisplayProgressBar("Save", "Processing...", 0f);
                SaveConfiguration();
                EditorUtility.ClearProgressBar();
            }
        }

        private void DrawSourceFolder(SourceFolder sourceFolder)
        {
            if (m_HideAssignedSourceAssets && IsAssignedSourceFolder(sourceFolder))
            {
                return;
            }

            bool expand = IsExpandedSourceFolder(sourceFolder);
            EditorGUILayout.BeginHorizontal();
            {
                bool select = IsSelectedSourceFolder(sourceFolder);
                if (select != EditorGUILayout.Toggle(select, GUILayout.Width(12f + 14f * sourceFolder.Depth)))
                {
                    select = !select;
                    SetSelectedSourceFolder(sourceFolder, select);
                }

                GUILayout.Space(-14f * sourceFolder.Depth);
                if (expand != EditorGUI.Foldout(new Rect(18f + 14f * sourceFolder.Depth, 20f * m_CurrentSourceRowOnDraw + 2f, int.MaxValue, 14f), expand, string.Empty, true))
                {
                    expand = !expand;
                    SetExpandedSourceFolder(sourceFolder, expand);
                }

                GUI.DrawTexture(new Rect(32f + 14f * sourceFolder.Depth, 20f * m_CurrentSourceRowOnDraw + 1f, 16f, 16f), SourceFolder.Icon);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(26f + 14f * sourceFolder.Depth), GUILayout.Height(18f));
                EditorGUILayout.LabelField(sourceFolder.Name);
            }
            EditorGUILayout.EndHorizontal();

            m_CurrentSourceRowOnDraw++;

            if (expand)
            {
                foreach (SourceFolder subSourceFolder in sourceFolder.GetFolders())
                {
                    DrawSourceFolder(subSourceFolder);
                }

                foreach (SourceAsset sourceAsset in sourceFolder.GetAssets())
                {
                    DrawSourceAsset(sourceAsset);
                }
            }
        }

        private void DrawSourceAsset(SourceAsset sourceAsset)
        {
            if (m_HideAssignedSourceAssets && IsAssignedSourceAsset(sourceAsset))
            {
                return;
            }

            EditorGUILayout.BeginHorizontal();
            {
                float emptySpace = position.width;
                bool select = IsSelectedSourceAsset(sourceAsset);
                if (select != EditorGUILayout.Toggle(select, GUILayout.Width(emptySpace - 12f)))
                {
                    select = !select;
                    SetSelectedSourceAsset(sourceAsset, select);
                }

                GUILayout.Space(-emptySpace + 24f);
                GUI.DrawTexture(new Rect(32f + 14f * sourceAsset.Depth, 20f * m_CurrentSourceRowOnDraw + 1f, 16f, 16f), sourceAsset.Icon);
                EditorGUILayout.LabelField(string.Empty, GUILayout.Width(26f + 14f * sourceAsset.Depth), GUILayout.Height(18f));
                EditorGUILayout.LabelField(sourceAsset.Name);
                Asset asset = m_Controller.GetAsset(sourceAsset.Guid);
                EditorGUILayout.LabelField(asset != null ? GetAssetBundleFullName(asset.AssetBundle.Name, asset.AssetBundle.Variant) : string.Empty, GUILayout.Width(position.width * 0.15f));
            }
            EditorGUILayout.EndHorizontal();
            m_CurrentSourceRowOnDraw++;
        }

        private void ChangeSelectedAssetBundle(AssetBundle assetBundle)
        {
            if (m_SelectedAssetBundle == assetBundle)
            {
                return;
            }

            m_SelectedAssetBundle = assetBundle;
            m_SelectedAssetsInSelectedAssetBundle.Clear();
        }

        private void SaveConfiguration()
        {
            if (m_Controller.Save())
            {
                Debug.Log("Save configuration success.");
            }
            else
            {
                Debug.LogWarning("Save configuration failure.");
            }
        }

        private void AddAssetBundle(string assetBundleName, string assetBundleVariant, bool refresh)
        {
            if (assetBundleVariant == string.Empty)
            {
                assetBundleVariant = null;
            }

            string assetBundleFullName = GetAssetBundleFullName(assetBundleName, assetBundleVariant);
            if (m_Controller.AddAssetBundle(assetBundleName, assetBundleVariant, AssetBundleLoadType.LoadFromFile, false))
            {
                if (refresh)
                {
                    RefreshAssetBundleTree();
                }

                Debug.Log(string.Format("Add AssetBundle '{0}' success.", assetBundleFullName));
                m_MenuState = MenuState.Normal;
            }
            else
            {
                Debug.LogWarning(string.Format("Add AssetBundle '{0}' failure.", assetBundleFullName));
            }
        }

        private void RenameAssetBundle(AssetBundle assetBundle, string newAssetBundleName, string newAssetBundleVariant)
        {
            if (assetBundle == null)
            {
                Debug.LogWarning("AssetBundle is invalid.");
                return;
            }

            if (newAssetBundleVariant == string.Empty)
            {
                newAssetBundleVariant = null;
            }

            string oldAssetBundleFullName = assetBundle.FullName;
            string newAssetBundleFullName = GetAssetBundleFullName(newAssetBundleName, newAssetBundleVariant);
            if (m_Controller.RenameAssetBundle(assetBundle.Name, assetBundle.Variant, newAssetBundleName, newAssetBundleVariant))
            {
                RefreshAssetBundleTree();
                Debug.Log(string.Format("Rename AssetBundle '{0}' to '{1}' success.", oldAssetBundleFullName, newAssetBundleFullName));
                m_MenuState = MenuState.Normal;
            }
            else
            {
                Debug.LogWarning(string.Format("Rename AssetBundle '{0}' to '{1}' failure.", oldAssetBundleFullName, newAssetBundleFullName));
            }
        }

        private void RemoveAssetBundle()
        {
            string assetBundleFullName = m_SelectedAssetBundle.FullName;
            if (m_Controller.RemoveAssetBundle(m_SelectedAssetBundle.Name, m_SelectedAssetBundle.Variant))
            {
                ChangeSelectedAssetBundle(null);
                RefreshAssetBundleTree();
                Debug.Log(string.Format("Remove AssetBundle '{0}' success.", assetBundleFullName));
            }
            else
            {
                Debug.LogWarning(string.Format("Remove AssetBundle '{0}' failure.", assetBundleFullName));
            }
        }

        private void SetAssetBundleLoadType(AssetBundleLoadType loadType)
        {
            string assetBundleFullName = m_SelectedAssetBundle.FullName;
            if (m_Controller.SetAssetBundleLoadType(m_SelectedAssetBundle.Name, m_SelectedAssetBundle.Variant, loadType))
            {
                Debug.Log(string.Format("Set AssetBundle '{0}' load type to '{1}' success.", assetBundleFullName, loadType.ToString()));
            }
            else
            {
                Debug.LogWarning(string.Format("Set AssetBundle '{0}' load type to '{1}' failure.", assetBundleFullName, loadType.ToString()));
            }
        }

        private void SetAssetBundlePacked(bool pack)
        {
            string assetBundleFullName = m_SelectedAssetBundle.FullName;
            if (m_Controller.SetAssetBundlePacked(m_SelectedAssetBundle.Name, m_SelectedAssetBundle.Variant, pack))
            {
                Debug.Log(string.Format("{1} AssetBundle '{0}' success.", assetBundleFullName, pack ? "Pack" : "Unpack"));
            }
            else
            {
                Debug.LogWarning(string.Format("{1} AssetBundle '{0}' failure.", assetBundleFullName, pack ? "Pack" : "Unpack"));
            }
        }

        private void AssignAsset(SourceAsset sourceAsset, AssetBundle assetBundle)
        {
            if (!m_Controller.AssignAsset(sourceAsset.Guid, assetBundle.Name, assetBundle.Variant))
            {
                Debug.LogWarning(string.Format("Assign asset '{0}' to AssetBundle '{1}' failure.", sourceAsset.Name, m_SelectedAssetBundle.FullName));
            }
        }

        private void UnassignAsset(Asset asset)
        {
            if (!m_Controller.UnassignAsset(asset.Guid))
            {
                Debug.LogWarning(string.Format("Unassign asset '{0}' from AssetBundle '{1}' failure.", asset.Guid, m_SelectedAssetBundle.FullName));
            }
        }

        private void CleanAssetBundle()
        {
            int unknownAssetCount = m_Controller.RemoveUnknownAssets();
            int unusedAssetBundleCount = m_Controller.RemoveUnusedAssetBundles();
            RefreshAssetBundleTree();

            Debug.Log(string.Format("Clean complete, {0} unknown assets and {1} unused AssetBundles has been removed.", unknownAssetCount.ToString(), unusedAssetBundleCount.ToString()));
        }

        private void RefreshAssetBundleTree()
        {
            m_AssetBundleRoot.Clear();
            AssetBundle[] assetBundles = m_Controller.GetAssetBundles();
            foreach (AssetBundle assetBundle in assetBundles)
            {
                string[] splitedPath = assetBundle.Name.Split('/');
                AssetBundleFolder folder = m_AssetBundleRoot;
                for (int i = 0; i < splitedPath.Length - 1; i++)
                {
                    AssetBundleFolder subFolder = folder.GetFolder(splitedPath[i]);
                    folder = subFolder == null ? folder.AddFolder(splitedPath[i]) : subFolder;
                }

                string assetBundleFullName = assetBundle.Variant != null ? string.Format("{0}.{1}", splitedPath[splitedPath.Length - 1], assetBundle.Variant) : splitedPath[splitedPath.Length - 1];
                folder.AddItem(assetBundleFullName, assetBundle);
            }
        }

        private bool IsExpandedAssetBundleFolder(AssetBundleFolder assetBundleFolder)
        {
            return m_ExpandedAssetBundleFolderNames.Contains(assetBundleFolder.FromRootPath);
        }

        private void SetExpandedAssetBundleFolder(AssetBundleFolder assetBundleFolder, bool expand)
        {
            if (expand)
            {
                m_ExpandedAssetBundleFolderNames.Add(assetBundleFolder.FromRootPath);
            }
            else
            {
                m_ExpandedAssetBundleFolderNames.Remove(assetBundleFolder.FromRootPath);
            }
        }

        private bool IsSelectedAssetInSelectedAssetBundle(Asset asset)
        {
            return m_SelectedAssetsInSelectedAssetBundle.Contains(asset);
        }

        private void SetSelectedAssetInSelectedAssetBundle(Asset asset, bool select)
        {
            if (select)
            {
                m_SelectedAssetsInSelectedAssetBundle.Add(asset);
            }
            else
            {
                m_SelectedAssetsInSelectedAssetBundle.Remove(asset);
            }
        }

        private bool IsExpandedSourceFolder(SourceFolder sourceFolder)
        {
            return m_ExpandedSourceFolders.Contains(sourceFolder);
        }

        private void SetExpandedSourceFolder(SourceFolder sourceFolder, bool expand)
        {
            if (expand)
            {
                m_ExpandedSourceFolders.Add(sourceFolder);
            }
            else
            {
                m_ExpandedSourceFolders.Remove(sourceFolder);
            }
        }

        private bool IsSelectedSourceFolder(SourceFolder sourceFolder)
        {
            if (m_CachedSelectedSourceFolders.Contains(sourceFolder))
            {
                return true;
            }

            if (m_CachedUnselectedSourceFolders.Contains(sourceFolder))
            {
                return false;
            }

            foreach (SourceAsset sourceAsset in sourceFolder.GetAssets())
            {
                if (m_HideAssignedSourceAssets && IsAssignedSourceAsset(sourceAsset))
                {
                    continue;
                }

                if (!IsSelectedSourceAsset(sourceAsset))
                {
                    m_CachedUnselectedSourceFolders.Add(sourceFolder);
                    return false;
                }
            }

            foreach (SourceFolder subSourceFolder in sourceFolder.GetFolders())
            {
                if (m_HideAssignedSourceAssets && IsAssignedSourceFolder(sourceFolder))
                {
                    continue;
                }

                if (!IsSelectedSourceFolder(subSourceFolder))
                {
                    m_CachedUnselectedSourceFolders.Add(sourceFolder);
                    return false;
                }
            }

            m_CachedSelectedSourceFolders.Add(sourceFolder);
            return true;
        }

        private void SetSelectedSourceFolder(SourceFolder sourceFolder, bool select)
        {
            if (select)
            {
                m_CachedSelectedSourceFolders.Add(sourceFolder);
                m_CachedUnselectedSourceFolders.Remove(sourceFolder);

                SourceFolder folder = sourceFolder;
                while (folder != null)
                {
                    m_CachedUnselectedSourceFolders.Remove(folder);
                    folder = folder.Folder;
                }
            }
            else
            {
                m_CachedSelectedSourceFolders.Remove(sourceFolder);
                m_CachedUnselectedSourceFolders.Add(sourceFolder);

                SourceFolder folder = sourceFolder;
                while (folder != null)
                {
                    m_CachedSelectedSourceFolders.Remove(folder);
                    folder = folder.Folder;
                }
            }

            foreach (SourceAsset sourceAsset in sourceFolder.GetAssets())
            {
                if (m_HideAssignedSourceAssets && IsAssignedSourceAsset(sourceAsset))
                {
                    continue;
                }

                SetSelectedSourceAsset(sourceAsset, select);
            }

            foreach (SourceFolder subSourceFolder in sourceFolder.GetFolders())
            {
                if (m_HideAssignedSourceAssets && IsAssignedSourceFolder(subSourceFolder))
                {
                    continue;
                }

                SetSelectedSourceFolder(subSourceFolder, select);
            }
        }

        private bool IsSelectedSourceAsset(SourceAsset sourceAsset)
        {
            return m_SelectedSourceAssets.Contains(sourceAsset);
        }

        private void SetSelectedSourceAsset(SourceAsset sourceAsset, bool select)
        {
            if (select)
            {
                m_SelectedSourceAssets.Add(sourceAsset);

                SourceFolder folder = sourceAsset.Folder;
                while (folder != null)
                {
                    m_CachedUnselectedSourceFolders.Remove(folder);
                    folder = folder.Folder;
                }
            }
            else
            {
                m_SelectedSourceAssets.Remove(sourceAsset);

                SourceFolder folder = sourceAsset.Folder;
                while (folder != null)
                {
                    m_CachedSelectedSourceFolders.Remove(folder);
                    folder = folder.Folder;
                }
            }
        }

        private bool IsAssignedSourceAsset(SourceAsset sourceAsset)
        {
            if (m_CachedAssignedSourceAssets.Contains(sourceAsset))
            {
                return true;
            }

            if (m_CachedUnassignedSourceAssets.Contains(sourceAsset))
            {
                return false;
            }

            return m_Controller.GetAsset(sourceAsset.Guid) != null;
        }

        private bool IsAssignedSourceFolder(SourceFolder sourceFolder)
        {
            if (m_CachedAssignedSourceFolders.Contains(sourceFolder))
            {
                return true;
            }

            if (m_CachedUnassignedSourceFolders.Contains(sourceFolder))
            {
                return false;
            }

            foreach (SourceAsset sourceAsset in sourceFolder.GetAssets())
            {
                if (!IsAssignedSourceAsset(sourceAsset))
                {
                    m_CachedUnassignedSourceFolders.Add(sourceFolder);
                    return false;
                }
            }

            foreach (SourceFolder subSourceFolder in sourceFolder.GetFolders())
            {
                if (!IsAssignedSourceFolder(subSourceFolder))
                {
                    m_CachedUnassignedSourceFolders.Add(sourceFolder);
                    return false;
                }
            }

            m_CachedAssignedSourceFolders.Add(sourceFolder);
            return true;
        }

        private HashSet<SourceAsset> GetSelectedSourceAssets()
        {
            if (!m_HideAssignedSourceAssets)
            {
                return m_SelectedSourceAssets;
            }

            HashSet<SourceAsset> selectedUnassignedSourceAssets = new HashSet<SourceAsset>();
            foreach (SourceAsset sourceAsset in m_SelectedSourceAssets)
            {
                if (!IsAssignedSourceAsset(sourceAsset))
                {
                    selectedUnassignedSourceAssets.Add(sourceAsset);
                }
            }

            return selectedUnassignedSourceAssets;
        }

        private string GetAssetBundleFullName(string assetBundleName, string assetBundleVariant)
        {
            return assetBundleVariant != null ? string.Format("{0}.{1}", assetBundleName, assetBundleVariant) : assetBundleName;
        }

        private void OnLoadingAssetBundle(int index, int count)
        {
            EditorUtility.DisplayProgressBar("Loading AssetBundles", string.Format("Loading AssetBundles, {0}/{1} loaded.", index.ToString(), count.ToString()), (float)index / count);
        }

        private void OnLoadingAsset(int index, int count)
        {
            EditorUtility.DisplayProgressBar("Loading Assets", string.Format("Loading assets, {0}/{1} loaded.", index.ToString(), count.ToString()), (float)index / count);
        }

        private void OnLoadCompleted()
        {
            EditorUtility.ClearProgressBar();
        }

        private void OnAssetAssigned(SourceAsset[] sourceAssets)
        {
            HashSet<SourceFolder> affectedFolders = new HashSet<SourceFolder>();
            foreach (SourceAsset sourceAsset in sourceAssets)
            {
                m_CachedAssignedSourceAssets.Add(sourceAsset);
                m_CachedUnassignedSourceAssets.Remove(sourceAsset);

                affectedFolders.Add(sourceAsset.Folder);
            }

            foreach (SourceFolder sourceFolder in affectedFolders)
            {
                SourceFolder folder = sourceFolder;
                while (folder != null)
                {
                    m_CachedUnassignedSourceFolders.Remove(folder);
                    folder = folder.Folder;
                }
            }
        }

        private void OnAssetUnassigned(SourceAsset[] sourceAssets)
        {
            HashSet<SourceFolder> affectedFolders = new HashSet<SourceFolder>();
            foreach (SourceAsset sourceAsset in sourceAssets)
            {
                m_CachedAssignedSourceAssets.Remove(sourceAsset);
                m_CachedUnassignedSourceAssets.Add(sourceAsset);

                affectedFolders.Add(sourceAsset.Folder);
            }

            foreach (SourceFolder sourceFolder in affectedFolders)
            {
                SourceFolder folder = sourceFolder;
                while (folder != null)
                {
                    m_CachedSelectedSourceFolders.Remove(folder);
                    m_CachedAssignedSourceFolders.Remove(folder);
                    m_CachedUnassignedSourceFolders.Add(folder);
                    folder = folder.Folder;
                }
            }
        }
    }
}
