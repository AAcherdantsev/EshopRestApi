using FastEndpoints;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Api.Endpoints.V1;

public class PostProductEndpoint : Endpoint<CreateProductRequest, ProductDto>
{
    /// <inheritdoc/>
    public override void Configure()
    {
        Post("/products");
        Version(1);
        
        Summary(s =>
        {
            s.Summary = "Create a new product";
            s.Description = "Creates a new product and returns the created product with its ID.";
            s.ExampleRequest = new CreateProductRequest
            {
                Price = 100,
                Quantity = 10,
                Name = "Phone",
                ImageUrl = "https://example.com/phone.jpg",
                Description = "A sample product for demonstration.",
            };
            s.Response<ProductDto>(201, "Product successfully created");
            s.Response(400, "Invalid request data");
            s.Response(500, "Internal server error");
        });
        
        AllowAnonymous();
    }

    /// <inheritdoc/>
    public override Task<ProductDto> ExecuteAsync(CreateProductRequest req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
