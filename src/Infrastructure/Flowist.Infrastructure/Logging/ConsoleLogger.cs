using Flowlist.Core.Interfaces;

namespace Flowist.Infrastructure.Logging;

public class ConsoleLogger : IAppLogger
{
    public void LogError(string message, params object[] args)
    {
        if (args == null || args.Length == 0)
        {
            Console.WriteLine($"[ERROR] {message}");
        }  
        else
        {
            Console.WriteLine($"[ERROR] {string.Format(message, args)}");
        }

        Console.WriteLine($"[ERROR] {string.Format(message, args)}");
    }

    public void LogInfo(string message, params object[] args)
    {
        if (args == null || args.Length == 0)
        {
            Console.WriteLine($"[INFO] {message}");
        }
        else
        {
            Console.WriteLine($"[INFO] {string.Format(message, args)}");
        }
    }
}
