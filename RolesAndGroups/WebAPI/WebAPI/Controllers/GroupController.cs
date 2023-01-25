using Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Dto;
using Models.Responses;
using Models.ViewModels;
namespace WebAPI.Controllers;

/// <summary>
/// Контроллер группы
/// </summary>
[ApiController]
[Route("/[controller]/[action]")]
public class GroupController : ControllerBase
{
    /// <summary>
    /// Служба ролей
    /// </summary>
    private readonly IGroupService _groupService;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="groupService">Служба ролей</param>
    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    /// <summary>
    /// Создание группы
    /// </summary>
    /// <param name="model">Данные новой группы</param>
    /// <returns>Модель успешного ответа</returns>
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(SuccessResponse))]
    [ProducesResponseType(400, Type = typeof(ErrorResponse))]
    [ProducesResponseType(500, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> CreateGroup([FromBody] GroupDto model)
    {
        var response = await _groupService.CreateGroup(model);

        return Ok(new SuccessResponse(response.Result));
    }

    /// <summary>
    /// Редактирование группы
    /// </summary>
    /// <param name="model">Данные группы</param>
    /// <returns>Модель успешного ответа</returns>
    [HttpPut]
    [ProducesResponseType(200, Type = typeof(SuccessResponse))]
    [ProducesResponseType(400, Type = typeof(ErrorResponse))]
    [ProducesResponseType(500, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> EditGroup([FromBody] GroupDto model)
    {
        var response = await _groupService.EditGroup(model);

        return Ok(new SuccessResponse(response.Result));
    }

    /// <summary>
    /// Удаление группы
    /// </summary>
    /// <param name="groupId">Идентификатор группы</param>
    /// <returns>Модель успешного ответа</returns>
    [HttpDelete]
    [ProducesResponseType(200, Type = typeof(SuccessResponse))]
    [ProducesResponseType(400, Type = typeof(ErrorResponse))]
    [ProducesResponseType(500, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> DeleteGroup([FromQuery] int groupId)
    {
        var response = await _groupService.DeleteGroup(groupId);

        return Ok(new SuccessResponse(response.Result));
    }

    /// <summary>
    /// Получение всех групп
    /// </summary>
    /// <returns>Массив групп</returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(GroupDto[]))]
    [ProducesResponseType(400, Type = typeof(ErrorResponse))]
    [ProducesResponseType(500, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> GetAllGroups()
    {
        var response = await _groupService.GetAllGroups();

        return Ok(response.Result);
    }

    /// <summary>
    /// Получение групп постранично
    /// </summary>
    /// <returns>Страница групп</returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(PageViewData<GroupDto>))]
    [ProducesResponseType(400, Type = typeof(ErrorResponse))]
    [ProducesResponseType(500, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> GetPagedGroups(int page = 1)
    {
        var response = await _groupService.GetPagedGroups(page);

        return Ok(response.Result);
    }
}