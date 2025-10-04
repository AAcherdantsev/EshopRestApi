using FastEndpoints;
using Shop.Products.Application.Dto.Products;

namespace Shop.Products.Api.Endpoints.V1;

public class GetProductEndpoint : EndpointWithoutRequest<ProductDto>
{
    /// <inheritdoc/>
    public override void Configure()
    {
        Get("/products/{id:int}");
        Version(1);
        
        Summary(s =>
        {
            s.Summary = "Get product by ID";
            s.Description = "Returns a single product";
            s.Params["id"] = "Product ID";
            s.Response<ProductDto>(200, "Product found");
            s.Response(404, "Product not found");
            s.Response(500, "Internal server error");
        });
        
        AllowAnonymous();
    }
    
    /// <inheritdoc/>
    public override Task<ProductDto> ExecuteAsync(  CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}