using AutoMapper;
using ShopApi.Model.Models;
using ShopApi.Service.Models.ProductDto;

namespace ShopApi.Service.Models.ProductCategoryDto
{
    public class ProductCategoryMapperProfile : Profile
    {
        public ProductCategoryMapperProfile() 
        {
            CreateMap<ProductCategory, ProductCategoryResponse>();
            CreateMap<ProductCategoryResponse, ProductCategory>();
        }
    }
}
