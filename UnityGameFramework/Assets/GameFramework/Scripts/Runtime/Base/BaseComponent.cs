//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using GameFramework.Localization;
using GameFramework.Resource;
using System;
using UnityEngine;

namespace UnityGameFramework.Runtime
{
    /// <summary>
    /// 基础组件。
    /// </summary>
    [AddComponentMenu("Game Framework/Base")]
    public sealed class BaseComponent : GameFrameworkComponent
    {
        private const int DefaultDpi = 96;  // default windows dpi

        private string m_GameVersion = string.Empty;
        private int m_InternalApplicationVersion = 0;
        private float m_GameSpeedBeforePause = 1f;

        [SerializeField]
        private bool m_EditorResourceMode = true;

        [SerializeField]
        private Language m_EditorLanguage = Language.Unspecified;

        [SerializeField]
        private string m_ZipHelperTypeName = "Utility.ZipHelper";

        [SerializeField]
        private string m_JsonHelperTypeName = "Utility.JsonHelper";

        [SerializeField]
        private int m_FrameRate = 30;

        [SerializeField]
        private float m_GameSpeed = 1f;

        [SerializeField]
        private bool m_RunInBackground = true;

        [SerializeField]
        private bool m_NeverSleep = true;

        /// <summary>
        /// 获取或设置游戏版本号。
        /// </summary>
        public string GameVersion
        {
            get
            {
                return m_GameVersion;
            }
            set
            {
                m_GameVersion = value;
            }
        }

        /// <summary>
        /// 获取或设置应用程序内部版本号。
        /// </summary>
        public int InternalApplicationVersion
        {
            get
            {
                return m_InternalApplicationVersion;
            }
            set
            {
                m_InternalApplicationVersion = value;
            }
        }

        /// <summary>
        /// 获取或设置是否使用编辑器资源模式（仅编辑器内有效）。
        /// </summary>
        public bool EditorResourceMode
        {
            get
            {
                return m_EditorResourceMode;
            }
            set
            {
                m_EditorResourceMode = value;
            }
        }

        /// <summary>
        /// 获取或设置编辑器语言（仅编辑器内有效）。
        /// </summary>
        public Language EditorLanguage
        {
            get
            {
                return m_EditorLanguage;
            }
            set
            {
                m_EditorLanguage = value;
            }
        }

        /// <summary>
        /// 获取或设置编辑器资源辅助器。
        /// </summary>
        public IResourceManager EditorResourceHelper
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置游戏帧率。
        /// </summary>
        public int FrameRate
        {
            get
            {
                return m_FrameRate;
            }
            set
            {
                Application.targetFrameRate = m_FrameRate = value;
            }
        }

        /// <summary>
        /// 获取或设置游戏速度。
        /// </summary>
        public float GameSpeed
        {
            get
            {
                return m_GameSpeed;
            }
            set
            {
                Time.timeScale = m_GameSpeed = (value >= 0f ? value : 0f);
            }
        }

        /// <summary>
        /// 获取游戏是否暂停。
        /// </summary>
        public bool IsGamePaused
        {
            get
            {
                return m_GameSpeed <= 0f;
            }
        }

        /// <summary>
        /// 获取是否正常游戏速度。
        /// </summary>
        public bool IsNormalGameSpeed
        {
            get
            {
                return m_GameSpeed == 1f;
            }
        }

        /// <summary>
        /// 获取或设置是否允许后台运行。
        /// </summary>
        public bool RunInBackground
        {
            get
            {
                return m_RunInBackground;
            }
            set
            {
                Application.runInBackground = m_RunInBackground = value;
            }
        }

        /// <summary>
        /// 获取或设置是否禁止休眠。
        /// </summary>
        public bool NeverSleep
        {
            get
            {
                return m_NeverSleep;
            }
            set
            {
                m_NeverSleep = value;
                Screen.sleepTimeout = value ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
            }
        }

        /// <summary>
        /// 游戏框架组件初始化。
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            Log.SetLogCallback(LogCallback);

            Log.Info("Game Framework version is {0}. Unity Game Framework version is {1}.", GameFrameworkEntry.Version, GameEntry.Version);

#if UNITY_5_3_OR_NEWER || UNITY_5_3
            InitZipHelper();
            InitJsonHelper();

