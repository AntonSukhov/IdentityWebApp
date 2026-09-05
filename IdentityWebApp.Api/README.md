# About

The IdentityWebApp.Api package provides a comprehensive set of tools for implementing authentication and authorization in .NET applications. It offers a clean and modern API for handling user credentials and managing access tokens.

# How to Use

## Using the Authentication Service class

The `AuthenticationService` class is used to authenticate a user. It encapsulates:
* Correct creation and configuration of `HttpClient`;
* Serialization/deserialization of requests and responses;
* Proper error handling;
* Communication with IdentityWebApp REST API endpoints.

**Important:** The `AuthenticationService` must be created via dependency injection or with a properly configured `IHttpClientFactory`.

### Example for Regular Applications (Manual Setup)

```csharp
using IdentityWebApp.Api.Helpers;
using IdentityWebApp.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

// Configure authentication settings
var authSettings = new AuthenticationSettings
{
    ServerName = "localhost",
    Port = 7121,
    UseHttps = false
};

// Get base address
var baseAddress = ApiUrlHelper.GetBaseAddress(authSettings);

// Create service collection and HttpClientFactory
var services = new ServiceCollection();

services.AddHttpClient(ApiConstants.HttpClientName, client =>
{
    client.BaseAddress = baseAddress;
    client.Timeout = TimeSpan.FromSeconds(ApiConstants.DefaultHttpClientTimeoutSeconds);
});

var serviceProvider = services.BuildServiceProvider();
var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

// Create AuthenticationService instance
var authService = new AuthenticationService(httpClientFactory);

// Perform login
try
{
    var token = await authService.LoginAsync(
        "user@example.com",
        "securepassword"
    );
    
    Console.WriteLine($"Token: {token.Value}");
    Console.WriteLine($"Expires: {token.Expires}");
}
catch (Exception ex)
{
    Console.WriteLine($"Authentication failed: {ex.Message}");
}
```

## Using the ServiceCollectionExtensions Class

For web applications, use the ServiceCollectionExtensions class to register the authentication service with the DI container.

### Example for Web Applications (ASP.NET Core)

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add authentication service with default Scoped lifetime
builder.Services.AddIdentityWebAppAuthentication(builder.Configuration);

// Or specify different service lifetime:

// Transient
builder.Services.AddIdentityWebAppAuthentication(
    builder.Configuration,
    ServiceLifetime.Transient
);

// Singleton
builder.Services.AddIdentityWebAppAuthentication(
    builder.Configuration,
    ServiceLifetime.Singleton
);
```

**Important:** Make sure to add the Authentication section to your appsettings.json:
```json
{
  "Authentication": {
    "ServerName": "localhost",
    "Port": 7121,
    "UseHttps": false
  }
}
```

# Main Types  

The main types provided by this library are:
* IdentityWebApp.Api.Services.IAuthenticationService
* IdentityWebApp.Api.Services.AuthenticationService
* IdentityWebApp.Api.Models.TokenModel
* IdentityWebApp.Api.Models.UserModel
* IdentityWebApp.Api.Settings.AuthenticationSettings
* IdentityWebApp.Api.Extensions.ServiceCollectionExtensions
* IdentityWebApp.Api.Helpers.ApiUrlHelper
* IdentityWebApp.Api.Constants.ApiConstants
* IdentityWebApp.Api.Constants.ErrorMessagesConstants

### Service Methods

| Service | Method | Description |
|---------|--------|-------------|
| `AuthenticationService` | `LoginAsync(userName, password, cancellationToken?)` | Authenticates a user against the IdentityWebApp REST API (`POST /api/token-auth/login`) and returns a `TokenModel` (JWT token value and its expiration date) |

> 💡 **Note:** The `cancellationToken` parameter is optional (defaults to `CancellationToken.None`). For detailed signatures and XML documentation, see `IAuthenticationService` in your IDE.

### Error Handling

The `AuthenticationService` handles API errors centrally and converts them into exceptions:

| Situation | HTTP status | Exception | Message |
|-----------|-------------|-----------|---------|
| Invalid login or password | `401 Unauthorized` | `InvalidOperationException` | «Неверный логин или пароль.» |
| Server unreachable or non-success response (e.g. `400 BadRequest`, `500 Internal Server Error`) | any non-success | `IOException` | «Ошибка подключения к серверу.» |
| Unexpected error (e.g. token deserialization failure) | — | `InvalidOperationException` | «Произошла непредвиденная ошибка при аутентификации.» |
| Operation cancelled via `CancellationToken` | — | `OperationCanceledException` | «Аутентификация отменена.» |

Server-side validation errors are returned as `400 BadRequest` (ModelState), invalid credentials — as `401 Unauthorized`. All error messages are centralized in `ErrorMessagesConstants`.

# Feedback & Contributing

IdentityWebApp.Api is released as open source under the MIT license. Bug reports, feature requests, and contributions are welcome at the GitHub repository.