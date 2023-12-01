using FluentValidation.Results;

using Microsoft.AspNetCore.Http;

namespace Yaver.App;

public static class ResultToResponse {
  /// <summary>
  ///   Converts a <see cref="Result{T}" /> object to an HTTP response.
  /// </summary>
  /// <typeparam name="T">The type of the result value.</typeparam>
  /// <param name="result">The result object to convert.</param>
  /// <returns>An <see cref="Microsoft.AspNetCore.Http.IResult" /> representing the converted HTTP response.</returns>
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
  ///   Creates a <see cref="Microsoft.AspNetCore.Http.IResult" /> representing a not found entity.
  /// </summary>
  /// <param name="result">The result containing the errors.</param>
  /// <returns>A <see cref="Microsoft.AspNetCore.Http.IResult" /> representing a not found entity.</returns>
  private static Microsoft.AspNetCore.Http.IResult NotFoundEntity(IResult result) {
    return new ProblemDetails(
      result.Errors.Select(e => new ValidationFailure("", e)).ToList(),
      StatusCodes.Status404NotFound);
  }

  /// <summary>
  ///   Converts the given <see cref="IResult" /> to an HTTP 422 Unprocessable Entity response.
  /// </summary>
  /// <param name="result">The result to convert.</param>
  /// <returns>An HTTP 422 Unprocessable Entity response.</returns>
  private static Microsoft.AspNetCore.Http.IResult UnprocessableEntity(IResult result) {
    return new ProblemDetails(
      result.Errors.Select(e => new ValidationFailure("", e)).ToList(),
      StatusCodes.Status422UnprocessableEntity);
  }

  /// <summary>
  ///   Creates a conflict response with the specified result.
  /// </summary>
  /// <param name="result">The result containing the errors.</param>
  /// <returns>A conflict response with the specified result.</returns>
  private static Microsoft.AspNetCore.Http.IResult ConflictEntity(IResult result) {
    return new ProblemDetails(
      result.Errors.Select(e => new ValidationFailure("", e)).ToList(),
      StatusCodes.Status409Conflict);
  }

  /// <summary>
  ///   Converts the given <see cref="IResult" /> to a critical entity <see cref="IResult" />.
  /// </summary>
  /// <param name="result">The result to convert.</param>
  /// <returns>A critical entity <see cref="IResult" />.</returns>
  private static Microsoft.AspNetCore.Http.IResult CriticalEntity(IResult result) {
    return new ProblemDetails(
      result.Errors.Select(e => new ValidationFailure("", e)).ToList(),
      StatusCodes.Status500InternalServerError);
  }

  /// <summary>
  ///   Converts an <see cref="IResult" /> to a <see cref="Microsoft.AspNetCore.Http.IResult" /> with a 503 Service
  ///   Unavailable status code.
  /// </summary>
  /// <param name="result">The result to convert.</param>
  /// <returns>A <see cref="Microsoft.AspNetCore.Http.IResult" /> with a 503 Service Unavailable status code.</returns>
  private static Microsoft.AspNetCore.Http.IResult UnavailableEntity(IResult result) {
    return new ProblemDetails(
      result.Errors.Select(e => new ValidationFailure("", e)).ToList(),
      StatusCodes.Status503ServiceUnavailable);
  }
}
