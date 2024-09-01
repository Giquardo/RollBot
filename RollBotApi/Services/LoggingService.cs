using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace RollBotApi.Services;
public interface ILoggingService
{
    void LogInformation(string message);
    void LogWarning(string message);
    void LogError(string message);
    void LogError(Exception exception, string message);
    void LogDebug(string message);
}

public class LoggingService : ILoggingService
{
    private readonly string _filePath;
    private readonly object _lock = new object();

    public LoggingService(string filePath)
    {
        _filePath = filePath;
        EnsureLogFileExists();
    }

    private void EnsureLogFileExists()
    {
        var directory = Path.GetDirectoryName(_filePath);
        if (directory != null && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        if (!File.Exists(_filePath))
        {
            File.Create(_filePath).Dispose();
        }
    }

    public void LogInformation(string message)
    {
        Log("Information", message);
    }

    public void LogWarning(string message)
    {
        Log("Warning", message);
    }

    public void LogError(string message)
    {
        Log("Error", message);
    }

    public void LogError(Exception exception, string message)
    {
        Log("Error", $"{message}: {exception}");
    }

    public void LogDebug(string message)
    {
        Log("Debug", message);
    }

    private void Log(string logLevel, string message)
    {
        var logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [{logLevel}] - {message}";
        try
        {
            lock (_lock)
            {
                File.AppendAllText(_filePath, logMessage + Environment.NewLine);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to write to log file: {ex}");
        }
    }
}
