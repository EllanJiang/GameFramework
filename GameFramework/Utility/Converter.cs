//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System;
using System.Text;

namespace Utility
{
    /// <summary>
    /// 类型转换相关的实用函数。
    /// </summary>
    public static class Converter
    {
        private const float INCHES_TO_CENTIMETERS = 2.54f; // 1 inch = 2.54 cm
        private const float CENTIMETERS_TO_INCHES = 1f / INCHES_TO_CENTIMETERS; // 1 cm = 0.3937 inches

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

            return INCHES_TO_CENTIMETERS * pixels / ScreenDpi;
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

            return CENTIMETERS_TO_INCHES * centimeters * ScreenDpi;
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
        /// 以字节数组的形式返回指定的 32 位有符号整数值。
        /// </summary>
        /// <param name="int32">要转换的数字。</param>
        /// <returns>长度为 4 的字节数组。</returns>
        public static byte[] GetBytesFromInt(int int32)
        {
            return BitConverter.GetBytes(int32);
        }

        /// <summary>
        /// 返回由字节数组中指定位置的四个字节转换来的 32 位有符号整数。
        /// </summary>
        /// <param name="bytes">字节数组。</param>
        /// <returns>由四个字节构成、从 startIndex 开始的 32 位有符号整数。</returns>
        public static int GetIntFromBytes(byte[] bytes)
        {
            return GetIntFromBytes(bytes, 0);
        }

        /// <summary>
        /// 返回由字节数组中指定位置的四个字节转换来的 32 位有符号整数。
        /// </summary>
        /// <param name="bytes">字节数组。</param>
        /// <param name="startIndex">bytes 内的起始位置。</param>
        /// <returns>由四个字节构成、从 startIndex 开始的 32 位有符号整数。</returns>
        public static int GetIntFromBytes(byte[] bytes, int startIndex)
        {
            return BitConverter.ToInt32(bytes, startIndex);
        }

        /// <summary>
        /// 将 UTF-8 字节流转换为字符串。
        /// </summary>
        /// <param name="bytes">要转换的字节流。</param>
        /// <returns>转化后的字符串。</returns>
        public static string GetStringFromBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new GameFrameworkException("Bytes is invalid.");
            }

            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// 将字符串转换为 UTF-8 字节流。
        /// </summary>
        /// <param name="str">要转换的字符串。</param>
        /// <returns>转换后的 UTF-8 字节流。</returns>
        public static byte[] GetBytesFromString(string str)
        {
            return Encoding.UTF8.GetBytes(str);
        }
    }
}
