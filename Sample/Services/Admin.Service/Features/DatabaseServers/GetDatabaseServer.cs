using Admin.Service.Data;
using Admin.ServiceBase.Features.DatabaseServers;

using Microsoft.EntityFrameworkCore;

using Yaver.App;
using Yaver.App.Result;

namespace Admin.Service.Features.DatabaseServers;

public static class GetDatabaseServer {
  private static readonly Func<ServiceDbContext, Guid, CancellationToken, Task<DatabaseServerResult?>>
    _getEntityForResultAsync =
      EF.CompileAsyncQuery((ServiceDbContext context, Guid id, CancellationToken ct) =>
        context.DatabaseServers
          .Select(x => new DatabaseServerResult(
            x.Id,
            x.Host,
            x.Port,
            x.Name,
            x.ConnectionStringFormat,
            x.IsDefault,
            x.Status
          ))
          .FirstOrDefault(c => c.Id == id));

  public sealed class Handler(ServiceDbContext db)
    : RpcCommandHandler<GetDatabaseServerCommand, Result<DatabaseServerResult>> {
    public override async Task<Result<DatabaseServerResult>> ExecuteAsync(
      GetDatabaseServerCommand command,
      CancellationToken ct) {
      var entity = await _getEntityForResultAsync(db, command.Id, ct);

      if (entity is null) {
        return Result<DatabaseServerResult>.NotFound("Requested Resource Not Found");
      }

      return Result<DatabaseServerResult>.Success(entity);
    }
  }
}
