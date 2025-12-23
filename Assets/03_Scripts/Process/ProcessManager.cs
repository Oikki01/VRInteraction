using System.Collections.Generic;
using System.Linq;
using GameEvent;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 流程管理器
/// </summary>
public class ProcessManager : MonoSingleton<ProcessManager>
{
    #region 公有变量

    /// <summary>
    /// 流程数据
    /// </summary>
    public List<FlowDataBase> FlowDataList
    {
        get { return flowDataList; }
    }

    /// <summary>
    /// 当前步骤下标
    /// </summary>
    public int CurStepIndex { private set; get; }

    #endregion

    #region 私有变量

    /// <summary>
    /// 科目数据
    /// </summary>
    protected ScenarioJson scenarioJson;

    /// <summary>
    /// 流程数据列表
    /// </summary>
    protected List<FlowDataBase> flowDataList;

    /// <summary>
    /// 步骤总数
    /// </summary>
    protected int subjectStepCount;

    /// <summary>
    /// 表现
    /// </summary>
    protected CarryOutBase curCarryOut;

    /// <summary>
    /// 表现列表
    /// </summary>
    protected List<CarryOutBase> carryOutList = new List<CarryOutBase>();

    /// <summary>
    /// 工具字典
    /// </summary>
    private Dictionary<ToolInfoData, GameObject> toolGobjDict = new Dictionary<ToolInfoData, GameObject>();

    #endregion

    #region Mono

    private void OnEnable()
    {
        // GameEvent.GameEventCenter.OnModelNodeInteract.AddListener(OnModelNodeInteract);
    }

    private void OnDisable()
    {
        // GameEvent.GameEventCenter.OnModelNodeInteract.RemoveListener(OnModelNodeInteract);
    }

    private void Update()
    {
        #region pc端使用屏幕点击

        //判断点击的时自身ui
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (CurStepIndex >= flowDataList.Count)
            {
                Debug.Log("已全部完成");
                return;
            }

            bool isHitTrue = false;
            CarryOutBase carryOut = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hitArr = Physics.RaycastAll(ray, 100);
            for (int i = 0; i < hitArr.Length; i++)
            {
                Transform hitTrs = hitArr[i].collider.transform;
                List<CarryOutBase> carryOutList = hitTrs.GetComponents<CarryOutBase>().ToList();
                carryOut = carryOutList.Find((x) => x.StepID == flowDataList[CurStepIndex].StepId);
                if (carryOut != null)
                {
                    isHitTrue = true;
                    break;
                }
            }

            if (isHitTrue)
            {
                HitModel(isHitTrue, carryOut);
            }
            else
            {
                Debug.Log("点击错误 记录 扣分等");
            }
        }

