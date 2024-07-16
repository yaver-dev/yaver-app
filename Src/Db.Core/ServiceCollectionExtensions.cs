using System.Data.Common;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using Yaver.App;

namespace Yaver.Db;

public static class ServiceCollectionExtensions {
  public static IServiceCollection AddDbDataSource<TDataSource>(
    this IServiceCollection services,
    object serviceKey,
    Func<IServiceProvider, object, TDataSource> configureDataSource)
    where TDataSource : DbDataSource {

    services.AddKeyedSingleton(
        serviceKey,
        (provider, options) => {
          return configureDataSource.Invoke(provider, options);
        });
    return services;
  }

  public static IServiceCollection RegisterDbContext<TDbContext>(this IServiceCollection services,
      Action<IServiceProvider, DbContextOptionsBuilder> configureDbContextOptionsBuilder,
      bool registerDbContextFactory = true) where TDbContext : BaseDbContext {

    if (registerDbContextFactory) {
      //Register Db context factory (this also registers db context)
      services.AddDbContextFactory<TDbContext>((provider, options) => {
        configureDbContextOptionsBuilder.Invoke(provider, options);
        var currentUserId = Guid.Empty;
        using (var scope = provider.CreateScope()) {
          var auditMetadata = scope.ServiceProvider.GetService<IAuditMetadata>();
          currentUserId = auditMetadata?.AuditInfo?.UserId ?? Guid.Empty;
        }
        options.AddInterceptors(new AuditableEntitiesInterceptor(currentUserId));
        options.UseSnakeCaseNamingConvention();
      });
    } else {
      //Register Db context
      services.AddDbContext<TDbContext>((provider, options) => {
        configureDbContextOptionsBuilder?.Invoke(provider, options);
        var currentUserId = Guid.Empty;
        using (var scope = provider.CreateScope()) {
          var auditMetadata = scope.ServiceProvider.GetService<IAuditMetadata>();
          currentUserId = auditMetadata?.AuditInfo?.UserId ?? Guid.Empty;
        }
        options.AddInterceptors(new AuditableEntitiesInterceptor(currentUserId));
        options.UseSnakeCaseNamingConvention();
      });
    }
    return services;
  }
}
