using Microsoft.EntityFrameworkCore;
using IdentityWebApp.Data;
using IdentityWebApp.Services;
using IdentityWebApp.Other.Settings;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using IdentityWebApp.Extensions;
using Microsoft.AspNetCore.Identity;

namespace IdentityWebApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        ConfigureServices(builder);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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

        app.Run();
    }

    /// <summary>
    /// Добавление и настройка сервисов.
    /// </summary>
    /// <param name="builder">Создатель веб-приложения.</param>
    /// <exception cref="InvalidOperationException"/>
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        var isDevelopment = builder.Environment.IsDevelopment();
        
        var connectionString = builder.Configuration.GetConnectionString(ConstantsService.DefaultConnectionSectionName) ?? 
            throw new InvalidOperationException($"Строка подключения '{ConstantsService.DefaultConnectionSectionName}' не найдена.");
            
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
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

        builder.Services.AddRazorPages();                                 
        builder.Services.AddControllers();
      
        var smtpSettings = builder.Configuration.GetSection(ConstantsService.SmtpSettingsSectionName) ?? 
            throw new InvalidOperationException(ConstantsService.GenerateSectionNotFoundErrorMessage(ConstantsService.SmtpSettingsSectionName));

        builder.Services.Configure<SmtpSettings>(smtpSettings);

        var jwtSettings = builder.Configuration.GetSection(ConstantsService.JwtSettingsSectionName) ?? 
            throw new InvalidOperationException(ConstantsService.GenerateSectionNotFoundErrorMessage(ConstantsService.JwtSettingsSectionName));

        builder.Services.Configure<JwtSettings>(jwtSettings);

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = ConstantsService.DefaultCookieName;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);  //Задает дату истечения срока действия файла cookie
        });

        builder.Services.AddMemoryCache();
        builder.Services.RegisterServices(); 

        //Настраиваем аутентификацию: работу с JWT-токенами на предъявителя.
        builder.Services.AddAuthentication()
                        .AddJwtBearer(options => 
        {
            var apiKey = builder.Configuration[ConstantsService.JwtKeySectionName] ?? string.Empty;
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
}
