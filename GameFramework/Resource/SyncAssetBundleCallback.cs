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
    /// 同步加载AssetBundle回调
    /// </summary>
    /// <param name="fullPath">要加载资源的完整路径名。</param>
    /// <param name="bytes">要加载资源的二进制流。</param>
    public delegate object SyncAssetBundleCallback(string fullPath, byte[] bytes);
}
