using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Author:lidongwei
/// Description:
/// </summary>
public class LaunchEngine : MonoBehaviour
{
    [Header("帧数设置s")] public int Frame = 60;

    void Awake()
    {
        DontDestroyOnLoad(this);
        Application.targetFrameRate = Frame;
#if !UNITY_EDITOR
        // Debug.unityLogger.logEnabled = false; 
        // ToolControlTaskBar.ShowTaskBar(Screen.currentResolution.width,Screen.currentResolution.height);
#endif
    }

    private void Start()
    {
        InitData();
        Invoke("InitBaseComponent", 0.2f);
    }

    /// <summary>
    /// 
    /// </summary>
    private void InitData()
    {
        VEMAssemblyManager.Instance.Init(Application.isEditor);
    }

    /// <summary>
    /// 初始化框架
    /// </summary>
    private void InitBaseComponent()
    {
        SceneManager.LoadScene("1_StartScene");
    }
}