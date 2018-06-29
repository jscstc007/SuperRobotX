using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class ResourcesLoadManager : ISingleton<ResourcesLoadManager> {

    /// <summary>
    /// 加载一个资源(仅加载)
    /// </summary>
	private static T LoadResources<T> (string name) where T : Object
    {
        return Resources.Load<T>(name);
    }

    /// <summary>
    /// 加载一个资源(仅加载)
    /// </summary>
    public static T LoadRobotResources<T>(string name) where T : Object
    {
        return LoadResources<T>(string.Format("Robot/{0}", name));
    }

    /// <summary>
    /// 加载一个资源(仅加载)
    /// </summary>
    public static T LoadEnemyResources<T>(string name) where T : Object
    {
        return LoadResources<T>(string.Format("Enemy/{0}", name));
    }

    /// <summary>
    /// 加载一个资源(仅加载)
    /// </summary>
    public static T LoadWeaponResources<T>(string name) where T : Object
    {
        return LoadResources<T>(string.Format("Weapon/{0}", name));
    }

    /// <summary>
    /// 加载一个资源(仅加载)
    /// </summary>
    public static T LoadBulletResources<T>(string name) where T : Object
    {
        return LoadResources<T>(string.Format("Bullet/{0}", name));
    }


    /// <summary>
    /// 获取Json数据
    /// </summary>
    public static JsonData LoadJsonData(string jsonName)
    {
        return JsonMapper.ToObject( Resources.Load<TextAsset>(string.Format("Data/{0}", jsonName)).text );
    }
}
