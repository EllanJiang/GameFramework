//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework
{
    /// <summary>
    /// 数据片段。
    /// </summary>
    /// <typeparam name="T">数据源类型。</typeparam>
    public struct GameFrameworkSegment<T>
    {
        private readonly T m_Source;
        private readonly int m_Offset;
        private readonly int m_Length;

        /// <summary>
        /// 初始化数据片段的新实例。
        /// </summary>
        /// <param name="source">数据源。</param>
        /// <param name="offset">偏移。</param>
        /// <param name="length">长度。</param>
        public GameFrameworkSegment(T source, int offset, int length)
        {
            if (source == null)
            {
                throw new GameFrameworkException("Source is invalid.");
            }

            if (offset < 0)
            {
                throw new GameFrameworkException("Offset is invalid.");
            }

            if (length <= 0)
            {
                throw new GameFrameworkException("Length is invalid.");
            }

            m_Source = source;
            m_Offset = offset;
            m_Length = length;
        }

        /// <summary>
        /// 获取数据源。
        /// </summary>
        public T Source
        {
            get
            {
                return m_Source;
            }
        }

        /// <summary>
        /// 获取偏移。
        /// </summary>
        public int Offset
        {
            get
            {
                return m_Offset;
            }
        }

        /// <summary>
        /// 获取长度。
        /// </summary>
        public int Length
        {
            get
            {
                return m_Length;
            }
        }
    }
}
