//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using ICSharpCode.SharpZipLib.GZip;
using System.IO;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 压缩解压缩辅助器。
    /// </summary>
    internal class ZipHelper : Utility.Zip.IZipHelper
    {
        /// <summary>
        /// 压缩数据。
        /// </summary>
        /// <param name="bytes">要压缩的数据。</param>
        /// <returns>压缩后的数据。</returns>
        public byte[] Compress(byte[] bytes)
        {
            Log.Fatal("Compress is not implemented.");
            return null;
        }

        /// <summary>
        /// 解压缩数据。
        /// </summary>
        /// <param name="bytes">要解压缩的数据。</param>
        /// <returns>解压缩后的数据。</returns>
        public byte[] Decompress(byte[] bytes)
        {
            if (bytes == null || bytes.Length <= 0)
            {
                return bytes;
            }

            MemoryStream decompressedStream = null;
            MemoryStream memoryStream = null;
            try
            {
                decompressedStream = new MemoryStream();
                memoryStream = new MemoryStream(bytes);
                using (GZipInputStream gZipInputStream = new GZipInputStream(memoryStream))
                {
                    memoryStream = null;
                    int bytesRead = 0;
                    byte[] clip = new byte[0x1000];
                    while ((bytesRead = gZipInputStream.Read(clip, 0, clip.Length)) != 0)
                    {
                        decompressedStream.Write(clip, 0, bytesRead);
                    }
                }

                return decompressedStream.ToArray();
            }
            finally
            {
                if (decompressedStream != null)
                {
                    decompressedStream.Dispose();
                    decompressedStream = null;
                }

                if (memoryStream != null)
                {
                    memoryStream.Dispose();
                    memoryStream = null;
                }
            }
        }
    }
}
