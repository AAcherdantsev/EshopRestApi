using AutoMapper;
using Shop.Products.Application.Common;
using Shop.Products.Application.Dto.Products;
using Shop.Products.Domain.Entities;

namespace Shop.Products.Api.Mapping;

/// <summary>
///     <b>MappingProfile</b> is a configuration class for <b>AutoMapper</b> that defines the object mappings
/// </summary>
public class MappingProfile : Profile
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="MappingProfile" /> class.
    /// </summary>
    public MappingProfile()
    {
        CreateMap<ProductDto, Product>();
        CreateMap<Product, ProductDto>();
        CreateMap<PagedList<Product>, PagedList<ProductDto>>()
            .ConvertUsing((src, dest, context) =>
                new PagedList<ProductDto>(
                    context.Mapper.Map<List<ProductDto>>(src.Values),
                    src.TotalCount,
                    src.PageNumber,
                    src.PageSize));
    }
}