public class ResourcesItemData
{
    public string ResourcesName;

    public string FileName;

    public string FullPath;

    public string Author;

    public string CreateTime;

    public ResourcesType ResourcesType;

    public string ResourcesSize;

    public string ParentFolderFullName;

    public string imgPath;

    public ResourcesItemData(string resourcesName, string fileName, string fullPath, string author, string createTime, ResourcesType resourcesType, string resourcesSize, string parentFolderFullName)
    {
        ResourcesName = resourcesName;
        FileName = fileName;
        FullPath = fullPath;
        Author = author;
        CreateTime = createTime;
        ResourcesType = resourcesType;
        ResourcesSize = resourcesSize;
        ParentFolderFullName = parentFolderFullName;
    }

    public ResourcesItemData(string resourcesName, string fullPath, ResourcesType resourcesType)
    {
        ResourcesName = resourcesName;
        FullPath = fullPath;
        Author = "";
        CreateTime = "";
        ResourcesType = resourcesType;
        ResourcesSize = "";
        ParentFolderFullName = "";
    }

    public ResourcesItemData Clone()
    {
        return (ResourcesItemData)MemberwiseClone();
    }
}