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
    [CustomEditor(typeof(DebuggerComponent))]
    internal sealed class DebuggerComponentInspector : GameFrameworkInspector
    {
        private SerializedProperty m_Skin = null;
        private SerializedProperty m_ShowFullWindow = null;
        private SerializedProperty m_ConsoleWindow = null;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            EditorGUILayout.PropertyField(m_Skin);
            EditorGUILayout.PropertyField(m_ShowFullWindow);
            EditorGUILayout.PropertyField(m_ConsoleWindow, true);

            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable()
        {
            m_Skin = serializedObject.FindProperty("m_Skin");
            m_ShowFullWindow = serializedObject.FindProperty("m_ShowFullWindow");
            m_ConsoleWindow = serializedObject.FindProperty("m_ConsoleWindow");
        }
    }
}
