//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework
{
    public static partial class Utility
    {
        public static partial class Text
        {
            /// <summary>
            /// 字符辅助器接口。
            /// </summary>
            public interface ITextHelper
            {
                /// <summary>
                /// 获取格式化字符串。
                /// </summary>
                /// <typeparam name="T">字符串参数的类型。</typeparam>
                /// <param name="format">字符串格式。</param>
                /// <param name="arg">字符串参数。</param>
                /// <returns>格式化后的字符串。</returns>
                string Format<T>(string format, T arg);

                /// <summary>
                /// 获取格式化字符串。
                /// </summary>
                /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
                /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
                /// <param name="format">字符串格式。</param>
                /// <param name="arg1">字符串参数 1。</param>
                /// <param name="arg2">字符串参数 2。</param>
                /// <returns>格式化后的字符串。</returns>
                string Format<T1, T2>(string format, T1 arg1, T2 arg2);

                /// <summary>
                /// 获取格式化字符串。
                /// </summary>
                /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
                /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
                /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
                /// <param name="format">字符串格式。</param>
                /// <param name="arg1">字符串参数 1。</param>
                /// <param name="arg2">字符串参数 2。</param>
                /// <param name="arg3">字符串参数 3。</param>
                /// <returns>格式化后的字符串。</returns>
                string Format<T1, T2, T3>(string format, T1 arg1, T2 arg2, T3 arg3);

                /// <summary>
                /// 获取格式化字符串。
                /// </summary>
                /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
                /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
                /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
                /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
                /// <param name="format">字符串格式。</param>
                /// <param name="arg1">字符串参数 1。</param>
                /// <param name="arg2">字符串参数 2。</param>
                /// <param name="arg3">字符串参数 3。</param>
                /// <param name="arg4">字符串参数 4。</param>
                /// <returns>格式化后的字符串。</returns>
                string Format<T1, T2, T3, T4>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4);

                /// <summary>
                /// 获取格式化字符串。
                /// </summary>
                /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
                /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
                /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
                /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
                /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
                /// <param name="format">字符串格式。</param>
                /// <param name="arg1">字符串参数 1。</param>
                /// <param name="arg2">字符串参数 2。</param>
                /// <param name="arg3">字符串参数 3。</param>
                /// <param name="arg4">字符串参数 4。</param>
                /// <param name="arg5">字符串参数 5。</param>
                /// <returns>格式化后的字符串。</returns>
                string Format<T1, T2, T3, T4, T5>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

                /// <summary>
                /// 获取格式化字符串。
                /// </summary>
                /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
                /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
                /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
                /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
                /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
                /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
                /// <param name="format">字符串格式。</param>
                /// <param name="arg1">字符串参数 1。</param>
                /// <param name="arg2">字符串参数 2。</param>
                /// <param name="arg3">字符串参数 3。</param>
                /// <param name="arg4">字符串参数 4。</param>
                /// <param name="arg5">字符串参数 5。</param>
                /// <param name="arg6">字符串参数 6。</param>
                /// <returns>格式化后的字符串。</returns>
                string Format<T1, T2, T3, T4, T5, T6>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);

                /// <summary>
                /// 获取格式化字符串。
                /// </summary>
                /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
                /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
                /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
                /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
                /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
                /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
                /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
                /// <param name="format">字符串格式。</param>
                /// <param name="arg1">字符串参数 1。</param>
                /// <param name="arg2">字符串参数 2。</param>
                /// <param name="arg3">字符串参数 3。</param>
                /// <param name="arg4">字符串参数 4。</param>
                /// <param name="arg5">字符串参数 5。</param>
                /// <param name="arg6">字符串参数 6。</param>
                /// <param name="arg7">字符串参数 7。</param>
                /// <returns>格式化后的字符串。</returns>
                string Format<T1, T2, T3, T4, T5, T6, T7>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);

                /// <summary>
                /// 获取格式化字符串。
                /// </summary>
                /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
                /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
                /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
                /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
                /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
                /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
                /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
                /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
                /// <param name="format">字符串格式。</param>
                /// <param name="arg1">字符串参数 1。</param>
                /// <param name="arg2">字符串参数 2。</param>
                /// <param name="arg3">字符串参数 3。</param>
                /// <param name="arg4">字符串参数 4。</param>
                /// <param name="arg5">字符串参数 5。</param>
                /// <param name="arg6">字符串参数 6。</param>
                /// <param name="arg7">字符串参数 7。</param>
                /// <param name="arg8">字符串参数 8。</param>
                /// <returns>格式化后的字符串。</returns>
                string Format<T1, T2, T3, T4, T5, T6, T7, T8>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

                /// <summary>
                /// 获取格式化字符串。
                /// </summary>
                /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
                /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
                /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
                /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
                /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
                /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
                /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
                /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
                /// <typeparam name="T9">字符串参数 9 的类型。</typeparam>
                /// <param name="format">字符串格式。</param>
                /// <param name="arg1">字符串参数 1。</param>
                /// <param name="arg2">字符串参数 2。</param>
                /// <param name="arg3">字符串参数 3。</param>
                /// <param name="arg4">字符串参数 4。</param>
                /// <param name="arg5">字符串参数 5。</param>
                /// <param name="arg6">字符串参数 6。</param>
                /// <param name="arg7">字符串参数 7。</param>
                /// <param name="arg8">字符串参数 8。</param>
                /// <param name="arg9">字符串参数 9。</param>
                /// <returns>格式化后的字符串。</returns>
                string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9);

                /// <summary>
                /// 获取格式化字符串。
                /// </summary>
                /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
                /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
                /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
                /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
                /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
                /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
                /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
                /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
                /// <typeparam name="T9">字符串参数 9 的类型。</typeparam>
                /// <typeparam name="T10">字符串参数 10 的类型。</typeparam>
                /// <param name="format">字符串格式。</param>
                /// <param name="arg1">字符串参数 1。</param>
                /// <param name="arg2">字符串参数 2。</param>
                /// <param name="arg3">字符串参数 3。</param>
                /// <param name="arg4">字符串参数 4。</param>
                /// <param name="arg5">字符串参数 5。</param>
                /// <param name="arg6">字符串参数 6。</param>
                /// <param name="arg7">字符串参数 7。</param>
                /// <param name="arg8">字符串参数 8。</param>
                /// <param name="arg9">字符串参数 9。</param>
                /// <param name="arg10">字符串参数 10。</param>
                /// <returns>格式化后的字符串。</returns>
                string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10);

                /// <summary>
                /// 获取格式化字符串。
                /// </summary>
                /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
                /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
                /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
                /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
                /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
                /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
                /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
                /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
                /// <typeparam name="T9">字符串参数 9 的类型。</typeparam>
                /// <typeparam name="T10">字符串参数 10 的类型。</typeparam>
                /// <typeparam name="T11">字符串参数 11 的类型。</typeparam>
                /// <param name="format">字符串格式。</param>
                /// <param name="arg1">字符串参数 1。</param>
                /// <param name="arg2">字符串参数 2。</param>
                /// <param name="arg3">字符串参数 3。</param>
                /// <param name="arg4">字符串参数 4。</param>
                /// <param name="arg5">字符串参数 5。</param>
                /// <param name="arg6">字符串参数 6。</param>
                /// <param name="arg7">字符串参数 7。</param>
                /// <param name="arg8">字符串参数 8。</param>
                /// <param name="arg9">字符串参数 9。</param>
                /// <param name="arg10">字符串参数 10。</param>
                /// <param name="arg11">字符串参数 11。</param>
                /// <returns>格式化后的字符串。</returns>
                string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11);

                /// <summary>
                /// 获取格式化字符串。
                /// </summary>
                /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
                /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
                /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
                /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
                /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
                /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
                /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
                /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
                /// <typeparam name="T9">字符串参数 9 的类型。</typeparam>
                /// <typeparam name="T10">字符串参数 10 的类型。</typeparam>
                /// <typeparam name="T11">字符串参数 11 的类型。</typeparam>
                /// <typeparam name="T12">字符串参数 12 的类型。</typeparam>
                /// <param name="format">字符串格式。</param>
                /// <param name="arg1">字符串参数 1。</param>
                /// <param name="arg2">字符串参数 2。</param>
                /// <param name="arg3">字符串参数 3。</param>
                /// <param name="arg4">字符串参数 4。</param>
                /// <param name="arg5">字符串参数 5。</param>
                /// <param name="arg6">字符串参数 6。</param>
                /// <param name="arg7">字符串参数 7。</param>
                /// <param name="arg8">字符串参数 8。</param>
                /// <param name="arg9">字符串参数 9。</param>
                /// <param name="arg10">字符串参数 10。</param>
                /// <param name="arg11">字符串参数 11。</param>
                /// <param name="arg12">字符串参数 12。</param>
                /// <returns>格式化后的字符串。</returns>
                string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12);

                /// <summary>
                /// 获取格式化字符串。
                /// </summary>
                /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
                /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
                /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
                /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
                /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
                /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
                /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
                /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
                /// <typeparam name="T9">字符串参数 9 的类型。</typeparam>
                /// <typeparam name="T10">字符串参数 10 的类型。</typeparam>
                /// <typeparam name="T11">字符串参数 11 的类型。</typeparam>
                /// <typeparam name="T12">字符串参数 12 的类型。</typeparam>
                /// <typeparam name="T13">字符串参数 13 的类型。</typeparam>
                /// <param name="format">字符串格式。</param>
                /// <param name="arg1">字符串参数 1。</param>
                /// <param name="arg2">字符串参数 2。</param>
                /// <param name="arg3">字符串参数 3。</param>
                /// <param name="arg4">字符串参数 4。</param>
                /// <param name="arg5">字符串参数 5。</param>
                /// <param name="arg6">字符串参数 6。</param>
                /// <param name="arg7">字符串参数 7。</param>
                /// <param name="arg8">字符串参数 8。</param>
                /// <param name="arg9">字符串参数 9。</param>
                /// <param name="arg10">字符串参数 10。</param>
                /// <param name="arg11">字符串参数 11。</param>
                /// <param name="arg12">字符串参数 12。</param>
                /// <param name="arg13">字符串参数 13。</param>
                /// <returns>格式化后的字符串。</returns>
                string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13);

                /// <summary>
                /// 获取格式化字符串。
                /// </summary>
                /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
                /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
                /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
                /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
                /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
                /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
                /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
                /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
                /// <typeparam name="T9">字符串参数 9 的类型。</typeparam>
                /// <typeparam name="T10">字符串参数 10 的类型。</typeparam>
                /// <typeparam name="T11">字符串参数 11 的类型。</typeparam>
                /// <typeparam name="T12">字符串参数 12 的类型。</typeparam>
                /// <typeparam name="T13">字符串参数 13 的类型。</typeparam>
                /// <typeparam name="T14">字符串参数 14 的类型。</typeparam>
                /// <param name="format">字符串格式。</param>
                /// <param name="arg1">字符串参数 1。</param>
                /// <param name="arg2">字符串参数 2。</param>
                /// <param name="arg3">字符串参数 3。</param>
                /// <param name="arg4">字符串参数 4。</param>
                /// <param name="arg5">字符串参数 5。</param>
                /// <param name="arg6">字符串参数 6。</param>
                /// <param name="arg7">字符串参数 7。</param>
                /// <param name="arg8">字符串参数 8。</param>
                /// <param name="arg9">字符串参数 9。</param>
                /// <param name="arg10">字符串参数 10。</param>
                /// <param name="arg11">字符串参数 11。</param>
                /// <param name="arg12">字符串参数 12。</param>
                /// <param name="arg13">字符串参数 13。</param>
                /// <param name="arg14">字符串参数 14。</param>
                /// <returns>格式化后的字符串。</returns>
                string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14);

                /// <summary>
                /// 获取格式化字符串。
                /// </summary>
                /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
                /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
                /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
                /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
                /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
                /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
                /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
                /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
                /// <typeparam name="T9">字符串参数 9 的类型。</typeparam>
                /// <typeparam name="T10">字符串参数 10 的类型。</typeparam>
                /// <typeparam name="T11">字符串参数 11 的类型。</typeparam>
                /// <typeparam name="T12">字符串参数 12 的类型。</typeparam>
                /// <typeparam name="T13">字符串参数 13 的类型。</typeparam>
                /// <typeparam name="T14">字符串参数 14 的类型。</typeparam>
                /// <typeparam name="T15">字符串参数 15 的类型。</typeparam>
                /// <param name="format">字符串格式。</param>
                /// <param name="arg1">字符串参数 1。</param>
                /// <param name="arg2">字符串参数 2。</param>
                /// <param name="arg3">字符串参数 3。</param>
                /// <param name="arg4">字符串参数 4。</param>
                /// <param name="arg5">字符串参数 5。</param>
                /// <param name="arg6">字符串参数 6。</param>
                /// <param name="arg7">字符串参数 7。</param>
                /// <param name="arg8">字符串参数 8。</param>
                /// <param name="arg9">字符串参数 9。</param>
                /// <param name="arg10">字符串参数 10。</param>
                /// <param name="arg11">字符串参数 11。</param>
                /// <param name="arg12">字符串参数 12。</param>
                /// <param name="arg13">字符串参数 13。</param>
                /// <param name="arg14">字符串参数 14。</param>
                /// <param name="arg15">字符串参数 15。</param>
                /// <returns>格式化后的字符串。</returns>
                string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15);

                /// <summary>
                /// 获取格式化字符串。
                /// </summary>
                /// <typeparam name="T1">字符串参数 1 的类型。</typeparam>
                /// <typeparam name="T2">字符串参数 2 的类型。</typeparam>
                /// <typeparam name="T3">字符串参数 3 的类型。</typeparam>
                /// <typeparam name="T4">字符串参数 4 的类型。</typeparam>
                /// <typeparam name="T5">字符串参数 5 的类型。</typeparam>
                /// <typeparam name="T6">字符串参数 6 的类型。</typeparam>
                /// <typeparam name="T7">字符串参数 7 的类型。</typeparam>
                /// <typeparam name="T8">字符串参数 8 的类型。</typeparam>
                /// <typeparam name="T9">字符串参数 9 的类型。</typeparam>
                /// <typeparam name="T10">字符串参数 10 的类型。</typeparam>
                /// <typeparam name="T11">字符串参数 11 的类型。</typeparam>
                /// <typeparam name="T12">字符串参数 12 的类型。</typeparam>
                /// <typeparam name="T13">字符串参数 13 的类型。</typeparam>
                /// <typeparam name="T14">字符串参数 14 的类型。</typeparam>
                /// <typeparam name="T15">字符串参数 15 的类型。</typeparam>
                /// <typeparam name="T16">字符串参数 16 的类型。</typeparam>
                /// <param name="format">字符串格式。</param>
                /// <param name="arg1">字符串参数 1。</param>
                /// <param name="arg2">字符串参数 2。</param>
                /// <param name="arg3">字符串参数 3。</param>
                /// <param name="arg4">字符串参数 4。</param>
                /// <param name="arg5">字符串参数 5。</param>
                /// <param name="arg6">字符串参数 6。</param>
                /// <param name="arg7">字符串参数 7。</param>
                /// <param name="arg8">字符串参数 8。</param>
                /// <param name="arg9">字符串参数 9。</param>
                /// <param name="arg10">字符串参数 10。</param>
                /// <param name="arg11">字符串参数 11。</param>
                /// <param name="arg12">字符串参数 12。</param>
                /// <param name="arg13">字符串参数 13。</param>
                /// <param name="arg14">字符串参数 14。</param>
                /// <param name="arg15">字符串参数 15。</param>
                /// <param name="arg16">字符串参数 16。</param>
                /// <returns>格式化后的字符串。</returns>
                string Format<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(string format, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15, T16 arg16);
            }
        }
    }
}
