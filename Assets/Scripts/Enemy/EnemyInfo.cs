using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public enum EnemyType
{
    Base_Lv1 = 30101,
    Base_Lv2 = 30102,
    Base_Lv3 = 30103,
}

public class BaseEnemyInfo
{
    /// <summary> 敌军类别(唯一) </summary>
    public EnemyType enemyType;
    /// <summary> res名称 </summary>
    public string enemyResName;
    /// <summary> 移动速度 </summary>
    public float moveSpeed;
    /// <summary> 生命 </summary>
    public int HP;
    /// <summary> 最大生命 </summary>
    public int maxHP;
    /// <summary> 武器模块 </summary>
    public BaseWeaponInfo[] weaponModule;
    /// <summary> 击杀得分 </summary>
    public int score;
    /// <summary> 撞击伤害 </summary>
    public int hitPower;

    public BaseEnemyInfo(EnemyType enemyType, string enemyResName, float moveSpeed, int hP, int maxHP, BaseWeaponInfo[] weaponModule, int score, int hitPower)
    {
        this.enemyType = enemyType;
        this.enemyResName = enemyResName;
        this.moveSpeed = moveSpeed;
        this.HP = hP;
        this.maxHP = maxHP;
        this.weaponModule = weaponModule;
        this.score = score;
        this.hitPower = hitPower;
    }
}

public class EnemyInfo : ISingleton<EnemyInfo>
{
    /// <summary>
    /// 已加载过的敌军数据
    /// </summary>
    private Dictionary<EnemyType, BaseEnemyInfo> LoadedData = new Dictionary<EnemyType, BaseEnemyInfo>();

    private static JsonData enemyJsonData;
    private static JsonData EnemyJsonData
    {
        get
        {
            if (null == enemyJsonData)
            {
                enemyJsonData = ResourcesLoadManager.LoadJsonData("enemy");
            }
            return enemyJsonData;
        }
    }

    /// <summary>
    /// 获取敌军的基础数据(默认为0级武器)
    /// </summary>
    public BaseEnemyInfo LoadBaseEnemyData(EnemyType type, int weaponLevel = 0)
    {
        BaseEnemyInfo data = null;

        string key = ((int)type).ToString();
        if (!LoadedData.ContainsKey(type))
        {
            string resName = EnemyJsonData[key]["res"].ToString();
            int speed = (int)EnemyJsonData[key]["speed"];
            int maxHP = (int)EnemyJsonData[key]["maxHP"];
            string weapons = EnemyJsonData[key]["weapons"].ToString();
            int hit = (int)EnemyJsonData[key]["hit"];
            int score = (int)EnemyJsonData[key]["score"];

            string[] weaponstrs = weapons.Split(',');
            int count = weaponstrs.Length;
            BaseWeaponInfo[] weaponModule = new BaseWeaponInfo[count];
            //空武器
            if (null == weaponstrs || weaponstrs.Length == 0)
            {
                weaponModule = null;
            }
            else if (weaponstrs.Length == 1 && string.IsNullOrEmpty(weaponstrs[0]))
            {
                weaponModule = null;
            }
            //更新武器数据
            else
            {
                for (int i = 0; i < count; i++)
                {
                    weaponModule[i] = WeaponInfo.Instance.LoadBaseWeaponData((WeaponType)(int.Parse(weaponstrs[i])), weaponLevel);
                }
            }
            //默认满血量
            data = new BaseEnemyInfo(type, resName, speed, maxHP, maxHP, weaponModule, score, hit);

            //缓存
            LoadedData.Add(type, data);
        }

        return LoadedData[type];
    }
}
