using Microsoft.AspNetCore.Http;

namespace ShopApi.Service.Models.ProductDto
{
    public class ProductFormFile
    {
        public string ProductData { get; set; }
        public IFormFile? File { get; set; }
    }
}
