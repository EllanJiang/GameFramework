//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using System;
using System.Runtime.InteropServices;

namespace GameFramework
{
    /// <summary>
    /// 类型和名称的组合值。
    /// </summary>
    [StructLayout(LayoutKind.Auto)]
    internal struct TypeNamePair : IEquatable<TypeNamePair>
    {
        private readonly Type m_Type;
        private readonly string m_Name;

        /// <summary>
        /// 初始化类型和名称的组合值的新实例。
        /// </summary>
        /// <param name="type">类型。</param>
        public TypeNamePair(Type type)
            : this(type, string.Empty)
        {
        }

        /// <summary>
        /// 初始化类型和名称的组合值的新实例。
        /// </summary>
        /// <param name="type">类型。</param>
        /// <param name="name">名称。</param>
        public TypeNamePair(Type type, string name)
        {
            if (type == null)
            {
                throw new GameFrameworkException("Type is invalid.");
            }

            m_Type = type;
            m_Name = name ?? string.Empty;
        }

        /// <summary>
        /// 获取类型。
        /// </summary>
        public Type Type
        {
            get
            {
                return m_Type;
            }
        }

        /// <summary>
        /// 获取名称。
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
        }

        /// <summary>
        /// 获取类型和名称的组合值字符串。
        /// </summary>
        /// <returns>类型和名称的组合值字符串。</returns>
        public override string ToString()
        {
            if (m_Type == null)
            {
                throw new GameFrameworkException("Type is invalid.");
            }

            string typeName = m_Type.FullName;
            return string.IsNullOrEmpty(m_Name) ? typeName : Utility.Text.Format("{0}.{1}", typeName, m_Name);
        }

        /// <summary>
        /// 获取对象的哈希值。
        /// </summary>
        /// <returns>对象的哈希值。</returns>
        public override int GetHashCode()
        {
            return m_Type.GetHashCode() ^ m_Name.GetHashCode();
        }

        /// <summary>
        /// 比较对象是否与自身相等。
        /// </summary>
        /// <param name="obj">要比较的对象。</param>
        /// <returns>被比较的对象是否与自身相等。</returns>
        public override bool Equals(object obj)
        {
            return obj is TypeNamePair && Equals((TypeNamePair)obj);
        }

        /// <summary>
        /// 比较对象是否与自身相等。
        /// </summary>
        /// <param name="value">要比较的对象。</param>
        /// <returns>被比较的对象是否与自身相等。</returns>
        public bool Equals(TypeNamePair value)
        {
            return m_Type == value.m_Type && m_Name == value.m_Name;
        }

        /// <summary>
        /// 判断两个对象是否相等。
        /// </summary>
        /// <param name="a">值 a。</param>
        /// <param name="b">值 b。</param>
        /// <returns>两个对象是否相等。</returns>
        public static bool operator ==(TypeNamePair a, TypeNamePair b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// 判断两个对象是否不相等。
        /// </summary>
        /// <param name="a">值 a。</param>
        /// <param name="b">值 b。</param>
        /// <returns>两个对象是否不相等。</returns>
        public static bool operator !=(TypeNamePair a, TypeNamePair b)
        {
            return !(a == b);
        }
    }
}
