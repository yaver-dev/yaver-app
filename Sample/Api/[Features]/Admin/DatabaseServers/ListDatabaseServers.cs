using System.Linq;

using Admin.ApiBase.EndpointBase.ListDatabaseServers;
using Admin.ApiBase.Model;
using Admin.ServiceBase.DatabaseServers;

using Grpc.Core;

using Yaver.App;

namespace YaverMinimalApi.Admin.DatabaseServers;
public static class ListDatabaseServers {
  public sealed class Endpoint(IYaverContext yaverContext)
      : ListDatabaseServersEndpointBase<Mapper> {

    public override async Task HandleAsync(ListDatabaseServersRequest req, CancellationToken ct) {

      var callOptions = new CallOptions()
        .SetContext(yaverContext, ct);

      var result = await Map.ToCommand(req)
        .RemoteExecuteAsync(callOptions);

      await SendOkAsync(Map.ToResponse(result), cancellation: ct);
    }
  }

  public sealed class Mapper
  : YaverMapper<ListDatabaseServersRequest, DatabaseServerListModel, ListDatabaseServersCommand, DatabaseServerListResult> {

    public override ListDatabaseServersCommand ToCommand(ListDatabaseServersRequest r) => new(
        Offset: r.Offset,
        Limit: r.Limit,
        Term: r.Term,
        Sort: r.Sort
      );

    public override DatabaseServerListModel ToResponse(DatabaseServerListResult r) => new() {
      Items = r.Items.Select(item => new DatabaseServerListItemModel {
        Id = Guid.Parse(item.Id),
        Host = item.Host,
        Port = item.Port,
        Name = item.Name,
        ConnectionStringFormat = item.ConnectionStringFormat,
        IsDefault = item.IsDefault,
        // Status = item.Status // Add this line to fix the error
      }).ToList(),
      TotalCount = (int)r.TotalCount
    };

  }
}
