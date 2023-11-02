using Yaver.App.Result;

namespace Admin.ServiceBase.DatabaseServers;

public record GetDatabaseServerCommand(
  Guid Id
) : ICommand<Result<DatabaseServerResult>>;
