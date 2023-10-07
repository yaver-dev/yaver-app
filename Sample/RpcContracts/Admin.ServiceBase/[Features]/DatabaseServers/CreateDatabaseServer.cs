namespace Admin.ServiceBase.DatabaseServers;

public record CreateDatabaseServerCommand(
  string Host,
  int Port,
  string Name,
  string ConnectionStringFormat,
  bool IsDefault,
  DatabaseServerStatus Status
) : ICommand<DatabaseServerResult>;
