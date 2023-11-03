using Admin.Service.Data;
using Admin.ServiceBase.Features.DatabaseServers;

using Microsoft.EntityFrameworkCore;

using Yaver.App.Result;

namespace Admin.Service.Features.DatabaseServers;

public static class DeleteDatabaseServer {
  public sealed class Handler(ServiceDbContext db)
    : ICommandHandler<DeleteDatabaseServerCommand, Result> {
    public async Task<Result> ExecuteAsync(
      DeleteDatabaseServerCommand command,
      CancellationToken ct
    ) {
      // var deleted = await db.DatabaseServers
      //   .Where(x => x.Id == command.Id)
      //   .ExecuteDeleteAsync(ct);

      var deleted = 0;
      if (deleted == 0) {
        return Result.NotFound("Requested Resource Not Found");
      } else {
        return Result.Success();
      }
    }
  }
}
