using Admin.ApiBase.EndpointBase.GetDatabaseServer;
using Admin.ApiBase.Model;
using Admin.ServiceBase.Features.DatabaseServers;

using Grpc.Core;

using Yaver.App;

namespace Api.Features.Admin.DatabaseServers;

public static class GetDatabaseServer {
  public sealed class Endpoint(IYaverContext yaverContext)
    : GetDatabaseServerEndpointBase<Mapper> {
    public override async Task HandleAsync(GetDatabaseServerRequest req, CancellationToken ct) {
      var callOptions = new CallOptions()
        .SetYaverContext(yaverContext, ct);

      var command = Map.ToCommand(req);
      var result = await command.RemoteExecuteAsync(callOptions);

      if (result.IsSuccess) {
        await SendOkAsync(Map.ToResponse(result), ct);
      } else {
        await SendResultAsync(result.ToHttpResponse());
      }
    }
  }

  public sealed class Mapper
    : YaverMapper<GetDatabaseServerRequest, DatabaseServerViewModel, GetDatabaseServerCommand, DatabaseServerResult> {
    public override GetDatabaseServerCommand ToCommand(GetDatabaseServerRequest r) {
      return new GetDatabaseServerCommand(r.Id);
    }

    public override DatabaseServerViewModel ToResponse(DatabaseServerResult r) {
      return new DatabaseServerViewModel {
        Id = r.Id,
        Host = r.Host,
        Port = r.Port,
        Name = r.Name,
        ConnectionStringFormat = r.ConnectionStringFormat,
        IsDefault = r.IsDefault
      };
    }
  }
}
