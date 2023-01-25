using Microsoft.EntityFrameworkCore;
using Models.Domain;
namespace DAL.Contexts;

/// <summary>
/// Контекст подключения к базе данных форума
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// Роли
    /// </summary>
    public DbSet<Role> Roles { get; set; }

    /// <summary>
    /// Группы
    /// </summary>
    public DbSet<Group> Groups { get; set; }

    public ApplicationDbContext(DbContextOptions options) : base(options) { }
}