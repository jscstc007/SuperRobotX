using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public enum BulletType
{
    Base_Lv1 = 80101,
    Base_Lv2 = 80102,
    Base_Lv3 = 80103,
}

public enum BulletSearchTargetType
{
    GoForward = 1,//朝前方发射
    GoTarget ,//自动追踪目标
}

public class BaseBulletInfo
{
    /// <summary> 子弹类别(唯一) </summary>
    public BulletType bulletType;
    /// <summary> 子弹追踪方式 </summary>
    public BulletSearchTargetType bulletSearchTargetType;
    /// <summary> res名称 </summary>
    public string bulletResName;
    /// <summary> 移动速度 </summary>
    public int moveSpeed;
    /// <summary> 最大存在时间(0为无限) </summary>
    public int maxLastTime;
    /// <summary> 子弹有效次数(为0表示无限次) </summary>
    public int shootNum;

    /// <summary> 子弹伤害力 </summary>
    public int power;

    /// <summary>
    /// 初始化子弹数据
    /// </summary>
    /// <param name="bulletType"></param>
    /// <param name="bulletSearchTargetType"></param>
    /// <param name="parentTransform"></param>
    /// <param name="cacheTransform"></param>
    /// <param name="cacheSpriteRender"></param>
    /// <param name="moveSpeed"></param>
    /// <param name="maxLastTime"></param>
    public BaseBulletInfo(BulletType bulletType, BulletSearchTargetType bulletSearchTargetType,string resName, int moveSpeed, int maxLastTime,int shootNum ,int power)
    {
        this.bulletType = bulletType;
        this.bulletSearchTargetType = bulletSearchTargetType;
        this.bulletResName = resName;
        this.moveSpeed = moveSpeed;
        this.maxLastTime = maxLastTime;
        this.shootNum = shootNum;
        this.power = power;
    }
}

public class BulletInfo : ISingleton<BulletInfo> {

    /// <summary>
    /// 已加载过的数据
    /// </summary>
    private Dictionary<BulletType, BaseBulletInfo> LoadedData = new Dictionary<BulletType, BaseBulletInfo>();

    private static JsonData bulletJsonData;
    private static JsonData BulletJsonData
    {
        get
        {
            if (null == bulletJsonData)
            {
                bulletJsonData = ResourcesLoadManager.LoadJsonData("bullet");
            }
            return bulletJsonData;
        }
    }

    /// <summary>
    /// 获取基础数据(默认为0 伤害值)
    /// </summary>
    private BaseBulletInfo LoadBaseBulletData(BulletType type)
    {
        BaseBulletInfo data = null;

        string key = ((int)type).ToString();
        if (!LoadedData.ContainsKey(type))
        {
            string resName = BulletJsonData[key]["res"].ToString();
            BulletSearchTargetType searchTargetType = (BulletSearchTargetType)((int)BulletJsonData[key]["searchType"]);
            int speed = (int)BulletJsonData[key]["speed"];
            int maxTime = (int)BulletJsonData[key]["maxTime"];
            int shootNum = (int)BulletJsonData[key]["shootNum"];

            //伤害值默认为0
            data = new BaseBulletInfo(type, searchTargetType, resName, speed, maxTime, shootNum, 0);

            //缓存
            LoadedData.Add(type, data);
        }

        return LoadedData[type];
    }

    /// <summary>
    /// 获得一个子弹信息
    /// </summary>
    public BaseBulletInfo GetBulletData(BulletType type,int power)
    {
        BaseBulletInfo data = LoadBaseBulletData(type);
        data.power = power;

        return data;
    }
}
