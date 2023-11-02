using FluentValidation.Results;

using Microsoft.AspNetCore.Http;

namespace Yaver.App;

/// <summary>
/// Represents a pre-processor that validates the tenant ID header in the HTTP request.
/// </summary>
public class TenantPreProcessor : IGlobalPreProcessor {
  /// <summary>
  /// Pre-processes the incoming request by checking the X-tenant-id header and validating its value.
  /// If the header is missing or the value is not valid, it sends an appropriate response.
  /// </summary>
  /// <param name="req">The incoming request object.</param>
  /// <param name="ctx">The current HTTP context.</param>
  /// <param name="failures">A list of validation failures to add to if the header is missing or the value is not valid.</param>
  /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
  /// <returns>A task that represents the asynchronous operation.</returns>
  public Task PreProcessAsync(
    object req,
    HttpContext ctx,
    List<ValidationFailure> failures,
    CancellationToken ct) {

    var tenantID = ctx.Request.Headers["X-tenant-id"].FirstOrDefault();
    // ctx.Features.Set("tenant-id", tenantID);

    if (tenantID == null) {
      failures.Add(new("MissingHeaders", "The [x-tenant-id] header needs to be set!"));
      return ctx.Response.SendErrorsAsync(failures, cancellation: ct); //sending response here
    }

    if (tenantID != "qwerty")
      return ctx.Response.SendForbiddenAsync(cancellation: ct); //sending response here

    return Task.CompletedTask;
  }
}
