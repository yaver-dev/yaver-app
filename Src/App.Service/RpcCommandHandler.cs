using FastEndpoints;

using FluentValidation;
using FluentValidation.Results;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Yaver.App;
public abstract class RpcCommandHandler<TCommand, TResult>
  : ICommandHandler<TCommand, TResult>, IRpcCommandHandler<TCommand, TResult>
  where TCommand : notnull, ICommand<TResult>
  where TResult : notnull {
  // Microsoft.Extensions.DependencyInjection.ServiceProvider serviceProvider;
  // readonly IServiceProvider _serviceProvider;
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

public interface IRpcCommandHandler<TCommand, TResult>
  where TCommand : notnull, ICommand<TResult>
  where TResult : notnull {
}

