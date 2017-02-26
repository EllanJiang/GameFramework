//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.IO;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    /// <summary>
    /// 资源包生成器。
    /// </summary>
    internal sealed class AssetBundleBuilder : EditorWindow
    {
        private AssetBundleBuilderController m_Controller = null;
        private bool m_OrderBuildAssetBundles = false;

        [MenuItem("Game Framework/AssetBundle Tools/AssetBundle Builder", false, 31)]
        private static void Open()
        {
            AssetBundleBuilder window = GetWindow<AssetBundleBuilder>(true, "AssetBundle Builder", true);
            window.minSize = window.maxSize = new Vector2(666f, 555f);
        }

        private void OnEnable()
        {
            m_Controller = new AssetBundleBuilderController();
            m_Controller.OnLoadingAssetBundle += OnLoadingAssetBundle;
            m_Controller.OnLoadingAsset += OnLoadingAsset;
            m_Controller.OnLoadCompleted += OnLoadCompleted;
            m_Controller.OnAnalyzingAsset += OnAnalyzingAsset;
            m_Controller.OnAnalyzeCompleted += OnAnalyzeCompleted;
            m_Controller.ProcessingAssetBundle += OnProcessingAssetBundle;
            m_Controller.ProcessAssetBundleComplete += OnProcessAssetBundleComplete;
            m_Controller.BuildAssetBundlesError += OnBuildAssetBundlesError;

            m_OrderBuildAssetBundles = false;

            if (m_Controller.Load())
            {
                Debug.Log("Load configuration success.");
            }
            else
            {
                Debug.LogWarning("Load configuration failure.");
            }
        }

        private void Update()
        {
            if (m_OrderBuildAssetBundles)
            {
                m_OrderBuildAssetBundles = false;
                BuildAssetBundles();
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(position.width), GUILayout.Height(position.height));
            {
                GUILayout.Space(5f);
                EditorGUILayout.LabelField("Environment Information", EditorStyles.boldLabel);
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Product Name", GUILayout.Width(160f));
                        EditorGUILayout.LabelField(m_Controller.ProductName);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Company Name", GUILayout.Width(160f));
                        EditorGUILayout.LabelField(m_Controller.CompanyName);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Game Identifier", GUILayout.Width(160f));
                        EditorGUILayout.LabelField(m_Controller.GameIdentifier);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Applicable Game Version", GUILayout.Width(160f));
                        EditorGUILayout.LabelField(m_Controller.ApplicableGameVersion);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                GUILayout.Space(5f);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.LabelField("Build Target", EditorStyles.boldLabel);
                        EditorGUILayout.BeginVertical("box");
                        {
                            m_Controller.WindowsSelected = EditorGUILayout.ToggleLeft("Microsoft Windows", m_Controller.WindowsSelected);
                            m_Controller.MacOSXSelected = EditorGUILayout.ToggleLeft("Apple Mac OS X", m_Controller.MacOSXSelected);
                            m_Controller.IOSSelected = EditorGUILayout.ToggleLeft("Apple iPhone/iPad", m_Controller.IOSSelected);
                            m_Controller.AndroidSelected = EditorGUILayout.ToggleLeft("Google Android", m_Controller.AndroidSelected);
                            m_Controller.WindowsStoreSelected = EditorGUILayout.ToggleLeft("Microsoft Windows Store", m_Controller.WindowsStoreSelected);
                        }
                        EditorGUILayout.EndVertical();
                        EditorGUILayout.BeginVertical();
                        {
                            m_Controller.ZipSelected = EditorGUILayout.ToggleLeft("Zip All AssetBundles", m_Controller.ZipSelected);
                            m_Controller.RecordScatteredDependencyAssetsSelected = EditorGUILayout.ToggleLeft("Record Scattered Dependency Assets", m_Controller.RecordScatteredDependencyAssetsSelected);
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginVertical();
                    {
                        EditorGUILayout.LabelField("AssetBundle Options", EditorStyles.boldLabel);
                        EditorGUILayout.BeginVertical("box");
                        {
                            bool uncompressedAssetBundleSelected = EditorGUILayout.ToggleLeft("Uncompressed AssetBundle", m_Controller.UncompressedAssetBundleSelected);
                            if (m_Controller.UncompressedAssetBundleSelected != uncompressedAssetBundleSelected)
                            {
                                m_Controller.UncompressedAssetBundleSelected = uncompressedAssetBundleSelected;
                                if (m_Controller.UncompressedAssetBundleSelected)
                                {
                                    m_Controller.ChunkBasedCompressionSelected = false;
                                }
                            }

                            bool disableWriteTypeTreeSelected = EditorGUILayout.ToggleLeft("Disable Write TypeTree", m_Controller.DisableWriteTypeTreeSelected);
                            if (m_Controller.DisableWriteTypeTreeSelected != disableWriteTypeTreeSelected)
                            {
                                m_Controller.DisableWriteTypeTreeSelected = disableWriteTypeTreeSelected;
                                if (m_Controller.DisableWriteTypeTreeSelected)
                                {
                                    m_Controller.IgnoreTypeTreeChangesSelected = false;
                                }
                            }

                            m_Controller.DeterministicAssetBundleSelected = EditorGUILayout.ToggleLeft("Deterministic AssetBundle", m_Controller.DeterministicAssetBundleSelected);
                            m_Controller.ForceRebuildAssetBundleSelected = EditorGUILayout.ToggleLeft("Force Rebuild AssetBundle", m_Controller.ForceRebuildAssetBundleSelected);

                            bool ignoreTypeTreeChangesSelected = EditorGUILayout.ToggleLeft("Ignore TypeTree Changes", m_Controller.IgnoreTypeTreeChangesSelected);
                            if (m_Controller.IgnoreTypeTreeChangesSelected != ignoreTypeTreeChangesSelected)
                            {
                                m_Controller.IgnoreTypeTreeChangesSelected = ignoreTypeTreeChangesSelected;
                                if (m_Controller.IgnoreTypeTreeChangesSelected)
                                {
                                    m_Controller.DisableWriteTypeTreeSelected = false;
                                }
                            }

                            EditorGUI.BeginDisabledGroup(true);
                            {
                                m_Controller.AppendHashToAssetBundleNameSelected = EditorGUILayout.ToggleLeft("Append Hash To AssetBundle Name", m_Controller.AppendHashToAssetBundleNameSelected);
                            }
                            EditorGUI.EndDisabledGroup();

                            bool chunkBasedCompressionSelected = EditorGUILayout.ToggleLeft("Chunk Based Compression", m_Controller.ChunkBasedCompressionSelected);
                            if (m_Controller.ChunkBasedCompressionSelected != chunkBasedCompressionSelected)
                            {
                                m_Controller.ChunkBasedCompressionSelected = chunkBasedCompressionSelected;
                                if (m_Controller.ChunkBasedCompressionSelected)
                                {
                                    m_Controller.UncompressedAssetBundleSelected = false;
                                }
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndHorizontal();
                string compressMessage = string.Empty;
                MessageType compressMessageType = MessageType.None;
                GetCompressMessage(out compressMessage, out compressMessageType);
                EditorGUILayout.HelpBox(compressMessage, compressMessageType);
                GUILayout.Space(5f);
                EditorGUILayout.LabelField("Build", EditorStyles.boldLabel);
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Internal Resource Version", GUILayout.Width(160f));
                        m_Controller.InternalResourceVersion = EditorGUILayout.IntField(m_Controller.InternalResourceVersion);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Resource Version", GUILayout.Width(160f));
                        GUILayout.Label(string.Format("{0} ({1})", m_Controller.ApplicableGameVersion, m_Controller.InternalResourceVersion.ToString()));
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Output Directory", GUILayout.Width(160f));
                        m_Controller.OutputDirectory = EditorGUILayout.TextField(m_Controller.OutputDirectory);
                        if (GUILayout.Button("Browse...", GUILayout.Width(80f)))
                        {
                            BrowseOutputDirectory();
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Working Path", GUILayout.Width(160f));
                        GUILayout.Label(m_Controller.WorkingPath);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Output Package Path", GUILayout.Width(160f));
                        GUILayout.Label(m_Controller.OutputPackagePath);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Output Full Path", GUILayout.Width(160f));
                        GUILayout.Label(m_Controller.OutputFullPath);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Output Packed Path", GUILayout.Width(160f));
                        GUILayout.Label(m_Controller.OutputPackedPath);
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("Build Report Path", GUILayout.Width(160f));
                        GUILayout.Label(m_Controller.BuildReportPath);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                string buildMessage = string.Empty;
                MessageType buildMessageType = MessageType.None;
                GetBuildMessage(out buildMessage, out buildMessageType);
                EditorGUILayout.HelpBox(buildMessage, buildMessageType);
                GUILayout.Space(2f);
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUI.BeginDisabledGroup(!m_Controller.IsValidOutputDirectory);
                    {
                        if (GUILayout.Button("Start Build AssetBundles"))
                        {
                            m_OrderBuildAssetBundles = true;
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                    if (GUILayout.Button("Save", GUILayout.Width(80f)))
                    {
                        SaveConfiguration();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }

        private void BrowseOutputDirectory()
        {
            string directory = EditorUtility.OpenFolderPanel("Select Output Directory", m_Controller.OutputDirectory, string.Empty);
            if (!string.IsNullOrEmpty(directory))
            {
                m_Controller.OutputDirectory = directory;
            }
        }

        private void GetCompressMessage(out string message, out MessageType messageType)
        {
            if (m_Controller.ZipSelected)
            {
                if (m_Controller.UncompressedAssetBundleSelected)
                {
                    message = "Compresses AssetBundles with ZIP only. It uses more storage but it's faster when loading the AssetBundles.";
                    messageType = MessageType.Info;
                }
                else if (m_Controller.ChunkBasedCompressionSelected)
                {
                    message = "Compresses AssetBundles with both chunk-based compression and ZIP. Recommended when you use 'AssetBundle.LoadFromFile'.";
                    messageType = MessageType.Info;
                }
                else
                {
                    message = "Compresses AssetBundles with both LZMA and ZIP. Not recommended.";
                    messageType = MessageType.Warning;
                }
            }
            else
            {
                if (m_Controller.UncompressedAssetBundleSelected)
                {
                    message = "Doesn't compress AssetBundles at all. Not recommended.";
                    messageType = MessageType.Warning;
                }
                else if (m_Controller.ChunkBasedCompressionSelected)
                {
                    message = "Compresses AssetBundles with chunk-based compression only. Recommended when you use 'AssetBundle.LoadFromFile'.";
                    messageType = MessageType.Info;
                }
                else
                {
                    message = "Compresses AssetBundles with LZMA only. Recommended when you use 'AssetBundle.LoadFromMemory'.";
                    messageType = MessageType.Info;
                }
            }
        }

        private void GetBuildMessage(out string message, out MessageType messageType)
        {
            if (!m_Controller.IsValidOutputDirectory)
            {
                message = "Output directory is invalid.";
                messageType = MessageType.Error;
                return;
            }

            message = string.Empty;
            messageType = MessageType.Info;
            if (Directory.Exists(m_Controller.OutputPackagePath))
            {
                message += string.Format("{0} will be overwritten.", m_Controller.OutputPackagePath);
                messageType = MessageType.Warning;
            }

            if (Directory.Exists(m_Controller.OutputFullPath))
            {
                if (message.Length > 0)
                {
                    message += " ";
                }

                message += string.Format("{0} will be overwritten.", m_Controller.OutputFullPath);
                messageType = MessageType.Warning;
            }

            if (Directory.Exists(m_Controller.OutputPackedPath))
            {
                if (message.Length > 0)
                {
                    message += " ";
                }

                message += string.Format("{0} will be overwritten.", m_Controller.OutputPackedPath);
                messageType = MessageType.Warning;
            }

            if (messageType == MessageType.Warning)
            {
                return;
            }

            message = "Ready to build.";
        }

        private void BuildAssetBundles()
        {
            if (m_Controller.BuildAssetBundles())
            {
                Debug.Log("Build AssetBundles success.");
                SaveConfiguration();
            }
            else
            {
                Debug.LogWarning("Build AssetBundles failure.");
            }
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

        private void OnAnalyzingAsset(int index, int count)
        {
            EditorUtility.DisplayProgressBar("Analyzing assets", string.Format("Analyzing assets, {0}/{1} analyzed.", index.ToString(), count.ToString()), (float)index / count);
        }

        private void OnAnalyzeCompleted()
        {
            EditorUtility.ClearProgressBar();
        }

        private bool OnProcessingAssetBundle(string assetBundleName, float progress)
        {
            if (EditorUtility.DisplayCancelableProgressBar("Processing AssetBundle", string.Format("Processing '{0}'...", assetBundleName), progress))
            {
                EditorUtility.ClearProgressBar();
                return true;
            }
            else
            {
                Repaint();
                return false;
            }
        }

        private void OnProcessAssetBundleComplete(BuildTarget buildTarget, string versionListPath, int versionListLength, int versionListHashCode, int versionListZipLength, int versionListZipHashCode)
        {
            EditorUtility.ClearProgressBar();
            Debug.Log(string.Format("Build AssetBundles for '{0}' complete, version list path is '{1}', length is '{2}', hash code is '{3}', zip length is '{4}', zip hash code is '{5}'.", buildTarget.ToString(), versionListPath, versionListLength.ToString(), versionListHashCode.ToString("X8"), versionListZipLength.ToString(), versionListZipHashCode.ToString("X8")));
        }

        private void OnBuildAssetBundlesError(string errorMessage)
        {
            EditorUtility.ClearProgressBar();
            Debug.LogWarning(string.Format("Build AssetBundles error with error message '{0}'.", errorMessage));
        }
    }
}
