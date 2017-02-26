//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public partial class DebuggerComponent
    {
        private sealed class QualitySettingsWindow : ScrollableDebuggerWindowBase
        {
            private bool m_ApplyExpensiveChanges = false;

            protected override void OnDrawScrollableWindow()
            {
                GUILayout.Label("<b>Quality Settings</b>");
                GUILayout.BeginVertical("box");
                {
                    int currentQualityLevel = QualitySettings.GetQualityLevel();

                    GUILayout.Label(string.Format("Quality Level: {0}", QualitySettings.names[currentQualityLevel]), GUILayout.Height(30f));
                    m_ApplyExpensiveChanges = GUILayout.Toggle(m_ApplyExpensiveChanges, "Apply expensive changes on quality level change.");

                    GUILayout.Space(10f);

                    int newQualityLevel = GUILayout.SelectionGrid(currentQualityLevel, QualitySettings.names, 1, "toggle");
                    if (newQualityLevel != currentQualityLevel)
                    {
                        QualitySettings.SetQualityLevel(newQualityLevel, m_ApplyExpensiveChanges);
                    }
                }
                GUILayout.EndVertical();
            }
        }
    }
}
