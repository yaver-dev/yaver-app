using System.Linq;
using System.Text.Json;

using Serilog;

namespace Api;

public class RequestLogger : IGlobalPreProcessor {
  public async Task PreProcessAsync(IPreProcessorContext ctx, CancellationToken ct) {
    await Task.CompletedTask;
    // var logger = ctx.HttpContext.Resolve<ILogger>();

    Log.Information(
      $"request:{ctx.Request.GetType().FullName} path: {ctx.HttpContext.Request.Path}"
    );

    // var logger = ctx.RequestServices.GetRequiredService<ILogger>();
    // logger.LogInformation($"request:{req?.GetType().FullName} path: {ctx.Request.Path}");

    var userInfo = ctx.HttpContext.Request.Headers["X-UserInfo"].FirstOrDefault();
    // if (userInfo == null) {
    // 	failures.Add(new("MissingHeaders", "The [X-UserId] header needs to be set!"));
    // 	await ctx.Response.SendErrorsAsync(failures, cancellation: ct); //sending response here
    // 	return;
    // }


    Console.WriteLine("Headers:------------------");
    Console.WriteLine(JsonSerializer.Serialize(ctx.HttpContext.Request.Headers,
      new JsonSerializerOptions { WriteIndented = true })
    );
    Console.WriteLine("------------------");
    Console.WriteLine(
      $"roles: {JsonSerializer.Serialize(ctx.HttpContext.User?.Claims.Where(c => c.Type == "roles").Select(c => c.Value).ToList())}"
    );
    Console.WriteLine("------------------");
    Console.WriteLine($"request:{ctx.HttpContext.Request?.GetType().FullName} path: {ctx.HttpContext.Request.Path}");
    Console.WriteLine("------------------");
  }
}
