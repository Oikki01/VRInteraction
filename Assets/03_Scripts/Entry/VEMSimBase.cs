using System.Collections.Generic;
using UnityEngine;

public class VEMSimBase : MonoBehaviour
{
    public List<string> SceneNameList;

    public static VEMSimBase Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        Load();
    }

    public void Load()
    {
        Object.DontDestroyOnLoad(base.gameObject);
        Singleton<ModuleMgr>.Instance.Load();
    }

    private void Start()
    {
        Singleton<ModuleMgr>.Instance.Start();
    }

    public void Update()
    {
        Singleton<ModuleMgr>.Instance.Update();
    }

    public void FixedUpdate()
    {
        Singleton<ModuleMgr>.Instance.FixedUpdate();
    }

    public void LateUpdate()
    {
        Singleton<ModuleMgr>.Instance.LateUpdate();
    }

    private void OnDestroy()
    {
        Exit();
    }

    public void Exit()
    {
        Singleton<ModuleMgr>.Instance.Dispose();
    }
}