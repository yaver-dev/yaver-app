using Admin.ApiBase.EndpointBase.UpdateDatabaseServer;
using Admin.ApiBase.Model;
using Admin.ServiceBase.Features.DatabaseServers;

using Grpc.Core;

using Yaver.App;

namespace Api.Features.Admin.DatabaseServers;

public static class UpdateDatabaseServer {
  public sealed class Endpoint(IYaverContext yaverContext)
    : UpdateDatabaseServerEndpointBase<Mapper> {
    public override async Task HandleAsync(UpdateDatabaseServerRequest req, CancellationToken ct) {
      var callOptions = new CallOptions()
        .SetContext(yaverContext, ct);

      var result = await Map.ToCommand(req)
        .RemoteExecuteAsync(callOptions);

      if (result.IsSuccess) {
        await SendOkAsync(Map.ToResponse(result), ct);
      } else {
        await SendResultAsync(result.ToHttpResponse());
      }
    }
  }


  public sealed class Mapper
    : YaverMapper<UpdateDatabaseServerRequest, DatabaseServerViewModel, UpdateDatabaseServerCommand,
      DatabaseServerResult> {
    public override UpdateDatabaseServerCommand ToCommand(UpdateDatabaseServerRequest r) {
      return new UpdateDatabaseServerCommand(
        r.Id,
        r.DatabaseServerViewModel.Host,
        r.DatabaseServerViewModel.Port,
        r.DatabaseServerViewModel.Name,
        r.DatabaseServerViewModel.ConnectionStringFormat,
        r.DatabaseServerViewModel.IsDefault,
        DatabaseServerStatus.Available
      );
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
