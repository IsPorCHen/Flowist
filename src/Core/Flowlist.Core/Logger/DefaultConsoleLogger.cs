namespace Flowlist.Core.Logger;

public class DefaultConsoleLogger : IConsoleLogger
{
    /// <summary>
    /// Logs an informational message to the console with a timestamp and [INFO] tag.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public void LogInfo(string message, params object[] args) =>
        Console.WriteLine("[INFO] {0}", string.Format(message, args));

    /// <summary>
    /// Logs a warning message to the console with a timestamp and [WARN] tag.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public void LogWarn(string message, params object[] args) =>
        Console.WriteLine("[WARN] {0}", string.Format(message, args));

    /// <summary>
    /// Logs an error message to the console with a timestamp and [ERROR] tag, including exception details and stack trace.
    /// </summary>
    /// <param name="ex"></param>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public void LogError(Exception ex, string message, params object[] args)
    {
        Console.WriteLine("[ERROR] {0} - Exception: {1}", string.Format(message, args), ex.Message);
        Console.WriteLine(ex.StackTrace);
    }

    /// <summary>
    /// Logs a trace message to the console with a timestamp and [TRACE] tag, useful for debugging and detailed execution flow information.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="args"></param>
    public void LogTrace(string message, params object[] args) =>
        Console.WriteLine("[TRACE] {0}", string.Format(message, args));
}
