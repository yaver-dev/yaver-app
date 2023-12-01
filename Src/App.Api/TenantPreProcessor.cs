using FluentValidation.Results;

using Microsoft.AspNetCore.Http;

namespace Yaver.App;
/// <summary>
/// Represents a pre-processor that handles tenant-specific operations before processing the request.
/// </summary>
public class TenantPreProcessor : IGlobalPreProcessor
{
  /// <summary>
  /// Represents an asynchronous operation that performs pre-processing tasks.
  /// </summary>
  /// <param name="ctx">The pre-processor context.</param>
  /// <param name="ct">The cancellation token.</param>
  /// <returns>A task that represents the asynchronous operation.</returns>
  public Task PreProcessAsync(IPreProcessorContext ctx, CancellationToken ct)
  {
    var tenantId = ctx.HttpContext.Request.Headers["X-tenant-id"].FirstOrDefault();

    if (tenantId == null)
    {
      ctx.ValidationFailures.Add(new("MissingHeaders", "The [x-tenant-id] header needs to be set!"));
      return ctx.HttpContext.Response.SendErrorsAsync(ctx.ValidationFailures, cancellation: ct);
    }

    if (tenantId != "qwerty")
      return ctx.HttpContext.Response.SendForbiddenAsync(cancellation: ct);

    return Task.CompletedTask;
  }
}
