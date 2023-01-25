using DAL.Contexts;
using DAL.Repositories;
using Interfaces.Dal;
using Interfaces.Services;
using Models.Domain;
namespace DAL.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    private IGenericRepository<Group> _groupRepository;
    private IGenericRepository<Role> _roleRepository;
    private readonly ILoggerService _logger;

    /// <summary>
    /// Конструктор
    /// </summary>
    public UnitOfWork(ApplicationDbContext dbContext, ILoggerService logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public IGenericRepository<Group> GroupRepository => _groupRepository ??= GetGenericRepository<Group>();

    public IGenericRepository<Role> RoleRepository => _roleRepository ??= GetGenericRepository<Role>();

    /// <inheritdoc/>
    public IGenericRepository<TEntity> GetGenericRepository<TEntity>()
        where TEntity : class, IUniqueDomainEntity => new GenericRepository<TEntity>(_dbContext, _logger);

    /// <inheritdoc/>
    public async Task SaveChangesAsync() => await _dbContext.SaveChangesAsync();

    /// <inheritdoc />
    public void Dispose()
    {
        _dbContext?.Dispose();
        GC.SuppressFinalize(this);
    }
}