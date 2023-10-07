namespace Admin.ServiceBase.DatabaseServers;

public record GetDatabaseServerCommand(
  Guid Id
) : ICommand<DatabaseServerResult>;
