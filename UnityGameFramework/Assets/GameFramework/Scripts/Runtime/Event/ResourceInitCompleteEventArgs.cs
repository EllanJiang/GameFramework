//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Event;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 资源初始化完成事件。
    /// </summary>
    public sealed class ResourceInitCompleteEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化资源初始化完成事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public ResourceInitCompleteEventArgs(GameFramework.Resource.ResourceInitCompleteEventArgs e)
        {

        }

        /// <summary>
        /// 获取资源初始化完成事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.ResourceInitComplete;
            }
        }
    }
}
