namespace Yaver.Db;
public static class GuidExtensions {
  public static DbId ToDbId(this Guid id) {
    return new DbId(id);
  }
}
