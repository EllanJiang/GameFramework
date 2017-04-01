//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(SceneComponent))]
    internal sealed class SceneComponentInspector : GameFrameworkInspector
    {
        private SerializedProperty m_EnableLoadSceneSuccessEvent = null;
        private SerializedProperty m_EnableLoadSceneFailureEvent = null;
        private SerializedProperty m_EnableLoadSceneUpdateEvent = null;
        private SerializedProperty m_EnableLoadSceneDependencyAssetEvent = null;
        private SerializedProperty m_EnableUnloadSceneSuccessEvent = null;
        private SerializedProperty m_EnableUnloadSceneFailureEvent = null;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            SceneComponent t = (SceneComponent)target;

            EditorGUILayout.PropertyField(m_EnableLoadSceneSuccessEvent);
            EditorGUILayout.PropertyField(m_EnableLoadSceneFailureEvent);
            EditorGUILayout.PropertyField(m_EnableLoadSceneUpdateEvent);
            EditorGUILayout.PropertyField(m_EnableLoadSceneDependencyAssetEvent);
            EditorGUILayout.PropertyField(m_EnableUnloadSceneSuccessEvent);
            EditorGUILayout.PropertyField(m_EnableUnloadSceneFailureEvent);

            serializedObject.ApplyModifiedProperties();

            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField("Loaded Scene Asset Names", GetSceneNameString(t.GetLoadedSceneAssetNames()));
                EditorGUILayout.LabelField("Loading Scene Asset Names", GetSceneNameString(t.GetLoadingSceneAssetNames()));
                EditorGUILayout.LabelField("Unloading Scene Asset Names", GetSceneNameString(t.GetUnloadingSceneAssetNames()));
                EditorGUILayout.ObjectField("Main Camera", t.MainCamera, typeof(Camera), true);

                Repaint();
            }
        }

        private void OnEnable()
        {
            m_EnableLoadSceneSuccessEvent = serializedObject.FindProperty("m_EnableLoadSceneSuccessEvent");
            m_EnableLoadSceneFailureEvent = serializedObject.FindProperty("m_EnableLoadSceneFailureEvent");
            m_EnableLoadSceneUpdateEvent = serializedObject.FindProperty("m_EnableLoadSceneUpdateEvent");
            m_EnableLoadSceneDependencyAssetEvent = serializedObject.FindProperty("m_EnableLoadSceneDependencyAssetEvent");
            m_EnableUnloadSceneSuccessEvent = serializedObject.FindProperty("m_EnableUnloadSceneSuccessEvent");
            m_EnableUnloadSceneFailureEvent = serializedObject.FindProperty("m_EnableUnloadSceneFailureEvent");
        }

        private string GetSceneNameString(string[] sceneAssetNames)
        {
            if (sceneAssetNames == null || sceneAssetNames.Length <= 0)
            {
                return "<Empty>";
            }

            string sceneNameString = string.Empty;
            foreach (string sceneAssetName in sceneAssetNames)
            {
                if (!string.IsNullOrEmpty(sceneNameString))
                {
                    sceneNameString += ", ";
                }

                sceneNameString += SceneComponent.GetSceneName(sceneAssetName);
            }

            return sceneNameString;
        }
    }
}
