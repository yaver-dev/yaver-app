using System.IdentityModel.Tokens.Jwt;

using Admin.ServiceBase.Features.DatabaseServers;

using Fepis;
using Fepis.Features.Test;

using Grpc.Core;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Yaver.App;

using ZLogger;

//namespace Fepis;


var builder = ConsoleApp.CreateBuilder(args);
builder.ConfigureServices((ctx, services) => {
  // Register EntityFramework database context
  // services.AddDbContext<MyDbContext>();

  // Register appconfig.json to IOption<MyConfig>
  services.Configure<MyConfig>(ctx.Configuration);

  // Using Cysharp/ZLogger for logging to file
  services.AddLogging(logging => {
    // optional(MS.E.Logging):clear default providers.
    logging.ClearProviders();

    // optional(MS.E.Logging): default is Info, you can use this or AddFilter to filtering log.
    logging.SetMinimumLevel(LogLevel.Debug);

    // Add Console Logging.
    // logging.AddZLoggerConsole();

    // Add File Logging.
    logging.AddZLoggerFile("fepis.log", options => {
      options.EnableStructuredLogging = true;
    });

    // Add Rolling File Logging.
    // logging.AddZLoggerRollingFile((dt, x) => $"logs/{dt.ToLocalTime():yyyy-MM-dd}_{x:000}.log",
    //   x => x.ToLocalTime().Date, 1024);

    // Enable Structured Logging
    // logging.AddZLoggerConsole(options => {
    //   options.EnableStructuredLogging = true;
    // });
  });
});

var app = builder.Build();

// setup many command, async, short-name/description option, subcommand, DI
// app.AddCommand("calc-sum", (int x, int y) => Console.WriteLine(x + y));
// app.AddCommand("sleep", async ([Option("t", "seconds of sleep time.")] int time) =>
// {
//   await Task.Delay(TimeSpan.FromSeconds(time));
// });
// app.AddSubCommand("verb", "childverb", () => Console.WriteLine("called via 'verb childverb'"));

// You can insert all public methods as sub command => db select / db insert
// or AddCommand<T>() all public methods as command => select / insert
app.AddSubCommands<DatabaseServersApp>();

// some argument from DI.
app.AddRootCommand((ConsoleAppContext ctx, IOptions<MyConfig> config, string name) => { });

app.Host.MapRpcHandlers(
  "Admin.ServiceBase",
  app.Configuration.GetSection("Services").GetValue<string>("ADMIN")
);

app.Run();


// var builder = WebApplication.CreateBuilder(args);
// builder.Logging.ClearProviders();
// builder.Logging.SetMinimumLevel(LogLevel.Trace);
// builder.Logging.AddZLoggerFile("fepis.log", options => { options.EnableStructuredLogging = true; });
// builder.Logging.AddZLoggerRollingFile((dt, x) => $"logs/{dt.ToLocalTime():yyyy-MM-dd}_{x:000}.log", x => x.ToLocalTime().Date, 1024);
// builder.Logging.AddZLoggerConsole(options => { options.EnableStructuredLogging = true; });

// await builder.Host.RunConsoleAppFrameworkAsync(args);

// builder.Services.Configure<JsonOptions>(o =>
//   o.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

// builder.Services.AddScoped<IYaverContext, YaverContext>();
// var app = builder.Build();

// const string UserInfo =
//   "ewogICJleHAiOiAxNjcwNTgxNTcxLAogICJpYXQiOiAxNjcwNTgxMjcxLAogICJhdXRoX3RpbWUiOiAxNjcwNTgxMjcxLAogICJqdGkiOiAiNzgzZDRhNjUtMWVlYi00MTgxLTk1NDMtYjgxM2M3MmEyNjMxIiwKICAiaXNzIjogImh0dHBzOi8vYXV0aC5kZXYuYWdyaW8uY29tLnRyL3JlYWxtcy9tYWluIiwKICAiYXVkIjogImFjY291bnQiLAogICJzdWIiOiAiODg1NzY3NjQtOWEzNi00NjE0LWE3ZjQtYTYxZWIxOGJkMjA0IiwKICAidHlwIjogIkJlYXJlciIsCiAgImF6cCI6ICJwb3J0YWwtY2xpZW50IiwKICAibm9uY2UiOiAiZTkwOWVhMTctNGM0ZS00MDRiLTgwMGEtNTVkYTA5N2ZjMjdmIiwKICAic2Vzc2lvbl9zdGF0ZSI6ICJkYmNmMmUwZC1lMzQ4LTQyYWMtOTY3Mi01MGY2NjJjMjliNGMiLAogICJhY3IiOiAiMSIsCiAgImFsbG93ZWQtb3JpZ2lucyI6IFsKICAgICJodHRwczovL2IyYi5kZXYuYWdyaW8uY29tLnRyIiwKICAgICJodHRwOi8vbG9jYWxob3N0OjQyMDAiLAogICAgImh0dHBzOi8vcC5kZXYuYWdyaW8uY29tLnRyIgogIF0sCiAgInJlYWxtX2FjY2VzcyI6IHsKICAgICJyb2xlcyI6IFsKICAgICAgIngtbW5nIiwKICAgICAgIm9mZmxpbmVfYWNjZXNzIiwKICAgICAgInVtYV9hdXRob3JpemF0aW9uIiwKICAgICAgImRlZmF1bHQtcm9sZXMtbWFpbiIsCiAgICAgICJzYSIKICAgIF0KICB9LAogICJyZXNvdXJjZV9hY2Nlc3MiOiB7CiAgICAiYWNjb3VudCI6IHsKICAgICAgInJvbGVzIjogWwogICAgICAgICJtYW5hZ2UtYWNjb3VudCIsCiAgICAgICAgIm1hbmFnZS1hY2NvdW50LWxpbmtzIiwKICAgICAgICAidmlldy1wcm9maWxlIgogICAgICBdCiAgICB9CiAgfSwKICAicm9sZXMiOiBbImFkbWluIl0sCiAgInNjb3BlIjogIm9wZW5pZCBlbWFpbCBwcm9maWxlIiwKICAic2lkIjogImRiY2YyZTBkLWUzNDgtNDJhYy05NjcyLTUwZjY2MmMyOWI0YyIsCiAgInRlbmFudHMiOiB7CiAgICAiYWdyaW9wYXkiOiBbCiAgICAgICJzYSIsCiAgICAgICJmaW5hbmNlLXQiCiAgICBdLAogICAgImFncmlva29vcCI6IFsKICAgICAgIml5cy1kIiwKICAgICAgIml5cy1tIiwKICAgICAgInRhIgogICAgXQogIH0sCiAgImVtYWlsX3ZlcmlmaWVkIjogdHJ1ZSwKICAibmFtZSI6ICJHb3pkZSBUYW5yaWt1bHUiLAogICJwcmVmZXJyZWRfdXNlcm5hbWUiOiAiZ296ZGV0IiwKICAiZ2l2ZW5fbmFtZSI6ICJHb3pkZSIsCiAgImZhbWlseV9uYW1lIjogIlRhbnJpa3VsdSIsCiAgImVtYWlsIjogImdvemRlLnRhbnJpa3VsdUBhZ3Jpb2ZpbmFucy5jb20iCn0=";
//
// var yaverContext = GenerateYaverContext(UserInfo);
//
// var callOptions = new CallOptions().SetContext(yaverContext, new CancellationToken());
//
// var command = new ListDatabaseServersCommand(0, 50, "", "");
// Console.WriteLine("command: " + JsonSerializer.Serialize(command));

// builder.Host.MapAdminServices("http://localhost:6000");
