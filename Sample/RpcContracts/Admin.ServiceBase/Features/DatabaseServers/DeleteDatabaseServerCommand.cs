namespace Admin.ServiceBase.Features.DatabaseServers;

public record DeleteDatabaseServerCommand(
  Guid Id
) : ICommand<DeleteDatabaseServerResult>;

public record DeleteDatabaseServerResult(
  Guid Id
);
