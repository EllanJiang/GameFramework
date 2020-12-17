//------------------------------------------------------------
// Game Framework
// Copyright © 2013-2021 Jiang Yin. All rights reserved.
// Homepage: https://gameframework.cn/
// Feedback: mailto:ellan@gameframework.cn
//------------------------------------------------------------

using GameFramework.Fsm;
using System;

namespace GameFramework.Procedure
{
    /// <summary>
    /// 流程管理器接口。
    /// </summary>
    public interface IProcedureManager
    {
        /// <summary>
        /// 获取当前流程。
        /// </summary>
        ProcedureBase CurrentProcedure
        {
            get;
        }

        /// <summary>
        /// 获取当前流程持续时间。
        /// </summary>
        float CurrentProcedureTime
        {
            get;
        }

        /// <summary>
        /// 初始化流程管理器。
        /// </summary>
        /// <param name="fsmManager">有限状态机管理器。</param>
        /// <param name="procedures">流程管理器包含的流程。</param>
        void Initialize(IFsmManager fsmManager, params ProcedureBase[] procedures);

        /// <summary>
        /// 开始流程。
        /// </summary>
        /// <typeparam name="T">要开始的流程类型。</typeparam>
        void StartProcedure<T>() where T : ProcedureBase;

        /// <summary>
        /// 开始流程。
        /// </summary>
        /// <param name="procedureType">要开始的流程类型。</param>
        void StartProcedure(Type procedureType);

        /// <summary>
        /// 是否存在流程。
        /// </summary>
        /// <typeparam name="T">要检查的流程类型。</typeparam>
        /// <returns>是否存在流程。</returns>
        bool HasProcedure<T>() where T : ProcedureBase;

        /// <summary>
        /// 是否存在流程。
        /// </summary>
        /// <param name="procedureType">要检查的流程类型。</param>
        /// <returns>是否存在流程。</returns>
        bool HasProcedure(Type procedureType);

        /// <summary>
        /// 获取流程。
        /// </summary>
        /// <typeparam name="T">要获取的流程类型。</typeparam>
        /// <returns>要获取的流程。</returns>
        ProcedureBase GetProcedure<T>() where T : ProcedureBase;

        /// <summary>
        /// 获取流程。
        /// </summary>
        /// <param name="procedureType">要获取的流程类型。</param>
        /// <returns>要获取的流程。</returns>
        ProcedureBase GetProcedure(Type procedureType);
    }
}
