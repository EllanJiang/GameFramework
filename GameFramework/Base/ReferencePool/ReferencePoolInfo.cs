//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework
{
    /// <summary>
    /// 引用池信息。
    /// </summary>
    public sealed class ReferencePoolInfo
    {
        private readonly string m_TypeName;
        private readonly int m_UnusedReferenceCount;
        private readonly int m_UsingReferenceCount;
        private readonly int m_AddReferenceCount;
        private readonly int m_RemoveReferenceCount;

        /// <summary>
        /// 初始化引用池信息的新实例。
        /// </summary>
        /// <param name="typeName">引用池类型名称。</param>
        /// <param name="unusedReferenceCount">未使用引用数量。</param>
        /// <param name="usingReferenceCount">正在使用引用数量。</param>
        /// <param name="addReferenceCount">已增加引用数量。</param>
        /// <param name="removeReferenceCount">已移除引用数量。</param>
        public ReferencePoolInfo(string typeName, int unusedReferenceCount, int usingReferenceCount, int addReferenceCount, int removeReferenceCount)
        {
            m_TypeName = typeName;
            m_UnusedReferenceCount = unusedReferenceCount;
            m_UsingReferenceCount = usingReferenceCount;
            m_AddReferenceCount = addReferenceCount;
            m_RemoveReferenceCount = removeReferenceCount;
        }

        /// <summary>
        /// 获取引用池类型名称。
        /// </summary>
        public string TypeName
        {
            get
            {
                return m_TypeName;
            }
        }

        /// <summary>
        /// 获取未使用引用数量。
        /// </summary>
        public int UnusedReferenceCount
        {
            get
            {
                return m_UnusedReferenceCount;
            }
        }

        /// <summary>
        /// 获取正在使用引用数量。
        /// </summary>
        public int UsingReferenceCount
        {
            get
            {
                return m_UsingReferenceCount;
            }
        }

        /// <summary>
        /// 获取已增加引用数量。
        /// </summary>
        public int AddReferenceCount
        {
            get
            {
                return m_AddReferenceCount;
            }
        }

        /// <summary>
        /// 获取已移除引用数量。
        /// </summary>
        public int RemoveReferenceCount
        {
            get
            {
                return m_RemoveReferenceCount;
            }
        }
    }
}
