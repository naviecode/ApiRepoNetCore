using ShopApi.Model.Models;
using ShopApi.Service.Models;

namespace ShopApi.Service.Abstractions
{
    public interface IProductService
    {
        ResponseActionDto<Product> Add(ProductFormFile post);
        ResponseActionDto<Product> Update(ProductFormFile post);
        ResponseActionDto<Product> Delete(int id);
        ResponseDataDto<Product> GetAll();
        ResponseActionDto<Product> GetById(int id);
        //ResponseDataDto<Product> GetAllByParentId(int parentId);
        //IEnumerable<Product> GetAllPaging(int page, int pageSize, out int total);
    }
}
