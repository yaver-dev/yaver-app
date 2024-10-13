using Microsoft.EntityFrameworkCore;

namespace Yaver.Db;

/// <summary>
///   Represents an in-memory database context for temporary and testing purposes.
/// </summary>
/// <remarks>
///   This context is intended for use with in-memory data storage, typically in scenarios such as
///   unit testing or temporary data management, without the need for a persistent database.
/// </remarks>
public class InMemoryDbContext : DbContext {
  /// <summary>
  ///   Initializes a new instance of the <see cref="InMemoryDbContext"/> class with default options.
  /// </summary>
  protected InMemoryDbContext() : base(new DbContextOptions<InMemoryDbContext>()) {
  }

  /// <summary>
  ///   Initializes a new instance of the <see cref="InMemoryDbContext"/> class with the specified options.
  /// </summary>
  /// <param name="options">The options to configure the context.</param>
  public InMemoryDbContext(DbContextOptions options) : base(options) {
  }
}
