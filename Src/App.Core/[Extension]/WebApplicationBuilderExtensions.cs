using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

// ReSharper disable once CheckNamespace
namespace Yaver.App;

/// <summary>
///   Provides extension methods for configuratıon
/// </summary>
public static class WebApplicationBuilderExtensions {

  /// <summary>
  ///   Provides a way to add configuration jsons
  /// </summary>
  public static WebApplicationBuilder AddYaverConfiguration(this WebApplicationBuilder builder) {
    builder.Configuration
      .AddJsonFile("appsettings.json", true, true)
      .AddJsonFile("appsecrets.json", optional: true, true)
      .AddEnvironmentVariables();

    return builder;
  }
}
