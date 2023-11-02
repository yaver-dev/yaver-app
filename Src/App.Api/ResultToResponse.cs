using FluentValidation.Results;

using Microsoft.AspNetCore.Http;

using Yaver.App.Result;
namespace Yaver.App;
/// <summary>
/// Provides extension methods for converting a <see cref="Result{T}"/> object to an <see cref="Microsoft.AspNetCore.Http.IResult"/> object.
/// </summary>
public static class ResultToResponse {
  /// <summary>
  /// Converts a <see cref="Result{T}"/> object to an <see cref="Microsoft.AspNetCore.Http.IResult"/> object.
  /// </summary>
  /// <typeparam name="T">The type of the result.</typeparam>
  /// <param name="result">The <see cref="Result{T}"/> object to convert.</param>
  /// <returns>An <see cref="Microsoft.AspNetCore.Http.IResult"/> object representing the result.</returns>


  public static Microsoft.AspNetCore.Http.IResult ToHttpResponse<T>(this Result<T> result) => result.Status switch {
    ResultStatus.NotFound => NotFoundEntity(result),
    ResultStatus.Unauthorized => Results.Unauthorized(),
    ResultStatus.Forbidden => Results.Forbid(),
    ResultStatus.Invalid => Results.BadRequest(result.ValidationErrors),
    ResultStatus.Error => UnprocessableEntity(result),
    ResultStatus.Conflict => ConflictEntity(result),
    ResultStatus.CriticalError => CriticalEntity(result),
    ResultStatus.Unavailable => UnavailableEntity(result),
    _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported."),
  };


  /// <summary>
  /// Returns a 404 Not Found response with the specified validation errors.
  /// </summary>
  /// <param name="result">The result containing the validation errors.</param>
  private static Microsoft.AspNetCore.Http.IResult NotFoundEntity(Result.IResult result) =>
    new ProblemDetails(
      result.Errors.Select(e => new ValidationFailure("", e)).ToList(),
      StatusCodes.Status404NotFound);

  /// <summary>
  /// Returns an HTTP 422 Unprocessable Entity result with the validation errors from the specified <paramref name="result"/>.
  /// </summary>
  /// <param name="result">The result containing the validation errors.</param>
  /// <returns>An HTTP 422 Unprocessable Entity result with the validation errors.</returns>
  private static Microsoft.AspNetCore.Http.IResult UnprocessableEntity(Result.IResult result) =>
    new ProblemDetails(
      result.Errors.Select(e => new ValidationFailure("", e)).ToList(),
      StatusCodes.Status422UnprocessableEntity);

  /// <summary>
  /// Returns a 409 Conflict status code with the validation errors as a ProblemDetails object.
  /// </summary>
  /// <param name="result">The result object containing the validation errors.</param>
  /// <returns>A ProblemDetails object representing the validation errors with a 409 Conflict status code.</returns>
  private static Microsoft.AspNetCore.Http.IResult ConflictEntity(Result.IResult result) =>
    new ProblemDetails(
      result.Errors.Select(e => new ValidationFailure("", e)).ToList(),
      StatusCodes.Status409Conflict);

  /// <summary>
  /// Converts a <see cref="Result.IResult"/> object to an <see cref="Microsoft.AspNetCore.Http.IResult"/> object with a 500 Internal Server Error status code and a <see cref="ProblemDetails"/> object containing the validation failures.
  /// </summary>
  /// <param name="result">The <see cref="Result.IResult"/> object to convert.</param>
  /// <returns>An <see cref="Microsoft.AspNetCore.Http.IResult"/> object with a 500 Internal Server Error status code and a <see cref="ProblemDetails"/> object containing the validation failures.</returns>
  private static Microsoft.AspNetCore.Http.IResult CriticalEntity(Result.IResult result) =>
    new ProblemDetails(
      result.Errors.Select(e => new ValidationFailure("", e)).ToList(),
      StatusCodes.Status500InternalServerError);

  /// <summary>
  /// Returns a <see cref="Microsoft.AspNetCore.Http.IResult"/> representing a 503 Service Unavailable status code and a <see cref="ProblemDetails"/> object containing the specified <paramref name="result"/>'s errors as <see cref="ValidationFailure"/> objects.
  /// </summary>
  /// <param name="result">The <see cref="Result.IResult"/> object containing the errors to include in the response.</param>
  /// <returns>A <see cref="Microsoft.AspNetCore.Http.IResult"/> representing a 503 Service Unavailable status code and a <see cref="ProblemDetails"/> object containing the specified <paramref name="result"/>'s errors as <see cref="ValidationFailure"/> objects.</returns>
  private static Microsoft.AspNetCore.Http.IResult UnavailableEntity(Result.IResult result) =>
    new ProblemDetails(
      result.Errors.Select(e => new ValidationFailure("", e)).ToList(),
      StatusCodes.Status503ServiceUnavailable);

}
