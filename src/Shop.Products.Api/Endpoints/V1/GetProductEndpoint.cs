using FastEndpoints;
using Shop.Products.Application.Common.Repositories;
using Shop.Products.Application.Dto.Products;
using IMapper = AutoMapper.IMapper;

namespace Shop.Products.Api.Endpoints.V1;

/// <summary>
///     Endpoint for retrieving a single product by its ID.
/// </summary>
public class GetProductEndpoint : EndpointWithoutRequest<ProductDto>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetProductEndpoint" /> class.
    /// </summary>
    /// <param name="productRepository">The repository used to access product data.</param>
    /// <param name="mapper">The AutoMapper instance for mapping entities to DTOs.</param>
    public GetProductEndpoint(IProductRepository productRepository, IMapper mapper)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    /// <summary>
    ///     Configures the route, version, and Swagger summary for the endpoint.
    /// </summary>
    /// <inheritdoc />
    public override void Configure()
    {
        Get("/products/{id:int}");
        Version(1);

        Summary(s =>
        {
            s.Summary = "Get product by ID";
            s.Description = "Returns a single product";
            s.Params["id"] = "Product ID";
            s.Response<ProductDto>(StatusCodes.Status200OK, "Product found");
            s.Response(StatusCodes.Status404NotFound, "Product not found");
            s.Response(StatusCodes.Status500InternalServerError, "Internal server error");
        });

        AllowAnonymous();
    }

    /// <inheritdoc />
    public override async Task HandleAsync(CancellationToken ct)
    {
        var productId = Route<int>("id");
        var result = await _productRepository.GetProductByIdAsync(productId, ct);

        if (result.IsSuccess)
        {
            await Send.ResponseAsync(_mapper.Map<ProductDto>(result.Value), cancellation: ct);
            return;
        }

        await Send.NotFoundAsync(ct);
    }
}