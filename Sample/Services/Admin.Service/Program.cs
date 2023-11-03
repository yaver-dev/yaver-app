using System.Reflection;

using Admin.Service.Data;
using Admin.Service.Features.DatabaseServers;

using FluentValidation;

using Microsoft.AspNetCore.Server.Kestrel.Core;

using Yaver.App;

var builder = WebApplication.CreateBuilder(args);

// Accept only HTTP/2 to allow insecure connections for development.
builder.WebHost.ConfigureKestrel(o => o.ListenAnyIP(6000, c => c.Protocols = HttpProtocols.Http2));
builder.AddHandlerServer(
  options => {
    // options.Interceptors.Add<ServerLogInterceptor>();
    options.Interceptors.Add<ServerFeaturesInterceptor>();
  });

builder.Services.AddScoped<IYaverContext, YaverContext>();
builder.Services.AddDbContext<ServiceDbContext>();


builder.Services.Configure<RequestLocalizationOptions>(options => {
  var supportedCultures = new[] { "tr", "en" };
  options.SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
});

builder.Services.AddHttpContextAccessor();

// add  all IMapper implementations in the assembly

// var mappers = AppDomain.CurrentDomain.GetAssemblies()
//            .SelectMany(s => s.GetTypes())
//            .Where(t => t.GetInterfaces().Contains(typeof(IMapper)))
//            .Where(t => !t.IsInterface && !t.IsAbstract);

// foreach (var mapper in mappers) {
//   // Console.WriteLine($"registered {mapper.ToString()}");
//   builder.Services.AddSingleton(mapper, mapper);
// }

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

//from lib
// builder.Services.AddRpcCommandHandlers();
// builder.Services.AddValidator<IdRequestValidator>();

var app = builder.Build();
app.UseRequestLocalization();

// app.MapRpcCommandHandlers();
app.MapDatabaseServersHandlers();

var context = new ServiceDbContext(app.Configuration,
  new YaverContext(
    new RequestInfo(
      UserId: Guid.NewGuid(),
      AcceptLanguage: "",
      RequestId: "",
      UserName: "",
      Email: "",
      GivenName: "",
      FamilyName: "",
      Roles: [],
      TenantIdentifier: ""
    )
  )
);


context.Database.EnsureCreated();

app.Run();
