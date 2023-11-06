using Microsoft.AspNetCore.Builder;

namespace Yaver.App;

/// <summary>
/// Provides extension methods for mapping RPC handlers to a web application.
/// </summary>
public static class MapRpcHandlersExtensions {
  /// <summary>
  /// Represents a minimal API web application.
  /// </summary>
  public static WebApplication MapRpcHandlers(
    this WebApplication app,
    string serviceName,
    string serviceUrl) {

    if (serviceUrl is null) throw new ArgumentNullException(nameof(serviceUrl));

    if (serviceName is null) throw new ArgumentNullException(nameof(serviceName));

    var commands = AppDomain.CurrentDomain
      .GetAssemblies().First(x => x.GetName().Name == serviceName)
      .GetTypes()
      .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRpcCommand<>)))
      .Where(t => !t.IsInterface && !t.IsAbstract);

    app.MapRemote(serviceUrl, rc => {
      var registerMethod = rc
        .GetType()
        .GetMethods()
        //TODO: fix this
        .Last(m => m.Name == "Register");

      rc.ChannelOptions.MaxRetryAttempts = 3;

      foreach (var cmd in commands) {
        var args = cmd
          .GetInterface("IRpcCommand`1")?
          .GetGenericArguments()!;

        registerMethod
          .MakeGenericMethod([cmd, args[0]])
          .Invoke(rc, []);
      };
    });

    return app;
  }
}
