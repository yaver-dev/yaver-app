using System.Text.Json;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;

namespace Yaver.App;

public class InMemoryJsonStringLocalizer(IMemoryCache cache) : IStringLocalizer {
  private readonly IMemoryCache _cache = cache;
  private readonly JsonSerializerOptions _serializerOptions = new();

  /// <summary>
  /// Gets the localized string for the specified resource name.
  /// </summary>
  /// <param name="name">The resource name to localize.</param>
  /// <returns>
  /// The localized string for the specified name. If not found, returns the original name and sets <see cref="LocalizedString.ResourceNotFound"/> to true.
  /// </returns>
  public LocalizedString this[string name] {
    get {
      var value = GetString(name);
      return new LocalizedString(name, value ?? name, value == null);
    }
  }

  /// <summary>
  /// Gets the localized string for the specified resource name and formats it with the provided arguments.
  /// </summary>
  /// <param name="name">The resource name to localize.</param>
  /// <param name="arguments">Arguments to format the localized string.</param>
  /// <returns>
  /// The formatted localized string as a <see cref="LocalizedString"/>. If not found, returns the original name and sets <see cref="LocalizedString.ResourceNotFound"/> to true.
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
  /// Retrieves all localized strings from the JSON file for the current culture.
  /// </summary>
  /// <param name="includeParentCultures">Whether to include parent cultures in the search.</param>
  /// <returns>An enumerable collection of <see cref="LocalizedString"/> objects.</returns>
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
  /// Gets the localized string value for the specified key from the cache or JSON file.
  /// </summary>
  /// <param name="key">The resource key to look up.</param>
  /// <returns>The localized string value, or null if not found.</returns>
  private string? GetString(string key) {
    var relativeFilePath = $"i18n/{Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName}.json";
    var fullFilePath = Path.GetFullPath(relativeFilePath);
    if (File.Exists(fullFilePath)) {
      var cacheKey = $"locale_{Thread.CurrentThread.CurrentCulture.Name}_{key}";
      var cacheValue = _cache.Get<string>(cacheKey);
      if (!string.IsNullOrEmpty(cacheValue)) {
        return cacheValue;
      }

      var result = GetValueFromJSON(key, Path.GetFullPath(relativeFilePath));

      if (!string.IsNullOrEmpty(result)) {
        _cache.Set<string>(cacheKey, result);
      }
      return result;
    }
    return default;
  }

  /// <summary>
  /// Retrieves the value for a given property name from the specified JSON file.
  /// </summary>
  /// <param name="propertyName">The property name to look up.</param>
  /// <param name="filePath">The path to the JSON file.</param>
  /// <returns>The value as a string, or null if not found.</returns>
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
