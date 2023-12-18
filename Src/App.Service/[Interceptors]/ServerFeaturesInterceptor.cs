using System.Globalization;
using System.Text.Json;

using Grpc.Core;
using Grpc.Core.Interceptors;

// ReSharper disable once CheckNamespace
namespace Yaver.App;

/// <summary>
///   Interceptor for setting server features before continuing with the request.
/// </summary>
public class ServerFeaturesInterceptor : Interceptor {
  /// <summary>
  ///   Overrides the default server streaming server handler to set server features before continuing with the request.
  /// </summary>
  /// <typeparam name="TRequest">The type of the request message.</typeparam>
  /// <typeparam name="TResponse">The type of the response message.</typeparam>
  /// <param name="request">The request message.</param>
  /// <param name="responseStream">The response stream.</param>
  /// <param name="serverCallContext">The server call context.</param>
  /// <param name="continuation">The continuation function.</param>
  /// <returns>A task representing the asynchronous operation.</returns>
  public override async Task ServerStreamingServerHandler<TRequest, TResponse>(
    TRequest request,
    IServerStreamWriter<TResponse> responseStream,
    ServerCallContext serverCallContext,
    ServerStreamingServerMethod<TRequest, TResponse> continuation
  ) {
    SetFeatures(serverCallContext);

    await continuation(request, responseStream, serverCallContext);
  }

  /// <summary>
  ///   Intercepts unary server calls and sets the http context features before continuing the pipeline execution.
  /// </summary>
  public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(
    TRequest request,
    ServerCallContext serverCallContext,
    UnaryServerMethod<TRequest, TResponse> continuation
  ) {
    // Get the http context from the server call context
    SetFeatures(serverCallContext);

    // Continue the execution of the pipeline
    return await continuation(request, serverCallContext);
  }

  private static void SetFeatures(ServerCallContext context) {
    var httpContext = context.GetHttpContext();

    // Console.WriteLine(JsonSerializer.Serialize(httpContext.Request.Headers, new JsonSerializerOptions { WriteIndented = true }));

    var yaverContextJson = httpContext.Request.Headers["x-yaver-context"];
    // if (!string.IsNullOrEmpty(yaverContextJson)) {
    var yaverContext = JsonSerializer.Deserialize<RequestInfo>(yaverContextJson);

    // Console.WriteLine(JsonSerializer.Serialize(yaverContext, new JsonSerializerOptions { WriteIndented = true }));

    httpContext.Features.Set(yaverContext);
    // httpContext.Features.Set(httpContext.Request.Headers["Accept-Language"]);

    var cultureKey = yaverContext.AcceptLanguage; //  httpContext.Request.Headers["Accept-Language"];
    if (!string.IsNullOrEmpty(cultureKey)) {
      if (DoesCultureExist(cultureKey)) {
        var culture = new CultureInfo(cultureKey);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
      }
    }

    var tenantContextJson = httpContext.Request.Headers["x-tenant-context"];
    if (string.IsNullOrWhiteSpace(tenantContextJson)) return;
    var tenantContext = JsonSerializer.Deserialize<TenantInfo>(tenantContextJson);
    httpContext.Features.Set(tenantContext);
  }

  private static bool DoesCultureExist(string cultureName) {
    return CultureInfo.GetCultures(CultureTypes.AllCultures)
      .Any(culture => string.Equals(culture.Name, cultureName,
        StringComparison.CurrentCultureIgnoreCase));
  }
}
