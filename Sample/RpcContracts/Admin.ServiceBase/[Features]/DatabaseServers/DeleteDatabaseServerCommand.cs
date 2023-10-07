namespace Admin.ServiceBase.DatabaseServers;

public record DeleteDatabaseServerCommand(
Guid Id
) : ICommand<DeleteDatabaseServerResult>;

public record DeleteDatabaseServerResult(
Guid Id
);
