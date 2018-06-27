using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBulletController : MonoBehaviour
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
    /// 父节点(发射器)
    /// </summary>
    public Transform parentTransform;
    /// <summary>
    /// 自身
    /// </summary>
    public Transform cacheTransform;
    /// <summary>
    /// 自身
    /// </summary>
    public SpriteRenderer cacheSpriteRender;

    /// <summary>
    /// 子弹信息
    /// </summary>
    public BaseBulletInfo baseBulletInfo;

    /// <summary>
    /// 初始化子弹数据
    /// </summary>
    public void InitBulletData(Transform parent, Transform self, BaseBulletInfo bulletInfo)
    {
        parentTransform = parent;
        cacheTransform = self;
        cacheSpriteRender = self.GetComponent<SpriteRenderer>();
        baseBulletInfo = bulletInfo;

        ResumeAndInitPosition();
    }

    /// <summary>
    /// 初始化子弹数据(仅变更父物体,其他数据保持不变)
    /// </summary>
    public void InitBulletData(Transform parent)
    {
        parentTransform = parent;
        ResumeAndInitPosition();
    }

    /// <summary>
    /// 初始化子弹数据(仅变更父物体和伤害,其他数据保持不变)
    /// </summary>
    public void InitBulletData(Transform parent, int power)
    {
        parentTransform = parent;
        baseBulletInfo.power = power;
        ResumeAndInitPosition();
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void ResumeAndInitPosition()
    {
        IsUsing = true;
        NowLastTime = 0;
        cacheSpriteRender.enabled = true;

        //设置位置信息
        cacheTransform.localPosition = parentTransform.localPosition;
        cacheTransform.localEulerAngles = parentTransform.localEulerAngles;
        cacheTransform.localScale = Vector3.one;
    }

    /// <summary>
    /// 设置为不使用状态
    /// </summary>
    public void SetNotUse()
    {
        IsUsing = false;

        cacheSpriteRender.enabled = false;
    }

    /// <summary>
    /// 当前已持续时间
    /// </summary>
    private float NowLastTime = 0f;

    private void Update()
    {
        if (GameController.IsGame && !GameController.IsPause)
        {
            if (IsUsing)
            {
                //有时间控制
                if (baseBulletInfo.maxLastTime != 0)
                {
                    NowLastTime += Time.deltaTime;
                    if (NowLastTime >= baseBulletInfo.maxLastTime)
                    {
                        SetNotUse();
                    }
                }

                //根据追击类别进行移动
                if (baseBulletInfo.bulletSearchTargetType == BulletSearchTargetType.GoForward)
                {
                    cacheTransform.localPosition += baseBulletInfo.moveSpeed * Time.deltaTime * Vector3.up;

                    //超边界判定 TODO
                }
                else if (baseBulletInfo.bulletSearchTargetType == BulletSearchTargetType.GoTarget)
                {
                    //TODO
                }
            }
        }

    }

    /// <summary>
    /// 子弹命中了对象
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (IsUsing)
        {
            //子弹击中了敌人
            if (collision.gameObject.CompareTag(TagManager.TAG_ENEMY))
            {
                BaseEnemyController enemyController = collision.gameObject.GetComponent<BaseEnemyController>();

                if (enemyController.IsUsing)
                {
                    //Enemy Flash
                    enemyController.DoFlash();
                    //HP Check 
                    enemyController.baseEnemyInfo.HP -= baseBulletInfo.power;

                    if (enemyController.baseEnemyInfo.HP <= 0)
                    {
                        enemyController.SetNotUse();

                        //增加得分
                        RobotInfo.Instance.Score += enemyController.baseEnemyInfo.score;

                        EventManager.Instance.ExcuteEvent(EventType.PlayerScoreChange, null);
                    }

                    //子弹消失
                    SetNotUse();
                }
            }
            //子弹击中了玩家
            if (collision.gameObject.CompareTag(TagManager.TAG_PLAYER))
            {
                //TODO
            }
        }
    }
}
