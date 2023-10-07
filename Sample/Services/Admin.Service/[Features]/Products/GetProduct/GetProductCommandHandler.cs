// using Agrio.PIM.Service.Data;

// using Microsoft.EntityFrameworkCore;

// namespace Agrio.PIM.Service.Features.Products.GetProduct;

// public sealed class GetProductCommandHandler
// 	(ServiceDbContext db) : ICommandHandler<GetProductCommand, GetProductResult> {
// 	public async Task<GetProductResult> ExecuteAsync(GetProductCommand command, CancellationToken ct) {
// 		var mapper = new Mapper();

// 		var product = await db.Products
// 			.AsNoTracking()
// 			.Where(t => t.Id == command.Id)
// 			.FirstOrDefaultAsync(ct) ?? throw new Exception("Product not found");
// 		//TODO: Standardize exceptions

// 		var response = mapper.FromEntity(product);

// 		return response;
// 	}
// }
