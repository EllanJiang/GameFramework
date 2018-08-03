//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System;
using System.Text;

namespace GameFramework
{
    public static partial class Utility
    {
        /// <summary>
        /// 类型转换相关的实用函数。
        /// </summary>
        public static class Converter
        {
            private const float InchesToCentimeters = 2.54f; // 1 inch = 2.54 cm
            private const float CentimetersToInches = 1f / InchesToCentimeters; // 1 cm = 0.3937 inches

            /// <summary>
            /// 获取数据在此计算机结构中存储时的字节顺序。
            /// </summary>
            public static bool IsLittleEndian
            {
                get
                {
                    return BitConverter.IsLittleEndian;
                }
            }

            /// <summary>
            /// 获取或设置屏幕每英寸点数。
            /// </summary>
            public static float ScreenDpi
            {
                get;
                set;
            }

            /// <summary>
            /// 将像素转换为厘米。
            /// </summary>
            /// <param name="pixels">像素。</param>
            /// <returns>厘米。</returns>
            public static float GetCentimetersFromPixels(float pixels)
            {
                if (ScreenDpi <= 0)
                {
                    throw new GameFrameworkException("You must set screen DPI first.");
                }

                return InchesToCentimeters * pixels / ScreenDpi;
            }

            /// <summary>
            /// 将厘米转换为像素。
            /// </summary>
            /// <param name="centimeters">厘米。</param>
            /// <returns>像素。</returns>
            public static float GetPixelsFromCentimeters(float centimeters)
            {
                if (ScreenDpi <= 0)
                {
                    throw new GameFrameworkException("You must set screen DPI first.");
                }

                return CentimetersToInches * centimeters * ScreenDpi;
            }

            /// <summary>
            /// 将像素转换为英寸。
            /// </summary>
            /// <param name="pixels">像素。</param>
            /// <returns>英寸。</returns>
            public static float GetInchesFromPixels(float pixels)
            {
                if (ScreenDpi <= 0)
                {
                    throw new GameFrameworkException("You must set screen DPI first.");
                }

                return pixels / ScreenDpi;
            }

            /// <summary>
            /// 将英寸转换为像素。
            /// </summary>
            /// <param name="inches">英寸。</param>
            /// <returns>像素。</returns>
            public static float GetPixelsFromInches(float inches)
            {
                if (ScreenDpi <= 0)
                {
                    throw new GameFrameworkException("You must set screen DPI first.");
                }

                return inches * ScreenDpi;
            }

            /// <summary>
            /// 以字节数组的形式返回指定的布尔值。
            /// </summary>
            /// <param name="value">要转换的布尔值。</param>
            /// <returns>长度为 1 的字节数组。</returns>
            public static byte[] GetBytes(bool value)
            {
                return BitConverter.GetBytes(value);
            }

            /// <summary>
            /// 返回由字节数组中首字节转换来的布尔值。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <returns>如果 value 中的首字节非零，则为 true，否则为 false。</returns>
            public static bool GetBoolean(byte[] value)
            {
                return BitConverter.ToBoolean(value, 0);
            }

            /// <summary>
            /// 返回由字节数组中指定位置的一个字节转换来的布尔值。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <param name="startIndex">value 内的起始位置。</param>
            /// <returns>如果 value 中指定位置的字节非零，则为 true，否则为 false。</returns>
            public static bool GetBoolean(byte[] value, int startIndex)
            {
                return BitConverter.ToBoolean(value, startIndex);
            }

            /// <summary>
            /// 以字节数组的形式返回指定的 Unicode 字符值。
            /// </summary>
            /// <param name="value">要转换的字符。</param>
            /// <returns>长度为 2 的字节数组。</returns>
            public static byte[] GetBytes(char value)
            {
                return BitConverter.GetBytes(value);
            }

            /// <summary>
            /// 返回由字节数组中前两个字节转换来的 Unicode 字符。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <returns>由两个字节构成的字符。</returns>
            public static char GetChar(byte[] value)
            {
                return BitConverter.ToChar(value, 0);
            }

            /// <summary>
            /// 返回由字节数组中指定位置的两个字节转换来的 Unicode 字符。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <param name="startIndex">value 内的起始位置。</param>
            /// <returns>由两个字节构成的字符。</returns>
            public static char GetChar(byte[] value, int startIndex)
            {
                return BitConverter.ToChar(value, startIndex);
            }

