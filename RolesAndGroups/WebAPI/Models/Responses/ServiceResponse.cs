namespace Models.Responses;

/// <summary>
/// Ответ службы
/// </summary>
public class ServiceResponse<T>
{
    /// <summary>
    /// Результат
    /// </summary>
    public T Result { get; }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="result">Результат</param>
    public ServiceResponse(T result) => Result = result;
}