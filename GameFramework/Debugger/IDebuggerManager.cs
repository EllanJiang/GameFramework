//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Debugger
{
    /// <summary>
    /// 调试器管理器接口。
    /// </summary>
    public interface IDebuggerManager
    {
        /// <summary>
        /// 获取或设置调试器窗口是否激活。
        /// </summary>
        bool ActiveWindow
        {
            get;
            set;
        }

        /// <summary>
        /// 调试器窗口根结点。
        /// </summary>
        IDebuggerWindowGroup DebuggerWindowRoot
        {
            get;
        }

        /// <summary>
        /// 注册调试器窗口。
        /// </summary>
        /// <param name="path">调试器窗口路径。</param>
        /// <param name="debuggerWindow">要注册的调试器窗口。</param>
        /// <param name="args">初始化调试器窗口参数。</param>
        void RegisterDebuggerWindow(string path, IDebuggerWindow debuggerWindow, params object[] args);

        /// <summary>
        /// 获取调试器窗口。
        /// </summary>
        /// <param name="path">调试器窗口路径。</param>
        /// <returns>要获取的调试器窗口。</returns>
        IDebuggerWindow GetDebuggerWindow(string path);

        /// <summary>
        /// 选中调试器窗口。
        /// </summary>
        /// <param name="path">调试器窗口路径。</param>
        /// <returns>是否成功选中调试器窗口。</returns>
        bool SelectDebuggerWindow(string path);
    }
}
