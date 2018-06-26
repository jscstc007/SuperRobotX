using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Enemy_Base,
}
public class BaseEnemyController : MonoBehaviour
{
    //TODO
}
public enum BulletType
{
    Bullet_Base,
}
public enum BulletSearchTargetType
{
    GoForward,
    GoTarget,
}

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
    public Transform ParentTransform;
    /// <summary>
    /// 自身
    /// </summary>
    public Transform CacheTransform;
    /// <summary>
    /// 自身
    /// </summary>
    public SpriteRenderer CacheSpriteRender;
    /// <summary>
    /// 移动速度
    /// </summary>
    public float Speed = 2f;
    /// <summary>
    /// 追击敌人的方式
    /// </summary>
    public BulletSearchTargetType SearchTargetType = BulletSearchTargetType.GoForward;
    /// <summary>
    /// 最大存在时间(0为无限)
    /// </summary>
    public float MaxLastTime = 5f;

    /// <summary>
    /// 初始化子弹数据
    /// </summary>
    public void InitBulletData (Transform parent,Transform self,float speed,BulletSearchTargetType searchTargetType,float maxTime)
    {
        ParentTransform = parent;
        CacheTransform = self;
        CacheSpriteRender = self.GetComponent<SpriteRenderer>();
        Speed = speed;
        SearchTargetType = searchTargetType;
        MaxLastTime = maxTime;

        ResumeAndInitPosition();
    }

    /// <summary>
    /// 初始化子弹数据(仅变更父物体,其他数据保持不变)
    /// </summary>
    public void InitBulletData(Transform parent)
    {
        ParentTransform = parent;

        ResumeAndInitPosition();
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void ResumeAndInitPosition ()
    {
        IsUsing = true;
        NowLastTime = 0;
        CacheSpriteRender.enabled = true;

        //设置位置信息
        CacheTransform.localPosition = ParentTransform.localPosition;
        CacheTransform.localEulerAngles = ParentTransform.localEulerAngles;
        CacheTransform.localScale = Vector3.one;
    }

    /// <summary>
    /// 设置为不使用状态
    /// </summary>
    public void SetNotUse ()
    {
        IsUsing = false;

        CacheSpriteRender.enabled = false;
    }

    /// <summary>
    /// 当前已持续时间
    /// </summary>
    private float NowLastTime = 0f;

    private void Update()
    {
        if (IsUsing)
        {
            //有时间控制
            if (MaxLastTime != 0)
            {
                NowLastTime += Time.deltaTime;
                if (NowLastTime >= MaxLastTime)
                {
                    SetNotUse();
                }
            }

            //根据追击类别进行移动
            if (SearchTargetType == BulletSearchTargetType.GoForward)
            {
                CacheTransform.localPosition += Speed * Time.deltaTime * Vector3.up;

                //命中判定 TODO

                //超边界判定 TODO
            }
            else if (SearchTargetType == BulletSearchTargetType.GoTarget)
            {
                //TODO
            }
        }
    }
}

public class PoolManager : ISingleton<PoolManager> {

    private Dictionary<EnemyType, List<BaseEnemyController>> EnemyPool = new Dictionary<EnemyType, List<BaseEnemyController>>();
    private Dictionary<BulletType, List<BaseBulletController>> BulletPool = new Dictionary<BulletType, List<BaseBulletController>>();

    /// <summary>
    /// 激活一个敌军
    /// </summary>
    public void CreateEnemy (EnemyType type)
    {
        //TODO
    }

    /// <summary>
    /// 激活一个子弹
    /// </summary>
    public void CreateBullet(BulletType type,Transform parent)
    {
        bool needCreate = true;

        Debug.Log("CreateBullet");

        //初始化
        if (!BulletPool.ContainsKey(type))
        {
            BulletPool.Add(type, new List<BaseBulletController>());
        }

        //尝试查找可复用资源
        foreach (BaseBulletController tempController in BulletPool[type])
        {
            //如果有可复用的则无需生成新的
            if (!tempController.IsUsing)
            {
                needCreate = false;
                //更新子弹数据
                tempController.InitBulletData(parent);

                break;
            }
        }

        //否则生成新的
        if (needCreate)
        {
            BulletPool[type].Add(CreateBulletUtil(type, parent));
        }
    }

    /// <summary>
    /// 创建一个新的子弹
    /// </summary>
    private BaseBulletController CreateBulletUtil (BulletType type,Transform parent)
    {
        //根据子弹类型 生成数据
        string bulletName = "bullet_0";
        float moveSpeed = 2f;
        BulletSearchTargetType searchTargetType = BulletSearchTargetType.GoForward;
        float maxTime = 10f;

        GameObject bulletGo = Resources.Load<GameObject>(bulletName);
        GameObject bullet = GameObject.Instantiate<GameObject>(bulletGo);
        bullet.transform.SetParent( GameController.Instance.BulletGroup );

        BaseBulletController bulletController = bullet.AddComponent<BaseBulletController>();
        bulletController.InitBulletData(parent, bullet.transform, moveSpeed, searchTargetType, maxTime);

        return bulletController;
    }

    /// <summary>
    /// 清理所有的敌人与子弹
    /// </summary>
    public void ClearAllEnemyAndBullet ()
    {
        foreach(List<BaseEnemyController> enemyList in EnemyPool.Values)
        {
            foreach(BaseEnemyController eController in enemyList)
            {
                //TODO
            }
        }

        foreach (List<BaseBulletController> bulletList in BulletPool.Values)
        {
            foreach (BaseBulletController bController in bulletList)
            {
                bController.SetNotUse();
}
        }
    }
}
