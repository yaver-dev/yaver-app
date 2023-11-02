// using Agrio.Bo.PIM.ApiBase.Models;
// using Agrio.Bo.PIM.ApiBase.Tags.Products;
// using Agrio.PIM.ServiceBase.Features.Products.UpdateProduct;

// namespace Api.Features.PIM.Products.Update;

// public class Mapper : Mapper<UpdateProductRequest, UpdateProductResponse, UpdateProductResult> {
// 	public override UpdateProductResponse FromEntity(UpdateProductResult e) {
// 		return new UpdateProductResponse(
// 			e.Id,
// 			e.IsOutOfStock,
// 			e.Title,
// 			e.Quantity
// 		);
// 	}

// 	// public override ProductEntity UpdateEntity(Request r, ProductEntity e)
// 	// {
// 	//     e.IsOutOfStock = r.IsOutOfStock;
// 	//     e.Title = r.Title;
// 	//     e.Quantity = r.Quantity;
// 	//     return e;
// 	// }
// }


