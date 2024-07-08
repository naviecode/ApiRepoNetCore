using Mapster;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Service.Models;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Helpers;
using WebApiCore.Models;
using ShopApi.Common;
using static Azure.Core.HttpHeader;
using ShopApi.Model.Models;

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
        public async Task<ResponseDataDto<Product>> Gets()
        {
            var response = _serviceManager.ProductService.GetAll();
            return response;
        }
        [HttpGet("getById")]
        public async Task<ResponseActionDto<Product>> GetById(int id)
        {
            var response = _serviceManager.ProductService.GetById(id);
            return response;
        }

        [HttpPost("create")]
        public async Task<ResponseActionDto<Product>> Create([FromForm] ProductFormFile input)
        {
            var response = _serviceManager.ProductService.Add(input);
            return response;
        }
        [HttpPut("update")]
        public async Task<ResponseActionDto<Product>> Update([FromForm] ProductFormFile input)
        {
            var response = _serviceManager.ProductService.Update(input);
            return response;
        }
        [HttpDelete("delete")]
        public async Task<ResponseActionDto<Product>> Delete(int id)
        {
            var response = _serviceManager.ProductService.Delete(id);
            return response;
        }
        //[HttpGet("{page:int},{pageSize:int}")]
        //public async Task<IActionResult> GetAllPaging(int page, int pageSize)
        //{
        //    int total;
        //    var reponse = _serviceManager.ProductService.GetAllPaging(page, pageSize,out total);
        //    return Ok(reponse);
        //}
    }
}
