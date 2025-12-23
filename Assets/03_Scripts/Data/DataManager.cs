using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//========================================
//作    者:
//创建时间:
//备    注:
//========================================
public class DataManager : Singleton<DataManager>
{
    private string projectPath;

    private readonly string partInfoExcelPath = "/Config/PartInfo.xlsx";

    private readonly string toolInfoExcelPath = "/Config/ToolInfo.xlsx";

    private List<PartInfoData> partInfoDataManager;

    private List<ToolInfoData> toolInfoDataManager;

    private readonly string simulationPartExcelPath = "/Config/SimulationPart.xlsx";

    private List<SimulationPartData> simulationPartList;

    private readonly string faultFixPartExcelPath = "/Config/FaultPartInfo.xlsx";

    private List<FaultFixPartData> faultFixPartDataList;

    private List<FaultFixToolData> faultFixToolDataList;

    public void Init(string projectPath)
    {
        this.projectPath = projectPath;
        ReaderPartInfoExcel(projectPath + partInfoExcelPath);
        ReaderToolInfoExcel(projectPath + toolInfoExcelPath);
        ReaderSimulationPartExcel(projectPath + simulationPartExcelPath);
        ReaderFaultFixPartExcel(projectPath + faultFixPartExcelPath);
    }

    private void ReaderPartInfoExcel(string fullPath)
    {
        partInfoDataManager = new List<PartInfoData>();
        object[,] array = ExcelUtil.ReadExcelRange(fullPath);
        for (int i = 0; i < array.GetLength(0); i++)
        {
            PartInfoData partInfoData = new PartInfoData();
            partInfoData.Id = int.Parse(array[i, 0].ToString());
            partInfoData.PartType = array[i, 1].ToString();
            partInfoData.PartSpecification = array[i, 2].ToString();
            partInfoData.PartOffsetPos = ExcelUtil.ExcelDataToVector3(array[i, 3].ToString());
            partInfoData.PartDirection = ExcelUtil.ExcelDataToVector3(array[i, 4].ToString());
            partInfoData.ToolId = int.Parse(array[i, 5].ToString());
            partInfoData.ClassFullName = array[i, 6].ToString();
            partInfoData.ThumbnailImg = array[i, 7].ToString();
            partInfoDataManager.Add(partInfoData);
        }
    }

    private void ReaderToolInfoExcel(string fullPath)
    {
        toolInfoDataManager = new List<ToolInfoData>();
        object[,] array = ExcelUtil.ReadExcelRange(fullPath);
        for (int i = 0; i < array.GetLength(0); i++)
        {
            ToolInfoData toolInfoData = new ToolInfoData();
            toolInfoData.Id = int.Parse(array[i, 0].ToString());
            toolInfoData.ToolType = array[i, 1].ToString();
            toolInfoData.ToolSpecification = array[i, 2].ToString();
            toolInfoData.ClassFullName = array[i, 3].ToString();
            toolInfoData.Gesture = array[i, 4].ToString();
            toolInfoData.OffsetPos = ExcelUtil.ExcelDataToVector3(array[i, 5].ToString());
            toolInfoData.OffsetRot = ExcelUtil.ExcelDataToVector3(array[i, 6].ToString());
            toolInfoData.IsSceneContain = (array[i, 7].ToString().Equals("是") ? true : false);
            toolInfoData.ThumbnailImg = array[i, 8].ToString();
            toolInfoDataManager.Add(toolInfoData);
        }
    }

    public string GetPartInfoExcelPath()
    {
        return partInfoExcelPath;
    }

    public string GetToolInfoExcelPath()
    {
        return toolInfoExcelPath;
    }

    public List<int> GetToolListByPart(string partType, string partSpecification)
    {
        List<int> list = new List<int>();
        if (partInfoDataManager == null)
        {
            return list;
        }

        List<PartInfoData> list2 = partInfoDataManager.FindAll((PartInfoData x) => x.PartType.Equals(partType) && x.PartSpecification.Equals(partSpecification));
        for (int num = 0; num < list2.Count; num++)
        {
            list.Add(list2[num].ToolId);
        }

        return list;
    }

