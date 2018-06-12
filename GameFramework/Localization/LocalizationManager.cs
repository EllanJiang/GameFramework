//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework.Resource;
using System;
using System.Collections.Generic;

namespace GameFramework.Localization
{
    /// <summary>
    /// 本地化管理器。
    /// </summary>
    internal sealed class LocalizationManager : GameFrameworkModule, ILocalizationManager
    {
        private readonly Dictionary<string, string> m_Dictionary;
        private readonly LoadAssetCallbacks m_LoadAssetCallbacks;
        private IResourceManager m_ResourceManager;
        private ILocalizationHelper m_LocalizationHelper;
        private Language m_Language;
        private EventHandler<LoadDictionarySuccessEventArgs> m_LoadDictionarySuccessEventHandler;
        private EventHandler<LoadDictionaryFailureEventArgs> m_LoadDictionaryFailureEventHandler;
        private EventHandler<LoadDictionaryUpdateEventArgs> m_LoadDictionaryUpdateEventHandler;
        private EventHandler<LoadDictionaryDependencyAssetEventArgs> m_LoadDictionaryDependencyAssetEventHandler;

        /// <summary>
        /// 初始化本地化管理器的新实例。
        /// </summary>
        public LocalizationManager()
        {
            m_Dictionary = new Dictionary<string, string>();
            m_LoadAssetCallbacks = new LoadAssetCallbacks(LoadDictionarySuccessCallback, LoadDictionaryFailureCallback, LoadDictionaryUpdateCallback, LoadDictionaryDependencyAssetCallback);
            m_ResourceManager = null;
            m_LocalizationHelper = null;
            m_Language = Language.Unspecified;
            m_LoadDictionarySuccessEventHandler = null;
            m_LoadDictionaryFailureEventHandler = null;
            m_LoadDictionaryUpdateEventHandler = null;
            m_LoadDictionaryDependencyAssetEventHandler = null;
        }

        /// <summary>
        /// 获取或设置本地化语言。
        /// </summary>
        public Language Language
        {
            get
            {
                return m_Language;
            }
            set
            {
                if (value == Language.Unspecified)
                {
                    throw new GameFrameworkException("Language is invalid.");
                }

                m_Language = value;
            }
        }

        /// <summary>
        /// 获取系统语言。
        /// </summary>
        public Language SystemLanguage
        {
            get
            {
                if (m_LocalizationHelper == null)
                {
                    throw new GameFrameworkException("You must set localization helper first.");
                }

                return m_LocalizationHelper.SystemLanguage;
            }
        }

        /// <summary>
        /// 获取字典数量。
        /// </summary>
        public int DictionaryCount
        {
            get
            {
                return m_Dictionary.Count;
            }
        }

