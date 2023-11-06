using Yaver.App;
using Yaver.App.Result;

namespace Admin.ServiceBase.Features.DatabaseServers;

public record GetDatabaseServerCommand(
  Guid Id
) : IRpcCommand<Result<DatabaseServerResult>>;
