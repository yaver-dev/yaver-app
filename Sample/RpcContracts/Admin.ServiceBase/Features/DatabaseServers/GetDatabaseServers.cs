using Yaver.App;

namespace Admin.ServiceBase.Features.DatabaseServers;

public sealed record ListDatabaseServersCommand(
  int Offset,
  int Limit,
  string Term,
  string Sort
) : IRpcCommand<PagedResult<DatabaseServerListItem>>;

public sealed record DatabaseServerListItem(
  string Id,
  string Host,
  int Port,
  string Name,
  string ConnectionStringFormat,
  bool IsDefault,
  DatabaseServerStatus Status);
