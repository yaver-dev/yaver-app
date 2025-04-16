﻿using System.Text.Json.Serialization;

// ReSharper disable once CheckNamespace
namespace Yaver.App;

/// <summary>
/// Generic result class that encapsulates the outcome of an operation.
/// </summary>
/// <typeparam name="T">Type of the result value</typeparam>
public class Result<T> : IResult {
  /// <summary>
  /// Default constructor for creating a result instance.
  /// </summary>
  protected Result() { }

  /// <summary>
  /// Creates a result instance with a specific value.
  /// </summary>
  /// <param name="value">The value to be wrapped in the result</param>
  public Result(T value) {
    Value = value;
  }

  /// <summary>
  /// Creates a result instance with a value and a success message.
  /// </summary>
  /// <param name="value">The value to be wrapped in the result</param>
  /// <param name="successMessage">Message indicating successful operation</param>
  protected internal Result(T value, string successMessage) : this(value) {
    SuccessMessage = successMessage;
  }

  /// <summary>
  /// Creates a result instance with a specific status.
  /// </summary>
  /// <param name="status">Status of the result</param>
  protected Result(ResultStatus status) {
    Status = status;
  }

  /// <summary>
  /// Gets the value of the result.
  /// </summary>
  public T Value { get; }

  /// <summary>
  /// Indicates whether the result is successful.
  /// </summary>
  public bool IsSuccess => Status == ResultStatus.Ok;

  /// <summary>
  /// Indicates whether the result is a failure.
  /// </summary>
  public bool IsFailure => !IsSuccess;

  /// <summary>
  /// Success message associated with the result.
  /// </summary>
  public string SuccessMessage { get; set; } = string.Empty;

  /// <summary>
  /// Correlation ID associated with the result.
  /// </summary>
  public string CorrelationId { get; set; } = string.Empty;

  /// <summary>
  /// Type of the value contained in the result.
  /// </summary>
  [JsonIgnore]
  public Type ValueType => typeof(T);

  /// <summary>
  /// Status of the result.
  /// </summary>
  public ResultStatus Status { get; set; } = ResultStatus.Ok;

  /// <summary>
  /// Collection of errors associated with the result.
  /// </summary>
  public IEnumerable<string> Errors { get; set; } = new List<string>();

  /// <summary>
  /// List of validation errors.
  /// </summary>
  public List<ValidationError> ValidationErrors { get; set; } = new();

  /// <summary>
  /// Returns the current value.
  /// </summary>
  /// <returns>The value of the result</returns>
  public object? GetValue() {
    return Value;
  }

  /// <summary>
  /// Implicitly converts a Result{T} to its underlying value of type T.
  /// </summary>
  /// <param name="result">The result to convert</param>
  /// <returns>The value contained in the result</returns>
  public static implicit operator T(Result<T> result) {
    return result.Value;
  }

  /// <summary>
  /// Implicitly converts a value of type T to a successful Result{T}.
  /// </summary>
  /// <param name="value">The value to wrap in a result</param>
  /// <returns>A new Result{T} containing the value</returns>
  public static implicit operator Result<T>(T value) {
    return new Result<T>(value);
  }

  /// <summary>
  /// Implicitly converts a non-generic Result to a Result{T} with default value.
  /// </summary>
  /// <param name="result">The result to convert</param>
  /// <returns>A new Result{T} with the same status and messages</returns>
  public static implicit operator Result<T>(Result result) {
    return new Result<T>(default(T)) {
      Status = result.Status,
      Errors = result.Errors,
      SuccessMessage = result.SuccessMessage,
      CorrelationId = result.CorrelationId,
      ValidationErrors = result.ValidationErrors
    };
  }

  /// <summary>
  /// Represents a successful operation and accepts a value as the result of the operation.
  /// </summary>
  /// <param name="value">Sets the Value property</param>
  /// <returns>A Result<typeparamref name="T" /></returns>
  public static Result<T> Success(T value) {
    return new Result<T>(value);
  }

  /// <summary>
  /// Represents a successful operation and accepts a value as the result of the operation.
  /// Sets the SuccessMessage property to the provided value.
  /// </summary>
  /// <param name="value">Sets the Value property</param>
  /// <param name="successMessage">Sets the SuccessMessage property</param>
  /// <returns>A Result<typeparamref name="T" /></returns>
  public static Result<T> Success(T value, string successMessage) {
    return new Result<T>(value, successMessage);
  }

  /// <summary>
  /// Represents an error that occurred during the execution of the service.
  /// Error messages may be provided and will be exposed via the Errors property.
  /// </summary>
  /// <param name="errorMessages">A list of string error messages</param>
  /// <returns>A Result<typeparamref name="T" /></returns>
  public static Result<T> Error(params string[] errorMessages) {
    return new Result<T>(ResultStatus.Error) { Errors = errorMessages };
  }

