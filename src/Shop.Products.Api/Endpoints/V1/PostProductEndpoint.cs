using FastEndpoints;
using Shop.Products.Api.Endpoints.Processors;
using Shop.Products.Application.Common.Repositories;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Application.Dto.Requests;
using IMapper = AutoMapper.IMapper;

namespace Shop.Products.Api.Endpoints.V1;

public class PostProductEndpoint : Endpoint<CreateProductRequest, ProductDto>
{
    private readonly IMapper _mapper;
    private readonly IProductRepository _productRepository;
    
    public PostProductEndpoint(IProductRepository productRepository, IMapper mapper)
    {
        _mapper = mapper;
        _productRepository = productRepository;
    }
    
    /// <inheritdoc/>
    public override void Configure()
    {
        Post("/products");
        Version(1);
        
        Summary(s =>
        {
            s.Summary = "Create a new product";
            s.Description = "Creates a new product and returns the created product with its ID.";
            s.ExampleRequest = new CreateProductRequest
            {
                Price = 100,
                Quantity = 10,
                Name = "Phone",
                ImageUrl = "https://example.com/phone.jpg",
                Description = "A sample product for demonstration.",
            };
            s.Response<ProductDto>(201, "Product successfully created");
            s.Response(400, "Invalid request data");
            s.Response(500, "Internal server error");
        });
        
        PreProcessors(new CreateProductProcessor());

        AllowAnonymous();
    }

    /// <inheritdoc/>
    public override async Task HandleAsync(CreateProductRequest req, CancellationToken ct)
    {
        var result = await _productRepository.CreateProductAsync(req, ct);

        if (!result.IsSuccess)
        {
            await Send.ResponseAsync(null!, 400, ct);
            return;
        }

        var dto = _mapper.Map<ProductDto>(result.Value);
        await Send.ResponseAsync(dto, 201, ct);
    }
}
