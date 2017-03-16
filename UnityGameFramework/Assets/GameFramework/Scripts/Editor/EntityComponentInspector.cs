//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEditor;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(EntityComponent))]
    internal sealed class EntityComponentInspector : GameFrameworkInspector
    {
        private SerializedProperty m_EnableShowEntitySuccessEvent = null;
        private SerializedProperty m_EnableShowEntityFailureEvent = null;
        private SerializedProperty m_EnableShowEntityUpdateEvent = null;
        private SerializedProperty m_EnableShowEntityDependencyAssetEvent = null;
        private SerializedProperty m_EnableHideEntityCompleteEvent = null;
        private SerializedProperty m_InstanceRoot = null;
        private SerializedProperty m_EntityGroups = null;

        private HelperInfo<EntityHelperBase> m_EntityHelperInfo = new HelperInfo<EntityHelperBase>("Entity");
        private HelperInfo<EntityGroupHelperBase> m_EntityGroupHelperInfo = new HelperInfo<EntityGroupHelperBase>("EntityGroup");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EntityComponent t = (EntityComponent)target;

            EditorGUILayout.PropertyField(m_EnableShowEntitySuccessEvent);
            EditorGUILayout.PropertyField(m_EnableShowEntityFailureEvent);
            EditorGUILayout.PropertyField(m_EnableShowEntityUpdateEvent);
            EditorGUILayout.PropertyField(m_EnableShowEntityDependencyAssetEvent);
            EditorGUILayout.PropertyField(m_EnableHideEntityCompleteEvent);

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(m_InstanceRoot);
                m_EntityHelperInfo.Draw();
                m_EntityGroupHelperInfo.Draw();
                EditorGUILayout.PropertyField(m_EntityGroups, true);
            }
            EditorGUI.EndDisabledGroup();

            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField("Entity Count", t.EntityCount.ToString());
                EditorGUILayout.LabelField("Entity Group Count", t.EntityGroupCount.ToString());
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
            m_EnableShowEntitySuccessEvent = serializedObject.FindProperty("m_EnableShowEntitySuccessEvent");
            m_EnableShowEntityFailureEvent = serializedObject.FindProperty("m_EnableShowEntityFailureEvent");
            m_EnableShowEntityUpdateEvent = serializedObject.FindProperty("m_EnableShowEntityUpdateEvent");
            m_EnableShowEntityDependencyAssetEvent = serializedObject.FindProperty("m_EnableShowEntityDependencyAssetEvent");
            m_EnableHideEntityCompleteEvent = serializedObject.FindProperty("m_EnableHideEntityCompleteEvent");
            m_InstanceRoot = serializedObject.FindProperty("m_InstanceRoot");
            m_EntityGroups = serializedObject.FindProperty("m_EntityGroups");

            m_EntityHelperInfo.Init(serializedObject);
            m_EntityGroupHelperInfo.Init(serializedObject);

            RefreshTypeNames();
        }

        private void RefreshTypeNames()
        {
            m_EntityHelperInfo.Refresh();
            m_EntityGroupHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
