using Models.Dto;
using Models.Responses;
using Models.ViewModels;
namespace Interfaces.Services;

/// <summary>
/// Служба для работы с ролями
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// Создание роли
    /// </summary>
    Task<ServiceResponse<string>> CreateRole(RoleDto roleDto);

    /// <summary>
    /// Редактирование роли
    /// </summary>
    Task<ServiceResponse<string>> EditRole(RoleDto roleDto);

    /// <summary>
    /// Удаление роли
    /// </summary>
    Task<ServiceResponse<string>> DeleteRole(int roleId);

    /// <summary>
    /// Получить все роли
    /// </summary>
    Task<ServiceResponse<RoleDto[]>> GetAllRoles();

    /// <summary>
    /// Получить роли постранично
    /// </summary>
    Task<ServiceResponse<PageViewData<RoleDto>>> GetPagedRoles(int page);
}