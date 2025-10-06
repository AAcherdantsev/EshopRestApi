using FastEndpoints;
using Shop.Products.Api.Endpoints.Processors;
using Shop.Products.Application.Common;
using Shop.Products.Application.Common.Repositories;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Application.Dto.Requests;
using IMapper = AutoMapper.IMapper;

namespace Shop.Products.Api.Endpoints.V2;

/// <summary>
///     Endpoint for retrieving a paginated list of products.
/// </summary>
public class GetPagedProductListEndpoint : Endpoint<GetPagedProductListRequest, PagedList<ProductDto>>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetPagedProductListEndpoint" /> class.
    /// </summary>
    /// <param name="productRepository">The repository used to retrieve product data.</param>
    /// <param name="mapper">The AutoMapper instance for mapping entities to DTOs.</param>
    public GetPagedProductListEndpoint(IProductRepository productRepository, IMapper mapper)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    /// <summary>
    ///     Configures the endpoint route, version, Swagger documentation, pre-processors, and access.
    /// </summary>
    public override void Configure()
    {
        Get("/products");
        Version(2);

        Summary(s =>
        {
            s.Summary = "Get paged list of products";
            s.Description = "Returns a paginated list of products with specified page number and page size.";
            s.Params["pageNumber"] =
                "Current page number. Set this value to -1 to get all products without pagination.";
            s.Params["pageSize"] = "Number of products per page";
            s.Response<PagedList<ProductDto>>(StatusCodes.Status200OK, "Paged list of products retrieved successfully");
            s.Response(StatusCodes.Status500InternalServerError, "Internal server error");
        });

        PreProcessors(new GetPagedProductListProcessor());

        AllowAnonymous();
    }

    /// <summary>
    ///     Handles the GET request to retrieve a paginated list of products.
    /// </summary>
    /// <param name="req">The request containing pagination parameters.</param>
    /// <param name="ct">The cancellation token.</param>
    public override async Task HandleAsync(GetPagedProductListRequest req, CancellationToken ct)
    {
        var result = await _productRepository.GetAllProductsAsync(req, ct);

        if (result.IsSuccess)
        {
            await Send.ResponseAsync(_mapper.Map<PagedList<ProductDto>>(result.Value), cancellation: ct);
            return;
        }

        await Send.NotFoundAsync(ct);
    }
}