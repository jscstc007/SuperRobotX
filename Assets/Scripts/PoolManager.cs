using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Enemy Data

public enum EnemyType
{
    Enemy_Base,
}
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
    /// 生命值
    /// </summary>
    public int HP = 100;

    /// <summary>
    /// 初始化数据
    /// </summary>
    public void InitEnemyData(Transform self, float speed, int hp)
    {
        CacheTransform = self;
        CacheSpriteRender = self.GetComponent<SpriteRenderer>();
        Speed = speed;
        HP = hp;

        ResumeAndInitPosition();
    }

    /// <summary>
    /// 初始化数据(仅重置位置)
    /// </summary>
    public void InitEnemyData()
    {
        ResumeAndInitPosition();
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void ResumeAndInitPosition()
    {
        IsUsing = true;
        CacheSpriteRender.enabled = true;

        //设置位置信息
        CacheTransform.localPosition = new Vector3(Random.Range(-10, 10), 12, 0);//TODO
        CacheTransform.localEulerAngles = Vector3.zero;
        CacheTransform.localScale = 5 * Vector3.one;
    }

    /// <summary>
    /// 设置为不使用状态
    /// </summary>
    public void SetNotUse()
    {
        IsUsing = false;

        CacheSpriteRender.enabled = false;
    }

    private void Update()
    {
        if (IsUsing)
        {
            //朝向玩家并追击
            if (null != GameController.MainRobot)
            {
                Vector3 forwardDir = GameController.MainRobotTransform.position - CacheTransform.position;

                CacheTransform.up = forwardDir;
            }
            CacheTransform.localPosition += Speed * Time.deltaTime * CacheTransform.up;
        }
    }

    /// <summary>
    /// 撞击
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsUsing)
        {
            //敌军撞击了我方
            if (collision.gameObject.CompareTag("Player"))
            {
                //TODO
                RobotInfo.Instance.HP -= 15;

                SetNotUse();
            }
        }
    }
}

#endregion

#region BulletData

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
    public void InitBulletData(Transform parent, Transform self, float speed, BulletSearchTargetType searchTargetType, float maxTime)
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
    private void ResumeAndInitPosition()
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
    public void SetNotUse()
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
                
                //超边界判定 TODO
            }
            else if (SearchTargetType == BulletSearchTargetType.GoTarget)
            {
                //TODO
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
            if (collision.gameObject.CompareTag("Enemy"))
            {
                BaseEnemyController enemyController = collision.gameObject.GetComponent<BaseEnemyController>();
                //TODO
                enemyController.SetNotUse();

                //子弹消失
                SetNotUse();
            }
            //子弹击中了玩家
            if (collision.gameObject.CompareTag("Player"))
            {
                //TODO
            }
        }
    }
}

#endregion

public class PoolManager : ISingleton<PoolManager>
{

    private Dictionary<EnemyType, List<BaseEnemyController>> EnemyPool = new Dictionary<EnemyType, List<BaseEnemyController>>();
    private Dictionary<BulletType, List<BaseBulletController>> BulletPool = new Dictionary<BulletType, List<BaseBulletController>>();

    /// <summary>
    /// 激活一个敌军
    /// </summary>
    public void CreateEnemy(EnemyType type)
    {
        bool needCreate = true;

        //初始化
        if (!EnemyPool.ContainsKey(type))
        {
            EnemyPool.Add(type, new List<BaseEnemyController>());
        }

        //尝试查找可复用资源
        foreach (BaseEnemyController tempController in EnemyPool[type])
        {
            //如果有可复用的则无需生成新的
            if (!tempController.IsUsing)
            {
                needCreate = false;
                //更新数据
                tempController.InitEnemyData();

                break;
            }
        }

        //否则生成新的
        if (needCreate)
        {
            EnemyPool[type].Add(CreateEnemyUtil(type));
        }
    }

    private BaseEnemyController CreateEnemyUtil(EnemyType type)
    {
        //根据子弹类型 生成数据
        string enemyName = "enemy_0";
        float moveSpeed = 3f;
        int hp = 10;

        GameObject enemyGo = Resources.Load<GameObject>(enemyName);
        GameObject enemy = GameObject.Instantiate<GameObject>(enemyGo);
        enemy.transform.SetParent(GameController.Instance.EnemyGroup);

        BaseEnemyController enemyController = enemy.AddComponent<BaseEnemyController>();
        enemyController.InitEnemyData(enemy.transform, moveSpeed, hp);

        return enemyController;
    }

    /// <summary>
    /// 激活一个子弹
    /// </summary>
    public void CreateBullet(BulletType type, Transform parent)
    {
        bool needCreate = true;

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
                //更新数据
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
    private BaseBulletController CreateBulletUtil(BulletType type, Transform parent)
    {
        //根据子弹类型 生成数据
        string bulletName = "bullet_0";
        float moveSpeed = 5f;
        BulletSearchTargetType searchTargetType = BulletSearchTargetType.GoForward;
        float maxTime = 5f;

        GameObject bulletGo = Resources.Load<GameObject>(bulletName);
        GameObject bullet = GameObject.Instantiate<GameObject>(bulletGo);
        bullet.transform.SetParent(GameController.Instance.BulletGroup);

        BaseBulletController bulletController = bullet.AddComponent<BaseBulletController>();
        bulletController.InitBulletData(parent, bullet.transform, moveSpeed, searchTargetType, maxTime);

        return bulletController;
    }

    /// <summary>
    /// 清理所有的敌人与子弹
    /// </summary>
    public void ClearAllEnemyAndBullet()
    {
        foreach (List<BaseEnemyController> enemyList in EnemyPool.Values)
        {
            foreach (BaseEnemyController eController in enemyList)
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

