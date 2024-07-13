using Microsoft.AspNetCore.Http;

namespace ShopApi.Service.Models.ProductCategoryDto
{
    public class ProductCategoryFromFile
    {
        public string ProductCategoryData { get; set; }
        public IFormFile? File { get; set; }
    }
}
