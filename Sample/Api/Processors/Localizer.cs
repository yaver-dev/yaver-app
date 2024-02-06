using System.Globalization;

namespace Api;

public class Localizer : IGlobalPreProcessor {
  public async Task PreProcessAsync(IPreProcessorContext ctx, CancellationToken ct) {
    await Task.CompletedTask;

    var cultureKey = ctx.HttpContext.Request.Headers["Accept-Language"];
    Console.WriteLine("cultureKey:------------------" + cultureKey);
    if (!string.IsNullOrEmpty(cultureKey)) {
      // if (DoesCultureExist(cultureKey)) {
      var culture = new CultureInfo(cultureKey);
      Thread.CurrentThread.CurrentCulture = culture;
      Thread.CurrentThread.CurrentUICulture = culture;
      // }
    }
  }
}
