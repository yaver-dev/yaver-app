# Yaver

[![Build Status](https://img.shields.io/github/actions/workflow/status/yaverdev/Yaver/dotnet.yml?branch=main&style=flat-square)](https://github.com/yaverdev/Yaver/actions)
[![NuGet](https://img.shields.io/nuget/v/Yaver.App.Core?style=flat-square)](https://www.nuget.org/packages/Yaver.App.Core)
[![License](https://img.shields.io/github/license/yaverdev/Yaver?style=flat-square)](LICENSE)

A modular .NET library for building modern, high-performance APIs with FastEndpoints, supporting both REST and RPC patterns.

## Features

- **FastEndpoints Integration**: Built on top of FastEndpoints for high-performance API development
- **Modular Architecture**: Clean separation of concerns with dedicated modules
- **Multi-tenant Support**: Built-in capabilities for multi-tenant applications
- **Localization**: Comprehensive i18n support with JSON-based resources
- **Database Abstraction**: ORM integration with PostgreSQL and In-memory providers
- **Authentication**: JWT and custom authentication handlers
- **RPC Support**: Cross-service communication with FastEndpoints Messaging

## Installation

Install the core package via NuGet:

```bash
dotnet add package Yaver.App.Core
```

Add additional modules as needed:

```bash
dotnet add package Yaver.App.Api     # API utilities
dotnet add package Yaver.App.Service # Service components
dotnet add package Yaver.Db.Core     # Database abstraction
dotnet add package Yaver.Db.PostgreSQL # PostgreSQL provider
```

## Quick Start

### API Project Setup

```csharp
var builder = WebApplication.CreateSlimBuilder(args);

// Add configuration
builder.AddYaverConfiguration();

// Register services
builder.Services
    .AddFastEndpoints()
    .AddAuthentication(UserInfoAuthenticationHandler.SchemaName)
    .AddScheme<AuthenticationSchemeOptions, UserInfoAuthenticationHandler>(UserInfoAuthenticationHandler.SchemaName, null);

var app = builder.Build();

// Configure middleware
app
    .UseYaverExceptionHandler()
    .UseAuthentication()
    .UseAuthorization()
    .UseFastEndpoints();

// Map RPC handlers
app.MapRpcHandlers("ServiceBase", "service-url");

app.Run();
```

### Creating an Endpoint

```csharp
public class GetItemEndpoint : Endpoint<ItemRequest, ItemResponse>
{
    public override void Configure()
    {
        Get("/items/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ItemRequest req, CancellationToken ct)
    {
        // Handle request
        await SendAsync(new ItemResponse {
            Id = req.Id,
            Name = "Sample Item"
        });
    }
}
```

### Database Access

```csharp
public class MyDbContext : PgDbContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure entities
        builder.ApplyConfiguration(new UserEntityTypeConfiguration());
    }
}
```

## Project Structure

- **Yaver.App.Core**: Common components and base interfaces
- **Yaver.App.Api**: API-specific components and handlers
- **Yaver.App.Service**: Service components for RPC implementation
- **Yaver.Db.Core**: Database abstraction layer
- **Yaver.Db.PostgreSQL**: PostgreSQL implementation
- **Yaver.Db.InMemory**: In-memory database for testing

## Documentation

For detailed documentation, see [Documentation](https://yaver.dev/).

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## Credits

- [**FastEndpoints**](https://github.com/FastEndpoints/FastEndpoints) - The high-performance REST API framework that powers Yaver
- [**Entity Framework Core**](https://github.com/dotnet/efcore) - ORM for database access
- [**Npgsql**](https://github.com/npgsql/npgsql) - PostgreSQL provider for .NET
- [**FluentValidation**](https://github.com/FluentValidation/FluentValidation) - Request validation framework
- The entire Yaver team and contributors

Special thanks to all open source developers whose work has made this project possible.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
