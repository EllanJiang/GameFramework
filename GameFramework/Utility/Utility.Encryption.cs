//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework
{
    public static partial class Utility
    {
        /// <summary>
        /// 加密解密相关的实用函数。
        /// </summary>
        public static class Encryption
        {
            internal const int QuickEncryptLength = 220;

            /// <summary>
            /// 将 bytes 使用 code 做异或运算的快速版本。
            /// </summary>
            /// <param name="bytes">原始二进制流。</param>
            /// <param name="code">异或二进制流。</param>
            /// <returns>异或后的二进制流。</returns>
            public static byte[] GetQuickXorBytes(byte[] bytes, byte[] code)
            {
                return GetXorBytes(bytes, code, QuickEncryptLength);
            }

            /// <summary>
            /// 将 bytes 使用 code 做异或运算的快速版本。此方法将复用并改写传入的 bytes 作为返回值，而不额外分配内存空间。
            /// </summary>
            /// <param name="bytes">原始及异或后的二进制流。</param>
            /// <param name="code">异或二进制流。</param>
            public static void GetQuickSelfXorBytes(byte[] bytes, byte[] code)
            {
                GetSelfXorBytes(bytes, code, QuickEncryptLength);
            }

            /// <summary>
            /// 将 bytes 使用 code 做异或运算。
            /// </summary>
            /// <param name="bytes">原始二进制流。</param>
            /// <param name="code">异或二进制流。</param>
            /// <returns>异或后的二进制流。</returns>
            public static byte[] GetXorBytes(byte[] bytes, byte[] code)
            {
                return GetXorBytes(bytes, code, -1);
            }

            /// <summary>
            /// 将 bytes 使用 code 做异或运算。此方法将复用并改写传入的 bytes 作为返回值，而不额外分配内存空间。
            /// </summary>
            /// <param name="bytes">原始及异或后的二进制流。</param>
            /// <param name="code">异或二进制流。</param>
            public static void GetSelfXorBytes(byte[] bytes, byte[] code)
            {
                GetSelfXorBytes(bytes, code, -1);
            }

            /// <summary>
            /// 将 bytes 使用 code 做异或运算。
            /// </summary>
            /// <param name="bytes">原始二进制流。</param>
            /// <param name="code">异或二进制流。</param>
            /// <param name="length">异或计算长度，若小于 0，则计算整个二进制流。</param>
            /// <returns>异或后的二进制流。</returns>
            public static byte[] GetXorBytes(byte[] bytes, byte[] code, int length)
            {
                if (bytes == null)
                {
                    return null;
                }

                int bytesLength = bytes.Length;
                byte[] results = new byte[bytesLength];
                Buffer.BlockCopy(bytes, 0, results, 0, bytesLength);
                GetSelfXorBytes(results, code, length);
                return results;
            }

            /// <summary>
            /// 将 bytes 使用 code 做异或运算。此方法将复用并改写传入的 bytes 作为返回值，而不额外分配内存空间。
            /// </summary>
            /// <param name="bytes">原始及异或后的二进制流。</param>
            /// <param name="code">异或二进制流。</param>
            /// <param name="length">异或计算长度，若小于 0，则计算整个二进制流。</param>
            public static void GetSelfXorBytes(byte[] bytes, byte[] code, int length)
            {
                if (bytes == null)
                {
                    return;
                }

                if (code == null)
                {
                    throw new GameFrameworkException("Code is invalid.");
                }

                int codeLength = code.Length;
                if (codeLength <= 0)
                {
                    throw new GameFrameworkException("Code length is invalid.");
                }

                int bytesLength = bytes.Length;
                if (length < 0 || length > bytesLength)
                {
                    length = bytesLength;
                }

                int codeIndex = 0;
                for (int i = 0; i < length; i++)
                {
                    bytes[i] ^= code[codeIndex++];
                    codeIndex %= codeLength;
                }
            }
        }
    }
}
