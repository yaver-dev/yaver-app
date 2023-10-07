// using System.Collections.Generic;
// using System.Threading;
// using System.Threading.Tasks;

// using Agrio.PIM.Service.Data;
// using Agrio.PIM.ServiceBase.Features.Products.CreateProduct;

// using Grpc.Core;

// using Microsoft.AspNetCore.Http;

// namespace Agrio.PIM.Service.Features.Products.CreateProduct;

// public sealed class CreateProductHandler
// 	(ServiceDbContext db, IHttpContextAccessor accessor) : ICommandHandler<CreateProductCommand, CreateProductResult> {
// 	public async Task<CreateProductResult> ExecuteAsync(CreateProductCommand command, CancellationToken ct) {
// 		// Using TenantHttpFeature from ServerFeaturesInterceptor
// 		// var tenantContext = accessor.HttpContext!.Features.Get<TenantHttpFeature>()!.TenantContext;

// 		// Using Only Http Context
// 		var headers = accessor.HttpContext!.Request.Headers;
// 		headers.TryGetValue("y-tenant-identifier", out var tenantIdentifier);
// 		headers.TryGetValue("y-correlation-id", out var crId);

// 		// Throw RpcException with Metadata to send extra information to client
// 		// var status = new Status(StatusCode.NotFound, "falanca not found");
// 		//
// 		// var meta = new Metadata {{"extra", "bize yakismadi"}, {"tenant", tenantIdentifier}};
// 		//
// 		// throw new RpcException(status, meta);

// 		var mapper = new ProductMapper();
// 		var product = mapper.ToEntity(command);
// 		product.Description = $"{tenantIdentifier} - {crId}";
// 		db.Products.Add(product);
// 		await db.SaveChangesAsync(ct);

// 		var response = await db.Products.FindAsync(product.Id) is not null ? mapper.FromEntity(product) : null!;

// 		return response;
// 	}
// }
