using Microsoft.EntityFrameworkCore;
using IdentityWebApp.Data;
using IdentityWebApp.Other;
using Microsoft.AspNetCore.Identity.UI.Services;
using IdentityWebApp.Services.Senders;
using IdentityWebApp.Services;

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
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapRazorPages()
           .WithStaticAssets();

        app.Run();
    }

    /// <summary>
    /// Добавление и настройка сервисов.
    /// </summary>
    /// <param name="builder">Создатель веб-приложения.</param>
    /// <exception cref="InvalidOperationException"/>
    private static void ConfigureServices(WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString(ConstantsService.DefaultConnectionSectionName) ?? 
            throw new InvalidOperationException($"Строка подключения '{ConstantsService.DefaultConnectionSectionName}' не найдена.");
            
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        {

            options.SignIn.RequireConfirmedAccount = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 6;
            options.Lockout.MaxFailedAccessAttempts = 5;                      //Максимально допустимое количество неудачных попыток доступа
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); //Время блокировки пользователя по умолчанию.
            options.Lockout.AllowedForNewUsers = true;                        //Новый пользователь может быть заблокирован при неудачных попытках доступа.
        })
        .AddEntityFrameworkStores<ApplicationDbContext>();

        builder.Services.AddRazorPages();

      
        var smtpSettings = builder.Configuration.GetSection(ConstantsService.SmtpSettingsSectionName) ?? 
            throw new InvalidOperationException($"Секция '{ConstantsService.SmtpSettingsSectionName}' не найдена.");

        builder.Services.Configure<SmtpSettings>(smtpSettings);

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.Name = ConstantsService.DefaultCookieName;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(20);  //Задает дату истечения срока действия файла cookie
        });

        builder.Services.AddTransient<IEmailSender, SmtpEmailSender>();

    }
}
