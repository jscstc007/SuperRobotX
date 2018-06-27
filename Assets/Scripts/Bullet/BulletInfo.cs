using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BulletType
{
    Base_Lv1 = 10101,
    Base_Lv2 = 10102,
    Base_Lv3 = 10103,
}

public enum BulletSearchTargetType
{
    GoForward,//朝前方发射
    GoTarget,//自动追踪目标
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
    public float moveSpeed;
    /// <summary> 最大存在时间(0为无限) </summary>
    public float maxLastTime;
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
    public BaseBulletInfo(BulletType bulletType, BulletSearchTargetType bulletSearchTargetType,string resName, float moveSpeed, float maxLastTime,int shootNum ,int power)
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
    /// 获得一个子弹信息
    /// </summary>
    public BaseBulletInfo GetBulletData(BulletType type,int power)
    {
        BaseBulletInfo data = null;
        switch (type)
        {
            case BulletType.Base_Lv1:
                data = new BaseBulletInfo(BulletType.Base_Lv1, BulletSearchTargetType.GoForward,"bullet_0", 3f, 10f, 1, power);
                break;

            default:
                //TODO
                break;
        }

        return data;
    }
}
