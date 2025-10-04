using FastEndpoints;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Api.Endpoints.V1;

public class PatchProductEndpoint : Endpoint<PatchProductRequest, ProductDto>
{
    public override void Configure()
    {
        Patch("/products/{id:int}");
        Version(1);
        
        Description(b =>
        {
            b.Produces<ProductDto>(200);
            b.Produces(400);
        });
        AllowAnonymous();
    }
    
    public override Task<ProductDto> ExecuteAsync(PatchProductRequest req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}