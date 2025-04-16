﻿// ReSharper disable once CheckNamespace

namespace Yaver.App;

/// <summary>
/// Represents a generic result that can be returned from a service operation without a value.
/// </summary>
public class Result : Result<Result> {
  /// <summary>
  /// Initializes a new instance of the <see cref="Result"/> class.
  /// </summary>
  public Result() { }

  /// <summary>
  /// Initializes a new instance of the <see cref="Result"/> class with the specified status.
  /// </summary>
  /// <param name="status">The status of the result.</param>
  protected internal Result(ResultStatus status) : base(status) { }

  /// <summary>
  /// Creates a successful result without a return value.
  /// </summary>
  /// <returns>A successful <see cref="Result"/>.</returns>
  public static Result Success() {
    return new Result();
  }

  /// <summary>
  /// Creates a successful result with a success message.
  /// </summary>
  /// <param name="successMessage">The success message to set.</param>
  /// <returns>A successful <see cref="Result"/> with a message.</returns>
  public static Result SuccessWithMessage(string successMessage) {
    return new Result { SuccessMessage = successMessage };
  }

  /// <summary>
  /// Creates a successful result with a value.
  /// </summary>
  /// <typeparam name="T">The type of the value.</typeparam>
  /// <param name="value">The value to set.</param>
  /// <returns>A successful <see cref="Result{T}"/> containing the value.</returns>
  public static Result<T> Success<T>(T value) {
    return new Result<T>(value);
  }

  /// <summary>
  /// Creates a successful result with a value and a success message.
  /// </summary>
  /// <typeparam name="T">The type of the value.</typeparam>
  /// <param name="value">The value to set.</param>
  /// <param name="successMessage">The success message to set.</param>
  /// <returns>A successful <see cref="Result{T}"/> with a message.</returns>
  public static Result<T> Success<T>(T value, string successMessage) {
    return new Result<T>(value, successMessage);
  }

  /// <summary>
  /// Creates an error result with the specified error messages.
  /// </summary>
  /// <param name="errorMessages">The error messages to include.</param>
  /// <returns>An error <see cref="Result"/>.</returns>
  public new static Result Error(params string[] errorMessages) {
    return new Result(ResultStatus.Error) { Errors = errorMessages };
  }

  /// <summary>
  /// Creates an error result with a correlation ID and error messages.
  /// </summary>
  /// <param name="correlationId">The correlation ID to set.</param>
  /// <param name="errorMessages">The error messages to include.</param>
  /// <returns>An error <see cref="Result"/> with a correlation ID.</returns>
  public static Result ErrorWithCorrelationId(string correlationId, params string[] errorMessages) {
    return new Result(ResultStatus.Error) { CorrelationId = correlationId, Errors = errorMessages };
  }

  /// <summary>
  /// Creates a result representing a single validation error.
  /// </summary>
  /// <param name="validationError">The validation error to include.</param>
  /// <returns>An invalid <see cref="Result"/>.</returns>
  public new static Result Invalid(ValidationError validationError) {
    return new Result(ResultStatus.Invalid) { ValidationErrors = { validationError } };
  }

  /// <summary>
  /// Creates a result representing multiple validation errors.
  /// </summary>
  /// <param name="validationErrors">The validation errors to include.</param>
  /// <returns>An invalid <see cref="Result"/>.</returns>
  public new static Result Invalid(params ValidationError[] validationErrors) {
    return new Result(ResultStatus.Invalid) { ValidationErrors = new List<ValidationError>(validationErrors) };
  }

  /// <summary>
  /// Creates a result representing a list of validation errors.
  /// </summary>
  /// <param name="validationErrors">The list of validation errors to include.</param>
  /// <returns>An invalid <see cref="Result"/>.</returns>
  public new static Result Invalid(List<ValidationError> validationErrors) {
    return new Result(ResultStatus.Invalid) { ValidationErrors = validationErrors };
  }

  /// <summary>
  /// Creates a result indicating that a requested resource was not found.
  /// </summary>
  /// <returns>A not found <see cref="Result"/>.</returns>
  public new static Result NotFound() {
    return new Result(ResultStatus.NotFound);
  }

  /// <summary>
  /// Creates a result indicating that a requested resource was not found, with error messages.
  /// </summary>
  /// <param name="errorMessages">The error messages to include.</param>
  /// <returns>A not found <see cref="Result"/>.</returns>
  public new static Result NotFound(params string[] errorMessages) {
    return new Result(ResultStatus.NotFound) { Errors = errorMessages };
  }

  /// <summary>
  /// Creates a result indicating that the user does not have permission to perform the action.
  /// See also HTTP 403 Forbidden.
  /// </summary>
  /// <returns>A forbidden <see cref="Result"/>.</returns>
  public new static Result Forbidden() {
    return new Result(ResultStatus.Forbidden);
  }

  /// <summary>
  /// Creates a result indicating that the user is not authenticated or authentication failed.
  /// See also HTTP 401 Unauthorized.
  /// </summary>
  /// <returns>An unauthorized <see cref="Result"/>.</returns>
  public new static Result Unauthorized() {
    return new Result(ResultStatus.Unauthorized);
  }

  /// <summary>
  /// Creates a result indicating a conflict due to the current state of a resource.
  /// See also HTTP 409 Conflict.
  /// </summary>
  /// <returns>A conflict <see cref="Result"/>.</returns>
  public new static Result Conflict() {
    return new Result(ResultStatus.Conflict);
  }

  /// <summary>
  /// Creates a result indicating a conflict due to the current state of a resource, with error messages.
  /// See also HTTP 409 Conflict.
  /// </summary>
  /// <param name="errorMessages">The error messages to include.</param>
  /// <returns>A conflict <see cref="Result"/>.</returns>
  public new static Result Conflict(params string[] errorMessages) {
    return new Result(ResultStatus.Conflict) { Errors = errorMessages };
  }

  /// <summary>
  /// Creates a result indicating that the service is unavailable. Errors may be transient.
  /// See also HTTP 503 Service Unavailable.
  /// </summary>
  /// <param name="errorMessages">The error messages to include.</param>
  /// <returns>An unavailable <see cref="Result"/>.</returns>
  public new static Result Unavailable(params string[] errorMessages) {
    return new Result(ResultStatus.Unavailable) { Errors = errorMessages };
  }

  /// <summary>
  /// Creates a result indicating a critical error with the specified error messages.
  /// </summary>
  /// <param name="errorMessages">The error messages to include.</param>
  /// <returns>A critical error <see cref="Result"/>.</returns>
  public new static Result CriticalError(params string[] errorMessages) {
    return new Result(ResultStatus.CriticalError) { Errors = errorMessages };
  }
}
