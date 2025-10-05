using FastEndpoints;
using Shop.Products.Application.Common.Repositories;
using Shop.Products.Application.Dto.Products;
using IMapper = AutoMapper.IMapper;

namespace Shop.Products.Api.Endpoints.V1;

public class GetAllProductsEndpoint : EndpointWithoutRequest<List<ProductDto>>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    
    public GetAllProductsEndpoint(IProductRepository productRepository, IMapper mapper)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }
    
    /// <inheritdoc/>
    public override void Configure()
    {
        Get("/products");
        Version(1);
        
        Summary(s =>
        {
            s.Summary = "Get all products";
            s.Description = "Returns a list of all available products.";
            s.Response<List<ProductDto>>(200, "List of products retrieved successfully");
            s.Response(404, "No products found");
            s.Response(500, "Internal server error");
        });
        
        AllowAnonymous();
    }
    
    /// <inheritdoc/>
    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _productRepository.GetAllProductsAsync(ct);

        if (result.IsSuccess)
        {
            await Send.ResponseAsync(_mapper.Map<List<ProductDto>>(result.Value), cancellation: ct);
            return;
        }

        await Send.NotFoundAsync(ct);
    }
}