namespace Models.Responses;

/// <summary>
/// Модель успешного ответа
/// </summary>
public class SuccessResponse
{
    /// <summary>
    /// Сообщение корректного ответа
    /// </summary>
    public string ResponseMessage { get; }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="responseMessage">Сообщение корректного ответа</param>
    public SuccessResponse(string responseMessage) => ResponseMessage = responseMessage;
}