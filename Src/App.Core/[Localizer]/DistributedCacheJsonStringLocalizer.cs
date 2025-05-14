using System.Text.Json;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace Yaver.App;

/// <summary>
/// Implements a distributed cache-based string localizer that uses JSON files as the resource source.
/// This implementation combines the benefits of distributed caching with JSON-based localization,
/// providing efficient and scalable string localization across multiple application instances.
/// </summary>
public class DistributedCacheJsonStringLocalizer(IDistributedCache cache) : IStringLocalizer {
  private readonly IDistributedCache _cache = cache;
  private readonly JsonSerializerOptions _serializerOptions = new();

  /// <summary>
  /// Retrieves a localized string for the specified resource name.
  /// First checks the distributed cache, then falls back to the JSON resource file if not found.
  /// </summary>
  /// <param name="name">The resource name/key to localize.</param>
  /// <returns>
  /// A <see cref="LocalizedString"/> containing the localized value. If the resource is not found,
  /// returns the original name and sets <see cref="LocalizedString.ResourceNotFound"/> to true.
  /// </returns>
  public LocalizedString this[string name] {
    get {
      var value = GetString(name);
      return new LocalizedString(name, value ?? name, value == null);
    }
  }

  /// <summary>
  /// Retrieves a localized string and formats it with the provided arguments.
  /// Supports composite format strings with placeholders for dynamic content.
  /// </summary>
  /// <param name="name">The resource name/key to localize.</param>
  /// <param name="arguments">Variable number of arguments to format the localized string.</param>
  /// <returns>
  /// A formatted <see cref="LocalizedString"/>. If the resource is not found,
  /// returns the original name and sets <see cref="LocalizedString.ResourceNotFound"/> to true.
  /// </returns>
  public LocalizedString this[string name, params object[] arguments] {
    get {
      var actualValue = this[name];
      return !actualValue.ResourceNotFound
          ? new LocalizedString(name, string.Format(actualValue.Value, arguments), false)
          : actualValue;
    }
  }

  /// <summary>
  /// Retrieves all localized strings for the current culture from the JSON resource file.
  /// This method reads the entire JSON file and returns all key-value pairs as localized strings.
  /// </summary>
  /// <param name="includeParentCultures">Whether to include strings from parent cultures in the result.</param>
  /// <returns>An enumerable collection of all <see cref="LocalizedString"/> objects for the current culture.</returns>
  public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) {
    var filePath = $"i18n/{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName}.json";
    using var str = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
    using var doc = JsonDocument.Parse(str);
    var root = doc.RootElement;
    foreach (var prop in root.EnumerateObject()) {
      yield return new LocalizedString(prop.Name, prop.Value.GetString(), false);
    }
  }

  /// <summary>
  /// Retrieves a localized string value using a two-level caching strategy:
  /// 1. First checks the distributed cache for the value
  /// 2. If not found in cache, reads from the JSON file and updates the cache
  /// </summary>
  /// <param name="key">The resource key to look up.</param>
  /// <returns>The localized string value if found, null otherwise.</returns>
  private string? GetString(string key) {
    var relativeFilePath = $"i18n/{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName}.json";
    var fullFilePath = Path.GetFullPath(relativeFilePath);
    if (File.Exists(fullFilePath)) {
      var cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_{key}";
      var cacheValue = _cache.GetString(cacheKey);
      if (!string.IsNullOrEmpty(cacheValue)) {
        return cacheValue;
      }

      var result = GetValueFromJSON(key, Path.GetFullPath(relativeFilePath));

      if (!string.IsNullOrEmpty(result)) {
        _cache.SetString(cacheKey, result);
      }
      return result;
    }
    return default;
  }

  /// <summary>
  /// Extracts a specific value from a JSON resource file.
  /// This method handles the low-level JSON parsing and property extraction.
  /// </summary>
  /// <param name="propertyName">The name of the property to extract from the JSON file.</param>
  /// <param name="filePath">The absolute path to the JSON resource file.</param>
  /// <returns>The string value of the property if found, null otherwise.</returns>
  private static string? GetValueFromJSON(string propertyName, string filePath) {
    if (propertyName == null || filePath == null) {
      return default;
    }

    using var str = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
    using var doc = JsonDocument.Parse(str);
    var root = doc.RootElement;

    if (root.TryGetProperty(propertyName, out var property)) {
      return property.GetString();
    }

    return default;
  }
}
