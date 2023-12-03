using Admin.Service.Data;
using Admin.ServiceBase.Features.DatabaseServers;

using Yaver.App;

namespace Admin.Service.Features.DatabaseServers;

public static class DeleteDatabaseServer
{
  public sealed class Handler(ServiceDbContext db)
    : RpcCommandHandler<DeleteDatabaseServerCommand, Result>
  {
    public override async Task<Result> ExecuteAsync(
      DeleteDatabaseServerCommand command,
      CancellationToken ct
    )
    {
      // var deleted = await db.DatabaseServers
      //   .Where(x => x.Id == command.Id)
      //   .ExecuteDeleteAsync(ct);

      var deleted = 0;
      if (deleted == 0)
      {
        return Result.NotFound("Requested Resource Not Found");
      }
      else
      {
        return Result.Success();
      }
    }
  }
}
