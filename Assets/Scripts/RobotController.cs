using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
/// <summary>
/// 控制我方机器人
/// </summary>
public class RobotController : MonoBehaviour {

    private SpriteRenderer robotSprite;
    public SpriteRenderer RobotSprite
    {
        get
        {
            return robotSprite;
        }
    }
    private Transform robotTransform;
    public Transform RobotTransform
    {
        get
        {
            return robotTransform;
        }
    }

    //临时数据 记录鼠标按下时的位置
    private Vector3 tempMouseDownPoint;

    private void Awake()
    {
        robotSprite = GetComponent<SpriteRenderer>();
        robotTransform = GetComponent<Transform>();
    }

    // Use this for initialization
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        //操控
        ControlRobot();
        //战斗
        AutoFight();
    }

    /// <summary>
    /// 机器人控制函数
    /// </summary>
    private void ControlRobot()
    {
        Vector2 detalPos = Vector2.zero;

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            //Reset
            tempMouseDownPoint = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            //计算偏移量 并更新临时位置
            Vector3 hitPoint = Input.mousePosition;
            Vector3 tempDeltaPos = hitPoint - tempMouseDownPoint;
            tempMouseDownPoint = hitPoint;
            detalPos = new Vector2(tempDeltaPos.x, tempDeltaPos.y);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            //Release
            tempMouseDownPoint = Vector3.zero;
        }
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            detalPos = Input.GetTouch(0).deltaPosition;
        }
#endif

        //进行位置修订
        Vector3 finalPos = RobotTransform.localPosition + new Vector3(detalPos.x, detalPos.y) * Time.deltaTime * RobotInfo.Instance.Speed;
        finalPos.x = Mathf.Clamp(finalPos.x ,- 5, 5);
        finalPos.y = Mathf.Clamp(finalPos.y ,- 8, 8);

        RobotTransform.localPosition = finalPos;
    }

    /// <summary>
    /// 机器人战斗函数
    /// </summary>
    private void AutoFight ()
    {
        if (Time.frameCount % 30 == 0)
        {
            PoolManager.Instance.CreateBullet(BulletType.Bullet_Base, RobotTransform);
        }
    }
}
