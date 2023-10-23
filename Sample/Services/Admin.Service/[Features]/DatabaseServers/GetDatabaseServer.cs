using Admin.ServiceBase.DatabaseServers;
using Admin.Service.DatabaseServers.Entities;
using Admin.Service.Data;
using Microsoft.EntityFrameworkCore;

using System.Text.Json;
using Yaver.App;

namespace Admin.Service.DatabaseServers;
public static class GetDatabaseServer {
  public sealed class Handler(ServiceDbContext db)
    : ICommandHandler<GetDatabaseServerCommand, DatabaseServerResult> {
    public async Task<DatabaseServerResult> ExecuteAsync(GetDatabaseServerCommand command,
      CancellationToken ct) {
      var entity = await db
        .DatabaseServers
        .AsNoTracking()
        .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: ct);

      Console.WriteLine($"entity: {entity}");
      // var x = RpcResult.Failure<DatabaseServerResult?>(new RpcError("not found", "not found"));
      // if (entity is null) return x;


      //map
      var mapper = new Mapper();

      return mapper.FromEntity(entity);
    }

    // private DatabaseServerResult NoContent() {
    //   return Result.NotFound();
    // }

    public sealed class Mapper : Mapper<GetDatabaseServerCommand, DatabaseServerResult, DatabaseServer> {
      public override DatabaseServerResult FromEntity(DatabaseServer e) => new(
        Id: e.Id,
        Host: e.Host,
        Port: e.Port,
        Name: e.Name,
        ConnectionStringFormat: e.ConnectionStringFormat,
        IsDefault: e.IsDefault,
        Status: e.Status
      );
    }
  }
}
