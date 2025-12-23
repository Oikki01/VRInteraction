public class AssetInfo
{
    public ResourcesType ResourcesType;

    public string FullPath;

    public string ResName;

    public AssetInfo()
    {
    }

    public AssetInfo(ResourcesType ResourcesType, string fullPath, string resName)
    {
        this.ResourcesType = ResourcesType;
        FullPath = fullPath;
        ResName = resName;
    }

    public virtual void UnLoad()
    {
    }
}