namespace Admin.ServiceBase.DatabaseServers;

public record ListDatabaseServersCommand(
  int Offset,
  int Limit,
  string Term,
  string Sort
) : ICommand<DatabaseServerListResult>;

public record DatabaseServerListResult(List<DatabaseServerListItem> Items, int? TotalCount = 0);

public record DatabaseServerListItem(
  string Id,
  string Host,
  int Port,
  string Name,
  string ConnectionStringFormat,
  bool IsDefault,
  DatabaseServerStatus Status);
