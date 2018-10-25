//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// ChangeBy: Shine Wu 2018/10/16 同步加载资源
    /// <summary>
    /// 同步加载AssetObject回调
    /// </summary>
    /// <param name="resource">资源。</param>
    /// <param name="resourceAssetName">要加载资源的名称。</param>
    /// <param name="resourceSubName">要加载子资源的名称。</param>
    public delegate object SyncAssetObjectCallback(object resource, string resourceAssetName, string resourceSubName);
}
