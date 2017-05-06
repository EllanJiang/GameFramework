//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;

namespace UnityGameFramework.Editor
{
    /// <summary>
    /// 日志重定向相关的实用函数。
    /// </summary>
    internal static class LogRedirection
    {
        private static readonly Regex s_LogRegex = new Regex(@" \(at (.+)\:(\d+)\)\r?\n");

        [OnOpenAsset(0)]
        private static bool OnOpenAsset(int instanceId, int line)
        {
            string selectedStackTrace = GetSelectedStackTrace();
            if (string.IsNullOrEmpty(selectedStackTrace))
            {
                return false;
            }

            if (!selectedStackTrace.Contains("UnityGameFramework.Runtime.BaseComponent:LogCallback"))
            {
                return false;
            }

            Match match = s_LogRegex.Match(selectedStackTrace);
            if (!match.Success)
            {
                return false;
            }

            if (!match.Groups[1].Value.Contains("BaseComponent.cs"))
            {
                return false;
            }

            // 跳过第一次匹配的堆栈
            match = match.NextMatch();
            if (!match.Success)
            {
                return false;
            }

            if (match.Groups[1].Value.Contains("Log.cs"))
            {
                // 直接使用 GameFramework.dll 源码而非 dll 的工程会多一次匹配的堆栈
                match = match.NextMatch();
                if (!match.Success)
                {
                    return false;
                }
            }

            InternalEditorUtility.OpenFileAtLineExternal(Utility.Path.GetCombinePath(Application.dataPath, match.Groups[1].Value.Substring(7)), int.Parse(match.Groups[2].Value));
            return true;
        }

        private static string GetSelectedStackTrace()
        {
            Assembly editorWindowAssembly = typeof(EditorWindow).Assembly;
            if (editorWindowAssembly == null)
            {
                return null;
            }

            System.Type consoleWindowType = editorWindowAssembly.GetType("UnityEditor.ConsoleWindow");
            if (consoleWindowType == null)
            {
                return null;
            }

            FieldInfo consoleWindowFieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
            if (consoleWindowFieldInfo == null)
            {
                return null;
            }

            EditorWindow consoleWindow = consoleWindowFieldInfo.GetValue(null) as EditorWindow;
            if (consoleWindow == null)
            {
                return null;
            }

            if (consoleWindow != EditorWindow.focusedWindow)
            {
                return null;
            }

            FieldInfo activeTextFieldInfo = consoleWindowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
            if (activeTextFieldInfo == null)
            {
                return null;
            }

            return (string)activeTextFieldInfo.GetValue(consoleWindow);
        }
    }
}
