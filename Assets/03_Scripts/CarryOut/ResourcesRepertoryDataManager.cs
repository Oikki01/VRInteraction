using System.Collections.Generic;
using UnityEngine;

public class ResourcesRepertoryDataManager : Singleton<ResourcesRepertoryDataManager>
{
    private readonly string EquipModelPath = "/ResourcesRepertory/3DModelRepertory/EquipModelRepertory";

    private readonly string ToolModelPath = "/ResourcesRepertory/3DModelRepertory/ToolModelRepertory";

    private readonly string SceneModelPath = "/ResourcesRepertory/3DModelRepertory/SceneModelRepertory";

    private readonly string AudioPath = "/ResourcesRepertory/MultimediaRepertory/Audio";

    private readonly string VideoPath = "/ResourcesRepertory/MultimediaRepertory/Video";

    private readonly string PicturePath = "/ResourcesRepertory/MultimediaRepertory/Picture";

    private readonly string documentPath = "/ResourcesRepertory/MultimediaRepertory/Ducument";

    private readonly string effectPath = "/ResourcesRepertory/MultimediaRepertory/SpeciallyEffect";

    private readonly string materialPath = "/ResourcesRepertory/MaterialRepertory";

    private readonly string JsonDataPath = "/ResourcesRepertory/ResourcesRepertory.json";

    private string projectPath;

    private Dictionary<ResourcesType, List<ResourcesItemData>> resourcesDataDict;

    public void Init(string projectPath)
    {
        this.projectPath = projectPath;
        ImportJson(projectPath + JsonDataPath);
    }

    public void ImportJson(string xmlPath)
    {
        resourcesDataDict = JsonFullSerializer.LoadJsonFile<Dictionary<ResourcesType, List<ResourcesItemData>>>(xmlPath);
        if (resourcesDataDict == null)
        {
            resourcesDataDict = new Dictionary<ResourcesType, List<ResourcesItemData>>();
        }
    }

    private void Save()
    {
        JsonFullSerializer.SaveJsonFile(projectPath + JsonDataPath, resourcesDataDict);
    }

    public bool AddResourcesData(ResourcesType type, ResourcesItemData resourcesItemData)
    {
        if (resourcesDataDict.ContainsKey(type))
        {
            if (resourcesDataDict[type].Find((ResourcesItemData x) => x.ResourcesName.Equals(resourcesItemData.ResourcesName)) != null)
            {
                return false;
            }

            resourcesDataDict[type].Add(resourcesItemData);
            Save();
            return true;
        }

        List<ResourcesItemData> list = new List<ResourcesItemData>();
        list.Add(resourcesItemData);
        resourcesDataDict.Add(type, list);
        Save();
        return true;
    }

    public bool RemoveResourcesData(ResourcesType type, string resourcesName)
    {
        if (resourcesDataDict.ContainsKey(type))
        {
            for (int i = 0; i < resourcesDataDict[type].Count; i++)
            {
                if (resourcesDataDict[type][i].ResourcesName.Equals(resourcesName))
                {
                    resourcesDataDict[type].RemoveAt(i);
                    Save();
                    return true;
                }
            }

            return false;
        }

        return false;
    }

    public bool SetResourcesImage(ResourcesType type, string resourcesName, string imgPath)
    {
        if (resourcesDataDict.ContainsKey(type))
        {
            for (int i = 0; i < resourcesDataDict[type].Count; i++)
            {
                if (resourcesDataDict[type][i].ResourcesName.Equals(resourcesName))
                {
                    resourcesDataDict[type][i].imgPath = imgPath;
                    Save();
                    return true;
                }
            }

            return false;
        }

        return false;
    }

    public bool SetResourcesPath(ResourcesType type, string resourcesName, string fullPath, string parentFolderFullName)
    {
        if (resourcesDataDict.ContainsKey(type))
        {
            for (int i = 0; i < resourcesDataDict[type].Count; i++)
            {
                if (resourcesDataDict[type][i].ResourcesName.Equals(resourcesName))
                {
                    resourcesDataDict[type][i].FullPath = fullPath;
                    resourcesDataDict[type][i].ParentFolderFullName = parentFolderFullName;
                    resourcesDataDict[type][i].imgPath = fullPath + "/large.png";
                    Save();
                    return true;
                }
            }

            return false;
        }

        return false;
    }

    public List<ResourcesItemData> GetResourcesDataList(ResourcesType type)
    {
        if (resourcesDataDict.ContainsKey(type))
        {
            return resourcesDataDict[type].FindAll((ResourcesItemData x) => x.ResourcesType != ResourcesType.Folder);
        }

        Debug.LogError("No Tool Type");
        return new List<ResourcesItemData>();
    }

    public List<ResourcesItemData> GetResourcesDataByFolderFullPath(ResourcesType type, string folderFullPath)
    {
        if (resourcesDataDict.ContainsKey(type))
        {
            return resourcesDataDict[type].FindAll((ResourcesItemData x) => folderFullPath.Equals(x.ParentFolderFullName) && x.ResourcesType != ResourcesType.Folder);
        }

        return null;
    }

    public List<ResourcesItemData> GetResourcesDataFullPath(ResourcesType type)
    {
        List<ResourcesItemData> resourcesDataList = GetResourcesDataList(type);
        List<ResourcesItemData> list = new List<ResourcesItemData>();
        if (resourcesDataList == null)
        {
            return list;
        }

        foreach (ResourcesItemData item in resourcesDataList)
        {
            ResourcesItemData resourcesItemData = new ResourcesItemData(item.ResourcesName, item.FileName, projectPath + item.FullPath, item.Author, item.CreateTime, item.ResourcesType, item.ResourcesSize, item.ParentFolderFullName);
            resourcesItemData.imgPath = projectPath + item.imgPath;
            list.Add(resourcesItemData);
        }

        return list;
    }

