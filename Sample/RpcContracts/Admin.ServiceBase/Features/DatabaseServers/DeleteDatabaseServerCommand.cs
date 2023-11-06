using Yaver.App;
using Yaver.App.Result;

namespace Admin.ServiceBase.Features.DatabaseServers;

public record DeleteDatabaseServerCommand(
  Guid Id
) : IRpcCommand<Result>;

// public record DeleteDatabaseServerResult(
//   Guid Id
// );
