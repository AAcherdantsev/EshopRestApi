using FastEndpoints;
using Shop.Products.Application.Common;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Application.Dto.Requests;

namespace Shop.Products.Api.Endpoints.V2;

public class GetPagedProductListEndpoint : Endpoint<GetPagedProductListRequest, PagedList<ProductDto>>
{
    /// <inheritdoc/>
    public override void Configure()
    {
        Get("/products");
        Version(2);

        Summary(s =>
        {
            s.Summary = "Get paged list of products";
            s.Description = "Returns a paginated list of products with specified page number and page size.";
            s.Params["pageNumber"] = "Current page number.";
            s.Params["pageSize"] = "Number of products per page.";
            s.Response<PagedList<ProductDto>>(200, "Paged list of products retrieved successfully");
            s.Response(404, "No products found");
            s.Response(500, "Internal server error");
        });
        
        AllowAnonymous();
    }
    
    /// <inheritdoc/>
    public override Task<PagedList<ProductDto>> ExecuteAsync(GetPagedProductListRequest req, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}