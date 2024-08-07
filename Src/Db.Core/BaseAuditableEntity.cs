﻿namespace Yaver.Db;

/// <summary>
///   Represents an entity that tracks audit information such as creation and update timestamps and user IDs.
/// </summary>
public class BaseAuditableEntity {
  /// <summary>
  ///   Gets or sets the date and time when the entity was created.
  /// </summary>
  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  /// <summary>
  ///   Gets or sets the ID of the user who created the entity.
  /// </summary>
  public Guid CreatedBy { get; set; }

  /// <summary>
  ///   Gets or sets the date and time when the entity was last updated.
  /// </summary>
  public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

  /// <summary>
  ///   Gets or sets the ID of the user who last updated the entity.
  /// </summary>
  public Guid UpdatedBy { get; set; }
}
