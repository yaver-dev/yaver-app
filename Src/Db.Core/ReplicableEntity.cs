// ReSharper disable once CheckNamespace

namespace Yaver.Db;

/// <summary>
///   Represents an entity that tracks audit information and origin of the replication system such as origin, creation and update timestamps and user Ids.
/// </summary>
public abstract class ReplicableEntity : AuditableEntity {
  /// <summary>
  ///   Gets or sets the origin of the replication.
  /// </summary>
  public string Origin { get; set; } = string.Empty;
}
