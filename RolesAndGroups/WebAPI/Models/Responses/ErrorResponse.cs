namespace Models.Responses;

/// <summary>
/// Модель для ошибок
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Сообщение об ошибке
    /// </summary>
    public string ErrorMessage { get; }

    public ErrorResponse(string errorMessage) => ErrorMessage = errorMessage;
}