public class Logger 
{
    public static void Log(string value)
    {
        Debuger.Log(value);
    }

    public static void LogError(string value)
    {
        Debuger.LogError(value);
    }

    public static void LogWarning(string value)
    {
        Debuger.LogWarning(value);
    }
}