//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using GameFramework;
using UnityEditor;
using UnityEngine;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    /// <summary>
    /// 生成资源包。
    /// </summary>
    internal sealed class BuildAssetBundle
    {
        /// <summary>
        /// 运行生成资源包。
        /// </summary>
        [MenuItem("Game Framework/AssetBundle Tools/Build AssetBundle", false, 30)]
        private static void Run()
        {
            AssetBundleBuilderController controller = new AssetBundleBuilderController();
            if (!controller.Load())
            {
                throw new GameFrameworkException("Load configuration failure.");
            }
            else
            {
                Debug.Log("Load configuration success.");
            }

            if (!controller.IsValidOutputDirectory)
            {
                throw new GameFrameworkException(string.Format("Output directory '{0}' is invalid.", controller.OutputDirectory));
            }

            if (!controller.BuildAssetBundles())
            {
                throw new GameFrameworkException("Build AssetBundles failure.");
            }
            else
            {
                Debug.Log("Build AssetBundles success.");
                controller.Save();
            }
        }
    }
}
