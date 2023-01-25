using Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Models.Dto;
using Models.Responses;
using Models.ViewModels;
namespace WebAPI.Controllers;

/// <summary>
/// Контроллер Роли
/// </summary>
[ApiController]
[Route("/[controller]/[action]")]
public class RoleController : ControllerBase
{
    /// <summary>
    /// Служба ролей
    /// </summary>
    private readonly IRoleService _roleService;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="roleService">Служба ролей</param>
    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    /// <summary>
    /// Создание роли
    /// </summary>
    /// <param name="model">Данные новой роли</param>
    /// <returns>Модель успешного ответа</returns>
    [HttpPost]
    [ProducesResponseType(200, Type = typeof(SuccessResponse))]
    [ProducesResponseType(400, Type = typeof(ErrorResponse))]
    [ProducesResponseType(500, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> CreateRole([FromBody] RoleDto model)
    {
        var response = await _roleService.CreateRole(model);

        return Ok(new SuccessResponse(response.Result));
    }

    /// <summary>
    /// Редактирование роли
    /// </summary>
    /// <param name="model">Данные роли</param>
    /// <returns>Модель успешного ответа</returns>
    [HttpPut]
    [ProducesResponseType(200, Type = typeof(SuccessResponse))]
    [ProducesResponseType(400, Type = typeof(ErrorResponse))]
    [ProducesResponseType(500, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> EditRole([FromBody] RoleDto model)
    {
        var response = await _roleService.EditRole(model);

        return Ok(new SuccessResponse(response.Result));
    }

    /// <summary>
    /// Удаление роли
    /// </summary>
    /// <param name="roleId">Идентификатор роли</param>
    /// <returns>Модель успешного ответа</returns>
    [HttpDelete]
    [ProducesResponseType(200, Type = typeof(SuccessResponse))]
    [ProducesResponseType(400, Type = typeof(ErrorResponse))]
    [ProducesResponseType(500, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> DeleteRole([FromQuery] int roleId)
    {
        var response = await _roleService.DeleteRole(roleId);

        return Ok(new SuccessResponse(response.Result));
    }

    /// <summary>
    /// Получение всех ролей
    /// </summary>
    /// <returns>Массив ролей</returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(RoleDto[]))]
    [ProducesResponseType(400, Type = typeof(ErrorResponse))]
    [ProducesResponseType(500, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> GetAllRoles()
    {
        var response = await _roleService.GetAllRoles();

        return Ok(response.Result);
    }

    /// <summary>
    /// Получение ролей постранично
    /// </summary>
    /// <returns>Страница ролей</returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(PageViewData<RoleDto>))]
    [ProducesResponseType(400, Type = typeof(ErrorResponse))]
    [ProducesResponseType(500, Type = typeof(ErrorResponse))]
    public async Task<IActionResult> GetPagedRoles(int page = 1)
    {
        var response = await _roleService.GetPagedRoles(page);

        return Ok(response.Result);
    }
}