using Grpc.Core;
using Grpc.Core.Interceptors;

using Microsoft.Extensions.Logging;

namespace Yaver.App;

/// <summary>
/// Interceptor for logging gRPC server calls and any exceptions thrown by the handler.
/// </summary>
public class ServerLogInterceptor(ILogger<ServerLogInterceptor> logger) : Interceptor {
  private readonly ILogger<ServerLogInterceptor> _logger = logger;

  /// <summary>
  /// Intercepts unary server calls and logs the call details and any exceptions thrown by the handler.
  /// </summary>
  /// <typeparam name="TRequest">The type of the request message.</typeparam>
  /// <typeparam name="TResponse">The type of the response message.</typeparam>
  /// <param name="request">The request message.</param>
  /// <param name="context">The server call context.</param>
  /// <param name="continuation">The continuation method to invoke.</param>
  /// <returns>The response message.</returns>
  public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
    TRequest request,
    ServerCallContext context,
    UnaryServerMethod<TRequest, TResponse> continuation
  ) {
    LogCall<TRequest, TResponse>(MethodType.Unary, context);

    try {
      return await continuation(request, context);
    }
    catch (Exception ex) {
      // Note: The gRPC framework also logs exceptions thrown by handlers to .NET Core logging.
      _logger.LogError(ex,
        "Error thrown by {err}.", context.Method);

      throw;
    }
  }

  /// <summary>
  /// Overrides the base method to intercept client streaming server calls and log the request and response.
  /// </summary>
  /// <typeparam name="TRequest">The type of the request message.</typeparam>
  /// <typeparam name="TResponse">The type of the response message.</typeparam>
  /// <param name="requestStream">The asynchronous stream of client requests.</param>
  /// <param name="context">The server call context.</param>
  /// <param name="continuation">The server method continuation to invoke.</param>
  /// <returns>A task that represents the asynchronous operation. The task result contains the response message.</returns>
  public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
    IAsyncStreamReader<TRequest> requestStream,
    ServerCallContext context,
    ClientStreamingServerMethod<TRequest, TResponse> continuation
  ) {
    LogCall<TRequest, TResponse>(MethodType.ClientStreaming, context);
    return base.ClientStreamingServerHandler(requestStream, context, continuation);
  }

  /// <summary>
  /// Overrides the default server streaming server handler to log the server call and then call the base implementation.
  /// </summary>
  public override Task ServerStreamingServerHandler<TRequest, TResponse>(
    TRequest request,
    IServerStreamWriter<TResponse> responseStream,
    ServerCallContext context,
    ServerStreamingServerMethod<TRequest, TResponse> continuation
  ) {
    LogCall<TRequest, TResponse>(MethodType.ServerStreaming, context);
    return base.ServerStreamingServerHandler(request, responseStream, context, continuation);
  }

  /// <summary>
  /// Overrides the base method to log the call and then invoke the base method.
  /// </summary>
  /// <typeparam name="TRequest">The type of the request message.</typeparam>
  /// <typeparam name="TResponse">The type of the response message.</typeparam>
  /// <param name="requestStream">The stream of incoming request messages.</param>
  /// <param name="responseStream">The stream of outgoing response messages.</param>
  /// <param name="context">The server call context.</param>
  /// <param name="continuation">The continuation delegate to invoke.</param>
  /// <returns>A task that represents the asynchronous operation.</returns>
  public override Task DuplexStreamingServerHandler<TRequest, TResponse>(
    IAsyncStreamReader<TRequest> requestStream,
    IServerStreamWriter<TResponse> responseStream,
    ServerCallContext context,
    DuplexStreamingServerMethod<TRequest, TResponse> continuation
  ) {
    LogCall<TRequest, TResponse>(MethodType.DuplexStreaming, context);
    return base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
  }

  private void LogCall<TRequest, TResponse>(MethodType methodType, ServerCallContext context)
    where TRequest : class
    where TResponse : class {
    _logger.LogInformation("Starting call. Type: {methodType}. Request: {req}. Response: {res}",
      methodType,
      typeof(TRequest),
      typeof(TResponse));
  }
}
