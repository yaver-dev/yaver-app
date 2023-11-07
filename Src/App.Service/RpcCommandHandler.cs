using FastEndpoints;

namespace Yaver.App;
/// <summary>
/// Base class for RPC command handlers that implement both ICommandHandler and IRpcCommandHandler interfaces.
/// </summary>
/// <typeparam name="TCommand">The type of command to handle.</typeparam>
/// <typeparam name="TResult">The type of result to return.</typeparam>
/// <remarks>
/// This class provides a default implementation for ICommandHandler.ExecuteAsync method and also defines an abstract method
/// ExecuteAsync that must be implemented by derived classes.
/// </remarks>
public abstract class RpcCommandHandler<TCommand, TResult>
  : ICommandHandler<TCommand, TResult>, IRpcCommandHandler<TCommand, TResult>
  where TCommand : notnull, ICommand<TResult>
  where TResult : notnull {
  // Microsoft.Extensions.DependencyInjection.ServiceProvider serviceProvider;
  // readonly IServiceProvider _serviceProvider;
  /// <summary>
  /// Represents a handler for RPC commands.
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


  /// <inheritdoc />
  // TMapper? _mapper;

  // /// <summary>
  // /// the entity mapper for the endpoint
  // /// <para>HINT: entity mappers are singletons for performance reasons. do not maintain state in the mappers.</para>
  // /// </summary>
  // [DontInject]
  // //access is public to support testing
  // public TMapper Map {
  //   get => _mapper = _serviceProvider.GetRequiredService<TMapper>();// ??= (TMapper)Definition.GetMapper()!;
  //   set => _mapper = value; //allow unit tests to set mapper from outside
  // }
  /// <inheritdoc />
  public abstract Task<TResult> ExecuteAsync(TCommand command, CancellationToken ct = default);

}
