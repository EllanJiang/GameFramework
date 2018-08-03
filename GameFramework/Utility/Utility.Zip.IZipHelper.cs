//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework
{
    public static partial class Utility
    {
        public static partial class Zip
        {
            /// <summary>
            /// 压缩解压缩辅助器接口。
            /// </summary>
            public interface IZipHelper
            {
                /// <summary>
                /// 压缩数据。
                /// </summary>
                /// <param name="bytes">要压缩的数据。</param>
                /// <returns>压缩后的数据。</returns>
                byte[] Compress(byte[] bytes);

                /// <summary>
                /// 解压缩数据。
                /// </summary>
                /// <param name="bytes">要解压缩的数据。</param>
                /// <returns>解压缩后的数据。</returns>
                byte[] Decompress(byte[] bytes);
            }
        }
    }
}
