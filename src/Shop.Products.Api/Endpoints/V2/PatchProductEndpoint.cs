using FastEndpoints;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Api.Endpoints.V2;

public class PatchProductEndpoint : Endpoint<PatchProductRequest, ProductDto>
{
    public override void Configure()
    {
        Patch("/products/{id:int}");
        Version(2);
        
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