using System.Diagnostics.CodeAnalysis;

namespace Yaver.Db;
public static class DbIdExtensions {
  private const long UnixEpochMilliseconds = 62_135_596_800_000;
  private const long TicksPerMillisecond = 10_000;

  private static byte[] GetBytes(this DbId dbId) {
    var bytes = dbId.ToGuid().ToByteArray();
    if (BitConverter.IsLittleEndian) {  // little endian pretends the first 8 bytes are int, short, short
      (bytes[0], bytes[1], bytes[2], bytes[3]) = (bytes[3], bytes[2], bytes[1], bytes[0]);
      (bytes[4], bytes[5]) = (bytes[5], bytes[4]);
      (bytes[6], bytes[7]) = (bytes[7], bytes[6]);
    }
    return bytes;
  }

  private static (bool, byte[]) IsUuidV7Internal(this DbId dbId) {
    var bytes = dbId.GetBytes();
    return ((bytes[6] & 0xF0) == 0x70, bytes);
  }

  public static bool IsUuidV7(this DbId dbId) {
    var (isV7, _) = dbId.IsUuidV7Internal();
    return isV7;
  }

  public static DateTime GetDateTime(this DbId dbId) {
    if (dbId.TryGetDateTime(out var dateTime)) {
      return dateTime.Value;
    }
    throw new InvalidOperationException("Not compatible with Uuid version 7.");
  }

  public static bool TryGetDateTime(this DbId dbId, [NotNullWhen(true)] out DateTime? dateTime) {
    dateTime = null;
    var (isV7, bytes) = dbId.IsUuidV7Internal();
    if (!isV7) { return false; }
    var unixMs = (long)bytes[0] << 40 | (long)bytes[1] << 32 | (long)bytes[2] << 24 | (long)bytes[3] << 16 | (long)bytes[4] << 8 | bytes[5];
    var ticks = (UnixEpochMilliseconds + unixMs) * TicksPerMillisecond;
    dateTime = new DateTime(ticks, DateTimeKind.Utc);
    return true;
  }

  public static DateTimeOffset GetDateTimeOffset(this DbId dbId) {
    if (dbId.TryGetDateTimeOffset(out var dateTimeOffset)) {
      return dateTimeOffset.Value;
    }
    throw new InvalidOperationException("Not compatible with Uuid version 7.");
  }

  public static bool TryGetDateTimeOffset(this DbId dbId, [NotNullWhen(true)] out DateTimeOffset? dateTimeOffset) {
    dateTimeOffset = null;
    var (isV7, bytes) = dbId.IsUuidV7Internal();
    if (!isV7) { return false; }
    var unixMs = (long)bytes[0] << 40 | (long)bytes[1] << 32 | (long)bytes[2] << 24 | (long)bytes[3] << 16 | (long)bytes[4] << 8 | bytes[5];
    var ticks = (UnixEpochMilliseconds + unixMs) * TicksPerMillisecond;
    dateTimeOffset = new DateTimeOffset(ticks, TimeSpan.Zero);
    return true;
  }
}
