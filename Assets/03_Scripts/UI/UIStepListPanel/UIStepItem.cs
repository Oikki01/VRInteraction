using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 步骤列表项
/// </summary>
public class UIStepItem : MonoBehaviour
{
    #region GameObject引用

    /// <summary>
    /// 文本
    /// </summary>
    [SerializeField] private Text textContent;

    /// <summary>
    /// 未完成的背景
    /// </summary>
    [SerializeField] private GameObject imgNotBg;

    /// <summary>
    /// 当前的背景
    /// </summary>
    [SerializeField] private GameObject imgCurBg;

    /// <summary>
    /// 已完成的背景
    /// </summary>
    [SerializeField] private GameObject imgCompleteBg;

    #endregion

    #region 私有变量

    /// <summary>
    /// 流程数据
    /// </summary>
    private FlowDataBase flowData;

    /// <summary>
    /// 
    /// </summary>
    private int index;

    #endregion

    #region 公有函数

    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="flowDataBase"></param>
    /// <param name="index"></param>
    public void Init(FlowDataBase flowDataBase, int index)
    {
        this.flowData = flowDataBase;
        this.index = index;
        string content;
        if (flowDataBase.control == ControlType.TheoryStep)
        {
            content = "理论答题";
        }
        else
        {
            content = flowDataBase.StepName;
            if (content.Length > 12)
            {
                content = content.Substring(0, 12) + "...";
            }
        }

        textContent.text = String.Format("{0}.{1}", (index + 1), content);
        UpdateStateBg();
    }

    /// <summary>
    /// 设置状态背景
    /// </summary>
    public void UpdateStateBg()
    {
        imgNotBg.SetActive(false);
        imgCurBg.SetActive(false);
        imgCompleteBg.SetActive(false);
        textContent.fontStyle = FontStyle.Normal;

        if (index == ProcessManager.Instance.CurStepIndex)
        {
            imgCurBg.SetActive(true);
            textContent.fontStyle = FontStyle.Bold;
            textContent.color = Color.white;
        }
        else if (index < ProcessManager.Instance.CurStepIndex)
        {
            imgCompleteBg.SetActive(true);
            textContent.color = Color.green;
        }
        else
        {
            imgNotBg.SetActive(true);
            textContent.color = new Color(0, 0, 90.0f / 255.0f);
        }
    }

    #endregion
}