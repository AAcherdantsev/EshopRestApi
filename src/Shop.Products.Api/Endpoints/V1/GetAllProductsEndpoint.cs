using FastEndpoints;
using Shop.Products.Application.Common.Repositories;
using Shop.Products.Application.Dto.Products;
using IMapper = AutoMapper.IMapper;

namespace Shop.Products.Api.Endpoints.V1;

/// <summary>
///     Handles requests to retrieve all products.
/// </summary>
public class GetAllProductsEndpoint : EndpointWithoutRequest<List<ProductDto>>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="GetAllProductsEndpoint" /> class.
    /// </summary>
    /// <param name="productRepository">The repository used to access product data.</param>
    /// <param name="mapper">The AutoMapper instance for mapping entities to DTOs.</param>
    public GetAllProductsEndpoint(IProductRepository productRepository, IMapper mapper)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    /// <summary>
    ///     Configures the endpoint route, version, and Swagger documentation.
    /// </summary>
    public override void Configure()
    {
        Get("/products");
        Version(1);

        Summary(s =>
        {
            s.Summary = "Get all products";
            s.Description = "Returns a list of all available products.";
            s.Response<List<ProductDto>>(StatusCodes.Status200OK, "List of products retrieved successfully");
            s.Response(StatusCodes.Status500InternalServerError, "Internal server error");
        });

        AllowAnonymous();
    }

    /// <inheritdoc />
    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _productRepository.GetAllProductsAsync(ct);

        if (result.IsSuccess)
        {
            await Send.ResponseAsync(_mapper.Map<List<ProductDto>>(result.Value), cancellation: ct);
            return;
        }

        ThrowError(string.Join("; ", result.Errors), StatusCodes.Status400BadRequest);
    }
}