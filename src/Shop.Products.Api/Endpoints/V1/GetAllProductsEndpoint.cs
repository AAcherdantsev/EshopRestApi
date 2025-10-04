using FastEndpoints;
using Shop.Products.Application.Dto.Products;

namespace Shop.Products.Api.Endpoints.V1;

public class GetAllProductsEndpoint : EndpointWithoutRequest<List<ProductDto>>
{
    public override void Configure()
    {
        Get("/products");
        Version(1);
        Description(b =>
        {
            b.Produces(200);
            b.Produces(404);
        });
        
        AllowAnonymous();
    }
    
    public override Task<List<ProductDto>> ExecuteAsync( CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}