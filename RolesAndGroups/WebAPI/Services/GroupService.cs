using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Helpers;
using Interfaces.Dal;
using Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.Dto;
using Models.Responses;
using Models.ViewModels;
namespace Services;

/// <summary>
/// Служба для работы с группами
/// </summary>
public class GroupService : IGroupService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfigurationProvider _configurationProvider;

    public GroupService(IUnitOfWork unitOfWork, IConfigurationProvider configuration)
    {
        _unitOfWork = unitOfWork;
        _configurationProvider = configuration;
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<string>> CreateGroup(GroupDto groupDto)
    {
        var group = await _unitOfWork.GroupRepository.Where(x => x.Name == groupDto.Name).FirstOrDefaultAsync();

        if(group is not null)
            throw new Exception("Данная группа уже существует");

        group = new Group();

        EntityMapHelper.MapSourceToEntity(groupDto, ref group);

        await _unitOfWork.GroupRepository.AddAsync(group);
        await _unitOfWork.SaveChangesAsync();

        return new ServiceResponse<string>("Группа успешно создана");
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<string>> EditGroup(GroupDto groupDto)
    {
        var group = await _unitOfWork.GroupRepository
                .Where(x => x.Id == groupDto.Id)
                .Include(x => x.Roles)
                .FirstOrDefaultAsync()
         ?? throw new Exception("Данная группа не существует");

        EditGroupRoles(groupDto, group);
        EntityMapHelper.MapSourceToEntity(groupDto, ref group);

        _unitOfWork.GroupRepository.Update(group);
        await _unitOfWork.SaveChangesAsync();

        return new ServiceResponse<string>("Группа успешно отредактирована");
    }

    /// <summary>
    /// Обновить роли группы
    /// </summary>
    private void EditGroupRoles(GroupDto data, Group item)
    {
        foreach (var i in item.Roles)
        {
            var newRole = data.Roles.FirstOrDefault(y => y.Id == i.Id);

            if(newRole is null)
            {
                item.Roles.Remove(i);

                continue;
            }
            var oldRole = item.Roles.FirstOrDefault(y => y.Id == i.Id);
            EntityMapHelper.MapSourceToEntity(newRole, ref oldRole);
        }

        foreach (var i in data.Roles.Where(x => !item.Roles.Select(y => y.Id).Contains(x.Id)))
        {
            var newRole = new Role();
            EntityMapHelper.MapSourceToEntity(i, ref newRole);
            item.Roles.Add(newRole);
        }
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<string>> DeleteGroup(int groupId)
    {
        var group = await _unitOfWork.GroupRepository
                .Where(x => x.Id == groupId)
                .FirstOrDefaultAsync()
         ?? throw new Exception("Данная роль не существует");

        _unitOfWork.GroupRepository.Delete(group);
        await _unitOfWork.SaveChangesAsync();

        return new ServiceResponse<string>("Группа успешно удалена");
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<GroupDto[]>> GetAllGroups()
    {
        var groups = await _unitOfWork.GroupRepository
            .Where(x => !x.DeletedDateTime.HasValue)
            .ProjectTo<GroupDto>(_configurationProvider)
            .ToArrayAsync();

        return new ServiceResponse<GroupDto[]>(groups);
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<PageViewData<GroupDto>>> GetPagedGroups(int page)
    {
        var pageSize = 5; // количество объектов на страницу

        var groups = await _unitOfWork.GroupRepository
            .Where(x => !x.DeletedDateTime.HasValue)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<GroupDto>(_configurationProvider)
            .ToArrayAsync();

        var pageData = new PageViewData<GroupDto>
        {
            PageNumber = page,
            PageSize = pageSize,
            TotalItems = _unitOfWork.GroupRepository.TotalCount(),
            Items = groups
        };

        return new ServiceResponse<PageViewData<GroupDto>>(pageData);
    }
}