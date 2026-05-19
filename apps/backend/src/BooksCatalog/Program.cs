using System.Text.Json;
using BooksCatalog.Adapters.Persistence;
using BooksCatalog.Application.Services;
using BooksCatalog.Domain;
using BooksCatalog.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddJsonConsole(options =>
{
    options.JsonWriterOptions = new JsonWriterOptions { Indented = false };
});

builder.Services.AddControllers()
    .ConfigureApplicationPartManager(manager =>
    {
        manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Books Catalog API",
        Version = "v1"
    });
});

builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();
builder.Services.AddScoped<BookService>();

builder.Services.Configure<BooksCatalogLoaderOptions>(
    builder.Configuration.GetSection("BooksCatalogLoader"));
builder.Services.AddHostedService<BooksCatalogLoader>();

var app = builder.Build();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var logger = context.RequestServices
            .GetRequiredService<ILogger<Program>>();

        var feature = context.Features.Get<IExceptionHandlerFeature>();

        if (feature?.Error is not null)
        {
            logger.LogError(feature.Error, "Unhandled exception");
        }

        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        await context.Response.WriteAsJsonAsync(new { error = "Internal server error" });
    });
});

app.UseSwagger(options =>
{
    options.RouteTemplate = "api/swagger/{documentName}/swagger.json";
});
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/api/swagger/v1/swagger.json", "Books Catalog API v1");
    options.RoutePrefix = "api/swagger";
});

app.MapControllers();

app.Run();

public partial class Program;
