//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GameFramework
{
    public static partial class Utility
    {
        /// <summary>
        /// 校验相关的实用函数。
        /// </summary>
        public static partial class Verifier
        {
            private static readonly byte[] Zero = new byte[] { 0, 0, 0, 0 };

            /// <summary>
            /// 计算二进制流的 CRC32。
            /// </summary>
            /// <param name="bytes">指定的二进制流。</param>
            /// <returns>计算后的 CRC32。</returns>
            public static byte[] GetCrc32(byte[] bytes)
            {
                if (bytes == null)
                {
                    return Zero;
                }

                using (MemoryStream memoryStream = new MemoryStream(bytes))
                {
                    Crc32 calculator = new Crc32();
                    byte[] result = calculator.ComputeHash(memoryStream);
                    calculator.Clear();
                    return result;
                }
            }

            /// <summary>
            /// 计算指定文件的 CRC32。
            /// </summary>
            /// <param name="fileName">指定文件的完全限定名称。</param>
            /// <returns>计算后的 CRC32。</returns>
            public static byte[] GetCrc32(string fileName)
            {
                if (!File.Exists(fileName))
                {
                    return Zero;
                }

                using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
                {
                    Crc32 calculator = new Crc32();
                    byte[] result = calculator.ComputeHash(fileStream);
                    calculator.Clear();
                    return result;
                }
            }

            /// <summary>
            /// 计算二进制流的 MD5。
            /// </summary>
            /// <param name="bytes">指定的二进制流。</param>
            /// <returns>计算后的 MD5。</returns>
            public static string GetMD5(byte[] bytes)
            {
                MD5 alg = new MD5CryptoServiceProvider();
                byte[] data = alg.ComputeHash(bytes);
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    stringBuilder.Append(data[i].ToString("x2"));
                }

                return stringBuilder.ToString();
            }
        }
    }
}