        /// <summary>
        /// 加载字典成功事件。
        /// </summary>
        public event EventHandler<LoadDictionarySuccessEventArgs> LoadDictionarySuccess
        {
            add
            {
                m_LoadDictionarySuccessEventHandler += value;
            }
            remove
            {
                m_LoadDictionarySuccessEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载字典失败事件。
        /// </summary>
        public event EventHandler<LoadDictionaryFailureEventArgs> LoadDictionaryFailure
        {
            add
            {
                m_LoadDictionaryFailureEventHandler += value;
            }
            remove
            {
                m_LoadDictionaryFailureEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载字典更新事件。
        /// </summary>
        public event EventHandler<LoadDictionaryUpdateEventArgs> LoadDictionaryUpdate
        {
            add
            {
                m_LoadDictionaryUpdateEventHandler += value;
            }
            remove
            {
                m_LoadDictionaryUpdateEventHandler -= value;
            }
        }

        /// <summary>
        /// 加载字典时加载依赖资源事件。
        /// </summary>
        public event EventHandler<LoadDictionaryDependencyAssetEventArgs> LoadDictionaryDependencyAsset
        {
            add
            {
                m_LoadDictionaryDependencyAssetEventHandler += value;
            }
            remove
            {
                m_LoadDictionaryDependencyAssetEventHandler -= value;
            }
        }

        /// <summary>
        /// 本地化管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        internal override void Update(float elapseSeconds, float realElapseSeconds)
        {

        }

        /// <summary>
        /// 关闭并清理本地化管理器。
        /// </summary>
        internal override void Shutdown()
        {

        }

        /// <summary>
        /// 设置资源管理器。
        /// </summary>
        /// <param name="resourceManager">资源管理器。</param>
        public void SetResourceManager(IResourceManager resourceManager)
        {
            if (resourceManager == null)
            {
                throw new GameFrameworkException("Resource manager is invalid.");
            }

            m_ResourceManager = resourceManager;
        }

        /// <summary>
        /// 设置本地化辅助器。
        /// </summary>
        /// <param name="localizationHelper">本地化辅助器。</param>
        public void SetLocalizationHelper(ILocalizationHelper localizationHelper)
        {
            if (localizationHelper == null)
            {
                throw new GameFrameworkException("Localization helper is invalid.");
            }

            m_LocalizationHelper = localizationHelper;
        }

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        public void LoadDictionary(string dictionaryAssetName)
        {
            LoadDictionary(dictionaryAssetName, Constant.DefaultPriority, null);
        }

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="priority">加载字典资源的优先级。</param>
        public void LoadDictionary(string dictionaryAssetName, int priority)
        {
            LoadDictionary(dictionaryAssetName, priority, null);
        }

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadDictionary(string dictionaryAssetName, object userData)
        {
            LoadDictionary(dictionaryAssetName, Constant.DefaultPriority, userData);
        }

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="priority">加载字典资源的优先级。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadDictionary(string dictionaryAssetName, int priority, object userData)
        {
            if (m_ResourceManager == null)
            {
                throw new GameFrameworkException("You must set resource manager first.");
            }

            if (m_LocalizationHelper == null)
            {
                throw new GameFrameworkException("You must set localization helper first.");
            }

            m_ResourceManager.LoadAsset(dictionaryAssetName, priority, m_LoadAssetCallbacks, userData);
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="text">要解析的字典文本。</param>
        /// <returns>是否解析字典成功。</returns>
        public bool ParseDictionary(string text)
        {
            return ParseDictionary(text, null);
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="text">要解析的字典文本。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析字典成功。</returns>
        public bool ParseDictionary(string text, object userData)
        {
            if (m_LocalizationHelper == null)
            {
                throw new GameFrameworkException("You must set localization helper first.");
            }

            return m_LocalizationHelper.ParseDictionary(text, userData);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="args">字典参数。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString(string key, params object[] args)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new GameFrameworkException("Key is invalid.");
            }

            string value = null;
            if (!m_Dictionary.TryGetValue(key, out value))
            {
                return string.Format("<NoKey>{0}", key);
            }

            try
            {
                return string.Format(value, args);
            }
            catch (Exception exception)
            {
                string errorString = string.Format("<Error>{0},{1}", key, value);
                if (args != null)
                {
                    foreach (object arg in args)
                    {
                        errorString += "," + arg.ToString();
                    }
                }

                errorString += "," + exception.Message;
                return errorString;
            }
        }

        /// <summary>
        /// 是否存在字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>是否存在字典。</returns>
        public bool HasRawString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new GameFrameworkException("Key is invalid.");
            }

            return m_Dictionary.ContainsKey(key);
        }

        /// <summary>
        /// 根据字典主键获取字典值。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>字典值。</returns>
        public string GetRawString(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new GameFrameworkException("Key is invalid.");
            }

            string value = null;
            if (m_Dictionary.TryGetValue(key, out value))
            {
                return value;
            }

            return string.Format("<NoKey>{0}", key);
        }

        /// <summary>
        /// 增加字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="value">字典内容。</param>
        /// <returns>是否增加字典成功。</returns>
        public bool AddRawString(string key, string value)
        {
            if (HasRawString(key))
            {
                return false;
            }

            m_Dictionary.Add(key, value ?? string.Empty);
            return true;
        }

        /// <summary>
        /// 移除字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>是否移除字典成功。</returns>
        public bool RemoveRawString(string key)
        {
            if (!HasRawString(key))
            {
                return false;
            }

            return m_Dictionary.Remove(key);
        }

        private void LoadDictionarySuccessCallback(string dictionaryAssetName, object dictionaryAsset, float duration, object userData)
        {
            try
            {
                if (!m_LocalizationHelper.LoadDictionary(dictionaryAsset, userData))
                {
                    throw new GameFrameworkException(string.Format("Load dictionary failure in helper, asset name '{0}'.", dictionaryAssetName));
                }
            }
            catch (Exception exception)
            {
                if (m_LoadDictionaryFailureEventHandler != null)
                {
                    m_LoadDictionaryFailureEventHandler(this, new LoadDictionaryFailureEventArgs(dictionaryAssetName, exception.ToString(), userData));
                    return;
                }

                throw;
            }
            finally
            {
                m_LocalizationHelper.ReleaseDictionaryAsset(dictionaryAsset);
            }

            if (m_LoadDictionarySuccessEventHandler != null)
            {
                m_LoadDictionarySuccessEventHandler(this, new LoadDictionarySuccessEventArgs(dictionaryAssetName, duration, userData));
            }
        }

        private void LoadDictionaryFailureCallback(string dictionaryAssetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            string appendErrorMessage = string.Format("Load dictionary failure, asset name '{0}', status '{1}', error message '{2}'.", dictionaryAssetName, status.ToString(), errorMessage);
            if (m_LoadDictionaryFailureEventHandler != null)
            {
                m_LoadDictionaryFailureEventHandler(this, new LoadDictionaryFailureEventArgs(dictionaryAssetName, appendErrorMessage, userData));
                return;
            }

            throw new GameFrameworkException(appendErrorMessage);
        }

        private void LoadDictionaryUpdateCallback(string dictionaryAssetName, float progress, object userData)
        {
            if (m_LoadDictionaryUpdateEventHandler != null)
            {
                m_LoadDictionaryUpdateEventHandler(this, new LoadDictionaryUpdateEventArgs(dictionaryAssetName, progress, userData));
            }
        }

        private void LoadDictionaryDependencyAssetCallback(string dictionaryAssetName, string dependencyAssetName, int loadedCount, int totalCount, object userData)
        {
            if (m_LoadDictionaryDependencyAssetEventHandler != null)
            {
                m_LoadDictionaryDependencyAssetEventHandler(this, new LoadDictionaryDependencyAssetEventArgs(dictionaryAssetName, dependencyAssetName, loadedCount, totalCount, userData));
            }
        }
    }
}
