//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace GameFramework
{
    public static partial class Utility
    {
        /// <summary>
        /// 字符相关的实用函数。
        /// </summary>
        public static class Text
        {
            /// <summary>
            /// 将文本按行切分。
            /// </summary>
            /// <param name="text">要切分的文本。</param>
            /// <returns>按行切分后的文本。</returns>
            public static string[] SplitToLines(string text)
            {
                List<string> texts = new List<string>();
                int position = 0;
                string rowText = null;
                while ((rowText = ReadLine(text, ref position)) != null)
                {
                    texts.Add(rowText);
                }

                return texts.ToArray();
            }

            /// <summary>
            /// 根据类型和名称获取完整名称。
            /// </summary>
            /// <typeparam name="T">类型。</typeparam>
            /// <param name="name">名称。</param>
            /// <returns>完整名称。</returns>
            public static string GetFullName<T>(string name)
            {
                return GetFullName(typeof(T), name);
            }

            /// <summary>
            /// 根据类型和名称获取完整名称。
            /// </summary>
            /// <param name="type">类型。</param>
            /// <param name="name">名称。</param>
            /// <returns>完整名称。</returns>
            public static string GetFullName(Type type, string name)
            {
                if (type == null)
                {
                    throw new GameFrameworkException("Type is invalid.");
                }

                string typeName = type.FullName;
                return string.IsNullOrEmpty(name) ? typeName : string.Format("{0}.{1}", typeName, name);
            }

            /// <summary>
            /// 获取用于编辑器显示的名称。
            /// </summary>
            /// <param name="fieldName">字段名称。</param>
            /// <returns>编辑器显示名称。</returns>
            public static string FieldNameForDisplay(string fieldName)
            {
                if (string.IsNullOrEmpty(fieldName))
                {
                    return string.Empty;
                }

                string str = Regex.Replace(fieldName, @"^m_", string.Empty);
                str = Regex.Replace(str, @"((?<=[a-z])[A-Z]|[A-Z](?=[a-z]))", @" $1").TrimStart();
                return str;
            }

            /// <summary>
            /// 读取一行文本。
            /// </summary>
            /// <param name="text">要读取的文本。</param>
            /// <param name="position">开始的位置。</param>
            /// <returns>一行文本。</returns>
            private static string ReadLine(string text, ref int position)
            {
                if (text == null)
                {
                    return null;
                }

                int length = text.Length;
                int offset = position;
                while (offset < length)
                {
                    char ch = text[offset];
                    switch (ch)
                    {
                        case '\r':
                        case '\n':
                            string str = text.Substring(position, offset - position);
                            position = offset + 1;
                            if (((ch == '\r') && (position < length)) && (text[position] == '\n'))
                            {
                                position++;
                            }

                            return str;
                        default:
                            offset++;
                            break;
                    }
                }

                if (offset > position)
                {
                    string str = text.Substring(position, offset - position);
                    position = offset;
                    return str;
                }

                return null;
            }
        }
    }
}
