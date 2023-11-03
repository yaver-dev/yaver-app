﻿using Admin.ServiceBase.Features.DatabaseServers;

using Yaver.App.Result;

namespace Admin.Service.Features.DatabaseServers;

public static class ModuleExtensions {
  public static WebApplication MapDatabaseServersHandlers(this WebApplication app) {
    app.MapHandlers(h => {
      h.Register<ListDatabaseServersCommand, ListDatabaseServers.Handler, Result<DatabaseServerListResult>>();
      h.Register<CreateDatabaseServerCommand, CreateDatabase.Handler, Result<DatabaseServerResult>>();
      h.Register<GetDatabaseServerCommand, GetDatabaseServer.Handler, Result<DatabaseServerResult>>();
      h.Register<UpdateDatabaseServerCommand, UpdateDatabaseServer.Handler, Result<DatabaseServerResult>>();
      h.Register<DeleteDatabaseServerCommand, DeleteDatabaseServer.Handler, Result<DeleteDatabaseServerResult>>();
    });

    return app;
  }
}