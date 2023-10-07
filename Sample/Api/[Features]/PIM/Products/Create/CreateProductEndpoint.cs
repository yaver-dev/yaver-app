// using System.Text.Json;

// using Agrio.Bo.PIM.ApiBase.Models;
// using Agrio.Bo.PIM.ApiBase.Tags.Products;
// using Agrio.PIM.ServiceBase.Features.Products.CreateProduct;

// using Grpc.Core;

// using Microsoft.AspNetCore.Http.HttpResults;

// namespace Api.Features.PIM.Products.Create;

// public class CreateProductEndpoint : CreateProductEndpointBase<Mapper> {
// 	public override async
// 		Task<Results<Created<CreateProductResponse>, BadRequest, ProblemDetails, Conflict, ProblemHttpResult,
// 			JsonHttpResult<object>>>
// 		ExecuteAsync(
// 			CreateProductRequest req,
// 			CancellationToken ct
// 		) {
// 		// return TypedResults.Problem(detail: "Garib", statusCode: 422, title: "Zor", type: null);
// 		// return TypedResults.Json<object>(data: new {msg = "Garib"}, statusCode: 406);

// 		var callOptions = new CallOptions()
// 			.WithHeaders(new Metadata {
// 				{"y-tenant-identifier", req.TenantIdentifier},
// 				{"y-accept-language", req.AcceptLanguage},
// 				{"y-user-id", req.UserId},
// 				{"y-correlation-id", Guid.NewGuid().ToString()}
// 			})
// 			.WithCancellationToken(ct);

// 		Console.WriteLine(JsonSerializer.Serialize(req));

// 		var result = await new CreateProductCommand {Title = req.Body.Title}.RemoteExecuteAsync(callOptions);

// 		var response = Map.FromEntity(result);

// 		return TypedResults.Created($"/products/{response.Id}", response);
// 	}
// }
