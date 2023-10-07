using Admin.ServiceBase.DatabaseServers;
namespace Admin.Service.DatabaseServers;

public static partial class ModuleExtensions
{
  public static WebApplication MapDatabaseServersHandlers(this WebApplication app)
  {
    app.MapHandlers(h =>
    {
      h.Register<ListDatabaseServersCommand, ListDatabaseServersCommandHandler, DatabaseServerListResult>();
      h.Register<CreateDatabaseServerCommand, CreateDatabaseHandler, DatabaseServerResult>();
      h.Register<GetDatabaseServerCommand, GetDatabaseServerCommandHandler, DatabaseServerResult>();
      h.Register<UpdateDatabaseServerCommand, UpdateDatabaseServerCommandHandler, DatabaseServerResult>();
      h.Register<DeleteDatabaseServerCommand, DeleteDatabaseServerCommandHandler, DeleteDatabaseServerResult>();
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
