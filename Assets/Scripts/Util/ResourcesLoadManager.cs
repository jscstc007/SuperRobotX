using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesLoadManager : ISingleton<ResourcesLoadManager> {

    /// <summary>
    /// 加载一个资源(仅加载)
    /// </summary>
	public static T LoadResources<T> (string name) where T : Object
    {
        return Resources.Load<T>(name);
    }

    /// <summary>
    /// 获取Json数据
    /// </summary>
    //private JsonUtility LoadJsonData (string jsonName)
    //{
    //    //TODO
    //}
}
