//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework
{
    public static partial class Utility
    {
        /// <summary>
        /// JSON 相关的实用函数。
        /// </summary>
        public static partial class Json
        {
            private static IJsonHelper s_JsonHelper = null;

            /// <summary>
            /// 设置 JSON 辅助器。
            /// </summary>
            /// <param name="jsonHelper">要设置的 JSON 辅助器。</param>
            public static void SetJsonHelper(IJsonHelper jsonHelper)
            {
                s_JsonHelper = jsonHelper;
            }

            /// <summary>
            /// 将对象序列化为 JSON 字符串。
            /// </summary>
            /// <param name="obj">要序列化的对象。</param>
            /// <returns>序列化后的 JSON 字符串。</returns>
            public static string ToJson(object obj)
            {
                if (s_JsonHelper == null)
                {
                    throw new GameFrameworkException("JSON helper is invalid.");
                }

                return s_JsonHelper.ToJson(obj);
            }

            /// <summary>
            /// 将对象序列化为 JSON 流。
            /// </summary>
            /// <param name="obj">要序列化的对象。</param>
            /// <returns>序列化后的 JSON 流。</returns>
            public static byte[] ToJsonData(object obj)
            {
                return Converter.GetBytes(ToJson(obj));
            }

            /// <summary>
            /// 将 JSON 字符串反序列化为对象。
            /// </summary>
            /// <typeparam name="T">对象类型。</typeparam>
            /// <param name="json">要反序列化的 JSON 字符串。</param>
            /// <returns>反序列化后的对象。</returns>
            public static T ToObject<T>(string json)
            {
                if (s_JsonHelper == null)
                {
                    throw new GameFrameworkException("JSON helper is invalid.");
                }

                return s_JsonHelper.ToObject<T>(json);
            }

            /// <summary>
            /// 将 JSON 流反序列化为对象。
            /// </summary>
            /// <typeparam name="T">对象类型。</typeparam>
            /// <param name="jsonData">要反序列化的 JSON 流。</param>
            /// <returns>反序列化后的对象。</returns>
            public static T ToObject<T>(byte[] jsonData)
            {
                return ToObject<T>(Converter.GetString(jsonData));
            }
        }
    }
}
