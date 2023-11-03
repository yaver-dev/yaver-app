// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;

// using Agrio.PIM.Service.Data;
// using Agrio.PIM.ServiceBase.Features.Products.UpdateProduct;

// using Microsoft.EntityFrameworkCore;

// namespace Agrio.PIM.Service.Features.Products.UpdateProduct;

// public sealed class UpdateProductCommandHandler
// 	(ServiceDbContext db) : ICommandHandler<UpdateProductCommand, UpdateProductResult> {
// 	public async Task<UpdateProductResult> ExecuteAsync(UpdateProductCommand command, CancellationToken ct) {
// 		var mapper = new Mapper();

// 		var product = await db.Products
// 			.AsNoTracking()
// 			.Where(t => t.Id == command.Id)
// 			.FirstOrDefaultAsync(ct) ?? throw new Exception("Product not found");
// 		//TODO: Standardize exceptions


// 		product = mapper.UpdateEntity(command, product);
// 		db.Products.Update(product);
// 		await db.SaveChangesAsync(ct);

// 		var response = mapper.FromEntity(product);

// 		return response;
// 	}
// }


