using System;

public class LoomHelper
{
    public static void QueueOnMainThread(Action action)
    {
        Loom.QueueOnMainThread(action);
    }
}
