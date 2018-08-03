//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Diagnostics;

namespace GameFramework
{
    public static partial class Utility
    {
        /// <summary>
        /// 性能分析相关的实用函数。
        /// </summary>
        public static partial class Profiler
        {
            private static IProfilerHelper s_ProfilerHelper = null;

            /// <summary>
            /// 设置性能分析辅助器。
            /// </summary>
            /// <param name="profilerHelper">要设置的性能分析辅助器。</param>
            public static void SetProfilerHelper(IProfilerHelper profilerHelper)
            {
                s_ProfilerHelper = profilerHelper;
            }

            /// <summary>
            /// 开始采样。
            /// </summary>
            /// <param name="name">采样名称。</param>
            [Conditional("DEBUG")]
            public static void BeginSample(string name)
            {
                if (s_ProfilerHelper == null)
                {
                    throw new GameFrameworkException("Profiler helper is invalid.");
                }

                s_ProfilerHelper.BeginSample(name);
            }

            /// <summary>
            /// 结束采样。
            /// </summary>
            [Conditional("DEBUG")]
            public static void EndSample()
            {
                if (s_ProfilerHelper == null)
                {
                    throw new GameFrameworkException("Profiler helper is invalid.");
                }

                s_ProfilerHelper.EndSample();
            }
        }
    }
}
