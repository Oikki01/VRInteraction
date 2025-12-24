using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//========================================
//作    者:
//创建时间:
//备    注:
//========================================
public enum OperationType
{
    /// <summary>
    /// 无（UI及无工具情况下模型交互）
    /// </summary>
    None,
    /// <summary>
    /// 使用工具状态下的模型交互
    /// </summary>
    Tool,
}

/// <summary>
/// 工具交互类型
/// </summary>
public enum ToolInteratType
{
    None,
    /// <summary>
    /// 绘制
    /// </summary>
    Paint,
    /// <summary>
    /// 模型交互
    /// </summary>
    Interact
}


public enum MenuBtnType
{
    [EnumMark("步骤列表")] BtnStepList,

    [EnumMark("工具列表")] BtnToolList,

    [EnumMark("退出")] BtnExit,
}