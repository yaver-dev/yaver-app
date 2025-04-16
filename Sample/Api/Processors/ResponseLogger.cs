
namespace Api;

public class ResponseLogger : IGlobalPostProcessor {
  public async Task PostProcessAsync(IPostProcessorContext ctx, CancellationToken ct) {
    await Task.CompletedTask;



  }
}
