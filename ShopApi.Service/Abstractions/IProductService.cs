using ShopApi.Model.Models;
using ShopApi.Service.Models.ProductDto;

namespace ShopApi.Service.Abstractions
{
    public interface IProductService
    {
        ResponseActionDto<ProductResponse> Add(ProductFormFile post);
        ResponseActionDto<ProductResponse> Update(ProductFormFile post);
        ResponseActionDto<ProductResponse> Delete(int id);
        ResponseDataDto<ProductResponse> GetAll();
        ResponseActionDto<ProductResponse> GetById(int id);
        //ResponseDataDto<Product> GetAllByParentId(int parentId);
        //IEnumerable<Product> GetAllPaging(int page, int pageSize, out int total);
    }
}
