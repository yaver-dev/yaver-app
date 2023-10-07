using Admin.ApiBase.EndpointBase.UpdateDatabaseServer;
using Admin.ApiBase.Model;
using Admin.ServiceBase.DatabaseServers;

using Grpc.Core;

using Yaver.App;

namespace YaverMinimalApi.Admin.DatabaseServers;

public class UpdateDatabaseServerEndpoint(IYaverContext yaverContext)
  : UpdateDatabaseServerEndpointBase<UpdateDatabaseServerMapper> {

  public override async Task HandleAsync(UpdateDatabaseServerRequest req, CancellationToken ct) {

    var callOptions = new CallOptions()
      .SetContext(yaverContext, ct);

    var result = await Map.ToCommand(req)
      .RemoteExecuteAsync(callOptions);

    var response = Map.ToResponse(result);

    await SendOkAsync(
      response: response,
      cancellation: ct);
  }
}


public class UpdateDatabaseServerMapper
: YaverMapper<UpdateDatabaseServerRequest, DatabaseServerViewModel, UpdateDatabaseServerCommand, DatabaseServerResult> {
  public override UpdateDatabaseServerCommand ToCommand(UpdateDatabaseServerRequest r) => new(
      Id: r.DatabaseServerViewModel.Id,
      Host: r.DatabaseServerViewModel.Host,
      Port: r.DatabaseServerViewModel.Port,
      Name: r.DatabaseServerViewModel.Name,
      ConnectionStringFormat: r.DatabaseServerViewModel.ConnectionStringFormat,
      IsDefault: r.DatabaseServerViewModel.IsDefault,
      Status: DatabaseServerStatus.Available
    );

  public override DatabaseServerViewModel ToResponse(DatabaseServerResult r) => new() {
    Id = r.Id,
    Host = r.Host,
    Port = r.Port,
    Name = r.Name,
    ConnectionStringFormat = r.ConnectionStringFormat,
    IsDefault = r.IsDefault
  };
}
