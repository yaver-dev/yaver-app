// using Fepis.Features.Test;
//
// using Microsoft.Extensions.Logging;
// using Microsoft.Extensions.Options;
//
// namespace Fepis.Features.Admin;
//
// public class DatabaseServers : ConsoleAppBase {
//   
//   IOptions<MyConfig> config;
//   ILogger<Batch> logger;
//
//   public DatabaseServers(IOptions<MyConfig> config, ILogger<DatabaseServers> logger) {
//     this.config = config;
//     this.logger = logger;
//   }
//   
//   public void List(string msg) {
//     Console.WriteLine(msg);
//   }
//
//   public void Create([Option(0)] int x, [Option(1)] int y) {
//     Console.WriteLine((x + y).ToString());
//   }
//
//   public void Get([Option(0)] int id) {
//     Console.WriteLine(id.ToString());
//   }
//
//   public void Delete([Option(0)] int id) {
//     Console.WriteLine(id.ToString());
//   }
// }

using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

using Admin.ServiceBase.Features.DatabaseServers;

using FastEndpoints;

using Fepis.Features.Test;

using Grpc.Core;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Yaver.App;

[Command("database-servers")]
public class DatabaseServersApp : ConsoleAppBase, IAsyncDisposable {
  readonly ILogger<DatabaseServersApp> logger;

  // readonly MyDbContext dbContext;
  readonly IOptions<MyConfig> config;

  // you can get DI parameters.
  public DatabaseServersApp(ILogger<DatabaseServersApp> logger, IOptions<MyConfig> config) {
    //, MyDbContext dbContext) {
    this.logger = logger;
    // this.dbContext = dbContext;
    this.config = config;
  }

  [Command("list")]
  public async Task ListAsync() {
    const string UserInfo =
      "ewogICJleHAiOiAxNjcwNTgxNTcxLAogICJpYXQiOiAxNjcwNTgxMjcxLAogICJhdXRoX3RpbWUiOiAxNjcwNTgxMjcxLAogICJqdGkiOiAiNzgzZDRhNjUtMWVlYi00MTgxLTk1NDMtYjgxM2M3MmEyNjMxIiwKICAiaXNzIjogImh0dHBzOi8vYXV0aC5kZXYuYWdyaW8uY29tLnRyL3JlYWxtcy9tYWluIiwKICAiYXVkIjogImFjY291bnQiLAogICJzdWIiOiAiODg1NzY3NjQtOWEzNi00NjE0LWE3ZjQtYTYxZWIxOGJkMjA0IiwKICAidHlwIjogIkJlYXJlciIsCiAgImF6cCI6ICJwb3J0YWwtY2xpZW50IiwKICAibm9uY2UiOiAiZTkwOWVhMTctNGM0ZS00MDRiLTgwMGEtNTVkYTA5N2ZjMjdmIiwKICAic2Vzc2lvbl9zdGF0ZSI6ICJkYmNmMmUwZC1lMzQ4LTQyYWMtOTY3Mi01MGY2NjJjMjliNGMiLAogICJhY3IiOiAiMSIsCiAgImFsbG93ZWQtb3JpZ2lucyI6IFsKICAgICJodHRwczovL2IyYi5kZXYuYWdyaW8uY29tLnRyIiwKICAgICJodHRwOi8vbG9jYWxob3N0OjQyMDAiLAogICAgImh0dHBzOi8vcC5kZXYuYWdyaW8uY29tLnRyIgogIF0sCiAgInJlYWxtX2FjY2VzcyI6IHsKICAgICJyb2xlcyI6IFsKICAgICAgIngtbW5nIiwKICAgICAgIm9mZmxpbmVfYWNjZXNzIiwKICAgICAgInVtYV9hdXRob3JpemF0aW9uIiwKICAgICAgImRlZmF1bHQtcm9sZXMtbWFpbiIsCiAgICAgICJzYSIKICAgIF0KICB9LAogICJyZXNvdXJjZV9hY2Nlc3MiOiB7CiAgICAiYWNjb3VudCI6IHsKICAgICAgInJvbGVzIjogWwogICAgICAgICJtYW5hZ2UtYWNjb3VudCIsCiAgICAgICAgIm1hbmFnZS1hY2NvdW50LWxpbmtzIiwKICAgICAgICAidmlldy1wcm9maWxlIgogICAgICBdCiAgICB9CiAgfSwKICAicm9sZXMiOiBbImFkbWluIl0sCiAgInNjb3BlIjogIm9wZW5pZCBlbWFpbCBwcm9maWxlIiwKICAic2lkIjogImRiY2YyZTBkLWUzNDgtNDJhYy05NjcyLTUwZjY2MmMyOWI0YyIsCiAgInRlbmFudHMiOiB7CiAgICAiYWdyaW9wYXkiOiBbCiAgICAgICJzYSIsCiAgICAgICJmaW5hbmNlLXQiCiAgICBdLAogICAgImFncmlva29vcCI6IFsKICAgICAgIml5cy1kIiwKICAgICAgIml5cy1tIiwKICAgICAgInRhIgogICAgXQogIH0sCiAgImVtYWlsX3ZlcmlmaWVkIjogdHJ1ZSwKICAibmFtZSI6ICJHb3pkZSBUYW5yaWt1bHUiLAogICJwcmVmZXJyZWRfdXNlcm5hbWUiOiAiZ296ZGV0IiwKICAiZ2l2ZW5fbmFtZSI6ICJHb3pkZSIsCiAgImZhbWlseV9uYW1lIjogIlRhbnJpa3VsdSIsCiAgImVtYWlsIjogImdvemRlLnRhbnJpa3VsdUBhZ3Jpb2ZpbmFucy5jb20iCn0=";

    var yaverContext = GenerateYaverContext(UserInfo);

    var callOptions = new CallOptions().SetContext(yaverContext, new CancellationToken());

    var command = new ListDatabaseServersCommand(0, 50, "", "");

    var jso = new JsonSerializerOptions { WriteIndented = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    
    Console.WriteLine("command: " + JsonSerializer.Serialize(command, jso));
    var result = await command.RemoteExecuteAsync(callOptions);
    Console.WriteLine("result: " + JsonSerializer.Serialize(result.Value, jso));
  }

  // also allow defaultValue.
  [Command("insert")]
  public async Task InsertAsync(string value, int id = 0) {
    // insert into...
  }

  // support cleanup(IDisposable/IAsyncDisposable)
  public async ValueTask DisposeAsync() {
    // await dbContext.DisposeAsync();
  }

  private static YaverContext GenerateYaverContext(string xUserInfo) {
    var payload = JwtPayload.Base64UrlDeserialize(xUserInfo);
    var xRequestInfo = new RequestInfo(
      UserId: Guid.Parse(payload.Sub),
      AcceptLanguage: "tr",
      RequestId: "",
      UserName: payload.FirstOrDefault(p => p.Key == "preferred_username").Value?.ToString() ?? "",
      GivenName: payload.FirstOrDefault(p => p.Key == "given_name").Value?.ToString() ?? "",
      FamilyName: payload.FirstOrDefault(p => p.Key == "family_name").Value?.ToString() ?? "",
      Roles: new List<string> { "admin", "falan" }, //payload.FirstOrDefault(p => p.Key == "roles").Value?.ToString(),
      Email: payload.FirstOrDefault(p => p.Key == "email").Value?.ToString() ?? "",
      TenantIdentifier: "filan"
    );
    var context = new YaverContext(xRequestInfo);
    return context;
  }
}
