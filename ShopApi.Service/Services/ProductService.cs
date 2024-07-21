using AutoMapper;
using Newtonsoft.Json;
using ShopApi.Common;
using ShopApi.Data.Infrastructure;
using ShopApi.Data.Repositories;
using ShopApi.Model.Models;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Models.ProductCategoryDto;
using ShopApi.Service.Models.ProductDto;

namespace ShopApi.Service.Services
{

    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductTagRepository _productTagRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUploadFileService _uploadFileService;
        private readonly IMapper _mapper;
        private readonly IUserContextService _userContextService;


        public ProductService(IProductRepository productRepository,
            IProductTagRepository productTagRepository,
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork,
            IUploadFileService uploadFileService,
            IMapper mapper,
            IUserContextService userContextService)
        {
            _productRepository = productRepository;
            _productTagRepository = productTagRepository;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _uploadFileService = uploadFileService;
            _mapper = mapper;
            _userContextService = userContextService;

        }
        public ResponseDataDto<ProductResponse> GetAll()
        {
            var data =  _productRepository.GetAll().ToList();
            int totalItem = data.Count();
            return new ResponseDataDto<ProductResponse>(_mapper.Map<List<Product>, List<ProductResponse>>(data), totalItem);
        }
        public ResponseDataDto<ProductResponse> GetAllByFilter(ProductRequest filter)
        {
            var data = _productRepository.GetAll()
                .Where(x => (string.IsNullOrEmpty(filter.Name) || x.Name.Contains(filter.Name)) && (filter.Status == null || x.Status == filter.Status)
                && (filter.FromDate == null || x.CreatedDate > filter.FromDate) && (filter.ToDate == null || x.CreatedDate < filter.ToDate)
                ).ToList();
            int totalItem = data.Count();
            return new ResponseDataDto<ProductResponse>(_mapper.Map<List<Product>, List<ProductResponse>>(data), totalItem);
        }
        public ResponseActionDto<ProductResponse> GetById(int id)
        {
            var data = _productRepository.GetSingleById(id);
            if (data == null)
            {
                return new ResponseActionDto<ProductResponse>(null, CommonConstants.Error, "Không tìm thấy sản phẩm", "");
            }
            return new ResponseActionDto<ProductResponse>(_mapper.Map<Product, ProductResponse>(data), CommonConstants.Success, "", "");
        }

        
        public ResponseActionDto<ProductResponse> Add(ProductFormFile input)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            var product = JsonConvert.DeserializeObject<ProductResponse>(input.ProductData, settings);
            IDictionary<int, string> resUpload = new Dictionary<int, string>();