    public List<int> GetToolListByPart(int PartID)
    {
        List<int> list = new List<int>();
        if (partInfoDataManager == null)
        {
            return list;
        }

        List<PartInfoData> list2 = partInfoDataManager.FindAll((PartInfoData x) => x.Id.Equals(PartID));
        for (int num = 0; num < list2.Count; num++)
        {
            list.Add(list2[num].ToolId);
        }

        return list;
    }

    public bool IsMatchToolAndPart(int PartID, int ToolId)
    {
        return GetToolListByPart(PartID).Contains(ToolId);
    }

    public ToolInfoData GetToolInfoById(int id)
    {
        if (toolInfoDataManager == null)
        {
            return null;
        }

        return toolInfoDataManager.Find((ToolInfoData x) => x.Id == id);
    }

    public List<ToolInfoData> GetToolInfoByName(string ToolName)
    {
        if (toolInfoDataManager == null)
        {
            return null;
        }

        return toolInfoDataManager.FindAll((ToolInfoData x) => x.ToolType == ToolName);
    }

    public ToolInfoData GetFirstToolByName(string ToolName)
    {
        return toolInfoDataManager.Find((ToolInfoData x) => x.ToolType == ToolName);
    }

    public ToolInfoData GetToolInfoByNameAndSpecification(string Name, string Specification)
    {
        return GetToolInfoByName(Name).Find((ToolInfoData x) => x.ToolSpecification.Equals(Specification));
    }

    public PartInfoData GetPartInfoById(int id)
    {
        if (partInfoDataManager == null)
        {
            return null;
        }

        return partInfoDataManager.Find((PartInfoData x) => x.Id == id);
    }

    public List<string> GetAllPartType()
    {
        if (partInfoDataManager == null)
        {
            return null;
        }

        List<string> list = new List<string>();
        for (int i = 0; i < partInfoDataManager.Count; i++)
        {
            string partType = partInfoDataManager[i].PartType;
            bool flag = partInfoDataManager[i].ToolId != -1;
            if (!list.Contains(partType) && flag)
            {
                list.Add(partType);
            }
        }

        return list;
    }

    public List<string> GetAllToolType()
    {
        if (toolInfoDataManager == null)
        {
            return null;
        }

        List<string> list = new List<string>();
        for (int i = 0; i < toolInfoDataManager.Count; i++)
        {
            string toolType = toolInfoDataManager[i].ToolType;
            if (!list.Contains(toolType))
            {
                list.Add(toolType);
            }
        }

        return list;
    }

    public List<string> GetAllToolTypeByPartId(int partId)
    {
        if (toolInfoDataManager == null)
        {
            return null;
        }

        if (partInfoDataManager == null)
        {
            return null;
        }

        PartInfoData partInfoById = GetPartInfoById(partId);
        if (partInfoById == null)
        {
            return null;
        }

        List<string> list = new List<string>();
        ToolInfoData toolInfoById = GetToolInfoById(partInfoById.ToolId);
        if (toolInfoById != null)
        {
            string toolType = toolInfoById.ToolType;
            if (!list.Contains(toolType))
            {
                list.Add(toolType);
            }
        }

        return list;
    }

    public List<PartInfoData> GetPartInfoByName(string Name)
    {
        if (partInfoDataManager == null)
        {
            return null;
        }

        return partInfoDataManager.FindAll((PartInfoData x) => x.PartType.Equals(Name));
    }

    public PartInfoData GetPartInfoByNameAndSpecification(string Name, string Specification)
    {
        return GetPartInfoByName(Name).Find((PartInfoData x) => x.PartSpecification.Equals(Specification));
    }

    public PartInfoData GetPartInfoByClassType(string TypeName)
    {
        return partInfoDataManager.Find((PartInfoData x) => x.ClassFullName.Equals(TypeName));
    }

    private void ReaderSimulationPartExcel(string fullPath)
    {
        simulationPartList = new List<SimulationPartData>();
        object[,] array = ExcelUtil.ReadExcelRange(fullPath);
        for (int i = 0; i < array.GetLength(0); i++)
        {
            SimulationPartData simulationPartData = new SimulationPartData();
            simulationPartData.Id = int.Parse(array[i, 0].ToString());
            simulationPartData.PartType = array[i, 1].ToString();
            simulationPartData.ClassFullName = array[i, 2].ToString();
            simulationPartData.ThumbnailImg = array[i, 3].ToString();
            simulationPartList.Add(simulationPartData);
        }
    }

