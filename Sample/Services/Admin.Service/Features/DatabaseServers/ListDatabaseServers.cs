using Admin.Service.Data;
using Admin.ServiceBase.Features.DatabaseServers;

using Microsoft.EntityFrameworkCore;

using Yaver.App.Result;
using Yaver.Db;

namespace Admin.Service.Features.DatabaseServers;

public static class ListDatabaseServers {
  public sealed class Handler(ServiceDbContext db)
    : ICommandHandler<ListDatabaseServersCommand, Result<DatabaseServerListResult>> {
    private readonly ServiceDbContext _db = db;

    public async Task<Result<DatabaseServerListResult>> ExecuteAsync(
      ListDatabaseServersCommand command,
      CancellationToken ct) {

      var (count, data) = await _db.DatabaseServers
        .PaginateAsync(
        searchTerm: command.Term,
        sort: command.Sort,
        offset: command.Offset,
        limit: command.Limit,
        searchFields: ["name", "host"]);

      return new DatabaseServerListResult(
        TotalCount: count,
        Items: await data.Select(x => new DatabaseServerListItem(
            x.Id.ToString(),
            x.Host,
            x.Port,
            x.Name,
            x.ConnectionStringFormat,
            x.IsDefault,
            x.Status))
          .ToListAsync(ct)
      );
    }
  }
}
