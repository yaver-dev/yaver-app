using System.Linq;
using System.Text.Json;

using Admin.ApiBase.EndpointBase.ListDatabaseServers;
using Admin.ApiBase.Model;
using Admin.ServiceBase.Features.DatabaseServers;

using Grpc.Core;

using Yaver.App;

namespace Api.Features.Admin.DatabaseServers;

public static class ListDatabaseServers {
  public sealed class Endpoint(IRequestMetadata requestMetadata, IAuditMetadata auditMetadata)
    : ListDatabaseServersEndpointBase<Mapper> {
    public override async Task HandleAsync(ListDatabaseServersRequest req, CancellationToken ct) {
      var callOptions = new CallOptions()
        .WithYaverMetadata(
          cancellationToken: ct,
          requestMetadata: requestMetadata,
          auditMetadata: auditMetadata
        );

      var command = Map.ToCommand(req);
      Console.WriteLine("command: " + JsonSerializer.Serialize(command));

      var result = await command
        .RemoteExecuteAsync(callOptions);

      if (result.IsSuccess) {
        await SendOkAsync(Map.ToResponse(result), ct);
      } else {
        await SendResultAsync(result.ToHttpResponse());
      }
    }
  }

  public sealed class Mapper
    : YaverMapper<ListDatabaseServersRequest, DatabaseServerListModel, ListDatabaseServersCommand,
      PagedResult<DatabaseServerListItem>> {
    public override ListDatabaseServersCommand ToCommand(ListDatabaseServersRequest r) {
      return new ListDatabaseServersCommand(
        Offset: r.Offset,
        Limit: r.Limit,
        Term: r.Term,
        Sort: r.Sort
      );
    }

    public override DatabaseServerListModel ToResponse(PagedResult<DatabaseServerListItem> r) {
      return new DatabaseServerListModel {
        Items = r.Value.Select(item => new DatabaseServerListItemModel {
          Id = Guid.Parse(item.Id),
          Host = item.Host,
          Port = item.Port,
          Name = item.Name,
          ConnectionStringFormat = item.ConnectionStringFormat,
          IsDefault = item.IsDefault
        }).ToList(),
        TotalCount = r.TotalCount
      };
    }
  }
}
