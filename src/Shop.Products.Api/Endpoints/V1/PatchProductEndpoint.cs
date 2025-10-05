using FastEndpoints;
using Shop.Products.Api.Endpoints.Processors;
using Shop.Products.Application.Common.Repositories;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Application.Dto.Requests;
using Shop.Products.Infrastructure.Errors;
using IMapper = AutoMapper.IMapper;

namespace Shop.Products.Api.Endpoints.V1;

/// <summary>
///     Endpoint for updating specific product fields by ID.
/// </summary>
public class PatchProductEndpoint : Endpoint<PatchProductRequest, ProductDto>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;

    /// <summary>
    ///     Initializes a new instance of the <see cref="PatchProductEndpoint" /> class.
    /// </summary>
    /// <param name="productRepository">The repository used to access and update product data.</param>
    /// <param name="mapper">The AutoMapper instance for mapping entities to DTOs.</param>
    public PatchProductEndpoint(IProductRepository productRepository, IMapper mapper)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }

    /// <summary>
    ///     Configures the endpoint route, version, Swagger documentation, pre-processors, and access.
    /// </summary>
    public override void Configure()
    {
        Patch("/products/{id:int}");
        Version(1);

        Summary(s =>
        {
            s.Summary = "Update product by ID";
            s.Description = "Updates specific product fields by its ID.";
            s.Params["id"] = "Product ID";
            s.Response<ProductDto>(StatusCodes.Status200OK, "Product updated successfully");
            s.Response(StatusCodes.Status400BadRequest, "Invalid request data");
            s.Response(StatusCodes.Status404NotFound, "Product not found");
            s.Response(StatusCodes.Status500InternalServerError, "Internal server error");
        });

        PreProcessors(new PatchProductProcessor());

        AllowAnonymous();
    }

    /// <summary>
    ///     Handles the PATCH request to update a product's quantity.
    /// </summary>
    /// <param name="req">The incoming patch request containing the new quantity.</param>
    /// <param name="ct">The cancellation token.</param>
    public override async Task HandleAsync(PatchProductRequest req, CancellationToken ct)
    {
        var productId = Route<int>("id");

        var result = await _productRepository.UpdateProductQuantityAsync(productId, req.NewQuantity, ct);

        if (result.IsSuccess)
        {
            await Send.ResponseAsync(_mapper.Map<ProductDto>(result.Value), cancellation: ct);
            return;
        }

        if (result.HasError<NotFoundError>())
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.ResponseAsync(null!, StatusCodes.Status400BadRequest, ct);
    }
}