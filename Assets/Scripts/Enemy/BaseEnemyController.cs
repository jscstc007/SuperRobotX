using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BaseEnemyController : MonoBehaviour
{
    private bool isUsing = false;
    /// <summary>
    /// 是否处于激活状态
    /// </summary>
    public bool IsUsing
    {
        get
        {
            return isUsing;
        }
        set
        {
            isUsing = value;
            //TODO
        }
    }

    /// <summary>
    /// 自身
    /// </summary>
    public Transform cacheTransform;
    /// <summary>
    /// 自身SpriteRender
    /// </summary>
    public SpriteRenderer cacheSpriteRender;

    /// <summary>
    /// 敌人数据
    /// </summary>
    public BaseEnemyInfo baseEnemyInfo;

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitEnemyData(Transform self, BaseEnemyInfo baseEnemyInfo)
    {
        cacheTransform = self;
        cacheSpriteRender = self.GetComponent<SpriteRenderer>();

        this.baseEnemyInfo = baseEnemyInfo;

        ResumeAndInitData();
    }

    /// <summary>
    /// 初始化数据(仅重置位置和血量)
    /// </summary>
    public void InitEnemyData()
    {
        baseEnemyInfo.HP = baseEnemyInfo.maxHP;

        ResumeAndInitData();
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void ResumeAndInitData()
    {
        IsUsing = true;
        cacheSpriteRender.enabled = true;

        //设置位置信息
        cacheTransform.localPosition = new Vector3(Random.Range(-10, 10), 12, 0);//TODO
        cacheTransform.localEulerAngles = Vector3.zero;
        cacheTransform.localScale = 5 * Vector3.one;
    }

    /// <summary>
    /// 设置为不使用状态
    /// </summary>
    public void SetNotUse()
    {
        IsUsing = false;

        cacheSpriteRender.enabled = false;
    }

    private void Update()
    {
        if (GameController.IsGame && !GameController.IsPause)
        {
            if (IsUsing)
            {
                //朝向玩家并追击
                if (null != GameController.MainRobot)
                {
                    Vector3 forwardDir = GameController.MainRobotTransform.position - cacheTransform.position;

                    cacheTransform.up = forwardDir.normalized;
                }
                cacheTransform.localPosition += baseEnemyInfo.moveSpeed * Time.deltaTime * cacheTransform.up;
            }
        }
    }

    /// <summary>
    /// 命中后闪烁
    /// </summary>
    public void DoFlash ()
    {
        //TODO
    }

    /// <summary>
    /// 撞击
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsUsing)
        {
            //敌军撞击了我方
            if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
            {
                RobotInfo.Instance.HP -= baseEnemyInfo.hitPower;

                //自杀式攻击
                SetNotUse();
            }
        }
    }

    /// <summary>
    /// 战斗函数
    /// </summary>
    private void AutoFight()
    {
        //武器模块
        if (null != baseEnemyInfo.weaponModule)
        {
            foreach (BaseWeaponInfo weapon in baseEnemyInfo.weaponModule)
            {
                int speed = weapon.shootSpeed + weapon.shootSpeedUpPerLevel * weapon.weaponLevel;
                int power = weapon.shootPower + weapon.shootPowerUpPerLevel * weapon.weaponLevel;

                int frame = WeaponInfo.GetWeaponShootFrame(speed);

                if (Time.frameCount % frame == 0)
                {
                    PoolManager.Instance.CreateBullet(weapon.weaponBulletType, cacheTransform, power);
                }
            }
        }
    }
}
