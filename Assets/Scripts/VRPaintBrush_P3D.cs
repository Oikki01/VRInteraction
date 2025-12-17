using CW;
using HTC.UnityPlugin.Vive;
using PaintCore;
using PaintIn3D;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static PaintIn3D.CwHitBetween;

//========================================
//作    者:
//创建时间:
//备    注:
//========================================
public class VRPaintBrush_P3D : MonoBehaviour
{
    #region 私有变量
    public bool CanBePaintBrush { set { canBePaintBrush = value; } get { return canBePaintBrush; } }
    [SerializeField] private bool canBePaintBrush = true;

    #endregion

    #region 公共变量
	
	public CwHitBetween VrHitBetween;

    #endregion

    #region Mono相关
	
	private void Start()
	{

    }

    private void Update()
    {
        if (canBePaintBrush)
        {
            if (ViveInput.GetTriggerValue(HandRole.RightHand) > 0.1f)
            {
                VrHitBetween.CurPaintState = CwHitBetween.PaintState.Painting;
            }
            else
            {
                VrHitBetween.CurPaintState = CwHitBetween.PaintState.None;
            }
            if (ViveInput.GetPressDown(HandRole.RightHand, ControllerButton.GripTouch))
            {
                CwStateManager.UndoAll();
            }
        }
    }

    #endregion

    #region 私有函数/业务逻辑



    #endregion

    #region 公共函数/业务逻辑



    #endregion
}
