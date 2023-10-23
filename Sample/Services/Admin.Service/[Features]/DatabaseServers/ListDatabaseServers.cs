using System.Linq.Expressions;

using Admin.Service.Data;
using Admin.ServiceBase.DatabaseServers;
using Admin.Service.DatabaseServers.Entities;

using Yaver.Db;


namespace Admin.Service.DatabaseServers;
public static class ListDatabaseServers {
  public sealed class Handler(ServiceDbContext db)
    : ICommandHandler<ListDatabaseServersCommand, DatabaseServerListResult> {

    public async Task<DatabaseServerListResult> ExecuteAsync(
      ListDatabaseServersCommand command,
      CancellationToken ct) {
      #region filter

      Expression<Func<DatabaseServer, bool>>? filter = null;
      var sort = command.Sort;

      if (!string.IsNullOrWhiteSpace(command.Term)) {
        filter = LinqHelper.BuildFilter<DatabaseServer>(command.Term, [$"name", $"host"]);
      }

      #endregion

      #region sorting
      if (!string.IsNullOrWhiteSpace(sort)) {
        sort = LinqHelper.BuildSort(
            sort,
            new Dictionary<string, string> { { "name", "host" } }
        );
      }

      #endregion

      var paging = await db.DatabaseServers
          .PaginateAsync(filter, sort, command.Offset, command.Limit);

      //map
      var mapper = new Mapper();

      return mapper.FromEntity(paging);
    }

    internal class Mapper : Mapper<ListDatabaseServersCommand, DatabaseServerListResult, PaginationResult<DatabaseServer>> {
      public override DatabaseServerListResult FromEntity(PaginationResult<DatabaseServer> e) => new(
              Items: e.Items.Select(item => new DatabaseServerListItem(
                item.Id.ToString(),
                item.Host,
                item.Port,
                item.Name,
                item.ConnectionStringFormat,
                item.IsDefault,
                item.Status))
                .ToList(),
              TotalCount: e.Count);
    }
  }
}
