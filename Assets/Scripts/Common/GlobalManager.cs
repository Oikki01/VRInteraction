using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//========================================
//作    者:HJK
//创建时间:2025.12.22
//备    注:全局管理类
//========================================
public class GlobalManager
{
    private static GlobalManager m_Instancce;

    public static GlobalManager Instance
    {
        get
        {
            if (m_Instancce == null)
            {
                m_Instancce = new GlobalManager();
            }
            return m_Instancce;
        }
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public OperationType OperationType = OperationType.None;

    /// <summary>
    /// 工具操作类型
    /// </summary>
    public ToolInteratType ToolInteractionType = ToolInteratType.None;
}
