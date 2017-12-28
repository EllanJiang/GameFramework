//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Debugger
{
    /// <summary>
    /// 调试窗口接口。
    /// </summary>
    public interface IDebuggerWindow
    {
        /// <summary>
        /// 初始化调试窗口。
        /// </summary>
        /// <param name="args">初始化调试窗口参数。</param>
        void Initialize(params object[] args);

        /// <summary>
        /// 关闭调试窗口。
        /// </summary>
        void Shutdown();

        /// <summary>
        /// 进入调试窗口。
        /// </summary>
        void OnEnter();

        /// <summary>
        /// 离开调试窗口。
        /// </summary>
        void OnLeave();

        /// <summary>
        /// 调试窗口轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        void OnUpdate(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 调试窗口绘制。
        /// </summary>
        void OnDraw();
    }
}
