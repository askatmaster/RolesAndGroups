using Interfaces.Services;
using NLog;
namespace Services;

/// <inheritdoc />
public class LoggerService : ILoggerService
{
    private readonly Logger _logger;
    private readonly Logger _requestResponseLogger;
    private readonly Logger _dbRequestsLogger;

    public LoggerService()
    {
        _logger = LogManager.GetCurrentClassLogger();
        _requestResponseLogger = LogManager.GetLogger("RequestLogger") ?? throw new Exception("Не удалось получить логер 'RequestLogger'");
        _dbRequestsLogger = LogManager.GetLogger("DbRequestsLogger") ?? throw new Exception("Не удалось получить логер 'DbRequestsLogger'");
    }

    public void Info(string logMessage)
    {
        _logger.Info(logMessage);
    }

    public void Error(string logMessage)
    {
        _logger.Error(logMessage);
    }

    public void RequestResponseInfo(string logMessage)
    {
        _requestResponseLogger.Info(logMessage);
    }

    public void DbRequestsInfo(string logMessage)
    {
        _dbRequestsLogger.Info(logMessage);
    }
}