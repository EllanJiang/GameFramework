//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 资源检查完成事件。
    /// </summary>
    public sealed class ResourceCheckCompleteEventArgs : GameFrameworkEventArgs
    {
        /// <summary>
        /// 初始化资源检查完成事件的新实例。
        /// </summary>
        /// <param name="removedCount">已移除的资源数量。</param>
        /// <param name="updateCount">要更新的资源数量。</param>
        /// <param name="updateTotalLength">要更新的资源总大小。</param>
        /// <param name="updateTotalZipLength">要更新的压缩包总大小。</param>
        public ResourceCheckCompleteEventArgs(int removedCount, int updateCount, int updateTotalLength, int updateTotalZipLength)
        {
            RemovedCount = removedCount;
            UpdateCount = updateCount;
            UpdateTotalLength = updateTotalLength;
            UpdateTotalZipLength = updateTotalZipLength;
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
