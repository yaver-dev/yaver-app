namespace Yaver.App;

/// <summary>
///   Base class for RPC command handlers that implement both ICommandHandler and IRpcCommandHandler interfaces.
/// </summary>
/// <typeparam name="TCommand">The type of command to handle.</typeparam>
/// <typeparam name="TResult">The type of result to return.</typeparam>
/// <remarks>
///   This class provides a default implementation for ICommandHandler.ExecuteAsync method and also defines an abstract
///   method
///   ExecuteAsync that must be implemented by derived classes.
/// </remarks>
public abstract class RpcCommandHandler<TCommand, TResult>
  : ICommandHandler<TCommand, TResult>, IRpcCommandHandler<TCommand, TResult>
  where TCommand : notnull, ICommand<TResult>
  where TResult : notnull {
  // Microsoft.Extensions.DependencyInjection.ServiceProvider serviceProvider;
  // readonly IServiceProvider _serviceProvider;
  /// <summary>
  ///   Represents a handler for RPC commands.
  /// </summary>
  public RpcCommandHandler() {
    // get current http context
    // var context = serviceProvider.GetRequiredService<IHttpContextAccessor>().HttpContext;
  }


  //get context for services
  // var context = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider;


  // var serviceProvider = context.RequestServices;


  // var instance = serviceProvider.GetRequiredService<IValidator<TCommand>>();

  // async Task<ValidationResult> ValidateAsync(TCommand command, CancellationToken ct = default(CancellationToken)) {
  //   var validationResult = await instance.ValidateAsync(command, ct);

  // }

  // IValidator<CreateDatabaseServerCommand> validator
  // _serviceProvider = serviceProvider;

  // Console.WriteLine($"------------------------------------------------------");

  // var services = serviceProvider.GetServices<TMapper>();
  // foreach (var service in services) {
  //   Console.WriteLine($"registered {service.ToString()}");
  // }
  // Console.WriteLine($"------------------------------------------------------");
  // var mapper = serviceProvider.GetRequiredService<TMapper>();
  // Console.WriteLine(mapper.ToString());
  // Console.WriteLine($"------------------------------------------------------");



  /// <summary>
  /// Executes the specified command asynchronously.
  /// </summary>
  /// <typeparam name="TResult">The type of the result.</typeparam>
  /// <param name="command">The command to execute.</param>
  /// <param name="ct">The cancellation token.</param>
  /// <returns>A task representing the asynchronous operation, which returns the result of the execution.</returns>
  public abstract Task<TResult> ExecuteAsync(TCommand command, CancellationToken ct = default);
}
