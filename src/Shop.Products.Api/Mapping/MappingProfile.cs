using AutoMapper;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Domain.Entities;

namespace Shop.Products.Api.Mapping;

/// <summary>
/// <b>MappingProfile</b> is a configuration class for <b>AutoMapper</b> that defines the object mappings
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MappingProfile"/> class.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<ProductDto, Product>();
        CreateMap<Product, ProductDto>();
    }
}