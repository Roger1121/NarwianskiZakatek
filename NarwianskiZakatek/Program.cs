using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NarwianskiZakatek.Data;
using NarwianskiZakatek.Models;
using NarwianskiZakatek.Repositories;
using NarwianskiZakatek.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
/*builder.WebHost.ConfigureKestrel(serverOptions =>
{
    serverOptions.ConfigureEndpointDefaults(listenOptions =>
    {
        listenOptions.UseHttps("/app/devkey.pfx", "P@$$w0rd1234");
    });
});*/

// Database config
var connectionString = config.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Identity config
builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddErrorDescriber<LocalizedIdentityErrorDescriber>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredUniqueChars = 6;

    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
    options.Lockout.MaxFailedAccessAttempts = 5;

    options.SignIn.RequireConfirmedEmail = true;

    options.User.RequireUniqueEmail = true;
});

// cookie settings
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(1);
    options.SlidingExpiration = true;
});

//HSTS config
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

// SMTP config
builder.Services.AddSingleton(config.GetSection("EmailConfig").Get<EmailConfig>());
builder.Services.AddScoped<IEmailService, EmailService>();

//Services
builder.Services.AddScoped<IRoomsService, RoomsService>();
builder.Services.AddScoped<IDescriptionsService, DescriptionsService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IPostsService, PostsService>();
builder.Services.AddScoped<IReservationsService, ReservationsService>();

// Captcha config
builder.Services.AddSingleton(config.GetSection("CaptchaConfig").Get<CaptchaConfig>());
builder.Services.AddScoped<ICaptchaService, CaptchaService>();
builder.Services.AddHttpClient<CaptchaService>();

//External login config
builder.Services.AddAuthentication()
   .AddGoogle(options =>
   {
       IConfigurationSection googleAuthNSection =
       config.GetSection("Authentication:Google");
       options.ClientId = googleAuthNSection["ClientId"];
       options.ClientSecret = googleAuthNSection["ClientSecret"];
   })
   .AddFacebook(options =>
   {
       IConfigurationSection FBAuthNSection =
       config.GetSection("Authentication:Facebook");
       options.ClientId = FBAuthNSection["AppId"];
       options.ClientSecret = FBAuthNSection["AppSecret"];
   })
   .AddMicrosoftAccount(microsoftOptions =>
   {
       microsoftOptions.ClientId = config["Authentication:Microsoft:ClientId"];
       microsoftOptions.ClientSecret = config["Authentication:Microsoft:ClientSecret"];
   });

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
var app = builder.Build();

app.Use(async (context, next) =>
{
    context.Response.Headers.Add("Content-Security-Policy", "frame-src maps.google.com; script-src 'self' www.google.com www.gstatic.com; connect-src api.weatherbit.io; style-src 'unsafe-hashes' 'sha256-aqNNdDLnnrDOnTNdkJpYlAxKVJtLt9CtFLklmInuUAE=' 'self'; default-src 'self';");
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Posts}/{action=Index}/{id?}");
app.MapRazorPages();

// Inicjalizacja bazy danych
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<ApplicationDbContext>();
    if (db.Database.IsRelational())
    {
        db.Database.Migrate();
    }
    SeedData.Initialize(services);
}

app.Run();

public partial class Program { }