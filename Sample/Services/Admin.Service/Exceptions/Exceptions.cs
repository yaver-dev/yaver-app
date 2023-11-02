using Grpc.Core;

namespace Admin.Service.Exceptions;

public class Exceptions : Exception {
  public RpcException EmptyGuid(string serviceName) {
    return new RpcException(new Status(StatusCode.InvalidArgument, $"{serviceName.ToLower()} id must be guid"));
  }

  public RpcException NotFound(string serviceName) {
    return new RpcException(new Status(StatusCode.NotFound, $"{serviceName.ToLower()} not found"));
  }
}
