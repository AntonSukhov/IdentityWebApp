# AGENTS.md

## Обзор проекта

**IdentityWebApp** — решение на ASP.NET Core для работы с пользователями приложений на основе ASP.NET Core Identity.

## Структура решения

```
IdentityWebApp.sln
├── IdentityWebApp/          — Основное веб-приложение (ASP.NET Core 9, MVC + Razor Pages)
├── IdentityWebApp.Api/      — Клиентская библиотека аутентификации через API (NuGet-пакет v3.2.1)
└── IdentityWebApp.Tests/    — Unit-тесты (xUnit + Moq)
```

## Технологии

- **.NET 9.0**, C#, ASP.NET Core
- **ASP.NET Core Identity** (`ApplicationUser`, `ApplicationRole`)
- **Entity Framework Core 9** + **Npgsql** (PostgreSQL)
- **JWT-аутентификация** (`Microsoft.AspNetCore.Authentication.JwtBearer`)
- Пакеты `Infrastructure.*` (Caching, Security, AspNetCore)
- **xUnit + Moq** для тестирования

## Основной проект (IdentityWebApp)

- **Точка входа**: `Program.cs` → `Startup.cs` (наследует `Infrastructure.AspNetCore.StartupBase`)
- **Контекст БД**: `Data/ApplicationDbContext.cs` (наследует `IdentityDbContext<ApplicationUser, ApplicationRole, string>`)
- **Миграции EF Core**: `Data/Migrations/` (3 применённые миграции)
- **Контроллеры**: `Controllers/` (в т.ч. `TokenAuthController`)
- **Настройки**: `appsettings.json`, секции `ConnectionStrings:DefaultConnection`, `JwtSettings`, `SmtpSettings`

### База данных

- **Имя БД**: `DbAccounts`
- **Строка подключения**: `Host=localhost;Port=5432;Database=DbAccounts;Username=postgres;Password=sa`
- **Таблицы**: 8 таблиц Identity (`AspNetUsers`, `AspNetRoles`, и т.д.) + `__EFMigrationsHistory`

### Параметры Identity (в `Startup.cs`)

- `Password.RequiredLength = 8`, `RequiredUniqueChars = 6`
- `Lockout.MaxFailedAccessAttempts = 5`, `LockoutTimeSpan = 5 мин`
- JWT-ключ берётся из конфигурации (`AppConstants.JwtKeySectionName`), секрет хранится в User Secrets

## Библиотека API (IdentityWebApp.Api)

- **Назначение**: клиент для упрощения интеграции с API аутентификации
- **Версия**: 3.2.1, MIT, публикуется как NuGet-пакет
- **Основной сервис**: `IAuthenticationService.LoginAsync(userName, password)` → `TokenModel`
- **DI-регистрация**: `AddIdentityWebAppAuthentication(IServiceCollection, IConfiguration)`
- **Настройки**: секция `Authentication` (`AuthenticationSettings`: `ServerName`, `Port`, `UseHttps`)
- **Endpoint по умолчанию**: `/api/token-auth/login`

## Тесты (IdentityWebApp.Tests)

- Тесты контроллера `TokenAuthController` находятся в `Controllers/TokenAuthController/`
- Фикстура: `TokenAuthControllerFixture` (моки `UserManager`, `IConfiguration`, `IOptions<JwtSettings>`)
- Тест-кейсы: `LoginAsyncTests.cs`, `LoginAsyncTestCases.cs`, `UserContext.cs`

## Команды

```bash
# Сборка решения
dotnet build IdentityWebApp.sln

# Запуск тестов
dotnet test IdentityWebApp.sln

# Применение миграций (из каталога IdentityWebApp)
dotnet ef database update --project ../IdentityWebApp/IdentityWebApp.csproj

# Сборка NuGet-пакета API
dotnet pack IdentityWebApp.Api/IdentityWebApp.Api.csproj
```

## Примечания

- Комментарии в коде на русском языке.
- Код веб-приложения имеет пакет-предок `Infrastructure.AspNetCore.StartupBase` — при конфигурации учитывайте порядок вызова `base.ConfigureServices` / `base.Configure`.
- Не изменяйте существующие тесты без необходимости.