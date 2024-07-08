using Microsoft.AspNetCore.Mvc;
using ShopApi.Model.Models;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Services;
using ShopApi.Web.Infrastructure.core;
using System.Net;
using Mapster.Adapters;
using Mapster;
using Azure;
using ShopApi.Service.Helpers;

namespace ShopApi.Web.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PostCategoryController : ControllerBase
    {
        private IServiceManager _serviceManager;
        public PostCategoryController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        [HttpGet]
        public async Task<IActionResult> Gets()
        {
            var reponse = _serviceManager.PostCategoryService.GetAll(); 

            return Ok(reponse);
        }
        [HttpPost]
        public async Task<IActionResult> Create(PostCategoryVM postCategoryVM)
        {
            var reponse = _serviceManager.PostCategoryService.Add(postCategoryVM.Adapt<PostCategory>());
            _serviceManager.PostCategoryService.SaveChanges();
            return Ok(reponse);
        }
    }
}
