//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework
{
    /// <summary>
    /// 日志回调函数。
    /// </summary>
    /// <param name="level">日志等级。</param>
    /// <param name="message">日志内容。</param>
    public delegate void LogCallback(LogLevel level, object message);
}
