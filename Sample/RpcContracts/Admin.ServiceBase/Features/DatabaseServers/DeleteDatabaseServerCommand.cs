using Yaver.App;

namespace Admin.ServiceBase.Features.DatabaseServers;

public record DeleteDatabaseServerCommand(
  Guid Id
) : IRpcCommand<Result>;

// public record DeleteDatabaseServerResult(
//   Guid Id
// );
