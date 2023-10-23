using Admin.Service.Data;
using Admin.Service.DatabaseServers;

using Microsoft.AspNetCore.Server.Kestrel.Core;

using Yaver.App;

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

//from lib
// builder.Services.AddValidator<IdRequestValidator>();

// builder.Services.AddDbContext<ServiceDbContext>(options => options.UseInMemoryDatabase("PIM"));

// builder.Services.AddGrpcValidation();

// builder.Services.AddSingleton<IValidatorErrorMessageHandler>(new CustomMessageHandler());

var app = builder.Build();


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
