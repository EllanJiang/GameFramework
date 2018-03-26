//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Debugger
{
    /// <summary>
    /// 调试管理器接口。
    /// </summary>
    public interface IDebuggerManager
    {
        /// <summary>
        /// 获取或设置调试窗口是否激活。
        /// </summary>
        bool ActiveWindow
        {
            get;
            set;
        }

        /// <summary>
        /// 调试窗口根节点。
        /// </summary>
        IDebuggerWindowGroup DebuggerWindowRoot
        {
            get;
        }

        /// <summary>
        /// 注册调试窗口。
        /// </summary>
        /// <param name="path">调试窗口路径。</param>
        /// <param name="debuggerWindow">要注册的调试窗口。</param>
        /// <param name="args">初始化调试窗口参数。</param>
        void RegisterDebuggerWindow(string path, IDebuggerWindow debuggerWindow, params object[] args);

        /// <summary>
        /// 获取调试窗口。
        /// </summary>
        /// <param name="path">调试窗口路径。</param>
        /// <returns>要获取的调试窗口。</returns>
        IDebuggerWindow GetDebuggerWindow(string path);

        /// <summary>
        /// 选中调试窗口。
        /// </summary>
        /// <param name="path">调试窗口路径。</param>
        /// <returns>是否成功选中调试窗口。</returns>
        bool SelectDebuggerWindow(string path);
    }
}
