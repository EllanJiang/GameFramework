//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 解密资源回调函数。
    /// </summary>
    /// <param name="bytes">要解密的资源二进制流。</param>
    /// <param name="startIndex">解密二进制流的起始位置。</param>
    /// <param name="count">解密二进制流的长度。</param>
    /// <param name="name">资源名称。</param>
    /// <param name="variant">变体名称。</param>
    /// <param name="extension">扩展名称。</param>
    /// <param name="storageInReadOnly">资源是否在只读区。</param>
    /// <param name="fileSystem">文件系统名称。</param>
    /// <param name="loadType">资源加载方式。</param>
    /// <param name="length">资源大小。</param>
    /// <param name="hashCode">资源哈希值。</param>
    public delegate void DecryptResourceCallback(byte[] bytes, int startIndex, int count, string name, string variant, string extension, bool storageInReadOnly, string fileSystem, byte loadType, int length, int hashCode);
}
