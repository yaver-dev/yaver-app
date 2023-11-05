using Admin.ServiceBase.Features.DatabaseServers;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

using Yaver.App.Result;

namespace Api.Features.Admin;

public static class ModuleExtensions {
  public static WebApplication MapAdminService(this WebApplication app) {
    var serviceUrl = app.Configuration.GetSection("Services").GetValue<string>("ADMIN");

    app.MapRemote(serviceUrl, c => {
      c.ChannelOptions.MaxRetryAttempts = 3;
      c.Register<CreateDatabaseServerCommand, Result<DatabaseServerResult>>();
      c.Register<UpdateDatabaseServerCommand, Result<DatabaseServerResult>>();
      c.Register<ListDatabaseServersCommand, Result<DatabaseServerListResult>>();
      c.Register<GetDatabaseServerCommand, Result<DatabaseServerResult>>();
      c.Register<DeleteDatabaseServerCommand, Result>();
    });

    return app;
  }
}
