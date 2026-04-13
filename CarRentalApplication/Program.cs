var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Remove DB registration for Docker testing
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient("ApiGateway", (sp, client) =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();

    client.BaseAddress = new Uri(configuration["ApiGateway:BaseUrl"]!);
    client.DefaultRequestHeaders.Add("X-Api-Key", configuration["ApiGateway:ApiKey"]!);
});

var app = builder.Build();

app.UseExceptionHandler("/Home/Error");

// Comment these for Docker HTTP testing
// app.UseHsts();
// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();