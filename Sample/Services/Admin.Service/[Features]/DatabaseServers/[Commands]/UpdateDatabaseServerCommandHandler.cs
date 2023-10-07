using Admin.ServiceBase.DatabaseServers;
using Admin.Service.DatabaseServers.Entities;
using Admin.Service.Data;
using Microsoft.EntityFrameworkCore;
using Grpc.Core;

namespace Admin.Service.DatabaseServers;
public sealed class UpdateDatabaseServerCommandHandler(ServiceDbContext db)
  : ICommandHandler<UpdateDatabaseServerCommand, DatabaseServerResult> {
  public async Task<DatabaseServerResult> ExecuteAsync(UpdateDatabaseServerCommand cmd, CancellationToken ct) {
    var entity = await db
      .DatabaseServers
      .FirstOrDefaultAsync(r => r.Id == cmd.Id, cancellationToken: ct)
    ?? throw new RpcException(new Status(StatusCode.NotFound, $"databaseServer not found"));


    //TODO: EP de singleton ama burada degil
    var mapper = new Mapper();
    mapper.UpdateEntity(cmd, entity);

    await db.DatabaseServers.AddAsync(entity, ct);
    await db.SaveChangesAsync(ct);

    var response = await db.DatabaseServers.FindAsync([entity.Id], cancellationToken: ct) is not null ? mapper.FromEntity(entity) : null!;
    return response;
  }

  internal class Mapper : Mapper<UpdateDatabaseServerCommand, DatabaseServerResult, DatabaseServer> {
    public override DatabaseServerResult FromEntity(DatabaseServer e) => new(
       e.Id,
       e.Host,
       e.Port,
       e.Name,
       e.ConnectionStringFormat,
       e.IsDefault,
       e.Status
     );

    public override DatabaseServer UpdateEntity(UpdateDatabaseServerCommand r, DatabaseServer e) {
      e.Host = r.Host;
      e.Port = r.Port;
      e.Name = r.Name;
      e.ConnectionStringFormat = r.ConnectionStringFormat;
      e.IsDefault = r.IsDefault;
      e.Status = r.Status;

      return e;
    }
  }
}
