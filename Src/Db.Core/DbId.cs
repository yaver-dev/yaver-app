using UUIDNext;

namespace Yaver.Db;
public readonly struct DbId
    : IEquatable<DbId>, IComparable<DbId> {
  private readonly Guid _guid;
  private static readonly DbId _empty = new(Guid.Empty);

  public static DbId Empty => _empty;

  public DbId(Guid guid) {
    _guid = guid;
  }

  /// <summary>
  /// Uuid V7 comapatible implementation compatible with PostgreSQL, SQLite
  /// </summary>
  /// <returns>Uuid V7 compatible new DbId</returns>
  public static DbId NewDbId() {
    return new(Uuid.NewDatabaseFriendly(Database.PostgreSql));
  }

  /// <summary>
  /// Microsoft SQL Server uniqueidentifier data type compatible implementation
  /// </summary>
  /// <returns>Ms SQL uniqueidentifier compatible new DbId</returns>
  //public static DbId NewMsSqlDbId() {
  //  return new(Uuid.NewDatabaseFriendly(Database.SqlServer));
  //}

  public static DbId FromGuid(Guid guid) {
    return new(guid);
  }

  public Guid ToGuid() {
    return _guid;
  }

  public override string ToString() {
    return _guid.ToString();
  }

  public override bool Equals(object? obj) {
    if (ReferenceEquals(null, obj)) return false;
    if (obj is DbId dbId)
      return Equals(dbId);
    return false;
  }

  public bool Equals(DbId other) {
    return _guid.Equals(other._guid);
  }

  public override int GetHashCode() {
    return HashCode.Combine(_guid);
  }

  public int CompareTo(DbId other) {
    return _guid.CompareTo(other._guid);
  }

  public static bool operator ==(DbId left, DbId right) {
    return left.Equals(right);
  }

  public static bool operator !=(DbId left, DbId right) {
    return !(left == right);
  }

  public static bool operator <(DbId left, DbId right) {
    return left.CompareTo(right) < 0;
  }

  public static bool operator <=(DbId left, DbId right) {
    return left.CompareTo(right) <= 0;
  }

  public static bool operator >(DbId left, DbId right) {
    return left.CompareTo(right) > 0;
  }

  public static bool operator >=(DbId left, DbId right) {
    return left.CompareTo(right) >= 0;
  }
}
