namespace Admin.Service.Exceptions;

using Grpc.Core;

public class Exceptions : System.Exception
{
  public RpcException EmptyGuid(string serviceName)
  {
    return new RpcException(new Status(StatusCode.InvalidArgument, $"{serviceName.ToLower()} id must be guid"));
  }

  public RpcException NotFound(string serviceName)
  {
    return new RpcException(new Status(StatusCode.NotFound, $"{serviceName.ToLower()} not found"));
  }
}