            /// <summary>
            /// 以字节数组的形式返回指定的 16 位有符号整数值。
            /// </summary>
            /// <param name="value">要转换的数字。</param>
            /// <returns>长度为 2 的字节数组。</returns>
            public static byte[] GetBytes(short value)
            {
                return BitConverter.GetBytes(value);
            }

            /// <summary>
            /// 返回由字节数组中前两个字节转换来的 16 位有符号整数。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <returns>由两个字节构成的 16 位有符号整数。</returns>
            public static short GetInt16(byte[] value)
            {
                return BitConverter.ToInt16(value, 0);
            }

            /// <summary>
            /// 返回由字节数组中指定位置的两个字节转换来的 16 位有符号整数。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <param name="startIndex">value 内的起始位置。</param>
            /// <returns>由两个字节构成的 16 位有符号整数。</returns>
            public static short GetInt16(byte[] value, int startIndex)
            {
                return BitConverter.ToInt16(value, startIndex);
            }

            /// <summary>
            /// 以字节数组的形式返回指定的 16 位无符号整数值。
            /// </summary>
            /// <param name="value">要转换的数字。</param>
            /// <returns>长度为 2 的字节数组。</returns>
            public static byte[] GetBytes(ushort value)
            {
                return BitConverter.GetBytes(value);
            }

            /// <summary>
            /// 返回由字节数组中前两个字节转换来的 16 位无符号整数。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <returns>由两个字节构成的 16 位无符号整数。</returns>
            public static ushort GetUInt16(byte[] value)
            {
                return BitConverter.ToUInt16(value, 0);
            }

            /// <summary>
            /// 返回由字节数组中指定位置的两个字节转换来的 16 位无符号整数。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <param name="startIndex">value 内的起始位置。</param>
            /// <returns>由两个字节构成的 16 位无符号整数。</returns>
            public static ushort GetUInt16(byte[] value, int startIndex)
            {
                return BitConverter.ToUInt16(value, startIndex);
            }

            /// <summary>
            /// 以字节数组的形式返回指定的 32 位有符号整数值。
            /// </summary>
            /// <param name="value">要转换的数字。</param>
            /// <returns>长度为 4 的字节数组。</returns>
            public static byte[] GetBytes(int value)
            {
                return BitConverter.GetBytes(value);
            }

            /// <summary>
            /// 返回由字节数组中前四个字节转换来的 32 位有符号整数。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <returns>由四个字节构成的 32 位有符号整数。</returns>
            public static int GetInt32(byte[] value)
            {
                return BitConverter.ToInt32(value, 0);
            }

            /// <summary>
            /// 返回由字节数组中指定位置的四个字节转换来的 32 位有符号整数。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <param name="startIndex">value 内的起始位置。</param>
            /// <returns>由四个字节构成的 32 位有符号整数。</returns>
            public static int GetInt32(byte[] value, int startIndex)
            {
                return BitConverter.ToInt32(value, startIndex);
            }

            /// <summary>
            /// 以字节数组的形式返回指定的 32 位无符号整数值。
            /// </summary>
            /// <param name="value">要转换的数字。</param>
            /// <returns>长度为 4 的字节数组。</returns>
            public static byte[] GetBytes(uint value)
            {
                return BitConverter.GetBytes(value);
            }

            /// <summary>
            /// 返回由字节数组中前四个字节转换来的 32 位无符号整数。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <returns>由四个字节构成的 32 位无符号整数。</returns>
            public static uint GetUInt32(byte[] value)
            {
                return BitConverter.ToUInt32(value, 0);
            }

            /// <summary>
            /// 返回由字节数组中指定位置的四个字节转换来的 32 位无符号整数。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <param name="startIndex">value 内的起始位置。</param>
            /// <returns>由四个字节构成的 32 位无符号整数。</returns>
            public static uint GetUInt32(byte[] value, int startIndex)
            {
                return BitConverter.ToUInt32(value, startIndex);
            }

            /// <summary>
            /// 以字节数组的形式返回指定的 64 位有符号整数值。
            /// </summary>
            /// <param name="value">要转换的数字。</param>
            /// <returns>长度为 8 的字节数组。</returns>
            public static byte[] GetBytes(long value)
            {
                return BitConverter.GetBytes(value);
            }

