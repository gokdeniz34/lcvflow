using Serilog;
using LcvFlow.Web.Components;
using LcvFlow.Data;
using LcvFlow.Data.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Serilog.Debugging.SelfLog.Enable(msg => Console.WriteLine(msg));

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.MySQL(
        connectionString: connectionString,
        tableName: "Logs",
        storeTimestampInUtc: true
    )
    .CreateLogger();

Log.Information("testtt gökk");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 31)), // Versiyonu elle yaz
        b => b.MigrationsAssembly("LcvFlow.Data")));

builder.Services.AddDataServices(builder.Configuration);
// builder.Services.AddApplicationServices(); // Daha sonra eklenecek

// 3. BLAZOR VE UI SERVISLERI
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// 4. HTTP PIPELINE & MIDDLEWARE
// GlobalExceptionMiddleware'i buraya, pipeline'ın en başına ekleyeceğiz.
// app.UseMiddleware<GlobalExceptionMiddleware>(); 

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();