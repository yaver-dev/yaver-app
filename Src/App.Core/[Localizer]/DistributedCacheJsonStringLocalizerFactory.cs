using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace Yaver.App;

/// <summary>
/// Represents a factory for creating instances of <see cref="DistributedCacheJsonStringLocalizer"/>.
/// </summary>
public class DistributedCacheJsonStringLocalizerFactory(IDistributedCache cache) : IStringLocalizerFactory {
  private readonly IDistributedCache _cache = cache;

  /// <summary>
  /// Represents a service that provides localized strings for an application.
  /// </summary>
  public IStringLocalizer Create(Type resourceSource) =>
      new DistributedCacheJsonStringLocalizer(_cache);

  /// <summary>
  /// Represents a service that provides localized strings for an application.
  /// </summary>
  public IStringLocalizer Create(string baseName, string location) =>
     new DistributedCacheJsonStringLocalizer(_cache);
}
