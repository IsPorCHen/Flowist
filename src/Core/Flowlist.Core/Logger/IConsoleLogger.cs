namespace Flowlist.Core.Logger;

public interface IConsoleLogger
{
    void LogInfo(string message, params object[] args);
    void LogWarn(string message, params object[] args);
    void LogError(Exception ex, string message, params object[] args);
    void LogTrace(string message, params object[] args);
}
