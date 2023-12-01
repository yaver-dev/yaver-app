﻿// ReSharper disable once CheckNamespace

namespace Yaver.App;

/// <summary>
///   Represents the result of an operation.
/// </summary>
public interface IResult {
  /// <summary>
  ///   Gets the status of the result.
  /// </summary>
  ResultStatus Status { get; }

  /// <summary>
  ///   Gets the errors associated with the result.
  /// </summary>
  IEnumerable<string> Errors { get; }

  /// <summary>
  ///   Gets the validation errors associated with the result.
  /// </summary>
  List<ValidationError> ValidationErrors { get; }

  /// <summary>
  ///   Gets the type of the value associated with the result.
  /// </summary>
  Type ValueType { get; }

  /// <summary>
  ///   Gets the value associated with the result.
  /// </summary>
  /// <returns>The value associated with the result.</returns>
  object GetValue();
}
