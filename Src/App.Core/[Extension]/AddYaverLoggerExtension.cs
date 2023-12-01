using Microsoft.AspNetCore.Builder;

using Serilog;

// ReSharper disable once CheckNamespace
namespace Yaver.App;

/// <summary>
///   Provides extension methods for configuring logging in a <see cref="Microsoft.AspNetCore.Builder.WebApplication" />.
/// </summary>
public static class AddYaverLoggerExtension {
  /// <summary>
  ///   Provides a way to configure a <see cref="Microsoft.AspNetCore.Builder.WebApplication" />.
  /// </summary>
  public static WebApplicationBuilder AddYaverLogger(
    this WebApplicationBuilder builder) {
    _ = builder.Host.UseSerilog((ctx, lc) => lc
      .ReadFrom.Configuration(ctx.Configuration)
      .Enrich.WithProperty("AppName", Environment.GetEnvironmentVariable("YAVER_APP_NAME") ?? "YAVER_APP")
    );

    return builder;
  }

  // public static WebApplicationBuilder AddYaverLogger(
  //   this WebApplicationBuilder builder,
  //   Action<HostBuilderContext, LoggerConfiguration> auditLogConfiguraton) {

  //   builder.Host.UseSerilog((ctx, lc) => {
  //     lc.ReadFrom.Configuration(ctx.Configuration)
  //     .Enrich.WithProperty("AppName", Environment.GetEnvironmentVariable("YAVER_APP_NAME") ?? "YAVER_APP");
  //     if (auditLogConfiguraton is not null) {
  //       lc.Destructure.With<LogWrapperDestructuringPolicy>();
  //       auditLogConfiguraton(ctx, lc);
  //     }
  //   });
  //   return builder;
  // }w
}

// public class LogWrapperDestructuringPolicy : IDestructuringPolicy {
//   public bool TryDestructure(
//     object value,
//     ILogEventPropertyValueFactory propertyValueFactory,
//     [NotNullWhen(true)] out LogEventPropertyValue? result) {

//     if (value is ILogWrapper l) {
//       // Get underlying object and stop destructuring
//       result = propertyValueFactory.CreatePropertyValue(l.Wrapped, false);
//       return true;
//     }
//     result = null;
//     return false; // Let the default policy handle it
//   }
// }
