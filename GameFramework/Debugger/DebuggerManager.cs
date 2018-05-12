//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Debugger
{
    /// <summary>
    /// 调试管理器。
    /// </summary>
    internal sealed partial class DebuggerManager : GameFrameworkModule, IDebuggerManager
    {
        private readonly DebuggerWindowGroup m_DebuggerWindowRoot;
        private bool m_ActiveWindow;

        /// <summary>
        /// 初始化调试管理器的新实例。
        /// </summary>
        public DebuggerManager()
        {
            m_DebuggerWindowRoot = new DebuggerWindowGroup();
            m_ActiveWindow = false;
        }

        /// <summary>
        /// 获取游戏框架模块优先级。
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
        internal override int Priority
        {
            get
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取或设置调试窗口是否激活。
        /// </summary>
        public bool ActiveWindow
        {
            get
            {
                return m_ActiveWindow;
            }
            set
            {
                m_ActiveWindow = value;
            }
        }

        /// <summary>
        /// 调试窗口根节点。
        /// </summary>
        public IDebuggerWindowGroup DebuggerWindowRoot
        {
            get
            {
                return m_DebuggerWindowRoot;
            }
        }

        /// <summary>
        /// 调试管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (!m_ActiveWindow)
            {
                return;
            }

            m_DebuggerWindowRoot.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        /// <summary>
        /// 关闭并清理调试管理器。
        /// </summary>
        internal override void Shutdown()
        {
            m_ActiveWindow = false;
            m_DebuggerWindowRoot.Shutdown();
        }

        /// <summary>
        /// 注册调试窗口。
        /// </summary>
        /// <param name="path">调试窗口路径。</param>
        /// <param name="debuggerWindow">要注册的调试窗口。</param>
        /// <param name="args">初始化调试窗口参数。</param>
        public void RegisterDebuggerWindow(string path, IDebuggerWindow debuggerWindow, params object[] args)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new GameFrameworkException("Path is invalid.");
            }

            if (debuggerWindow == null)
            {
                throw new GameFrameworkException("Debugger window is invalid.");
            }

            m_DebuggerWindowRoot.RegisterDebuggerWindow(path, debuggerWindow);
            debuggerWindow.Initialize(args);
        }

        /// <summary>
        /// 获取调试窗口。
        /// </summary>
        /// <param name="path">调试窗口路径。</param>
        /// <returns>要获取的调试窗口。</returns>
        public IDebuggerWindow GetDebuggerWindow(string path)
        {
            return m_DebuggerWindowRoot.GetDebuggerWindow(path);
        }

        /// <summary>
        /// 选中调试窗口。
        /// </summary>
        /// <param name="path">调试窗口路径。</param>
        /// <returns>是否成功选中调试窗口。</returns>
        public bool SelectDebuggerWindow(string path)
        {
            return m_DebuggerWindowRoot.SelectDebuggerWindow(path);
        }
    }
}
