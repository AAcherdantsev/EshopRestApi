using FastEndpoints;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Api.Endpoints.V1;

public class PostProductEndpoint : Endpoint<CreateProductRequest, ProductDto>
{
    public override void Configure()
    {
        Post("/products");
        Version(1);
        Description(b =>
        {
            b.Produces<ProductDto>(201);
            b.Produces(400);
        });
        AllowAnonymous();
    }

    public override Task<ProductDto> ExecuteAsync(CreateProductRequest req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
