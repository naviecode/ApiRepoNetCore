using Azure;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Service.Models;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Helpers;
using WebApiCore.Models;
using ShopApi.Model.Models;

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
        public async Task<ResponseDataDto<ProductCategory>> Gets()
        {
            var response = _serviceManager.ProductCategoryService.GetAll();
            return response;
        }
        [HttpGet("getById")]
        public async Task<ResponseActionDto<ProductCategory>> GetById(int id)
        {
            var response = _serviceManager.ProductCategoryService.GetById(id);
            return response;
        }
        [HttpPost("create")]
        public async Task<ResponseActionDto<ProductCategory>> Create([FromForm]ProductCategoryFromFile data)
        {
            var response = _serviceManager.ProductCategoryService.Add(data);
            return response;
        }
        [HttpPut("update")]
        public async Task<ResponseActionDto<ProductCategory>> Update([FromForm] ProductCategoryFromFile data)
        {
            var response = _serviceManager.ProductCategoryService.Update(data);
            return response;
        }
        [HttpDelete("delete")]
        public async Task<ResponseActionDto<ProductCategory>> Delete(int id)
        {
            var response = _serviceManager.ProductCategoryService.Delete(id);
            return response;
        }
    }
}
