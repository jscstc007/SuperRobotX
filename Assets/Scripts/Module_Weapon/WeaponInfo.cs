using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;
using System;

public enum WeaponType
{
    Base_BB_Lv1 = 20101,
    Base_BB_Lv2 = 20102,
    Base_BB_Lv3 = 20103,
}

public class BaseWeaponInfo
{
    /// <summary> 武器类别(唯一) </summary>
    public WeaponType weaponType;
    /// <summary> 武器名称 </summary>
    public string weaponName;
    /// <summary> 武器res名称 </summary>
    public string weaponResName;
    /// <summary> 武器信息 </summary>
    public string weaponInfo;
    /// <summary> 武器子弹类别 </summary>
    public BulletType weaponBulletType;
    /// <summary> 武器等级 </summary>
    public int weaponLevel;
    /// <summary> 武器点数消耗 </summary>
    public int weaponPointsCost;
    /// <summary> 武器速度 </summary>
    public int shootSpeed;
    /// <summary> 武器速度提升 </summary>
    public int shootSpeedUpPerLevel;
    /// <summary> 武器威力 </summary>
    public int shootPower;
    /// <summary> 武器威力提升 </summary>
    public int shootPowerUpPerLevel;
    /// <summary> 武器升级消耗 </summary>
    public int weaponUpgradeCost;
    /// <summary> 武器升级消耗提升 </summary>
    public int weaponUpgradeCostUp;

    public BaseWeaponInfo(WeaponType weaponType, string weaponName, string weaponResName, string weaponInfo, BulletType weaponBulletType, int weaponLevel, int weaponPointsCost, int shootSpeed, int shootSpeedUpPerLevel, int shootPower, int shootPowerUpPerLevel, int weaponUpgradeCost, int weaponUpgradeCostUp)
    {
        this.weaponType = weaponType;
        this.weaponName = weaponName;
        this.weaponResName = weaponResName;
        this.weaponInfo = weaponInfo;
        this.weaponBulletType = weaponBulletType;
        this.weaponLevel = weaponLevel;
        this.weaponPointsCost = weaponPointsCost;
        this.shootSpeed = shootSpeed;
        this.shootSpeedUpPerLevel = shootSpeedUpPerLevel;
        this.shootPower = shootPower;
        this.shootPowerUpPerLevel = shootPowerUpPerLevel;
        this.weaponUpgradeCost = weaponUpgradeCost;
        this.weaponUpgradeCostUp = weaponUpgradeCostUp;
    }
}

public class WeaponInfo : ISingleton<WeaponInfo>
{
    /// <summary>
    /// 最大武器等级
    /// </summary>
    public const int MAX_WEAPON_LEVEL = 99;

    /// <summary>
    /// 已加载过的武器数据
    /// </summary>
    private Dictionary<WeaponType, BaseWeaponInfo> LoadedData = new Dictionary<WeaponType, BaseWeaponInfo>();

    private static JsonData weaponJsonData;
    private static JsonData WeaponJsonData
    {
        get
        {
            if (null == weaponJsonData)
            {
                weaponJsonData = ResourcesLoadManager.LoadJsonData("weapon");
            }
            return weaponJsonData;
        }
    }

    /// <summary>
    /// 获取武器的基础数据(默认为0级武器)
    /// </summary>
    public BaseWeaponInfo LoadBaseWeaponData(WeaponType type, int weaponLevel = 0)
    {
        BaseWeaponInfo data = null;

        string key = ((int)type).ToString();
        if (!LoadedData.ContainsKey(type))
        {
            string weaponName = WeaponJsonData[key]["name"].ToString();
            string weaponInfo = WeaponJsonData[key]["info"].ToString();
            string resName = WeaponJsonData[key]["res"].ToString();
            BulletType bulletType = (BulletType)((int)WeaponJsonData[key]["bullet"]);
            int cost = (int)WeaponJsonData[key]["cost"];
            int speed = (int)WeaponJsonData[key]["shootSpeed"];
            int speedUp = (int)WeaponJsonData[key]["shootSpeedUp"];
            int power = (int)WeaponJsonData[key]["power"];
            int powerUp = (int)WeaponJsonData[key]["powerUp"];
            int upgradeCost = (int)WeaponJsonData[key]["upgradeCost"];
            int upgradeCostUp = (int)WeaponJsonData[key]["upgradeCostUp"];

            data = new BaseWeaponInfo(type, weaponName, resName, weaponInfo, bulletType, weaponLevel, cost, speed, speedUp, power, powerUp, upgradeCost, upgradeCostUp);

            //缓存
            LoadedData.Add(type, data);
        }

        return LoadedData[type];
    }

    /// <summary>
    /// 获得所有的武器Type
    /// </summary>
    /// <returns></returns>
    public static WeaponType[] GetAllWeapons ()
    {
        List<WeaponType> list = new List<WeaponType>();
       
        foreach(WeaponType type in Enum.GetValues(typeof(WeaponType)))
        {
            list.Add(type);
        }
        
        return list.ToArray();
    }

    /// <summary>
    /// 根据子弹速度获得射击频率
    /// </summary>
    public static int GetWeaponShootFrame(int speed)
    {
        //TODO
        return (60 * 10 / speed);
    }
}
