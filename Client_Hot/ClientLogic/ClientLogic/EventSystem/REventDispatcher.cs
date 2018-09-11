#region << 版 本 注 释 >>
/*
	 * ========================================================================
     * Copyright Notice  2016-2016 moyutec.com All rights reserved .
     * ========================================================================
     * 文件名	： REventDispatcher 
     * 创建人	： roy
     * 创建时间	： 2016/12/19 16:40:17 
     * 描述  	： 事件载体：派发，接收
*/
#endregion

using System;
using System.Collections.Generic;

public class REventDispatcher
{
    private Dictionary<string, Delegate> _dictAllEvents = new Dictionary<string, Delegate>();
    //public string m_name { get; private set; }
    public REventDispatcher()
    {
    }

    public void RemoveAllEvent()
    {
        if (_dictAllEvents != null)
            _dictAllEvents.Clear();
    }

    #region register event;
    public void AddEvent(string eventType, REventDelegate method)
    {
        Delegate delegateEvent = null;
        if (_dictAllEvents.ContainsKey(eventType))
        {
            delegateEvent = _dictAllEvents[eventType];
        }
        _dictAllEvents[eventType] = (REventDelegate)Delegate.Combine((REventDelegate)delegateEvent, method);
    }

    public void AddEvent<T>(string eventType, REventDelegate<T> method)
    {
        Delegate delegateEvent = null;
        if (_dictAllEvents.ContainsKey(eventType))
        {
            delegateEvent = _dictAllEvents[eventType];
        }
        _dictAllEvents[eventType] = (REventDelegate<T>)Delegate.Combine((REventDelegate<T>)delegateEvent, method);
    }

    public void AddEvent<T, U>(string eventType, REventDelegate<T, U> method)
    {
        Delegate delegateEvent = null;
        if (_dictAllEvents.ContainsKey(eventType))
        {
            delegateEvent = _dictAllEvents[eventType];
        }
        _dictAllEvents[eventType] = (REventDelegate<T, U>)Delegate.Combine((REventDelegate<T, U>)delegateEvent, method);
    }

    public void AddEvent<T, U, K>(string eventType, REventDelegate<T, U, K> method)
    {
        Delegate delegateEvent = null;
        if (_dictAllEvents.ContainsKey(eventType))
        {
            delegateEvent = _dictAllEvents[eventType];
        }
        _dictAllEvents[eventType] = (REventDelegate<T, U, K>)Delegate.Combine((REventDelegate<T, U, K>)delegateEvent, method);
    }

    #endregion

    #region unregister event;
    public void RemoveEvent(string eventType, REventDelegate method)
    {
        if (!HasEvent(eventType))
            return;
        _dictAllEvents[eventType] = (REventDelegate)Delegate.Remove((REventDelegate)_dictAllEvents[eventType], method);
    }

    public void RemoveEvent<T>(string eventType, REventDelegate<T> method)
    {
        if (!HasEvent(eventType))
            return;
        _dictAllEvents[eventType] = (REventDelegate<T>)Delegate.Remove((REventDelegate<T>)_dictAllEvents[eventType], method);
    }

    public void RemoveEvent<T, U>(string eventType, REventDelegate<T, U> method)
    {
        if (!HasEvent(eventType))
            return;
        _dictAllEvents[eventType] = (REventDelegate<T, U>)Delegate.Remove((REventDelegate<T, U>)_dictAllEvents[eventType], method);
    }

    public void RemoveEvent<T, U, K>(string eventType, REventDelegate<T, U, K> method)
    {
        if (!HasEvent(eventType))
            return;
        _dictAllEvents[eventType] = (REventDelegate<T, U, K>)Delegate.Remove((REventDelegate<T, U, K>)_dictAllEvents[eventType], method);
    }
    #endregion;

    public bool HasEvent(string eventType)
    {
        if (_dictAllEvents.ContainsKey(eventType) && _dictAllEvents[eventType] == null)
        {
            return false;
        }
        return _dictAllEvents.ContainsKey(eventType);
    }

    #region trigger event;
    public void DispathEvent(string eventType)
    {
        if (!HasEvent(eventType))
            return;
        Delegate[] allDelegate = _dictAllEvents[eventType].GetInvocationList();
        REventDelegate method;
        for (int i = 0; i < allDelegate.Length; i++)
        {
            if (allDelegate[i].GetType() != typeof(REventDelegate))
                continue;

            method = (REventDelegate)allDelegate[i];
            if (method != null)
                method.Invoke();
        }
    }

    public void DispathEvent<T>(string eventType, T p1)
    {
        if (!HasEvent(eventType))
            return;
        Delegate[] allDelegate = _dictAllEvents[eventType].GetInvocationList();
        REventDelegate<T> method;
        for (int i = 0; i < allDelegate.Length; i++)
        {
            if (allDelegate[i].GetType() != typeof(REventDelegate<T>))
                continue;

            method = (REventDelegate<T>)allDelegate[i];
            if (method != null)
                method.Invoke(p1);
        }
    }

    public void DispathEvent<T, U>(string eventType, T p1, U p2)
    {
        if (!HasEvent(eventType))
            return;
        Delegate[] allDelegate = _dictAllEvents[eventType].GetInvocationList();
        REventDelegate<T, U> method;
        for (int i = 0; i < allDelegate.Length; i++)
        {
            if (allDelegate[i].GetType() != typeof(REventDelegate<T, U>))
                continue;

            method = (REventDelegate<T, U>)allDelegate[i];
            if (method != null)
                method.Invoke(p1, p2);
        }
    }

    public void DispathEvent<T, U, K>(string eventType, T p1, U p2, K p3)
    {
        if (!HasEvent(eventType))
            return;
        Delegate[] allDelegate = _dictAllEvents[eventType].GetInvocationList();
        REventDelegate<T, U, K> method;
        for (int i = 0; i < allDelegate.Length; i++)
        {
            if (allDelegate[i].GetType() != typeof(REventDelegate<T, U, K>))
                continue;

            method = (REventDelegate<T, U, K>)allDelegate[i];
            if (method != null)
                method.Invoke(p1, p2, p3);
        }
    }
    #endregion
}
