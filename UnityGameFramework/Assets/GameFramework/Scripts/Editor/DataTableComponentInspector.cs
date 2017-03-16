//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.DataTable;
using UnityEditor;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(DataTableComponent))]
    internal sealed class DataTableComponentInspector : GameFrameworkInspector
    {
        private SerializedProperty m_EnableLoadDataTableSuccessEvent = null;
        private SerializedProperty m_EnableLoadDataTableFailureEvent = null;
        private SerializedProperty m_EnableLoadDataTableUpdateEvent = null;
        private SerializedProperty m_EnableLoadDataTableDependencyAssetEvent = null;

        private HelperInfo<DataTableHelperBase> m_DataTableHelperInfo = new HelperInfo<DataTableHelperBase>("DataTable");

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();

            DataTableComponent t = (DataTableComponent)target;

            EditorGUILayout.PropertyField(m_EnableLoadDataTableSuccessEvent);
            EditorGUILayout.PropertyField(m_EnableLoadDataTableFailureEvent);
            EditorGUILayout.PropertyField(m_EnableLoadDataTableUpdateEvent);
            EditorGUILayout.PropertyField(m_EnableLoadDataTableDependencyAssetEvent);

            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
            {
                m_DataTableHelperInfo.Draw();
            }
            EditorGUI.EndDisabledGroup();

            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField("Data Table Count", t.Count.ToString());

                DataTableBase[] dataTables = t.GetAllDataTables();
                foreach (DataTableBase dataTable in dataTables)
                {
                    DrawDataTable(dataTable);
                }
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
            m_EnableLoadDataTableSuccessEvent = serializedObject.FindProperty("m_EnableLoadDataTableSuccessEvent");
            m_EnableLoadDataTableFailureEvent = serializedObject.FindProperty("m_EnableLoadDataTableFailureEvent");
            m_EnableLoadDataTableUpdateEvent = serializedObject.FindProperty("m_EnableLoadDataTableUpdateEvent");
            m_EnableLoadDataTableDependencyAssetEvent = serializedObject.FindProperty("m_EnableLoadDataTableDependencyAssetEvent");

            m_DataTableHelperInfo.Init(serializedObject);

            RefreshTypeNames();
        }

        private void DrawDataTable(DataTableBase dataTable)
        {
            EditorGUILayout.LabelField(Utility.Text.GetFullName(dataTable.Type, dataTable.Name), string.Format("{0} Rows", dataTable.Count.ToString()));
        }

        private void RefreshTypeNames()
        {
            m_DataTableHelperInfo.Refresh();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
