using Admin.ServiceBase.DatabaseServers;
using Admin.Service.Data;
using Microsoft.EntityFrameworkCore;
using Yaver.App.Result;

namespace Admin.Service.DatabaseServers;
public static class GetDatabaseServer {
  public sealed class Handler(ServiceDbContext db)
    : ICommandHandler<GetDatabaseServerCommand, Result<DatabaseServerResult>> {
    public async Task<Result<DatabaseServerResult>> ExecuteAsync(
      GetDatabaseServerCommand command,
      CancellationToken ct) {

      var entity = await _getEntityForResultAsync(db, command.Id, ct);

      if (entity is null) {
        return Result<DatabaseServerResult>.NotFound();
      }

      return entity;
    }
  }

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
}
