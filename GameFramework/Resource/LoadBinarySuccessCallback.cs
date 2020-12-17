//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 加载二进制资源成功回调函数。
    /// </summary>
    /// <param name="binaryAssetName">要加载的二进制资源名称。</param>
    /// <param name="binaryBytes">已加载的二进制资源。</param>
    /// <param name="duration">加载持续时间。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void LoadBinarySuccessCallback(string binaryAssetName, byte[] binaryBytes, float duration, object userData);
}
