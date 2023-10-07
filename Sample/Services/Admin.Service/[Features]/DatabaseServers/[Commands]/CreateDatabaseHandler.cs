using Admin.ServiceBase.DatabaseServers;
using Admin.Service.DatabaseServers.Entities;
using Admin.Service.Data;
namespace Admin.Service.DatabaseServers;
public sealed class CreateDatabaseHandler(ServiceDbContext db)
  : ICommandHandler<CreateDatabaseServerCommand, DatabaseServerResult>
{

  public async Task<DatabaseServerResult> ExecuteAsync(CreateDatabaseServerCommand command, CancellationToken ct)
  {

    var mapper = new Mapper();
    var entity = mapper.ToEntity(command);

    await db.DatabaseServers.AddAsync(entity, ct);
    await db.SaveChangesAsync(ct);

    var response = await db.DatabaseServers.FindAsync([entity.Id], cancellationToken: ct) is not null ? mapper.FromEntity(entity) : null!;
    return response;
  }

  internal class Mapper : Mapper<CreateDatabaseServerCommand, DatabaseServerResult, DatabaseServer>
  {
    public override DatabaseServer ToEntity(CreateDatabaseServerCommand r) => new(
         host: r.Host,
         port: r.Port,
         name: r.Name,
         connectionStringFormat: r.ConnectionStringFormat,
         isDefault: r.IsDefault,
         status: r.Status
       );

    public override DatabaseServerResult FromEntity(DatabaseServer e) => new(
        e.Id,
        e.Host,
        e.Port,
        e.Name,
        e.ConnectionStringFormat,
        e.IsDefault,
        e.Status
      );
  }
}
