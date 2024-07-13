using AutoMapper;
using Newtonsoft.Json;
using ShopApi.Common;
using ShopApi.Data.Infrastructure;
using ShopApi.Data.Repositories;
using ShopApi.Model.Models;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Models.ProductCategoryDto;

namespace ShopApi.Service.Services
{

    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUploadFileService _uploadFileService;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;
        public ProductCategoryService(IProductCategoryRepository productCategoryRepository,
            IUnitOfWork unitOfWork,
            IUploadFileService uploadFileService,
            IMapper mapper,
            IUserContextService userContextService)
        {
            _productCategoryRepository = productCategoryRepository;
            _unitOfWork = unitOfWork;
            _uploadFileService = uploadFileService;
            _mapper = mapper;
            _userContextService = userContextService;
        }
        
        public ResponseDataDto<ProductCategoryResponse> GetAll()
        {
            var data = _productCategoryRepository.GetAll().ToList();
            int totalItem = data.Count();
            return new ResponseDataDto<ProductCategoryResponse>(_mapper.Map<List<ProductCategory>, List<ProductCategoryResponse>>(data), totalItem);
        }

        public ResponseActionDto<ProductCategoryResponse> GetById(int id)
        {
            var data = _productCategoryRepository.GetSingleById(id);
            if (data == null)
            {
                return new ResponseActionDto<ProductCategoryResponse>(null, CommonConstants.Error, "Không tìm thấy sản phẩm", "");
            }
            return new ResponseActionDto<ProductCategoryResponse>(_mapper.Map<ProductCategory, ProductCategoryResponse>(data), CommonConstants.Success, "", "");
        }
        public ResponseActionDto<ProductCategoryResponse> Add(ProductCategoryFromFile input)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore , MissingMemberHandling = MissingMemberHandling.Ignore};
            var dataConvert = JsonConvert.DeserializeObject<ProductCategory>(input.ProductCategoryData, settings);
            IDictionary<int, string> resUpload = new Dictionary<int,string>();
            try
            {
                if(dataConvert == null)
                {
                    return new ResponseActionDto<ProductCategoryResponse>(null, CommonConstants.Error, "Thêm mới thất bại", "ConvertJson not working");
                }
                //Nếu không có File post lên thì kh thực hiện service upload 
                if(input.File != null)
                {
                    resUpload = _uploadFileService.UploadFile(CommonConstants.ProductCategoryFolder, input.File);
                    if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                    {
                        return new ResponseActionDto<ProductCategoryResponse>(null, CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                    }
                    dataConvert.ImageKey = resUpload.FirstOrDefault().Value;
                }

                dataConvert.CreatedBy = this._userContextService.GetUserName();
                dataConvert.CreatedDate = DateTime.Now;
                var resultAdd = _productCategoryRepository.Add(dataConvert);
                _unitOfWork.Commit();
                return new ResponseActionDto<ProductCategoryResponse>(null, CommonConstants.Success, "Thêm mới thành công", resultAdd.ID.ToString());

            }
            catch (Exception ex)
            {
                return new ResponseActionDto<ProductCategoryResponse>(null, CommonConstants.Error, "Lỗi hệ thống", ex.ToString());

            }
        }
        public ResponseActionDto<ProductCategoryResponse> Update(ProductCategoryFromFile input)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            var dataConvert = JsonConvert.DeserializeObject<ProductCategory>(input.ProductCategoryData, settings);
            IDictionary<int, string> resUpload = new Dictionary<int, string>();
            try
            {
                if (dataConvert == null)
                {
                    return new ResponseActionDto<ProductCategoryResponse>(null, CommonConstants.Error, "Cập nhập thất bại", "ConvertJson not working");
                }
                if(input.File != null)
                {
                    //Nếu đã có file 
                    if(dataConvert.ImageKey != null)
                    {
                        resUpload = _uploadFileService.UpdateFile(CommonConstants.ProductCategoryFolder, input.File, dataConvert.ImageKey);
                        if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                        {
                            return new ResponseActionDto<ProductCategoryResponse>(null, CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                        }
                        //Upload lại khi có keyImage trả về
                        dataConvert.ImageKey = resUpload.FirstOrDefault().Value;
                    }
                    else //Nếu chưa có file
                    {
                        resUpload = _uploadFileService.UploadFile(CommonConstants.ProductCategoryFolder, input.File);
                        if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                        {
                            return new ResponseActionDto<ProductCategoryResponse>(null, CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                        }
                        dataConvert.ImageKey = resUpload.FirstOrDefault().Value;
                    }
                }
                dataConvert.UpdatedBy = this._userContextService.GetUserName();
                dataConvert.UpdatedDate = DateTime.Now;
                _productCategoryRepository.Update(dataConvert);
                _unitOfWork.Commit();
                return new ResponseActionDto<ProductCategoryResponse>(null, Common.CommonConstants.Success, "Cập nhập thành công", dataConvert.ID.ToString());

            }
            catch (Exception ex)
            {
                return new ResponseActionDto<ProductCategoryResponse>(null, CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }

        }
        public ResponseActionDto<ProductCategoryResponse> Delete(int id)
        {
            try
            {
                var productCategoryId = _productCategoryRepository.GetSingleById(id);
                if (productCategoryId == null)
                {
                    return new ResponseActionDto<ProductCategoryResponse>(null, Common.CommonConstants.Error, "Không tìm thấy sản phẩm", "");
                }
                var data = _productCategoryRepository.Delete(id);
                _unitOfWork.Commit();
                return new ResponseActionDto<ProductCategoryResponse>(null, Common.CommonConstants.Success, "Xóa thành công", data.ID.ToString());
            }
            catch(Exception ex)
            {
                return new ResponseActionDto<ProductCategoryResponse>(null, CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }
           
        }

        public ResponseDataDto<ProductCategoryResponse> GetProductCategoryCombobox()
        {
            var data = _productCategoryRepository.GetAll().Where(x=>x.Status == true).ToList();
            int totalItem = data.Count();
            return new ResponseDataDto<ProductCategoryResponse>(_mapper.Map<List<ProductCategory>, List<ProductCategoryResponse>>(data), totalItem);
        }
    }
}
