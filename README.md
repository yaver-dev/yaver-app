[![Build status](https://github.com/michelcedric/YaverMinimalApi/actions/workflows/dotnet.yml/badge.svg)](https://github.com/michelcedric/YaverMinimalApi/actions/workflows/dotnet.yml)
[![Downloads](https://img.shields.io/nuget/dt/TaskManager.ApiBase?color=blue&label=Downloads&logo=nuget)](https://www.nuget.org/packages/TaskManager.ApiBase)
# YaverMinimalApi
The goal of this project it's to show how to use TaskManager.ApiBase package.
It demontrate how to configure API endpoints as individual classes based on minimal Api (.Net 6)

## Program.cs
Use [AddEndpoints](https://github.com/michelcedric/YaverMinimalApi/blob/master/TaskManager.ApiBase/Extensions/IServiceCollectionExtensions.cs#L7) extenion method to create each endpoint.

And also [MapEndpoint](https://github.com/michelcedric/YaverMinimalApi/blob/master/TaskManager.ApiBase/Extensions/IEndpointRouteBuilderExtensions.cs#L8) extension method to use new routing APIs

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpoints();

var app = builder.Build();

app.MapEndpoints();

app.Run();
```

## Define an endpoint
To create and define one endpoint, it needs to implement [IEndpoint](https://github.com/michelcedric/YaverMinimalApi/blob/master/TaskManager.ApiBase/IEndpoint.cs) interface

```csharp
public class GetWithParamEndpoint : IEndpoint<string, string>
    {
        public void AddRoute(IEndpointRouteBuilder app)
        {
            app.MapGet("/Todo/2/{param1}", (string param1) => HandleAsync(param1));
        }

        public Task<string> HandleAsync(string request)
        {
            return Task.FromResult($"Hello World! 2 {request}");
        }
    }
```

## Projects Using TaskManager.ApiBase

- [eShopOnWeb](https://github.com/dotnet-architecture/eShopOnWeb): Sample ASP.NET Core reference application, powered by Microsoft
    - [Use in PublicApi project](https://github.com/dotnet-architecture/eShopOnWeb/tree/main/src/PublicApi): This project demonstrates how to configure endpoints as individual classes

- [EshopOnVue.js](https://github.com/michelcedric/EshopOnVue.js): Same as EshopOnWeb project in Vue.js

- [YaverMinimalApi](https://github.com/michelcedric/YaverMinimalApi/tree/master/YaverMinimalApi): Sample project to show some usage

- [WebApiBestPractices](https://github.com/ardalis/WebApiBestPractices): Resources related to Ardalis Pluralsight course on this topic.
    - [Pluralsight : ASP.NET Core 6 Web API: Best Practices](https://www.pluralsight.com/courses/aspdotnet-core-6-web-api-best-practices): Organizing Minimal API demo and best practices.

## Nuget Package
A nuget package is available [here](https://www.nuget.org/packages/TaskManager.ApiBase/).
