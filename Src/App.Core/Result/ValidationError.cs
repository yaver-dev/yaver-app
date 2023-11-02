﻿namespace Yaver.App.Result;

/// <summary>
/// Represents a validation error that occurred during a request.
/// </summary>
public class ValidationError {
  /// <summary>
  /// Initializes a new instance of the <see cref="ValidationError"/> class.
  /// </summary>
  public ValidationError() {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="ValidationError"/> class with a specific error message.
  /// </summary>
  /// <param name="errorMessage">The error message for the validation error.</param>
  public ValidationError(string errorMessage) => ErrorMessage = errorMessage;

  /// <summary>
  /// Initializes a new instance of the <see cref="ValidationError"/> class with an identifier, error message, error code, and severity.
  /// </summary>
  /// <param name="identifier">The identifier of the validation error.</param>
  /// <param name="errorMessage">The error message of the validation error.</param>
  /// <param name="errorCode">The error code of the validation error.</param>
  /// <param name="severity">The severity of the validation error.</param>
  public ValidationError(string identifier, string errorMessage, string errorCode, ValidationSeverity severity) {
    Identifier = identifier;
    ErrorMessage = errorMessage;
    ErrorCode = errorCode;
    Severity = severity;
  }

  /// <summary>
  /// Gets or sets the identifier of the validation error.
  /// </summary>
  public string Identifier { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the error message associated with the validation error.
  /// </summary>
  public string ErrorMessage { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the error code associated with the validation error.
  /// </summary>
  public string ErrorCode { get; set; } = string.Empty;
  /// <summary>
  /// Gets or sets the severity of the validation error.
  /// </summary>
  public ValidationSeverity Severity { get; set; } = ValidationSeverity.Error;
}
