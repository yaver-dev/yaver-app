namespace Admin.ServiceBase.DatabaseServers;

public record DatabaseServerResult(
  Guid Id,
  string Host,
  int Port,
  string Name,
  string ConnectionStringFormat,
  bool IsDefault,
  DatabaseServerStatus Status);
