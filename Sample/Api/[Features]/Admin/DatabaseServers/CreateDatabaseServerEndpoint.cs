
using Admin.ApiBase.EndpointBase.CreateDatabaseServer;
using Admin.ApiBase.Model;
using Admin.ServiceBase.DatabaseServers;

using Grpc.Core;


using Yaver.App;

namespace YaverMinimalApi.Admin.DatabaseServers;

public class CreateDatabaseServerEndpoint(IYaverContext yaverContext)
  : CreateDatabaseServerEndpointBase<CreateDatabaseServerMapper> {

  public override async Task HandleAsync(CreateDatabaseServerRequest req, CancellationToken ct) {

    var callOptions = new CallOptions()
      .SetContext(yaverContext, ct);

    var result = await Map.ToCommand(req)
      .RemoteExecuteAsync(callOptions);

    var response = Map.ToResponse(result);

    await SendCreatedAtAsync<GetDatabaseServerEndpoint>(
      //TODO: convert  query param to route param
      // bu arada apisix vs derken tam url i bilmiyoruz burada :thinking:
      routeValues: response.Id,
      responseBody: response,
      cancellation: ct);
  }
}


public class CreateDatabaseServerMapper : YaverMapper<CreateDatabaseServerRequest, CreatedResponseModel, CreateDatabaseServerCommand, DatabaseServerResult> {

  public override CreateDatabaseServerCommand ToCommand(CreateDatabaseServerRequest r) => new(
      Host: r.DatabaseServerViewModel.Host,
      Port: r.DatabaseServerViewModel.Port,
      Name: r.DatabaseServerViewModel.Name,
      ConnectionStringFormat: r.DatabaseServerViewModel.ConnectionStringFormat,
      IsDefault: r.DatabaseServerViewModel.IsDefault,
      Status: DatabaseServerStatus.Available
    );

  public override CreatedResponseModel ToResponse(DatabaseServerResult r) => new() {
    Id = r.Id
  };
}
