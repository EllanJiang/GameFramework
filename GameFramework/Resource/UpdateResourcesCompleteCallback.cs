//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 使用可更新模式并更新资源全部完成的回调函数。
    /// </summary>
    /// <param name="result">更新资源结果，全部成功为 true，否则为 false。</param>
    public delegate void UpdateResourcesCompleteCallback(bool result);
}
