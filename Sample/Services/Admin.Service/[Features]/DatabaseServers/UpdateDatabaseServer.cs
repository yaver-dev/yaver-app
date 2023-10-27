using Admin.ServiceBase.DatabaseServers;
using Admin.Service.DatabaseServers.Entities;
using Admin.Service.Data;
using Microsoft.EntityFrameworkCore;
using Grpc.Core;
using System.Text.Json;

namespace Admin.Service.DatabaseServers;
public static class UpdateDatabaseServer {
  public sealed class Handler(ServiceDbContext db)
    : ICommandHandler<UpdateDatabaseServerCommand, DatabaseServerResult> {
    public async Task<DatabaseServerResult> ExecuteAsync(UpdateDatabaseServerCommand command, CancellationToken ct) {

      var entity = await _getEntityForUpdateAsync(db, command.Id, ct) ??
         throw new Exception("Database server not found.");


      //validate command
      // return Result<DatabaseServerResult>.Invalid(validation.AsErrors());
      UpdateEntity(command, entity);

      db.DatabaseServers.Update(entity);
      await db.SaveChangesAsync(ct);

      var result = await _getEntityForResultAsync(db, entity.Id, ct) ??
         throw new Exception("Database server not found.");
      return result;
    }
  }


  private static readonly Func<ServiceDbContext, Guid, CancellationToken, Task<DatabaseServer?>> _getEntityForUpdateAsync =
     EF.CompileAsyncQuery((ServiceDbContext context, Guid id, CancellationToken ct) =>
       context.DatabaseServers
        .FirstOrDefault(c => c.Id == id));

  private static readonly Func<ServiceDbContext, Guid, CancellationToken, Task<DatabaseServerResult?>> _getEntityForResultAsync =
     EF.CompileAsyncQuery((ServiceDbContext context, Guid id, CancellationToken ct) =>
       context.DatabaseServers
       .AsNoTracking()
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

  private static DatabaseServer UpdateEntity(this UpdateDatabaseServerCommand r, DatabaseServer e) {
    e.Host = r.Host;
    e.Port = r.Port;
    e.Name = r.Name;
    e.ConnectionStringFormat = r.ConnectionStringFormat;
    e.IsDefault = r.IsDefault;
    e.Status = r.Status;
    return e;
  }
}