    public ResourcesItemData GetResourcesDataByFileName(ResourcesType type, string fileName)
    {
        if (resourcesDataDict.ContainsKey(type))
        {
            return resourcesDataDict[type].Find((ResourcesItemData x) => x.FileName.Equals(fileName));
        }

        return null;
    }

    public ResourcesItemData GetResourcesData(ResourcesType type, string resourcesName)
    {
        if (resourcesDataDict.ContainsKey(type))
        {
            return resourcesDataDict[type].Find((ResourcesItemData x) => x.ResourcesName.Equals(resourcesName));
        }

        return null;
    }

    public List<ResourcesItemData> GetResourcesDataByParentFolderPath(ResourcesType type, string parentFolderFullPath)
    {
        if (resourcesDataDict.ContainsKey(type))
        {
            return resourcesDataDict[type].FindAll((ResourcesItemData x) => x.ParentFolderFullName.Equals(parentFolderFullPath));
        }

        return null;
    }

    public string GetResourcesPathByType(ResourcesType type, string projectPath = "")
    {
        return type switch
        {
            ResourcesType.EquipModel => projectPath + EquipModelPath,
            ResourcesType.ToolModel => projectPath + ToolModelPath,
            ResourcesType.Scene => projectPath + SceneModelPath,
            ResourcesType.Picture => projectPath + PicturePath,
            ResourcesType.Video => projectPath + VideoPath,
            ResourcesType.Audio => projectPath + AudioPath,
            ResourcesType.Document => projectPath + documentPath,
            ResourcesType.SpeciallyEffect => projectPath + effectPath,
            ResourcesType.Material => projectPath + materialPath,
            _ => string.Empty,
        };
    }

    public string GetFullPath(ResourcesItemData data)
    {
        if (data == null)
        {
            return string.Empty;
        }

        return projectPath + data.FullPath;
    }

    public bool AddResourcesFolderData(ResourcesType type, ResourcesItemData resourcesFolderData)
    {
        if (resourcesDataDict.ContainsKey(type))
        {
            if (resourcesDataDict[type].Find((ResourcesItemData x) => x.FullPath.Equals(resourcesFolderData.FullPath)) != null)
            {
                return false;
            }

            resourcesDataDict[type].Add(resourcesFolderData);
            Save();
            return true;
        }

        List<ResourcesItemData> list = new List<ResourcesItemData>();
        list.Add(resourcesFolderData);
        resourcesDataDict.Add(type, list);
        Save();
        return true;
    }

    public bool RemoveResourcesFolderData(ResourcesType type, string fullPath)
    {
        if (resourcesDataDict.ContainsKey(type))
        {
            for (int i = 0; i < resourcesDataDict[type].Count; i++)
            {
                if (resourcesDataDict[type][i].FullPath.Equals(fullPath))
                {
                    resourcesDataDict[type].RemoveAt(i);
                    Save();
                    return true;
                }
            }

            return false;
        }

        return false;
    }

    public bool UpdateFolderName(ResourcesType resourcesType, ResourcesItemData folderData, string newFolderName)
    {
        if (resourcesDataDict.ContainsKey(resourcesType))
        {
            for (int i = 0; i < resourcesDataDict[resourcesType].Count; i++)
            {
                if (resourcesDataDict[resourcesType][i].FullPath.Equals(folderData.FullPath))
                {
                    resourcesDataDict[resourcesType][i].ResourcesName = newFolderName;
                    resourcesDataDict[resourcesType][i].FullPath = resourcesDataDict[resourcesType][i].ParentFolderFullName + "/" + newFolderName;
                    Save();
                    return true;
                }
            }

            return false;
        }

        return false;
    }

    public bool UpdateFolderParentFolderFullPath(ResourcesType resourcesType, ResourcesItemData folderData, string newParentFolderFullName)
    {
        if (resourcesDataDict.ContainsKey(resourcesType))
        {
            for (int i = 0; i < resourcesDataDict[resourcesType].Count; i++)
            {
                if (resourcesDataDict[resourcesType][i].FullPath.Equals(folderData.FullPath))
                {
                    resourcesDataDict[resourcesType][i].ParentFolderFullName = newParentFolderFullName;
                    resourcesDataDict[resourcesType][i].FullPath = newParentFolderFullName + "/" + folderData.ResourcesName;
                    Save();
                    return true;
                }
            }

            return false;
        }

        return false;
    }

    public List<ResourcesItemData> GetFolderDataByParentFullPath(ResourcesType type, string parentFolderFullPath)
    {
        if (resourcesDataDict.ContainsKey(type))
        {
            return resourcesDataDict[type].FindAll((ResourcesItemData x) => x.ParentFolderFullName.Equals(parentFolderFullPath) && x.ResourcesType == ResourcesType.Folder);
        }

        return null;
    }

    public ResourcesItemData GetFolderDataByFullPath(ResourcesType type, string fullPath)
    {
        if (resourcesDataDict.ContainsKey(type))
        {
            return resourcesDataDict[type].Find((ResourcesItemData x) => x.FullPath.Equals(fullPath));
        }

        return null;
    }

    public void GetAllChildsFolder(ref List<ResourcesItemData> resourcesFolderList, ResourcesType type, string folderFullPath)
    {
        List<ResourcesItemData> folderDataByParentFullPath = GetFolderDataByParentFullPath(type, folderFullPath);
        resourcesFolderList.AddRange(folderDataByParentFullPath);
        for (int i = 0; i < folderDataByParentFullPath.Count; i++)
        {
            GetAllChildsFolder(ref resourcesFolderList, type, folderDataByParentFullPath[i].FullPath);
        }
    }
}