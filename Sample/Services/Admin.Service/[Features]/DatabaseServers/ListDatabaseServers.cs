using System.Linq.Expressions;

using Admin.Service.Data;
using Admin.ServiceBase.DatabaseServers;
using Admin.Service.DatabaseServers.Entities;

using Yaver.Db;
using Yaver.App;
using Microsoft.EntityFrameworkCore;


namespace Admin.Service.DatabaseServers;
public static class ListDatabaseServers {
  public sealed class Handler(ServiceDbContext db, IServiceProvider serviceProvider)
        : ICommandHandler<ListDatabaseServersCommand, DatabaseServerListResult> {
    //(serviceProvider) {
    private readonly ServiceDbContext _db = db;

    public async Task<DatabaseServerListResult> ExecuteAsync(
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
