using System.Net;
using System.Net.Mime;
using System.Security.Authentication;
using System.Text;
using Interfaces.Services;
using Models.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
namespace WebAPI.Middlewares;

/// <summary>
/// Middleware глобальной обработки исключений.
/// </summary>
public class ErrorHandlingMiddleware
{
    /// <summary>
    /// Функция, которая может обрабатывать HTTP-запрос.
    /// </summary>
    private readonly RequestDelegate _next;

    /// <summary>
    /// логгер
    /// </summary>
    private readonly ILoggerService _logger;

    /// <summary>
    /// Лог запроса
    /// </summary>
    private string _requestLog;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="next">Функция, которая может обрабатывать HTTP-запрос.</param>
    /// <param name="logger">Служба логирование</param>
    public ErrorHandlingMiddleware(RequestDelegate next, ILoggerService logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Выполнить текущие запросы
    /// </summary>
    public async Task Invoke(HttpContext context)
    {
        var guid = Guid.NewGuid();

        try
        {
            //логгирование тела запроса
            await RequestBodyRead(context, guid);

            // Вызов следующего промежуточного ПО
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex, guid);
        }
    }

    /// <summary>
    /// Считать тело ответа
    /// </summary>
    private async Task RequestBodyRead(HttpContext context, Guid guid)
    {
        _requestLog = $"REQUEST ({guid}) HttpMethod: {context.Request.Method}, Path: {context.Request.Path}";

        var injectedRequestStream = new MemoryStream();

        using var bodyReader = new StreamReader(context.Request.Body);

        var bodyAsText = await bodyReader.ReadToEndAsync();

        if (!string.IsNullOrWhiteSpace(bodyAsText))
            _requestLog += $", Body : {bodyAsText}";

        var bytesToWrite = Encoding.UTF8.GetBytes(bodyAsText);
        injectedRequestStream.Write(bytesToWrite, 0, bytesToWrite.Length);
        injectedRequestStream.Seek(0, SeekOrigin.Begin);
        context.Request.Body = injectedRequestStream;
        _logger.RequestResponseInfo(_requestLog);
    }

    /// <summary>
    /// Обработать исключение
    /// </summary>
    private Task HandleExceptionAsync( HttpContext context, Exception exception, Guid guid)
    {
        var code = GetHttpStatusCode(exception);

        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)code;

        var exMessage = GetExceptionMessage(exception, guid);

        var responseMessage = JsonConvert.SerializeObject(new ErrorResponse(exMessage),
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

        return context.Response.WriteAsync(responseMessage);
    }

    /// <summary>
    /// Преобразование ошибок
    /// </summary>
    private HttpStatusCode GetHttpStatusCode(Exception exception)
    {
        var code = HttpStatusCode.InternalServerError;

        switch (exception)
        {
            case ArgumentNullException:
            case InvalidOperationException:
            {
                code = HttpStatusCode.BadRequest;

                break;
            }
            case UnauthorizedAccessException:
            case AuthenticationException:
            {
                code = HttpStatusCode.Unauthorized;

                break;
            }
        }

        return code;
    }

    /// <summary>
    /// Получение сообщения об ошибке
    /// </summary>
    private string GetExceptionMessage(Exception exception, Guid guid)
    {
        _logger.Error($"{exception.Message} ({guid}) \n {JsonConvert.SerializeObject(exception)} \n {_requestLog}");

        //добавляем к сообщению уникашьный идентификатор
        return $"{exception.Message} ({guid})";
    }
}