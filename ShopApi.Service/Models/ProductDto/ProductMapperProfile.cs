using AutoMapper;
using ShopApi.Model.Models;

namespace ShopApi.Service.Models.ProductDto
{
    public class ProductMapperProfile : Profile
    {
        public ProductMapperProfile()
        {
            CreateMap<Product, ProductResponse>();
            CreateMap<ProductResponse, Product>();
        }
    }
}
