// ReSharper disable once CheckNamespace

namespace Yaver.Db;

/// <summary>
///   Represents an entity that tracks audit information such as creation and update timestamps and user IDs.
/// </summary>
public abstract class AuditableEntity : BaseAuditableEntity {
  /// <summary>
  ///   Gets or sets the unique identifier for the entity.
  /// </summary>
  public Guid Id { get; set; } = Guid.CreateVersion7();
}
