using Admin.ApiBase.EndpointBase.DeleteDatabaseServer;
using Admin.ApiBase.Model;
using Admin.ServiceBase.Features.DatabaseServers;

using Grpc.Core;

using Yaver.App;

namespace Api.Features.Admin.DatabaseServers;

public static class DeleteDatabaseServer
{
  public sealed class Endpoint(IYaverContext yaverContext)
    : DeleteDatabaseServerEndpointBase<Mapper>
  {
    public override async Task HandleAsync(DeleteDatabaseServerRequest req, CancellationToken ct)
    {
      var callOptions = new CallOptions()
        .SetContext(yaverContext, ct);

      var result = await Map.ToCommand(req)
        .RemoteExecuteAsync(callOptions);

      if (result.IsSuccess)
      {
        await SendNoContentAsync(ct);
      }
      else
      {
        await SendResultAsync(result.ToHttpResponse());
      }
    }
  }

  public sealed class Mapper
    : YaverMapper<DeleteDatabaseServerRequest, DatabaseServerViewModel, DeleteDatabaseServerCommand,
      Result>
  {
    public override DeleteDatabaseServerCommand ToCommand(DeleteDatabaseServerRequest r)
    {
      return new DeleteDatabaseServerCommand(r.Id);
    }

  }
}
