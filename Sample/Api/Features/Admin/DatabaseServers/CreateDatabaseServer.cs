using Admin.ApiBase.EndpointBase.CreateDatabaseServer;
using Admin.ApiBase.Model;
using Admin.ServiceBase.Features.DatabaseServers;

using Grpc.Core;

using Yaver.App;

namespace Api.Features.Admin.DatabaseServers;

public static class CreateDatabaseServer {
  public sealed class Endpoint(IYaverContext yaverContext)//, ITenantContext tenantContext)
    : CreateDatabaseServerEndpointBase<Mapper> {
    public override async Task HandleAsync(CreateDatabaseServerRequest req, CancellationToken ct) {
      var callOptions = new CallOptions()
        .SetYaverContext(yaverContext, ct);
        // .SetTenantContext(tenantContext);

      var result = await Map.ToCommand(req)
        .RemoteExecuteAsync(callOptions);

      if (result.IsSuccess) {
        var response = Map.ToResponse(result);
        await SendCreatedAtAsync(
          endpointName: "CreateDatabaseServer",
          routeValues: new { id = response.Id },
          responseBody: Map.ToResponse(result),
          cancellation: ct
        );
      } else {
        await SendResultAsync(result.ToHttpResponse());
      }
    }
  }


  public sealed class Mapper : YaverMapper<CreateDatabaseServerRequest, CreatedResponseModel,
    CreateDatabaseServerCommand, DatabaseServerResult> {
    public override CreateDatabaseServerCommand ToCommand(CreateDatabaseServerRequest r) {
      return new CreateDatabaseServerCommand(
        r.DatabaseServerViewModel.Host,
        r.DatabaseServerViewModel.Port,
        r.DatabaseServerViewModel.Name,
        r.DatabaseServerViewModel.ConnectionStringFormat,
        r.DatabaseServerViewModel.IsDefault,
        DatabaseServerStatus.Available
      );
    }

    public override CreatedResponseModel ToResponse(DatabaseServerResult r) {
      return new CreatedResponseModel { Id = r.Id };
    }
  }
}
