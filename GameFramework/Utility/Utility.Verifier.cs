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
            /// <summary>
            /// 计算二进制流的 CRC32。
            /// </summary>
            /// <param name="bytes">指定的二进制流。</param>
            /// <returns>计算后的 CRC32。</returns>
            public static byte[] GetCrc32(byte[] bytes)
            {
                Crc32 algorithm = new Crc32();
                byte[] result = algorithm.ComputeHash(bytes);
                algorithm.Clear();
                return result;
            }

            /// <summary>
            /// 计算指定文件的 CRC32。
            /// </summary>
            /// <param name="fileName">指定文件的完全限定名称。</param>
            /// <returns>计算后的 CRC32。</returns>
            public static byte[] GetCrc32(string fileName)
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    Crc32 algorithm = new Crc32();
                    byte[] result = algorithm.ComputeHash(fileStream);
                    algorithm.Clear();
                    return result;
                }
            }

            /// <summary>
            /// 计算二进制流的 MD5。
            /// </summary>
            /// <param name="bytes">指定的二进制流。</param>
            /// <returns>计算后的 MD5。</returns>
            public static byte[] GetMD5(byte[] bytes)
            {
                MD5 algorithm = new MD5CryptoServiceProvider();
                byte[] result = algorithm.ComputeHash(bytes);
                algorithm.Clear();
                return result;
            }

            /// <summary>
            /// 计算文件的 MD5。
            /// </summary>
            /// <param name="fileName">指定文件的完全限定名称。</param>
            /// <returns>计算后的 MD5。</returns>
            public static byte[] GetMD5(string fileName)
            {
                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    MD5 algorithm = new MD5CryptoServiceProvider();
                    byte[] result = algorithm.ComputeHash(fileStream);
                    algorithm.Clear();
                    return result;
                }
            }
        }
    }
}
