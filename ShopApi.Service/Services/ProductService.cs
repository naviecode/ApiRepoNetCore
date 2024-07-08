using Newtonsoft.Json;
using ShopApi.Common;
using ShopApi.Data.Infrastructure;
using ShopApi.Data.Repositories;
using ShopApi.Model.Models;
using ShopApi.Service.Abstractions;
using ShopApi.Service.Models;

namespace ShopApi.Service.Services
{

    public class ProductService : IProductService
    {
        IProductRepository _productRepository;
        IProductTagRepository _productTagRepository;
        ITagRepository _tagRepository;
        IUnitOfWork _unitOfWork;
        IUploadFileService _uploadFileService;

        public ProductService(IProductRepository productRepository, IProductTagRepository productTagRepository, ITagRepository tagRepository, IUnitOfWork unitOfWork, IUploadFileService uploadFileService)
        {
            _productRepository = productRepository;
            _productTagRepository = productTagRepository;
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _uploadFileService = uploadFileService;

        }
        public ResponseDataDto<Product> GetAll()
        {
            var data =  _productRepository.GetAll().ToList();
            int totalItem = data.Count();
            return new ResponseDataDto<Product>(data, totalItem);
        }
        public ResponseActionDto<Product> GetById(int id)
        {
            var data = _productRepository.GetSingleById(id);
            if (data == null)
            {
                return new ResponseActionDto<Product>(new Product(), CommonConstants.Error, "Không tìm thấy sản phẩm", "");
            }
            return new ResponseActionDto<Product>(data, CommonConstants.Success, "", "");
        }

        
        public ResponseActionDto<Product> Add(ProductFormFile input)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            var product = JsonConvert.DeserializeObject<Product>(input.ProductData, settings);
            IDictionary<int, string> resUpload = new Dictionary<int, string>();

            try
            {
                if (product == null)
                {
                    return new ResponseActionDto<Product>(new Product(), CommonConstants.Error, "Thêm mới thất bại", "ConvertJson not working");
                }
                //Nếu không có File post lên thì kh thực hiện service upload 
                if (input.File != null)
                {
                    resUpload = _uploadFileService.UploadFile("", input.File);
                    if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                    {
                        return new ResponseActionDto<Product>(new Product(), CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                    }
                    product.ImageKey = resUpload.FirstOrDefault().Value;
                }
                var result = _productRepository.Add(product);
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
                return new ResponseActionDto<Product>(new Product(), CommonConstants.Success, "Thêm mới thành công", result.ID.ToString());

            }
            catch (Exception ex)
            {
                return new ResponseActionDto<Product>(new Product(), CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }

        }

        public ResponseActionDto<Product> Delete(int id)
        {
            try
            {
                var data = _productRepository.GetSingleById(id);
                if (data == null)
                {
                    return new ResponseActionDto<Product>(new Product(), CommonConstants.Error, "Không tìm thấy sản phẩm", "");
                }
                var idDelete = _productRepository.Delete(id);
                _unitOfWork.Commit();
                return new ResponseActionDto<Product>(new Product(), CommonConstants.Success, "Xóa thành công", idDelete.ID.ToString());
            }
            catch (Exception ex)
            {
                return new ResponseActionDto<Product>(new Product(), CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }
            
        }
        public ResponseActionDto<Product> Update(ProductFormFile input)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            var product = JsonConvert.DeserializeObject<Product>(input.ProductData, settings);
            IDictionary<int, string> resUpload = new Dictionary<int, string>();

            try
            {
                if (product == null)
                {
                    return new ResponseActionDto<Product>(new Product(), CommonConstants.Error, "Cập nhập thất bại", "ConvertJson not working");
                }

                if (input.File != null)
                {
                    //Nếu đã có file 
                    if (product.ImageKey != null)
                    {
                        resUpload = _uploadFileService.UpdateFile("", input.File, product.ImageKey);
                        if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                        {
                            return new ResponseActionDto<Product>(new Product(), CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                        }
                        //Upload lại khi có keyImage trả về
                        product.ImageKey = resUpload.FirstOrDefault().Value;
                    }
                    else //Nếu chưa có file
                    {
                        resUpload = _uploadFileService.UploadFile("", input.File);
                        if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                        {
                            return new ResponseActionDto<Product>(new Product(), CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                        }
                        product.ImageKey = resUpload.FirstOrDefault().Value;
                    }
                }

                _productRepository.Update(product);
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
                        productTag.ProductID = product.ID;
                        productTag.TagID = tagId;
                        _productTagRepository.Add(productTag);
                    }
                    _unitOfWork.Commit();
                }
                return new ResponseActionDto<Product>(new Product(), CommonConstants.Success, "Cập nhập thành công", product.ID.ToString());
            }
            catch (Exception ex)
            {
                return new ResponseActionDto<Product>(new Product(), CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
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