  /// <summary>
  /// Represents a validation error that prevents the underlying service from completing.
  /// </summary>
  /// <param name="validationError">The validation error encountered</param>
  /// <returns>A Result<typeparamref name="T" /></returns>
  public static Result<T> Invalid(ValidationError validationError) {
    return new Result<T>(ResultStatus.Invalid) { ValidationErrors = { validationError } };
  }

  /// <summary>
  /// Represents validation errors that prevent the underlying service from completing.
  /// </summary>
  /// <param name="validationErrors">A list of validation errors encountered</param>
  /// <returns>A Result<typeparamref name="T" /></returns>
  public static Result<T> Invalid(params ValidationError[] validationErrors) {
    return new Result<T>(ResultStatus.Invalid) { ValidationErrors = new List<ValidationError>(validationErrors) };
  }

  /// <summary>
  /// Represents validation errors that prevent the underlying service from completing.
  /// </summary>
  /// <param name="validationErrors">A list of validation errors encountered</param>
  /// <returns>A Result<typeparamref name="T" /></returns>
  public static Result<T> Invalid(List<ValidationError> validationErrors) {
    return new Result<T>(ResultStatus.Invalid) { ValidationErrors = validationErrors };
  }

  /// <summary>
  /// Represents the situation where a service was unable to find a requested resource.
  /// </summary>
  /// <returns>A Result<typeparamref name="T" /></returns>
  public static Result<T> NotFound() {
    return new Result<T>(ResultStatus.NotFound);
  }

  /// <summary>
  /// Represents the situation where a service was unable to find a requested resource.
  /// Error messages may be provided and will be exposed via the Errors property.
  /// </summary>
  /// <param name="errorMessages">A list of string error messages</param>
  /// <returns>A Result<typeparamref name="T" /></returns>
  public static Result<T> NotFound(params string[] errorMessages) {
    return new Result<T>(ResultStatus.NotFound) { Errors = errorMessages };
  }

  /// <summary>
  /// The parameters to the call were correct, but the user does not have permission to perform some action.
  /// See also HTTP 403 Forbidden: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
  /// </summary>
  /// <returns>A Result<typeparamref name="T" /></returns>
  public static Result<T> Forbidden() {
    return new Result<T>(ResultStatus.Forbidden);
  }

  /// <summary>
  /// This is similar to Forbidden, but should be used when the user has not authenticated or has attempted to authenticate
  /// but failed.
  /// See also HTTP 401 Unauthorized: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
  /// </summary>
  /// <returns>A Result<typeparamref name="T" /></returns>
  public static Result<T> Unauthorized() {
    return new Result<T>(ResultStatus.Unauthorized);
  }

  /// <summary>
  /// Represents a situation where a service is in conflict due to the current state of a resource,
  /// such as an edit conflict between multiple concurrent updates.
  /// See also HTTP 409 Conflict: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
  /// </summary>
  /// <returns>A Result<typeparamref name="T" /></returns>
  public static Result<T> Conflict() {
    return new Result<T>(ResultStatus.Conflict);
  }

  /// <summary>
  /// Represents a situation where a service is in conflict due to the current state of a resource,
  /// such as an edit conflict between multiple concurrent updates.
  /// Error messages may be provided and will be exposed via the Errors property.
  /// See also HTTP 409 Conflict: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#4xx_client_errors
  /// </summary>
  /// <param name="errorMessages">A list of string error messages</param>
  /// <returns>A Result<typeparamref name="T" /></returns>
  public static Result<T> Conflict(params string[] errorMessages) {
    return new Result<T>(ResultStatus.Conflict) { Errors = errorMessages };
  }

  /// <summary>
  /// Represents a critical error that occurred during the execution of the service.
  /// Everything provided by the user was valid, but the service was unable to complete due to an exception.
  /// See also HTTP 500 Internal Server Error: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#5xx_server_errors
  /// </summary>
  /// <param name="errorMessages">A list of string error messages</param>
  /// <returns>A Result<typeparamref name="T" /></returns>
  public static Result<T> CriticalError(params string[] errorMessages) {
    return new Result<T>(ResultStatus.CriticalError) { Errors = errorMessages };
  }

  /// <summary>
  /// Represents a situation where a service is unavailable, such as when the underlying data store is unavailable.
  /// Errors may be transient, so the caller may wish to retry the operation.
  /// See also HTTP 503 Service Unavailable: https://en.wikipedia.org/wiki/List_of_HTTP_status_codes#5xx_server_errors
  /// </summary>
  /// <param name="errorMessages">A list of string error messages</param>
  /// <returns>A Result<typeparamref name="T" /></returns>
  public static Result<T> Unavailable(params string[] errorMessages) {
    return new Result<T>(ResultStatus.Unavailable) { Errors = errorMessages };
  }
}
