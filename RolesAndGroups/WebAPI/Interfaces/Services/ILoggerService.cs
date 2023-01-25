namespace Interfaces.Services;

/// <summary>
/// Служба логирования
/// </summary>
public interface ILoggerService
{
    /// <summary>
    /// Логирование общей информации
    /// </summary>
    void Info(string logMessage);

    /// <summary>
    /// Логирование общих ошибок
    /// </summary>
    void Error(string logMessage);

    /// <summary>
    /// Логирование только информаци по запросам и ответам контроллеров
    /// </summary>
    void RequestResponseInfo(string logMessage);

    /// <summary>
    /// Логирвоание запросов в Базу Данных
    /// </summary>
    void DbRequestsInfo(string logMessage);
}