//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2020 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 加载数据流成功回调函数。
    /// </summary>
    /// <param name="fileUri">文件路径。</param>
    /// <param name="bytes">数据流。</param>
    /// <param name="duration">加载持续时间。</param>
    /// <param name="userData">用户自定义数据。</param>
    public delegate void LoadBytesSuccessCallback(string fileUri, byte[] bytes, float duration, object userData);
}
