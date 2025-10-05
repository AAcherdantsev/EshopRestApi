using FastEndpoints;
using Shop.Products.Api.Endpoints.Processors;
using Shop.Products.Application.Common.Repositories;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Application.Dto.Requests;
using Shop.Products.Infrastructure.Errors;
using IMapper = AutoMapper.IMapper;

namespace Shop.Products.Api.Endpoints.V1;

public class PatchProductEndpoint : Endpoint<PatchProductRequest, ProductDto>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    
    public PatchProductEndpoint(IProductRepository productRepository, IMapper mapper)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }
    
    /// <inheritdoc/>
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
    
    /// <inheritdoc/>
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