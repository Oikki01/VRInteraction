using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMod : BaseModule
{
    private string curSceneName;

    private BaseScene curScene;

    private float mLoadProgress;

    private Dictionary<string, BaseScene> sceneDict;

    public BaseScene CurScene => curScene;

    protected override void OnLoad()
    {
        InitSceneDict(VEMSimBase.Instance.SceneNameList);
    }

    private void InitSceneDict(List<string> sceneNameList)
    {
        sceneDict = new Dictionary<string, BaseScene>();
        for (int i = 0; i < sceneNameList.Count; i++)
        {
            object obj = Activator.CreateInstance(Singleton<VEMAssemblyManager>.Instance.CSharp_Assembly.GetType(sceneNameList[i].ToString()));
            sceneDict.Add(sceneNameList[i], obj as BaseScene);
        }
    }

    protected override void OnDispose()
    {
        curScene?.Dispose();
    }

    protected override void OnUpdate()
    {
        if (curScene != null && curScene.IsPreloadDone)
        {
            curScene.Update();
        }
    }

    protected override void OnFixedUpdate()
    {
        if (curScene != null && curScene.IsPreloadDone)
        {
            curScene.FixedUpdate();
        }
    }

    public void LoadScene(string sceneName)
    {
        if (sceneDict.TryGetValue(sceneName, out var value))
        {
            if (curScene != null)
            {
                curScene.Dispose();
            }

            curSceneName = sceneName;
            curScene = null;
            VEMSimBase.Instance.StartCoroutine(StartLoad(sceneName, value));
        }
    }

    private IEnumerator StartLoad(string name, BaseScene scene)
    {
        Resources.UnloadUnusedAssets();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        yield return null;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        scene.Preload();
        while (!scene.IsPreloadDone)
        {
            yield return null;
        }

        yield return null;
        scene.Load();
        FacadeScene.OnSceneLoadFinish?.Invoke(name);
        curScene = scene;
    }
}