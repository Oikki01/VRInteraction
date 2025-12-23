using System.Collections;
using System.Collections.Generic;
using HTC.UnityPlugin.Vive;
using UnityEngine;

//========================================
//作    者:
//创建时间:
//备    注:
//========================================
public class TestInputDown : MonoBehaviour
{
    #region 私有变量



    #endregion

    #region 公共变量
    public Transform LeftHandTrans;
    public Transform RightHandTrans;

    public HandRole LeftHand = HandRole.LeftHand;

    public HandRole RightHand = HandRole.RightHand;

    public float RayDistance = 2f;
    public LayerMask DetectLayer;

    #endregion

    #region Mono相关

    private void Start()
	{
		
	}

    private void Update()
    {
        //foreach (ControllerButton button in
        //         System.Enum.GetValues(typeof(ControllerButton)))
        //{
        // if (ViveInput.GetPressDown(LeftHand, button))
        // {
        //  Debug.Log($"按下按钮：{button}");
        // }
        //}
        //foreach (ControllerButton button in
        //         System.Enum.GetValues(typeof(ControllerButton)))
        //{
        // if (ViveInput.GetPress(RightHand, button))
        // {
        //  Debug.Log($"按下按钮：{button}");
        // }
        //}

        ////前进
        //if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.DPadUpTouch))
        //{

        //}
        ////后退
        //if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.DPadDownTouch))
        //{

        //}
        ////向左
        //if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.DPadLeftTouch))
        //{

        //}
        ////向右
        //if (ViveInput.GetPress(HandRole.RightHand, ControllerButton.DPadRightTouch))
        //{

        //}
        CheckTrigger(ViveRoleProperty.New(RightHand), RightHandTrans);
    }

	#endregion

	#region 私有函数/业务逻辑

	void CheckTrigger(ViveRoleProperty handRole, Transform handTrans)
	{
        // Trigger 按下瞬间
        if (ViveInput.GetPressDown(handRole, ControllerButton.Trigger))
        {
            if (!handTrans) return;

            // 用射线找物体
            if (Physics.Raycast(
                handTrans.position,
                handTrans.forward,
                out RaycastHit hit,
                RayDistance,
                DetectLayer))
            {
                Debug.Log(
                    $"[{handRole.roleValue}] Trigger Press Down → {hit.collider.gameObject.name}"
                );
            }
            else
            {
                Debug.Log(
                    $"[{handRole.roleValue}] Trigger Press Down → Nothing"
                );
            }
        }
    }

        #endregion

        #region 公共函数/业务逻辑



        #endregion
    }
