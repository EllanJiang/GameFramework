//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 解密资源回调函数。
    /// </summary>
    /// <param name="name">资源名称。</param>
    /// <param name="variant">变体名称。</param>
    /// <param name="loadType">资源加载方式。</param>
    /// <param name="length">资源大小。</param>
    /// <param name="hashCode">资源哈希值。</param>
    /// <param name="storageInReadOnly">资源是否在只读区。</param>
    /// <param name="bytes">待解密的资源二进制流。</param>
    /// <returns>解密后的资源二进制流。</returns>
    public delegate byte[] DecryptResourceCallback(string name, string variant, int loadType, int length, int hashCode, bool storageInReadOnly, byte[] bytes);
}
