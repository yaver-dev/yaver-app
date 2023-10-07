// using Agrio.Bo.PIM.ApiBase.Models;
// using Agrio.PIM.ServiceBase.Features.Products.GetProducts;


// namespace Api.Features.PIM.Products.GetAll;

// public class Mapper : Mapper<GetProductsRequest, GetProductsResponse, GetProductsResult> {
// 	public override GetProductsResponse FromEntity(GetProductsResult e) {
// 		return new GetProductsResponse(
// 			e.TotalCount,
// 			e.Items.Select(i => new Agrio.Bo.PIM.ApiBase.Models.ProductListItem(
// 				i.Id,
// 				i.IsOutOfStock,
// 				i.Title,
// 				i.Quantity
// 			)).ToList()
// 		);
// 	}
// }
