namespace Flowlist.Core.Logger;

public class DefaultConsoleLogger : IConsoleLogger
{
    public void LogInfo(string message, params object[] args) =>
        Console.WriteLine("[INFO] {0}", string.Format(message, args));

    public void LogWarn(string message, params object[] args) =>
        Console.WriteLine("[WARN] {0}", string.Format(message, args));

    public void LogError(Exception ex, string message, params object[] args)
    {
        Console.WriteLine("[ERROR] {0} - Exception: {1}", string.Format(message, args), ex.Message);
        Console.WriteLine(ex.StackTrace);
    }

    public void LogTrace(string message, params object[] args) =>
        Console.WriteLine("[TRACE] {0}", string.Format(message, args));
}