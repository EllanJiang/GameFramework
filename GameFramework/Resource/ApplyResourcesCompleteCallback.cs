//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 使用可更新模式并应用资源包资源完成时的回调函数。
    /// </summary>
    /// <param name="resourcePackPath">应用的资源包路径。</param>
    /// <param name="result">应用资源包资源结果，全部成功为 true，否则为 false。</param>
    public delegate void ApplyResourcesCompleteCallback(string resourcePackPath, bool result);
}
