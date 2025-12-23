using UnityEngine;

/// <summary>
/// 自由漫游相机
/// lidongwei
/// </summary>
public class FlyCamera : MonoBehaviour
{
    public float lookSpeed = 50f;
    public float moveSpeed = 10f;

    private float rotationX;
    private float rotationY;


    /// <summary>
    /// 是否前侧被遮挡
    /// </summary>
    private bool m_IsForwardTrig;

    /// <summary>
    /// 是否后侧被遮挡
    /// </summary>
    private bool m_IsBackTrig;

    /// <summary>
    /// 是否左侧被遮挡
    /// </summary>
    private bool m_IsLeftTrig;

    /// <summary>
    /// 是否右侧被遮挡
    /// </summary>
    private bool m_IsRightTrig;

    /// <summary>
    /// 横向（Input.GetAxis("Horizontal")）输入值
    /// </summary>
    private float m_HoriValue;

    /// <summary>
    /// 纵向（Input.GetAxis("Vertical")）输入值
    /// </summary>
    private float m_VerValue;

    /// <summary>
    /// 滚轮（Input.GetAxis("Mouse ScrollWheel")）输入值
    /// </summary>
    private float m_ScrollValue;

    private void Start()
    {
        rotationX = transform.localRotation.eulerAngles.y;
    }

    // Update is called once per frame
    private void Update()
    {
        //在UI上不触发
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetMouseButton(1))
        {
            rotationX += Input.GetAxis("Mouse X") * lookSpeed * Time.deltaTime;
            rotationY += Input.GetAxis("Mouse Y") * lookSpeed * Time.deltaTime;
            transform.localRotation = Quaternion.AngleAxis(rotationX, Vector3.up) * Quaternion.AngleAxis(rotationY, Vector3.left);
        }

        int scale = 1;
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            scale = 2;
        }

        WanderRayCheck();

        m_HoriValue = Input.GetAxis("Horizontal");
        m_VerValue = Input.GetAxis("Vertical");
        m_ScrollValue = Input.GetAxis("Mouse ScrollWheel") * 400 * Time.deltaTime;

        if (m_IsForwardTrig)
        {
            m_VerValue = Mathf.Clamp(m_VerValue, -1.0f, 0.0f);
        }

        if (m_IsBackTrig)
        {
            m_VerValue = Mathf.Clamp(m_VerValue, 0.0f, 1.0f);
        }

        if (m_IsLeftTrig)
        {
            m_HoriValue = Mathf.Clamp(m_HoriValue, 0.0f, 1.0f);
        }

        if (m_IsRightTrig)
        {
            m_HoriValue = Mathf.Clamp(m_HoriValue, -1.0f, 0.0f);
        }

        transform.Translate(m_HoriValue * moveSpeed * scale * Time.deltaTime, 0, (m_VerValue + m_ScrollValue) * moveSpeed * scale * Time.deltaTime);

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(0, -moveSpeed / 2 * Time.deltaTime, 0);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(0, moveSpeed / 2 * Time.deltaTime, 0);
        }
    }

    private void WanderRayCheck()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward + Vector3.down * 0.25f);
        RaycastHit fwdHit;
        if (Physics.Raycast(transform.position, fwd, out fwdHit, 1.5f))
        {
            m_IsForwardTrig = true;
        }
        else
            m_IsForwardTrig = false;


        Vector3 bak = transform.TransformDirection(Vector3.back + Vector3.down * 0.25f);
        RaycastHit bakHit;
        if (Physics.Raycast(transform.position, bak, out bakHit, 1.5f))
        {
            m_IsBackTrig = true;
        }
        else
            m_IsBackTrig = false;


        Vector3 lft = transform.TransformDirection(Vector3.left + Vector3.down * 0.25f);

        RaycastHit lftHit;
        if (Physics.Raycast(transform.position, lft, out lftHit, 1.5f))
        {
            m_IsLeftTrig = true;
        }
        else
        {
            m_IsLeftTrig = false;
        }

        Vector3 rgt = transform.TransformDirection(Vector3.right + Vector3.down * 0.25f);
        RaycastHit rgtHit;
        if (Physics.Raycast(transform.position, rgt, out rgtHit, 1.5f))
        {
            m_IsRightTrig = true;
        }
        else
            m_IsRightTrig = false;
    }
}