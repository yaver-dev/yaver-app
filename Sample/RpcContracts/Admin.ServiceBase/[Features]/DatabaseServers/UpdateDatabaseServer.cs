namespace Admin.ServiceBase.DatabaseServers;

public record UpdateDatabaseServerCommand(
  Guid Id,
  string Host,
  int Port,
  string Name,
  string ConnectionStringFormat,
  bool IsDefault,
  DatabaseServerStatus Status
) : ICommand<DatabaseServerResult>;
