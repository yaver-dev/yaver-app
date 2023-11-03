using Yaver.App.Result;

namespace Admin.ServiceBase.Features.DatabaseServers;

public record DeleteDatabaseServerCommand(
  Guid Id
) : ICommand<Result>;

// public record DeleteDatabaseServerResult(
//   Guid Id
// );
