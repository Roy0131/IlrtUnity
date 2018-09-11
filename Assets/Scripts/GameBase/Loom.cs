using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class Loom : RComponent
{
    public struct DelayedQueueItem
    {
        public float time;
        public Action action;
    }

    public static Loom Instance { get; private set; }
    protected override void OnAwake()
    {
        base.OnAwake();
        Instance = this;
    }

    private List<Action> _actions = new List<Action>();
    private List<Action> _currentActions = new List<Action>();

    private List<DelayedQueueItem> _delayed = new List<DelayedQueueItem>();
    private List<DelayedQueueItem> _currentDelayed = new List<DelayedQueueItem>();

    public static void QueueOnMainThread(Action action)
    {
        QueueOnMainThread(action, 0f);
    }

    public static void QueueOnMainThread(Action action, float time)
    {
        if (time != 0)
        {
            lock (Instance._delayed)
            {
                Instance._delayed.Add(new DelayedQueueItem { time = Time.time + time, action = action });
            }
        }
        else
        {
            lock (Instance._actions)
            {
                Instance._actions.Add(action);
            }
        }
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        lock (_actions)
        {
            _currentActions.Clear();
            _currentActions.AddRange(_actions);
            _actions.Clear();
        }
        foreach (var a in _currentActions)
            a();
        lock (_delayed)
        {
            _currentDelayed.Clear();
            _currentDelayed.AddRange(_delayed.Where(d => d.time <= Time.time));
            foreach (var item in _currentDelayed)
                _delayed.Remove(item);
        }
        foreach (var delayed in _currentDelayed)
            delayed.action();
    }
}