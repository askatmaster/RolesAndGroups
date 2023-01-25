using Models.Domain;
namespace Interfaces.Dal;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Получить репозиторий сущности
    /// </summary>
    /// <typeparam name="TEntity">Доменная сущность</typeparam>
    IGenericRepository<TEntity> GetGenericRepository<TEntity>()
        where TEntity : class, IUniqueDomainEntity;

    public IGenericRepository<Group> GroupRepository { get; }

    public IGenericRepository<Role> RoleRepository { get; }

    /// <summary>
    /// Сохранить изменения асинхронно
    /// </summary>
    Task SaveChangesAsync();
}