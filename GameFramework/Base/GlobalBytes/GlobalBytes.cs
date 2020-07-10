//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework
{
    /// <summary>
    /// 全局二进制流缓存。
    /// </summary>
    internal sealed class GlobalBytes
    {
        [ThreadStatic]
        private static byte[] s_CachedBytes = null;

        /// <summary>
        /// 获取全局二进制流缓存。
        /// </summary>
        /// <param name="ensureSize">要确保全局二进制流缓存分配内存的大小。</param>
        /// <returns>全局二进制流缓存。</returns>
        public static byte[] Get(int ensureSize)
        {
            if (ensureSize < 0)
            {
                throw new GameFrameworkException("Ensure size is invalid.");
            }

            if (s_CachedBytes == null || s_CachedBytes.Length < ensureSize)
            {
                s_CachedBytes = new byte[ensureSize];
            }

            return s_CachedBytes;
        }
    }
}
