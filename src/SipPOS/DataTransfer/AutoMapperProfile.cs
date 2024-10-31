using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SipPOS.Models;

namespace SipPOS.DataTransfer;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<ProductDto, Product>();
        CreateMap<Category, CategoryDto>();
        CreateMap<CategoryDto, Category>();
        CreateMap<Pagination<Product>, Pagination<ProductDto>>();
        CreateMap<Pagination<ProductDto>, Pagination<Product>>();
        CreateMap<Pagination<Category>, Pagination<CategoryDto>>();
        CreateMap<Pagination<CategoryDto>, Pagination<Category>>();
    }
}
