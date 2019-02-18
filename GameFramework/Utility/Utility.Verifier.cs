//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.IO;
using System.Security.Cryptography;

namespace GameFramework
{
    public static partial class Utility
    {
        /// <summary>
        /// 校验相关的实用函数。
        /// </summary>
        public static partial class Verifier
        {
            private static readonly Crc32 s_AlgorithmCrc32 = new Crc32();
            private static readonly MD5 s_AlgorithmMD5 = new MD5CryptoServiceProvider();

            /// <summary>
            /// 计算二进制流的 CRC32。
            /// </summary>
            /// <param name="bytes">指定的二进制流。</param>
            /// <returns>计算后的 CRC32。</returns>
            public static byte[] GetCrc32(byte[] bytes)
            {
                return s_AlgorithmCrc32.ComputeHash(bytes);
            }

            /// <summary>
            /// 计算二进制流的 CRC32。
            /// </summary>
            /// <param name="bytes">指定的二进制流。</param>
            /// <param name="offset">二进制流的偏移。</param>
            /// <param name="length">二进制流的长度。</param>
            /// <returns>计算后的 CRC32。</returns>
            public static byte[] GetCrc32(byte[] bytes, int offset, int length)
            {
                return s_AlgorithmCrc32.ComputeHash(bytes, offset, length);
            }

            /// <summary>
            /// 计算二进制流的 CRC32。
            /// </summary>
            /// <param name="stream">指定的二进制流。</param>
            /// <returns>计算后的 CRC32。</returns>
            public static byte[] GetCrc32(Stream stream)
            {
                return s_AlgorithmCrc32.ComputeHash(stream);
            }

            /// <summary>
            /// 计算二进制流的 MD5。
            /// </summary>
            /// <param name="bytes">指定的二进制流。</param>
            /// <returns>计算后的 MD5。</returns>
            public static byte[] GetMD5(byte[] bytes)
            {
                return s_AlgorithmMD5.ComputeHash(bytes);
            }

            /// <summary>
            /// 计算二进制流的 MD5。
            /// </summary>
            /// <param name="bytes">指定的二进制流。</param>
            /// <param name="offset">二进制流的偏移。</param>
            /// <param name="length">二进制流的长度。</param>
            /// <returns>计算后的 MD5。</returns>
            public static byte[] GetMD5(byte[] bytes, int offset, int length)
            {
                return s_AlgorithmMD5.ComputeHash(bytes, offset, length);
            }

            /// <summary>
            /// 计算二进制流的 MD5。
            /// </summary>
            /// <param name="stream">指定的二进制流。</param>
            /// <returns>计算后的 MD5。</returns>
            public static byte[] GetMD5(Stream stream)
            {
                return s_AlgorithmMD5.ComputeHash(stream);
            }
        }
    }
}
