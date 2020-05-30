//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 使用可更新模式并检查资源完成时的回调函数。
    /// </summary>
    /// <param name="removedCount">已移除的资源数量。</param>
    /// <param name="updateCount">可更新的资源数量。</param>
    /// <param name="updateTotalLength">可更新的资源总大小。</param>
    /// <param name="updateTotalZipLength">可更新的压缩后总大小。</param>
    public delegate void CheckResourcesCompleteCallback(int removedCount, int updateCount, long updateTotalLength, long updateTotalZipLength);
}
