using Microsoft.AspNetCore.Mvc;
using ShopApi.Model.Models;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Helpers;
using ShopApi.Service.Models.ProductDto;
using ShopApi.Service;

namespace WebApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private IServiceManager _serviceManager;
        public ProductController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpGet("getAll")]
        public async Task<ResponseDataDto<ProductResponse>> Gets()
        {
            var response = _serviceManager.ProductService.GetAll();
            return response;
        }
        [HttpGet("getById")]
        public async Task<ResponseActionDto<ProductResponse>> GetById(int id)
        {
            var response = _serviceManager.ProductService.GetById(id);
            return response;
        }

        [HttpPost("create")]
        public async Task<ResponseActionDto<ProductResponse>> Create([FromForm] ProductFormFile input)
        {
            var response = _serviceManager.ProductService.Add(input);
            return response;
        }
        [HttpPut("update")]
        public async Task<ResponseActionDto<ProductResponse>> Update([FromForm] ProductFormFile input)
        {
            var response = _serviceManager.ProductService.Update(input);
            return response;
        }
        [HttpDelete("delete")]
        public async Task<ResponseActionDto<ProductResponse>> Delete(int id)
        {
            var response = _serviceManager.ProductService.Delete(id);
            return response;
        }
    }
}
