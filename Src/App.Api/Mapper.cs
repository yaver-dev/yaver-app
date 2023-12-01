// using Microsoft.Extensions.DependencyInjection;
// using Conf = FastEndpoints.Config;

namespace Yaver.App;

/// <summary>
///   Base class for defining domain entity mappers for your endpoints.
///   <para>HINT: entity mappers are used as singletons for performance reasons. Do not maintain state in the mappers.</para>
/// </summary>
/// <typeparam name="TRequest">The type of request DTO.</typeparam>
/// <typeparam name="TResponse">The type of response DTO.</typeparam>
/// <typeparam name="TCommand">The type of domain entity to map to/from.</typeparam>
/// <typeparam name="TResult">The type of result object to convert.</typeparam>
public abstract class YaverMapper<TRequest, TResponse, TCommand, TResult> : IMapper {
  //, IServiceResolverBase where TRequest : notnull where TResponse : notnull {

  /// <summary>
  ///   Converts a request object to a command object.
  /// </summary>
  /// <param name="r">The request object to convert.</param>
  /// <returns>The command object.</returns>
  public virtual TCommand ToCommand(TRequest r) {
    throw new NotImplementedException($"Please override the {nameof(ToCommand)} method!");
  }

  /// <summary>
  ///   Converts the given <typeparamref name="TResult" /> object to a <typeparamref name="TResponse" /> object.
  /// </summary>
  /// <param name="r">The <typeparamref name="TResult" /> object to convert.</param>
  /// <returns>The converted <typeparamref name="TResponse" /> object.</returns>
  public virtual TResponse ToResponse(TResult r) {
    throw new NotImplementedException($"Please override the {nameof(ToResponse)} method!");
  }

  // /// <summary>
  // /// override this method and place the logic for mapping the request dto to the desired domain entity
  // /// </summary>
  // /// <param name="r">the request dto</param>
  // public virtual TCommand ToEntity(TRequest r) => throw new NotImplementedException($"Please override the {nameof(ToEntity)} method!");
  // /// <summary>
  // /// override this method and place the logic for mapping the request dto to the desired domain entity
  // /// </summary>
  // /// <param name="r">the request dto to map from</param>
  // /// <param name="ct">a cancellation token</param>
  // public virtual Task<TCommand> ToEntityAsync(TRequest r, CancellationToken ct = default) => throw new NotImplementedException($"Please override the {nameof(ToEntityAsync)} method!");

  // /// <summary>
  // /// override this method and place the logic for mapping a domain entity to a response dto
  // /// </summary>
  // /// <param name="e">the domain entity to map from</param>
  // public virtual TResponse FromEntity(TCommand e) => throw new NotImplementedException($"Please override the {nameof(FromEntity)} method!");
  // /// <summary>
  // /// override this method and place the logic for mapping a domain entity to a response dto
  // /// </summary>
  // /// <param name="e">the domain entity to map from</param>
  // /// <param name="ct">a cancellation token</param>
  // public virtual Task<TResponse> FromEntityAsync(TCommand e, CancellationToken ct = default) => throw new NotImplementedException($"Please override the {nameof(FromEntityAsync)} method!");

  // /// <summary>
  // /// override this method and place the logic for mapping the updated request dto to the desired domain entity
  // /// </summary>
  // /// <param name="r">the request dto to update from</param>
  // /// <param name="e">the domain entity to update</param>
  // public virtual TEntity UpdateEntity(TRequest r, TEntity e) => throw new NotImplementedException($"Please override the {nameof(UpdateEntity)} method!");


  // ///<inheritdoc/>
  // public TService? TryResolve<TService>() where TService : class => Conf.ServiceResolver.TryResolve<TService>();
  // ///<inheritdoc/>
  // public object? TryResolve(Type typeOfService) => Conf.ServiceResolver.TryResolve(typeOfService);
  // ///<inheritdoc/>
  // public TService Resolve<TService>() where TService : class => Conf.ServiceResolver.Resolve<TService>();
  // ///<inheritdoc/>
  // public object Resolve(Type typeOfService) => Conf.ServiceResolver.Resolve(typeOfService);
  // ///<inheritdoc/>
  // public IServiceScope CreateScope() => Conf.ServiceResolver.CreateScope();
}
