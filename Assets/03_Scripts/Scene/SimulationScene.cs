using System.Collections.Generic;
using System.Threading.Tasks;
using HuaRuXR.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 操作模拟场景
/// </summary>
public class SimulationScene : BaseScene
{
    #region 私有变量

    /// <summary>
    /// 流程管理器
    /// </summary>
    protected ProcessManager processManager;

    /// <summary>
    /// Loading
    /// </summary>
    //protected UILoading loadingUI = null;

    /// <summary>
    /// 基本信息
    /// </summary>
    protected SubjectSceneInfo subjectSceneInfo;

    /// <summary>
    /// 模型等待加载列表
    /// </summary>
    protected List<string> modelLoadingList = new List<string>();

    #endregion

    #region 生命周期函数

    /// <summary>
    /// 进入
    /// </summary>
    protected override void OnLoad()
    {
        Debug.Log("进入操作模拟场景");
        base.OnLoad();

        //顶部ui
        //UIMainTitleData panelData = new UIMainTitleData(BtnCloseOnClick);
        UIForm uiForm = UIEntry.UICom.GetUIForm("Prefabs/UI/UIMainTitle");
        //uiForm.OnOpen(panelData);

        //基本信息
        subjectSceneInfo = JsonFullSerializer.LoadJsonFile<SubjectSceneInfo>(VEMFacade.CurSubjectFolder + "/SubjectSceneInfo.json");
        InitModelLoadingList();

        //加载场景
        InitModelScene(subjectSceneInfo.SceneName);
    }

    /// <summary>
    /// 卸载
    /// </summary>
    protected override void OnDispose()
    {
        base.OnDispose();
        //关闭UI
        UIForm simulationPanel = UIEntry.UICom.GetUIForm("Prefabs/UI/UISimulationPanel");
        if (simulationPanel != null)
        {
            UIEntry.UICom.CloseUIForm(simulationPanel);
        }
    }

    #endregion

    #region 私有函数

    /// <summary>
    /// 加载场景
    /// </summary>
    protected void InitModelScene(string sceneName)
    {
        ResourcesItemData resourcesItemData =
            ResourcesRepertoryDataManager.Instance.GetResourcesData(ResourcesType.Scene, sceneName);
        if (resourcesItemData != null)
        {
            int uiLoadingSerialId = UIEntry.UICom.OpenUIForm("Prefabs/UI/UILoading", UIConfig.Dialog);
            //loadingUI = UIEntry.UICom.GetUIForm(uiLoadingSerialId).GetComponent<UILoading>();
            //loadingUI.SetContent("正在加载场景...");
            ModuleMgr.Instance.ResourcesMod.LoadAsset(ResourcesType.Scene, VEMFacade.VEMSimPlatformPath + resourcesItemData.FullPath, resourcesItemData.ResourcesName,
                (a) => { ModuleMgr.Instance.ResourcesMod.AssetInstantiate(a, SetterScene); });
        }
    }

    /// <summary>
    /// 场景生成回调
    /// </summary>
    /// <param name="gObj"></param>
    /// <param name="assetInfo"></param>
    private void SetterScene(GameObject gObj, AssetInfo assetInfo)
    {
        Debug.Log("场景AB包加载完成");
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(assetInfo.ResName));

        Camera sceneCameraPrefab = Resources.Load<Camera>("Prefabs/MainCamera");
        Camera sceneCamera = MonoBehaviour.Instantiate(sceneCameraPrefab);
        if (sceneCamera == null)
        {
            Debug.LogError("场景中未包含相机组件！");
        }
        else
        {
            sceneCamera.transform.position = subjectSceneInfo.CtrlInitPos;
            sceneCamera.transform.eulerAngles = subjectSceneInfo.CtrlInitRot;
        }

