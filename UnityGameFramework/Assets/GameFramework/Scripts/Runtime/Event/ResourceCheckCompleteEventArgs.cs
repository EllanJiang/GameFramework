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
    /// 资源检查完成事件。
    /// </summary>
    public sealed class ResourceCheckCompleteEventArgs : GameEventArgs
    {
        /// <summary>
        /// 初始化资源检查完成事件的新实例。
        /// </summary>
        /// <param name="e">内部事件。</param>
        public ResourceCheckCompleteEventArgs(GameFramework.Resource.ResourceCheckCompleteEventArgs e)
        {
            RemovedCount = e.RemovedCount;
            UpdateCount = e.UpdateCount;
            UpdateTotalLength = e.UpdateTotalLength;
            UpdateTotalZipLength = e.UpdateTotalZipLength;
        }

        /// <summary>
        /// 获取资源检查完成事件编号。
        /// </summary>
        public override int Id
        {
            get
            {
                return (int)EventId.ResourceCheckComplete;
            }
        }

        /// <summary>
        /// 获取已移除的资源数量。
        /// </summary>
        public int RemovedCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取要更新的资源数量。
        /// </summary>
        public int UpdateCount
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取要更新的资源总大小。
        /// </summary>
        public int UpdateTotalLength
        {
            get;
            private set;
        }

        /// <summary>
        /// 获取要更新的压缩包总大小。
        /// </summary>
        public int UpdateTotalZipLength
        {
            get;
            private set;
        }
    }
}
