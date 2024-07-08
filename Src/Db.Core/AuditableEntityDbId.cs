// ReSharper disable once CheckNamespace

namespace Yaver.Db;

/// <summary>
///   Represents an entity that tracks audit information such as creation and update timestamps and user IDs.
/// </summary>
public abstract class AuditableEntityDbId : BaseAuditableEntity {
  /// <summary>
  ///   Gets or sets the unique identifier for the entity.
  /// </summary>
  public DbId Id { get; set; } = DbId.NewDbId();
}