            Utility.Converter.ScreenDpi = Screen.dpi;
            if (Utility.Converter.ScreenDpi <= 0)
            {
                Utility.Converter.ScreenDpi = DefaultDpi;
            }

            m_EditorResourceMode &= Application.isEditor;
            if (m_EditorResourceMode)
            {
                Log.Info("During this run, Game Framework will use editor resource files, which you should validate first.");
            }

            Application.targetFrameRate = m_FrameRate;
            Time.timeScale = m_GameSpeed;
            Application.runInBackground = m_RunInBackground;
            Screen.sleepTimeout = m_NeverSleep ? SleepTimeout.NeverSleep : SleepTimeout.SystemSetting;
#else
            Log.Error("Game Framework only applies with Unity 5.3 and above, but current Unity version is {0}.", Application.unityVersion);
            GameEntry.Shutdown(ShutdownType.Quit);
#endif
#if UNITY_5_6_OR_NEWER
            Application.lowMemory += OnLowMemory;
#endif
        }

        private void Start()
        {

        }

        private void Update()
        {
            GameFrameworkEntry.Update(Time.deltaTime, Time.unscaledDeltaTime);
        }

        /// <summary>
        /// 暂停游戏。
        /// </summary>
        public void PauseGame()
        {
            if (IsGamePaused)
            {
                return;
            }

            m_GameSpeedBeforePause = GameSpeed;
            GameSpeed = 0f;
        }

        /// <summary>
        /// 恢复游戏。
        /// </summary>
        public void ResumeGame()
        {
            if (!IsGamePaused)
            {
                return;
            }

            GameSpeed = m_GameSpeedBeforePause;
        }

        /// <summary>
        /// 重置为正常游戏速度。
        /// </summary>
        public void ResetNormalGameSpeed()
        {
            if (IsNormalGameSpeed)
            {
                return;
            }

            GameSpeed = 1f;
        }

        internal void Shutdown()
        {
#if UNITY_5_6_OR_NEWER
            Application.lowMemory -= OnLowMemory;
#endif
            GameFrameworkEntry.Shutdown();
            Destroy(gameObject);
        }

        private void InitZipHelper()
        {
            Type zipHelperType = Utility.Assembly.GetTypeWithinLoadedAssemblies(m_ZipHelperTypeName);
            if (zipHelperType == null)
            {
                Log.Error("Can not find Zip helper type '{0}'.", m_ZipHelperTypeName);
                return;
            }

            Utility.Zip.IZipHelper zipHelper = (Utility.Zip.IZipHelper)Activator.CreateInstance(zipHelperType);
            if (zipHelper == null)
            {
                Log.Error("Can not create Zip helper instance '{0}'.", m_ZipHelperTypeName);
                return;
            }

            Utility.Zip.SetZipHelper(zipHelper);
        }

        private void InitJsonHelper()
        {
            Type jsonHelperType = Utility.Assembly.GetTypeWithinLoadedAssemblies(m_JsonHelperTypeName);
            if (jsonHelperType == null)
            {
                Log.Error("Can not find Json helper type '{0}'.", m_JsonHelperTypeName);
                return;
            }

            Utility.Json.IJsonHelper jsonHelper = (Utility.Json.IJsonHelper)Activator.CreateInstance(jsonHelperType);
            if (jsonHelper == null)
            {
                Log.Error("Can not create Json helper instance '{0}'.", m_JsonHelperTypeName);
                return;
            }

            Utility.Json.SetJsonHelper(jsonHelper);
        }

        private void LogCallback(LogLevel level, object message)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    Debug.Log(string.Format("<color=#888888>{0}</color>", message.ToString()));
                    break;
                case LogLevel.Info:
                    Debug.Log(message.ToString());
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(message.ToString());
                    break;
                case LogLevel.Error:
                    Debug.LogError(message.ToString());
                    break;
                default:
                    throw new GameFrameworkException(message.ToString());
            }
        }

        private void OnLowMemory()
        {
            Log.Info("Low memory reported...");

            ObjectPoolComponent objectPoolComponent = GameEntry.GetComponent<ObjectPoolComponent>();
            if (objectPoolComponent != null)
            {
                objectPoolComponent.ReleaseAllUnused();
            }

            ResourceComponent resourceCompoent = GameEntry.GetComponent<ResourceComponent>();
            if (resourceCompoent != null)
            {
                resourceCompoent.ForceUnloadUnusedAssets(true);
            }
        }
    }
}
