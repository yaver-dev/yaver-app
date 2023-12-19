using Admin.ApiBase.EndpointBase.UpdateDatabaseServer;
using Admin.ApiBase.Model;
using Admin.ServiceBase.Features.DatabaseServers;

using Grpc.Core;

using Yaver.App;

namespace Api.Features.Admin.DatabaseServers;

public static class UpdateDatabaseServer {
  public sealed class Endpoint(IRequestMetadata requestMetadata)
    : UpdateDatabaseServerEndpointBase<Mapper> {
    public override async Task HandleAsync(UpdateDatabaseServerRequest req, CancellationToken ct) {
      var callOptions = new CallOptions()
        .WithCancellationToken(ct)
        .WithRequestMetadata(requestMetadata);

      var result = await Map.ToCommand(req)
        .RemoteExecuteAsync(callOptions);

      if (result.IsFailure) {
        await SendResultAsync(result.ToHttpResponse());
        return;
      }

      await SendNoContentAsync(ct);
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
