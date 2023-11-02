

using Yaver.App.Result;

namespace Admin.ServiceBase.DatabaseServers;

public sealed record ListDatabaseServersCommand(
  int Offset,
  int Limit,
  string Term,
  string Sort
) : ICommand<Result<DatabaseServerListResult>>;

public sealed record DatabaseServerListResult(List<DatabaseServerListItem> Items, int? TotalCount = 0);

public sealed record DatabaseServerListItem(
  string Id,
  string Host,
  int Port,
  string Name,
  string ConnectionStringFormat,
  bool IsDefault,
  DatabaseServerStatus Status);
