using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Yaver.Db;
public class DbIdToGuidValueConverter : ValueConverter<DbId, Guid> {
  public DbIdToGuidValueConverter() : this(null) { }
  public DbIdToGuidValueConverter(ConverterMappingHints? mappingHints = null)
    : base(
      id => id.ToGuid(),
      value => new DbId(value),
      mappingHints) { }
}
