using System.Linq.Expressions;
using Models.Domain;
namespace Interfaces.Dal;

public interface IGenericRepository<TEntity>
    where TEntity : class, IUniqueDomainEntity
{
    /// <summary>
    /// Получить список сущностей по условию
    /// </summary>
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

    /// <summary>
    /// Общее кол-во обьектов
    /// </summary>
    int TotalCount();

    /// <summary>
    /// Обновить сущность
    /// </summary>
    void Update(TEntity entity);

    /// <summary>
    /// Удалить сущность
    /// </summary>
    void Delete(TEntity entity);

    /// <summary>
    /// Получить первую запись или значение по умолчанию
    /// </summary>
    Task<TEntity> FirsOrDefaultAsync();

    /// <summary>
    /// Получить список всех сущностей TEntity
    /// </summary>
    IQueryable<TEntity> GetAll();

    /// <summary>
    /// Добавить сущность
    /// </summary>
    Task AddAsync(TEntity entity);
}