using Admin.ServiceBase.DatabaseServers;
using Admin.Service.DatabaseServers.Entities;
using Admin.Service.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Grpc.Core;

namespace Admin.Service.DatabaseServers;
public static class CreateDatabase {
  public sealed class Handler(ServiceDbContext db, IValidator<CreateDatabaseServerCommand> validator)
    : ICommandHandler<CreateDatabaseServerCommand, DatabaseServerResult> {

    public async Task<DatabaseServerResult> ExecuteAsync(
      CreateDatabaseServerCommand command,
      CancellationToken ct) {

      //validate command
      var validationResult = await validator.ValidateAsync(command, ct);
      // throw grpc exeption with validation errors
      if (!validationResult.IsValid)
        throw new RpcException(new Status(StatusCode.InvalidArgument, JsonSerializer.Serialize(validationResult.Errors)));


      var entity = command.ToEntity();

      await db.DatabaseServers.AddAsync(entity, ct);
      await db.SaveChangesAsync(ct);

      var result = await _getEntityForResultAsync(db, entity.Id, ct) ??
          throw new Exception("Database server not found.");

      return result;
    }

  }

  public sealed class Validator : AbstractValidator<CreateDatabaseServerCommand> {
    public Validator(ServiceDbContext context) {

      RuleFor(x => x.Port)
        .NotNull().WithMessage("Port Number is required!")
        .NotEmpty().WithMessage("Port Number is required!")
        .GreaterThan(1024).WithMessage("Port Number must be greater than 0!")
        .LessThan(10000).WithMessage("Port Number must be greater than 0!");

      RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name is required.")
        .MustAsync(async (id, ct) => {
          return !await context.DatabaseServers.AnyAsync(x => x.Name == id, ct);
        }).WithMessage("Name Must be unique");
    }

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
