//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 实例化资源成功回调函数。
    /// </summary>
    /// <param name="assetName">要实例化的资源名称。</param>
    /// <param name="instance">已实例化的资源。</param>
    /// <param name="duration">实例化持续时间。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void InstantiateAssetSuccessCallback(string assetName, object instance, float duration, object userData);
}
