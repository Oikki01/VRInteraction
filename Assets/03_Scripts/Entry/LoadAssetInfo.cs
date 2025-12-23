using System;

public class LoadAssetInfo : AssetInfo
{
    public Action<AssetInfo> LoadCallBack;

    public LoadAssetInfo(ResourcesType type, string fullPath, string resName, Action<AssetInfo> action)
    {
        ResourcesType = type;
        FullPath = fullPath;
        ResName = resName;
        LoadCallBack = action;
    }
}