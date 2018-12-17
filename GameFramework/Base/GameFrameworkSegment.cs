//------------------------------------------------------------
// Game Framework
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
    public struct GameFrameworkSegment<T> where T : class
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

        /// <summary>
        /// 获取对象的哈希值。
        /// </summary>
        /// <returns>对象的哈希值。</returns>
        public override int GetHashCode()
        {
            return m_Source.GetHashCode() ^ m_Offset ^ m_Length;
        }

        /// <summary>
        /// 比较对象是否与自身相等。
        /// </summary>
        /// <param name="obj">要比较的对象。</param>
        /// <returns>被比较的对象是否与自身相等。</returns>
        public override bool Equals(object obj)
        {
            return obj is GameFrameworkSegment<T> && Equals((GameFrameworkSegment<T>)obj);
        }

        /// <summary>
        /// 比较对象是否与自身相等。
        /// </summary>
        /// <param name="obj">要比较的对象。</param>
        /// <returns>被比较的对象是否与自身相等。</returns>
        public bool Equals(GameFrameworkSegment<T> obj)
        {
            return obj.m_Source == m_Source && obj.m_Offset == m_Offset && obj.m_Length == m_Length;
        }

        /// <summary>
        /// 判断两个对象是否相等。
        /// </summary>
        /// <param name="a">值 a。</param>
        /// <param name="b">值 b。</param>
        /// <returns>两个对象是否相等。</returns>
        public static bool operator ==(GameFrameworkSegment<T> a, GameFrameworkSegment<T> b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// 判断两个对象是否不相等。
        /// </summary>
        /// <param name="a">值 a。</param>
        /// <param name="b">值 b。</param>
        /// <returns>两个对象是否不相等。</returns>
        public static bool operator !=(GameFrameworkSegment<T> a, GameFrameworkSegment<T> b)
        {
            return !(a == b);
        }
    }
}
