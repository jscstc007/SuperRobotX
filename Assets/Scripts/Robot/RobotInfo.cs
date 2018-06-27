﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotInfo : ISingleton<RobotInfo> {

    private float speed = 0.6f;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float Speed
    {
        get
        {
            return speed;
        }
    }

    private int hp = 100;
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
            if (hp >= MaxHP)
            {
                hp = MaxHP;
            }

            EventManager.Instance.ExcuteEvent(EventType.PlayerHPChange, hp);
        }
    }
    /// <summary>
    /// 最大生命值
    /// </summary>
    public int MaxHP = 100;

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

    private BaseWeaponInfo[] weaponModule;
    /// <summary>
    /// 当前武器模块
    /// </summary>
    public BaseWeaponInfo[] WeaponModule
    {
        get
        {
            //TODO
            BaseWeaponInfo[] test = new BaseWeaponInfo[] { new BaseWeaponInfo(WeaponType.Base_BB, "测试", BulletType.Base_Lv1, 5, 20, 20, 2, 15, 2) };
            return test;

            //return weaponModule;
        }
        set
        {
            weaponModule = value;
        }
    }
}
