using Admin.ServiceBase.DatabaseServers;

using Yaver.App;
namespace Admin.Service.DatabaseServers;

public static partial class ModuleExtensions {
  public static WebApplication MapDatabaseServersHandlers(this WebApplication app) {
    app.MapHandlers(h => {
      h.Register<ListDatabaseServersCommand, ListDatabaseServers.Handler, DatabaseServerListResult>();
      h.Register<CreateDatabaseServerCommand, CreateDatabase.Handler, DatabaseServerResult>();
      h.Register<GetDatabaseServerCommand, GetDatabaseServer.Handler, DatabaseServerResult>();
      h.Register<UpdateDatabaseServerCommand, UpdateDatabaseServer.Handler, DatabaseServerResult>();
      h.Register<DeleteDatabaseServerCommand, DeleteDatabaseServer.Handler, DeleteDatabaseServerResult>();
      // 	h.Register<UpdateProductCommand, UpdateProductCommandHandler, UpdateProductResult>();
      // 	//https://github.dev/FastEndpoints/Remote-Procedure-Call-Demo
      // 	//https://github.dev/FastEndpoints/Remote-Procedure-Call-Demo
      // 	//h.RegisterServerStream<StatusStreamCommand, StatusUpdateHandler, StatusUpdate>();
      // 	//h.RegisterClientStream<CurrentPosition, PositionProgressHandler, ProgressReport>();
      // 	//h.RegisterEventHub<SomethingHappened>();
    });

    return app;
  }
}
