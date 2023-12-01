using FluentValidation.Results;

using Microsoft.AspNetCore.Http;

namespace Yaver.App;

/// <summary>
///   Provides extension methods to convert a <see cref="Result{T}" /> object to an HTTP response.
/// </summary>
public static class ResultToResponse {

  /// <summary>
  /// Converts a <see cref="Result{T}"/> object to an <see cref="Microsoft.AspNetCore.Http.IResult"/> object based on the result status.
  /// </summary>
  /// <typeparam name="T">The type of the result value.</typeparam>
  /// <param name="result">The <see cref="Result{T}"/> object to convert.</param>
  /// <returns>An <see cref="Microsoft.AspNetCore.Http.IResult"/> object representing the converted result.</returns>
  public static Microsoft.AspNetCore.Http.IResult ToHttpResponse<T>(this Result<T> result) {
    return result.Status switch {
      ResultStatus.NotFound => NotFoundEntity(result),
      ResultStatus.Unauthorized => Results.Unauthorized(),
      ResultStatus.Forbidden => Results.Forbid(),
      ResultStatus.Invalid => Results.BadRequest(result.ValidationErrors),
      ResultStatus.Error => UnprocessableEntity(result),
      ResultStatus.Conflict => ConflictEntity(result),
      ResultStatus.CriticalError => CriticalEntity(result),
      ResultStatus.Unavailable => UnavailableEntity(result),
      _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported.")
    };
  }

  /// <summary>
  /// Represents a problem details object that provides additional information about an error or problem.
  /// </summary>
  private static ProblemDetails NotFoundEntity(IResult result) => new(
      result.Errors.Select(e => new ValidationFailure("", e)).ToList(),
      StatusCodes.Status404NotFound);


  /// <summary>
  /// Represents a problem details object that provides additional information about an error.
  /// </summary>
  private static ProblemDetails UnprocessableEntity(IResult result) => new(
      result.Errors.Select(e => new ValidationFailure("", e)).ToList(),
      StatusCodes.Status422UnprocessableEntity);

  /// <summary>
  /// Represents a problem details object that provides additional information about an error.
  /// </summary>
  private static ProblemDetails ConflictEntity(IResult result) => new(
      result.Errors.Select(e => new ValidationFailure("", e)).ToList(),
      StatusCodes.Status409Conflict);

  /// <summary>
  /// Represents a problem details object that provides additional information about an error.
  /// </summary>
  private static ProblemDetails CriticalEntity(IResult result) => new(
      result.Errors.Select(e => new ValidationFailure("", e)).ToList(),
      StatusCodes.Status500InternalServerError);

  /// <summary>
  /// Represents a problem details object that provides additional information about an error.
  /// </summary>
  private static ProblemDetails UnavailableEntity(IResult result) => new(
      result.Errors.Select(e => new ValidationFailure("", e)).ToList(),
      StatusCodes.Status503ServiceUnavailable);
}
