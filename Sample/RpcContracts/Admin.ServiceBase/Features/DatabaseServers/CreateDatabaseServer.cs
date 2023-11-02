using Yaver.App.Result;

namespace Admin.ServiceBase.Features.DatabaseServers;

public sealed record CreateDatabaseServerCommand(
  string Host,
  int Port,
  string Name,
  string ConnectionStringFormat,
  bool IsDefault,
  DatabaseServerStatus Status
) : ICommand<Result<DatabaseServerResult>>;
