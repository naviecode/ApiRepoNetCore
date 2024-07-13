using Microsoft.AspNetCore.Mvc;
using ShopApi.Service;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Helpers;
using ShopApi.Service.Models.ProductCategoryDto;

namespace ShopApi.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductCategoryController : ControllerBase
    {
        private IServiceManager _serviceManager;
        public ProductCategoryController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpGet("getAll")]
        public async Task<ResponseDataDto<ProductCategoryResponse>> Gets()
        {
            var response = _serviceManager.ProductCategoryService.GetAll();
            return response;
        }
        [HttpGet("getCombobox")]
        public async Task<ResponseDataDto<ProductCategoryResponse>> GetCombobox()
        {
            var response = _serviceManager.ProductCategoryService.GetProductCategoryCombobox();
            return response;
        }
        [HttpGet("getById")]
        public async Task<ResponseActionDto<ProductCategoryResponse>> GetById(int id)
        {
            var response = _serviceManager.ProductCategoryService.GetById(id);
            return response;
        }
        [HttpPost("create")]
        public async Task<ResponseActionDto<ProductCategoryResponse>> Create([FromForm]ProductCategoryFromFile data)
        {
            var response = _serviceManager.ProductCategoryService.Add(data);
            return response;
        }
        [HttpPut("update")]
        public async Task<ResponseActionDto<ProductCategoryResponse>> Update([FromForm] ProductCategoryFromFile data)
        {
            var response = _serviceManager.ProductCategoryService.Update(data);
            return response;
        }
        [HttpDelete("delete")]
        public async Task<ResponseActionDto<ProductCategoryResponse>> Delete(int id)
        {
            var response = _serviceManager.ProductCategoryService.Delete(id);
            return response;
        }
    }
}
