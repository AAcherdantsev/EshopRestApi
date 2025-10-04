using FastEndpoints;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Api.Endpoints.V2;

public class PatchProductEndpoint : Endpoint<PatchProductRequest, ProductDto>
{
    /// <inheritdoc/>
    public override void Configure()
    {
        Patch("/products/{id:int}");
        Version(2);
        
        Summary(s =>
        {
            s.Summary = "Update product by ID";
            s.Description = "Updates specific product fields by its ID.";
            s.Params["id"] = "Product ID";
            s.Response<ProductDto>(200, "Product updated successfully");
            s.Response(400, "Invalid request data");
            s.Response(404, "Product not found");
            s.Response(500, "Internal server error");
        });
        
        AllowAnonymous();
    }
    
    /// <inheritdoc/>
    public override Task<ProductDto> ExecuteAsync(PatchProductRequest req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}