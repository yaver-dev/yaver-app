using Admin.ApiBase.EndpointBase.GetDatabaseServer;
using Admin.ApiBase.Model;
using Admin.ServiceBase.DatabaseServers;

using Grpc.Core;

using Yaver.App;

namespace YaverMinimalApi.Admin.DatabaseServers;

public class GetDatabaseServerEndpoint(IYaverContext yaverContext)
    : GetDatabaseServerEndpointBase<GetDatabaseServerMapper> {

  public override async Task HandleAsync(GetDatabaseServerRequest req, CancellationToken ct) {

    var callOptions = new CallOptions()
      .SetContext(yaverContext, ct);

    var result = await Map.ToCommand(req)
      .RemoteExecuteAsync(callOptions);

    await SendOkAsync(Map.ToResponse(result), cancellation: ct);
  }
}

//TODO: public cunkuuuuu bkz: line 16
public class GetDatabaseServerMapper
  : YaverMapper<GetDatabaseServerRequest, DatabaseServerViewModel, GetDatabaseServerCommand, DatabaseServerResult> {

  public override GetDatabaseServerCommand ToCommand(GetDatabaseServerRequest r) => new(Id: r.Id);

  public override DatabaseServerViewModel ToResponse(DatabaseServerResult r) => new() {
    Id = r.Id,
    Host = r.Host,
    Port = r.Port,
    Name = r.Name,
    ConnectionStringFormat = r.ConnectionStringFormat,
    IsDefault = r.IsDefault
  };


}
