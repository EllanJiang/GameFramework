//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 加载资源时加载依赖资源回调函数。
    /// </summary>
    /// <param name="assetName">要加载的资源名称。</param>
    /// <param name="dependencyAssetName">被加载的依赖资源名称。</param>
    /// <param name="loadedCount">当前已加载依赖资源数量。</param>
    /// <param name="totalCount">总共加载依赖资源数量。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void LoadAssetDependencyAssetCallback(string assetName, string dependencyAssetName, int loadedCount, int totalCount, object userData);
}
