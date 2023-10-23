using Admin.ServiceBase.DatabaseServers;
using Admin.Service.DatabaseServers.Entities;
using Admin.Service.Data;
using FluentValidation;

namespace Admin.Service.DatabaseServers;
public static class CreateDatabase {
  public sealed class Handler(ServiceDbContext db)
    : ICommandHandler<CreateDatabaseServerCommand, DatabaseServerResult> {

    public async Task<DatabaseServerResult> ExecuteAsync(
      CreateDatabaseServerCommand command,
      CancellationToken ct) {

      //validate command
      // return Result<DatabaseServerResult>.Invalid(validation.AsErrors());
      var mapper = new Mapper();
      var entity = mapper.ToEntity(command);

      await db.DatabaseServers.AddAsync(entity, ct);
      await db.SaveChangesAsync(ct);

      var response = await db.DatabaseServers.FindAsync([entity.Id], cancellationToken: ct) is not null ? mapper.FromEntity(entity) : null!;
      return response;
    }



    public sealed class Validator : AbstractValidator<CreateDatabaseServerCommand> {

    }

    public sealed class Mapper : Mapper<CreateDatabaseServerCommand, DatabaseServerResult, DatabaseServer> {
      public override DatabaseServer ToEntity(CreateDatabaseServerCommand r) => new(
           host: r.Host,
           port: r.Port,
           name: r.Name,
           connectionStringFormat: r.ConnectionStringFormat,
           isDefault: r.IsDefault,
           status: r.Status
         );

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
