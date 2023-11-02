using Yaver.App.Result;

namespace Admin.ServiceBase.Features.DatabaseServers;

public record GetDatabaseServerCommand(
  Guid Id
) : ICommand<Result<DatabaseServerResult>>;
