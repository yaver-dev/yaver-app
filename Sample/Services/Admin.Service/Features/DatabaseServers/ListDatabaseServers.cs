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

      //TODO: where fields hard coded, proper implementation needs to be done
      var (count, data) = await _db.DatabaseServers
        .PaginateAsync(command.Term, command.Sort, command.Offset, command.Limit);

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
