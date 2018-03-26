//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Collections.Generic;

namespace GameFramework.Debugger
{
    internal partial class DebuggerManager
    {
        /// <summary>
        /// 调试窗口组。
        /// </summary>
        private sealed class DebuggerWindowGroup : IDebuggerWindowGroup
        {
            private readonly List<KeyValuePair<string, IDebuggerWindow>> m_DebuggerWindows;
            private int m_SelectedIndex;
            private string[] m_DebuggerWindowNames;

            public DebuggerWindowGroup()
            {
                m_DebuggerWindows = new List<KeyValuePair<string, IDebuggerWindow>>();
                m_SelectedIndex = 0;
                m_DebuggerWindowNames = null;
            }

            /// <summary>
            /// 获取调试窗口数量。
            /// </summary>
            public int DebuggerWindowCount
            {
                get
                {
                    return m_DebuggerWindows.Count;
                }
            }

            /// <summary>
            /// 获取或设置当前选中的调试窗口索引。
            /// </summary>
            public int SelectedIndex
            {
                get
                {
                    return m_SelectedIndex;
                }
                set
                {
                    m_SelectedIndex = value;
                }
            }

            /// <summary>
            /// 获取当前选中的调试窗口。
            /// </summary>
            public IDebuggerWindow SelectedWindow
            {
                get
                {
                    if (m_SelectedIndex >= m_DebuggerWindows.Count)
                    {
                        return null;
                    }

                    return m_DebuggerWindows[m_SelectedIndex].Value;
                }
            }

            /// <summary>
            /// 初始化调试组。
            /// </summary>
            /// <param name="args">初始化调试组参数。</param>
            public void Initialize(params object[] args)
            {

            }

            /// <summary>
            /// 关闭调试组。
            /// </summary>
            public void Shutdown()
            {
                foreach (KeyValuePair<string, IDebuggerWindow> debuggerWindow in m_DebuggerWindows)
                {
                    debuggerWindow.Value.Shutdown();
                }

                m_DebuggerWindows.Clear();
            }

            /// <summary>
            /// 进入调试窗口。
            /// </summary>
            public void OnEnter()
            {
                SelectedWindow.OnEnter();
            }

            /// <summary>
            /// 离开调试窗口。
            /// </summary>
            public void OnLeave()
            {
                SelectedWindow.OnLeave();
            }

            /// <summary>
            /// 调试组轮询。
            /// </summary>
            /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
            /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
            public void OnUpdate(float elapseSeconds, float realElapseSeconds)
            {
                SelectedWindow.OnUpdate(elapseSeconds, realElapseSeconds);
            }

            /// <summary>
            /// 调试窗口绘制。
            /// </summary>
            public void OnDraw()
            {

            }

            private void RefreshDebuggerWindowNames()
            {
                m_DebuggerWindowNames = new string[m_DebuggerWindows.Count];
                int index = 0;
                foreach (KeyValuePair<string, IDebuggerWindow> debuggerWindow in m_DebuggerWindows)
                {
                    m_DebuggerWindowNames[index++] = debuggerWindow.Key;
                }
            }

            /// <summary>
            /// 获取调试组的调试窗口名称集合。
            /// </summary>
            public string[] GetDebuggerWindowNames()
            {
                return m_DebuggerWindowNames;
            }

            /// <summary>
            /// 获取调试窗口。
            /// </summary>
            /// <param name="path">调试窗口路径。</param>
            /// <returns>要获取的调试窗口。</returns>
            public IDebuggerWindow GetDebuggerWindow(string path)
            {
                if (string.IsNullOrEmpty(path))
                {
                    return null;
                }

                int pos = path.IndexOf('/');
                if (pos < 0 || pos >= path.Length - 1)
                {
                    return InternalGetDebuggerWindow(path);
                }

                string debuggerWindowGroupName = path.Substring(0, pos);
                string leftPath = path.Substring(pos + 1);
                DebuggerWindowGroup debuggerWindowGroup = (DebuggerWindowGroup)InternalGetDebuggerWindow(debuggerWindowGroupName);
                if (debuggerWindowGroup == null)
                {
                    return null;
                }

                return debuggerWindowGroup.GetDebuggerWindow(leftPath);
            }

            /// <summary>
            /// 选中调试窗口。
            /// </summary>
            /// <param name="path">调试窗口路径。</param>
            /// <returns>是否成功选中调试窗口。</returns>
            public bool SelectDebuggerWindow(string path)
            {
                if (string.IsNullOrEmpty(path))
                {
                    return false;
                }

                int pos = path.IndexOf('/');
                if (pos < 0 || pos >= path.Length - 1)
                {
                    return InternalSelectDebuggerWindow(path);
                }

                string debuggerWindowGroupName = path.Substring(0, pos);
                string leftPath = path.Substring(pos + 1);
                DebuggerWindowGroup debuggerWindowGroup = (DebuggerWindowGroup)InternalGetDebuggerWindow(debuggerWindowGroupName);
                if (debuggerWindowGroup == null || !InternalSelectDebuggerWindow(debuggerWindowGroupName))
                {
                    return false;
                }

                return debuggerWindowGroup.SelectDebuggerWindow(leftPath);
            }

            /// <summary>
            /// 注册调试窗口。
            /// </summary>
            /// <param name="path">调试窗口路径。</param>
            /// <param name="debuggerWindow">要注册的调试窗口。</param>
            public void RegisterDebuggerWindow(string path, IDebuggerWindow debuggerWindow)
            {
                if (string.IsNullOrEmpty(path))
                {
                    throw new GameFrameworkException("Path is invalid.");
                }

                int pos = path.IndexOf('/');
                if (pos < 0 || pos >= path.Length - 1)
                {
                    if (InternalGetDebuggerWindow(path) != null)
                    {
                        throw new GameFrameworkException("Debugger window has been registered.");
                    }

                    m_DebuggerWindows.Add(new KeyValuePair<string, IDebuggerWindow>(path, debuggerWindow));
                    RefreshDebuggerWindowNames();
                }
                else
                {
                    string debuggerWindowGroupName = path.Substring(0, pos);
                    string leftPath = path.Substring(pos + 1);
                    DebuggerWindowGroup debuggerWindowGroup = (DebuggerWindowGroup)InternalGetDebuggerWindow(debuggerWindowGroupName);
                    if (debuggerWindowGroup == null)
                    {
                        if (InternalGetDebuggerWindow(debuggerWindowGroupName) != null)
                        {
                            throw new GameFrameworkException("Debugger window has been registered, can not create debugger window group.");
                        }

                        debuggerWindowGroup = new DebuggerWindowGroup();
                        m_DebuggerWindows.Add(new KeyValuePair<string, IDebuggerWindow>(debuggerWindowGroupName, debuggerWindowGroup));
                        RefreshDebuggerWindowNames();
                    }

                    debuggerWindowGroup.RegisterDebuggerWindow(leftPath, debuggerWindow);
                }
            }

            private IDebuggerWindow InternalGetDebuggerWindow(string name)
            {
                foreach (KeyValuePair<string, IDebuggerWindow> debuggerWindow in m_DebuggerWindows)
                {
                    if (debuggerWindow.Key == name)
                    {
                        return debuggerWindow.Value;
                    }
                }

                return null;
            }

            private bool InternalSelectDebuggerWindow(string name)
            {
                for (int i = 0; i < m_DebuggerWindows.Count; i++)
                {
                    if (m_DebuggerWindows[i].Key == name)
                    {
                        m_SelectedIndex = i;
                        return true;
                    }
                }

                return false;
            }
        }
    }
}
