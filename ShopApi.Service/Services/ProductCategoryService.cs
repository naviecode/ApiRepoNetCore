using Newtonsoft.Json;
using ShopApi.Common;
using ShopApi.Data.Infrastructure;
using ShopApi.Data.Repositories;
using ShopApi.Model.Models;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Models;

namespace ShopApi.Service.Services
{

    public class ProductCategoryService : IProductCategoryService
    {
        IProductCategoryRepository _productCategoryRepository;
        IUnitOfWork _unitOfWork;
        IUploadFileService _uploadFileService;
        public ProductCategoryService(IProductCategoryRepository productCategoryRepository, IUnitOfWork unitOfWork, IUploadFileService uploadFileService)
        {
            _productCategoryRepository = productCategoryRepository;
            _unitOfWork = unitOfWork;
            _uploadFileService = uploadFileService;
        }
        
        public ResponseDataDto<ProductCategory> GetAll()
        {
            var data = _productCategoryRepository.GetAll().ToList();
            int totalItem = data.Count();
            return new ResponseDataDto<ProductCategory>(data, totalItem);
        }

        public ResponseActionDto<ProductCategory> GetById(int id)
        {
            var data = _productCategoryRepository.GetSingleById(id);
            if (data == null)
            {
                return new ResponseActionDto<ProductCategory>(new ProductCategory(), CommonConstants.Error, "Không tìm thấy sản phẩm", "");
            }
            return new ResponseActionDto<ProductCategory>(data, CommonConstants.Success, "", "");
        }
        public ResponseActionDto<ProductCategory> Add(ProductCategoryFromFile input)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore , MissingMemberHandling = MissingMemberHandling.Ignore};
            var dataConvert = JsonConvert.DeserializeObject<ProductCategory>(input.ProductCategoryData, settings);
            IDictionary<int, string> resUpload = new Dictionary<int,string>();
            try
            {
                if(dataConvert == null)
                {
                    return new ResponseActionDto<ProductCategory>(new ProductCategory(), CommonConstants.Error, "Thêm mới thất bại", "ConvertJson not working");
                }
                //Nếu không có File post lên thì kh thực hiện service upload 
                if(input.File != null)
                {
                    resUpload = _uploadFileService.UploadFile("", input.File);
                    if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                    {
                        return new ResponseActionDto<ProductCategory>(new ProductCategory(), CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                    }
                    dataConvert.ImageKey = resUpload.FirstOrDefault().Value;
                }

                var resultAdd = _productCategoryRepository.Add(dataConvert);
                _unitOfWork.Commit();
                return new ResponseActionDto<ProductCategory>(new ProductCategory(), CommonConstants.Success, "Thêm mới thành công", resultAdd.ID.ToString());

            }
            catch (Exception ex)
            {
                return new ResponseActionDto<ProductCategory>(new ProductCategory(), CommonConstants.Error, "Lỗi hệ thống", ex.ToString());

            }
        }
        public ResponseActionDto<ProductCategory> Update(ProductCategoryFromFile input)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            var dataConvert = JsonConvert.DeserializeObject<ProductCategory>(input.ProductCategoryData, settings);
            IDictionary<int, string> resUpload = new Dictionary<int, string>();
            try
            {
                if (dataConvert == null)
                {
                    return new ResponseActionDto<ProductCategory>(new ProductCategory(), CommonConstants.Error, "Cập nhập thất bại", "ConvertJson not working");
                }
                if(input.File != null)
                {
                    //Nếu đã có file 
                    if(dataConvert.ImageKey != null)
                    {
                        resUpload = _uploadFileService.UpdateFile("", input.File, dataConvert.ImageKey);
                        if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                        {
                            return new ResponseActionDto<ProductCategory>(new ProductCategory(), CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                        }
                        //Upload lại khi có keyImage trả về
                        dataConvert.ImageKey = resUpload.FirstOrDefault().Value;
                    }
                    else //Nếu chưa có file
                    {
                        resUpload = _uploadFileService.UploadFile("", input.File);
                        if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                        {
                            return new ResponseActionDto<ProductCategory>(new ProductCategory(), CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                        }
                        dataConvert.ImageKey = resUpload.FirstOrDefault().Value;
                    }
                }    
                _productCategoryRepository.Update(dataConvert);
                _unitOfWork.Commit();
                return new ResponseActionDto<ProductCategory>(new ProductCategory(), Common.CommonConstants.Success, "Cập nhập thành công", dataConvert.ID.ToString());

            }
            catch (Exception ex)
            {
                return new ResponseActionDto<ProductCategory>(new ProductCategory(), CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }

        }
        public ResponseActionDto<ProductCategory> Delete(int id)
        {
            try
            {
                var productCategoryId = _productCategoryRepository.GetSingleById(id);
                if (productCategoryId == null)
                {
                    return new ResponseActionDto<ProductCategory>(new ProductCategory(), Common.CommonConstants.Error, "Không tìm thấy sản phẩm", "");
                }
                ProductCategory data = _productCategoryRepository.Delete(id);
                _unitOfWork.Commit();
                return new ResponseActionDto<ProductCategory>(new ProductCategory(), Common.CommonConstants.Success, "Xóa thành công", data.ID.ToString());
            }
            catch(Exception ex)
            {
                return new ResponseActionDto<ProductCategory>(new ProductCategory(), CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }
           
        }
        //public ResponseDataDto<ProductCategory> GetAllByParentId(int parentId)
        //{
        //    _productCategoryRepository.GetMulti(x => x.Status && x.ParentID == parentId);
        //    return null;
        //}
    }
}
