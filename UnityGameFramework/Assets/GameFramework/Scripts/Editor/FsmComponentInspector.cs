//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Fsm;
using UnityEditor;
using UnityGameFramework.Runtime;

namespace UnityGameFramework.Editor
{
    [CustomEditor(typeof(FsmComponent))]
    internal sealed class FsmComponentInspector : GameFrameworkInspector
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Available during runtime only.", MessageType.Info);
                return;
            }

            FsmComponent t = (FsmComponent)target;

            EditorGUILayout.LabelField("FSM Count", t.Count.ToString());

            FsmBase[] fsms = t.GetAllFsms();
            foreach (FsmBase fsm in fsms)
            {
                DrawFsm(fsm);
            }

            Repaint();
        }

        private void OnEnable()
        {

        }

        private void DrawFsm(FsmBase fsm)
        {
            EditorGUILayout.LabelField(Utility.Text.GetFullName(fsm.OwnerType, fsm.Name), fsm.IsRunning ? string.Format("{0}, {1} s", fsm.CurrentStateName, fsm.CurrentStateTime.ToString("F1")) : (fsm.IsDestroyed ? "Destroyed" : "Not Running"));
        }
    }
}
