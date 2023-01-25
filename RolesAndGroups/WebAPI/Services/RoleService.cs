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
/// Служба для работы с ролями
/// </summary>
public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfigurationProvider _configurationProvider;

    public RoleService(IUnitOfWork unitOfWork, IConfigurationProvider configuration)
    {
        _unitOfWork = unitOfWork;
        _configurationProvider = configuration;
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<string>> CreateRole(RoleDto roleDto)
    {
        var role = await _unitOfWork.RoleRepository.Where(x => x.Name == roleDto.Name).FirstOrDefaultAsync();

        if(role is not null)
            throw new Exception("Данная роль уже существует");

        role = new Role();

        EntityMapHelper.MapSourceToEntity(roleDto, ref role);

        await _unitOfWork.RoleRepository.AddAsync(role);
        await _unitOfWork.SaveChangesAsync();

        return new ServiceResponse<string>("Роль успешно создана");
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<string>> EditRole(RoleDto roleDto)
    {
        var role = await _unitOfWork.RoleRepository
                .Where(x => x.Id == roleDto.Id)
                .Include(x => x.Groups)
                .FirstOrDefaultAsync()
         ?? throw new Exception("Данная роль не существует");

        EditRoleGroups(roleDto, role);
        EntityMapHelper.MapSourceToEntity(roleDto, ref role);

        _unitOfWork.RoleRepository.Update(role);
        await _unitOfWork.SaveChangesAsync();

        return new ServiceResponse<string>("Роль успешно отредактирована");
    }

    /// <summary>
    /// Обновить роли группы
    /// </summary>
    private void EditRoleGroups(RoleDto data, Role item)
    {
        foreach (var i in item.Groups)
        {
            var newGroup = data.Groups.FirstOrDefault(y => y.Id == i.Id);

            if(newGroup is null)
            {
                item.Groups.Remove(i);

                continue;
            }
            var oldCurr = item.Groups.FirstOrDefault(y => y.Id == i.Id);
            EntityMapHelper.MapSourceToEntity(newGroup, ref oldCurr);
        }

        foreach (var i in data.Groups.Where(x => !item.Groups.Select(y => y.Id).Contains(x.Id)))
        {
            var newGroup = new Group();
            EntityMapHelper.MapSourceToEntity(i, ref newGroup);
            item.Groups.Add(newGroup);
        }
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<string>> DeleteRole(int roleId)
    {
        var role = await _unitOfWork.RoleRepository
                .Where(x => x.Id == roleId)
                .FirstOrDefaultAsync()
         ?? throw new Exception("Данная роль не существует");

        _unitOfWork.RoleRepository.Delete(role);
        await _unitOfWork.SaveChangesAsync();

        return new ServiceResponse<string>("Роль успешно удалена");
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<RoleDto[]>> GetAllRoles()
    {
        var roles = await _unitOfWork.RoleRepository
            .Where(x => !x.DeletedDateTime.HasValue)
            .ProjectTo<RoleDto>(_configurationProvider)
            .ToArrayAsync();

        return new ServiceResponse<RoleDto[]>(roles);
    }

    /// <inheritdoc />
    public async Task<ServiceResponse<PageViewData<RoleDto>>> GetPagedRoles(int page)
    {
        var pageSize = 5; // количество объектов на страницу

        var roles = await _unitOfWork.RoleRepository
            .Where(x => !x.DeletedDateTime.HasValue)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<RoleDto>(_configurationProvider)
            .ToArrayAsync();

        var pageData = new PageViewData<RoleDto>
        {
            PageNumber = page,
            PageSize = pageSize,
            TotalItems = _unitOfWork.RoleRepository.TotalCount(),
            Items = roles
        };

        return new ServiceResponse<PageViewData<RoleDto>>(pageData);
    }
}