            try
            {
                if (product == null)
                {
                    return new ResponseActionDto<ProductResponse>(null, CommonConstants.Error, "Thêm mới thất bại", "ConvertJson not working");
                }
                //Nếu không có File post lên thì kh thực hiện service upload 
                if (input.File != null)
                {
                    resUpload = _uploadFileService.UploadFile(CommonConstants.ProductFolder, input.File);
                    if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                    {
                        return new ResponseActionDto<ProductResponse>(null, CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                    }
                    product.ImageKey = resUpload.FirstOrDefault().Value;
                }
                product.CreatedBy = this._userContextService.GetUserName();
                product.CreatedDate = DateTime.Now;
                var result = _productRepository.Add(_mapper.Map<ProductResponse, Product>(product));
                _unitOfWork.Commit();
                if (!string.IsNullOrEmpty(product.Tags))
                {
                    string[] tags = product.Tags.Split(',');
                    for (int i = 0; i < tags.Length; i++)
                    {
                        var tagId = tags[i];
                        if (_tagRepository.Count(x => x.ID == tagId) == 0)
                        {
                            Tag tag = new Tag();
                            tag.ID = tagId;
                            tag.Name = tags[i];
                            tag.Type = Common.CommonConstants.ProductTag;
                        }
                        ProductTag productTag = new ProductTag();
                        productTag.ProductID = result.ID;
                        productTag.TagID = tagId;
                        _productTagRepository.Add(productTag);
                    }
                    _unitOfWork.Commit();
                }
                return new ResponseActionDto<ProductResponse>(null, CommonConstants.Success, "Thêm mới thành công", result.ID.ToString());

            }
            catch (Exception ex)
            {
                return new ResponseActionDto<ProductResponse>(null, CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }

        }

        public ResponseActionDto<ProductResponse> Delete(int id)
        {
            try
            {
                var data = _productRepository.GetSingleById(id);
                if (data == null)
                {
                    return new ResponseActionDto<ProductResponse>(null, CommonConstants.Error, "Không tìm thấy sản phẩm", "");
                }
                var idDelete = _productRepository.Delete(id);
                _unitOfWork.Commit();
                return new ResponseActionDto<ProductResponse>(null, CommonConstants.Success, "Xóa thành công", idDelete.ID.ToString());
            }
            catch (Exception ex)
            {
                return new ResponseActionDto<ProductResponse>(null, CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }
            
        }
        public ResponseActionDto<ProductResponse> Update(ProductFormFile input)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            var product = JsonConvert.DeserializeObject<ProductResponse>(input.ProductData, settings);
            IDictionary<int, string> resUpload = new Dictionary<int, string>();

            try
            {
                if (product == null)
                {
                    return new ResponseActionDto<ProductResponse>(null, CommonConstants.Error, "Cập nhập thất bại", "ConvertJson not working");
                }

                if (input.File != null)
                {
                    //Nếu đã có file 
                    if (product.ImageKey != null)
                    {
                        resUpload = _uploadFileService.UpdateFile(CommonConstants.ProductFolder, input.File, product.ImageKey);
                        if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                        {
                            return new ResponseActionDto<ProductResponse>(null, CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                        }
                        //Upload lại khi có keyImage trả về
                        product.ImageKey = resUpload.FirstOrDefault().Value;
                    }
                    else //Nếu chưa có file
                    {
                        resUpload = _uploadFileService.UploadFile(CommonConstants.ProductFolder, input.File);
                        if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                        {
                            return new ResponseActionDto<ProductResponse>(null, CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                        }
                        product.ImageKey = resUpload.FirstOrDefault().Value;
                    }
                }
                product.UpdatedBy = this._userContextService.GetUserName();
                product.UpdatedDate = DateTime.Now;
                _productRepository.Update(_mapper.Map<ProductResponse, Product>(product));
                _unitOfWork.Commit();
                if (!string.IsNullOrEmpty(product.Tags))
                {
                    _productTagRepository.DeleteMulti(x=>x.ProductID == product.ID);
                    string[] tags = product.Tags.Split(',');
                    for (int i = 0; i < tags.Length; i++)
                    {
                        var tagId = tags[i];
                        if (_tagRepository.Count(x => x.ID == tagId) == 0)
                        {
                            Tag tag = new Tag();
                            tag.ID = tagId;
                            tag.Name = tags[i];
                            tag.Type = Common.CommonConstants.ProductTag;
                        }
                        ProductTag productTag = new ProductTag();
                        productTag.ProductID = product.ID;
                        productTag.TagID = tagId;
                        _productTagRepository.Add(productTag);
                    }
                    _unitOfWork.Commit();
                }
                return new ResponseActionDto<ProductResponse>(null, CommonConstants.Success, "Cập nhập thành công", product.ID.ToString());
            }
            catch (Exception ex)
            {
                return new ResponseActionDto<ProductResponse>(null, CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }
            
        }

        //public IEnumerable<Product> GetAllPaging(int page, int pageSize, out int total)
        //{
        //    return _productRepository.GetMultiPaging(x => x.Status, out total, page, pageSize);
        //}
        //public ResponseDataDto<Product> GetAllByParentId(int parentId)
        //{
        //    return _productRepository.GetMulti(x => x.Status);
        //}
    }
}
