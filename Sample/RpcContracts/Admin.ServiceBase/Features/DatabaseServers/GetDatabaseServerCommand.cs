using Yaver.App;

namespace Admin.ServiceBase.Features.DatabaseServers;

public record GetDatabaseServerCommand(
  Guid Id
) : IRpcCommand<Result<DatabaseServerResult>>;
