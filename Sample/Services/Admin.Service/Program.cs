using System.Reflection;

using Admin.Service.Data;

using FluentValidation;

using Microsoft.Extensions.Localization;

using Yaver.App;

var builder = WebApplication.CreateSlimBuilder(args);
// builder.AddYaverLogger();

// Accept only HTTP/2 to allow insecure connections for development.
// builder.WebHost.ConfigureKestrel(o => o.ListenAnyIP(6000, c => c.Protocols = HttpProtocols.Http2));
builder.AddHandlerServer(options => { options.Interceptors.Add<ServerFeaturesInterceptor>(); });

builder.Services.AddScoped<IRequestMetadata, RequestMetadata>();
builder.Services.AddScoped<IAuditMetadata, AuditMetadata>();

builder.Services.AddDbContext<ServiceDbContext>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options => {
  var supportedCultures = new[] { "en-UK", "tr" };
  options.SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
});
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
builder.Services.AddDistributedMemoryCache();


var app = builder.Build();
app.UseRequestLocalization();

app.RegisterRpcCommandHandlers();

var context = new ServiceDbContext();

context.Database.EnsureCreated();

app.Run();
