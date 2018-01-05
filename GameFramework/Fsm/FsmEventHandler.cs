//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Fsm
{
    /// <summary>
    /// 有限状态机事件响应函数。
    /// </summary>
    /// <typeparam name="T">有限状态机持有者类型。</typeparam>
    /// <param name="fsm">有限状态机引用。</param>
    /// <param name="sender">事件源。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void FsmEventHandler<T>(IFsm<T> fsm, object sender, object userData) where T : class;
}
