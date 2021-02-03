//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.IO;

namespace GameFramework
{
    public static partial class Utility
    {
        public static partial class Compression
        {
            /// <summary>
            /// 压缩解压缩辅助器接口。
            /// </summary>
            public interface ICompressionHelper
            {
                /// <summary>
                /// 压缩数据。
                /// </summary>
                /// <param name="bytes">要压缩的数据的二进制流。</param>
                /// <param name="offset">要压缩的数据的二进制流的偏移。</param>
                /// <param name="length">要压缩的数据的二进制流的长度。</param>
                /// <param name="compressedStream">压缩后的数据的二进制流。</param>
                /// <returns>是否压缩数据成功。</returns>
                bool Compress(byte[] bytes, int offset, int length, Stream compressedStream);

                /// <summary>
                /// 压缩数据。
                /// </summary>
                /// <param name="stream">要压缩的数据的二进制流。</param>
                /// <param name="compressedStream">压缩后的数据的二进制流。</param>
                /// <returns>是否压缩数据成功。</returns>
                bool Compress(Stream stream, Stream compressedStream);

                /// <summary>
                /// 解压缩数据。
                /// </summary>
                /// <param name="bytes">要解压缩的数据的二进制流。</param>
                /// <param name="offset">要解压缩的数据的二进制流的偏移。</param>
                /// <param name="length">要解压缩的数据的二进制流的长度。</param>
                /// <param name="decompressedStream">解压缩后的数据的二进制流。</param>
                /// <returns>是否解压缩数据成功。</returns>
                bool Decompress(byte[] bytes, int offset, int length, Stream decompressedStream);

                /// <summary>
                /// 解压缩数据。
                /// </summary>
                /// <param name="stream">要解压缩的数据的二进制流。</param>
                /// <param name="decompressedStream">解压缩后的数据的二进制流。</param>
                /// <returns>是否解压缩数据成功。</returns>
                bool Decompress(Stream stream, Stream decompressedStream);
            }
        }
    }
}
