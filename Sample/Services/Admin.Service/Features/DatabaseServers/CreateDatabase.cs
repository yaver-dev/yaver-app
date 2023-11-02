using Admin.Service.Data;
using Admin.Service.Features.DatabaseServers.Entities;
using Admin.ServiceBase.Features.DatabaseServers;

using FluentValidation;

using Microsoft.EntityFrameworkCore;

using Yaver.App;
using Yaver.App.Result;

namespace Admin.Service.Features.DatabaseServers;

public static class CreateDatabase {
  private static readonly Func<ServiceDbContext, Guid, CancellationToken, Task<DatabaseServerResult?>>
    _getEntityForResultAsync =
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

  private static DatabaseServer ToEntity(this CreateDatabaseServerCommand r) {
    return new DatabaseServer(
      host: r.Host,
      port: r.Port,
      name: r.Name,
      connectionStringFormat: r.ConnectionStringFormat,
      isDefault: r.IsDefault,
      status: r.Status);
  }

  public sealed class Handler(
      ServiceDbContext db, IValidator<CreateDatabaseServerCommand> validator)
    : RpcCommandHandler<CreateDatabaseServerCommand, Result<DatabaseServerResult>> {
    public override async Task<Result<DatabaseServerResult>> ExecuteAsync(
      CreateDatabaseServerCommand command,
      CancellationToken ct) {
      //validate command
      var validationResult = await validator.ValidateAsync(command, ct);

      if (!validationResult.IsValid) {
        return Result<DatabaseServerResult>.Invalid(validationResult.AsErrors());
      }

      var entity = command.ToEntity();

      await db.DatabaseServers.AddAsync(entity, ct);
      await db.SaveChangesAsync(ct);

      //TODO: buraya napmak lazim?
      var result = await _getEntityForResultAsync(db, entity.Id, ct) ??
                   throw new Exception("Database server not found.");

      return result;
    }
  }

  public sealed class Validator : AbstractValidator<CreateDatabaseServerCommand> {
    public Validator(ServiceDbContext context) {
      RuleFor(x => x.Port)
        .NotNull()
        .NotEmpty()
        .GreaterThan(1024)
        .LessThan(100000);

      RuleFor(x => x.Name)
        .NotEmpty().WithMessage("Name is required.")
        .MustAsync(async (id, ct) => {
          return !await context.DatabaseServers.AnyAsync(x => x.Name == id, ct);
        }).WithMessage("Name Must be unique");
    }
  }
}
