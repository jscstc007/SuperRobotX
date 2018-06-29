using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //根据类型 生成数据
        BaseEnemyInfo data = EnemyInfo.Instance.LoadBaseEnemyData(type);

        GameObject enemyGo = ResourcesLoadManager.LoadEnemyResources<GameObject>(data.enemyResName);
        GameObject enemy = GameObject.Instantiate<GameObject>(enemyGo);
        enemy.transform.SetParent(GameController.Instance.EnemyGroup);

        BaseEnemyController enemyController = enemy.AddComponent<BaseEnemyController>();
        enemyController.InitEnemyData(enemy.transform, data);

        return enemyController;
    }

    /// <summary>
    /// 激活一个子弹
    /// </summary>
    public void CreateBullet(BulletType type, Transform parent, Vector3 pos, int power)
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
                tempController.InitBulletData(parent, pos, power);

                break;
            }
        }

        //否则生成新的
        if (needCreate)
        {
            BulletPool[type].Add(CreateBulletUtil(type, parent, pos, power));
        }
    }

    /// <summary>
    /// 创建一个新的子弹
    /// </summary>
    private BaseBulletController CreateBulletUtil(BulletType type, Transform parent, Vector3 pos, int power)
    {
        //根据子弹类型 生成数据
        BaseBulletInfo data = BulletInfo.Instance.GetBulletData(type, power);

        GameObject bulletGo = ResourcesLoadManager.LoadBulletResources<GameObject>(data.bulletResName);
        GameObject bullet = GameObject.Instantiate<GameObject>(bulletGo);
        bullet.transform.SetParent(GameController.Instance.BulletGroup);

        BaseBulletController bulletController = bullet.AddComponent<BaseBulletController>();
        bulletController.InitBulletData(parent, bullet.transform, pos, data);

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
                eController.SetNotUse();
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

