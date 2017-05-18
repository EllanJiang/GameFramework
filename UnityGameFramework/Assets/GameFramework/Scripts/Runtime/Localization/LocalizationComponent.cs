//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Localization;
using GameFramework.Resource;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 本地化组件。
    /// </summary>
    [DisallowMultipleComponent]
    [AddComponentMenu("Game Framework/Localization")]
    public sealed class LocalizationComponent : GameFrameworkComponent
    {
        private ILocalizationManager m_LocalizationManager = null;
        private EventComponent m_EventComponent = null;

        [SerializeField]
        private bool m_EnableLoadDictionarySuccessEvent = true;

        [SerializeField]
        private bool m_EnableLoadDictionaryFailureEvent = true;

        [SerializeField]
        private bool m_EnableLoadDictionaryUpdateEvent = false;

        [SerializeField]
        private bool m_EnableLoadDictionaryDependencyAssetEvent = false;

        [SerializeField]
        private string m_LocalizationHelperTypeName = "UnityGameFramework.Runtime.DefaultLocalizationHelper";

        [SerializeField]
        private LocalizationHelperBase m_CustomLocalizationHelper = null;

        /// <summary>
        /// 获取或设置本地化语言。
        /// </summary>
        public Language Language
        {
            get
            {
                return m_LocalizationManager.Language;
            }
            set
            {
                m_LocalizationManager.Language = value;
            }
        }

        /// <summary>
        /// 获取系统语言。
        /// </summary>
        public Language SystemLanguage
        {
            get
            {
                return m_LocalizationManager.SystemLanguage;
            }
        }

        /// <summary>
        /// 获取字典条数。
        /// </summary>
        public int DictionaryCount
        {
            get
            {
                return m_LocalizationManager.DictionaryCount;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            m_LocalizationManager = GameFrameworkEntry.GetModule<ILocalizationManager>();
            if (m_LocalizationManager == null)
            {
                Log.Fatal("Localization manager is invalid.");
                return;
            }

            m_LocalizationManager.LoadDictionarySuccess += OnLoadDictionarySuccess;
            m_LocalizationManager.LoadDictionaryFailure += OnLoadDictionaryFailure;
            m_LocalizationManager.LoadDictionaryUpdate += OnLoadDictionaryUpdate;
            m_LocalizationManager.LoadDictionaryDependencyAsset += OnLoadDictionaryDependencyAsset;
        }

        private void Start()
        {
            BaseComponent baseComponent = GameEntry.GetComponent<BaseComponent>();
            if (baseComponent == null)
            {
                Log.Fatal("Base component is invalid.");
                return;
            }

            m_EventComponent = GameEntry.GetComponent<EventComponent>();
            if (m_EventComponent == null)
            {
                Log.Fatal("Event component is invalid.");
                return;
            }

            if (baseComponent.EditorResourceMode)
            {
                m_LocalizationManager.SetResourceManager(baseComponent.EditorResourceHelper);
            }
            else
            {
                m_LocalizationManager.SetResourceManager(GameFrameworkEntry.GetModule<IResourceManager>());
            }

            LocalizationHelperBase localizationHelper = Helper.CreateHelper(m_LocalizationHelperTypeName, m_CustomLocalizationHelper);
            if (localizationHelper == null)
            {
                Log.Error("Can not create localization helper.");
                return;
            }

            localizationHelper.name = string.Format("Localization Helper");
            Transform transform = localizationHelper.transform;
            transform.SetParent(this.transform);
            transform.localScale = Vector3.one;

            m_LocalizationManager.SetLocalizationHelper(localizationHelper);
            m_LocalizationManager.Language = (baseComponent.EditorResourceMode && baseComponent.EditorLanguage != Language.Unspecified ? baseComponent.EditorLanguage : m_LocalizationManager.SystemLanguage);
        }

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryName">字典名称。</param>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        public void LoadDictionary(string dictionaryName, string dictionaryAssetName)
        {
            LoadDictionary(dictionaryName, dictionaryAssetName, null);
        }

        /// <summary>
        /// 加载字典。
        /// </summary>
        /// <param name="dictionaryName">字典名称。</param>
        /// <param name="dictionaryAssetName">字典资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void LoadDictionary(string dictionaryName, string dictionaryAssetName, object userData)
        {
            if (string.IsNullOrEmpty(dictionaryName))
            {
                Log.Error("Dictionary name is invalid.");
                return;
            }

            m_LocalizationManager.LoadDictionary(dictionaryAssetName, new LoadDictionaryInfo(dictionaryName, userData));
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="text">要解析的字典文本。</param>
        /// <returns>是否解析字典成功。</returns>
        public bool ParseDictionary(string text)
        {
            return m_LocalizationManager.ParseDictionary(text);
        }

        /// <summary>
        /// 解析字典。
        /// </summary>
        /// <param name="text">要解析的字典文本。</param>
        /// <param name="userData">用户自定义数据。</param>
        /// <returns>是否解析字典成功。</returns>
        public bool ParseDictionary(string text, object userData)
        {
            return m_LocalizationManager.ParseDictionary(text, userData);
        }

        /// <summary>
        /// 根据字典主键获取字典内容字符串。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <param name="args">字典参数。</param>
        /// <returns>要获取的字典内容字符串。</returns>
        public string GetString(string key, params object[] args)
        {
            return m_LocalizationManager.GetString(key, args);
        }

        /// <summary>
        /// 是否存在字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>是否存在字典。</returns>
        public bool HasRawString(string key)
        {
            return m_LocalizationManager.HasRawString(key);
        }

        /// <summary>
        /// 根据字典主键获取字典值。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>字典值。</returns>
        public string GetRawString(string key)
        {
            return m_LocalizationManager.GetRawString(key);
        }

        /// <summary>
        /// 移除字典。
        /// </summary>
        /// <param name="key">字典主键。</param>
        /// <returns>是否移除字典成功。</returns>
        public bool RemoveRawString(string key)
        {
            return m_LocalizationManager.RemoveRawString(key);
        }

        private void OnLoadDictionarySuccess(object sender, GameFramework.Localization.LoadDictionarySuccessEventArgs e)
        {
            if (m_EnableLoadDictionarySuccessEvent)
            {
                m_EventComponent.Fire(this, new LoadDictionarySuccessEventArgs(e));
            }
        }

        private void OnLoadDictionaryFailure(object sender, GameFramework.Localization.LoadDictionaryFailureEventArgs e)
        {
            Log.Warning("Load dictionary failure, asset name '{0}', error message '{1}'.", e.DictionaryAssetName, e.ErrorMessage);
            if (m_EnableLoadDictionaryFailureEvent)
            {
                m_EventComponent.Fire(this, new LoadDictionaryFailureEventArgs(e));
            }
        }

        private void OnLoadDictionaryUpdate(object sender, GameFramework.Localization.LoadDictionaryUpdateEventArgs e)
        {
            if (m_EnableLoadDictionaryUpdateEvent)
            {
                m_EventComponent.Fire(this, new LoadDictionaryUpdateEventArgs(e));
            }
        }

        private void OnLoadDictionaryDependencyAsset(object sender, GameFramework.Localization.LoadDictionaryDependencyAssetEventArgs e)
        {
            if (m_EnableLoadDictionaryDependencyAssetEvent)
            {
                m_EventComponent.Fire(this, new LoadDictionaryDependencyAssetEventArgs(e));
            }
        }
    }
}
