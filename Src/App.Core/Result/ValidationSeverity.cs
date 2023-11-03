﻿namespace Yaver.App.Result;

/// <summary>
/// Represents the severity of a validation result.
/// </summary>
public enum ValidationSeverity {
  /// <summary>
  /// Indicates that the validation result is an error.
  /// </summary>
  Error = 0,
  /// <summary>
  /// Indicates that the validation result is a warning.
  /// </summary>
  Warning = 1,
  /// <summary>
  /// Indicates that the validation result is an informational message.
  /// </summary>
  Info = 2
}
