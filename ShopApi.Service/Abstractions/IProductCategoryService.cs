using ShopApi.Model.Models;
using ShopApi.Service.Models;

namespace ShopApi.Service.Abstractions
{
    public interface IProductCategoryService
    {
        ResponseActionDto<ProductCategory> Add(ProductCategoryFromFile data);
        ResponseActionDto<ProductCategory> Update(ProductCategoryFromFile data);
        ResponseActionDto<ProductCategory> Delete(int id);
        ResponseDataDto<ProductCategory> GetAll();
        ResponseActionDto<ProductCategory> GetById(int id);
        //ResponseDataDto<ProductCategory> GetAllByParentId(int parentId);
    }
}
