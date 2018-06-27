using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Base_Lv1 = 30101,
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

    public BaseEnemyInfo(EnemyType enemyType, string enemyResName, float moveSpeed, int hP, int maxHP, BaseWeaponInfo[] weaponModule,int score,int hitPower)
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

public class EnemyInfo : ISingleton<EnemyInfo> {

	//public BaseEnemyInfo GetEnemyData (EnemyType type)
 //   {

 //   }
}
