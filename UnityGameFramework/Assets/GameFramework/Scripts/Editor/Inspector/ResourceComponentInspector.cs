//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Reflection;
using UnityEditor;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(ResourceComponent))]
    internal sealed class ResourceComponentInspector : GameFrameworkInspector
    {
        private SerializedProperty m_ResourceMode = null;
        private SerializedProperty m_ReadWritePathType = null;
        private SerializedProperty m_UnloadUnusedAssetsInterval = null;
        private SerializedProperty m_AssetAutoReleaseInterval = null;
        private SerializedProperty m_AssetCapacity = null;
        private SerializedProperty m_AssetExpireTime = null;
        private SerializedProperty m_AssetPriority = null;
        private SerializedProperty m_ResourceAutoReleaseInterval = null;
        private SerializedProperty m_ResourceCapacity = null;
        private SerializedProperty m_ResourceExpireTime = null;
        private SerializedProperty m_ResourcePriority = null;
        private SerializedProperty m_UpdatePrefixUri = null;
        private SerializedProperty m_UpdateRetryCount = null;
        private SerializedProperty m_InstanceRoot = null;
        private SerializedProperty m_LoadResourceAgentHelperCount = null;

        private FieldInfo m_EditorResourceModeFieldInfo = null;

        private string[] m_ResourceModeNames = new string[] { "Package", "Updatable" };
        private int m_ResourceModeIndex = 0;
        private HelperInfo<ResourceHelperBase> m_ResourceHelperInfo = new HelperInfo<ResourceHelperBase>("Resource");
        private HelperInfo<LoadResourceAgentHelperBase> m_LoadResourceAgentHelperInfo = new HelperInfo<LoadResourceAgentHelperBase>("LoadResourceAgent");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            ResourceComponent t = (ResourceComponent)target;

            bool isEditorResourceMode = (bool)m_EditorResourceModeFieldInfo.GetValue(target);

            if (isEditorResourceMode)
            {
                EditorGUILayout.HelpBox("Editor resource mode is enabled. Some options are disabled.", MessageType.Warning);
            }

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                if (EditorApplication.isPlaying)
                {
                    EditorGUILayout.EnumPopup("Resource Mode", t.ResourceMode);
                }
                else
                {
                    int selectedIndex = EditorGUILayout.Popup("Resource Mode", m_ResourceModeIndex, m_ResourceModeNames);
                    if (selectedIndex != m_ResourceModeIndex)
                    {
                        m_ResourceModeIndex = selectedIndex;
                        m_ResourceMode.enumValueIndex = selectedIndex + 1;
                    }
                }

                m_ReadWritePathType.enumValueIndex = (int)(ReadWritePathType)EditorGUILayout.EnumPopup("Read Write Path Type", t.ReadWritePathType);
            }
            EditorGUI.EndDisabledGroup();

            float unloadUnusedAssetsInterval = EditorGUILayout.Slider("Unload Unused Assets Interval", m_UnloadUnusedAssetsInterval.floatValue, 0f, 3600f);
            if (unloadUnusedAssetsInterval != m_UnloadUnusedAssetsInterval.floatValue)
            {
                if (EditorApplication.isPlaying)
                {
                    t.UnloadUnusedAssetsInterval = unloadUnusedAssetsInterval;
                }
                else
                {
                    m_UnloadUnusedAssetsInterval.floatValue = unloadUnusedAssetsInterval;
                }
            }
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlaying && isEditorResourceMode);
            {
                float assetAutoReleaseInterval = EditorGUILayout.DelayedFloatField("Asset Auto Release Interval", m_AssetAutoReleaseInterval.floatValue);
                if (assetAutoReleaseInterval != m_AssetAutoReleaseInterval.floatValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.AssetAutoReleaseInterval = assetAutoReleaseInterval;
                    }
                    else
                    {
                        m_AssetAutoReleaseInterval.floatValue = assetAutoReleaseInterval;
                    }
                }

                int assetCapacity = EditorGUILayout.DelayedIntField("Asset Capacity", m_AssetCapacity.intValue);
                if (assetCapacity != m_AssetCapacity.intValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.AssetCapacity = assetCapacity;
                    }
                    else
                    {
                        m_AssetCapacity.intValue = assetCapacity;
                    }
                }

                float assetExpireTime = EditorGUILayout.DelayedFloatField("Asset Expire Time", m_AssetExpireTime.floatValue);
                if (assetExpireTime != m_AssetExpireTime.floatValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.AssetExpireTime = assetExpireTime;
                    }
                    else
                    {
                        m_AssetExpireTime.floatValue = assetExpireTime;
                    }
                }

                int assetPriority = EditorGUILayout.DelayedIntField("Asset Priority", m_AssetPriority.intValue);
                if (assetPriority != m_AssetPriority.intValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.AssetPriority = assetPriority;
                    }
                    else
                    {
                        m_AssetPriority.intValue = assetPriority;
                    }
                }

                float resourceAutoReleaseInterval = EditorGUILayout.DelayedFloatField("Resource Auto Release Interval", m_ResourceAutoReleaseInterval.floatValue);
                if (resourceAutoReleaseInterval != m_ResourceAutoReleaseInterval.floatValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.ResourceAutoReleaseInterval = resourceAutoReleaseInterval;
                    }
                    else
                    {
                        m_ResourceAutoReleaseInterval.floatValue = resourceAutoReleaseInterval;
                    }
                }

                int resourceCapacity = EditorGUILayout.DelayedIntField("Resource Capacity", m_ResourceCapacity.intValue);
                if (resourceCapacity != m_ResourceCapacity.intValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.ResourceCapacity = resourceCapacity;
                    }
                    else
                    {
                        m_ResourceCapacity.intValue = resourceCapacity;
                    }
                }

                float resourceExpireTime = EditorGUILayout.DelayedFloatField("Resource Expire Time", m_ResourceExpireTime.floatValue);
                if (resourceExpireTime != m_ResourceExpireTime.floatValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.ResourceExpireTime = resourceExpireTime;
                    }
                    else
                    {
                        m_ResourceExpireTime.floatValue = resourceExpireTime;
                    }
                }

                int resourcePriority = EditorGUILayout.DelayedIntField("Resource Priority", m_ResourcePriority.intValue);
                if (resourcePriority != m_ResourcePriority.intValue)
                {
                    if (EditorApplication.isPlaying)
                    {
                        t.ResourcePriority = resourcePriority;
                    }
                    else
                    {
                        m_ResourcePriority.intValue = resourcePriority;
                    }
                }

                if (m_ResourceModeIndex == 1)
                {
                    string updatePrefixUri = EditorGUILayout.DelayedTextField("Update Prefix Uri", m_UpdatePrefixUri.stringValue);
                    if (updatePrefixUri != m_UpdatePrefixUri.stringValue)
                    {
                        if (EditorApplication.isPlaying)
                        {
                            t.UpdatePrefixUri = updatePrefixUri;
                        }
                        else
                        {
                            m_UpdatePrefixUri.stringValue = updatePrefixUri;
                        }
                    }

                    int updateRetryCount = EditorGUILayout.DelayedIntField("Update Retry Count", m_UpdateRetryCount.intValue);
                    if (updateRetryCount != m_UpdateRetryCount.intValue)
                    {
                        if (EditorApplication.isPlaying)
                        {
                            t.UpdateRetryCount = resourceCapacity;
                        }
                        else
                        {
                            m_UpdateRetryCount.intValue = resourceCapacity;
                        }
                    }
                }
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(m_InstanceRoot);

                m_ResourceHelperInfo.Draw();
                m_LoadResourceAgentHelperInfo.Draw();
                m_LoadResourceAgentHelperCount.intValue = EditorGUILayout.IntSlider("Load Resource Agent Helper Count", m_LoadResourceAgentHelperCount.intValue, 1, 64);
            }
            EditorGUI.EndDisabledGroup();

            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField("Read Only Path", t.ReadOnlyPath.ToString());
                EditorGUILayout.LabelField("Read Write Path", t.ReadWritePath.ToString());
                EditorGUILayout.LabelField("Current Variant", t.CurrentVariant ?? "<Unknwon>");
                EditorGUILayout.LabelField("Applicable Game Version", isEditorResourceMode ? "N/A" : t.ApplicableGameVersion ?? "<Unknwon>");
                EditorGUILayout.LabelField("Internal Resource Version", isEditorResourceMode ? "N/A" : t.InternalResourceVersion.ToString());
                EditorGUILayout.LabelField("Asset Count", isEditorResourceMode ? "N/A" : t.AssetCount.ToString());
                EditorGUILayout.LabelField("Resource Count", isEditorResourceMode ? "N/A" : t.ResourceCount.ToString());
                EditorGUILayout.LabelField("Resource Group Count", isEditorResourceMode ? "N/A" : t.ResourceGroupCount.ToString());
                if (m_ResourceModeIndex == 1)
                {
                    EditorGUILayout.LabelField("Update Waiting Count", isEditorResourceMode ? "N/A" : t.UpdateWaitingCount.ToString());
                    EditorGUILayout.LabelField("Updating Count", isEditorResourceMode ? "N/A" : t.UpdatingCount.ToString());
                }
                EditorGUILayout.LabelField("Load Total Agent Count", isEditorResourceMode ? "N/A" : t.LoadTotalAgentCount.ToString());
                EditorGUILayout.LabelField("Load Free Agent Count", isEditorResourceMode ? "N/A" : t.LoadFreeAgentCount.ToString());
                EditorGUILayout.LabelField("Load Working Agent Count", isEditorResourceMode ? "N/A" : t.LoadWorkingAgentCount.ToString());
                EditorGUILayout.LabelField("Load Waiting Task Count", isEditorResourceMode ? "N/A" : t.LoadWaitingTaskCount.ToString());
            }

            serializedObject.ApplyModifiedProperties();

            Repaint();
        }

        protected override void OnCompileComplete()
        {
            base.OnCompileComplete();

            RefreshTypeNames();
        }

        private void OnEnable()
        {
            m_ResourceMode = serializedObject.FindProperty("m_ResourceMode");
            m_ReadWritePathType = serializedObject.FindProperty("m_ReadWritePathType");
            m_UnloadUnusedAssetsInterval = serializedObject.FindProperty("m_UnloadUnusedAssetsInterval");
            m_AssetAutoReleaseInterval = serializedObject.FindProperty("m_AssetAutoReleaseInterval");
            m_AssetCapacity = serializedObject.FindProperty("m_AssetCapacity");
            m_AssetExpireTime = serializedObject.FindProperty("m_AssetExpireTime");
            m_AssetPriority = serializedObject.FindProperty("m_AssetPriority");
            m_ResourceAutoReleaseInterval = serializedObject.FindProperty("m_ResourceAutoReleaseInterval");
            m_ResourceCapacity = serializedObject.FindProperty("m_ResourceCapacity");
            m_ResourceExpireTime = serializedObject.FindProperty("m_ResourceExpireTime");
            m_ResourcePriority = serializedObject.FindProperty("m_ResourcePriority");
            m_UpdatePrefixUri = serializedObject.FindProperty("m_UpdatePrefixUri");
            m_UpdateRetryCount = serializedObject.FindProperty("m_UpdateRetryCount");
            m_InstanceRoot = serializedObject.FindProperty("m_InstanceRoot");
            m_LoadResourceAgentHelperCount = serializedObject.FindProperty("m_LoadResourceAgentHelperCount");

            m_EditorResourceModeFieldInfo = target.GetType().GetField("m_EditorResourceMode", BindingFlags.NonPublic | BindingFlags.Instance);

            m_ResourceHelperInfo.Init(serializedObject);
            m_LoadResourceAgentHelperInfo.Init(serializedObject);

            RefreshModes();
            RefreshTypeNames();
        }

        private void RefreshModes()
        {
            m_ResourceModeIndex = (m_ResourceMode.enumValueIndex > 0 ? m_ResourceMode.enumValueIndex - 1 : 0);
        }

        private void RefreshTypeNames()
        {
            m_ResourceHelperInfo.Refresh();
            m_LoadResourceAgentHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
