using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : ISingleton<DataManager>
{

    private const string KEY_COIN = "COIN";
    private const string KEY_ROBOT_LEVEL = "ROBOT_LEVEL";
    private const string KEY_EQUIP_WEAPON = "ROBOT_WEAPON";

    /// <summary>
    /// 加载一个Int数据
    /// </summary>
	private int LoadIntData(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }
    /// <summary>
    /// 储存一个Int数据
    /// </summary>
    private void SaveIntData(string key, int defaultValue = 0)
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
    /// 清空全部数据
    /// </summary>
    public void ClearAllData ()
    {
        //金币
        SaveCoin(0);
        //武器等级
        WeaponType[] weaponTypes = WeaponInfo.GetAllWeapons();
        foreach(WeaponType weaponType in weaponTypes)
        {
            SaveWeaponLevel(weaponType, 0);
        }
        //已穿戴
        SaveEquipWeapon(null);
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
        SaveIntData(KEY_COIN, coin);
    }

    /// <summary>
    /// 从数据库中读取一个robot的等级
    /// </summary>
    public int LoadRobotLevel(RobotType type)
    {
        return LoadIntData(type.ToString());
    }
    /// <summary>
    /// 从数据库中设置一个robot的等级
    /// </summary>
    public void SaveRobotLevel(RobotType type, int level)
    {
        SaveIntData(type.ToString(), level);
    }

    /// <summary>
    /// 从数据库中读取一个武器的等级
    /// </summary>
    public int LoadWeaponLevel(WeaponType type)
    {
        return LoadIntData(type.ToString());
    }
    /// <summary>
    /// 从数据库中设置一个武器的等级
    /// </summary>
    public void SaveWeaponLevel(WeaponType type, int level)
    {
        SaveIntData(type.ToString(), level);
    }

    /// <summary>
    /// 从数据库中加载已穿戴的武器
    /// </summary>
    public List<WeaponType> LoadEquipWeapon()
    {
        List<WeaponType> list = new List<WeaponType>();
        string str = LoadStringData(KEY_EQUIP_WEAPON);

        //Debug.Log("LoadEquipWeapon:" + str);

        string[] strs = str.Split(',');

        if (null != strs && strs.Length == 1 && string.IsNullOrEmpty(strs[0]))
        {
            //空 不处理
        }
        else
        {
            foreach (string s in strs)
            {
                WeaponType type = (WeaponType)(int.Parse(s));
                list.Add(type);
            }
        }

        return list;
    }
    /// <summary>
    /// 从数据库中储存已穿戴的武器
    /// </summary>
    public void SaveEquipWeapon(List<WeaponType> types)
    {
        string value = string.Empty;
        if (null == types || types.Count == 0)
        {
            //清空武器
            SaveStringData(KEY_EQUIP_WEAPON, value);
        }
        else
        {
            value = string.Format("{0}", (int)types[0]);
            if (types.Count > 1)
            {
                for (int i = 1; i < types.Count; i++)
                {
                    value += string.Format(",{0}", (int)types[i]);
                }
            }

            //Debug.Log("SaveEquipWeapon:" + value);

            SaveStringData(KEY_EQUIP_WEAPON, value);
        }
    }

}
