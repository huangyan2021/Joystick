using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickUI : MonoBehaviour
{
    //摇杆半径
    private float m_joyRadius = 100;
    //摇杆外围起始位置
    private Vector2 m_joyRangeBeginPos = Vector2.zero;
    //摇杆中心起始位置
    private Vector2 m_joyCenterBeginPos = Vector2.zero;
    //接受事件类
    private ClickEventListener m_joystickEvent;
    //玩家移动方向
    public Vector3 playerMoveDir;
    //接受点击检测
    public RectTransform joystickDrag;
    //摇杆背景
    public RectTransform joystickBg;
    //摇杆移动点
    public RectTransform joystickHandle;
    //摇杆指针
    public Transform joystickArrow;
    //UI摄像机
    public Camera UICamera;
    //是否自由移动
    public bool isFree = true;

    private void Start()
    {
        //拖拽事件相关
        m_joystickEvent = ClickEventListener.Get(joystickDrag.gameObject);
        m_joystickEvent.SetPointerDownHandler(OnPointerDown);
        m_joystickEvent.SetPointerUpHandler(OnPointerUp);
        m_joystickEvent.SetDragHandler(OnDrag);
        //摇杆检测坐标初始化
        joystickDrag.localPosition = Vector3.zero;
        //默认显示摇杆背景
        joystickBg.gameObject.SetActive(true);
        //摇杆相关坐标初始化
        RestoreJoystick();
    }

    private void OnPointerDown(PointerEventData eventData)
    {
        //屏幕坐标和UI坐标的转换
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBg, eventData.position, UICamera, out m_joyRangeBeginPos);
        if (isFree)
        {
            //摇杆背景移动到点击位置
            joystickBg.anchoredPosition = m_joyRangeBeginPos;
        }
        //摇杆中心起始位置
        m_joyCenterBeginPos = joystickHandle.anchoredPosition;
        //显示摇杆
        ShowJoystick(true);
    }

    private void OnDrag(PointerEventData eventData)
    {
        //屏幕坐标和UI坐标的转换
        RectTransformUtility.ScreenPointToLocalPointInRectangle(joystickBg, eventData.position, UICamera, out Vector2 handlePos);
        //摇杆移动方向
        var direction = handlePos - m_joyCenterBeginPos;
        //摇杆移动距离
        var dis = direction.magnitude;
        //限制 joystickHandle 移动范围
        var radius = Mathf.Clamp(dis, 0, m_joyRadius); 
        //设置摇杆移动点的移动位置
        joystickHandle.anchoredPosition = direction.normalized * radius;
        //获取摇杆指针的旋转角度
        var angle = Quaternion.FromToRotation(Vector3.up, playerMoveDir).eulerAngles.z;
        //设置摇杆指针的旋转角度
        joystickArrow.localRotation = Quaternion.Euler(0, 0, angle);
        //设置玩家移动方向
        playerMoveDir = direction.normalized;
    }
        
    private void OnPointerUp(PointerEventData eventData)
    {
        RestoreJoystick();
    }
    
    /// <summary>
    /// 恢复摇杆默认值
    /// </summary>
    private void RestoreJoystick()
    {
        ShowJoystick(false);
        joystickHandle.anchoredPosition = Vector2.zero;
        joystickBg.anchoredPosition = Vector2.zero;
        joystickArrow.localEulerAngles = Vector3.zero;
    }
    
    private void ShowJoystick(bool isShow)
    {
        //如果是自由移动才控制动态显示
        if (isFree)
        {
            joystickBg.gameObject.SetActive(isShow);
        }
    }
}
