using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResourcesModule : BaseModule
{
    private Queue<LoadAssetInfo> assetQueue = new Queue<LoadAssetInfo>();

    private List<AssetInfo> assetInfoList = new List<AssetInfo>();

    private TrilibLoadBase trilibLoad;

    private Dictionary<string, string> assetBundelLoadedDict = new Dictionary<string, string>();

    protected override void OnLoad()
    {
        VEMSimBase.Instance.StartCoroutine(StartLoad());
    }

    protected override void OnDispose()
    {
    }

    protected override void OnUpdate()
    {
    }

    protected override void OnFixedUpdate()
    {
    }

    public void InitTrilibLoadClass(TrilibLoadBase trilibLoad)
    {
        this.trilibLoad = trilibLoad;
    }

    public void LoadAsset(ResourcesType type, string fullPath, string resName, Action<AssetInfo> action)
    {
        LoadAssetInfo item = new LoadAssetInfo(type, fullPath, resName, action);
        assetQueue.Enqueue(item);
    }

    public void AssetInstantiate(AssetInfo info, Action<GameObject, AssetInfo> action)
    {
        switch (info.ResourcesType)
        {
            default:
                _ = 8;
                break;
            case ResourcesType.Scene:
                {
                    string url = info.FullPath + "/" + info.ResName + ".bundle";
                    string assetUrl = info.FullPath + "/" + info.ResName + "Res.bundle";
                    VEMSimBase.Instance.StartCoroutine(LoadSceneFromAB(url, assetUrl, info.ResName, info, action));
                    break;
                }
            case ResourcesType.EquipModel:
                VEMSimBase.Instance.StartCoroutine(trilibLoad?.LoadGameObject(info, action));
                break;
            case ResourcesType.ToolModel:
                VEMSimBase.Instance.StartCoroutine(trilibLoad?.LoadGameObject(info, action));
                break;
        }
    }

    public IEnumerator LoadMaterial(AssetInfo info, Action<Material> action)
    {
        yield return null;
    }

    public void UnLoad(Predicate<AssetInfo> predicate)
    {
        for (int i = 0; i < assetInfoList.Count; i++)
        {
            if (predicate(assetInfoList[i]))
            {
                assetInfoList[i].UnLoad();
                assetInfoList.RemoveAt(i);
            }
        }
    }

    public void UnLoad(ResourcesType resourcesType, string resourcesName)
    {
        for (int i = 0; i < assetInfoList.Count; i++)
        {
            if (assetInfoList[i].ResourcesType == resourcesType && assetInfoList[i].ResName.Equals(resourcesName))
            {
                assetInfoList[i].UnLoad();
                assetInfoList.RemoveAt(i);
                break;
            }
        }
    }

    public void UnLoadAll()
    {
        foreach (AssetInfo assetInfo in assetInfoList)
        {
            assetInfo.UnLoad();
        }

        assetInfoList.Clear();
    }

    public void ASyncLoadScene(string url, string assetUrl, string sceneName)
    {
        VEMSimBase.Instance.StartCoroutine(LoadSceneFromAB(url, assetUrl, sceneName));
    }

    private IEnumerator StartLoad()
    {
        while (true)
        {
            if (assetQueue.Count > 0)
            {
                LoadAssetInfo loadAssetInfo = assetQueue.Dequeue();
                if (loadAssetInfo.ResourcesType.Equals(ResourcesType.Scene))
                {
                    yield return LoadModelFile(loadAssetInfo.FullPath, loadAssetInfo.ResName, loadAssetInfo.ResourcesType, loadAssetInfo.LoadCallBack);
                }
                else if (!loadAssetInfo.ResourcesType.Equals(ResourcesType.Material) && !loadAssetInfo.ResourcesType.Equals(ResourcesType.SpeciallyEffect) && (loadAssetInfo.ResourcesType.Equals(ResourcesType.EquipModel) || loadAssetInfo.ResourcesType.Equals(ResourcesType.ToolModel)))
                {
                    yield return LoadModelFile(loadAssetInfo.FullPath, loadAssetInfo.ResName, loadAssetInfo.ResourcesType, loadAssetInfo.LoadCallBack);
                }
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private IEnumerator LoadModelFile(string path, string resName, ResourcesType type, Action<AssetInfo> action)
    {
        AssetInfo assetInfo = assetInfoList.Find((AssetInfo a) => a.FullPath == path);
        if (assetInfo != null)
        {
            action(assetInfo);
            yield break;
        }

        AssetInfo assetInfo2 = new AssetInfo(type, path, resName);
        assetInfoList.Add(assetInfo2);
        action?.Invoke(assetInfo2);
    }

    private IEnumerator LoadSceneFromAB(string url, string assetUrl, string sceneName, AssetInfo info = null, Action<GameObject, AssetInfo> callback = null)
    {
        if (!string.IsNullOrEmpty(assetUrl))
        {
            UnloadDuplicateBundles(assetUrl);
            AssetBundleCreateRequest asseBundleCreateRequest = AssetBundle.LoadFromFileAsync(assetUrl);
            yield return asseBundleCreateRequest;
            assetBundelLoadedDict.Add(assetUrl, asseBundleCreateRequest.assetBundle.name);
        }

        AssetBundleCreateRequest bundleCreateRequest = AssetBundle.LoadFromFileAsync(url);
        yield return bundleCreateRequest;
        AssetBundle bundle = bundleCreateRequest.assetBundle;
        if (bundle != null)
        {
            if (Array.Find(bundle.GetAllScenePaths(), (string s) => s.EndsWith(sceneName + ".unity")) != null)
            {
                AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                while (!asyncOperation.isDone)
                {
                    Debug.Log("进度 == " + asyncOperation.progress);
                    FacadeScene.ABSceneLoadProgress?.Invoke(asyncOperation.progress);
                    yield return null;
                }

                Debug.Log("加载完成");
                callback?.Invoke(null, info);
            }
            else
            {
                Debug.LogError("scene not found in AssetBundle");
            }

            bundle.Unload(unloadAllLoadedObjects: false);
        }
        else
        {
            Debug.LogError("Failed to load AssetBundle");
        }
    }

    private void UnloadDuplicateBundles(string assetUrl)
    {
        if (!assetBundelLoadedDict.ContainsKey(assetUrl))
        {
            return;
        }

        string value = assetBundelLoadedDict[assetUrl];
        AssetBundle[] array = AssetBundle.GetAllLoadedAssetBundles().ToArray();
        foreach (AssetBundle assetBundle in array)
        {
            if (assetBundle.name.Equals(value))
            {
                Debug.Log("卸载Bundle == " + assetUrl);
                assetBundelLoadedDict.Remove(assetUrl);
                assetBundle.Unload(unloadAllLoadedObjects: false);
                break;
            }
        }
    }
}