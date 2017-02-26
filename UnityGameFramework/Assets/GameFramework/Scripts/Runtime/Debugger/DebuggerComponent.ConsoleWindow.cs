//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Debugger;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    public partial class DebuggerComponent
    {
        [Serializable]
        private sealed partial class ConsoleWindow : IDebuggerWindow
        {
            private SettingComponent m_SettingComponent = null;
            private LinkedList<LogNode> m_Logs = new LinkedList<LogNode>();
            private Vector2 m_LogScrollPosition = Vector2.zero;
            private Vector2 m_StackScrollPosition = Vector2.zero;
            private int m_InfoCount = 0;
            private int m_WarningCount = 0;
            private int m_ErrorCount = 0;
            private int m_FatalCount = 0;
            private LinkedListNode<LogNode> m_SelectedNode = null;
            private bool m_LastLockScroll = true;
            private bool m_LastInfoFilter = true;
            private bool m_LastWarningFilter = true;
            private bool m_LastErrorFilter = true;
            private bool m_LastFatalFilter = true;

            [SerializeField]
            private bool m_LockScroll = true;

            [SerializeField]
            private int m_MaxLine = 300;

            [SerializeField]
            private string m_DateTimeFormat = "[HH:mm:ss.fff] ";

            [SerializeField]
            private bool m_InfoFilter = true;

            [SerializeField]
            private bool m_WarningFilter = true;

            [SerializeField]
            private bool m_ErrorFilter = true;

            [SerializeField]
            private bool m_FatalFilter = true;

            [SerializeField]
            private Color32 m_InfoColor = Color.white;

            [SerializeField]
            private Color32 m_WarningColor = Color.yellow;

            [SerializeField]
            private Color32 m_ErrorColor = Color.red;

            [SerializeField]
            private Color32 m_FatalColor = new Color(0.7f, 0.2f, 0.2f);

            public bool LockScroll
            {
                get
                {
                    return m_LockScroll;
                }
                set
                {
                    m_LockScroll = value;
                }
            }

            public int MaxLine
            {
                get
                {
                    return m_MaxLine;
                }
                set
                {
                    m_MaxLine = value;
                }
            }

            public string DateTimeFormat
            {
                get
                {
                    return m_DateTimeFormat;
                }
                set
                {
                    m_DateTimeFormat = value ?? string.Empty;
                }
            }

            public bool InfoFilter
            {
                get
                {
                    return m_InfoFilter;
                }
                set
                {
                    m_InfoFilter = value;
                }
            }

            public bool WarningFilter
            {
                get
                {
                    return m_WarningFilter;
                }
                set
                {
                    m_WarningFilter = value;
                }
            }

            public bool ErrorFilter
            {
                get
                {
                    return m_ErrorFilter;
                }
                set
                {
                    m_ErrorFilter = value;
                }
            }

            public bool FatalFilter
            {
                get
                {
                    return m_FatalFilter;
                }
                set
                {
                    m_FatalFilter = value;
                }
            }

            public int InfoCount
            {
                get
                {
                    return m_InfoCount;
                }
            }

            public int WarningCount
            {
                get
                {
                    return m_WarningCount;
                }
            }

            public int ErrorCount
            {
                get
                {
                    return m_ErrorCount;
                }
            }

            public int FatalCount
            {
                get
                {
                    return m_FatalCount;
                }
            }

            public Color32 InfoColor
            {
                get
                {
                    return m_InfoColor;
                }
                set
                {
                    m_InfoColor = value;
                }
            }

            public Color32 WarningColor
            {
                get
                {
                    return m_WarningColor;
                }
                set
                {
                    m_WarningColor = value;
                }
            }

            public Color32 ErrorColor
            {
                get
                {
                    return m_ErrorColor;
                }
                set
                {
                    m_ErrorColor = value;
                }
            }

            public Color32 FatalColor
            {
                get
                {
                    return m_FatalColor;
                }
                set
                {
                    m_FatalColor = value;
                }
            }

            public void Initialize(params object[] args)
            {
                m_SettingComponent = GameEntry.GetComponent<SettingComponent>();
                if (m_SettingComponent == null)
                {
                    Log.Fatal("Setting component is invalid.");
                    return;
                }

                Application.logMessageReceived += OnLogMessageReceived;
                m_LockScroll = m_LastLockScroll = m_SettingComponent.GetBool("Debugger.Console.LockScroll", true);
                m_InfoFilter = m_LastInfoFilter = m_SettingComponent.GetBool("Debugger.Console.InfoFilter", true);
                m_WarningFilter = m_LastWarningFilter = m_SettingComponent.GetBool("Debugger.Console.WarningFilter", true);
                m_ErrorFilter = m_LastErrorFilter = m_SettingComponent.GetBool("Debugger.Console.ErrorFilter", true);
                m_FatalFilter = m_LastFatalFilter = m_SettingComponent.GetBool("Debugger.Console.FatalFilter", true);
            }

            public void Shutdown()
            {
                Application.logMessageReceived -= OnLogMessageReceived;
                Clear();
            }

            public void OnEnter()
            {

            }

            public void OnLeave()
            {

            }

            public void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
                if (m_LastLockScroll != m_LockScroll)
                {
                    m_LastLockScroll = m_LockScroll;
                    m_SettingComponent.SetBool("Debugger.Console.LockScroll", m_LockScroll);
                }

                if (m_LastInfoFilter != m_InfoFilter)
                {
                    m_LastInfoFilter = m_InfoFilter;
                    m_SettingComponent.SetBool("Debugger.Console.InfoFilter", m_InfoFilter);
                }

                if (m_LastWarningFilter != m_WarningFilter)
                {
                    m_LastWarningFilter = m_WarningFilter;
                    m_SettingComponent.SetBool("Debugger.Console.WarningFilter", m_WarningFilter);
                }

                if (m_LastErrorFilter != m_ErrorFilter)
                {
                    m_LastErrorFilter = m_ErrorFilter;
                    m_SettingComponent.SetBool("Debugger.Console.ErrorFilter", m_ErrorFilter);
                }

                if (m_LastFatalFilter != m_FatalFilter)
                {
                    m_LastFatalFilter = m_FatalFilter;
                    m_SettingComponent.SetBool("Debugger.Console.FatalFilter", m_FatalFilter);
                }
            }

            public void OnDraw()
            {
                RefreshCount();

                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Clear All", GUILayout.Width(100f)))
                    {
                        Clear();
                    }
                    m_LockScroll = GUILayout.Toggle(m_LockScroll, "Lock Scroll", GUILayout.Width(90f));
                    GUILayout.FlexibleSpace();
                    m_InfoFilter = GUILayout.Toggle(m_InfoFilter, string.Format("Info ({0})", m_InfoCount.ToString()), GUILayout.Width(90f));
                    m_WarningFilter = GUILayout.Toggle(m_WarningFilter, string.Format("Warning ({0})", m_WarningCount.ToString()), GUILayout.Width(90f));
                    m_ErrorFilter = GUILayout.Toggle(m_ErrorFilter, string.Format("Error ({0})", m_ErrorCount.ToString()), GUILayout.Width(90f));
                    m_FatalFilter = GUILayout.Toggle(m_FatalFilter, string.Format("Fatal ({0})", m_FatalCount.ToString()), GUILayout.Width(90f));
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginVertical("box");
                {
                    if (m_LockScroll)
                    {
                        m_LogScrollPosition.y = float.MaxValue;
                    }

                    m_LogScrollPosition = GUILayout.BeginScrollView(m_LogScrollPosition);
                    {
                        bool selected = false;
                        for (LinkedListNode<LogNode> i = m_Logs.First; i != null; i = i.Next)
                        {
                            switch (i.Value.LogType)
                            {
                                case LogType.Log:
                                    if (!m_InfoFilter)
                                    {
                                        continue;
                                    }
                                    break;
                                case LogType.Warning:
                                    if (!m_WarningFilter)
                                    {
                                        continue;
                                    }
                                    break;
                                case LogType.Error:
                                    if (!m_ErrorFilter)
                                    {
                                        continue;
                                    }
                                    break;
                                case LogType.Exception:
                                    if (!m_FatalFilter)
                                    {
                                        continue;
                                    }
                                    break;
                            }
                            if (GUILayout.Toggle(m_SelectedNode == i, GetLogString(i.Value)))
                            {
                                selected = true;
                                if (m_SelectedNode != i)
                                {
                                    m_SelectedNode = i;
                                    m_StackScrollPosition = Vector2.zero;
                                }
                            }
                        }
                        if (!selected)
                        {
                            m_SelectedNode = null;
                        }
                    }
                    GUILayout.EndScrollView();
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical("box");
                {
                    m_StackScrollPosition = GUILayout.BeginScrollView(m_StackScrollPosition, GUILayout.Height(100f));
                    {
                        if (m_SelectedNode != null)
                        {
                            Color32 color = GetLogStringColor(m_SelectedNode.Value.LogType);
                            GUILayout.Label(string.Format("<color=#{0}{1}{2}{3}><b>{4}</b></color>", color.r.ToString("x2"), color.g.ToString("x2"), color.b.ToString("x2"), color.a.ToString("x2"), m_SelectedNode.Value.LogMessage));
                            GUILayout.Label(m_SelectedNode.Value.StackTrack);
                        }
                        GUILayout.EndScrollView();
                    }
                }
                GUILayout.EndVertical();
            }

            private void Clear()
            {
                m_Logs.Clear();
            }

            public void RefreshCount()
            {
                m_InfoCount = 0;
                m_WarningCount = 0;
                m_ErrorCount = 0;
                m_FatalCount = 0;
                for (LinkedListNode<LogNode> i = m_Logs.First; i != null; i = i.Next)
                {
                    switch (i.Value.LogType)
                    {
                        case LogType.Log:
                            m_InfoCount++;
                            break;
                        case LogType.Warning:
                            m_WarningCount++;
                            break;
                        case LogType.Error:
                            m_ErrorCount++;
                            break;
                        case LogType.Exception:
                            m_FatalCount++;
                            break;
                    }
                }
            }

            private void OnLogMessageReceived(string logMessage, string stackTrace, LogType logType)
            {
                if (logType == LogType.Assert)
                {
                    logType = LogType.Error;
                }

                m_Logs.AddLast(new LogNode(logType, logMessage, stackTrace));
                while (m_Logs.Count > m_MaxLine)
                {
                    m_Logs.RemoveFirst();
                }
            }

            private string GetLogString(LogNode logNode)
            {
                Color32 color = GetLogStringColor(logNode.LogType);
                return string.Format("<color=#{0}{1}{2}{3}>{4}{5}</color>",
                    color.r.ToString("x2"), color.g.ToString("x2"), color.b.ToString("x2"), color.a.ToString("x2"),
                    logNode.LogTime.ToString(m_DateTimeFormat), logNode.LogMessage);
            }

            internal Color32 GetLogStringColor(LogType logType)
            {
                Color32 color = Color.white;
                switch (logType)
                {
                    case LogType.Log:
                        color = m_InfoColor;
                        break;
                    case LogType.Warning:
                        color = m_WarningColor;
                        break;
                    case LogType.Error:
                        color = m_ErrorColor;
                        break;
                    case LogType.Exception:
                        color = m_FatalColor;
                        break;
                }

                return color;
            }
        }
    }
}
