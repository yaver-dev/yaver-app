using Admin.ServiceBase.DatabaseServers;
using Admin.Service.DatabaseServers.Entities;
using Admin.Service.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Admin.Service.DatabaseServers;
public static class CreateDatabase {
  public sealed class Handler(ServiceDbContext db)
    : ICommandHandler<CreateDatabaseServerCommand, DatabaseServerResult> {

    public async Task<DatabaseServerResult> ExecuteAsync(
      CreateDatabaseServerCommand command,
      CancellationToken ct) {

      //validate command
      // return Result<DatabaseServerResult>.Invalid(validation.AsErrors());
      var entity = command.ToEntity();

      await db.DatabaseServers.AddAsync(entity, ct);
      await db.SaveChangesAsync(ct);

      var result = await _getEntityForResultAsync(db, entity.Id, ct) ??
          throw new Exception("Database server not found.");

      return result;
    }

  }

  public sealed class Validator : AbstractValidator<CreateDatabaseServerCommand> {

  }

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

  private static DatabaseServer ToEntity(this CreateDatabaseServerCommand r) => new(
    host: r.Host,
    port: r.Port,
    name: r.Name,
    connectionStringFormat: r.ConnectionStringFormat,
    isDefault: r.IsDefault,
    status: r.Status);
}
