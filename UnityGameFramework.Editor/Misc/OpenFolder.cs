//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor
{
    /// <summary>
    /// 打开文件夹相关的实用函数。
    /// </summary>
    internal static class OpenFolder
    {
        /// <summary>
        /// 打开 Temporary Cache Path 文件夹。
        /// </summary>
        [MenuItem("Game Framework/Open Folder/Temporary Cache Path", false, 10)]
        private static void OpenFolderTemporaryCachePath()
        {
            InternalOpenFolder(Application.temporaryCachePath);
        }

        /// <summary>
        /// 打开 Persistent Data Path 文件夹。
        /// </summary>
        [MenuItem("Game Framework/Open Folder/Persistent Data Path", false, 11)]
        private static void OpenFolderPersistentDataPath()
        {
            InternalOpenFolder(Application.persistentDataPath);
        }

        /// <summary>
        /// 打开 Streaming Assets Path 文件夹。
        /// </summary>
        [MenuItem("Game Framework/Open Folder/Streaming Assets Path", false, 12)]
        private static void OpenFolderStreamingAssetsPath()
        {
            InternalOpenFolder(Application.streamingAssetsPath);
        }

        /// <summary>
        /// 打开 Data Path 文件夹。
        /// </summary>
        [MenuItem("Game Framework/Open Folder/Data Path", false, 13)]
        private static void OpenFolderDataPath()
        {
            InternalOpenFolder(Application.dataPath);
        }

        private static void InternalOpenFolder(string folder)
        {
            folder = string.Format("\"{0}\"", folder);
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    Process.Start("Explorer.exe", folder.Replace('/', '\\'));
                    break;
                case RuntimePlatform.OSXEditor:
                    Process.Start("open", folder);
                    break;
                default:
                    throw new GameFrameworkException(string.Format("Not support open folder on '{0}' platform.", Application.platform.ToString()));
            }
        }
    }
}
