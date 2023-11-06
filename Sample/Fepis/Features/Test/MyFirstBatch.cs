using System.Reflection;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fepis.Features.Test;

public class MyFirstBatch : ConsoleAppBase {
  public void Hello(
    [Option("n", "name of send user.")] string name,
    [Option("r", "repeat count.")] int repeat = 3
  ) {
    for (int i = 0; i < repeat; i++) {
      this.Context.Logger.LogInformation($"Hello My Batch from {name}");
    }
  }

  IOptions<MyConfig> config;
  ILogger<MyFirstBatch> logger;

  public MyFirstBatch(IOptions<MyConfig> config, ILogger<MyFirstBatch> logger) {
    this.config = config;
    this.logger = logger;
  }

  [Command("log")]
  public void LogWrite() {
    Context.Logger.LogTrace("t r a c e");
    Context.Logger.LogDebug("d e b u g");
    Context.Logger.LogInformation("i n f o");
    Context.Logger.LogCritical("c r i t i c a l");
    Context.Logger.LogWarning("w a r n");
    Context.Logger.LogError("e r r o r");
  }

  [Command("opt")]
  public void ShowOption() {
    Console.WriteLine(config.Value.Bar);
    Console.WriteLine(config.Value.Foo);
  }


  [Command("version", "yeah, go!")]
  public void ShowVersion() {
    var version = Assembly.GetExecutingAssembly()
      .GetCustomAttribute<AssemblyFileVersionAttribute>()
      .Version;
    Console.WriteLine(version);
  }

  [Command("escape")]
  public void UrlEscape([Option(0)] string input) {
    Console.WriteLine(Uri.EscapeDataString(input));
  }

  [Command("timer")]
  public async Task Timer([Option(0)] uint waitSeconds) {
    Console.WriteLine(waitSeconds + " seconds");
    while (waitSeconds != 0) {
      await Task.Delay(TimeSpan.FromSeconds(1), Context.CancellationToken);
      waitSeconds--;
      Console.WriteLine(waitSeconds + " seconds");
    }
  }
}
