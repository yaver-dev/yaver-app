using FastEndpoints;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Yaver.App;
public abstract partial class RpcCommandHandler<TCommand, TResult, TMapper>
  : ICommandHandler<TCommand, TResult>, IHasMapper<TMapper>, IRpcCommandHandler<TCommand, TResult, TMapper>
  where TCommand : notnull, ICommand<TResult>
  where TResult : notnull
  where TMapper : notnull, IMapper {
  // Microsoft.Extensions.DependencyInjection.ServiceProvider serviceProvider;
  readonly IServiceProvider _serviceProvider;
  public RpcCommandHandler(IServiceProvider serviceProvider) {
    _serviceProvider = serviceProvider;

    // Console.WriteLine($"------------------------------------------------------");

    // var services = serviceProvider.GetServices<TMapper>();
    // foreach (var service in services) {
    //   Console.WriteLine($"registered {service.ToString()}");
    // }
    // Console.WriteLine($"------------------------------------------------------");
    // var mapper = serviceProvider.GetRequiredService<TMapper>();
    // Console.WriteLine(mapper.ToString());
    // Console.WriteLine($"------------------------------------------------------");
  }

  /// <inheritdoc />
  TMapper? _mapper;

  /// <summary>
  /// the entity mapper for the endpoint
  /// <para>HINT: entity mappers are singletons for performance reasons. do not maintain state in the mappers.</para>
  /// </summary>
  [DontInject]
  //access is public to support testing
  public TMapper Map {
    get => _mapper = _serviceProvider.GetRequiredService<TMapper>();// ??= (TMapper)Definition.GetMapper()!;
    set => _mapper = value; //allow unit tests to set mapper from outside
  }
  /// <inheritdoc />
  public abstract Task<TResult> ExecuteAsync(TCommand command, CancellationToken ct = default);

}

public interface IRpcCommandHandler<TCommand, TResult, TMapper>
  where TCommand : notnull, ICommand<TResult>
  where TResult : notnull
  where TMapper : notnull, IMapper {
}

