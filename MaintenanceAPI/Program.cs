using MaintenanceAPI.Services;
using MaintenanceAPI.Middleware;
using MaintenanceAPI.Middlelware;
using System.Collections.Concurrent;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepairHistoryService, FakeRepairHistoryService>();

var usageCounts = new ConcurrentDictionary<string, int>();
builder.Services.AddSingleton(usageCounts);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new
        {
            error = "ServerError",
            message = "An unexpected error occurred."
        });
    }
});

const string API_KEY = "MY_SECRET_KEY_123";

app.Use(async (context, next) =>
{
    if (!context.Request.Headers.TryGetValue("X-Api-Key", out var key) ||
        key != API_KEY)
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsJsonAsync(new
        {
            error = "Unauthorized",
            message = "Missing or invalid API key."
        });
        return;
    }

    await next();
});


app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
