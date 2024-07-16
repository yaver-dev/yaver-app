using Microsoft.EntityFrameworkCore;

namespace Yaver.Db;

/// <summary>
///   Represents an in-memory database context for the current user.
/// </summary>
public class InMemoryDbContext(DbContextOptions options) : BaseDbContext(options) {
}
