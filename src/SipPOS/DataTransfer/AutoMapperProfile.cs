using AutoMapper;
using SipPOS.DataTransfer.Entity;
using SipPOS.Models.Entity;
using SipPOS.Models.General;

namespace SipPOS.DataTransfer;

/// <summary>  
/// AutoMapper profile for mapping between models and data transfer objects.  
/// </summary>  
public class AutoMapperProfile : Profile
{
    /// <summary>  
    /// Initializes a new instance of the <see cref="AutoMapperProfile"/> class.  
    /// Configures the mappings between models and data transfer objects.  
    /// </summary>  
    public AutoMapperProfile()
    {
        CreateMap<SpecialOffer, SpecialOfferDto>();
        CreateMap<SpecialOfferDto, SpecialOffer>();
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>();
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, Category>();
        CreateMap<Pagination<SpecialOffer>, Pagination<SpecialOfferDto>>();
        CreateMap<Pagination<SpecialOfferDto>, Pagination<SpecialOffer>>();
        CreateMap<Pagination<Product>, Pagination<ProductDto>>();
        CreateMap<Pagination<ProductDto>, Pagination<Product>>();
        CreateMap<Pagination<Category>, Pagination<CategoryDto>>();
        CreateMap<Pagination<CategoryDto>, Pagination<Category>>();
    }
}
