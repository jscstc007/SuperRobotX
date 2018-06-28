using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public enum WeaponType
{
    Base_BB = 20101,
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
    /// <summary> 武器速度 </summary>
    public int shootSpeed;
    /// <summary> 武器速度提升 </summary>
    public int shootSpeedUpPerLevel;
    /// <summary> 武器威力 </summary>
    public int shootPower;
    /// <summary> 武器威力提升 </summary>
    public int shootPowerUpPerLevel;

    public BaseWeaponInfo(WeaponType weaponType, string weaponName, string weaponResName, string weaponInfo, BulletType weaponBulletType, int weaponLevel, int shootSpeed, int shootSpeedUpPerLevel, int shootPower, int shootPowerUpPerLevel)
    {
        this.weaponType = weaponType;
        this.weaponName = weaponName;
        this.weaponResName = weaponResName;
        this.weaponInfo = weaponInfo;
        this.weaponBulletType = weaponBulletType;
        this.weaponLevel = weaponLevel;
        this.shootSpeed = shootSpeed;
        this.shootSpeedUpPerLevel = shootSpeedUpPerLevel;
        this.shootPower = shootPower;
        this.shootPowerUpPerLevel = shootPowerUpPerLevel;
    }
}

public class WeaponInfo : ISingleton<WeaponInfo> {

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
            int speed = (int)WeaponJsonData[key]["shootSpeed"];
            int speedUp = (int)WeaponJsonData[key]["shootSpeedUp"];
            int power = (int)WeaponJsonData[key]["power"];
            int powerUp = (int)WeaponJsonData[key]["powerUp"];

            data = new BaseWeaponInfo(type, weaponName, resName, weaponInfo, bulletType, weaponLevel, speed, speedUp, power, powerUp);

            //缓存
            LoadedData.Add(type, data);
        }

        return LoadedData[type];
    }

    /// <summary>
    /// 根据子弹速度获得射击频率
    /// </summary>
    public static int GetWeaponShootFrame (int speed)
    {
        //TODO
        return (60 * 10 / speed);
    }
}
