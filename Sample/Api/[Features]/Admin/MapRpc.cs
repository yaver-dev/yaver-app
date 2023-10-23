using Admin.ServiceBase.DatabaseServers;



using Microsoft.AspNetCore.Builder;

using Yaver.App;

namespace YaverMinimalApi.Admin.DatabaseServers;

public static partial class ModuleExtensions {
  public static WebApplication MapAdminService(this WebApplication app, string serviceUrl) {
    app.MapRemote(serviceUrl, c => {
      c.ChannelOptions.MaxRetryAttempts = 3;
      c.Register<CreateDatabaseServerCommand, DatabaseServerResult>();
      c.Register<UpdateDatabaseServerCommand, DatabaseServerResult>();
      c.Register<ListDatabaseServersCommand, DatabaseServerListResult>();
      c.Register<GetDatabaseServerCommand, DatabaseServerResult>();
      c.Register<DeleteDatabaseServerCommand, DeleteDatabaseServerResult>();
    });

    return app;
  }
}
