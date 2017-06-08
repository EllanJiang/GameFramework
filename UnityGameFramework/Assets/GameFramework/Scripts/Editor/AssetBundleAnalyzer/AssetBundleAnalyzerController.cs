//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    internal sealed partial class AssetBundleAnalyzerController
    {
        private readonly AssetBundleCollection m_AssetBundleCollection;

        private readonly Dictionary<string, DependencyData> m_DependencyDatas;
        private readonly Dictionary<string, List<Asset>> m_ScatteredAssets;
        private readonly HashSet<Stamp> m_AnalyzedStamps;

        public AssetBundleAnalyzerController()
            : this(null)
        {

        }

        public AssetBundleAnalyzerController(AssetBundleCollection assetBundleCollection)
        {
            m_AssetBundleCollection = (assetBundleCollection != null ? assetBundleCollection : new AssetBundleCollection());

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

            m_DependencyDatas = new Dictionary<string, DependencyData>();
            m_ScatteredAssets = new Dictionary<string, List<Asset>>();
            m_AnalyzedStamps = new HashSet<Stamp>();
        }

        public event GameFrameworkAction<int, int> OnLoadingAssetBundle = null;

        public event GameFrameworkAction<int, int> OnLoadingAsset = null;

        public event GameFrameworkAction OnLoadCompleted = null;

        public event GameFrameworkAction<int, int> OnAnalyzingAsset = null;

        public event GameFrameworkAction OnAnalyzeCompleted = null;

        public void Clear()
        {
            m_AssetBundleCollection.Clear();
            m_DependencyDatas.Clear();
            m_ScatteredAssets.Clear();
            m_AnalyzedStamps.Clear();
        }

        public bool Prepare()
        {
            m_AssetBundleCollection.Clear();
            return m_AssetBundleCollection.Load();
        }

        public void Analyze()
        {
            m_DependencyDatas.Clear();
            m_ScatteredAssets.Clear();
            m_AnalyzedStamps.Clear();

            HashSet<string> scriptAssetNames = GetFilteredAssetNames("t:Script");
            Asset[] assets = m_AssetBundleCollection.GetAssets();
            int count = assets.Length;
            for (int i = 0; i < count; i++)
            {
                if (OnAnalyzingAsset != null)
                {
                    OnAnalyzingAsset(i, count);
                }

                string assetName = assets[i].Name;
                if (string.IsNullOrEmpty(assetName))
                {
                    Debug.LogWarning(string.Format("Can not find asset by guid '{0}'.", assets[i].Guid));
                    continue;
                }

                DependencyData dependencyData = new DependencyData();
                AnalyzeAsset(assetName, assets[i], dependencyData, scriptAssetNames);
                dependencyData.RefreshData();
                m_DependencyDatas.Add(assetName, dependencyData);
            }

            foreach (List<Asset> scatteredAsset in m_ScatteredAssets.Values)
            {
                scatteredAsset.Sort((a, b) => a.Name.CompareTo(b.Name));
            }

            if (OnAnalyzeCompleted != null)
            {
                OnAnalyzeCompleted();
            }
        }

        private void AnalyzeAsset(string assetName, Asset hostAsset, DependencyData dependencyData, HashSet<string> scriptAssetNames)
        {
            string[] dependencyAssetNames = AssetDatabase.GetDependencies(assetName, false);
            foreach (string dependencyAssetName in dependencyAssetNames)
            {
                if (scriptAssetNames.Contains(dependencyAssetName))
                {
                    continue;
                }

                if (dependencyAssetName == assetName)
                {
                    continue;
                }

                Stamp stamp = new Stamp(dependencyAssetName, hostAsset.Name);
                if (m_AnalyzedStamps.Contains(stamp))
                {
                    continue;
                }

                m_AnalyzedStamps.Add(stamp);

                string guid = AssetDatabase.AssetPathToGUID(dependencyAssetName);
                if (string.IsNullOrEmpty(guid))
                {
                    Debug.LogWarning(string.Format("Can not find guid by asset '{0}'.", dependencyAssetName));
                    continue;
                }

                Asset asset = m_AssetBundleCollection.GetAsset(guid);
                if (asset != null)
                {
                    dependencyData.AddDependencyAsset(asset);
                }
                else
                {
                    dependencyData.AddScatteredDependencyAsset(dependencyAssetName);

                    if (!m_ScatteredAssets.ContainsKey(dependencyAssetName))
                    {
                        m_ScatteredAssets[dependencyAssetName] = new List<Asset>();
                    }

                    m_ScatteredAssets[dependencyAssetName].Add(hostAsset);

                    AnalyzeAsset(dependencyAssetName, hostAsset, dependencyData, scriptAssetNames);
                }
            }
        }

        public Asset GetAsset(string assetName)
        {
            return m_AssetBundleCollection.GetAsset(AssetDatabase.AssetPathToGUID(assetName));
        }

        public string[] GetAssetNames()
        {
            return GetAssetNames(AssetsOrder.AssetNameAsc, null);
        }

        public string[] GetAssetNames(AssetsOrder order, string filter)
        {
            HashSet<string> filteredAssetNames = GetFilteredAssetNames(filter);
            IEnumerable<KeyValuePair<string, DependencyData>> filteredResult = m_DependencyDatas.Where(pair => filteredAssetNames.Contains(pair.Key));
            IEnumerable<KeyValuePair<string, DependencyData>> orderedResult = null;
            switch (order)
            {
                case AssetsOrder.AssetNameAsc:
                    orderedResult = filteredResult.OrderBy(pair => pair.Key);
                    break;
                case AssetsOrder.AssetNameDesc:
                    orderedResult = filteredResult.OrderByDescending(pair => pair.Key);
                    break;
                case AssetsOrder.DependencyAssetBundleCountAsc:
                    orderedResult = filteredResult.OrderBy(pair => pair.Value.DependencyAssetBundleCount);
                    break;
                case AssetsOrder.DependencyAssetBundleCountDesc:
                    orderedResult = filteredResult.OrderByDescending(pair => pair.Value.DependencyAssetBundleCount);
                    break;
                case AssetsOrder.DependencyAssetCountAsc:
                    orderedResult = filteredResult.OrderBy(pair => pair.Value.DependencyAssetCount);
                    break;
                case AssetsOrder.DependencyAssetCountDesc:
                    orderedResult = filteredResult.OrderByDescending(pair => pair.Value.DependencyAssetCount);
                    break;
                case AssetsOrder.ScatteredDependencyAssetCountAsc:
                    orderedResult = filteredResult.OrderBy(pair => pair.Value.ScatteredDependencyAssetCount);
                    break;
                case AssetsOrder.ScatteredDependencyAssetCountDesc:
                    orderedResult = filteredResult.OrderByDescending(pair => pair.Value.ScatteredDependencyAssetCount);
                    break;
                default:
                    orderedResult = filteredResult;
                    break;
            }

            return orderedResult.Select(pair => pair.Key).ToArray();
        }

        public DependencyData GetDependencyData(string assetName)
        {
            DependencyData dependencyData = null;
            if (m_DependencyDatas.TryGetValue(assetName, out dependencyData))
            {
                return dependencyData;
            }

            return dependencyData;
        }

        public string[] GetScatteredAssetNames()
        {
            return GetScatteredAssetNames(ScatteredAssetsOrder.HostAssetCountDesc, null);
        }

        public string[] GetScatteredAssetNames(ScatteredAssetsOrder order, string filter)
        {
            HashSet<string> filterAssetNames = GetFilteredAssetNames(filter);
            IEnumerable<KeyValuePair<string, List<Asset>>> filteredResult = m_ScatteredAssets.Where(pair => filterAssetNames.Contains(pair.Key) && pair.Value.Count > 1);
            IEnumerable<KeyValuePair<string, List<Asset>>> orderedResult = null;
            switch (order)
            {
                case ScatteredAssetsOrder.AssetNameAsc:
                    orderedResult = filteredResult.OrderBy(pair => pair.Key);
                    break;
                case ScatteredAssetsOrder.AssetNameDesc:
                    orderedResult = filteredResult.OrderByDescending(pair => pair.Key);
                    break;
                case ScatteredAssetsOrder.HostAssetCountAsc:
                    orderedResult = filteredResult.OrderBy(pair => pair.Value.Count);
                    break;
                case ScatteredAssetsOrder.HostAssetCountDesc:
                    orderedResult = filteredResult.OrderByDescending(pair => pair.Value.Count);
                    break;
                default:
                    orderedResult = filteredResult;
                    break;
            }

            return orderedResult.Select(pair => pair.Key).ToArray();
        }

        public Asset[] GetHostAssets(string scatteredAssetName)
        {
            List<Asset> assets = null;
            if (m_ScatteredAssets.TryGetValue(scatteredAssetName, out assets))
            {
                return assets.ToArray();
            }

            return null;
        }

        private HashSet<string> GetFilteredAssetNames(string filter)
        {
            string[] filterAssetGuids = AssetDatabase.FindAssets(filter);
            HashSet<string> filterAssetNames = new HashSet<string>();
            foreach (string filterAssetGuid in filterAssetGuids)
            {
                filterAssetNames.Add(AssetDatabase.GUIDToAssetPath(filterAssetGuid));
            }

            return filterAssetNames;
        }
    }
}
