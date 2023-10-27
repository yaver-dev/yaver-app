using Yaver.App;

public static class MainExtensions {
  //
  // Summary:
  //     adds the FastEndpoints services to the ASP.Net middleware pipeline
  //
  // Parameters:
  //   options:
  //     optionally specify the endpoint discovery options
  // public static IServiceCollection AddRpcCommandHandlers(this IServiceCollection services) {
  //   var hndlrs = AppDomain.CurrentDomain.GetAssemblies()
  //        .SelectMany(s => s.GetTypes())
  //        .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)))
  //        .Where(t => !t.IsInterface && !t.IsAbstract);

  //   // var args = h.GetInterface("IRpcCommandHandler`3")?.GetGenericArguments();
  //   handlers.AddRange(hndlrs.Select(h => {
  //     var args = h.GetInterface("ICommandHandler`2")?.GetGenericArguments()!;
  //     return new Handler(command: args[0], result: args[1]);
  //     // return new Handler(command: args[0], handler: args[1], result: args[2]);
  //   }));

  //   return services;
  // }


  // public static WebApplication MapRpcCommandHandlers(this WebApplication app) {
  //   var hndlrs = AppDomain.CurrentDomain.GetAssemblies()
  //           .SelectMany(s => s.GetTypes())
  //           .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<,>)))
  //           .Where(t => !t.IsInterface && !t.IsAbstract);

  //   app.MapHandlers(h => {
  //     foreach (var handler in hndlrs) {
  //       var args = handler.GetInterface("ICommandHandler`2")?.GetGenericArguments()!;
  //       var r = h
  //         .GetType()
  //         .GetMethod("Register", [args[0], handler, args[1]])?
  //         .Invoke(h, []);
  //       Console.WriteLine($"registered {handler.ToString()}  {args[0].ToString()}  {args[1].ToString()}");
  //       Console.WriteLine($"{r.ToString()}");
  //     };
  //   });
  //   return app;
  // }

  private record Handler(Type command, Type result);
  // private record Handler(Type command, Type handler, Type result);
  private static List<Handler> handlers = new();
}
