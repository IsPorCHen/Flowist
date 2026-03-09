namespace Flowist.Core.Interfaces;

public interface IAppLogger
{
    void LogError(string message, params object[] args);
    void LogInfo(string message, params object[] args);
}