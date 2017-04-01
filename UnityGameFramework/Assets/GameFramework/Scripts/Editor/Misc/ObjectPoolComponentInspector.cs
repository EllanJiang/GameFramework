//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.ObjectPool;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(ObjectPoolComponent))]
    internal sealed class ObjectPoolComponentInspector : GameFrameworkInspector
    {
        private HashSet<string> m_OpenedItems = new HashSet<string>();

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            ObjectPoolComponent t = (ObjectPoolComponent)target;

            EditorGUILayout.LabelField("Object Pool Count", t.Count.ToString());

            ObjectPoolBase[] objectPools = t.GetAllObjectPools(true);
            foreach (ObjectPoolBase objectPool in objectPools)
            {
                DrawObjectPool(objectPool);
            }

            Repaint();
        }

        private void OnEnable()
        {

        }

        private void DrawObjectPool(ObjectPoolBase objectPool)
        {
            string fullName = Utility.Text.GetFullName(objectPool.ObjectType, objectPool.Name);
            bool lastState = m_OpenedItems.Contains(fullName);
            bool currentState = EditorGUILayout.Foldout(lastState, string.IsNullOrEmpty(objectPool.Name) ? "<Unnamed>" : objectPool.Name);
            if (currentState != lastState)
            {
                if (currentState)
                {
                    m_OpenedItems.Add(fullName);
                }
                else
                {
                    m_OpenedItems.Remove(fullName);
                }
            }

            if (currentState)
            {
                EditorGUILayout.BeginVertical("box");
                {
                    EditorGUILayout.LabelField("Type", objectPool.ObjectType.FullName);
                    EditorGUILayout.LabelField("Auto Release Interval", objectPool.AutoReleaseInterval.ToString());
                    EditorGUILayout.LabelField("Capacity", string.Format("{0} / {1} / {2}", objectPool.CanReleaseCount.ToString(), objectPool.Count.ToString(), objectPool.Capacity.ToString()));
                    EditorGUILayout.LabelField("Expire Time", objectPool.ExpireTime.ToString());
                    EditorGUILayout.LabelField("Priority", objectPool.Priority.ToString());
                    ObjectInfo[] objectInfos = objectPool.GetAllObjectInfos();
                    if (objectInfos.Length > 0)
                    {
                        foreach (ObjectInfo objectInfo in objectInfos)
                        {
                            EditorGUILayout.LabelField(objectInfo.Name, string.Format("{0}, {1}, {2}, {3}", objectInfo.Locked.ToString(), objectPool.AllowMultiSpawn ? objectInfo.SpawnCount.ToString() : objectInfo.IsInUse.ToString(), objectInfo.Priority.ToString(), objectInfo.LastUseTime.ToString("yyyy-MM-dd HH:mm:ss")));
                        }
                    }
                    else
                    {
                        GUILayout.Label("Object Pool is Empty ...");
                    }
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.Separator();
            }
        }
    }
}
