using System.Text.Json;

using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Localization;

namespace Yaver.App;


/// <summary>
/// Represents a string localizer that retrieves localized strings from a JSON file.
/// </summary>
public class JsonStringLocalizer(IDistributedCache cache) : IStringLocalizer {
  private readonly IDistributedCache _cache = cache;
  private readonly JsonSerializerOptions _serializerOptions = new();

  public LocalizedString this[string name] {
    get {
      var value = GetString(name);
      return new LocalizedString(name, value ?? name, value == null);
    }
  }

  public LocalizedString this[string name, params object[] arguments] {
    get {
      var actualValue = this[name];
      return !actualValue.ResourceNotFound
          ? new LocalizedString(name, string.Format(actualValue.Value, arguments), false)
          : actualValue;
    }
  }

  /// <summary>
  /// Retrieves all localized strings from the JSON file based on the current culture.
  /// </summary>
  /// <param name="includeParentCultures">A boolean value indicating whether to include parent cultures.</param>
  /// <returns>An enumerable collection of LocalizedString objects.</returns>
  public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures) {
    var filePath = $"i18n/{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName}.json";
    using var str = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
    using var doc = JsonDocument.Parse(str);
    var root = doc.RootElement;
    foreach (var prop in root.EnumerateObject()) {
      yield return new LocalizedString(prop.Name, prop.Value.GetString(), false);
    }
  }

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
