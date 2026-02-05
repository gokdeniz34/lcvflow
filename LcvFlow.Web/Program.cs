using Serilog;
using LcvFlow.Web.Components;
using LcvFlow.Data; // Extension metotlar için
using LcvFlow.Service; // Extension metotlar için
using LcvFlow.Web.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// --- 1. LOGGING (En Başta) ---
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .Enrich.FromLogContext()
    .WriteTo.MySQL(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        tableName: "Logs")
    .CreateLogger();

// --- 2. LAYER REGISTRATIONS ---
builder.Services.AddDataServices(builder.Configuration); // Data & DB
builder.Services.AddBusinessServices(); // Business & Auth

// --- 3. AUTHENTICATION & UI ---
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/admin/login";
        options.Cookie.Name = "LcvFlow.Auth";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers(); // API Controllerlar için şart!
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// --- 4. MIDDLEWARE PIPELINE ---

// Global Exception Middleware her zaman en üstte olmalı
app.UseMiddleware<ExceptionMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting(); // Auth'dan önce routing
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapControllers();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();