        #endregion
    }

    #endregion

    #region 公有函数

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="subjectFolderPath"></param>
    public void Init(string subjectFolderPath)
    {
        //步骤数据
        scenarioJson = JsonFullSerializer.LoadJsonFile<ScenarioJson>(subjectFolderPath + "/StepData.json");
        ScenarioUtil.GetScenarionStepInfo(new List<ScenarioJson>() {scenarioJson}, out flowDataList, out subjectStepCount);

        CurStepIndex = 0;
        //创建工具
        CreateToolGobj();
        ModelNodePerformPrepare();
    }

    /// <summary>
    /// 重新开始
    /// </summary>
    public void RePlay()
    {
        CurStepIndex = 0;
        //模型归位
        for (int i = 0; i < carryOutList.Count; i++)
        {
            CarryOutBase carryOutBase = carryOutList[i];
            carryOutBase.ResetData(() =>
            {
                Destroy(carryOutBase);
            });
        }
        
        carryOutList.Clear();
    }

    /// <summary>
    /// 获取当前步骤数据
    /// </summary>
    /// <returns></returns>
    public FlowDataBase GetCurFlowDataBase()
    {
        if (CurStepIndex < flowDataList.Count)
        {
            return flowDataList[CurStepIndex];
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 获取当前表现节点
    /// </summary>
    /// <returns></returns>
    public CarryOutBase GetCurCarryOut()
    {
        return curCarryOut;
    }

    #endregion

    #region 私有函数

    /// <summary>
    /// 步骤完成
    /// </summary>
    private void StepComplete()
    {
        curCarryOut.HighLightOff(null);
        Destroy(curCarryOut);
        CurStepIndex++;
        GameEventCenter.OnTaskProgressChanged.Invoke();
        GetNextStep();
    }

    /// <summary>
    /// 下一步
    /// </summary>
    private void GetNextStep()
    {
        if (CurStepIndex < flowDataList.Count)
        {
            ModelNodePerformPrepare();
        }
        else
        {
            Debug.Log("步骤全部完成！");
            GameEventCenter.OnTaskCompleted.Invoke();
        }
    }

    /// <summary>
    /// 模型节点 交互 和表现准备
    /// </summary>
    /// <param name="stepId"></param>
    private void ModelNodePerformPrepare()
    {
        FlowDataBase flowData = flowDataList[CurStepIndex];
        GameObject gObj = GameObject.Find(flowData.ModelFullName);
        if (gObj == null)
        {
            Debug.Log("找不到模型节点：" + flowData.ModelFullName);
            return;
        }

        if (flowData.control == ControlType.SimulationStep)
        {
            PerformExpand.Bind(null, flowData, (action) =>
            {
                BindCallback(flowData, action);
                curCarryOut = action;
                //工具绑定脚本
                ToolInfoData toolInfoData = DataManager.Instance.GetToolInfoById(flowData.ToolId);
                if (toolInfoData != null && toolGobjDict.ContainsKey(toolInfoData))
                {
                    GameObject curToolGojb = toolGobjDict[toolInfoData];
                    curToolGojb.SetActive(true);

                    ToolCarryOutComponent toolCarryOutComponent =
                        (curToolGojb.GetOrAddComponent(VEMAssemblyManager.Instance.VEM_CarryOut_Assembly.GetType(toolInfoData.ClassFullName)) as ToolCarryOutComponent);
                    toolCarryOutComponent.ID = flowData.ToolId;
                    toolCarryOutComponent.BindPart(gObj.GetComponent<PartCarryOutComponent>());
                    toolCarryOutComponent.Init(null);
                    toolCarryOutComponent.AdsorbentCorrectionPosition(null);
                }
            });
        }

        if (curCarryOut)
        {
            curCarryOut.SetCollider(true, null);
            if (VEMFacade.CurTrainType == TrainType.Teach)
            {
                curCarryOut.HighLightFlash(Color.blue, Color.red, null);
            }
        }
    }

    /// <summary>
    /// 绑定回调
    /// </summary>
    /// <param name="flowData"></param>
    /// <param name="carryOut"></param>
    private void BindCallback(FlowDataBase flowData, CarryOutBase carryOut)
    {
        if (carryOut is PartCarryOutComponent)
        {
            carryOutList.Add(carryOut);
            (carryOut as PartCarryOutComponent).StepID = flowData.StepId;
            (carryOut as PartCarryOutComponent).Init(null);
            PartInfoData infoData = DataManager.Instance.GetPartInfoByClassType(carryOut.perform.GetType().ToString());
            carryOut.ID = infoData.Id;
            (carryOut as PartCarryOutComponent).ToolOffsetPos = infoData.PartOffsetPos;
            (carryOut as PartCarryOutComponent).ToolOffsetEuler = infoData.PartDirection;
            (carryOut as PartCarryOutComponent).OutLine = flowData.OutLineColor;
        }
        else if (carryOut is SimulationCarryOutComponent)
        {
            carryOutList.Add(carryOut);
            (carryOut as SimulationCarryOutComponent).StepID = flowData.StepId;
            (carryOut as SimulationCarryOutComponent).Init(null);
            (carryOut as SimulationCarryOutComponent).OutLine = flowData.OutLineColor;
        }

        //联动对象脚本绑定
        if (flowData.ExtendActionDict != null)
        {
            foreach (var data in flowData.ExtendActionDict)
            {
                PerformExpand.Bind(null, data.Key, data.Value, (extendCarryOut) =>
                {
                    extendCarryOut.IsExtend = true;
                    extendCarryOut.Init(null);
                    carryOut.LinksCarryOut.Add(extendCarryOut);
                    carryOutList.Add(extendCarryOut);
                });
            }
        }
    }

    /// <summary>
    /// 创建工具GameObject
    /// </summary>
    private void CreateToolGobj()
    {
        Dictionary<string, ResourcesItemData> createToolDict = new Dictionary<string, ResourcesItemData>();
        for (int i = 0; i < flowDataList.Count; i++)
        {
            if (flowDataList[i] is SimulationData == false)
            {
                continue;
            }

            SimulationData simulationData = (SimulationData) flowDataList[i];

            //工具信息
            ToolInfoData toolInfoData = DataManager.Instance.GetToolInfoById(simulationData.ToolId);
            if (toolInfoData == null)
            {
                continue;
            }

            //判断重复添加
            if (createToolDict.ContainsKey(toolInfoData.ToolType))
            {
                continue;
            }

            //是场景中包含的 无需创建
            if (toolInfoData.IsSceneContain)
            {
                toolGobjDict.Add(toolInfoData, GameObject.Find(toolInfoData.ToolType));
                continue;
            }

            //工具资源信息
            ResourcesItemData resourcesItemData =
                ResourcesRepertoryDataManager.Instance.GetResourcesData(ResourcesType.ToolModel, toolInfoData.ToolType);
            if (resourcesItemData == null)
            {
                Debug.LogError("找不到工具资源信息！！");
                continue;
            }

            createToolDict.Add(toolInfoData.ToolType, resourcesItemData);
        }

        foreach (KeyValuePair<string, ResourcesItemData> item in createToolDict)
        {
            ModuleMgr.Instance.ResourcesMod.LoadAsset(ResourcesType.ToolModel, VEMFacade.VEMSimPlatformPath + item.Value.FullPath, item.Value.FileName, CreateToolPrefabCallback);
        }
    }

    /// <summary>
    /// 创建工具回调
    /// </summary>
    private void CreateToolPrefabCallback(AssetInfo abInfo)
    {
        ModuleMgr.Instance.ResourcesMod.AssetInstantiate(abInfo, LoadModelCallback);
    }

    /// <summary>
    /// 加载模型回调
    /// </summary>
    /// <param name="gObj"></param>
    /// <param name="assetInfo"></param>
    private void LoadModelCallback(GameObject gObj, AssetInfo assetInfo)
    {
        gObj.SetActive(false);
        ToolInfoData toolInfoData = DataManager.Instance.GetFirstToolByName(gObj.name);
        toolGobjDict.Add(toolInfoData, gObj);
    }

    #region 触发

    /// <summary>
    /// 点击到物体
    /// </summary>
    /// <param name="isHitTrue"></param>
    /// <param name="carryOut"></param>
    private void HitModel(bool isHitTrue, CarryOutBase carryOut)
    {
        Debug.Log("点击到物体 == " + carryOut.transform.name);
        carryOut.Play(StepComplete);
    }

    #endregion

    #endregion
}