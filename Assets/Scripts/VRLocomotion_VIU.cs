using HTC.UnityPlugin.Vive;
using UnityEngine;

//========================================
//作    者:HJK
//创建时间:2025.12.15
//备    注:vr移动，基于Vive Input Utility
//========================================
[RequireComponent(typeof(CharacterController))]
public class VRLocomotion_VIU : MonoBehaviour
{
	#region 私有变量
	
	private CharacterController cc;
	private Transform head;

	#endregion

	#region 公共变量
	
	[Header("Move")]
	public HandRole moveHand = HandRole.LeftHand;
	public float moveSpeed = 2.0f;
	public float moveDeadZone = 0.2f;

	//禁止条件
	[Header("Disable Conditions")]
	public bool uiActive;
	public bool paintingActive;
	
	#endregion

	#region Mono相关
	void Start()
	{
		cc = GetComponent<CharacterController>();
		head = Camera.main.transform;
	}

	void Update()
	{
		if (uiActive || paintingActive) return;

		HandleMove();
	}
	
	#endregion

	#region 私有函数

	/// <summary>
	/// 移动
	/// </summary>
	private void HandleMove()
	{
		Vector2 axis =
			ViveInput.GetPadAxis(moveHand);

		if (axis.magnitude < moveDeadZone) return;

		Vector3 forward = head.forward;
		forward.y = 0;
		forward.Normalize();

		Vector3 right = head.right;
		right.y = 0;
		right.Normalize();

		Vector3 move =
			forward * axis.y + right * axis.x;

		cc.Move(move * moveSpeed * Time.deltaTime);
	}

	#endregion

}

