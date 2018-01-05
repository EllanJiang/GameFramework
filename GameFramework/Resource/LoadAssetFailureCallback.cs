//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 加载资源失败回调函数。
    /// </summary>
    /// <param name="assetName">要加载的资源名称。</param>
    /// <param name="status">加载资源状态。</param>
    /// <param name="errorMessage">错误信息。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void LoadAssetFailureCallback(string assetName, LoadResourceStatus status, string errorMessage, object userData);
}
