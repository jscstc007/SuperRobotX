using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    PlayerHPChange,
    PlayerScoreChange,
}

public class EventManager : ISingleton<EventManager> {

    public delegate void EventDelegate(params int[] data);

    private Dictionary<EventType, EventDelegate> EventHandlerMap = new Dictionary<EventType, EventDelegate>();

    /// <summary>
    /// 注册一个事件
    /// </summary>
    public void RegistEvent (EventType type, EventDelegate handler)
    {
        if (EventHandlerMap.ContainsKey(type))
        {
            EventHandlerMap[type] += handler;
        }
        else
        {
            EventHandlerMap.Add(type, handler);
        }
    }
    /// <summary>
    /// 移除一个事件
    /// </summary>
    public void RemoveEvent (EventType type, EventDelegate handler)
    {
        if (EventHandlerMap.ContainsKey(type))
        {
            EventHandlerMap[type] -= handler;
        }
    }
    /// <summary>
    /// 激活一个事件
    /// </summary>
    public void ExcuteEvent (EventType type, params int[] data)
    {
        if (EventHandlerMap.ContainsKey(type))
        {
            EventHandlerMap[type].Invoke(data);
        }
    }
}