        //loadingUI.SetContent(string.Format("正在加载模型 {0}/{1} ", (subjectSceneInfo.ModelList.Count - modelLoadingList.Count) + 1, subjectSceneInfo.ModelList.Count));
        //加载模型
        for (var i = 0; i < subjectSceneInfo.ModelList.Count; i++)
        {
            ResourcesItemData resourcesItemData = ResourcesRepertoryDataManager.Instance.GetResourcesDataByFileName(ResourcesType.EquipModel, subjectSceneInfo.ModelList[i].ModelName);
            ChoiceModelCallback(resourcesItemData);
        }
    }

    /// <summary>
    /// 待加载模型列表
    /// </summary>
    private void InitModelLoadingList()
    {
        for (int i = 0; i < subjectSceneInfo.ModelList.Count; i++)
        {
            modelLoadingList.Add(subjectSceneInfo.ModelList[i].ModelName);
        }
    }

    /// <summary>
    /// 选择模型导入回调 1
    /// </summary>
    /// <param name="resourcesItemData"></param>
    private void ChoiceModelCallback(ResourcesItemData resourcesItemData)
    {
        string path = resourcesItemData.FullPath;
        string fileName = resourcesItemData.FileName;
        string filePath = VEMFacade.VEMSimPlatformPath + path;

        ModuleMgr.Instance.ResourcesMod.LoadAsset(ResourcesType.EquipModel, filePath, fileName, (assetInfo) => { ModuleMgr.Instance.ResourcesMod.AssetInstantiate(assetInfo, LoadModelCallBack); });
    }

    /// <summary>
    /// 加载模型完成回调 2
    /// </summary>
    /// <param name="gObj"></param>
    /// <param name="assetInfo"></param>
    private void LoadModelCallBack(GameObject gObj, AssetInfo assetInfo)
    {
        ModelData modelData = subjectSceneInfo.ModelList.Find(x => x.ModelName.Equals(assetInfo.ResName));
        if (modelData != null)
        {
            //模型位置、旋转赋值
            gObj.transform.position = modelData.ModelInitPos;
            gObj.transform.eulerAngles = modelData.ModelInitRot;
            ModelLoadComplete(assetInfo.ResName);
        }
    }

    /// <summary>
    /// 检查所有模型都加载完毕
    /// </summary>
    private void ModelLoadComplete(string modelName)
    {
        modelLoadingList.Remove(modelName);
        if (modelLoadingList.Count != 0)
        {
            //loadingUI.SetContent(string.Format("正在加载模型 {0}/{1} ", (subjectSceneInfo.ModelList.Count - modelLoadingList.Count) + 1, subjectSceneInfo.ModelList.Count));
        }
        else
        {
            //loadingUI.SetContent("模型加载完成！");
            //关闭Loading UI
            //UIEntry.UICom.CloseUIForm(loadingUI.UIForm);
            //创建流程管理器
            CreateProcessManager();
            //加载操作模拟UI
            UIEntry.UICom.OpenUIForm("Prefabs/UI/UISimulationPanel", UIConfig.Content);
        }
    }

    /// <summary>
    /// 创建流程管理器
    /// </summary>
    private void CreateProcessManager()
    {
        //流程管理器
        GameObject processManagerGobj = new GameObject("ProcessManager");
        processManagerGobj.name = "ProcessManager";
        processManager = processManagerGobj.AddComponent<ProcessManager>();
        QuoteResourcesManager quoteResourcesManager = processManagerGobj.AddComponent<QuoteResourcesManager>();
        GameObject modelScreenPrefab = Resources.Load<GameObject>("Prefabs/ModelScreenCanvas");

        quoteResourcesManager.InitData(VEMFacade.CurSubjectFolder, modelScreenPrefab);
        processManager.Init(VEMFacade.CurSubjectFolder);
    }

    /// <summary>
    /// 返回
    /// </summary>
    private void BtnCloseOnClick()
    {
        ProcedureEntry.ProcedureManager.ChangeProcedure("ProcedureKMMainPage");
    }

    #endregion

    #region 异步加载测试

    public async Task InitA()
    {
        await loadConfigDadaAsync();
        await BBB();
        await CCC();
    }

    private async Task loadConfigDadaAsync()
    {
        await Task.Delay(500);
        Debug.Log("加载完 loadConfigDadaAsync");
    }

    private async Task BBB()
    {
        await Task.Delay(500);
        Debug.Log("加载完 BBB");
    }

    private async Task CCC()
    {
        await Task.Delay(500);
        Debug.Log("加载完 CCC");
    }

    #endregion
}