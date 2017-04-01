//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace UnityGameFramework.Runtime
{
    public partial class DebuggerComponent
    {
        private sealed partial class RuntimeMemoryInformationWindow<T>
        {
            private sealed class Sample
            {
                private readonly string m_Name;
                private readonly string m_Type;
                private readonly long m_Size;
                private bool m_Highlight;

                public Sample(string name, string type, long size)
                {
                    m_Name = name;
                    m_Type = type;
                    m_Size = size;
                    m_Highlight = false;
                }

                public string Name
                {
                    get
                    {
                        return m_Name;
                    }
                }

                public string Type
                {
                    get
                    {
                        return m_Type;
                    }
                }

                public long Size
                {
                    get
                    {
                        return m_Size;
                    }
                }

                public bool Highlight
                {
                    get
                    {
                        return m_Highlight;
                    }
                    set
                    {
                        m_Highlight = value;
                    }
                }
            }
        }
    }
}
