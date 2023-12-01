using Microsoft.Extensions.Hosting;

namespace Yaver.App;


/// <summary>
/// Provides extension methods for mapping RPC handlers to a host.
/// </summary>
public static class MapRpcHandlersExtensions {
  /// <summary>
  /// Maps RPC handlers to the specified host and service URL.
  /// </summary>
  /// <param name="host">The host to map the RPC handlers to.</param>
  /// <param name="serviceName">The name of the service assembly.</param>
  /// <param name="serviceUrl">The URL of the service.</param>
  /// <returns>The updated host with the mapped RPC handlers.</returns>
  public static IHost MapRpcHandlers(
    this IHost host,
    string serviceName,
    string serviceUrl
  ) {
    ArgumentNullException.ThrowIfNull(serviceUrl);

    ArgumentNullException.ThrowIfNull(serviceName);

    var commands = AppDomain.CurrentDomain
      .GetAssemblies().First(predicate: x => x.GetName().Name == serviceName)
      .GetTypes()
      .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRpcCommand<>)))
      .Where(t => !t.IsInterface && !t.IsAbstract);

    host.MapRemote(remoteAddress: serviceUrl, r: rc => {
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
      }
    });

    return host;
  }
}
