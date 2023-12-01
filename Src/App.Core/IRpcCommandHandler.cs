using FastEndpoints;

namespace Yaver.App;

/// <summary>
///   Represents a handler for a command that returns a result, used in RPC scenarios.
/// </summary>
/// <typeparam name="TCommand">The type of command to handle.</typeparam>
/// <typeparam name="TResult">The type of result returned by the command.</typeparam>
public interface IRpcCommandHandler<TCommand, TResult>
  where TCommand : notnull, ICommand<TResult>
  where TResult : notnull {
}

/// <summary>
///   Represents an RPC command that returns a result of type <typeparamref name="TResult" />.
/// </summary>
/// <typeparam name="TResult">The type of the result returned by the command.</typeparam>
public interface IRpcCommand<out TResult> : ICommand<TResult>
  where TResult : notnull {
}
