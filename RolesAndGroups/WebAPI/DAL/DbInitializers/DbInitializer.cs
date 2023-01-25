using DAL.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Models.Domain;
namespace DAL.DbInitializers;

/// <summary>
/// Инициализатор базы данных
/// </summary>
public static class DbInitializer
{
    /// <summary>
    /// Инициализвация базы данных
    /// </summary>
    public static async Task InitializeAsync(IServiceProvider serviceProvider)
    {
        //получаем контекс БД
        await using var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        //проводим миграции
        await context.Database.MigrateAsync();

        //заполняем таблицы
        await InitGroupAndRolesAsync(context);
        await context.SaveChangesAsync();
    }

    /// <summary>
    /// Инициалмзмровать группы и роли
    /// </summary>
    private static async Task InitGroupAndRolesAsync(ApplicationDbContext context)
    {
        var role = await context.Roles.FirstOrDefaultAsync();

        if(role is not null)
            return;

        var initRole1 = new Role { Name = "TestRole1" };
        var initRole2 = new Role { Name = "TestRole2" };
        var initRole3 = new Role { Name = "TestRole3" };
        var initRole4 = new Role { Name = "TestRole4" };
        var initRole5 = new Role { Name = "TestRole5" };
        var initRole6 = new Role { Name = "TestRole6" };
        var initRole7 = new Role { Name = "TestRole7" };
        var initRole8 = new Role { Name = "TestRole8" };

        await context.Roles.AddAsync(initRole1);
        await context.Roles.AddAsync(initRole2);
        await context.Roles.AddAsync(initRole3);
        await context.Roles.AddAsync(initRole4);
        await context.Roles.AddAsync(initRole5);
        await context.Roles.AddAsync(initRole6);
        await context.Roles.AddAsync(initRole7);
        await context.Roles.AddAsync(initRole8);

        var initGroup1 = new Group { Name = "TestGroup1" };
        var initGroup2 = new Group { Name = "TestGroup2" };
        var initGroup3 = new Group { Name = "TestGroup3" };
        var initGroup4 = new Group { Name = "TestGroup4" };
        var initGroup5 = new Group { Name = "TestGroup5" };
        var initGroup6 = new Group { Name = "TestGroup6" };
        var initGroup7 = new Group { Name = "TestGroup7" };
        var initGroup8 = new Group { Name = "TestGroup8" };

        await context.Groups.AddAsync(initGroup1);
        await context.Groups.AddAsync(initGroup2);
        await context.Groups.AddAsync(initGroup3);
        await context.Groups.AddAsync(initGroup4);
        await context.Groups.AddAsync(initGroup5);
        await context.Groups.AddAsync(initGroup6);
        await context.Groups.AddAsync(initGroup7);
        await context.Groups.AddAsync(initGroup8);
    }
}