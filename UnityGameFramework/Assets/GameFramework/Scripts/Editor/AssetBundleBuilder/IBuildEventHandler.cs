//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2017 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

using UnityEditor;

namespace UnityGameFramework.Editor.AssetBundleTools
{
    /// <summary>
    /// 生成资源包事件处理函数。
    /// </summary>
    public interface IBuildEventHandler
    {
        /// <summary>
        /// 所有生成开始前的预处理事件。
        /// </summary>
        /// <param name="productName">产品名称。</param>
        /// <param name="companyName">公司名称。</param>
        /// <param name="gameIdentifier">游戏识别号。</param>
        /// <param name="applicableGameVersion">适用游戏版本。</param>
        /// <param name="internalResourceVersion">内部资源版本。</param>
        /// <param name="unityVersion">Unity 版本。</param>
        /// <param name="buildOptions">生成选项。</param>
        /// <param name="zip">是否压缩。</param>
        /// <param name="outputDirectory">生成目录。</param>
        /// <param name="workingPath">生成时的工作路径。</param>
        /// <param name="outputPackagePath">为单机模式生成的文件存放于此路径。若游戏是单机游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>
        /// <param name="outputFullPath">为可更新模式生成的所有文件存放于此路径。若游戏是网络游戏，生成结束后应将此目录上传至 Web 服务器，供玩家下载用。</param>
        /// <param name="outputPackedPath">为可更新模式生成的文件存放于此路径。若游戏是网络游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>
        /// <param name="buildReportPath">生成报告路径。</param>
        void PreProcessBuildAll(string productName, string companyName, string gameIdentifier,
            string applicableGameVersion, int internalResourceVersion, string unityVersion, BuildAssetBundleOptions buildOptions, bool zip,
            string outputDirectory, string workingPath, string outputPackagePath, string outputFullPath, string outputPackedPath, string buildReportPath);

        /// <summary>
        /// 所有生成结束后的后处理事件。
        /// </summary>
        /// <param name="productName">产品名称。</param>
        /// <param name="companyName">公司名称。</param>
        /// <param name="gameIdentifier">游戏识别号。</param>
        /// <param name="applicableGameVersion">适用游戏版本。</param>
        /// <param name="internalResourceVersion">内部资源版本。</param>
        /// <param name="unityVersion">Unity 版本。</param>
        /// <param name="buildOptions">生成选项。</param>
        /// <param name="zip">是否压缩。</param>
        /// <param name="outputDirectory">生成目录。</param>
        /// <param name="workingPath">生成时的工作路径。</param>
        /// <param name="outputPackagePath">为单机模式生成的文件存放于此路径。若游戏是单机游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>
        /// <param name="outputFullPath">为可更新模式生成的所有文件存放于此路径。若游戏是网络游戏，生成结束后应将此目录上传至 Web 服务器，供玩家下载用。</param>
        /// <param name="outputPackedPath">为可更新模式生成的文件存放于此路径。若游戏是网络游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>
        /// <param name="buildReportPath">生成报告路径。</param>
        void PostProcessBuildAll(string productName, string companyName, string gameIdentifier,
            string applicableGameVersion, int internalResourceVersion, string unityVersion, BuildAssetBundleOptions buildOptions, bool zip,
            string outputDirectory, string workingPath, string outputPackagePath, string outputFullPath, string outputPackedPath, string buildReportPath);

        /// <summary>
        /// 生成某个平台开始前的预处理事件。
        /// </summary>
        /// <param name="buildTarget">生成平台。</param>
        /// <param name="workingPath">生成时的工作路径。</param>
        /// <param name="outputPackagePath">为单机模式生成的文件存放于此路径。若游戏是单机游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>
        /// <param name="outputFullPath">为可更新模式生成的所有文件存放于此路径。若游戏是网络游戏，生成结束后应将此目录上传至 Web 服务器，供玩家下载用。</param>
        /// <param name="outputPackedPath">为可更新模式生成的文件存放于此路径。若游戏是网络游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>
        void PreProcessBuild(BuildTarget buildTarget, string workingPath, string outputPackagePath, string outputFullPath, string outputPackedPath);

        /// <summary>
        /// 生成某个平台结束后的后处理事件。
        /// </summary>
        /// <param name="buildTarget">生成平台。</param>
        /// <param name="workingPath">生成时的工作路径。</param>
        /// <param name="outputPackagePath">为单机模式生成的文件存放于此路径。若游戏是单机游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>
        /// <param name="outputFullPath">为可更新模式生成的所有文件存放于此路径。若游戏是网络游戏，生成结束后应将此目录上传至 Web 服务器，供玩家下载用。</param>
        /// <param name="outputPackedPath">为可更新模式生成的文件存放于此路径。若游戏是网络游戏，生成结束后将此目录中对应平台的文件拷贝至 StreamingAssets 后打包 App 即可。</param>
        void PostProcessBuild(BuildTarget buildTarget, string workingPath, string outputPackagePath, string outputFullPath, string outputPackedPath);
    }
}
