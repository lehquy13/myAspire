using LibraryManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Infrastructure.AppLogger;

public class AppLogger<TCategory> : IAppLogger<TCategory>
{
    private readonly ILogger<TCategory> _logger;

    public AppLogger(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<TCategory>();
    }

    public void LogInformation(string? message, params object[] args)
    {
        _logger.LogInformation(message, args);
    }

    public void LogWarning(string? message, params object[] args)
    {
        _logger.LogWarning(message, args);
    }

    public void LogError(string? message, params object[] args)
    {
        _logger.LogError(message, args);
    }
}