using System.Text;
using IdentityWebApp.Data;
using IdentityWebApp.Extensions;
using IdentityWebApp.Other.Settings;
using IdentityWebApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace IdentityWebApp;

/// <summary>
/// Компонент начальной конфигурации приложения ASP.NET Core.
/// </summary>
/// <remarks>
/// Отвечает за настройку сервисов (DI‑контейнер) и конвейера обработки HTTP‑запросов.
/// </remarks>
internal class Startup: Infrastructure.AspNetCore.StartupBase
{
    /// <summary>
    /// Инициализирует экземпляр <see cref="Startup"/>.
    /// </summary>
    /// <param name="configuration">Настройки приложения.</param>
    public Startup(IConfiguration configuration, IWebHostEnvironment environment): 
        base(configuration, environment) {}

    /// <summary>
    /// Выполняет регистрацию сервисов, используемых в приложении.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public override void ConfigureServices(IServiceCollection services)
    {      
        base.ConfigureServices(services);

        var isDevelopment = Environment.IsDevelopment();
        
        var connectionString = Configuration.GetConnectionString(ConstantsService.DefaultConnectionSectionName) ?? 
            throw new InvalidOperationException($"Строка подключения '{ConstantsService.DefaultConnectionSectionName}' не найдена.");
            
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
        {

            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 6;
            options.Lockout.MaxFailedAccessAttempts = 5;                      //Максимально допустимое количество неудачных попыток доступа
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //Время блокировки пользователя по умолчанию.
            options.Lockout.AllowedForNewUsers = true;                        //Новый пользователь может быть заблокирован при неудачных попытках доступа.
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders(); // Добавлено для поддержки токенов

        services.AddRazorPages();                                 
        services.AddControllers();
      
        var smtpSettings = Configuration.GetSection(ConstantsService.SmtpSettingsSectionName) ?? 
            throw new InvalidOperationException(
                ConstantsService.GenerateSectionNotFoundErrorMessage(ConstantsService.SmtpSettingsSectionName));

        services.Configure<SmtpSettings>(smtpSettings);

        var jwtSettings = Configuration.GetSection(ConstantsService.JwtSettingsSectionName) ?? 
            throw new InvalidOperationException(
                ConstantsService.GenerateSectionNotFoundErrorMessage(ConstantsService.JwtSettingsSectionName));

        services.Configure<JwtSettings>(jwtSettings);

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = ConstantsService.DefaultCookieName;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);  //Задает дату истечения срока действия файла cookie
        });

        services.AddMemoryCache();
        services.RegisterServices(); 

        //Настраиваем аутентификацию: работу с JWT-токенами на предъявителя.
        services.AddAuthentication()
                .AddJwtBearer(options => 
        {
            var apiKey = Configuration[ConstantsService.JwtKeySectionName] ?? string.Empty;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(apiKey));

            options.SaveToken = true;                       //Токен будет сохранен в AuthenticationProperties
            options.RequireHttpsMetadata = !isDevelopment;  //Если задано значение false, то приложение будет работать и с обычным HTTP, 
                                                            //что может быть полезно для некоторых сценариев тестирования, но в рабочем окружении
                                                            //этого, конечно же, следует избегать.

            options.TokenValidationParameters = new TokenValidationParameters
            {
                 ValidateAudience = false,  //Выполнять или нет проверку аудитории токена или его предполагаемого получателя.
                 ValidateIssuer = false,    //Выполнять или нет проверку издателя токена.
                 IssuerSigningKey = key
            };
        });
    }

    /// <summary>
    /// Выполняет настройку конвейера HTTP-запросов. Вызывается средой выполнения.
    /// </summary>
    /// <param name="app">Веб-приложение.</param>
    public override void Configure(WebApplication app)
    {       
        base.Configure(app);

        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
            app.UseHttpsRedirection();
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();
        app.MapControllers();
    }
}

