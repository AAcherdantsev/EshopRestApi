using FastEndpoints;
using Shop.Products.Application.Dto.Products;

namespace Shop.Products.Api.Endpoints.V1;

public class GetAllProductsEndpoint : EndpointWithoutRequest<List<ProductDto>>
{
    /// <inheritdoc/>
    public override void Configure()
    {
        Get("/products");
        Version(1);
        
        Summary(s =>
        {
            s.Summary = "Get all products";
            s.Description = "Returns a list of all available products.";
            s.Response<List<ProductDto>>(200, "List of products retrieved successfully");
            s.Response(404, "No products found");
            s.Response(500, "Internal server error");
        });
        
        AllowAnonymous();
    }
    
    public override Task<List<ProductDto>> ExecuteAsync( CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}