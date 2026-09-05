# AGENTS.md

## Обзор проекта

**IdentityWebApp** — решение на ASP.NET Core для работы с пользователями приложений на основе ASP.NET Core Identity.

## Документация

- `README.md` — общее описание решения, архитектура, диаграммы (UML, ER), API-методы и обработка ошибок, конфигурация.
- `IdentityWebApp.Api/README.md` — документация клиентской библиотеки (методы, обработка ошибок, примеры подключения).

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

### Контроллер TokenAuthController (API аутентификации)

- **Endpoint**: `POST /api/token-auth/login`, принимает `UserLoginModel { Login, Password }`
- **Поток**: валидация (`UserLoginModelValidator` → `400 BadRequest`) → `FindByNameAsync` / `CheckPasswordAsync` (неуспех → `401 Unauthorized`) → выдача JWT (`200 OK`, `TokenModel { Value, Expires }`)
- **Кэширование**: выданный JWT кэшируется через `ICacheService<string, string>` и переиспользуется до истечения срока
- **JWT**: подпись HMAC-SHA512, время жизни из `JwtSettings.ExpiresInSeconds`

### База данных

- **Имя БД**: `DbAccounts`
- **Строка подключения**: `Host=localhost;Port=5432;Database=DbAccounts;Username=postgres;Password=<пароль — в appsettings.json / User Secrets>`
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
- **Обработка ошибок** (в `AuthenticationService`): `401` → `InvalidOperationException` («Неверный логин или пароль.»), недоступный сервер / прочие неуспешные статусы → `IOException`, отмена → `OperationCanceledException`; сообщения — в `ErrorMessagesConstants`

## Тесты (IdentityWebApp.Tests)

- Тесты контроллера `TokenAuthController` находятся в `Controllers/TokenAuthController/`
- Фикстура: `TokenAuthControllerFixture.cs` (моки `UserManager`, `IConfiguration`, `IOptions<JwtSettings>`)
- Тест-кейсы (в подпапке `LoginAsync/`): `LoginAsyncTests.cs`, `LoginAsyncTestCases.cs`, `UserContext.cs`

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