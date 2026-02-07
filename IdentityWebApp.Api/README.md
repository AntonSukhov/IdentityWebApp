About

The IdentityWebApp.Api package provides a comprehensive set of tools for implementing authentication and authorization in .NET applications. It offers a clean and modern API for handling user credentials and managing access tokens.

How to Use

Using the Authentication Service class

The AuthenticationService class is used to authenticate a user. Moreover, it encapsulates the correct creation of HttpClient, serialization/deserialization of the request/response, as well as encryption of the user's password. 
The AuthenticationService сommunication with IdentityWebApp REST API endpoints.

Example:

    // Create service instance
    var service = new AuthenticationService();

    // Perform login
    TokenModel loginResult = await service.LoginAsync(
        host: "api.example.com",
        port: 443,
        username: "user@example.com",
        password: "securepassword"
    );

    Console.WriteLine($"Token: {loginResult.Value} Expires: {loginResult.Expires}");


Main Types  

The main types provided by this library are:

    IdentityWebApp.Api.Services.AuthenticationService
    IdentityWebApp.Api.Models.TokenModel


Feedback & Contributing

IdentityWebApp.Api is released as open source under the MIT license. Bug reports, feature requests, and contributions are welcome at the GitHub repository.