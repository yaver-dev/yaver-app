// using Agrio.Bo.PIM.ApiBase.Models;
// using Agrio.Bo.PIM.ApiBase.Tags.Products;
// using Agrio.PIM.ServiceBase.Features.Products.UpdateProduct;

// using Microsoft.AspNetCore.Http.HttpResults;

// namespace Api.Features.PIM.Products.Update;

// public class UpdateProductEndpoint : UpdateProductEndpointBase<Mapper> {
// 	public override async Task HandleAsync(UpdateProductRequest req, CancellationToken ct) {
// 		var result = await new UpdateProductCommand {
// 			Id = req.Id,
// 			Title = req.Body.Title,
// 			IsOutOfStock = req.Body.IsOutOfStock,
// 			Quantity = req.Body.Quantity
// 		}
// .RemoteExecuteAsync();
// 		//TODO: Pass CancellationToken


// 		var response = Map.FromEntity(result);
// 		await SendAsync(response);
// 	}
// 	// public override async Task<Results<Ok<UpdateProductResponse>, NotFound, ProblemDetails>> ExecuteAsync(
// 	// 	UpdateProductRequest req,
// 	// 	CancellationToken ct) {

// 	// 	if (ValidationFailed) {
// 	// 		Console.WriteLine("ValidationFailed");
// 	// 	}
// 	// 	// ThrowError("creating a user did not go so well!", StatusCodes.Status418ImATeapot);

// 	// 	// if (req.Body.Quantity < 0)
// 	// 	// 	AddError(r => r.Body.Quantity, "must +");


// 	// 	ThrowIfAnyErrors(); // If there are errors, execution shouldn't go beyond this point


// 	// 	var result = await new UpdateProductCommand {
// 	// 		Id = req.Id,
// 	// 		Title = req.Body.Title,
// 	// 		IsOutOfStock = req.Body.IsOutOfStock,
// 	// 		Quantity = req.Body.Quantity
// 	// 	}
// 	// 		.RemoteExecuteAsync();
// 	// 	//TODO: Pass CancellationToken


// 	// 	var response = Map.FromEntity(result);
// 	// 	return TypedResults.Ok(response);
// 	// }
// }


