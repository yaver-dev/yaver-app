using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace Yaver.App;

public class InMemoryJsonStringLocalizerFactory(IMemoryCache cache)  : IStringLocalizerFactory {
  private readonly IMemoryCache _cache = cache;

  /// <summary>
  /// Represents a service that provides localized strings for an application.
  /// </summary>
  public IStringLocalizer Create(Type resourceSource)
    => new InMemoryJsonStringLocalizer(_cache);

  /// <summary>
  /// Represents a service that provides localized strings for an application.
  /// </summary>
  public IStringLocalizer Create(string baseName, string location)
    => new InMemoryJsonStringLocalizer(_cache);
}
