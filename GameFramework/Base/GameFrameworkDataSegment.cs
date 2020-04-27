//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;

namespace GameFramework
{
    /// <summary>
    /// 数据片段。
    /// </summary>
    public struct GameFrameworkDataSegment : IEquatable<GameFrameworkDataSegment>
    {
        private readonly object m_Data;
        private readonly int m_Offset;
        private readonly int m_Length;

        /// <summary>
        /// 初始化数据片段的新实例。
        /// </summary>
        /// <param name="data">完整数据。</param>
        /// <param name="offset">偏移。</param>
        /// <param name="length">长度。</param>
        public GameFrameworkDataSegment(object data, int offset, int length)
        {
            if (data == null)
            {
                throw new GameFrameworkException("Data is invalid.");
            }

            if (offset < 0)
            {
                throw new GameFrameworkException("Offset is invalid.");
            }

            if (length <= 0)
            {
                throw new GameFrameworkException("Length is invalid.");
            }

            m_Data = data;
            m_Offset = offset;
            m_Length = length;
        }

        /// <summary>
        /// 获取完整数据。
        /// </summary>
        public object Data
        {
            get
            {
                return m_Data;
            }
        }

        /// <summary>
        /// 获取数据类型。
        /// </summary>
        public Type DataType
        {
            get
            {
                return m_Data.GetType();
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
            return m_Data.GetHashCode() ^ m_Offset ^ m_Length;
        }

        /// <summary>
        /// 比较对象是否与自身相等。
        /// </summary>
        /// <param name="obj">要比较的对象。</param>
        /// <returns>被比较的对象是否与自身相等。</returns>
        public override bool Equals(object obj)
        {
            return obj is GameFrameworkDataSegment && Equals((GameFrameworkDataSegment)obj);
        }

        /// <summary>
        /// 比较对象是否与自身相等。
        /// </summary>
        /// <param name="value">要比较的对象。</param>
        /// <returns>被比较的对象是否与自身相等。</returns>
        public bool Equals(GameFrameworkDataSegment value)
        {
            return value.m_Data == m_Data && value.m_Offset == m_Offset && value.m_Length == m_Length;
        }

        /// <summary>
        /// 判断两个对象是否相等。
        /// </summary>
        /// <param name="a">值 a。</param>
        /// <param name="b">值 b。</param>
        /// <returns>两个对象是否相等。</returns>
        public static bool operator ==(GameFrameworkDataSegment a, GameFrameworkDataSegment b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// 判断两个对象是否不相等。
        /// </summary>
        /// <param name="a">值 a。</param>
        /// <param name="b">值 b。</param>
        /// <returns>两个对象是否不相等。</returns>
        public static bool operator !=(GameFrameworkDataSegment a, GameFrameworkDataSegment b)
        {
            return !(a == b);
        }
    }
}
