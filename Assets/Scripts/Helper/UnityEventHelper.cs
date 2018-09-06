using System;
using UnityEngine.UI;

public static class UnityEventHelper
{
    public static void Add(this Button.ButtonClickedEvent buttonClickedEvent, Action action)
    {
        buttonClickedEvent.AddListener(() => { action(); });
    }
}
