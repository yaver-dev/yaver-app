using Admin.ApiBase.EndpointBase.DeleteDatabaseServer;
using Admin.ApiBase.Model;
using Admin.ServiceBase.Features.DatabaseServers;

using Grpc.Core;

using Yaver.App;

namespace Api.Features.Admin.DatabaseServers;

public static class DeleteDatabaseServer {
  public sealed class Endpoint(IYaverContext yaverContext)
    : DeleteDatabaseServerEndpointBase<Mapper> {
    public override async Task HandleAsync(DeleteDatabaseServerRequest req, CancellationToken ct) {
      var callOptions = new CallOptions()
        .SetContext(yaverContext, ct);

      var result = await Map.ToCommand(req)
        .RemoteExecuteAsync(callOptions);

      await SendOkAsync(Map.ToResponse(result), ct);
    }
  }

  //TODO: public cunkuuuuu bkz: line 16
  public sealed class Mapper
    : YaverMapper<DeleteDatabaseServerRequest, DatabaseServerViewModel, DeleteDatabaseServerCommand,
      DeleteDatabaseServerResult> {
    public override DeleteDatabaseServerCommand ToCommand(DeleteDatabaseServerRequest r) {
      return new DeleteDatabaseServerCommand(r.Id);
    }

    public override DatabaseServerViewModel ToResponse(DeleteDatabaseServerResult r) {
      return new DatabaseServerViewModel { Id = r.Id };
    }
  }
}
