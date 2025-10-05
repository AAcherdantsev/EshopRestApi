using FastEndpoints;
using Shop.Products.Application.Common.Repositories;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Application.Dto.Requests;
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
            s.Response<ProductDto>(200, "Product updated successfully");
            s.Response(400, "Invalid request data");
            s.Response(404, "Product not found");
            s.Response(500, "Internal server error");
        });
        
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
        await Send.NotFoundAsync(ct);
    }
}