            /// <summary>
            /// 返回由字节数组中前八个字节转换来的 64 位有符号整数。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <returns>由八个字节构成的 64 位有符号整数。</returns>
            public static long GetInt64(byte[] value)
            {
                return BitConverter.ToInt64(value, 0);
            }

            /// <summary>
            /// 返回由字节数组中指定位置的八个字节转换来的 64 位有符号整数。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <param name="startIndex">value 内的起始位置。</param>
            /// <returns>由八个字节构成的 64 位有符号整数。</returns>
            public static long GetInt64(byte[] value, int startIndex)
            {
                return BitConverter.ToInt64(value, startIndex);
            }

            /// <summary>
            /// 以字节数组的形式返回指定的 64 位无符号整数值。
            /// </summary>
            /// <param name="value">要转换的数字。</param>
            /// <returns>长度为 8 的字节数组。</returns>
            public static byte[] GetBytes(ulong value)
            {
                return BitConverter.GetBytes(value);
            }

            /// <summary>
            /// 返回由字节数组中前八个字节转换来的 64 位无符号整数。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <returns>由八个字节构成的 64 位无符号整数。</returns>
            public static ulong GetUInt64(byte[] value)
            {
                return BitConverter.ToUInt64(value, 0);
            }

            /// <summary>
            /// 返回由字节数组中指定位置的八个字节转换来的 64 位无符号整数。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <param name="startIndex">value 内的起始位置。</param>
            /// <returns>由八个字节构成的 64 位无符号整数。</returns>
            public static ulong GetUInt64(byte[] value, int startIndex)
            {
                return BitConverter.ToUInt64(value, startIndex);
            }

            /// <summary>
            /// 以字节数组的形式返回指定的单精度浮点值。
            /// </summary>
            /// <param name="value">要转换的数字。</param>
            /// <returns>长度为 4 的字节数组。</returns>
            public static byte[] GetBytes(float value)
            {
                return BitConverter.GetBytes(value);
            }

            /// <summary>
            /// 返回由字节数组中前四个字节转换来的单精度浮点数。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <returns>由四个字节构成的单精度浮点数。</returns>
            public static float GetSingle(byte[] value)
            {
                return BitConverter.ToSingle(value, 0);
            }

            /// <summary>
            /// 返回由字节数组中指定位置的四个字节转换来的单精度浮点数。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <param name="startIndex">value 内的起始位置。</param>
            /// <returns>由四个字节构成的单精度浮点数。</returns>
            public static float GetSingle(byte[] value, int startIndex)
            {
                return BitConverter.ToSingle(value, startIndex);
            }

            /// <summary>
            /// 以字节数组的形式返回指定的双精度浮点值。
            /// </summary>
            /// <param name="value">要转换的数字。</param>
            /// <returns>长度为 8 的字节数组。</returns>
            public static byte[] GetBytes(double value)
            {
                return BitConverter.GetBytes(value);
            }

            /// <summary>
            /// 返回由字节数组中前八个字节转换来的双精度浮点数。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <returns>由八个字节构成的双精度浮点数。</returns>
            public static double GetDouble(byte[] value)
            {
                return BitConverter.ToDouble(value, 0);
            }

            /// <summary>
            /// 返回由字节数组中指定位置的八个字节转换来的双精度浮点数。
            /// </summary>
            /// <param name="value">字节数组。</param>
            /// <param name="startIndex">value 内的起始位置。</param>
            /// <returns>由八个字节构成的双精度浮点数。</returns>
            public static double GetDouble(byte[] value, int startIndex)
            {
                return BitConverter.ToDouble(value, startIndex);
            }

            /// <summary>
            /// 以 UTF-8 字节数组的形式返回指定的字符串。
            /// </summary>
            /// <param name="value">要转换的字符串。</param>
            /// <returns>UTF-8 字节数组。</returns>
            public static byte[] GetBytes(string value)
            {
                return Encoding.UTF8.GetBytes(value);
            }

            /// <summary>
            /// 返回由 UTF-8 字节数组转换来的字符串。
            /// </summary>
            /// <param name="value">UTF-8 字节数组。</param>
            /// <returns>字符串。</returns>
            public static string GetString(byte[] value)
            {
                if (value == null)
                {
                    throw new GameFrameworkException("Value is invalid.");
                }

                return Encoding.UTF8.GetString(value, 0, value.Length);
            }
        }
    }
}
