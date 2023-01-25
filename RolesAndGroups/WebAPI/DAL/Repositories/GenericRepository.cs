using System.Linq.Expressions;
using Interfaces.Dal;
using Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Newtonsoft.Json;
namespace DAL.Repositories;

/// <summary>
/// Репозиторий сущности
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
public class GenericRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class, IUniqueDomainEntity
{
    private readonly DbSet<TEntity> _dbSet;
    private readonly ILoggerService _logger;
    private readonly JsonSerializerSettings settings = new()
    {
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

    public GenericRepository(DbContext dbContext, ILoggerService logger)
    {
        _logger = logger;
        _dbSet = dbContext.Set<TEntity>();
    }

    /// <inheritdoc/>
    public int TotalCount() => _dbSet.Count();

    /// <inheritdoc/>
    public TEntity FirsOrDefault() => _dbSet.FirstOrDefault();

    /// <inheritdoc/>
    public TEntity LastOrDefault() => _dbSet.OrderBy(x => x.Id).LastOrDefault();

    /// <inheritdoc/>
    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression) => _dbSet.OrderBy(x => x.Id).Where(expression).AsQueryable();

    /// <inheritdoc/>
    public void Add(TEntity entity)
    {
        _dbSet.Add(entity);
        _logger.DbRequestsInfo("Add " + JsonConvert.SerializeObject(entity, settings));
    }

    /// <inheritdoc/>
    public void Update(TEntity entity)
    {
        entity.LastUpdateDateTime = DateTime.UtcNow;
        _dbSet.Update(entity);
        _logger.DbRequestsInfo("Update " + JsonConvert.SerializeObject(entity, settings));
    }

    /// <inheritdoc/>
    public void Delete(TEntity entity)
    {
        entity.DeletedDateTime = DateTime.UtcNow;
        _dbSet.Update(entity);
        _logger.DbRequestsInfo("Delete " + JsonConvert.SerializeObject(entity, settings));
    }

    /// <inheritdoc/>
    public IQueryable<TEntity> GetAll() => _dbSet.AsQueryable();

    /// <inheritdoc/>
    public void AddRange(IEnumerable<TEntity> entities)
    {
        _dbSet.AddRange(entities);
        _logger.DbRequestsInfo("Add" + JsonConvert.SerializeObject(entities, settings));
    }

    /// <inheritdoc/>
    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        foreach(var entity in entities)
            entity.LastUpdateDateTime = DateTime.UtcNow;
        _dbSet.UpdateRange(entities);
        _logger.DbRequestsInfo("Update " + JsonConvert.SerializeObject(entities, settings));
    }

    /// <inheritdoc/>
    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        foreach(var entity in entities)
            entity.DeletedDateTime = DateTime.UtcNow;
        _dbSet.UpdateRange(entities);
        _logger.DbRequestsInfo("Delete " + JsonConvert.SerializeObject(entities, settings));
    }

    /// <inheritdoc/>
    public async Task<TEntity> FirsOrDefaultAsync() => await _dbSet.FirstOrDefaultAsync();

    /// <inheritdoc/>
    public async Task<TEntity> LastOrDefaultAsync() => await _dbSet.OrderBy(x => x.Id).LastOrDefaultAsync();

    /// <inheritdoc/>
    public async Task AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
        _logger.DbRequestsInfo("Add" + JsonConvert.SerializeObject(entity, settings));
    }

    /// <inheritdoc/>
    public async Task AddRangeAsync(IEnumerable<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
        _logger.DbRequestsInfo("Add" + JsonConvert.SerializeObject(entities, settings));
    }
}