using Admin.ServiceBase.DatabaseServers;
using Admin.Service.DatabaseServers.Entities;
using Admin.Service.Data;
using Yaver.App;
using Microsoft.EntityFrameworkCore;

namespace Admin.Service.DatabaseServers;
public sealed class GetDatabaseServerCommandHandler(ServiceDbContext db)
  : ICommandHandler<GetDatabaseServerCommand, DatabaseServerResult>
{
  public async Task<DatabaseServerResult> ExecuteAsync(GetDatabaseServerCommand command, CancellationToken ct)
  {
    var entity = await db
      .DatabaseServers
      .AsNoTracking()
      .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken: ct)
      ?? throw new Exceptions.Exceptions().NotFound("DatabaseServer");

    //map
    var mapper = new Mapper();

    return mapper.FromEntity(entity);
  }

  internal class Mapper : Mapper<GetDatabaseServerCommand, DatabaseServerResult, DatabaseServer>
  {
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
