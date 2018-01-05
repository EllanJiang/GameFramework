//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Debugger
{
    /// <summary>
    /// 调试窗口组接口。
    /// </summary>
    public interface IDebuggerWindowGroup : IDebuggerWindow
    {
        /// <summary>
        /// 获取调试窗口数量。
        /// </summary>
        int DebuggerWindowCount
        {
            get;
        }

        /// <summary>
        /// 获取或设置当前选中的调试窗口索引。
        /// </summary>
        int SelectedIndex
        {
            get;
            set;
        }

        /// <summary>
        /// 获取当前选中的调试窗口。
        /// </summary>
        IDebuggerWindow SelectedWindow
        {
            get;
        }

        /// <summary>
        /// 获取调试组的调试窗口名称集合。
        /// </summary>
        string[] GetDebuggerWindowNames();

        /// <summary>
        /// 获取调试窗口。
        /// </summary>
        /// <param name="path">调试窗口路径。</param>
        /// <returns>要获取的调试窗口。</returns>
        IDebuggerWindow GetDebuggerWindow(string path);

        /// <summary>
        /// 注册调试窗口。
        /// </summary>
        /// <param name="path">调试窗口路径。</param>
        /// <param name="debuggerWindow">要注册的调试窗口。</param>
        void RegisterDebuggerWindow(string path, IDebuggerWindow debuggerWindow);
    }
}
