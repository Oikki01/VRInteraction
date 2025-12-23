using System;
using UnityEngine;

/// <summary>
/// Author:lidongwei
/// Create Data:2022.08.25
/// Description:项目 单例
/// </summary>
public static class VEMFacade
{
    #region 外部配置路径

    /// <summary>
    /// 软件配置路径
    /// </summary>
    public static string AppPropertyPath = VEMSimPlatformPath + "/UserConfig/AppProperty.config";

    /// <summary>
    /// 视频临时缓存目录
    /// </summary>
    public static string TempVideoPath = VEMSimPlatformPath + "/TempVideo";

    /// <summary>
    /// 本地登录用户配置Excel表
    /// </summary>
    public static string UserExcelPathPath = VEMSimPlatformPath + "/SystemData/UserData.xlsx";

    /// <summary>
    /// 理论试题配置Excel
    /// </summary>
    public static string TheoryDatabasePath = VEMSimPlatformPath + "/UserConfig/TheoryDatabase.xlsx";

    /// <summary>
    /// 
    /// </summary>
    public static string LoginUrl = "http://127.0.0.1:8080/login";
    
    /// <summary>
    /// 
    /// </summary>
    public static string LogoutUrl = "http://127.0.0.1:8080/logout";
    
    /// <summary>
    /// 
    /// </summary>
    public static string SubjectTreeUrl = "http://127.0.0.1:8080/subject/permissions/getUserSubjectTree/";
    
    /// <summary>
    /// 
    /// </summary>
    public static string TrainRecordUrl = "http://127.0.0.1:8080/train/record";
    
    /// <summary>
    /// 
    /// </summary>
    public static string ExamineRecordUrl = "http://127.0.0.1:8080/examine/record";

    /// <summary>
    /// 
    /// </summary>
    public static string RegisterUrl;

    #endregion

    /// <summary>
    /// 当前训练模式
    /// </summary>
    public static TrainType CurTrainType = TrainType.Teach;

    /// <summary>
    /// 当前科目类型
    /// </summary>
    public static SubjectType CurSubjectType = SubjectType.StructureCognition;

    /// <summary>
    /// 当前训练科目名称
    /// </summary>
    public static string CurSubjectFolder;

    /// <summary>
    /// token
    /// </summary>
    public static string Token;
    
    /// <summary>
    /// 用户ID
    /// </summary>
    public static Role CurRole;
    
    /// <summary>
    /// 用户ID
    /// </summary>
    public static int UserId;
    
    /// <summary>
    /// 用户ID
    /// </summary>
    public static string UserName;

    /// <summary>
    /// VEMSim路径
    /// </summary>
    public static string VEMSimPlatformPath
    {
        get
        {
            if (string.IsNullOrEmpty(m_VEMSimPlatformPath))
            {
                if (Application.isEditor)
                {
                    m_VEMSimPlatformPath = Application.dataPath.Replace("Assets", "") + "BattleSim/VEMSimPlatform";
                }
                else
                {
                    m_VEMSimPlatformPath = System.Environment.CurrentDirectory + @"\BattleSim/VEMSimPlatform";
                    m_VEMSimPlatformPath = m_VEMSimPlatformPath.Replace('\\', '/');
                }
            }

            return m_VEMSimPlatformPath;
        }
    }

    /// <summary>
    /// VEMSim路径
    /// </summary>
    private static string m_VEMSimPlatformPath;
    
}

/// <summary>
/// 科目类型
/// </summary>
public enum SubjectType
{
    /// <summary>
    /// 结构认知
    /// </summary>
    [EnumMark("结构认知")]
    StructureCognition,
    
    /// <summary>
    /// 技术准备
    /// </summary>
    [EnumMark("技术准备")]
    TechPreparation,
    
    /// <summary>
    /// 操作使用
    /// </summary>
    [EnumMark("操作使用")]
    Simulation
}

/// <summary>
/// 训练类型
/// </summary>
public enum TrainType
{
    /// <summary>
    /// 预览模式 教室端
    /// </summary>
    Inspect,
    
    /// <summary>
    /// 教学模式
    /// </summary>
    Teach,

    /// <summary>
    /// 训练模式
    /// </summary>
    Train,

    /// <summary>
    /// 考核模式
    /// </summary>
    Exam,
}

public enum Role
{
    /// <summary>
    /// 学生
    /// </summary>
    [EnumMark("学生")]
    Student,
    
    /// <summary>
    /// 教师
    /// </summary>
    [EnumMark("教师")]
    Teacher
}