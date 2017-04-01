//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEditor;

namespace UnityGameFramework.Editor
{
    /// <summary>
    /// 上下文相关的实用函数。
    /// </summary>
    internal static class ContextMenu
    {
        [MenuItem("CONTEXT/BaseComponent/Help")]
        private static void ShowBaseComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("base");
        }

        [MenuItem("CONTEXT/DataNodeComponent/Help")]
        private static void ShowDataNodeComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("datanode");
        }

        [MenuItem("CONTEXT/DataTableComponent/Help")]
        private static void ShowDataTableComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("datatable");
        }

        [MenuItem("CONTEXT/DebuggerComponent/Help")]
        private static void ShowDebuggerComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("debugger");
        }

        [MenuItem("CONTEXT/DownloadComponent/Help")]
        private static void ShowDownloadComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("download");
        }

        [MenuItem("CONTEXT/EntityComponent/Help")]
        private static void ShowEntityComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("entity");
        }

        [MenuItem("CONTEXT/EventComponent/Help")]
        private static void ShowEventComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("event");
        }

        [MenuItem("CONTEXT/FsmComponent/Help")]
        private static void ShowFsmComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("fsm");
        }

        [MenuItem("CONTEXT/LocalizationComponent/Help")]
        private static void ShowLocalizationComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("localization");
        }

        [MenuItem("CONTEXT/NetworkComponent/Help")]
        private static void ShowNetworkComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("network");
        }

        [MenuItem("CONTEXT/ObjectPoolComponent/Help")]
        private static void ShowObjectPoolComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("objectpool");
        }

        [MenuItem("CONTEXT/ProcedureComponent/Help")]
        private static void ShowProcedureComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("procedure");
        }

        [MenuItem("CONTEXT/ResourceComponent/Help")]
        private static void ShowResourceComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("resource");
        }

        [MenuItem("CONTEXT/SceneComponent/Help")]
        private static void ShowSceneComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("scene");
        }

        [MenuItem("CONTEXT/SettingComponent/Help")]
        private static void ShowSettingComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("setting");
        }

        [MenuItem("CONTEXT/SoundComponent/Help")]
        private static void ShowSoundComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("sound");
        }

        [MenuItem("CONTEXT/UIComponent/Help")]
        private static void ShowUIComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("ui");
        }

        [MenuItem("CONTEXT/WebRequestComponent/Help")]
        private static void ShowWebRequestComponentHelp(MenuCommand command)
        {
            Help.ShowComponentHelp("webrequest");
        }
    }
}
