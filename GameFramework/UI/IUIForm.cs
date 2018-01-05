//------------------------------------------------------------
// Game Framework v3.x
// Copyright © 2013-2018 Jiang Yin. All rights reserved.
// Homepage: http://gameframework.cn/
// Feedback: mailto:jiangyin@gameframework.cn
//------------------------------------------------------------

namespace GameFramework.UI
{
    /// <summary>
    /// 界面接口。
    /// </summary>
    public interface IUIForm
    {
        /// <summary>
        /// 获取界面序列编号。
        /// </summary>
        int SerialId
        {
            get;
        }

        /// <summary>
        /// 获取界面资源名称。
        /// </summary>
        string UIFormAssetName
        {
            get;
        }

        /// <summary>
        /// 获取界面实例。
        /// </summary>
        object Handle
        {
            get;
        }

        /// <summary>
        /// 获取界面所属的界面组。
        /// </summary>
        IUIGroup UIGroup
        {
            get;
        }

        /// <summary>
        /// 获取界面在界面组中的深度。
        /// </summary>
        int DepthInUIGroup
        {
            get;
        }

        /// <summary>
        /// 获取是否暂停被覆盖的界面。
        /// </summary>
        bool PauseCoveredUIForm
        {
            get;
        }

        /// <summary>
        /// 初始化界面。
        /// </summary>
        /// <param name="serialId">界面序列编号。</param>
        /// <param name="uiFormAssetName">界面资源名称。</param>
        /// <param name="uiGroup">界面所属的界面组。</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的界面。</param>
        /// <param name="isNewInstance">是否是新实例。</param>
        /// <param name="userData">用户自定义数据。</param>
        void OnInit(int serialId, string uiFormAssetName, IUIGroup uiGroup, bool pauseCoveredUIForm, bool isNewInstance, object userData);

        /// <summary>
        /// 界面回收。
        /// </summary>
        void OnRecycle();

        /// <summary>
        /// 界面打开。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        void OnOpen(object userData);

        /// <summary>
        /// 界面关闭。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        void OnClose(object userData);

        /// <summary>
        /// 界面暂停。
        /// </summary>
        void OnPause();

        /// <summary>
        /// 界面暂停恢复。
        /// </summary>
        void OnResume();

        /// <summary>
        /// 界面遮挡。
        /// </summary>
        void OnCover();

        /// <summary>
        /// 界面遮挡恢复。
        /// </summary>
        void OnReveal();

        /// <summary>
        /// 界面激活。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        void OnRefocus(object userData);

        /// <summary>
        /// 界面轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        void OnUpdate(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 界面深度改变。
        /// </summary>
        /// <param name="uiGroupDepth">界面组深度。</param>
        /// <param name="depthInUIGroup">界面在界面组中的深度。</param>
        void OnDepthChanged(int uiGroupDepth, int depthInUIGroup);
    }
}
