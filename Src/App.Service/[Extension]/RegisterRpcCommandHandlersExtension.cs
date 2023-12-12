using Microsoft.AspNetCore.Builder;

// ReSharper disable once CheckNamespace
namespace Yaver.App;

/// <summary>
///   Provides extension methods for registering RPC command handlers in a minimal API web application.
/// </summary>
public static class RegisterRpcCommandHandlersExtension {
  /// <summary>
  ///   Represents a minimal API web application.
  /// </summary>
  public static WebApplication RegisterRpcCommandHandlers(
    this WebApplication app
  ) {
    var handlers = AppDomain.CurrentDomain
      .GetAssemblies()
      .SelectMany(s => s.GetTypes())
      .Where(t => t.GetInterfaces()
        .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRpcCommandHandler<,>)))
      .Where(t => t is { IsInterface: false, IsAbstract: false });


    app.MapHandlers(ho => {
      var registerMethod = ho
        .GetType()
        .GetMethods()
        //TODO: fix this
        .Last(m => m.Name == "Register");

      foreach (var handler in handlers) {
        var args = handler
          .GetInterface("IRpcCommandHandler`2")?
          .GetGenericArguments()!;

        registerMethod
          .MakeGenericMethod([args[0], handler, args[1]])
          .Invoke(ho, []);
      }

      ;
    });

    return app;
  }
}
