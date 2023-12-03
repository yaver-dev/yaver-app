// ReSharper disable once CheckNamespace

namespace Yaver.App;

/// <summary>
///   Provides extension methods for the <see cref="Result{T}" /> class.
/// </summary>
public static class ResultExtensions {
  /// <summary>
  ///   Transforms a Result's type from a source type to a destination type. If the Result is successful, the func parameter
  ///   is invoked on the Result's source value to map it to a destination type.
  /// </summary>
  /// <typeparam name="TSource"></typeparam>
  /// <typeparam name="TDestination"></typeparam>
  /// <param name="result"></param>
  /// <param name="func"></param>
  /// <returns></returns>
  /// <exception cref="NotSupportedException"></exception>
  public static Result<TDestination> Map<TSource, TDestination>(this Result<TSource> result,
    Func<TSource, TDestination> func) {
    return result.Status switch {
      ResultStatus.Ok => func(result),
      ResultStatus.NotFound => result.Errors.Any()
        ? Result<TDestination>.NotFound(result.Errors.ToArray())
        : Result<TDestination>.NotFound(),
      ResultStatus.Unauthorized => Result<TDestination>.Unauthorized(),
      ResultStatus.Forbidden => Result<TDestination>.Forbidden(),
      ResultStatus.Invalid => Result<TDestination>.Invalid(result.ValidationErrors),
      ResultStatus.Error => Result<TDestination>.Error(result.Errors.ToArray()),
      ResultStatus.Conflict => result.Errors.Any()
        ? Result<TDestination>.Conflict(result.Errors.ToArray())
        : Result<TDestination>.Conflict(),
      ResultStatus.CriticalError => Result<TDestination>.CriticalError(result.Errors.ToArray()),
      ResultStatus.Unavailable => Result<TDestination>.Unavailable(result.Errors.ToArray()),
      _ => throw new NotSupportedException($"Result {result.Status} conversion is not supported.")
    };
  }
}
