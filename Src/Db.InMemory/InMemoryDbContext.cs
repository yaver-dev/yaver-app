namespace Yaver.Db;

/// <summary>
///   Represents an in-memory database context for the current user.
/// </summary>
public class InMemoryDbContext(Guid currentUserId) : BaseDbContext(currentUserId) {
}
