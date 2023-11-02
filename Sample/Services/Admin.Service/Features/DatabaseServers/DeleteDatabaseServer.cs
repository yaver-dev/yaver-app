using Admin.Service.Data;
using Admin.ServiceBase.Features.DatabaseServers;

using Microsoft.EntityFrameworkCore;

using Yaver.App.Result;

namespace Admin.Service.Features.DatabaseServers;

public static class DeleteDatabaseServer {
  public sealed class Handler(ServiceDbContext db)
    : ICommandHandler<DeleteDatabaseServerCommand, Result<DeleteDatabaseServerResult>> {
    public async Task<Result<DeleteDatabaseServerResult>> ExecuteAsync(
      DeleteDatabaseServerCommand command,
      CancellationToken ct
    ) {
      var deleted = await db.DatabaseServers
        .Where(x => x.Id == command.Id)
        .ExecuteDeleteAsync(ct);

      if (deleted == 0) {
        return Result<DeleteDatabaseServerResult>.NotFound();
      }

      //TODO: add support for void result or return bool result
      return new DeleteDatabaseServerResult(command.Id);
    }
  }
}
