//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework
{
    public static partial class Utility
    {
        /// <summary>
        /// 可空类型相关的实用函数。
        /// </summary>
        public static class Nullable
        {
            /// <summary>
            /// 获取对象是否是可空类型。
            /// </summary>
            /// <typeparam name="T">对象类型。</typeparam>
            /// <param name="t">对象。</param>
            /// <returns>对象是否是可空类型。</returns>
            public static bool IsNullable<T>(T t) { return false; }

            /// <summary>
            /// 获取对象是否是可空类型。
            /// </summary>
            /// <typeparam name="T">对象类型。</typeparam>
            /// <param name="t">对象。</param>
            /// <returns>对象是否是可空类型。</returns>
            public static bool IsNullable<T>(T? t) where T : struct { return true; }
        }
    }
}
