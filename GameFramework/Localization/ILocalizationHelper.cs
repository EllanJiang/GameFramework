//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Localization
{
    /// <summary>
    /// 本地化辅助器接口。
    /// </summary>
    public interface ILocalizationHelper
    {
        /// <summary>
        /// 获取系统语言。
        /// </summary>
        Language SystemLanguage
        {
            get;
        }

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryAsset">字典资源。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>加载是否成功。</returns>
        bool LoadDictionary(object dictionaryAsset, object userData);

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="text">要解析的字典文本。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析字典成功。</returns>
        bool ParseDictionary(string text, object userData);

        /// <summary>
        /// 释放字典资源。
        /// </summary>
        /// <param name="dictionaryAsset">要释放的字典资源。</param>
        void ReleaseDictionaryAsset(object dictionaryAsset);
    }
}
