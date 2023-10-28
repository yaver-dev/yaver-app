using System.Reflection;

using Admin.Service.Data;
using Admin.Service.DatabaseServers;

using Microsoft.AspNetCore.Server.Kestrel.Core;
using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using Yaver.App;
using Admin.ServiceBase.DatabaseServers;

var builder = WebApplication.CreateBuilder(args);

// Accept only HTTP/2 to allow insecure connections for development.
builder.WebHost.ConfigureKestrel(o => o.ListenAnyIP(6000, c => c.Protocols = HttpProtocols.Http2));
builder.AddHandlerServer(
  options => {
    // options.Interceptors.Add<ServerLogInterceptor>();
    options.Interceptors.Add<ServerFeaturesInterceptor>();
    // options.EnableMessageValidation();
  });

builder.Services.AddScoped<IYaverContext, YaverContext>();
builder.Services.AddDbContext<ServiceDbContext>();



builder.Services.AddHttpContextAccessor();

// add  all IMapper implementations in the assembly

var mappers = AppDomain.CurrentDomain.GetAssemblies()
           .SelectMany(s => s.GetTypes())
           .Where(t => t.GetInterfaces().Contains(typeof(IMapper)))
           .Where(t => !t.IsInterface && !t.IsAbstract);

foreach (var mapper in mappers) {
  // Console.WriteLine($"registered {mapper.ToString()}");
  builder.Services.AddSingleton(mapper, mapper);
}

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
// builder.Services.AddScoped<IValidator<CreateDatabaseServerCommand>, CreateDatabase.Validator>();



//from lib
// builder.Services.AddRpcCommandHandlers();
// builder.Services.AddValidator<IdRequestValidator>();

// builder.Services.AddDbContext<ServiceDbContext>(options => options.UseInMemoryDatabase("PIM"));

// builder.Services.AddGrpcValidation();

// builder.Services.AddSingleton<IValidatorErrorMessageHandler>(new CustomMessageHandler());

var app = builder.Build();


// app.MapRpcCommandHandlers();
app.MapDatabaseServersHandlers();


var rInfo = new RequestInfo(

UserId: Guid.NewGuid(),
AcceptLanguage: "",
RequestId: "",
UserName: "",
Email: "",
GivenName: "",
FamilyName: "",
Roles: [],
TenantIdentifier: ""
  );
// using (var scope = app.Services.CreateScope()) {
var context = new ServiceDbContext(app.Configuration, new YaverContext(rInfo));


context.Database.EnsureCreated();
// }

app.Run();
