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
    [CustomEditor(typeof(LocalizationComponent))]
    internal sealed class LocalizationComponentInspector : GameFrameworkInspector
    {
        private SerializedProperty m_EnableLoadDictionarySuccessEvent = null;
        private SerializedProperty m_EnableLoadDictionaryFailureEvent = null;
        private SerializedProperty m_EnableLoadDictionaryUpdateEvent = null;
        private SerializedProperty m_EnableLoadDictionaryDependencyAssetEvent = null;

        private HelperInfo<LocalizationHelperBase> m_LocalizationHelperInfo = new HelperInfo<LocalizationHelperBase>("Localization");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            LocalizationComponent t = (LocalizationComponent)target;

            EditorGUILayout.PropertyField(m_EnableLoadDictionarySuccessEvent);
            EditorGUILayout.PropertyField(m_EnableLoadDictionaryFailureEvent);
            EditorGUILayout.PropertyField(m_EnableLoadDictionaryUpdateEvent);
            EditorGUILayout.PropertyField(m_EnableLoadDictionaryDependencyAssetEvent);

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                m_LocalizationHelperInfo.Draw();
            }
            EditorGUI.EndDisabledGroup();

            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField("Language", t.Language.ToString());
                EditorGUILayout.LabelField("System Language", t.SystemLanguage.ToString());
                EditorGUILayout.LabelField("Dictionary Count", t.DictionaryCount.ToString());
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
            m_EnableLoadDictionarySuccessEvent = serializedObject.FindProperty("m_EnableLoadDictionarySuccessEvent");
            m_EnableLoadDictionaryFailureEvent = serializedObject.FindProperty("m_EnableLoadDictionaryFailureEvent");
            m_EnableLoadDictionaryUpdateEvent = serializedObject.FindProperty("m_EnableLoadDictionaryUpdateEvent");
            m_EnableLoadDictionaryDependencyAssetEvent = serializedObject.FindProperty("m_EnableLoadDictionaryDependencyAssetEvent");

            m_LocalizationHelperInfo.Init(serializedObject);

            RefreshTypeNames();
        }

        private void RefreshTypeNames()
        {
            m_LocalizationHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
