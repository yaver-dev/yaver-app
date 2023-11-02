using Admin.Service.Data;
using Admin.ServiceBase.DatabaseServers;

using Yaver.Db;
using Microsoft.EntityFrameworkCore;
using Yaver.App.Result;


namespace Admin.Service.DatabaseServers;
public static class ListDatabaseServers {
  public sealed class Handler(ServiceDbContext db)
        : ICommandHandler<ListDatabaseServersCommand, Result<DatabaseServerListResult>> {
    private readonly ServiceDbContext _db = db;

    public async Task<Result<DatabaseServerListResult>> ExecuteAsync(
      ListDatabaseServersCommand command,
      CancellationToken ct) {


      (var count, var data) = await _db.DatabaseServers
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
          .ToListAsync(cancellationToken: ct)
        );
    }
  }

}
