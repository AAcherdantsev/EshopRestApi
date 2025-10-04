using FastEndpoints;
using Shop.Products.Application.Common;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Api.Endpoints.V2;

public class GetPagedProductListEndpoint : Endpoint<GetPagedProductListRequest, PagedList<ProductDto>>
{
    public override void Configure()
    {
        Get("/products");
        Version(2);
        Description(b =>
        {
            b.Produces(200);
            b.Produces(404);
        });
        AllowAnonymous();
    }
    
    public override Task<PagedList<ProductDto>> ExecuteAsync(GetPagedProductListRequest req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}