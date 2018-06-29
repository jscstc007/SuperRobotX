using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : ISingleton<DataManager> {

    public const string KEY_COIN = "COIN";
    public const string KEY_ROBOT_LEVEL = "ROBOT_LEVEL";

    /// <summary>
    /// 加载一个Int数据
    /// </summary>
	private int LoadIntData (string key,int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }
    /// <summary>
    /// 储存一个Int数据
    /// </summary>
    private void SaveIntData (string key, int defaultValue = 0)
    {
        PlayerPrefs.SetInt(key, defaultValue);
    }

    /// <summary>
    /// 加载一个String数据
    /// </summary>
    private string LoadStringData(string key, string defaultValue = "")
    {
        return PlayerPrefs.GetString(key, defaultValue);
    }
    /// <summary>
    /// 储存一个String数据
    /// </summary>
    private void SaveStringData(string key, string defaultValue = "")
    {
        PlayerPrefs.SetString(key, defaultValue);
    }

    /// <summary>
    /// 从数据库中读取数据
    /// </summary>
    public int LoadCoin()
    {
        return LoadIntData(KEY_COIN);
    }
    /// <summary>
    /// 从数据库中设置数据
    /// </summary>
    public void SaveCoin(int coin)
    {
        SaveIntData(KEY_COIN,coin);
    }

    /// <summary>
    /// 从数据库中读取一个武器的等级
    /// </summary>
    public int LoadWeaponLevel (WeaponType type)
    {
        return LoadIntData( type.ToString() );
    }
    /// <summary>
    /// 从数据库中设置一个武器的等级
    /// </summary>
    public void SaveWeaponLevel (WeaponType type,int level)
    {
        SaveIntData(type.ToString() , level);
    }
}
