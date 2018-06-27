using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    #region 组件信息
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

    private UIController UI;
    /// <summary>
    /// UI组件
    /// </summary>
    public UIController UIControl
    {
        get
        {
            if (null == UI)
            {
                UI = GameObject.Find("/UI").GetComponent<UIController>();
            }
            return UI;
        }
    }
    #endregion

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
    public static GameObject MainRobot;
    /// <summary>
    /// 主角Transform
    /// </summary>
    public static Transform MainRobotTransform;

    /// <summary>
    /// 是否处于游戏状态
    /// </summary>
    public static bool IsGame = false;
    /// <summary>
    /// 是否处于暂停状态
    /// </summary>
    public static bool IsPause = false;

    /// <summary>
    /// 开始游戏 初始化主角
    /// </summary>
    public void CreatePlayerAndStartGame ()
    {
        IsGame = true;
        IsPause = false;

        //如果主角还存在 则销毁主角
        if (null != MainRobot)
        {
            Destroy(MainRobot);
        }

        //重置一部分数据
        RobotInfo.Instance.Score = 0;
        RobotInfo.Instance.HP = RobotInfo.Instance.MaxHP;

        //读取当前主角数据 
        //TODO
        string robotName = "Player";
        
        //根据角色数据生成主角
        GameObject robotGo = Resources.Load<GameObject>(robotName);
        MainRobot = Instantiate(robotGo);
        MainRobotTransform = MainRobot.transform;

        MainRobotTransform.SetParent(PlayerGroup);
        MainRobotTransform.localPosition = BEGIN_POS;
        MainRobotTransform.localEulerAngles = Vector3.zero;
        MainRobotTransform.localScale = 5 * Vector3.one;//TODO 

        MainRobot.AddComponent<RobotController>();

        //定时生成敌人
        InvokeRepeating("CreateEnemeyInTime",1f,1f);
    }

    /// <summary>
    /// 游戏结束
    /// </summary>
    public void GameOver ()
    {
        IsPause = true;

        UIControl.ShowGameOverUI();
    }

    /// <summary>
    /// 关闭游戏 返回主界面
    /// </summary>
    public void StopGameAndReturnToMenu ()
    {
        IsGame = false;
        IsPause = false;

        CancelInvoke("CreateEnemeyInTime");

        DestroyPlayer();
        PoolManager.Instance.ClearAllEnemyAndBullet();
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

    /// <summary>
    /// 定时生成敌人
    /// </summary>
    private void CreateEnemeyInTime ()
    {
        PoolManager.Instance.CreateEnemy(EnemyType.Base_Lv1);
    }
}
