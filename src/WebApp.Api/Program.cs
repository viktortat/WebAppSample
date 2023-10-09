using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using System.Reflection;
using WebApp.Api.Data;
using WebApp.Api.Data.Fakers;
using WebApp.Api.Middlewares;
using WebApp.Api.Services;

const string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

try
{
    Log.Logger = new LoggerConfiguration()
  //#if DEBUG
  .MinimumLevel.Debug()
  //#else
  //        .MinimumLevel.Information()
  //#endif
  .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
  .Enrich.FromLogContext()
  .WriteTo.Async(c => c.Console(new JsonFormatter()))
  .CreateLogger();

    var builder = WebApplication.CreateBuilder(args);
    var envAsp = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    if (string.IsNullOrEmpty(envAsp))
    {
        envAsp = "Development";
        Console.WriteLine("Not set Environment Variable - 'ASPNETCORE_ENVIRONMENT'. Default set - 'Development'");
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", envAsp);
    }

    Console.WriteLine(new string('=', 80));
    Console.WriteLine(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
    Console.WriteLine(new string('=', 80));

    //var connectionString = builder.Configuration.GetConnectionString("pizzas") ?? "Data Source=pizzas.db";
    //builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
    //var connectionString = builder.Configuration.GetConnectionString("pizzas") ?? "Data Source=pizzas.db";
    //builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(connectionString));
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnectionLite");

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        //var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        //var path = AppDomain.CurrentDomain.BaseDirectory;
        //options.UseSqlite($"Data Source={Path.Join(path, "pizzas.db")}");
        options.UseSqlite(connectionString);
    });

    builder.Services.AddTransient<Fakers>();
    builder.Services.AddTransient<IPizzaService, PizzaService>();

    builder.Services.AddControllers().AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen(c =>
    {
        var _name = Assembly.GetExecutingAssembly().GetName().Name;
        var _ver = Assembly.GetExecutingAssembly().GetName().Version;
        var env = Environment.GetEnvironmentVariables()["ASPNETCORE_ENVIRONMENT"] ?? "test";

        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = $"API - {_name}",
            Version = env + $"   v{_ver}",
            Description = "Test Api .",
            TermsOfService = new Uri("https://go.microsoft.com/fwlink/?LinkID=206977"),
            Contact = new OpenApiContact
            {
                Name = "Your name",
                Email = string.Empty,
                Url = new Uri("https://learn.microsoft.com/training")
            }
        });

        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });

    builder.Services.AddCors(options =>
    {
        options.AddPolicy(name: MyAllowSpecificOrigins,
            builder =>
            {
                builder.WithOrigins("*");
            });
    });

    builder.Services.AddTransient<IEmailSender, AuthMessageSender>();
    builder.Services.AddTransient<ISmsSender, AuthMessageSender>();
    builder.Services.AddTransient<ILoggingService, LoggingService>();

    builder.Services.AddHttpContextAccessor();

    var middlewareSettings = builder.Configuration.GetSection("MiddlewareSettings").Get<MiddlewareSettings>();

    var app = builder.Build();

    app.UseStatusCodePagesWithRedirects("/Error/{0}");

    var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();

    app.UseMiddleware<LayoutMiddleware>();
    app.UseLoggingMiddleware();
    if (middlewareSettings.UseCorrelationMiddleware) app.UseCorrelationMiddleware();
    if (middlewareSettings.UseErrorHandlingMiddleware) app.UseErrorHandlingMiddleware();
    if (middlewareSettings.UseTimeLoggingMiddleware) app.UseTimeLoggingMiddleware();
    if (middlewareSettings.UseCultureMiddleware) app.UseCultureMiddleware();
    if (middlewareSettings.UseIntentionalDelayMiddleware) app.UseIntentionalDelayMiddleware();

    using (var scope = scopeFactory.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //var db = scope.ServiceProvider.GetService<ApplicationDbContext>();

        var RecreateDb = Environment.GetEnvironmentVariable("RecreateDb");
        if (bool.TryParse(RecreateDb, out bool myVal))
        {
            db?.Database.EnsureDeleted();
        }


        //db?.Database.MigrateAsync();
        if (db.Database.EnsureCreated())
        {
            await SeedData.Initialize(db);
        }
    }

    app.UseCors(MyAllowSpecificOrigins);
    //if (app.Environment.IsDevelopment())
    //{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(c =>
    //{
    //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API V1");
    //});
    //}

    //app.UseHttpsRedirection();

    //app.Logger.LogWarning($"ASPNETCORE_ENVIRONMENT: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")} " );
    //app.Logger.LogWarning($"Is Development: {app.Environment.IsDevelopment().ToString()}" );

    app.UseDefaultFiles();
    app.UseStaticFiles();

    app.UseAuthorization();
    app.MapControllers();

    #region MapGets
    /*
    app.MapGet("/", () => "Hello World!");

    app.MapGet("/pizzas", async(ApplicationDbContext db) => await db.Pizzas.ToListAsync());

    app.MapPost("/pizzas", async(ApplicationDbContext db, Product pizza) => {
        await db.Pizzas.AddAsync(pizza);
        await db.SaveChangesAsync();
        return Results.Created($"/pizza/{pizza.Id}", pizza);
    });


    app.MapGet("/pizzas/{id}", async (ApplicationDbContext db, int id) =>
    {
        var pizzaItem = await db.Pizzas.FindAsync(id);
        if (pizzaItem is null) return Results.NotFound();
        return Results.Ok(pizzaItem);
    });


    app.MapPut("/pizzas/{id}", async (ApplicationDbContext db, Product updatePizza, int id) =>
    {
        var pizzaItem = await db.Pizzas.FindAsync(id);
        if (pizzaItem is null) return Results.NotFound();
        pizzaItem.Name = updatePizza.Name;
        pizzaItem.Description = updatePizza.Description;
        await db.SaveChangesAsync();
        return Results.NoContent();
    });

    app.MapDelete("/pizzas/{id}", async (ApplicationDbContext db, int id) =>
    {
        var todo = await db.Pizzas.FindAsync(id);
        if (todo is null)
        {
            return Results.NotFound();
        }
        db.Pizzas.Remove(todo);
        await db.SaveChangesAsync();
        return Results.Ok();
    });
    */

    #endregion

    await app.RunAsync();

    return 0;
}
catch (Exception ex)
{
    var type = ex.GetType().Name;
    if (type.Equals("StopTheHostException", StringComparison.Ordinal))
    {
        throw;
    }
    Log.Fatal(ex, "Unhandled exception");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

public partial class Program { }
