using Flowist.Core.Interfaces;

namespace Flowist.Infrastructure.Logging;

public class ConsoleLogger : IAppLogger
{
    public void LogError(string message, params object[] args)
    {
        Console.WriteLine($"[ERROR] {message}", args);
    }

    public void LogInfo(string message, params object[] args)
    {
        Console.WriteLine($"[INFO] {message}", args);
    }
}
