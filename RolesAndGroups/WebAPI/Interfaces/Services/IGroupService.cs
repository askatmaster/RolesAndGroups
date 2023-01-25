using Models.Dto;
using Models.Responses;
using Models.ViewModels;
namespace Interfaces.Services;

/// <summary>
/// Служба для работы с группами
/// </summary>
public interface IGroupService
{
    /// <summary>
    /// Создание группы
    /// </summary>
    Task<ServiceResponse<string>> CreateGroup(GroupDto groupDto);

    /// <summary>
    /// Редактирование группы
    /// </summary>
    Task<ServiceResponse<string>> EditGroup(GroupDto groupDto);

    /// <summary>
    /// Удаление группы
    /// </summary>
    Task<ServiceResponse<string>> DeleteGroup(int groupId);

    /// <summary>
    /// Получить все группы
    /// </summary>
    Task<ServiceResponse<GroupDto[]>> GetAllGroups();

    /// <summary>
    /// Получить группы постранично
    /// </summary>
    Task<ServiceResponse<PageViewData<GroupDto>>> GetPagedGroups(int page);
}