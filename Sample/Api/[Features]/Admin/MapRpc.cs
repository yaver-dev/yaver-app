using Admin.ServiceBase.DatabaseServers;

using Microsoft.AspNetCore.Builder;

using Yaver.App.Result;

namespace YaverMinimalApi.Admin.DatabaseServers;

public static partial class ModuleExtensions {
  public static WebApplication MapAdminService(this WebApplication app, string serviceUrl) {
    app.MapRemote(serviceUrl, c => {
      c.ChannelOptions.MaxRetryAttempts = 3;
      c.Register<CreateDatabaseServerCommand, Result<DatabaseServerResult>>();
      c.Register<UpdateDatabaseServerCommand, Result<DatabaseServerResult>>();
      c.Register<ListDatabaseServersCommand, Result<DatabaseServerListResult>>();
      c.Register<GetDatabaseServerCommand, Result<DatabaseServerResult>>();
      c.Register<DeleteDatabaseServerCommand, DeleteDatabaseServerResult>();
    });

    return app;
  }
}
