using Admin.ServiceBase.DatabaseServers;
using Admin.Service.Data;
using Microsoft.EntityFrameworkCore;

namespace Admin.Service.DatabaseServers;


public static class DeleteDatabaseServer {
  public sealed class Handler(ServiceDbContext db)
    : ICommandHandler<DeleteDatabaseServerCommand, DeleteDatabaseServerResult> {
    public async Task<DeleteDatabaseServerResult> ExecuteAsync(DeleteDatabaseServerCommand command, CancellationToken ct) {

      var deleted = await db.DatabaseServers
        .Where(x => x.Id == command.Id)
        .ExecuteDeleteAsync(cancellationToken: ct);

      if (deleted == 0) {
        throw new Exceptions.Exceptions().NotFound("DatabaseServer");
      }

      return new DeleteDatabaseServerResult(command.Id);
    }
  }
}
