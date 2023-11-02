using Admin.ServiceBase.DatabaseServers;
using Admin.Service.DatabaseServers.Entities;
using Admin.Service.Data;
using Microsoft.EntityFrameworkCore;
using Yaver.App.Result;
using FluentValidation;

namespace Admin.Service.DatabaseServers;
public static class UpdateDatabaseServer {
  public sealed class Handler(
    ServiceDbContext db,
    IValidator<UpdateDatabaseServerCommand> validator)
    : ICommandHandler<UpdateDatabaseServerCommand, Result<DatabaseServerResult>> {
    public async Task<Result<DatabaseServerResult>> ExecuteAsync(UpdateDatabaseServerCommand command, CancellationToken ct) {

      var validationResult = validator.Validate(command);

      if (!validationResult.IsValid) {
        return Result<DatabaseServerResult>.Invalid(validationResult.AsErrors());
      }

      var entity = await _getEntityForUpdateAsync(db, command.Id, ct);

      if (entity is null) {
        return Result<DatabaseServerResult>.NotFound();
      }

      //validate command
      // return Result<DatabaseServerResult>.Invalid(validation.AsErrors());
      UpdateEntity(command, entity);

      db.DatabaseServers.Update(entity);
      await db.SaveChangesAsync(ct);

      //TODO: buraya napmak lazim?
      var result = await _getEntityForResultAsync(db, entity.Id, ct) ??
         throw new Exception("Database server not found.");
      return result;
    }
  }


  public sealed class Validator : AbstractValidator<UpdateDatabaseServerCommand> {
    public Validator(ServiceDbContext context) {

      RuleFor(x => x.Port)
        .NotNull()//.WithMessage("Port Number is required!")
        .NotEmpty()//.WithMessage("Port Number is required!")
        .GreaterThan(1024)//.WithMessage("Port Number must be greater than 0!")
        .LessThan(100000);//.WithMessage("Port Number must be greater than 0!");

      RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name is required.");
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
