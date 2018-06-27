using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    Base_BB = 20001,
}

public class BaseWeaponInfo
{
    /// <summary> 武器类别(唯一) </summary>
    public WeaponType weaponType;
    /// <summary> 武器名称 </summary>
    public string weaponName;
    /// <summary> 武器子弹类别 </summary>
    public BulletType weaponBulletType;
    /// <summary> 武器等级 </summary>
    public int weaponLevel;
    /// <summary> 武器最大等级 </summary>
    public int weaponMaxLevel;
    /// <summary> 武器速度 </summary>
    public int shootSpeed;
    /// <summary> 武器速度提升 </summary>
    public int shootSpeedUpPerLevel;
    /// <summary> 武器威力 </summary>
    public int shootPower;
    /// <summary> 武器威力提升 </summary>
    public int shootPowerUpPerLevel;

    public BaseWeaponInfo(WeaponType weaponType, string weaponName, BulletType weaponBulletType, int weaponLevel, int weaponMaxLevel, int shootSpeed, int shootSpeedUpPerLevel, int shootPower, int shootPowerUpPerLevel)
    {
        this.weaponType = weaponType;
        this.weaponName = weaponName;
        this.weaponBulletType = weaponBulletType;
        this.weaponLevel = weaponLevel;
        this.weaponMaxLevel = weaponMaxLevel;
        this.shootSpeed = shootSpeed;
        this.shootSpeedUpPerLevel = shootSpeedUpPerLevel;
        this.shootPower = shootPower;
        this.shootPowerUpPerLevel = shootPowerUpPerLevel;
    }
}

public class WeaponInfo : ISingleton<WeaponInfo> {
    
    /// <summary>
    /// 根据子弹速度获得设计频率
    /// </summary>
    public static int GetWeaponShootFrame (int speed)
    {
        //TODO
        return (60 * 10 / speed);
    }

    /// <summary>
    /// 获得一个武器信息(默认为0级武器)
    /// </summary>
    public BaseWeaponInfo GetBaseWeaponData (WeaponType type,int weaponLevel = 0)
    {
        //TODO
        return null;
    }
}
