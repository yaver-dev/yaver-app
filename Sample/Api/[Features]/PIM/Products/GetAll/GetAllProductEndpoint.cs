// using Agrio.Bo.PIM.ApiBase.Models;
// using Agrio.Bo.PIM.ApiBase.Tags.Products;
// using Agrio.PIM.ServiceBase.Features.Products.GetProducts;

// using Microsoft.AspNetCore.Http.HttpResults;

// namespace Api.Features.PIM.Products.GetAll;

// public class GetAllProductEndpoint : GetAllProductEndpointBase<Mapper> {
// 	public override async Task HandleAsync(GetProductsRequest req, CancellationToken ct) {
// 		var result = await new GetProductsCommand {
// 			Term = req.Term,
// 			AcceptLanguage = req.AcceptLanguage,
// 			Limit = req.Limit,
// 			Offset = req.Offset,
// 			Sort = req.Sort
// 		}
// 			.RemoteExecuteAsync();
// 		//TODO: Pass CancellationToken


// 		var response = Map.FromEntity(result);
// 		await SendAsync(response: response, cancellation: ct);
// 		//  TypedResults.Ok(response);
// 	}
// 	// public override async Task<Results<Ok<GetProductsResponse>, ProblemDetails>> ExecuteAsync(GetProductsRequest req, CancellationToken ct) {
// 	// 	var result = await new GetProductsCommand {
// 	// 		Term = req.Term,
// 	// 		AcceptLanguage = req.AcceptLanguage,
// 	// 		Limit = req.Limit,
// 	// 		Offset = req.Offset,
// 	// 		Sort = req.Sort
// 	// 	}
// 	// 		.RemoteExecuteAsync();
// 	// 	//TODO: Pass CancellationToken


// 	// 	var response = Map.FromEntity(result);
// 	// 	return TypedResults.Ok(response);
// 	// }
// }
