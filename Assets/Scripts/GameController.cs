using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    private static GameController instance;
    public static GameController Instance
    {
        get
        {
            if (null == instance)
            {
                instance = GameObject.Find("/GameController").GetComponent<GameController>();
            }
            return instance;
        }
    }

    private Transform playerGroup;
    public Transform PlayerGroup
    {
        get
        {
            if (null == playerGroup)
            {
                playerGroup = GameObject.Find("/PlayerGroup").transform;
            }
            return playerGroup;
        }
    }
    private Transform enemyGroup;
    public Transform EnemyGroup
    {
        get
        {
            if (null == enemyGroup)
            {
                enemyGroup = GameObject.Find("/EnemyGroup").transform;
            }
            return enemyGroup;
        }
    }
    private Transform bulletGroup;
    public Transform BulletGroup
    {
        get
        {
            if (null == bulletGroup)
            {
                bulletGroup = GameObject.Find("/BulletGroup").transform;
            }
            return bulletGroup;
        }
    }
    private Transform otherGroup;
    public Transform OtherGroup
    {
        get
        {
            if (null == otherGroup)
            {
                otherGroup = GameObject.Find("/OtherGroup").transform;
            }
            return otherGroup;
        }
    }

    private void Awake()
    {
        //设置帧率
        Application.targetFrameRate = 60;
    }

    /// <summary>
    /// 玩家初始位置
    /// </summary>
    private static Vector3 BEGIN_POS = new Vector3(0, -8, 0);

    /// <summary>
    /// 主角
    /// </summary>
    private static GameObject MainRobot;


    /// <summary>
    /// 初始化主角并开始游戏
    /// </summary>
    public void CreatePlayerAndStartGame ()
    {
        //如果主角还存在 则销毁主角
        if (null != MainRobot)
        {
            Destroy(MainRobot);
        }

        //读取当前主角数据
        //TODO
        string robotName = "Player";

        //根据角色数据生成主角
        GameObject robotGo = Resources.Load<GameObject>(robotName);
        MainRobot = Instantiate(robotGo);

        MainRobot.transform.SetParent(PlayerGroup);
        MainRobot.transform.localPosition = BEGIN_POS;
        MainRobot.transform.localEulerAngles = Vector3.zero;
        MainRobot.transform.localScale = 5 * Vector3.one;//TODO 

        MainRobot.AddComponent<RobotController>();
    }

    /// <summary>
    /// 销毁主角
    /// </summary>
    public void DestroyPlayer ()
    {
        //如果主角还存在 则销毁主角
        if (null != MainRobot)
        {
            Destroy(MainRobot);
        }
    }
}
