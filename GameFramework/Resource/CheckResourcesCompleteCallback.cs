//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 使用可更新模式并检查资源完成的回调函数。
    /// </summary>
    /// <param name="needUpdateResources">是否需要进行资源更新。</param>
    /// <param name="removedCount">已移除的资源数量。</param>
    /// <param name="updateCount">要更新的资源数量。</param>
    /// <param name="updateTotalLength">要更新的资源总大小。</param>
    /// <param name="updateTotalZipLength">要更新的压缩包总大小。</param>
    public delegate void CheckResourcesCompleteCallback(bool needUpdateResources, int removedCount, int updateCount, int updateTotalLength, int updateTotalZipLength);
}
