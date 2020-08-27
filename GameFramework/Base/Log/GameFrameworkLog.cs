//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System.Diagnostics;

namespace GameFramework
{
    /// <summary>
    /// 游戏框架日志类。
    /// </summary>
    public static partial class GameFrameworkLog
    {
        private static ILogHelper s_LogHelper = null;

        /// <summary>
        /// 设置游戏框架日志辅助器。
        /// </summary>
        /// <param name="logHelper">要设置的游戏框架日志辅助器。</param>
        public static void SetLogHelper(ILogHelper logHelper)
        {
            s_LogHelper = logHelper;
        }

        /// <summary>
        /// 打印调试级别日志，用于记录调试类日志信息。
        /// </summary>
        /// <param name="message">日志内容。</param>
        /// <remarks>仅在带有 DEBUG 预编译选项时生效。</remarks>
        [Conditional("DEBUG")]
        public static void Debug(object message)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Debug, message);
        }

        /// <summary>
        /// 打印调试级别日志，用于记录调试类日志信息。
        /// </summary>
        /// <param name="message">日志内容。</param>
        /// <remarks>仅在带有 DEBUG 预编译选项时生效。</remarks>
        [Conditional("DEBUG")]
        public static void Debug(string message)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Debug, message);
        }

        /// <summary>
        /// 打印调试级别日志，用于记录调试类日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <remarks>仅在带有 DEBUG 预编译选项时生效。</remarks>
        [Conditional("DEBUG")]
        public static void Debug(string format, object arg0)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Debug, Utility.Text.Format(format, arg0));
        }

        /// <summary>
        /// 打印调试级别日志，用于记录调试类日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <remarks>仅在带有 DEBUG 预编译选项时生效。</remarks>
        [Conditional("DEBUG")]
        public static void Debug(string format, object arg0, object arg1)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Debug, Utility.Text.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 打印调试级别日志，用于记录调试类日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <param name="arg2">日志参数 2。</param>
        /// <remarks>仅在带有 DEBUG 预编译选项时生效。</remarks>
        [Conditional("DEBUG")]
        public static void Debug(string format, object arg0, object arg1, object arg2)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Debug, Utility.Text.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 打印调试级别日志，用于记录调试类日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="args">日志参数。</param>
        /// <remarks>仅在带有 DEBUG 预编译选项时生效。</remarks>
        [Conditional("DEBUG")]
        public static void Debug(string format, params object[] args)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Debug, Utility.Text.Format(format, args));
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行日志信息。
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Info(object message)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Info, message);
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行日志信息。
        /// </summary>
        /// <param name="message">日志内容</param>
        public static void Info(string message)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Info, message);
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        public static void Info(string format, object arg0)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Info, Utility.Text.Format(format, arg0));
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        public static void Info(string format, object arg0, object arg1)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Info, Utility.Text.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <param name="arg2">日志参数 2。</param>
        public static void Info(string format, object arg0, object arg1, object arg2)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Info, Utility.Text.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 打印信息级别日志，用于记录程序正常运行日志信息。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="args">日志参数。</param>
        public static void Info(string format, params object[] args)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Info, Utility.Text.Format(format, args));
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="message">日志内容。</param>
        public static void Warning(object message)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Warning, message);
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="message">日志内容。</param>
        public static void Warning(string message)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Warning, message);
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        public static void Warning(string format, object arg0)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Warning, Utility.Text.Format(format, arg0));
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        public static void Warning(string format, object arg0, object arg1)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Warning, Utility.Text.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <param name="arg2">日志参数 2。</param>
        public static void Warning(string format, object arg0, object arg1, object arg2)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Warning, Utility.Text.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 打印警告级别日志，建议在发生局部功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="args">日志参数。</param>
        public static void Warning(string format, params object[] args)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Warning, Utility.Text.Format(format, args));
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="message">日志内容。</param>
        public static void Error(object message)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Error, message);
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="message">日志内容。</param>
        public static void Error(string message)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Error, message);
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        public static void Error(string format, object arg0)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Error, Utility.Text.Format(format, arg0));
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        public static void Error(string format, object arg0, object arg1)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Error, Utility.Text.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <param name="arg2">日志参数 2。</param>
        public static void Error(string format, object arg0, object arg1, object arg2)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Error, Utility.Text.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 打印错误级别日志，建议在发生功能逻辑错误，但尚不会导致游戏崩溃或异常时使用。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="args">日志参数。</param>
        public static void Error(string format, params object[] args)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Error, Utility.Text.Format(format, args));
        }

        /// <summary>
        /// 打印严重错误级别日志，建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架。
        /// </summary>
        /// <param name="message">日志内容。</param>
        public static void Fatal(object message)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Fatal, message);
        }

        /// <summary>
        /// 打印严重错误级别日志，建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架。
        /// </summary>
        /// <param name="message">日志内容。</param>
        public static void Fatal(string message)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Fatal, message);
        }

        /// <summary>
        /// 打印严重错误级别日志，建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        public static void Fatal(string format, object arg0)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Fatal, Utility.Text.Format(format, arg0));
        }

        /// <summary>
        /// 打印严重错误级别日志，建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        public static void Fatal(string format, object arg0, object arg1)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Fatal, Utility.Text.Format(format, arg0, arg1));
        }

        /// <summary>
        /// 打印严重错误级别日志，建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="arg0">日志参数 0。</param>
        /// <param name="arg1">日志参数 1。</param>
        /// <param name="arg2">日志参数 2。</param>
        public static void Fatal(string format, object arg0, object arg1, object arg2)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Fatal, Utility.Text.Format(format, arg0, arg1, arg2));
        }

        /// <summary>
        /// 打印严重错误级别日志，建议在发生严重错误，可能导致游戏崩溃或异常时使用，此时应尝试重启进程或重建游戏框架。
        /// </summary>
        /// <param name="format">日志格式。</param>
        /// <param name="args">日志参数。</param>
        public static void Fatal(string format, params object[] args)
        {
            if (s_LogHelper == null)
            {
                return;
            }

            s_LogHelper.Log(GameFrameworkLogLevel.Fatal, Utility.Text.Format(format, args));
        }
    }
}
