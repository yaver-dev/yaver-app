﻿using System.Text.Json;

using Admin.ApiBase.EndpointBase.GetDatabaseServer;
using Admin.ApiBase.Model;
using Admin.ServiceBase.DatabaseServers;



using Grpc.Core;

using Yaver.App;

namespace YaverMinimalApi.Admin.DatabaseServers;
public static class GetDatabaseServer {
  public sealed class Endpoint(IYaverContext yaverContext)
      : GetDatabaseServerEndpointBase<Mapper> {

    public override async Task HandleAsync(GetDatabaseServerRequest req, CancellationToken ct) {

      var callOptions = new CallOptions()
        .SetContext(yaverContext, ct);

      var command = Map.ToCommand(req);
      var result = await command.RemoteExecuteAsync(callOptions);

      await SendOkAsync(Map.ToResponse(result), cancellation: ct);
    }
  }

  public sealed class Mapper
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
}
