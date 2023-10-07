using Microsoft.AspNetCore.Diagnostics;

using System.Net;

using Microsoft.AspNetCore.Builder;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;


namespace Yaver.App;

internal class ExceptionHandler {
}

/// <summary>
/// Represents an HTTP API response.
/// </summary>
public record ApiResponse(int StatusCode, string Message, object? Errors = null);

/// <summary>
/// extensions for global exception handling
/// </summary>
public static class ExceptionHandlerExtensions {
  /// <summary>
  /// registers the default global exception handler which will log the exceptions on the server and return a user-friendly json response to the client when unhandled exceptions occur.
  /// TIP: when using this exception handler, you may want to turn off the asp.net core exception middleware logging to avoid duplication like so:
  /// <code>
  /// "Logging": { "LogLevel": { "Default": "Warning", "Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddleware": "None" } }
  /// </code>
  /// </summary>
  /// <param name="logger">an optional logger instance</param>
  /// <param name="logStructuredException">set to true if you'd like to log the error in a structured manner</param>
  /// <summary>
  /// Registers the default global exception handler which will log the exceptions on the server and return a user-friendly json response to the client when unhandled exceptions occur.
  /// </summary>
  /// <param name="app">The <see cref="IApplicationBuilder"/> instance.</param>
  /// <param name="logger">An optional logger instance.</param>
  /// <param name="logStructuredException">Set to true if you'd like to log the error in a structured manner.</param>
  public static IApplicationBuilder UseYaverExceptionHandler(this IApplicationBuilder app, ILogger? logger = null,
          bool logStructuredException = false) {
    app.UseExceptionHandler(errApp => {
      errApp.Run(async ctx => {
        var exHandlerFeature = ctx.Features.Get<IExceptionHandlerFeature>();
        if (exHandlerFeature is not null) {
          logger ??= ctx.Resolve<ILogger<ExceptionHandler>>();
          var err = exHandlerFeature.Error;
          var http = exHandlerFeature.Endpoint?.DisplayName?.Split(" => ")[0];
          var type = exHandlerFeature.Error.GetType().Name;
          var error = exHandlerFeature.Error.Message;

          var msg =
                              $@"=================================
                        {http}
                        TYPE: {type}
                        REASON: {error}
                        ---------------------------------
                        {exHandlerFeature.Error.StackTrace}";

          if (logStructuredException)
            logger.LogError("{@http}{@type}{@reason}{@exception}", http, type, error, exHandlerFeature.Error);
          else
            logger.LogError(msg);

          if (exHandlerFeature.Error.GetType().Name == nameof(RpcException)) {
            var rex = (RpcException)err;
            var status = rex.Status;
            var meta = rex.Trailers;

            var code = rex.StatusCode switch {
              StatusCode.NotFound => (int)HttpStatusCode.NotFound,
              StatusCode.InvalidArgument => (int)HttpStatusCode.BadRequest,
              StatusCode.AlreadyExists => (int)HttpStatusCode.Conflict,
              StatusCode.PermissionDenied => (int)HttpStatusCode.Forbidden,
              StatusCode.Unauthenticated => (int)HttpStatusCode.Unauthorized,
              StatusCode.Unavailable => (int)HttpStatusCode.ServiceUnavailable,
              StatusCode.ResourceExhausted => (int)HttpStatusCode.TooManyRequests,
              StatusCode.FailedPrecondition => (int)HttpStatusCode.PreconditionFailed,
              StatusCode.Aborted => (int)HttpStatusCode.Conflict,
              StatusCode.Cancelled => (int)HttpStatusCode.BadRequest,
              StatusCode.DeadlineExceeded => (int)HttpStatusCode.GatewayTimeout,
              StatusCode.DataLoss => (int)HttpStatusCode.InternalServerError,
              StatusCode.Unknown => (int)HttpStatusCode.InternalServerError,
              StatusCode.OK => (int)HttpStatusCode.OK,
              StatusCode.Unimplemented => (int)HttpStatusCode.NotImplemented,
              _ => (int)HttpStatusCode.InternalServerError
            };

            var res = new Dictionary<string, object> { { "statusCode", code }, { "message", status.Detail } };
            // new {StatusCode = code, Message = status.Detail};

            var note = meta.GetValue("extra");

            if (!string.IsNullOrWhiteSpace(note)) {
              res.Add("note", note);
            }

            ctx.Response.StatusCode = code;
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsJsonAsync(res);
          } else {
            ctx.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            ctx.Response.ContentType = "application/json";
            await ctx.Response.WriteAsJsonAsync(new InternalErrorResponse {
              Status = "Internal Server Error!",
              Code = ctx.Response.StatusCode,
              Reason = error,
              Note = "See application log for stack trace."
            });
          }
        }
      });
    });

    return app;
  }
}
