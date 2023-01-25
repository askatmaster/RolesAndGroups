using System.Reflection;
using DAL.Contexts;
using DAL.Repositories;
using DAL.UnitOfWork;
using Interfaces.Dal;
using Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using Services;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
namespace WebAPI.Configuration;

/// <summary>
/// Конфигурация сервисов
/// </summary>
internal static class DIConfigurations
{
    /// <summary>
    /// Добавление политики CORS
    /// </summary>
    public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
    {
        return services.AddCors(x => x.AddPolicy("CorsPolicy",
            builder => builder.SetIsOriginAllowed(_ => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .WithExposedHeaders("Content-Disposition")
                .AllowCredentials()));
    }

    /// <summary>
    ///Добавить сваггер
    /// </summary>
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        return services.AddSwaggerGen(x =>
        {
            x.SwaggerDoc("v1", new OpenApiInfo { Title = "RolesAndGroups", Version = "v1" });
            x.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "WebAPI.xml"));
            x.DescribeAllParametersInCamelCase();
            x.UseOneOfForPolymorphism();
        });
    }

    /// <summary>
    /// Добавить логирование
    /// </summary>
    public static IServiceCollection AddLogger(this IServiceCollection services, WebApplicationBuilder builder)
    {
        LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
        builder.Logging.ClearProviders();
        builder.Logging.SetMinimumLevel(LogLevel.Information);
        builder.Host.UseNLog();

        return services;
    }

    /// <summary>
    /// Добавить инфраструктуру автомаппера
    /// </summary>
    public static IServiceCollection AddAutoMapperInfrastructure(this IServiceCollection services)
    {
        var profiles = Assembly.Load("Infrastructure");

        return services.AddAutoMapper(profiles);
    }

    /// <summary>
    /// Добавить строку подключения
    /// </summary>
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, ConfigurationManager manager)
    {
        var forumConnectionString = manager.GetConnectionString("DbConnection");
        services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(forumConnectionString));

        return services;
    }

    /// <summary>
    ///Добавить UnitOfWork
    /// </summary>
    public static IServiceCollection AddUnitIfWork(this IServiceCollection services) => services.AddScoped<IUnitOfWork, UnitOfWork>()
        .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));

    /// <summary>
    /// Добавление служб
    /// </summary>
    public static IServiceCollection AddServices(this IServiceCollection services) => services.AddTransient<IRoleService, RoleService>()
        .AddTransient<IGroupService, GroupService>()
        .AddSingleton<ILoggerService, LoggerService>();
}