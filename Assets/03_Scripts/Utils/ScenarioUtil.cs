using System.Collections.Generic;

/// <summary>
/// Author:lidongwei
/// Create Date:2023.04.25
/// Description:想定工具类
/// </summary>
public class ScenarioUtil
{
    /// <summary>
    /// 获取想定步骤数据
    /// </summary>
    /// <param name="scenarioData"></param>
    /// <param name="sortStepList"></param>
    /// <param name="stepCount"></param>
    /// <param name="scoreCount"></param>
    public static void GetScenarionStepInfo(List<ScenarioJson> scenarioDataList, out List<FlowDataBase> sortStepList, out int stepCount)
    {
        //整理顺序后的步骤列表
        sortStepList = new List<FlowDataBase>();
        //步骤数
        stepCount = 0;

        for (int i = 0; i < scenarioDataList.Count; i++)
        {
            ScenarioJson scenarioData = scenarioDataList[i];
            //获取起点
            FlowDataBase curStep = scenarioData.Step.Find(x => x.control == ControlType.Begin);
            //执行递归
            AA(scenarioData.Step, curStep, ref sortStepList, ref stepCount);
        }

        // //步骤对应的得分字典
        // stepScoreDict = new Dictionary<int, float>();
        // stepScoreDict.Add(0, 0);
        // for (int i = 1; i <= stepCount; i++)
        // {
        //     float curStepScore = 100.0f / stepCount * i; //百分制
        //     stepScoreDict.Add(i, curStepScore);
        // }
    }

    /// <summary>
    /// 递归查询
    /// </summary>
    /// <param name="stepList"></param>
    /// <param name="curStep"></param>
    /// <param name="sortStepList"></param>
    /// <param name="stepCount"></param>
    /// <param name="scoreOunt"></param>
    /// <param name="stepScoreDict"></param>
    private static void AA(List<FlowDataBase> stepList, FlowDataBase curStep, ref List<FlowDataBase> sortStepList, ref int stepCount)
    {
        BB(curStep, ref sortStepList, ref stepCount);
        if (curStep.NextStepList == null || curStep.NextStepList.Count == 0)
        {
            //无数据
            return;
        }

        //得到下一步骤
        FlowDataBase nextStepData = GetProcessData(stepList, curStep.NextStepList[0]);
        if (nextStepData == null || nextStepData.control == ControlType.End)
        {
            //结束
            return;
        }

        AA(stepList, nextStepData, ref sortStepList, ref stepCount);
    }

    /// <summary>
    /// 数据添加
    /// </summary>
    /// <param name="flowDataBase"></param>
    /// <param name="sortStepList"></param>
    /// <param name="stepCount"></param>
    /// <param name="scoreCount"></param>
    /// <param name="stepScoreDict"></param>
    private static void BB(FlowDataBase flowDataBase, ref List<FlowDataBase> sortStepList, ref int stepCount)
    {
        if (flowDataBase.control == ControlType.SimulationStep)
        {
            //操作步骤
            // if (flowDataBase.StepOperateType == StepOperateType.Simulation)
            {
                stepCount++;
                sortStepList.Add(flowDataBase);
            }
        }
        else if (flowDataBase.control == ControlType.TheoryStep)
        {
            //理论实体步骤
            stepCount++;
            sortStepList.Add(flowDataBase);
        }
    }

    /// <summary>
    /// 根据步骤ID 获取步骤
    /// </summary>
    /// <param name="stepDataList"></param>
    /// <param name="stepId"></param>
    /// <returns></returns>
    private static FlowDataBase GetProcessData(List<FlowDataBase> stepDataList, int stepId)
    {
        FlowDataBase stateMachineData = stepDataList.Find(x => x.StepId == stepId);
        if (stateMachineData == null)
        {
            return null;
        }
        else
        {
            return stateMachineData;
        }
    }
}