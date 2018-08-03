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
        /// <summary>
        /// 压缩解压缩相关的实用函数。
        /// </summary>
        public static partial class Zip
        {
            private static IZipHelper s_ZipHelper = null;

            /// <summary>
            /// 设置压缩解压缩辅助器。
            /// </summary>
            /// <param name="zipHelper">要设置的压缩解压缩辅助器。</param>
            public static void SetZipHelper(IZipHelper zipHelper)
            {
                s_ZipHelper = zipHelper;
            }

            /// <summary>
            /// 压缩数据。
            /// </summary>
            /// <param name="bytes">要压缩的数据。</param>
            /// <returns>压缩后的数据。</returns>
            public static byte[] Compress(byte[] bytes)
            {
                if (s_ZipHelper == null)
                {
                    throw new GameFrameworkException("Zip helper is invalid.");
                }

                return s_ZipHelper.Compress(bytes);
            }

            /// <summary>
            /// 解压缩数据。
            /// </summary>
            /// <param name="bytes">要解压缩的数据。</param>
            /// <returns>解压缩后的数据。</returns>
            public static byte[] Decompress(byte[] bytes)
            {
                if (s_ZipHelper == null)
                {
                    throw new GameFrameworkException("Zip helper is invalid.");
                }

                return s_ZipHelper.Decompress(bytes);
            }
        }
    }
}
