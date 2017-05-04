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
    [CustomEditor(typeof(SoundComponent))]
    internal sealed class SoundComponentInspector : GameFrameworkInspector
    {
        private SerializedProperty m_EnablePlaySoundSuccessEvent = null;
        private SerializedProperty m_EnablePlaySoundFailureEvent = null;
        private SerializedProperty m_EnablePlaySoundUpdateEvent = null;
        private SerializedProperty m_EnablePlaySoundDependencyAssetEvent = null;
        private SerializedProperty m_InstanceRoot = null;
        private SerializedProperty m_AudioMixer = null;
        private SerializedProperty m_SoundGroups = null;

        private HelperInfo<SoundHelperBase> m_SoundHelperInfo = new HelperInfo<SoundHelperBase>("Sound");
        private HelperInfo<SoundGroupHelperBase> m_SoundGroupHelperInfo = new HelperInfo<SoundGroupHelperBase>("SoundGroup");
        private HelperInfo<SoundAgentHelperBase> m_SoundAgentHelperInfo = new HelperInfo<SoundAgentHelperBase>("SoundAgent");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            SoundComponent t = (SoundComponent)target;

            EditorGUILayout.PropertyField(m_EnablePlaySoundSuccessEvent);
            EditorGUILayout.PropertyField(m_EnablePlaySoundFailureEvent);
            EditorGUILayout.PropertyField(m_EnablePlaySoundUpdateEvent);
            EditorGUILayout.PropertyField(m_EnablePlaySoundDependencyAssetEvent);

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                EditorGUILayout.PropertyField(m_InstanceRoot);
                EditorGUILayout.PropertyField(m_AudioMixer);
                m_SoundHelperInfo.Draw();
                m_SoundGroupHelperInfo.Draw();
                m_SoundAgentHelperInfo.Draw();
                EditorGUILayout.PropertyField(m_SoundGroups, true);
            }
            EditorGUI.EndDisabledGroup();

            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField("Sound Group Count", t.SoundGroupCount.ToString());
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
            m_EnablePlaySoundSuccessEvent = serializedObject.FindProperty("m_EnablePlaySoundSuccessEvent");
            m_EnablePlaySoundFailureEvent = serializedObject.FindProperty("m_EnablePlaySoundFailureEvent");
            m_EnablePlaySoundUpdateEvent = serializedObject.FindProperty("m_EnablePlaySoundUpdateEvent");
            m_EnablePlaySoundDependencyAssetEvent = serializedObject.FindProperty("m_EnablePlaySoundDependencyAssetEvent");
            m_InstanceRoot = serializedObject.FindProperty("m_InstanceRoot");
            m_AudioMixer = serializedObject.FindProperty("m_AudioMixer");
            m_SoundGroups = serializedObject.FindProperty("m_SoundGroups");

            m_SoundHelperInfo.Init(serializedObject);
            m_SoundGroupHelperInfo.Init(serializedObject);
            m_SoundAgentHelperInfo.Init(serializedObject);

            RefreshTypeNames();
        }

        private void RefreshTypeNames()
        {
            m_SoundHelperInfo.Refresh();
            m_SoundGroupHelperInfo.Refresh();
            m_SoundAgentHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
