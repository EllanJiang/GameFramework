//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2019 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.Resource
{
    /// <summary>
    /// 读取数据流回调函数。
    /// </summary>
    /// <param name="fileUri">文件路径。</param>
    /// <param name="bytes">数据流。</param>
    /// <param name="errorMessage">错误信息。</param>
    public delegate void LoadBytesCallback(string fileUri, byte[] bytes, string errorMessage);
}
