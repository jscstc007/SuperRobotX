using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public enum RobotType
{
    Base_Plane = 10101,
    Base_Plane_Upgrade = 10102,
}

public class BaseRobotInfo
{
    /// <summary> 类别(唯一) </summary>
    public RobotType robotType;
    /// <summary> res名称 </summary>
    public string robotResName;
    /// <summary> robot名称 </summary>
    public string robotName;
    /// <summary> robot信息 </summary>
    public string robotInfo;
    /// <summary> 机器等级 </summary>
    public int robotLevel;
    /// <summary> 机器最大等级 </summary>
    public int robotMaxLevel;
    /// <summary> 移动速度 </summary>
    public int moveSpeed;
    /// <summary> 最大生命 </summary>
    public int maxHP;
    /// <summary> 武器节点数量 </summary>
    public int weaponPoints;
    /// <summary> 防御节点数量 </summary>
    public int defensePoints;
    /// <summary> 引擎节点数量 </summary>
    public int movePoints;
    /// <summary> 特殊节点数量 </summary>
    public int otherPoints;
    /// <summary> 升级所需金币 </summary>
    public int cost;

    public BaseRobotInfo(RobotType robotType, string robotResName, string robotName, string robotInfo, int robotLevel, int robotMaxLevel, int moveSpeed, int maxHP, int weaponPoints, int defensePoints, int movePoints, int otherPoints, int cost)
    {
        this.robotType = robotType;
        this.robotResName = robotResName;
        this.robotName = robotName;
        this.robotInfo = robotInfo;
        this.robotLevel = robotLevel;
        this.robotMaxLevel = robotMaxLevel;
        this.moveSpeed = moveSpeed;
        this.maxHP = maxHP;
        this.weaponPoints = weaponPoints;
        this.defensePoints = defensePoints;
        this.movePoints = movePoints;
        this.otherPoints = otherPoints;
        this.cost = cost;
    }
}

public class RobotInfo : ISingleton<RobotInfo>
{

    #region 基础机器人数据

    /// <summary>
    /// 已加载过的数据
    /// </summary>
    private Dictionary<RobotType, Dictionary<int, BaseRobotInfo>> LoadedData = new Dictionary<RobotType, Dictionary<int, BaseRobotInfo>>();

    private static JsonData robotJsonData;
    private static JsonData RobotJsonData
    {
        get
        {
            if (null == robotJsonData)
            {
                robotJsonData = ResourcesLoadManager.LoadJsonData("robot");
            }
            return robotJsonData;
        }
    }

    /// <summary>
    /// 获取机器人的基础数据(默认为0级)
    /// </summary>
    public BaseRobotInfo LoadBaseRobotData(RobotType type, int robotLevel = 0)
    {
        BaseRobotInfo data = null;

        string key = ((int)type).ToString();
        if (!LoadedData.ContainsKey(type))
        {
            data = LoadBaseRobotDataUtil(type, robotLevel);

            //缓存
            LoadedData.Add(type, new Dictionary<int, BaseRobotInfo>());
            LoadedData[type].Add(robotLevel, data);
        }
        else if (!LoadedData[type].ContainsKey(robotLevel))
        {
            data = LoadBaseRobotDataUtil(type, robotLevel);

            //缓存
            LoadedData[type].Add(robotLevel, data);
        }

        return LoadedData[type][robotLevel];
    }

    private BaseRobotInfo LoadBaseRobotDataUtil(RobotType type, int robotLevel = 0)
    {
        BaseRobotInfo data = null;

        string key = ((int)type).ToString();

        string robotName = RobotJsonData[key]["name"].ToString();
        string resName = RobotJsonData[key]["res"].ToString();
        string robotInfo = RobotJsonData[key]["info"].ToString();

        int maxLevel = RobotJsonData[key]["attribute"].Count;
        int speed = (int)RobotJsonData[key]["attribute"][robotLevel]["speed"];
        int maxHP = (int)RobotJsonData[key]["attribute"][robotLevel]["maxHP"];
        int weapon = (int)RobotJsonData[key]["attribute"][robotLevel]["weapon"];
        int defense = (int)RobotJsonData[key]["attribute"][robotLevel]["defense"];
        int move = (int)RobotJsonData[key]["attribute"][robotLevel]["move"];
        int other = (int)RobotJsonData[key]["attribute"][robotLevel]["other"];
        int cost = (int)RobotJsonData[key]["attribute"][robotLevel]["cost"];

        data = new BaseRobotInfo(type, resName, robotName, robotInfo, robotLevel, maxLevel, speed, maxHP, weapon, defense, move, other, cost);

        return data;
    }

    #endregion

    /// <summary>
    /// 主控机器人数据
    /// </summary>
    public BaseRobotInfo MainRobotInfo;

    /// <summary>
    /// 主控机器人等级
    /// </summary>
    public BaseRobotInfo MainRobotLevel;

    private int hp = 0;
    /// <summary>
    /// 当前生命值
    /// </summary>
    public int HP
    {
        get
        {
            return hp;
        }
        set
        {
            hp = value;
            //修订范围
            if (hp <= 0)
            {
                hp = 0;
            }
            if (hp >= MainRobotInfo.maxHP)
            {
                hp = MainRobotInfo.maxHP;
            }

            EventManager.Instance.ExcuteEvent(EventType.PlayerHPChange, hp);
        }
    }

    private int score = 0;
    /// <summary>
    /// 得分
    /// </summary>
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;

            EventManager.Instance.ExcuteEvent(EventType.PlayerScoreChange, score);
        }
    }

    /// <summary>
    /// 金币
    /// </summary>
    public int Coin
    {
        get
        {
            return DataManager.Instance.LoadCoin();
        }
        set
        {
            DataManager.Instance.SaveCoin(value);

            EventManager.Instance.ExcuteEvent(EventType.PlayerCoinChange, value);
        }
    }

    #region 模块数据

    private BaseWeaponInfo[] weaponModule;
    /// <summary>
    /// 当前武器模块
    /// </summary>
    public BaseWeaponInfo[] WeaponModule
    {
        get
        {
            return weaponModule;
        }
        set
        {
            weaponModule = value;
        }
    }

    #endregion
}
