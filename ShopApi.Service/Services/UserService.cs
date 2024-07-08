using Newtonsoft.Json;
using ShopApi.Common;
using ShopApi.Data.Infrastructure;
using ShopApi.Data.Repositories;
using ShopApi.Service.Models;
using ShopApi.Service.Abstractions;
using ShopApi.Model.Models;

namespace ShopApi.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUploadFileService _uploadFileService;
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork, IUploadFileService uploadFileService)
        {
            _userRepository = userRepository;
            _uploadFileService = uploadFileService;
            _unitOfWork = unitOfWork;
        }
        public ResponseDataDto<User> GetAll()
        {
            var data = _userRepository.GetAll().ToList();
            int totalItem = data.Count();
            return new ResponseDataDto<User>(data, totalItem);
        }

        public ResponseActionDto<User> GetById(int id)
        {
            var data = _userRepository.GetSingleById(id);
            if (data == null)
            {
                return new ResponseActionDto<User>(new User(), CommonConstants.Error, "Không tìm thấy sản phẩm", "");
            }
            return new ResponseActionDto<User>(data, CommonConstants.Success, "", "");
        }
        public ResponseActionDto<User> Add(UserFormFile input)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            var dataConvert = JsonConvert.DeserializeObject<User>(input.UserData, settings);
            IDictionary<int, string> resUpload = new Dictionary<int, string>();
            try
            {
                if (dataConvert == null)
                {
                    return new ResponseActionDto<User>(new User(), CommonConstants.Error, "Thêm mới thất bại", "ConvertJson not working");
                }
                //Nếu không có File post lên thì kh thực hiện service upload 
                if (input.File != null)
                {
                    resUpload = _uploadFileService.UploadFile("", input.File);
                    if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                    {
                        return new ResponseActionDto<User>(new User(), CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                    }
                    dataConvert.ImageKey = resUpload.FirstOrDefault().Value;
                }
                var resultAdd = _userRepository.Add(dataConvert);
                _unitOfWork.Commit();
                return new ResponseActionDto<User>(new User(), CommonConstants.Success, "Thêm mới thành công", resultAdd.ID.ToString());

            }
            catch (Exception ex)
            {
                return new ResponseActionDto<User>(new User(), CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }
        }

        public ResponseActionDto<User> Update(UserFormFile input)
        {
            var settings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, MissingMemberHandling = MissingMemberHandling.Ignore };
            var dataConvert = JsonConvert.DeserializeObject<User>(input.UserData, settings);
            IDictionary<int, string> resUpload = new Dictionary<int, string>();
            try
            {
                if (dataConvert == null)
                {
                    return new ResponseActionDto<User>(new User(), CommonConstants.Error, "Cập nhập thất bại", "ConvertJson not working");
                }
                if (input.File != null)
                {
                    //Nếu đã có file 
                    if (dataConvert.ImageKey != null)
                    {
                        resUpload = _uploadFileService.UpdateFile("", input.File, dataConvert.ImageKey);
                        if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                        {
                            return new ResponseActionDto<User>(new User(), CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                        }
                        //Upload lại khi có keyImage trả về
                        dataConvert.ImageKey = resUpload.FirstOrDefault().Value;
                    }
                    else //Nếu chưa có file
                    {
                        resUpload = _uploadFileService.UploadFile("", input.File);
                        if (resUpload.FirstOrDefault().Key != CommonConstants.Success)
                        {
                            return new ResponseActionDto<User>(new User(), CommonConstants.Error, "Thêm mới thất bại", resUpload.FirstOrDefault().Value);
                        }
                        dataConvert.ImageKey = resUpload.FirstOrDefault().Value;
                    }
                }
                _userRepository.Update(dataConvert);
                _unitOfWork.Commit();
                return new ResponseActionDto<User>(new User(), CommonConstants.Success, "Cập nhập thành công", dataConvert.ID.ToString());

            }
            catch (Exception ex)
            {
                return new ResponseActionDto<User>(new User(), CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }
        }

        public ResponseActionDto<User> Delete(int id)
        {
            try
            {
                var productCategoryId = _userRepository.GetSingleById(id);
                if (productCategoryId == null)
                {
                    return new ResponseActionDto<User>(new User(), CommonConstants.Error, "Không tìm thấy user", "");
                }
                User data = _userRepository.Delete(id);
                _unitOfWork.Commit();
                return new ResponseActionDto<User>(new User(), CommonConstants.Success, "Xóa thành công", data.ID.ToString());
            }
            catch (Exception ex)
            {
                return new ResponseActionDto<User>(new User(), CommonConstants.Error, "Lỗi hệ thống", ex.ToString());
            }
        }

       

       
    }
}
