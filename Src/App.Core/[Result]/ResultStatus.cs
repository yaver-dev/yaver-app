﻿// ReSharper disable once CheckNamespace

namespace Yaver.App;

/// <summary>
///   Represents the status of a result.
/// </summary>
public enum ResultStatus {
  /// <summary>
  ///   The operation completed successfully.
  /// </summary>
  Ok,

  /// <summary>
  ///   An error occurred during the operation.
  /// </summary>
  Error,

  /// <summary>
  ///   The operation was forbidden.
  /// </summary>
  Forbidden,

  /// <summary>
  ///   The operation requires authentication.
  /// </summary>
  Unauthorized,

  /// <summary>
  ///   The input data was invalid.
  /// </summary>
  Invalid,

  /// <summary>
  ///   The requested resource was not found.
  /// </summary>
  NotFound,

  /// <summary>
  ///   The operation could not be completed due to a conflict.
  /// </summary>
  Conflict,

  /// <summary>
  ///   A critical error occurred during the operation.
  /// </summary>
  CriticalError,

  /// <summary>
  ///   The requested service is currently unavailable.
  /// </summary>
  Unavailable
}
