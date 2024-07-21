using ShopApi.Model.Models;
using ShopApi.Service.Models.ProductCategoryDto;

namespace ShopApi.Service.Abstractions
{
    public interface IProductCategoryService
    {
        ResponseActionDto<ProductCategoryResponse> Add(ProductCategoryFromFile data);
        ResponseActionDto<ProductCategoryResponse> Update(ProductCategoryFromFile data);
        ResponseActionDto<ProductCategoryResponse> Delete(int id);
        ResponseDataDto<ProductCategoryResponse> GetAll();
        ResponseDataDto<ProductCategoryResponse> GetAllByFilter(ProductCategoryRequest filter);
        ResponseDataDto<ProductCategoryResponse> GetProductCategoryCombobox();
        ResponseActionDto<ProductCategoryResponse> GetById(int id);

    }
}
