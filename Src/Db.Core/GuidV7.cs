using UUIDNext;

namespace Yaver.Db;
public static class GuidV7 {
  public static Guid CreateVersion7() {
    return Uuid.NewDatabaseFriendly(Database.PostgreSql);
  }
}
