﻿using FluentValidation;
using FluentValidation.Results;

// ReSharper disable once CheckNamespace
namespace Yaver.App;

/// <summary>
///   Provides extension methods for FluentValidation's ValidationResult class.
/// </summary>
public static class FluentValidationResultExtensions {
  /// <summary>
  ///   Converts a FluentValidation ValidationResult object to a list of ValidationError objects.
  /// </summary>
  /// <param name="valResult">The FluentValidation ValidationResult object to convert.</param>
  /// <returns>A list of ValidationError objects.</returns>
  public static List<ValidationError> AsValidationErrors(this ValidationResult valResult) {
    var resultErrors = new List<ValidationError>();

    foreach (var valFailure in valResult.Errors) {
      resultErrors.Add(new ValidationError {
        Severity = FromSeverity(valFailure.Severity),
        ErrorMessage = valFailure.ErrorMessage,
        ErrorCode = valFailure.ErrorCode,
        Identifier = valFailure.PropertyName
      });
    }

    return resultErrors;
  }

  /// <summary>
  /// Converts a FluentValidation ValidationResult object to a string array of error messages.
  /// </summary>
  /// <param name="valResult"></param>
  /// <returns></returns>
  public static string[] AsErrors(this ValidationResult valResult) {
    return valResult.Errors.Select(valFailure => valFailure.ErrorMessage).ToArray();
  }
  
  /// <summary>
  ///   Maps a <see cref="Severity" /> to a <see cref="ValidationSeverity" />.
  /// </summary>
  /// <param name="severity">The severity to map.</param>
  /// <returns>The mapped <see cref="ValidationSeverity" />.</returns>
  public static ValidationSeverity FromSeverity(Severity severity) {
    return severity switch {
      Severity.Error => ValidationSeverity.Error,
      Severity.Warning => ValidationSeverity.Warning,
      Severity.Info => ValidationSeverity.Info,
      _ => throw new ArgumentOutOfRangeException(nameof(severity), "Unexpected Severity")
    };
  }
}