    public List<string> GetAllSimulationPartType()
    {
        if (simulationPartList == null)
        {
            return null;
        }

        List<string> list = new List<string>();
        for (int i = 0; i < simulationPartList.Count; i++)
        {
            string partType = simulationPartList[i].PartType;
            if (!list.Contains(partType))
            {
                list.Add(partType);
            }
        }

        return list;
    }

    public List<SimulationPartData> GetSimulationPartInfoByName(string name)
    {
        if (simulationPartList == null)
        {
            return null;
        }

        return simulationPartList.FindAll((SimulationPartData x) => x.PartType.Equals(name));
    }

    public SimulationPartData GetSimulationPartInfoByPartType(string partType)
    {
        return simulationPartList.Find((SimulationPartData x) => x.PartType.Equals(partType));
    }

    public SimulationPartData GetSimulationPartInfoByFullName(string ClassFullName)
    {
        return simulationPartList.Find((SimulationPartData x) => x.ClassFullName.Equals(ClassFullName));
    }

    public string GetSimulationInfoExcelPath()
    {
        return simulationPartExcelPath;
    }

    private void ReaderFaultFixPartExcel(string fullPath)
    {
        faultFixPartDataList = new List<FaultFixPartData>();
        object[,] array = ExcelUtil.ReadExcelRange(fullPath);
        for (int i = 0; i < array.GetLength(0); i++)
        {
            FaultFixPartData faultFixPartData = new FaultFixPartData();
            faultFixPartData.Id = int.Parse(array[i, 0].ToString());
            faultFixPartData.PartType = array[i, 1].ToString();
            faultFixPartData.ClassFullName = array[i, 2].ToString();
            faultFixPartData.ToolIdList = ExcelUtil.ExcelDataToIntList(array[i, 3].ToString());
            faultFixPartData.ThumbnailImg = array[i, 4].ToString();
            faultFixPartDataList.Add(faultFixPartData);
        }
    }

    public List<string> GetAllFaultFixPartType()
    {
        if (faultFixPartDataList == null)
        {
            return null;
        }

        List<string> list = new List<string>();
        for (int i = 0; i < faultFixPartDataList.Count; i++)
        {
            string partType = faultFixPartDataList[i].PartType;
            if (!list.Contains(partType))
            {
                list.Add(partType);
            }
        }

        return list;
    }

    public List<string> GetAllFaultFixToolType()
    {
        if (faultFixToolDataList == null)
        {
            return null;
        }

        List<string> list = new List<string>();
        for (int i = 0; i < faultFixToolDataList.Count; i++)
        {
            string toolType = faultFixToolDataList[i].ToolType;
            if (!list.Contains(toolType))
            {
                list.Add(toolType);
            }
        }

        return list;
    }

    public FaultFixToolData GetFaultFixToolInfoByName(string toolType)
    {
        return faultFixToolDataList.Find((FaultFixToolData x) => x.ToolType.Equals(toolType));
    }

    public FaultFixToolData GetFaultFixToolInfoById(int id)
    {
        return faultFixToolDataList.Find((FaultFixToolData x) => x.Id.Equals(id));
    }

    public List<FaultFixPartData> GetFaultFixPartInfoByName(string name)
    {
        if (faultFixPartDataList == null)
        {
            return null;
        }

        return faultFixPartDataList.FindAll((FaultFixPartData x) => x.PartType.Equals(name));
    }

    public FaultFixPartData GetFaultFixPartInfoByPartType(string partType)
    {
        return faultFixPartDataList.Find((FaultFixPartData x) => x.PartType.Equals(partType));
    }

    public FaultFixPartData GetFaultFixPartInfoByFullName(string ClassFullName)
    {
        return faultFixPartDataList.Find((FaultFixPartData x) => x.ClassFullName.Equals(ClassFullName));
    }

    public string GetFaultFixInfoExcelPath()
    {
        return faultFixPartExcelPath;
    }
}
