using System.Diagnostics;
using DAL.DbInitializers;
using WebAPI.Configuration;
using WebAPI.Middlewares;
Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCorsPolicy()
    .AddLogger(builder)
    .AddSwagger()
    .AddAutoMapperInfrastructure()
    .AddApplicationDbContext(builder.Configuration)
    .AddUnitIfWork()
    .AddServices()
    .AddControllers();

var app = builder.Build();

//если миграции включены то выполняем их
var isMigrationsEnabled = builder.Configuration.GetSection("IsMigrationsEnabled").Get<bool>();
if(isMigrationsEnabled)
{
    var scope = app.Services.CreateScope();
    await DbInitializer.InitializeAsync(scope.ServiceProvider);
}

//подключаем сваггер если это запуск в дебаге
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "WebAPI Documentation";
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI V1");
    });
}

app.UseMiddleware<ErrorHandlingMiddleware>()
    .UseCors("CorsPolicy")
    .UseRouting()
    .UseAuthorization()
    .UseEndpoints(endpoints => endpoints.MapControllers())
    .UseHttpsRedirection();
app.MapControllers();

Console.WriteLine("App run");
//получаем порт на котором запустим АПИшку
var port = builder.Configuration.GetSection("Kestrel")["DefaultPort"];
app.Run($"https://localhost:{port}/");