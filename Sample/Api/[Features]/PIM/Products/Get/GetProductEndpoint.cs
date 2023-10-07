// using Agrio.Bo.PIM.ApiBase.Models;
// using Agrio.Bo.PIM.ApiBase.Tags.Products;
// using Agrio.PIM.ServiceBase.Features.Products.GetProduct;

// using Grpc.Core;

// using Microsoft.AspNetCore.Http.HttpResults;

// namespace Api.Features.PIM.Products.Get;

// public class GetProductEndpoint : GetProductEndpointBase<Mapper> {
// 	public override async Task<Results<Ok<GetProductResponse>, NotFound, ProblemDetails>> ExecuteAsync(
// 		GetProductRequest req, CancellationToken ct) {
// 		var result = await new GetProductCommand {Id = req.Id}
// 			.RemoteExecuteAsync();
// 		//TODO: Pass CancellationToken


// 		var response = Map.FromEntity(result);
// 		return TypedResults.Ok(response);
// 	}
// }
