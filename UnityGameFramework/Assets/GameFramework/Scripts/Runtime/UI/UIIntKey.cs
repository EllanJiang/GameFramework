//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 界面整型主键。
    /// </summary>
    public class UIIntKey : MonoBehaviour
    {
        [SerializeField]
        private int m_Key = 0;

        /// <summary>
        /// 获取或设置主键。
        /// </summary>
        public int Key
        {
            get
            {
                return m_Key;
            }
            set
            {
                m_Key = value;
            }
        }
    }
}
