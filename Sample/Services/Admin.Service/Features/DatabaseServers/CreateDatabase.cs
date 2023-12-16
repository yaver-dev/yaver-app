using Admin.Service.Data;
using Admin.Service.Features.DatabaseServers.Entities;
using Admin.ServiceBase.Features.DatabaseServers;

using FluentValidation;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

using Yaver.App;

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
      ServiceDbContext db,
      IValidator<CreateDatabaseServerCommand> validator,
       IStringLocalizer<Validator> localizer
    )
    : RpcCommandHandler<CreateDatabaseServerCommand, Result<DatabaseServerResult>> {
    public override async Task<Result<DatabaseServerResult>> ExecuteAsync(
      CreateDatabaseServerCommand command,
      CancellationToken ct
    ) {

      //validate model
      var modelValidation = validator.Validate(
        instance: command,
        options: o => o.IncludeRuleSets("Model"));

      if (!modelValidation.IsValid) {
        return Result.Invalid(modelValidation.AsErrors());
      }
      //validate app logic
      var businessValidation = await validator.ValidateAsync(
        instance: command,
        options: o => o.IncludeRuleSets("Business"),
        cancellation: ct);

      if (!businessValidation.IsValid) {
        return Result.Conflict(businessValidation.AsErrors());
      }

      var entity = command.ToEntity();

      await db.DatabaseServers.AddAsync(entity, ct);
      await db.SaveChangesAsync(ct);

      return (await _getEntityForResultAsync(db, entity.Id, ct))!;
    }
  }

  public sealed class Validator : AbstractValidator<CreateDatabaseServerCommand> {
    public Validator(ServiceDbContext context, IStringLocalizer<Validator> localizer) {
      RuleSet("Model", () => {
        RuleFor(x => x.Port)
        .NotNull()
        .NotEmpty()
        .GreaterThan(1024)
        .LessThan(100000);

        RuleFor(x => x.Name)
            .NotEmpty().MinimumLength(10);
      });

      RuleSet("Business", () => {
        RuleFor(x => x.Name)
          .MustAsync(async (name, ct) => {
            return !await context.DatabaseServers.AnyAsync(x => x.Name == name, ct);
          }).WithMessage(x => localizer["NameMustBeUnique"]);

      });
    }
  }
}
