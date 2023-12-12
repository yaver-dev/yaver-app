using System.Reflection;

using Admin.Service.Data;

using FluentValidation;

using Yaver.App;

var builder = WebApplication.CreateBuilder(args);
builder.AddYaverLogger();

// Accept only HTTP/2 to allow insecure connections for development.
// builder.WebHost.ConfigureKestrel(o => o.ListenAnyIP(6000, c => c.Protocols = HttpProtocols.Http2));
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


builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

//from lib
// builder.Services.AddRpcCommandHandlers();
// builder.Services.AddValidator<IdRequestValidator>();

var app = builder.Build();
app.UseRequestLocalization();

// app.MapRpcCommandHandlers();
app.RegisterRpcCommandHandlers();

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
      Tenant: ""
    )
  )
);


context.Database.EnsureCreated();

app.Run();
