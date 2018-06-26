using System.Collections;
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

    
}
